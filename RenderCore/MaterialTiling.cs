using System.Collections.Generic;

namespace WPRenderer
{
    public class MaterialTiling : Material
    {
        public Vector4 tilingOffset = new Vector4(1, 1, 0, 0);

        public MaterialTiling(Texture mainTexture, Color mainColor, Vector4 tilingOffset) 
            : base(mainTexture, mainColor)
        {
            this.tilingOffset = tilingOffset;
        }

        public override Vector4 CallVertexStage(ref Vertex vertex)
        {
            TransformTex(ref vertex.uv);
            return currentMVP * vertex.pos;
        }

        public override Color CallFragmentStage(ref Vertex vertex)
        {
            return vertex.color * mainColor * Tex2D(mainTexture, vertex.uv.x, vertex.uv.y);
        }

        protected void TransformTex(ref Vector2 uv)
        {
            uv.x = uv.x * tilingOffset.x + tilingOffset.z;
            uv.y = uv.y * tilingOffset.y + tilingOffset.w;
        }
    }
}
