using System.Collections.Generic;

namespace WPRenderer
{
    public class MaterialBumpSelf : MaterialNormal
    {
        float bumpNormalScale = 1f;

        public MaterialBumpSelf(Texture mainTexture, Color mainColor)
            : base(mainTexture, mainColor)
        {

        }

        public override Color CallFragmentStage(ref Vertex vertex)
        {
            Color color = vertex.color * mainColor * Tex2D(mainTexture, vertex.uv.x, vertex.uv.y);
            if (currentLight != null)
            {
                Vector3 normal = CalculateNormal(ref vertex);
                // unpack normal (same work with Unity builtin UnpackNormal)
                normal = normal * 2 - Vector3.one;

                // scale normal
                normal.x *= bumpNormalScale;
                normal.y *= bumpNormalScale;
                normal.Normalize();

                // lambert
                //float diffuse = currentLight.intensity * Vector3.Dot(currentLight.direction, normal);
                // half lambert
                float diffuse = 0.5f * currentLight.intensity * Vector3.Dot(currentLight.direction, normal) + 0.5f;
                color *= currentLight.color * diffuse;
            }
            return color;
        }
    }
}
