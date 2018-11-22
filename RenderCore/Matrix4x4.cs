
namespace WPRenderer
{
    public struct Matrix4x4
    {
        public float m00, m01, m02, m03;
        public float m10, m11, m12, m13;
        public float m20, m21, m22, m23;
        public float m30, m31, m32, m33;

        public static Matrix4x4 identity
        {
            get
            {
                return new Matrix4x4(
                    1,  0,  0,  0,
                    0,  1,  0,  0,
                    0,  0,  1,  0,
                    0,  0,  0,  1
                );
            }
        }

        public float determinant
        {
            get
            {
                return Determinant4x4(
                    m00, m01, m02, m03,
                    m10, m11, m12, m13,
                    m20, m21, m22, m23,
                    m30, m31, m32, m33
                );
            }
        }

        public Matrix4x4(
            float m00, float m01, float m02, float m03,
            float m10, float m11, float m12, float m13,
            float m20, float m21, float m22, float m23,
            float m30, float m31, float m32, float m33)
        {
            this.m00 = m00; this.m01 = m01; this.m02 = m02; this.m03 = m03;
            this.m10 = m10; this.m11 = m11; this.m12 = m12; this.m13 = m13;
            this.m20 = m20; this.m21 = m21; this.m22 = m22; this.m23 = m23;
            this.m30 = m30; this.m31 = m31; this.m32 = m32; this.m33 = m33;
        }

        public static Matrix4x4 Translate(Vector3 v)
        {
            return new Matrix4x4(
                1,      0,      0,      v.x,
                0,      1,      0,      v.y,
                0,      0,      1,      v.z,
                0,      0,      0,      1
            );
        }

        public static Matrix4x4 Scale(Vector3 v)
        {
            return new Matrix4x4(
                v.x,    0,      0,      0,
                0,      v.y,    0,      0,
                0,      0,      v.z,    0,
                0,      0,      0,      1
            );
        }

        public static Matrix4x4 EulerRotation(float x, float y, float z)
        {
            // order by ZXY axis: mul(rotateY(y), mul(rotateX(x), rotateZ(z)));
            float sx = Mathf.Sin(x);
            float cx = Mathf.Cos(x);
            float sy = Mathf.Sin(y);
            float cy = Mathf.Cos(y);
            float sz = Mathf.Sin(z);
            float cz = Mathf.Cos(z);
            return new Matrix4x4(
                cy * cz + sx * sy * sz,     cz * sx * sy - cy * sz,     cx * sy,        0,
                cx * sz,                    cx * cz,                    -sx,            0,
                cy * sx * sz - cz * sy,     cy * cz * sx + sy * sz,     cx * cy,        0,
                0,                          0,                          0,              1
            );
        }

        public static Matrix4x4 RotateX(float angle)
        {
            float s = Mathf.Sin(angle);
            float c = Mathf.Cos(angle);
            return new Matrix4x4(
                1,      0,      0,      0,
                0,      c,     -s,      0,
                0,      s,      c,      0,
                0,      0,      0,      1
            );
        }

        public static Matrix4x4 RotateY(float angle)
        {
            float s = Mathf.Sin(angle);
            float c = Mathf.Cos(angle);
            return new Matrix4x4(
                c,      0,      s,      0,
                0,      1,      0,      0,
                -s,     0,      c,      0,
                0,      0,      0,      1
            );
        }

        public static Matrix4x4 RotateZ(float angle)
        {
            float s = Mathf.Sin(angle);
            float c = Mathf.Cos(angle);
            return new Matrix4x4(
                c,     -s,      0,      0,
                s,      c,      0,      0,
                0,      0,      1,      0,
                0,      0,      0,      1
            );
        }

        public static Matrix4x4 TRS(Vector3 pos, Quaternion q, Vector3 s)
        {
            Matrix4x4 matScale = Scale(s);
            Matrix4x4 matTranslation = Translate(pos);
            Matrix4x4 matRotation = q.GetMatrix();
            return matTranslation * matRotation * matScale;
        }

        public static Matrix4x4 TRSInverse(Vector3 pos, Quaternion q, Vector3 s)
        {
            Matrix4x4 matScaleInverse = Scale(new Vector3(1 / s.x, 1 / s.y, 1 / s.z));
            Matrix4x4 matTranslationInverse = Translate(-pos);
            Matrix4x4 matRotationInverse = Transpose(q.GetMatrix());
            return matScaleInverse * matRotationInverse * matTranslationInverse;
        }

        public static Matrix4x4 Transpose(Matrix4x4 m)
        {
            return new Matrix4x4(
                m.m00,      m.m10,      m.m20,      m.m30,
                m.m01,      m.m11,      m.m21,      m.m31,
                m.m02,      m.m12,      m.m22,      m.m32,
                m.m03,      m.m13,      m.m23,      m.m33
            );
        }

