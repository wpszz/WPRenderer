using System.Collections.Generic;

namespace WPRenderer
{
    public class MaterialDiffuse : MaterialTiling
    {
        public MaterialDiffuse(Texture mainTexture, Color mainColor, Vector4 tilingOffset) 
            : base(mainTexture, mainColor, tilingOffset)
        {

        }

        public override Vector4 CallVertexStage(ref Vertex vertex)
        {
            TransformTex(ref vertex.uv);

            vertex.normal = Matrix4x4.Mul3x3(currentM, vertex.normal).normalized;

            return currentMVP * vertex.pos;
        }

        public override Color CallFragmentStage(ref Vertex vertex)
        {
            Color color = vertex.color * mainColor * Tex2D(mainTexture, vertex.uv.x, vertex.uv.y);
            if (currentLight != null)
            {
                vertex.normal.Normalize();
                // lambert
                //float diffuse = currentLight.intensity * Vector3.Dot(currentLight.direction, vertex.normal);
                // half lambert
                float diffuse = 0.5f * currentLight.intensity * Vector3.Dot(currentLight.direction, vertex.normal) + 0.5f;
                color *= currentLight.color * diffuse;
            }
            return color;
        }
    }
}
