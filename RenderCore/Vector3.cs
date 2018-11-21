
namespace WPRenderer
{
    public struct Vector3
    {
        public static Vector3 zero
        {
            get
            {
                return new Vector3(0f, 0f, 0f);
            }
        }

        public static Vector3 one
        {
            get
            {
                return new Vector3(1f, 1f, 1f);
            }
        }

        public static Vector3 up
        {
            get
            {
                return new Vector3(0f, 1f, 0f);
            }
        }

        public static Vector3 right
        {
            get
            {
                return new Vector3(1f, 0f, 0f);
            }
        }

        public static Vector3 forward
        {
            get
            {
                return new Vector3(0f, 0f, 1f);
            }
        }

        public Vector3 normalized
        {
            get
            {
                return Vector3.Normalize(this);
            }
        }

        public float x, y, z;

        public Vector3(float x, float y, float z = 0)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public void Normalize()
        {
            float num = Vector3.Magnitude(this);
            if (num > 1E-05f)
            {
                this /= num;
            }
            else
            {
                this = Vector3.zero;
            }
        }

        public override string ToString()
        {
            return string.Format("({0:F1}, {1:F1}, {2:F1})", this.x, this.y, this.z);
        }

        public static Vector3 Lerp(Vector3 a, Vector3 b, float t)
        {
            t = Mathf.Clamp01(t);
            return new Vector3(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t);
        }

        public static Vector3 Cross(Vector3 lhs, Vector3 rhs)
        {
            return new Vector3(lhs.y * rhs.z - lhs.z * rhs.y, lhs.z * rhs.x - lhs.x * rhs.z, lhs.x * rhs.y - lhs.y * rhs.x);
        }

        public static float Dot(Vector3 lhs, Vector3 rhs)
        {
            return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
        }

        public static float Magnitude(Vector3 a)
        {
            return Mathf.Sqrt(a.x * a.x + a.y * a.y + a.z * a.z);
        }

        public static Vector3 Normalize(Vector3 value)
        {
            float num = Vector3.Magnitude(value);
            if (num > 1E-08f)
            {
                return value / num;
            }
            return Vector3.zero;
        }

        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Vector3 operator -(Vector3 a)
        {
            return new Vector3(0f - a.x, 0f - a.y, 0f - a.z);
        }

        public static Vector3 operator *(Vector3 a, float d)
        {
            return new Vector3(a.x * d, a.y * d, a.z * d);
        }

        public static Vector3 operator *(float d, Vector3 a)
        {
            return new Vector3(a.x * d, a.y * d, a.z * d);
        }

        public static Vector3 operator /(Vector3 a, float d)
        {
            return new Vector3(a.x / d, a.y / d, a.z / d);
        }
    }
}
