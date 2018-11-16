using System.Collections.Generic;

namespace WPRenderer
{
    public class MaterialEffect : Material
    {
        public MaterialEffect(Texture mainTexture, Color mainColor)
            : base(mainTexture, mainColor)
        {

        }

        public override Vector4 CallVertexStage(ref Vertex vertex)
        {
            return currentMVP * vertex.pos;
        }

        public override Color CallFragmentStage(ref Vertex vertex)
        {
            return Effect2(ref vertex);
        }

        private Color Effect1(ref Vertex vertex)
        {
            Vector2 dv = new Vector2(0.5f, 0.5f) - vertex.uv;
            Vector2 dir = dv.normalized;
            vertex.uv += dir * currentSinTime.w * 0.2f;
            return vertex.color * mainColor * Tex2D(mainTexture, vertex.uv.x, vertex.uv.y);
        }

        private Color Effect2(ref Vertex vertex)
        {
            Vector2 dv = new Vector2(0.5f, 0.5f) - vertex.uv;
            Vector2 dir = dv.normalized;
            float dis = dv.magnitude;
            float sinFactor = Mathf.Sin(dis * 80f + currentTime.y * 20f) * 0.005f;
            vertex.uv += dir * sinFactor;
            return vertex.color * mainColor * Tex2D(mainTexture, vertex.uv.x, vertex.uv.y);
        }
    }
}