        public static Matrix4x4 Inverse(Matrix4x4 m)
        {
            float c00 = Determinant3x3(m.m11, m.m12, m.m13, m.m21, m.m22, m.m23, m.m31, m.m32, m.m33);
            float c01 = Determinant3x3(m.m10, m.m12, m.m13, m.m20, m.m22, m.m23, m.m30, m.m32, m.m33);
            float c02 = Determinant3x3(m.m10, m.m11, m.m13, m.m20, m.m21, m.m23, m.m30, m.m31, m.m33);
            float c03 = Determinant3x3(m.m10, m.m11, m.m12, m.m20, m.m21, m.m22, m.m30, m.m31, m.m32);

            float c10 = Determinant3x3(m.m01, m.m02, m.m03, m.m21, m.m22, m.m23, m.m31, m.m32, m.m33);
            float c11 = Determinant3x3(m.m00, m.m02, m.m03, m.m20, m.m22, m.m23, m.m30, m.m32, m.m33);
            float c12 = Determinant3x3(m.m00, m.m01, m.m03, m.m20, m.m21, m.m23, m.m30, m.m31, m.m33);
            float c13 = Determinant3x3(m.m00, m.m01, m.m02, m.m20, m.m21, m.m22, m.m30, m.m31, m.m32);

            float c20 = Determinant3x3(m.m01, m.m02, m.m03, m.m11, m.m12, m.m13, m.m31, m.m32, m.m33);
            float c21 = Determinant3x3(m.m00, m.m02, m.m03, m.m10, m.m12, m.m13, m.m30, m.m32, m.m33);
            float c22 = Determinant3x3(m.m00, m.m01, m.m03, m.m10, m.m11, m.m13, m.m30, m.m31, m.m33);
            float c23 = Determinant3x3(m.m00, m.m01, m.m02, m.m10, m.m11, m.m12, m.m30, m.m31, m.m32);

            float c30 = Determinant3x3(m.m01, m.m02, m.m03, m.m11, m.m12, m.m13, m.m21, m.m22, m.m23);
            float c31 = Determinant3x3(m.m00, m.m02, m.m03, m.m10, m.m12, m.m13, m.m20, m.m22, m.m23);
            float c32 = Determinant3x3(m.m00, m.m01, m.m03, m.m10, m.m11, m.m13, m.m20, m.m21, m.m23);
            float c33 = Determinant3x3(m.m00, m.m01, m.m02, m.m10, m.m11, m.m12, m.m20, m.m21, m.m22);

            Matrix4x4 adjoint = new Matrix4x4(
                c00,     -c01,      c02,       -c03,
               -c10,      c11,     -c12,        c13,
                c20,     -c21,      c22,       -c23,
               -c30,      c31,     -c32,        c33
            );

            Matrix4x4 adjointT = Transpose(adjoint);

            float detM = m.determinant;

            return adjointT * (1 / detM);
        }

        public static float Determinant4x4(
            float m00, float m01, float m02, float m03,
            float m10, float m11, float m12, float m13,
            float m20, float m21, float m22, float m23,
            float m30, float m31, float m32, float m33)
        {
            float c00 = m11 * (m22 * m33 - m32 * m23) - m12 * (m21 * m33 - m31 * m23) + m13 * (m21 * m32 - m31 * m22);
            float c01 = m10 * (m22 * m33 - m32 * m23) - m12 * (m20 * m33 - m30 * m23) + m13 * (m20 * m32 - m30 * m22);
            float c02 = m10 * (m21 * m33 - m31 * m23) - m11 * (m20 * m33 - m30 * m23) + m13 * (m20 * m31 - m30 * m21);
            float c03 = m10 * (m21 * m32 - m31 * m22) - m11 * (m20 * m32 - m30 * m22) + m12 * (m20 * m31 - m30 * m21);
            return m00 * c00 - m01 * c01 + m02 * c02 - m03 * c03;
        }

        public static float Determinant3x3(
            float m00, float m01, float m02,
            float m10, float m11, float m12,
            float m20, float m21, float m22)
        {
            return m00 * (m11 * m22 - m12 * m21) - m01 * (m10 * m22 - m12 * m20) + m02 * (m10 * m21 - m11 * m20);
        }

        public override string ToString()
        {
            return string.Format("{0:F5}\t{1:F5}\t{2:F5}\t{3:F5}\n{4:F5}\t{5:F5}\t{6:F5}\t{7:F5}\n{8:F5}\t{9:F5}\t{10:F5}\t{11:F5}\n{12:F5}\t{13:F5}\t{14:F5}\t{15:F5}\n", 
                this.m00, this.m01, this.m02, this.m03, this.m10, this.m11, this.m12, this.m13, this.m20, this.m21, this.m22, this.m23, this.m30, this.m31, this.m32, this.m33);
        }

