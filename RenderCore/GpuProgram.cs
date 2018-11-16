
namespace WPRenderer
{
    public sealed class GpuProgram : GpuShare
    {
        private static float[,] zBuffers;
        private static int zBuffersWidth, zBuffersHeight;

        public static void Initialize(IDevice device, int width, int height)
        {
            device.Initialize(width, height);

            zBuffers = new float[width, height];
            zBuffersWidth = width;
            zBuffersHeight = height;
        }

        public static void Clear(IDevice device, Color blackground)
        {
            device.Clear(blackground);

            for (int i = 0; i < zBuffersWidth; i++)
            {
                for (int j = 0; j < zBuffersHeight; j++)
                {
                    zBuffers[i, j] = float.MinValue;
                }
            }

            float time = TimeHelper.GetTimeSinceStartup();

            currentMaterial = null;
            currentLight = null;
            currentM = Matrix4x4.identity;
            currentV = Matrix4x4.identity;
            currentP = Matrix4x4.identity;
            currentMVP = Matrix4x4.identity;
            currentTime = new Vector4(time / 20f, time, time * 2f, time * 3f);
            currentSinTime = new Vector4(Mathf.Sin(time / 8f), Mathf.Sin(time / 4f), Mathf.Sin(time / 2f), Mathf.Sin(time));
        }

        public static void SetDirectionLight(Light light)
        {
            currentLight = light;
        }

        public static void SetCamera(Camera camera)
        {
            currentCamera = camera;
        }

        public static void DrawCall(IDevice device, Mesh mesh, Material material, Matrix4x4 M, Matrix4x4 V, Matrix4x4 P)
        {
            currentMaterial = material;
            currentM = M;
            currentV = V;
            currentP = P;
            currentMVP = P * (V * M);

            for (int i = 0, count = mesh.indexs.Count; i < count; i += 3)
            {
                Vertex v1 = mesh.vertexs[mesh.indexs[i]];
                Vertex v2 = mesh.vertexs[mesh.indexs[i + 1]];
                Vertex v3 = mesh.vertexs[mesh.indexs[i + 2]];

                Vector4 hc1 = CallVertexStage(ref v1);
                Vector4 hc2 = CallVertexStage(ref v2);
                Vector4 hc3 = CallVertexStage(ref v3);

                if (CanonicalViewVolumeCull(hc1) && CanonicalViewVolumeCull(hc2) && CanonicalViewVolumeCull(hc3))
                    continue;

                PerspectiveDivisionToNDC(hc1, ref v1);
                PerspectiveDivisionToNDC(hc2, ref v2);
                PerspectiveDivisionToNDC(hc3, ref v3);

                NormalizedDevice.DrawTriangle(device, v1, v2, v3);
            }
        }

        //=================================================================

        public static Vector4 CallVertexStage(ref Vertex vertex)
        {
            if (currentMaterial != null)
                return currentMaterial.CallVertexStage(ref vertex);
            return currentMVP * vertex.pos;
        }

        public static Color CallFragmentStage(ref Vertex vertex)
        {
            Color color = vertex.color;
            if (currentMaterial != null)
                color = currentMaterial.CallFragmentStage(ref vertex);
            // simple clamp color values to [0, 1]
            color = Color.Saturate(color);
            return color;
        }

        //=================================================================

        public static bool ZTest(int x, int y, float z, bool writeON)
        {
            if (x >= 0 && x < zBuffersWidth && y >= 0 && y < zBuffersHeight)
            {
                if (zBuffers[x, y] <= z)
                {
                    if (writeON)
                        zBuffers[x, y] = z;
                    return true;
                }
            }
            return false;
        }

        // CVV cull with homogeneous coordinate
        public static bool CanonicalViewVolumeCull(Vector4 hc)
        {
            float w = hc.w;
            float x = hc.x;
            if (x < -w || x > w)
                return true;
            float y = hc.y;
            if (y < -w || y > w)
                return true;
            float z = hc.z;
            if (z < -w || z > w)
                return true;
            return false;
        }

        public static void PerspectiveDivisionToNDC(Vector4 hc, ref Vertex vertex)
        {
            /*
             * hc(x'z, y'z, z'z, z)
             */
            float invertZ = 1 / hc.w;
            vertex.pos.x = hc.x * invertZ;
            vertex.pos.y = hc.y * invertZ;

            // discard z', instead by 1/z for ZTest and uv sample 
            vertex.pos.z = invertZ;

            // uv is linear with 1/z, finially texture sample need restore(by multiple z or division 1/z from vertex.pos.z)
            vertex.uv *= invertZ;
        }
    }
}
