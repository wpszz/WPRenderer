using System.Collections.Generic;

namespace WPRenderer
{
    public class MaterialGray : Material
    {
        public MaterialGray(Texture mainTexture, Color mainColor)
            : base(mainTexture, mainColor)
        {

        }

        public override Vector4 CallVertexStage(ref Vertex vertex)
        {
            return currentMVP * vertex.pos;
        }

        public override Color CallFragmentStage(ref Vertex vertex)
        {
            Color color = vertex.color * mainColor * Tex2D(mainTexture, vertex.uv.x, vertex.uv.y);
            float gray = color.r * 0.2126f + color.g * 0.7152f + color.b * 0.0722f;
            return new Color(gray, gray, gray);
        }
    }
}