        public static Matrix4x4 operator *(Matrix4x4 lhs, Matrix4x4 rhs)
        {
            Matrix4x4 result = default(Matrix4x4);
            result.m00 = lhs.m00 * rhs.m00 + lhs.m01 * rhs.m10 + lhs.m02 * rhs.m20 + lhs.m03 * rhs.m30;
            result.m01 = lhs.m00 * rhs.m01 + lhs.m01 * rhs.m11 + lhs.m02 * rhs.m21 + lhs.m03 * rhs.m31;
            result.m02 = lhs.m00 * rhs.m02 + lhs.m01 * rhs.m12 + lhs.m02 * rhs.m22 + lhs.m03 * rhs.m32;
            result.m03 = lhs.m00 * rhs.m03 + lhs.m01 * rhs.m13 + lhs.m02 * rhs.m23 + lhs.m03 * rhs.m33;
            result.m10 = lhs.m10 * rhs.m00 + lhs.m11 * rhs.m10 + lhs.m12 * rhs.m20 + lhs.m13 * rhs.m30;
            result.m11 = lhs.m10 * rhs.m01 + lhs.m11 * rhs.m11 + lhs.m12 * rhs.m21 + lhs.m13 * rhs.m31;
            result.m12 = lhs.m10 * rhs.m02 + lhs.m11 * rhs.m12 + lhs.m12 * rhs.m22 + lhs.m13 * rhs.m32;
            result.m13 = lhs.m10 * rhs.m03 + lhs.m11 * rhs.m13 + lhs.m12 * rhs.m23 + lhs.m13 * rhs.m33;
            result.m20 = lhs.m20 * rhs.m00 + lhs.m21 * rhs.m10 + lhs.m22 * rhs.m20 + lhs.m23 * rhs.m30;
            result.m21 = lhs.m20 * rhs.m01 + lhs.m21 * rhs.m11 + lhs.m22 * rhs.m21 + lhs.m23 * rhs.m31;
            result.m22 = lhs.m20 * rhs.m02 + lhs.m21 * rhs.m12 + lhs.m22 * rhs.m22 + lhs.m23 * rhs.m32;
            result.m23 = lhs.m20 * rhs.m03 + lhs.m21 * rhs.m13 + lhs.m22 * rhs.m23 + lhs.m23 * rhs.m33;
            result.m30 = lhs.m30 * rhs.m00 + lhs.m31 * rhs.m10 + lhs.m32 * rhs.m20 + lhs.m33 * rhs.m30;
            result.m31 = lhs.m30 * rhs.m01 + lhs.m31 * rhs.m11 + lhs.m32 * rhs.m21 + lhs.m33 * rhs.m31;
            result.m32 = lhs.m30 * rhs.m02 + lhs.m31 * rhs.m12 + lhs.m32 * rhs.m22 + lhs.m33 * rhs.m32;
            result.m33 = lhs.m30 * rhs.m03 + lhs.m31 * rhs.m13 + lhs.m32 * rhs.m23 + lhs.m33 * rhs.m33;
            return result;
        }

        public static Vector4 operator *(Matrix4x4 lhs, Vector4 v)
        {
            return new Vector4(
                lhs.m00 * v.x + lhs.m01 * v.y + lhs.m02 * v.z + lhs.m03 * v.w,
                lhs.m10 * v.x + lhs.m11 * v.y + lhs.m12 * v.z + lhs.m13 * v.w,
                lhs.m20 * v.x + lhs.m21 * v.y + lhs.m22 * v.z + lhs.m23 * v.w,
                lhs.m30 * v.x + lhs.m31 * v.y + lhs.m32 * v.z + lhs.m33 * v.w
            );
        }

        public static Vector4 operator *(Matrix4x4 lhs, Vector3 v)
        {
            // default w equals 1
            return new Vector4(
                lhs.m00 * v.x + lhs.m01 * v.y + lhs.m02 * v.z + lhs.m03,
                lhs.m10 * v.x + lhs.m11 * v.y + lhs.m12 * v.z + lhs.m13,
                lhs.m20 * v.x + lhs.m21 * v.y + lhs.m22 * v.z + lhs.m23,
                lhs.m30 * v.x + lhs.m31 * v.y + lhs.m32 * v.z + lhs.m33
            );
        }

        public static Matrix4x4 operator *(Matrix4x4 lhs, float rhs)
        {
            return new Matrix4x4(
                lhs.m00 * rhs, lhs.m01 * rhs, lhs.m02 * rhs, lhs.m03 * rhs,
                lhs.m10 * rhs, lhs.m11 * rhs, lhs.m12 * rhs, lhs.m13 * rhs,
                lhs.m20 * rhs, lhs.m21 * rhs, lhs.m22 * rhs, lhs.m23 * rhs,
                lhs.m30 * rhs, lhs.m31 * rhs, lhs.m32 * rhs, lhs.m33 * rhs
            );
        }

        public static Vector3 Mul3x3(Matrix4x4 lhs, Vector3 v)
        {
            return new Vector3(
                lhs.m00 * v.x + lhs.m01 * v.y + lhs.m02 * v.z,
                lhs.m10 * v.x + lhs.m11 * v.y + lhs.m12 * v.z,
                lhs.m20 * v.x + lhs.m21 * v.y + lhs.m22 * v.z
            );
        }
    }
}
