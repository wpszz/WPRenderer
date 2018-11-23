using System.Collections.Generic;

namespace WPRenderer
{
    public enum BlendOp
    {
        Add = 0,
        Sub = 1,
    }

    public enum BlendMode
    {
        // Blend factor is (0, 0, 0, 0).
        Zero = 0,
        // Blend factor is (1, 1, 1, 1).
        One = 1,
        // Blend factor is (Rd, Gd, Bd, Ad).
        DstColor = 2,
        // Blend factor is (Rs, Gs, Bs, As).
        SrcColor = 3,
        // Blend factor is (1 - Rd, 1 - Gd, 1 - Bd, 1 - Ad).
        OneMinusDstColor = 4,
        // Blend factor is (As, As, As, As).
        SrcAlpha = 5,
        // Blend factor is (1 - Rs, 1 - Gs, 1 - Bs, 1 - As).
        OneMinusSrcColor = 6,
        // Blend factor is (Ad, Ad, Ad, Ad).
        DstAlpha = 7,
        // Blend factor is (1 - Ad, 1 - Ad, 1 - Ad, 1 - Ad).
        OneMinusDstAlpha = 8,
        // Blend factor is (f, f, f, 1); where f = min(As, 1 - Ad).
        SrcAlphaSaturate = 9,
        // Blend factor is (1 - As, 1 - As, 1 - As, 1 - As).
        OneMinusSrcAlpha = 10
    }

    public static class BlendColor
    {
        public static Color FactorColor(Color srcColor, Color destColor, BlendMode factor)
        {
            switch (factor)
            {
                case BlendMode.Zero: return Color.black;
                case BlendMode.One: return Color.white;
                case BlendMode.DstColor: return destColor;
                case BlendMode.SrcColor: return srcColor;
                case BlendMode.OneMinusDstColor: return Color.white - destColor;
                case BlendMode.SrcAlpha: return new Color(srcColor.a, srcColor.a, srcColor.a, srcColor.a);
                case BlendMode.OneMinusSrcColor: return Color.white - srcColor;
                case BlendMode.DstAlpha: return new Color(destColor.a, destColor.a, destColor.a, destColor.a);
                case BlendMode.OneMinusDstAlpha: return new Color(1 - destColor.a, 1 - destColor.a, 1 - destColor.a, 1 - destColor.a);
                case BlendMode.OneMinusSrcAlpha: return new Color(1 - srcColor.a, 1 - srcColor.a, 1 - srcColor.a, 1 - srcColor.a);
            }
            return Color.white;
        }

        public static float FactorAlpha(float srcAlpha, float destAlpha, BlendMode factor)
        {
            switch (factor)
            {
                case BlendMode.Zero: return 0;
                case BlendMode.One: return 1;
                case BlendMode.DstColor: return destAlpha;
                case BlendMode.SrcColor: return srcAlpha;
                case BlendMode.OneMinusDstColor: return 1 - destAlpha;
                case BlendMode.SrcAlpha: return srcAlpha;
                case BlendMode.OneMinusSrcColor: return 1 - srcAlpha;
                case BlendMode.DstAlpha: return destAlpha;
                case BlendMode.OneMinusDstAlpha: return 1 - destAlpha;
                case BlendMode.OneMinusSrcAlpha: return 1 - srcAlpha;
            }
            return 1;
        }

        public static Color OpColor(Color left, Color right, BlendOp op)
        {
            switch (op)
            {
                case BlendOp.Add: return left + right;
                case BlendOp.Sub: return left - right;
            }
            return Color.white;
        }

        public static Color Calculate(Color srcColor, Color destColor, 
            BlendOp op, BlendMode srcFactor, BlendMode destFactor, BlendMode srcFactorA, BlendMode destFactorA)
        {
            Color left = FactorColor(srcColor, destColor, srcFactor) * srcColor;
            Color right = FactorColor(srcColor, destColor, destFactor) * destColor;

            left.a = FactorAlpha(srcColor.a, destColor.a, srcFactorA) * srcColor.a;
            right.a = FactorAlpha(srcColor.a, destColor.a, destFactorA) * destColor.a;

            return OpColor(left, right, op);
        }
    }
}
