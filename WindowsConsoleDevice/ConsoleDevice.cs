using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace WPRenderer
{
    class ConsoleDevice : IDevice
    {
        private struct RECT { public int left, top, right, bottom; }
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT rc);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool MoveWindow(IntPtr hWnd, int x, int y, int w, int h, bool repaint);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetSystemMetrics(int nIndex);

        private int screenWidth;
        private int screenHeight;

        private int deviceWidth;
        private int deviceHeight;

        private int consoleWidth;
        private int consoleHeight;

        private ConsoleColor backgroundColor = ConsoleColor.Black;

        private ConsoleColor[,] consoleBuffers;

        public void Initialize(int width, int height)
        {
            screenWidth = GetSystemMetrics(0);
            screenHeight = GetSystemMetrics(1);

            deviceWidth = width;
            deviceHeight = height;

            consoleWidth = Console.LargestWindowWidth * width / screenWidth;
            consoleHeight = Console.LargestWindowHeight * height / screenHeight;

            Console.SetWindowSize(consoleWidth, consoleHeight);
            Console.SetBufferSize(consoleWidth, consoleHeight);
            Console.CursorVisible = false;

            IntPtr hWnd = GetConsoleWindow();
            RECT rc;
            GetWindowRect(hWnd, out rc);
            int wndWidth = rc.right - rc.left;
            int wndHeight = rc.bottom - rc.top;
            int x = (screenWidth - wndWidth) / 2;
            int y = (screenHeight - wndHeight) / 2;
            MoveWindow(hWnd, x, y, wndWidth, wndHeight, true);

            consoleBuffers = new ConsoleColor[consoleWidth, consoleHeight];
        }

        public void Clear(Color blackground)
        {
            backgroundColor = ConvertToConsoleColor(blackground);
            Console.BackgroundColor = backgroundColor;
            Console.Clear();

            for (int i = 0; i < consoleWidth; i++)
            {
                for (int j = 0; j < consoleHeight; j++)
                {
                    consoleBuffers[i, j] = backgroundColor;
                }
            }
        }

        public void DrawText(string text, int x, int y, Color c)
        {
            int cx, cy;
            ConvertToConsoleXY(x, y, out cx, out cy);
            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = ConvertToConsoleColor(c);
            Console.SetCursorPosition(cx, cy);
            Console.Write(text);
        }

        public void DrawPixel(int x, int y, Color c)
        {
            int cx, cy;
            ConvertToConsoleXY(x, y, out cx, out cy);
            ConsoleColor cc = ConvertToConsoleColor(c);
            if (!ConsoleColorTest(cx, cy, cc, true))
                return;
            Console.BackgroundColor = cc;
            if (cx % 2 != 0)
                cx--;
            Console.SetCursorPosition(cx, cy);
            Console.Write(' ');
            Console.SetCursorPosition(cx + 1, cy);
            Console.Write(' ');
        }

        public void DrawLine(Vertex v1, Vertex v2)
        {
            ConvertToNDC(v1.pos.x, v1.pos.y, out v1.pos.x, out v1.pos.y);
            ConvertToNDC(v2.pos.x, v2.pos.y, out v2.pos.x, out v2.pos.y);
            NormalizedDevice.DrawLine(this, v1, v2);
        }

        public void DrawTriangle(Vertex v1, Vertex v2, Vertex v3)
        {
            ConvertToNDC(v1.pos.x, v1.pos.y, out v1.pos.x, out v1.pos.y);
            ConvertToNDC(v2.pos.x, v2.pos.y, out v2.pos.x, out v2.pos.y);
            ConvertToNDC(v3.pos.x, v3.pos.y, out v3.pos.x, out v3.pos.y);
            NormalizedDevice.DrawTriangle(this, v1, v2, v3);
        }

        public void ConvertFromNDC(float normalizedX, float normalizedY, out float deviceX, out float deviceY)
        {
            // NDC to console device coordinates(left up[0, 0])
            deviceX = (normalizedX + 1f) * 0.5f * deviceWidth;
            deviceY = (1f - normalizedY) * 0.5f * deviceHeight;
        }

        public void ConvertToNDC(float deviceX, float deviceY, out float normalizedX, out float normalizedY)
        {
            // Console device coordinates to NDC
            normalizedX = deviceX * 2f / deviceWidth - 1f;
            normalizedY = 1f - deviceY * 2f / deviceHeight;
        }

        public void WaitForPresent()
        {
            System.Threading.Thread.Sleep(1000);
        }

        //===========================================================

        private void ConvertToConsoleXY(int x, int y, out int cx, out int cy)
        {
            cx = x * consoleWidth / deviceWidth;
            cy = y * consoleHeight / deviceHeight;
            if (cx >= consoleWidth) cx = consoleWidth - 1;
            else if (cx < 0) cx = 0;
            if (cy >= consoleHeight) cy = consoleHeight - 1;
            else if (cy < 0) cy = 0;
        }

        private ConsoleColor ConvertToConsoleColor(Color c)
        {
            int R = (int)(c.r * 255);
            int G = (int)(c.g * 255);
            int B = (int)(c.b * 255);

            int index = (R > 128 | G > 128 | B > 128) ? 8 : 0; // Bright bit
            index |= (R > 64) ? 4 : 0; // Red bit
            index |= (G > 64) ? 2 : 0; // Green bit
            index |= (B > 64) ? 1 : 0; // Blue bit
            return (ConsoleColor)index;
        }

        private bool ConsoleColorTest(int x, int y, ConsoleColor c, bool writeOn)
        {
            if (x >= 0 && x < consoleWidth && y >= 0 && y < consoleHeight)
            {
                if (consoleBuffers[x, y] != c)
                {
                    if (writeOn)
                        consoleBuffers[x, y] = c;
                    return true;
                }
            }
            return false;
        }
    }
}
