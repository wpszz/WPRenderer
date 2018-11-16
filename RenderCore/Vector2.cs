
namespace WPRenderer
{
    public struct Vector2
    {
        public static Vector2 zero
        {
            get
            {
                return new Vector2(0f, 0f);
            }
        }

        public static Vector2 one
        {
            get
            {
                return new Vector2(1f, 1f);
            }
        }

        public float magnitude
        {
            get
            {
                return Mathf.Sqrt(this.x * this.x + this.y * this.y);
            }
        }

        public Vector2 normalized
        {
            get
            {
                Vector2 result = new Vector2(this.x, this.y);
                result.Normalize();
                return result;
            }
        }

        public float x, y;

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public void Normalize()
        {
            float magnitude = this.magnitude;
            if (magnitude > 1E-05f)
            {
                this /= magnitude;
            }
            else
            {
                this = Vector2.zero;
            }
        }

        public override string ToString()
        {
            return string.Format("({0:F1}, {1:F1})", this.x, this.y);
        }

        public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
        {
            t = Mathf.Clamp01(t);
            return new Vector2(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t);
        }

        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x + b.x, a.y + b.y);
        }

        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x - b.x, a.y - b.y);
        }

        public static Vector2 operator -(Vector2 a)
        {
            return new Vector2(0f - a.x, 0f - a.y);
        }

        public static Vector2 operator *(Vector2 a, float d)
        {
            return new Vector2(a.x * d, a.y * d);
        }

        public static Vector2 operator *(float d, Vector2 a)
        {
            return new Vector2(a.x * d, a.y * d);
        }

        public static Vector2 operator /(Vector2 a, float d)
        {
            return new Vector2(a.x / d, a.y / d);
        }
    }
}
