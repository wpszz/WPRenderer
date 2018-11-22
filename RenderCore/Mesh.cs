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

        public void RecalculateTangents()
        {
            for (int i = 0, count = indexs.Count; i < count; i += 3)
            {
                Vertex vert1 = vertexs[indexs[i]];
                Vertex vert2 = vertexs[indexs[i + 1]];
                Vertex vert3 = vertexs[indexs[i + 2]];

                /*
                 * Pi = P0 + ui(∂P/∂u) + vi(∂P/∂v)
                 * Actually Pi = P0 + (ui - u0)(∂P/∂u) + (vi - v0)(∂P/∂v), and P0<x0,y0,z0> is mapping to uv0<0,0>
                 * 
                 * Pi - P0 = (ui - u0)(∂P/∂u) + (vi - v0)(∂P/∂v)
                 *
                 * P2 - P1 = (u2 - u1)(∂P/∂u) + (v2 - v1)(∂P/∂v)
                 * P3 - P1 = (u3 - u1)(∂P/∂u) + (v3 - v1)(∂P/∂v)
                 * 
                 * | ∂P/∂u |   | u2-u1, v2-v1 |-1    | P2-P1 |
                 *           =                    *
                 * | ∂P/∂v |   | u3-u1, v3-v1 |      | P3-P1 |
                 * 
                 * | u2-u1, v2-v1 |-1                    1                 | v3-v1, v1-v2 |
                 *                    =  ---------------------------------
                 * | u3-u1, v3-v1 |      (u2-u1)*(v3-v1) - (v2-v1)*(u3-u1) | u1-u3, u2-u1 |
                 * 
                 * tangent = ∂P/∂u, binormal = ∂P/∂v
                 */

                Vector3 P21 = vert2.pos - vert1.pos;
                Vector3 P31 = vert3.pos - vert1.pos;

                float u1 = vert1.uv.x;
                float u2 = vert2.uv.x;
                float u3 = vert3.uv.x;

                float v1 = vert1.uv.y;
                float v2 = vert2.uv.y;
                float v3 = vert3.uv.y;

                float det = (u2 - u1) * (v3 - v1) - (v2 - v1) * (u3 - u1);

                float v31 = v3 - v1;
                float v12 = v1 - v2;

                float u13 = u1 - u3;
                float u21 = u2 - u1;

                if (det != 0f)
                {
                    Vector3 tangent = (v31 * P21 + v12 * P31) / det;

                    vert1.tangent = tangent;
                    vert2.tangent = tangent;
                    vert3.tangent = tangent;
                    vertexs[indexs[i]] = vert1;
                    vertexs[indexs[i + 1]] = vert2;
                    vertexs[indexs[i + 2]] = vert3;
                }
            }
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

            mesh.RecalculateTangents();
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

            mesh.RecalculateTangents();
            return mesh;
        }
    }
}
