using System;
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
        private static Bitmap ResizeBitmap(Bitmap bitmap)
        {
            var newHeight = bitmap.Height / WIDTH_OFFSET * MAX_WIDTH / bitmap.Height;

            if (bitmap.Width > MAX_WIDTH || bitmap.Height > newHeight)
                bitmap = new Bitmap(bitmap, new Size(MAX_WIDTH, (int)newHeight));

            return bitmap;
        }
        private static void MainMenu()
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Filter = "Images | *.bmp; *png; *jpg; *jpeg"
            };

            Console.WriteLine("Press ENTER to start");

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

                Console.SetCursorPosition(0, 0);
            }
        }
    }
}