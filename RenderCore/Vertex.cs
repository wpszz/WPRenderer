
namespace WPRenderer
{
    public struct Vertex
    {
        public Vector3 pos;
        public Color color;
        public Vector2 uv;
        public Vector3 normal;
        public Vector3 tangent;

        // extra datas, used by shader
        public Vector3 worldPos;

        public Vertex(Vector3 pos, Color color, Vector2 uv, Vector3 normal)
        {
            this.pos = pos;
            this.color = color;
            this.uv = uv;
            this.normal = normal;

            // tangent will calculate from triangles
            this.tangent = Vector3.right;

            // extra datas, used by shader
            this.worldPos = pos;
        }

        public static Vertex Lerp(Vertex from, Vertex to, float t)
        {
            Vertex v = new Vertex(
                  Vector3.Lerp(from.pos, to.pos, t),
                  Color.Lerp(from.color, to.color, t),
                  Vector2.Lerp(from.uv, to.uv, t),
                  Vector3.Lerp(from.normal, to.normal, t)
              );

            // copy tangent
            v.tangent = from.tangent;

            // extra datas, used by shader
            v.worldPos = Vector3.Lerp(from.worldPos, to.worldPos, t);
            return v;
        }
    }
}
