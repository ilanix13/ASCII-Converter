using System;
using System.IO;
using System.Linq;
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
            Console.WriteLine("Press any key to start...");

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

                Bitmap bitmap = new Bitmap(fileDialog.FileName);
                bitmap = ResizeBitmap(bitmap);
                bitmap.ToGrayColor();
                BitmapToASCII converter = new BitmapToASCII(bitmap);
                char[][] rows = converter.Convert();

                foreach (char [] row in rows)           
                    Console.WriteLine(row);
                
                File.WriteAllLines("../../ASCII_Image.txt", rows.Select(row => new string(row)));
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