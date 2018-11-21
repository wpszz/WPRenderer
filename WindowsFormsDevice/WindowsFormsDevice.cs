using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using WPRenderer;

namespace WindowsFormsDevice
{
    class WindowsFormsDevice : IDevice
    {
        [DllImport("gdi32.dll", EntryPoint = "SetPixelV")]
        static extern int SetPixelV(IntPtr hdc, int x, int y, int crColor);

        [DllImport("gdi32.dll")]
        static extern bool BitBlt(IntPtr hdcDest, int dx, int dy,int dW, int dH, IntPtr hdcSrc, int sx, int sy, int dwRop);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        static extern bool TextOutW(IntPtr hdc, int nXStart, int nYStart, string lpString, int cbString);

        private int deviceWidth;
        private int deviceHeight;

        RenderView renderView;
        System.Drawing.Graphics graphics;
        System.Drawing.Pen pen;
        System.Drawing.Font font;
        System.Drawing.SolidBrush brush;

        System.Drawing.BufferedGraphics doubleBuffer;
        IntPtr doubleBufferHDC;

        public WindowsFormsDevice(RenderView view)
        {
            renderView = view;
        }

        public void Initialize(int width, int height)
        {
            renderView.ClientSize = new System.Drawing.Size(width, height);

            graphics = renderView.CreateGraphics();
            pen = new System.Drawing.Pen(System.Drawing.Color.White);
            font = new System.Drawing.Font("Arial", 16);
            brush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);

            deviceWidth = width;
            deviceHeight = height;

            System.Drawing.BufferedGraphicsContext currentContext = System.Drawing.BufferedGraphicsManager.Current;
            doubleBuffer = currentContext.Allocate(graphics, new System.Drawing.Rectangle(0, 0, width, height));
        }

        public void Clear(Color blackground)
        {
            doubleBuffer.Graphics.Clear(ConvertToSystemDrawColor(blackground));
            doubleBufferHDC = doubleBuffer.Graphics.GetHdc();
            //BitBlt(doubleBufferHDC, 0, 0, deviceWidth, deviceHeight, IntPtr.Zero, 0, 0, 0x00000042);
        }

        public void DrawText(string text, int x, int y, Color c)
        {
            TextOutW(doubleBufferHDC, x, y, text, text.Length);
            //brush.Color = ConvertToSystemDrawColor(c);
            //graphics.DrawString(text, font, brush, x, y);
        }

        public void DrawPixel(int x, int y, Color c)
        {
            SetPixelV(doubleBufferHDC, x, y, ConvertToCOLORREF(c));
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

            // avoid gap between triangles by float accuracy loss
            deviceX = Mathf.CeilToInt(deviceX);
            deviceY = Mathf.CeilToInt(deviceY);
        }

        public void ConvertToNDC(float deviceX, float deviceY, out float normalizedX, out float normalizedY)
        {
            // Console device coordinates to NDC
            normalizedX = deviceX * 2f / deviceWidth - 1f;
            normalizedY = 1f - deviceY * 2f / deviceHeight;
        }

        public void WaitForPresent()
        {
            doubleBuffer.Graphics.ReleaseHdc();
            doubleBuffer.Render(graphics);
            //System.Threading.Thread.Sleep(1);
        }

        //===========================================================

        private System.Drawing.Color ConvertToSystemDrawColor(Color c)
        {
            int R = (int)(c.r * 255);
            int G = (int)(c.g * 255);
            int B = (int)(c.b * 255);
            int A = (int)(c.a * 255);
            return System.Drawing.Color.FromArgb(A, R, G, B);
        }

        private int ConvertToCOLORREF(Color c)
        {
            int R = (int)(c.r * 255);
            int G = (int)(c.g * 255);
            int B = (int)(c.b * 255);
            return R | (G << 8) | (B << 16);
        }
    }
}
