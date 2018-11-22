
namespace WPRenderer
{
    public struct Color
    {
        public float r, g, b, a;

        public static Color red
        {
            get
            {
                return new Color(1f, 0f, 0f, 1f);
            }
        }

        public static Color green
        {
            get
            {
                return new Color(0f, 1f, 0f, 1f);
            }
        }

        public static Color blue
        {
            get
            {
                return new Color(0f, 0f, 1f, 1f);
            }
        }

        public static Color white
        {
            get
            {
                return new Color(1f, 1f, 1f, 1f);
            }
        }

        public static Color black
        {
            get
            {
                return new Color(0f, 0f, 0f, 1f);
            }
        }

        public static Color yellow
        {
            get
            {
                return new Color(1f, 0.921568632f, 0.0156862754f, 1f);
            }
        }

        public static Color gray
        {
            get
            {
                return new Color(0.5f, 0.5f, 0.5f, 1f);
            }
        }

        public Color(float r, float g, float b, float a = 1)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public override string ToString()
        {
            return string.Format("RGBA({0:F3}, {1:F3}, {2:F3}, {3:F3})", this.r, this.g, this.b, this.a);
        }

        public static Color Lerp(Color a, Color b, float t)
        {
            t = Mathf.Clamp01(t);
            return new Color(a.r + (b.r - a.r) * t, a.g + (b.g - a.g) * t, a.b + (b.b - a.b) * t, a.a + (b.a - a.a) * t);
        }

        public static Color Saturate(Color c)
        {
            return new Color(Mathf.Clamp01(c.r), Mathf.Clamp01(c.g), Mathf.Clamp01(c.b), Mathf.Clamp01(c.a));
        }

        public static float Gray(Color c)
        {
            return c.r * 0.2126f + c.g * 0.7152f + c.b * 0.0722f;
        }

        public static Color operator +(Color a, Color b)
        {
            return new Color(a.r + b.r, a.g + b.g, a.b + b.b, a.a + b.a);
        }

        public static Color operator -(Color a, Color b)
        {
            return new Color(a.r - b.r, a.g - b.g, a.b - b.b, a.a - b.a);
        }

        public static Color operator *(Color a, Color b)
        {
            return new Color(a.r * b.r, a.g * b.g, a.b * b.b, a.a * b.a);
        }

        public static Color operator *(Color a, float b)
        {
            return new Color(a.r * b, a.g * b, a.b * b, a.a * b);
        }

        public static Color operator *(float b, Color a)
        {
            return new Color(a.r * b, a.g * b, a.b * b, a.a * b);
        }

        public static Color operator /(Color a, float b)
        {
            return new Color(a.r / b, a.g / b, a.b / b, a.a / b);
        }
    }
}
