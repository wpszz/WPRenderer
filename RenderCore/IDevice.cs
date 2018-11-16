
namespace WPRenderer
{
    public interface IDevice
    {
        void Initialize(int width, int height);

        void Clear(Color blackground);

        void DrawText(string text, int x, int y, Color c);

        void DrawPixel(int x, int y, Color c);

        void DrawLine(Vertex v1, Vertex v2);

        void DrawTriangle(Vertex v1, Vertex v2, Vertex v3);

        void ConvertFromNDC(float normalizedX, float normalizedY, out float deviceX, out float deviceY);

        void ConvertToNDC(float deviceX, float deviceY, out float normalizedX, out float normalizedY);

        void WaitForPresent();
    }
}
