using System.Collections.Generic;

namespace WPRenderer
{
    public class MaterialSpecular : MaterialTiling
    {
        public Color specColor = Color.white;
        public float specular = 1f;
        public float gloss = 1;

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

            vertex.normal = Matrix4x4.Mul3x3(currentM, vertex.normal).normalized;

            vertex.worldPos = currentM * vertex.pos;

            return currentMVP * vertex.pos;
        }

        public override Color CallFragmentStage(ref Vertex vertex)
        {
            Color color = vertex.color * mainColor * Tex2D(mainTexture, vertex.uv.x, vertex.uv.y);
            if (currentLight != null && currentCamera != null)
            {
                vertex.normal.Normalize();

                // BlinnPhong
                Vector3 V = (currentCamera.pos - vertex.worldPos).normalized;
                Vector3 L = currentLight.direction.normalized;
                Vector3 H = (V + L).normalized;
                float nh = Mathf.Max(0, Vector3.Dot(vertex.normal, H));
                float spec = Mathf.Pow(nh, specular * 128.0f) * gloss;

                //float diffuse = currentLight.intensity * Vector3.Dot(currentLight.direction, vertex.normal);
                // half lambert
                float diffuse = 0.5f * currentLight.intensity * Vector3.Dot(currentLight.direction, vertex.normal) + 0.5f;

                // alpha dont't need apply calculation
                float alpha = color.a;

                // finally colors
                color = color * currentLight.color * diffuse + currentLight.color * specColor * spec;
                color.a = alpha;
            }
            return color;
        }
    }
}
