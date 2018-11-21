using System.Collections.Generic;

namespace WPRenderer
{
    public class MaterialNormal : Material
    {
        public float deltaScale = 0.5f;
        public float heightScale = 0.03f;

        public MaterialNormal(Texture mainTexture, Color mainColor)
            : base(mainTexture, mainColor)
        {
        }

        public override Color CallFragmentStage(ref Vertex vertex)
        {
            Vector3 normal = CalculateNormal(ref vertex);
            return new Color(normal.x, normal.y, normal.z);
        }

        protected Vector3 CalculateNormal(ref Vertex vertex)
        {
            Vector2 deltaU = new Vector2(mainTextureTexelSize.x * deltaScale, 0);
            float h1_u = Color.Gray(Tex2D(mainTexture, vertex.uv - deltaU));
            float h2_u = Color.Gray(Tex2D(mainTexture, vertex.uv + deltaU));

            Vector3 tangent_u = new Vector3(deltaU.x, 0, (h2_u - h1_u) * heightScale);

            Vector2 deltaV = new Vector2(0, mainTextureTexelSize.y * deltaScale);
            float h1_v = Color.Gray(Tex2D(mainTexture, vertex.uv - deltaV));
            float h2_v = Color.Gray(Tex2D(mainTexture, vertex.uv + deltaV));

            Vector3 tangent_v = new Vector3(0, deltaV.y, (h2_v - h1_v) * heightScale);

            Vector3 normal = Vector3.Cross(tangent_u, tangent_v).normalized;

            // tangent Space (0, 0, 1) mapping to (0.5, 0.5, 1)
            normal = normal * 0.5f + Vector3.one * 0.5f;

            return normal;
        }
    }
}
