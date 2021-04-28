using System;
using System.IO;
using System.Linq;
using System.Media;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace ASCII_Converter
{
    class Program
    {
        private const double WIDTH_OFFSET = 2.0;
        private const int MAX_WIDTH = 400;
        private static string answer;

        #region maximize console
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int cmdShow);
        private static void Maximize()
        {
            Process proc = Process.GetCurrentProcess();
            ShowWindow(proc.MainWindowHandle, 3);
        }
        #endregion

        [STAThread]
        static void Main(string[] args)
        {
            Maximize();
            MainMenu();
        }
        private static void MainMenu()
        {
            do
            {
                Console.Write("Play music? y/n: ");
                answer = Console.ReadLine();

                switch (answer)
                {
                    case "y":
                    case "Y":
                        Console.Clear(); PlayMusic(); ProcessingBitmap();
                        break;
                    case "n":
                    case "N":
                        Console.Clear(); ProcessingBitmap();
                        break;
                }
            } while (answer != "y" || answer != "Y" || answer != "n" || answer != "N");
        }
        private static void PlayMusic()
        {
            SoundPlayer player = new SoundPlayer();
            player.SoundLocation = "../../background_music.wav";
            player.Play();

        }
        private static void ProcessingBitmap()
        {
            Console.WriteLine("Press Enter to start");
            Console.WriteLine("Press S to save into file");

            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Filter = "Images | *.bmp; *png; *jpg; *jpeg"
            };

            while (true)
            {
                    Console.ReadKey();
                
                    if (fileDialog.ShowDialog() != DialogResult.OK)
                        continue;

                    Console.Clear();

                    var bitmap = new Bitmap(fileDialog.FileName);
                    bitmap = ResizeBitmap(bitmap);
                    bitmap.ToGrayColor();

                    var converter = new BitmapToASCII(bitmap);
                    var rows = converter.Convert();

                    foreach (var row in rows)
                    {
                        Console.WriteLine(row);
                    }

                    if (Console.ReadKey().Key == ConsoleKey.S)
                    {
                        File.WriteAllLines("../../image.txt", rows.Select(row => new string(row)));
                    }

                    Console.SetCursorPosition(0, 0);         
            }
        }
        private static Bitmap ResizeBitmap(Bitmap bitmap)
        {
            var newHeight = bitmap.Height / WIDTH_OFFSET * MAX_WIDTH / bitmap.Height;

            if (bitmap.Width > MAX_WIDTH || bitmap.Height > newHeight)
                bitmap = new Bitmap(bitmap, new Size(MAX_WIDTH, (int)newHeight));

            return bitmap;
        }
    }
}