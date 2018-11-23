
namespace WPRenderer
{
    public static class NormalizedDevice
    {
        public static void DrawLine(IDevice device, Vertex v1, Vertex v2)
        {
            device.ConvertFromNDC(v1.pos.x, v1.pos.y, out v1.pos.x, out v1.pos.y);
            device.ConvertFromNDC(v2.pos.x, v2.pos.y, out v2.pos.x, out v2.pos.y);

            DrawLineInteger(device, ref v1, ref v2);
        }

        private static void DrawLineInteger(IDevice device, ref Vertex v1, ref Vertex v2)
        {
            int x1 = (int)v1.pos.x;
            int x2 = (int)v2.pos.x;
            int y1 = (int)v1.pos.y;
            int y2 = (int)v2.pos.y;

            int dx = x2 - x1;
            int dy = y2 - y1;

            int stepX = 1;
            int stepY = 1;

            if (dx < 0)
            {
                stepX = -1;
                dx = -dx;
            }

            if (dy < 0)
            {
                stepY = -1;
                dy = -dy;
            }

            int dy2 = dy << 1;
            int dx2 = dx << 1;

            if (dy > dx)
            {
                Mathf.Swap(ref x1, ref y1);
                Mathf.Swap(ref x2, ref y2);
                Mathf.Swap(ref dx, ref dy);
                Mathf.Swap(ref dx2, ref dy2);
                Mathf.Swap(ref stepX, ref stepY);
            }

            int x = x1;
            int y = y1;
            int errorValue = dy2 - dx;
            float delta = 1 / (dx == 0 ? 1.0f : dx);
            float t = 0;
            float invertZ = 0;
            bool zTest = GpuProgram.IsZTestOn();
            bool zWrite = GpuProgram.IsZWriteOn();
            for (int i = 0; i <= dx; i++, x += stepX)
            {
                t = i * delta;
                invertZ = Mathf.Lerp(v1.pos.z, v2.pos.z, t);
                if (!zTest || GpuProgram.ZTest(x, y, invertZ, zWrite))
                {
                    Vertex lv = Vertex.Lerp(v1, v2, t);
                    lv.uv /= invertZ; // real uv mapping by multiple z (or division 1/z)
                    device.DrawPixel(x, y, GpuProgram.CallFragmentStage(ref lv, x, y));
                }

                errorValue += dy2;
                if (errorValue >= 0)
                {
                    errorValue -= dx2;
                    y += stepY;
                }
            }
        }

        public static void DrawTriangle(IDevice device, Vertex v1, Vertex v2, Vertex v3)
        {
            device.ConvertFromNDC(v1.pos.x, v1.pos.y, out v1.pos.x, out v1.pos.y);
            device.ConvertFromNDC(v2.pos.x, v2.pos.y, out v2.pos.x, out v2.pos.y);
            device.ConvertFromNDC(v3.pos.x, v3.pos.y, out v3.pos.x, out v3.pos.y);

            // sorted by y, v1 < v2 < v3
            if (v2.pos.y < v1.pos.y)
            {
                Vertex tmp = v1;
                v1 = v2;
                v2 = tmp;
            }
            if (v3.pos.y < v1.pos.y)
            {
                Vertex tmp = v1;
                v1 = v3;
                v3 = tmp;
            }
            if (v3.pos.y < v2.pos.y)
            {
                Vertex tmp = v2;
                v2 = v3;
                v3 = tmp;
            }

            if (v1.pos.y == v2.pos.y)
            {
                DrawFlatBottomTriangle(device, v1, v2, v3);
            }
            else if (v2.pos.y == v3.pos.y)
            {
                DrawFlatTopTriangle(device, v2, v3, v1);
            }
            else
            {
                float t = (v2.pos.y - v1.pos.y) / (v3.pos.y - v1.pos.y);
                Vertex tmpV = Vertex.Lerp(v1, v3, t);
                // fixed float accuracy loss
                tmpV.pos.y = v2.pos.y;
                DrawFlatBottomTriangle(device, v2, tmpV, v3);
                DrawFlatTopTriangle(device, v2, tmpV, v1);
            }
        }

        private static void DrawFlatBottomTriangle(IDevice device, Vertex bottomLeft, Vertex bottomRight, Vertex top)
        {
            float topY = top.pos.y;
            float bottomY = bottomLeft.pos.y;
            float disY = topY - bottomY;
            if (disY == 0)
            {
                DrawLineInteger(device, ref bottomLeft, ref bottomRight);
            }
            else
            {
                for (float y = topY; y >= bottomY; y--)
                {
                    float t = (y - bottomY) / disY;
                    Vertex v1 = Vertex.Lerp(bottomLeft, top, t);
                    Vertex v2 = Vertex.Lerp(bottomRight, top, t);
                    // fixed float accuracy loss
                    v1.pos.y = y;
                    v2.pos.y = y;
                    DrawLineInteger(device, ref v1, ref v2);
                }
            }
        }

        private static void DrawFlatTopTriangle(IDevice device, Vertex topLeft, Vertex topRight, Vertex bottom)
        {
            float topY = topLeft.pos.y;
            float bottomY = bottom.pos.y;
            float disY = topY - bottomY;
            if (disY == 0)
            {
                DrawLineInteger(device, ref topLeft, ref topRight);
            }
            else
            {
                for (float y = topY; y >= bottomY; y--)
                {
                    float t = (y - bottomY) / disY;
                    Vertex v1 = Vertex.Lerp(bottom, topLeft, t);
                    Vertex v2 = Vertex.Lerp(bottom, topRight, t);
                    // fixed float accuracy loss
                    v1.pos.y = y;
                    v2.pos.y = y;
                    DrawLineInteger(device, ref v1, ref v2);
                }
            }
        }
    }
}
