using System.Collections.Generic;

namespace WPRenderer
{
    public class Material : GpuShare
    {
        public Texture mainTexture;
        public Color mainColor = Color.white;

        public Material(Texture mainTexture, Color mainColor)
        {
            this.mainTexture = mainTexture;
            this.mainColor = mainColor;
        }

        public virtual Vector4 CallVertexStage(ref Vertex vertex)
        {
            return currentMVP * vertex.pos;
        }

        public virtual Color CallFragmentStage(ref Vertex vertex)
        {
            return vertex.color * mainColor * Tex2D(mainTexture, vertex.uv.x, vertex.uv.y);
        }

        //===================================================================

        protected static Color Tex2D(Texture tex, float u, float v)
        {
            if (tex != null)
                return tex.Sample(u, v);
            return Color.white;
        }

        protected static Color Tex2D(Texture tex, Vector2 uv)
        {
            if (tex != null)
                return tex.Sample(uv.x, uv.y);
            return Color.white;
        }

        protected static Vector4 TexelSize(Texture tex)
        {
            if (tex != null)
                return new Vector4(1f / tex.width, 1f / tex.height, tex.width, tex.height);
            return new Vector4(1, 1, 1, 1);
        }
    }
}
