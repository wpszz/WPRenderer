
namespace WPRenderer
{
    public struct Quaternion
    {
        public float x, y, z, w;

        public static Quaternion identity
        {
            get
            {
                return new Quaternion(0, 0, 0, 1);
            }
        }
 
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
            // column-major
            return new Matrix4x4(
                1.0f - 2.0f * (y2 + z2),    2.0f * (xy - wz),           2.0f * (xz + wy),           0.0f,
                2.0f * (xy + wz),           1.0f - 2.0f * (x2 + z2),    2.0f * (yz - wx),           0.0f,
                2.0f * (xz - wy),           2.0f * (yz + wx),           1.0f - 2.0f * (x2 + y2),    0.0f,
                0.0f,                       0.0f,                       0.0f,                       1.0f
            );
        }

        public override string ToString()
        {
            return string.Format("({0:F3}, {1:F3}, {2:F3}, {3:F3})", this.x, this.y, this.z, this.w);
        }

        /// <summary>
        /// Order by ZXY axis.
        /// All rotation direction is clockwise when looking along the rotation axis towards the origin.
        /// Same as the AxisAngle(Vector3.up, y) * AxisAngle(Vector3.right, x) * AxisAngle(Vector3.forward, z).
        /// </summary>
        public static Quaternion Euler(float x, float y, float z)
        {
            x = x * Mathf.Deg2Rad * 0.5f;
            y = y * Mathf.Deg2Rad * 0.5f;
            z = z * Mathf.Deg2Rad * 0.5f;
            float sx = Mathf.Sin(x);
            float sy = Mathf.Sin(y);
            float sz = Mathf.Sin(z);
            float cx = Mathf.Cos(x);
            float cy = Mathf.Cos(y);
            float cz = Mathf.Cos(z);
            float qx = sx * cy * cz + cx * sy * sz;
            float qy = cx * sy * cz - sx * cy * sz;
            float qz = cx * cy * sz - sx * sy * cz;
            float qw = cx * cy * cz + sx * sy * sz;
            return new Quaternion(qx, qy, qz, qw);
        }

        /// <summary>
        /// The rotation direction is clockwise when looking along the rotation axis towards the origin.
        /// </summary>
        public static Quaternion AxisAngle(Vector3 axis, float angle)
        {
            angle *= Mathf.Deg2Rad * 0.5f;
            float sHalfA = Mathf.Sin(angle);
            float cHalfA = Mathf.Cos(angle);
            return new Quaternion(axis.x * sHalfA, axis.y * sHalfA, axis.z * sHalfA, cHalfA);
        }

        public static float Dot(Quaternion a, Quaternion b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
        }

        public static float Length(Quaternion q)
        {
            return Mathf.Sqrt(Dot(q, q));
        }

        public static Quaternion Normalize(Quaternion q)
        {
            float invLen = 1 / Length(q);
            return new Quaternion(q.x * invLen, q.y * invLen, q.z * invLen, q.w * invLen);
        }

        public static Quaternion Inverse(Quaternion q)
        {
            // q^-1 = q* / |q|^2
            float invNorm2 = 1 / Dot(q, q);
            return new Quaternion(-q.x * invNorm2, -q.y * invNorm2, -q.z * invNorm2, q.w * invNorm2);
        }

        public static Quaternion Lerp(Quaternion q1, Quaternion q2, float t)
        {
            float dt = Dot(q1, q2);
            if (dt < 0.0f)
            {
                q2.x = -q2.x;
                q2.y = -q2.y;
                q2.z = -q2.z;
                q2.w = -q2.w;
            }
            q1.x = Mathf.Lerp(q1.x, q2.x, t);
            q1.y = Mathf.Lerp(q1.y, q2.y, t);
            q1.z = Mathf.Lerp(q1.z, q2.z, t);
            q1.w = Mathf.Lerp(q1.w, q2.w, t);
            return Normalize(q1);
        }

        public static Quaternion Slerp(Quaternion q1, Quaternion q2, float t)
        {
            float dt = Dot(q1, q2);
            if (dt < 0.0f)
            {
                dt = -dt;
                q2.x = -q2.x;
                q2.y = -q2.y;
                q2.z = -q2.z;
                q2.w = -q2.w;
            }

            if (dt < 0.9995f)
            {
                float angle = Mathf.Acos(dt);
                float invSinA = 1 / Mathf.Sqrt(1.0f - dt * dt);    // 1.0f / sin(angle)
                float w1 = Mathf.Sin(angle * (1.0f - t)) * invSinA;
                float w2 = Mathf.Sin(angle * t) * invSinA;
                return new Quaternion(q1.x * w1 + q2.x * w2, q1.y * w1 + q2.y * w2, q1.z * w1 + q2.z * w2, q1.w * w1 + q2.w * w2);
            }
            else
            {
                // if the angle is small, use linear interpolation
                return Lerp(q1, q2, t);
            }
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
