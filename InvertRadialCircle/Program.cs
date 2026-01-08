using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace InvertRadialCircle
{
    public static class Program
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool ReleaseDC(IntPtr hWnd, IntPtr hdc);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateEllipticRgn(int left, int top, int right, int bottom);

        [DllImport("gdi32.dll")]
        public static extern bool InvertRgn(IntPtr hdc, IntPtr hrgn);

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject(IntPtr hObject);

        public static void GdiPayloadRadial()
        {
            IntPtr hdcScreen = GetDC(IntPtr.Zero);
            int x = Screen.PrimaryScreen.Bounds.Width;
            int y = Screen.PrimaryScreen.Bounds.Height;
            float centerX = x / 2f;
            float centerY = y / 2f;
            float orbitRadius = Math.Min(x, y) * 0.3f;
            float circleRadius = 120f;
            float angle = 0f;

            while (true)
            {
                hdcScreen = GetDC(IntPtr.Zero);
                float x2 = centerX + (float)Math.Cos(angle) * orbitRadius;
                float y2 = centerY + (float)Math.Sin(angle) * orbitRadius;

                IntPtr rgn = CreateEllipticRgn((int)(x2 - circleRadius), (int)(y2 - circleRadius), (int)(x2 + circleRadius), (int)(y2 + circleRadius));
                InvertRgn(hdcScreen, rgn);
                ReleaseDC(IntPtr.Zero, hdcScreen);
                DeleteDC(hdcScreen);
                DeleteObject(rgn);
                angle += 0.01f;

                Thread.Sleep(10);
            }
        }

        [STAThread]
        public static void Main(string[] args)
        {
            if (MessageBox.Show("InvertRadialCircle by ListopadTech\nPress OK to proceed with execution", "ANTI SKID", MessageBoxButtons.OK, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                new Thread(GdiPayloadRadial).Start();
            }
        }
    }
}
