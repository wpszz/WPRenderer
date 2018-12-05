using System;

namespace WPRenderer
{
    public static class Mathf
    {
        public const float PI = (float)Math.PI;
        public const float Deg2Rad = PI / 180f;
        public const float Rad2Deg = 180f / PI;

        public static float Max(float a, float b)
        {
            return a > b ? a : b;
        }

        public static float Min(float a, float b)
        {
            return a > b ? b : a;
        }

        public static float Pow(float a, float b)
        {
            return (float)Math.Pow(a, b);
        }

        public static float Clamp01(float value)
        {
            if (value < 0f)
            {
                return 0f;
            }
            if (value > 1f)
            {
                return 1f;
            }
            return value;
        }

        public static float Clamp(float value, float min, float max)
        {
            if (value < min)
            {
                value = min;
            }
            else if (value > max)
            {
                value = max;
            }
            return value;
        }
    
        public static int Clamp(int value, int min, int max)
        {
            if (value < min)
            {
                value = min;
            }
            else if (value > max)
            {
                value = max;
            }
            return value;
        }

        public static float Lerp(float a, float b, float t)
        {
            return a + (b - a) * Clamp01(t);
        }

        public static float Sqrt(float x)
        {
            return (float)Math.Sqrt(x);
        }

        public static float Sin(float x)
        {
            return (float)Math.Sin(x);
        }

        public static float Cos(float x)
        {
            return (float)Math.Cos(x);
        }

        public static float Tan(float x)
        {
            return (float)Math.Tan(x);
        }

        public static float Acos(float x)
        {
            return (float)Math.Acos(x);
        }

        public static float Asin(float x)
        {
            return (float)Math.Asin(x);
        }

        public static void Swap<T>(ref T a, ref T b)
        {
            T c = a;
            a = b;
            b = c;
        }

        public static float Abs(float x)
        {
            return Math.Abs(x);
        }

        public static float Repeat(float t, float length)
        {
            t = Mathf.Abs(t);
            return t - (int)(t / length) * length;
        }

        public static int Round(float x)
        {
            return (int)(x + 0.5f);
        }

        public static int FloorToInt(float f)
        {
            return (int)Math.Floor(f);
        }

        public static int CeilToInt(float f)
        {
            return (int)Math.Ceiling(f);
        }

    }
}
