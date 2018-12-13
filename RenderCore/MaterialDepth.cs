using System.Collections.Generic;

namespace WPRenderer
{
    public class MaterialDepth : Material
    {
        public MaterialDepth(Texture mainTexture, Color mainColor)
            : base(mainTexture, mainColor)
        {

        }

        public override Vector4 CallVertexStage(ref Vertex vertex)
        {
            return currentMVP * vertex.pos;
        }

        public override Color CallFragmentStage(ref Vertex vertex)
        {
            float z = Linear01Depth(vertex.pos.z);
            return new Color(z, z, z);
        }
    }
}
