using System;

namespace WPRenderer
{
    class Program
    {
        static IDevice device = new ConsoleDevice();

        const int DeviceWidth = 800;
        const int DeviceHeight = 480;
        static Camera camera = new Camera(new Vector3(0, 0, 0), new Vector3(0, 0, 1), Vector3.up, (float)DeviceWidth / DeviceHeight);

        static Mesh mesh = Mesh.CreatePanel(true);

        static Material material = new Material(null, Color.white);
 
        static void Main(string[] args)
        {
            GpuProgram.Initialize(device, DeviceWidth, DeviceHeight);

            while (true)
            {
                Render();
                FPS.Draw(device);
                device.WaitForPresent();
            }
        }

        static void Render()
        {
            GpuProgram.Clear(device, new Color(49 / 255.0f, 77 / 255.0f, 121 / 255.0f));

            // test draw codes
            //device.DrawPixel(10, 40, Color.red);
            //device.DrawPixel(20, 40, Color.green);
            //device.DrawPixel(20, 60, Color.green);

            //NormalizedDevice.DrawLine(device, new Vertex(new Vector3(0, 0), Color.red, new Vector2(0, 0)),
            //    new Vertex(new Vector3(0.5f, 0.5f), Color.green, new Vector2(1, 1)));

            //NormalizedDevice.DrawTriangle(device, new Vertex(new Vector3(0, 0.25f), Color.red, new Vector2(0, 0)),
            //    new Vertex(new Vector3(-0.25f, -0.25f), Color.green, new Vector2(0, 0)),
            //    new Vertex(new Vector3(0.25f, -0.45f), Color.yellow, new Vector2(0, 0)));

            Matrix4x4 M = Matrix4x4.TRS(new Vector3(0, 0, 2), Quaternion.Euler(TimeHelper.GetTimeSinceStartup() * 10, 0, 0), Vector3.one);
            Matrix4x4 V = camera.WorldToCameraMatrix();
            Matrix4x4 P = camera.ProjectionMatrix();
            GpuProgram.DrawCall(device, mesh, material, M, V, P);
        }
    }
}
