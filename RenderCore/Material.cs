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
    }
}
