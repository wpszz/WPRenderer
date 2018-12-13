using System.Collections.Generic;

namespace WPRenderer
{
    public class GpuShare
    {
        protected static Material currentMaterial;
        protected static Matrix4x4 currentMVP;
        protected static Matrix4x4 currentM;
        protected static Matrix4x4 currentV;
        protected static Matrix4x4 currentP;
        protected static Matrix4x4 currentInverseM; // world space to object space
        protected static Vector4 currentTime;
        protected static Vector4 currentSinTime;
        protected static Light currentLight;
        protected static Camera currentCamera;

        // x = 1 or -1 (-1 if projection is flipped)
        // y = near plane
        // z = far plane
        // w = 1/far plane
        protected static Vector4 currentProjectionParams;

        // Values used to linearize the Z buffer (http://www.humus.name/temp/Linearize%20depth.txt)
        // x = 1-far/near
        // y = far/near
        // z = x/far
        // w = y/far
        protected static Vector4 currentZBufferParams;

        // Z buffer to linear 0..1 depth
        protected static float Linear01Depth(float z)
        {
            if (currentCamera.orthographic)
            {
                // https://stackoverflow.com/questions/8990735/how-to-use-opengl-orthographic-projection-with-the-depth-buffer
                float zNear = currentProjectionParams.y;
                float zFar = currentProjectionParams.z;
                float cameraZ = (z * (zFar - zNear) + (zFar + zNear)) * -0.5f;
                return (-cameraZ - zNear) / zFar;
            }
            return 1.0f / (currentZBufferParams.x * z + currentZBufferParams.y);
        }
        // Z buffer to linear depth
        protected static float LinearEyeDepth(float z)
        {
            return 1.0f / (currentZBufferParams.z * z + currentZBufferParams.w);
        }


        // simple discard simulation
        private static bool discardCurrentPixel;
        protected static void Discard()
        {
            discardCurrentPixel = true;
        }
        protected static void ResetDiscard()
        {
            discardCurrentPixel = false;
        }
        protected static bool IsDiscard()
        {
            return discardCurrentPixel;
        }
        protected static void Clip(float value)
        {
            if (value < 0)
                Discard();
        }
    }
}
