using System.Collections.Generic;

namespace WPRenderer
{
    public class Mesh
    {
        public List<Vertex> vertexs = new List<Vertex>();
        public List<int> indexs = new List<int>();

        public void AddVertex(Vector3 pos, Color color, Vector2 uv, Vector3 normal)
        {
            vertexs.Add(new Vertex(pos, color, uv, normal));
        }

        public void AddTriangle(int i, int j, int k)
        {
            indexs.Add(i);
            indexs.Add(j);
            indexs.Add(k);
        }

        //============================================================================

        public static Mesh CreatePanel(bool colored = false)
        {
            /*
             * 1  0
             * 3  2
             */
            Mesh mesh = new Mesh();

            Color c1 = Color.white;
            Color c2 = colored ? Color.red : Color.white;
            Color c3 = colored ? Color.green : Color.white;
            Color c4 = colored ? Color.blue : Color.white;

            mesh.AddVertex(new Vector3(0.5f, 0f, 0.5f), c1, new Vector2(1, 0), new Vector3(0, 1, 0));
            mesh.AddVertex(new Vector3(-0.5f, 0f, 0.5f), c2, new Vector2(0, 0), new Vector3(0, 1, 0));
            mesh.AddVertex(new Vector3(0.5f, 0f, -0.5f), c3, new Vector2(1, 1), new Vector3(0, 1, 0));
            mesh.AddVertex(new Vector3(-0.5f, 0f, -0.5f), c4, new Vector2(0, 1), new Vector3(0, 1, 0));
            mesh.AddTriangle(0, 3, 1);
            mesh.AddTriangle(0, 2, 3);
            return mesh;
        }

        public static Mesh CreateCube(bool colored = false)
        {
            Mesh mesh = new Mesh();

            Color c1 = Color.white;
            Color c2 = colored ? Color.red : Color.white;
            Color c3 = colored ? Color.green : Color.white;
            Color c4 = colored ? Color.blue : Color.white;

            // bottom
            mesh.AddVertex(new Vector3(0.5f, -0.5f, 0.5f), c1, new Vector2(1, 0), new Vector3(0, -1, 0));
            mesh.AddVertex(new Vector3(-0.5f, -0.5f, 0.5f), c2, new Vector2(0, 0), new Vector3(0, -1, 0));
            mesh.AddVertex(new Vector3(0.5f, -0.5f, -0.5f), c3, new Vector2(1, 1), new Vector3(0, -1, 0));
            mesh.AddVertex(new Vector3(-0.5f, -0.5f, -0.5f), c4, new Vector2(0, 1), new Vector3(0, -1, 0));
            mesh.AddTriangle(0, 3, 1);
            mesh.AddTriangle(0, 2, 3);

            // top
            mesh.AddVertex(new Vector3(0.5f, 0.5f, 0.5f), c1, new Vector2(1, 0), new Vector3(0, 1, 0));
            mesh.AddVertex(new Vector3(-0.5f, 0.5f, 0.5f), c2, new Vector2(0, 0), new Vector3(0, 1, 0));
            mesh.AddVertex(new Vector3(0.5f, 0.5f, -0.5f), c3, new Vector2(1, 1), new Vector3(0, 1, 0));
            mesh.AddVertex(new Vector3(-0.5f, 0.5f, -0.5f), c4, new Vector2(0, 1), new Vector3(0, 1, 0));
            mesh.AddTriangle(4, 7, 5);
            mesh.AddTriangle(4, 6, 7);

            // front
            mesh.AddVertex(new Vector3(0.5f, 0.5f, -0.5f), c1, new Vector2(1, 0), new Vector3(0, 0, -1));
            mesh.AddVertex(new Vector3(-0.5f, 0.5f, -0.5f), c2, new Vector2(0, 0), new Vector3(0, 0, -1));
            mesh.AddVertex(new Vector3(0.5f, -0.5f, -0.5f), c3, new Vector2(1, 1), new Vector3(0, 0, -1));
            mesh.AddVertex(new Vector3(-0.5f, -0.5f, -0.5f), c4, new Vector2(0, 1), new Vector3(0, 0, -1));
            mesh.AddTriangle(8, 11, 9);
            mesh.AddTriangle(8, 10, 11);

            // back
            mesh.AddVertex(new Vector3(0.5f, 0.5f, 0.5f), c1, new Vector2(1, 0), new Vector3(0, 0, 1));
            mesh.AddVertex(new Vector3(-0.5f, 0.5f, 0.5f), c2, new Vector2(0, 0), new Vector3(0, 0, 1));
            mesh.AddVertex(new Vector3(0.5f, -0.5f, 0.5f), c3, new Vector2(1, 1), new Vector3(0, 0, 1));
            mesh.AddVertex(new Vector3(-0.5f, -0.5f, 0.5f), c4, new Vector2(0, 1), new Vector3(0, 0, 1));
            mesh.AddTriangle(12, 15, 13);
            mesh.AddTriangle(12, 14, 15);

            // left
            mesh.AddVertex(new Vector3(-0.5f, 0.5f, 0.5f), c1, new Vector2(1, 0), new Vector3(-1, 0, 0));
            mesh.AddVertex(new Vector3(-0.5f, -0.5f, 0.5f), c2, new Vector2(0, 0), new Vector3(-1, 0, 0));
            mesh.AddVertex(new Vector3(-0.5f, 0.5f, -0.5f), c3, new Vector2(1, 1), new Vector3(-1, 0, 0));
            mesh.AddVertex(new Vector3(-0.5f, -0.5f, -0.5f), c4, new Vector2(0, 1), new Vector3(-1, 0, 0));
            mesh.AddTriangle(16, 19, 17);
            mesh.AddTriangle(16, 18, 19);

            // right
            mesh.AddVertex(new Vector3(0.5f, 0.5f, 0.5f), c1, new Vector2(1, 0), new Vector3(1, 0, 0));
            mesh.AddVertex(new Vector3(0.5f, -0.5f, 0.5f), c2, new Vector2(0, 0), new Vector3(1, 0, 0));
            mesh.AddVertex(new Vector3(0.5f, 0.5f, -0.5f), c3, new Vector2(1, 1), new Vector3(1, 0, 0));
            mesh.AddVertex(new Vector3(0.5f, -0.5f, -0.5f), c4, new Vector2(0, 1), new Vector3(1, 0, 0));
            mesh.AddTriangle(20, 23, 21);
            mesh.AddTriangle(20, 22, 23);

            return mesh;
        }
    }
}
