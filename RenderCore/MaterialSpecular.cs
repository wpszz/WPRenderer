using System.Collections.Generic;

namespace WPRenderer
{
    public class MaterialSpecular : MaterialTiling
    {
        public Color specColor = Color.white;
        public float specular = 1f;
        public float gloss = 1;

        Vector3 invWorldSpaceLight;

        public MaterialSpecular(Texture mainTexture, Color mainColor, Vector4 tilingOffset) 
            : base(mainTexture, mainColor, tilingOffset)
        {

        }

        public MaterialSpecular SetSpecColor(Color c)
        {
            this.specColor = c;
            return this;
        }

        public MaterialSpecular SetSpecular(float s)
        {
            this.specular = s;
            return this;
        }

        public MaterialSpecular SetGloss(float g)
        {
            this.gloss = g;
            return this;
        }

        public override Vector4 CallVertexStage(ref Vertex vertex)
        {
            TransformTex(ref vertex.uv);

            // convert to world normal, Note that rotation only(Mul3x3).
            vertex.normal = Matrix4x4.Mul3x3(currentM, vertex.normal).normalized;

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

            // BlinnPhong
            Vector3 V = (currentCamera.pos - vertex.worldPos).normalized;
            Vector3 L = invWorldSpaceLight;
            Vector3 H = (V + L).normalized;
            float nh = Mathf.Max(0, Vector3.Dot(vertex.normal, H));
            float spec = Mathf.Pow(nh, specular * 128.0f) * gloss;

            //float diffuse = currentLight.intensity * Mathf.Max(0, Vector3.Dot(invWorldSpaceLight, vertex.normal));
            // half lambert
            float diffuse = 0.5f * currentLight.intensity * Mathf.Max(0, Vector3.Dot(invWorldSpaceLight, vertex.normal)) + 0.5f;

            // alpha dont't need apply calculation
            float alpha = color.a;

            // finally colors
            color = color * currentLight.color * diffuse + currentLight.color * specColor * spec;
            color.a = alpha;
            return color;
        }
    }
}
