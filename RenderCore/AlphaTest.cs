using System.Collections.Generic;

namespace WPRenderer
{
    public enum AlphaTestType
    {
        Greater = 0,    // Only render pixels whose alpha is greater than AlphaValue.
        GEqual =1,      // Only render pixels whose alpha is greater than or equal to AlphaValue.
        Less = 2,       // Only render pixels whose alpha value is less than AlphaValue.
        LEqual = 3,     // Only render pixels whose alpha value is less than or equal to from AlphaValue.
        Equal = 4,      // Only render pixels whose alpha value equals AlphaValue.
        NotEqual = 5,   // Only render pixels whose alpha value differs from AlphaValue.
        Always = 6,     // Render all pixels. This is functionally equivalent to AlphaTest Off.
        Never = 7,      // Don’t render any pixels.
    }

    public static class AlphaTest
    {
        public static bool IsPass(AlphaTestType type, float sourceAlpha, float targetAlpha)
        {
            switch (type)
            {
                case AlphaTestType.Greater: return sourceAlpha > targetAlpha;
                case AlphaTestType.GEqual: return sourceAlpha >= targetAlpha;
                case AlphaTestType.Less: return sourceAlpha < targetAlpha;
                case AlphaTestType.LEqual: return sourceAlpha <= targetAlpha;
                case AlphaTestType.Equal: return sourceAlpha == targetAlpha;
                case AlphaTestType.NotEqual: return sourceAlpha != targetAlpha;
                case AlphaTestType.Always: return true;
                case AlphaTestType.Never: return false;
            }
            return true;
        }
    }
}
