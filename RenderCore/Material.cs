using System.Collections.Generic;

namespace WPRenderer
{
    public class Material : GpuShare
    {
        // name
        public string name;

        public Texture mainTexture;
        public Color mainColor = Color.white;

        // zTest
        public bool zTest = true;
        public bool zWrite = true;

        // blend settings
        public bool blendEnbale = false;
        public BlendOp blendOp = BlendOp.Add;
        public BlendMode srcFactor = BlendMode.SrcAlpha;
        public BlendMode destFactor = BlendMode.OneMinusSrcAlpha;
        public BlendMode srcFactorA = BlendMode.SrcAlpha;
        public BlendMode destFactorA = BlendMode.OneMinusSrcAlpha;

        // cull
        public CullFaceType cull = CullFaceType.None;

        // internal cache variables
        protected Vector4 mainTextureTexelSize;

        public Material(Texture mainTexture, Color mainColor)
        {
            this.mainTexture = mainTexture;
            this.mainColor = mainColor;

            mainTextureTexelSize = TexelSize(mainTexture);

            this.name = "unnamed";
        }

        public Material SetName(string name)
        {
            this.name = name;
            return this;
        }

        //===================================================================

        public Material SetZTestEnable(bool enable)
        {
            this.zTest = enable;
            return this;
        }

        public Material SetZWriteEnable(bool enable)
        {
            this.zWrite = enable;
            return this;
        }

        //===================================================================

        public Material SetBlendEnable(bool enable)
        {
            this.blendEnbale = enable;
            return this;
        }

        public Material SetBlendOp(BlendOp op)
        {
            this.blendOp = op;
            return this;
        }

        public Material SetBlendFactors(BlendMode srcFactor, BlendMode destFactor)
        {
            this.srcFactor = srcFactor;
            this.destFactor = destFactor;
            this.srcFactorA = srcFactor;
            this.destFactorA = destFactor;
            return this;
        }

        public Material SetBlendFactors(BlendMode srcFactor, BlendMode destFactor, BlendMode srcFactorA, BlendMode destFactorA)
        {
            this.srcFactor = srcFactor;
            this.destFactor = destFactor;
            this.srcFactorA = srcFactorA;
            this.destFactorA = destFactorA;
            return this;
        }

        //===================================================================

        public Material SetCull(CullFaceType cull)
        {
            this.cull = cull;
            return this;
        }

        //===================================================================

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
