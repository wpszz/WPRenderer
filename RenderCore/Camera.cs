
namespace WPRenderer
{
    public class Camera
    {
        public Vector3 pos;
        public Vector3 lookAt;
        public Vector3 up;

        public float aspect;
        public float fieldOfView;
        public float nearClipPlane;
        public float farClipPlane;

        public Camera(Vector3 pos, Vector3 lookAt, Vector3 up, float aspect, float fieldOfView = 60f, float nearClipPlane = 0.3f, float farClipPlane = 1000f)
        {
            this.pos = pos;
            this.lookAt = lookAt;
            this.up = up;
            this.aspect = aspect;
            this.fieldOfView = fieldOfView;
            this.nearClipPlane = nearClipPlane;
            this.farClipPlane = farClipPlane;
        }

        // just compare with CameraToWorldMatrix(like Unity Transform.LocalToWorldMatrix)
        public Matrix4x4 LocalToWorldMatrix()
        {
            // TR = {0000, 0000, 0000, xyz1} * {right, up, forward, 0001}
            Vector3 forward = (lookAt - pos).normalized;    // positive Z-Axis in the world
            Vector3 right = Vector3.Cross(this.up, forward).normalized;
            Vector3 up = Vector3.Cross(forward, right);
            return new Matrix4x4(
                right.x, up.x, forward.x, pos.x,
                right.y, up.y, forward.y, pos.y,
                right.z, up.z, forward.z, pos.z,
                0, 0, 0, 1
            );
        }

        // just compare with WorldToCameraMatrix(like Unity Transform.WorldToCameraMatrix)
        public Matrix4x4 WorldToLocalMatrix()
        {
            // (TR)^-1 = (R^-1) * (T^-1)
            Vector3 forward = (lookAt - pos).normalized; // positive Z-Axis in the world
            Vector3 right = Vector3.Cross(this.up, forward).normalized;
            Vector3 up = Vector3.Cross(forward, right);
            return new Matrix4x4(
                right.x,    right.y,    right.z,    -Vector3.Dot(right, pos),
                up.x,       up.y,       up.z,       -Vector3.Dot(up, pos),
                forward.x,  forward.y,  forward.z,  -Vector3.Dot(forward, pos),
                0,          0,          0,          1
            );
        }

        public Matrix4x4 CameraToWorldMatrix()
        {
            // TR = {0000, 0000, 0000, xyz1} * {right, up, forward, 0001}
            Vector3 forward = (pos - lookAt).normalized;    // negative Z-Axis in the world
            Vector3 right = Vector3.Cross(forward, this.up).normalized;
            Vector3 up = Vector3.Cross(right, forward);
            return new Matrix4x4(
                right.x, up.x, forward.x, pos.x,
                right.y, up.y, forward.y, pos.y,
                right.z, up.z, forward.z, pos.z,
                0, 0, 0, 1
            );
        }

        public Matrix4x4 WorldToCameraMatrix()
        {
            // (TR)^-1 = (R^-1) * (T^-1)
            Vector3 forward = (pos - lookAt).normalized;    // negative Z-Axis in the world
            Vector3 right = Vector3.Cross(forward, this.up).normalized;
            Vector3 up = Vector3.Cross(right, forward);
            return new Matrix4x4(
                right.x,    right.y,    right.z,    -Vector3.Dot(right, pos),
                up.x,       up.y,       up.z,       -Vector3.Dot(up, pos),
                forward.x,  forward.y,  forward.z,  -Vector3.Dot(forward, pos),
                0,          0,          0,          1
            );
        }

        //http://www.songho.ca/opengl/gl_projectionmatrix.html
        public Matrix4x4 ProjectionMatrix()
        {
            float tanHalfFov = Mathf.Tan(Mathf.Deg2Rad * fieldOfView * 0.5f);
            float num1 = 1 / (aspect * tanHalfFov);
            float num2 = 1 / tanHalfFov;
            float num3 = -(farClipPlane + nearClipPlane) / (farClipPlane - nearClipPlane);
            float num4 = -(2 * farClipPlane * nearClipPlane) / (farClipPlane - nearClipPlane);
            return new Matrix4x4(
                num1,   0,      0,      0,
                0,      num2,   0,      0,
                0,      0,      num3,   num4,
                0,      0,      -1,     0
            );
        }
    }
}
