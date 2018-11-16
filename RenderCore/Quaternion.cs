
namespace WPRenderer
{
    public struct Quaternion
    {
        public float x, y, z, w;

        public Quaternion(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public Matrix4x4 GetMatrix()
        {
            float x2 = x * x;
            float y2 = y * y;
            float z2 = z * z;
            float xy = x * y;
            float xz = x * z;
            float yz = y * z;
            float wx = w * x;
            float wy = w * y;
            float wz = w * z;
            return new Matrix4x4(
                1.0f - 2.0f * (y2 + z2),    2.0f * (xy - wz),           2.0f * (xz + wy),           0.0f,
                2.0f * (xy + wz),           1.0f - 2.0f * (x2 + z2),    2.0f * (yz - wx),           0.0f,
                2.0f * (xz - wy),           2.0f * (yz + wx),           1.0f - 2.0f * (x2 + y2),    0.0f,
                0.0f,                       0.0f,                       0.0f,                       1.0f
            );
        }

        public static Quaternion Euler(float x, float y, float z)
        {
            // order by ZXY axis
            x = x * Mathf.Deg2Rad * 0.5f;
            y = y * Mathf.Deg2Rad * 0.5f;
            z = z * Mathf.Deg2Rad * 0.5f;
            float qx = Mathf.Sin(x) * Mathf.Cos(y) * Mathf.Cos(z) + Mathf.Cos(x) * Mathf.Sin(y) * Mathf.Sin(z);
            float qy = Mathf.Cos(x) * Mathf.Sin(y) * Mathf.Cos(z) - Mathf.Sin(x) * Mathf.Cos(y) * Mathf.Sin(z);
            float qz = Mathf.Cos(x) * Mathf.Cos(y) * Mathf.Sin(z) - Mathf.Sin(x) * Mathf.Sin(y) * Mathf.Cos(z);
            float qw = Mathf.Cos(x) * Mathf.Cos(y) * Mathf.Cos(z) + Mathf.Sin(x) * Mathf.Sin(y) * Mathf.Sin(z);
            return new Quaternion(qx, qy, qz, qw);
        }

        public static float Dot(Quaternion a, Quaternion b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
        }

        public static Quaternion operator *(Quaternion lhs, Quaternion rhs)
        {
            return new Quaternion(
                lhs.w * rhs.x + lhs.x * rhs.w + lhs.y * rhs.z - lhs.z * rhs.y, 
                lhs.w * rhs.y + lhs.y * rhs.w + lhs.z * rhs.x - lhs.x * rhs.z, 
                lhs.w * rhs.z + lhs.z * rhs.w + lhs.x * rhs.y - lhs.y * rhs.x, 
                lhs.w * rhs.w - lhs.x * rhs.x - lhs.y * rhs.y - lhs.z * rhs.z
            );
        }

        public static Vector3 operator *(Quaternion rotation, Vector3 point)
        {
            float num = rotation.x * 2f;
            float num2 = rotation.y * 2f;
            float num3 = rotation.z * 2f;
            float num4 = rotation.x * num;
            float num5 = rotation.y * num2;
            float num6 = rotation.z * num3;
            float num7 = rotation.x * num2;
            float num8 = rotation.x * num3;
            float num9 = rotation.y * num3;
            float num10 = rotation.w * num;
            float num11 = rotation.w * num2;
            float num12 = rotation.w * num3;
            Vector3 result = default(Vector3);
            result.x = (1f - (num5 + num6)) * point.x + (num7 - num12) * point.y + (num8 + num11) * point.z;
            result.y = (num7 + num12) * point.x + (1f - (num4 + num6)) * point.y + (num9 - num10) * point.z;
            result.z = (num8 - num11) * point.x + (num9 + num10) * point.y + (1f - (num4 + num5)) * point.z;
            return result;
        }
    }
}
