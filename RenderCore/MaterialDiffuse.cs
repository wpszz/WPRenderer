using System.Collections.Generic;

namespace WPRenderer
{
    public class MaterialDiffuse : MaterialTiling
    {
        Vector3 invWorldSpaceLight;

        public MaterialDiffuse(Texture mainTexture, Color mainColor, Vector4 tilingOffset) 
            : base(mainTexture, mainColor, tilingOffset)
        {

        }

        public override Vector4 CallVertexStage(ref Vertex vertex)
        {
            TransformTex(ref vertex.uv);

            // convert to world normal, Note that rotation only(MultiplyVector).
            vertex.normal = Matrix4x4.MultiplyVector(currentM, vertex.normal).normalized;

            // using inverse light direction dot product normal in the world space.
            invWorldSpaceLight = Vector3.Normalize(-currentLight.direction);

            return currentMVP * vertex.pos;
        }

        public override Color CallFragmentStage(ref Vertex vertex)
        {
            Color color = vertex.color * mainColor * Tex2D(mainTexture, vertex.uv.x, vertex.uv.y);
            vertex.normal.Normalize();

            // lambert
            //float diffuse = currentLight.intensity * Mathf.Max(0, Vector3.Dot(invWorldSpaceLight, vertex.normal));
            // half lambert
            float diffuse = 0.5f * currentLight.intensity * Mathf.Max(0, Vector3.Dot(invWorldSpaceLight, vertex.normal)) + 0.5f;

            // alpha dont't need apply calculation
            float alpha = color.a;

            // finally colors
            color *= currentLight.color * diffuse;
            color.a = alpha;
            return color;
        }
    }
}
