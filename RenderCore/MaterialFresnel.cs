using System.Collections.Generic;

namespace WPRenderer
{
    public class MaterialFresnel : MaterialTiling
    {
        public Color fresnelColor = Color.white;
        public float fresnelBase = 0f;
        public float fresnelPow = 1;

        Vector3 invWorldSpaceLight;

        public MaterialFresnel(Texture mainTexture, Color mainColor, Vector4 tilingOffset) 
            : base(mainTexture, mainColor, tilingOffset)
        {

        }

        public MaterialFresnel SetFresnelColor(Color c)
        {
            this.fresnelColor = c;
            return this;
        }

        public MaterialFresnel SetFresnelBase(float v)
        {
            this.fresnelBase = v;
            return this;
        }

        public MaterialFresnel SetFresnelPow(float v)
        {
            this.fresnelPow = v;
            return this;
        }

        public override Vector4 CallVertexStage(ref Vertex vertex)
        {
            TransformTex(ref vertex.uv);

            // convert to world normal, Note that rotation only(MultiplyVector).
            vertex.normal = Matrix4x4.MultiplyVector(currentM, vertex.normal).normalized;

            // using inverse light direction dot product normal in the world space.
            invWorldSpaceLight = Vector3.Normalize(-currentLight.direction);

            // world position for calculate view direction.
            vertex.worldPos = currentM * vertex.pos;

            return currentMVP * vertex.pos;
        }

        public override Color CallFragmentStage(ref Vertex vertex)
        {
            Color color = vertex.color * mainColor * Tex2D(mainTexture, vertex.uv.x, vertex.uv.y);
            vertex.normal.Normalize();

            // Fresnel reflection
            Vector3 V = (currentCamera.pos - vertex.worldPos).normalized;
            float fresnelRef = Mathf.Clamp01(fresnelBase + Mathf.Pow(1 - Vector3.Dot(vertex.normal, V), fresnelPow));

            //float diffuse = currentLight.intensity * Mathf.Max(0, Vector3.Dot(invWorldSpaceLight, vertex.normal));
            // half lambert
            float diffuse = 0.5f * currentLight.intensity * Mathf.Max(0, Vector3.Dot(invWorldSpaceLight, vertex.normal)) + 0.5f;

            // alpha dont't need apply calculation
            float alpha = color.a;

            // finally colors
            color = color * currentLight.color * diffuse;
            color = Color.Lerp(color, fresnelColor, fresnelRef);
            color.a = alpha;
            return color;
        }
    }
}
