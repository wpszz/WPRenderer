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

        protected static float NdcToDepth(float ndcZ)
        {
            return ndcZ * 0.5f + 0.5f;
        }

        // Z buffer to linear 0..1 depth (from near to far, z is NDC-Z)
        protected static float Linear01Depth(float z)
        {
            if (currentCamera.orthographic)
            {
                float cameraZ = OrthographicCameraDepth(z);
                return (-cameraZ - currentProjectionParams.y) / currentProjectionParams.z;
            }
            else
            {
                float n = currentProjectionParams.y;
                float f = currentProjectionParams.z;
                return n * (z + 1.0f) / (f + n - z * (f - n));
            }
        }

        // Z buffer to linear 0..1 depth (from eye to far, z is NDC-Z)
        protected static float Linear01EyeDepth(float z)
        {
            if (currentCamera.orthographic)
            {
                float cameraZ = OrthographicCameraDepth(z);
                return -cameraZ / currentProjectionParams.z;
            }
            else
            {
                float n = currentProjectionParams.y;
                float f = currentProjectionParams.z;
                return (2 * n) / (f + n - z * (f - n));

                //float depth = NdcToDepth(z);
                //return 1.0f / (currentZBufferParams.x * depth + currentZBufferParams.y);
            }
        }

        // Z buffer to linear depth (from eye to far, z is NDC-Z)
        protected static float LinearEyeDepth(float z)
        {
            if (currentCamera.orthographic)
            {
                float cameraZ = OrthographicCameraDepth(z);
                return -cameraZ;
            }
            else
            {
                float n = currentProjectionParams.y;
                float f = currentProjectionParams.z;
                return (2 * n * f) / (f + n - z * (f - n));

                //float depth = NdcToDepth(z);
                //return 1.0f / (currentZBufferParams.z * depth + currentZBufferParams.w);
            }
        }

        // Z buffer to orthographic camera space depth
        // (https://stackoverflow.com/questions/8990735/how-to-use-opengl-orthographic-projection-with-the-depth-buffer)
        protected static float OrthographicCameraDepth(float z)
        {
            float zNear = currentProjectionParams.y;
            float zFar = currentProjectionParams.z;
            return (z * (zFar - zNear) + (zFar + zNear)) * -0.5f;
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

        // Encoding/decoding [0..1) floats into 8 bit/channel RGBA. Note that 1.0 will not be encoded properly.
        protected static Vector4 EncodeFloatRGBA(float v)
        {
            Vector4 kEncodeMul = new Vector4(1.0f, 255.0f, 65025.0f, 16581375.0f);
            float kEncodeBit = 1.0f / 255.0f;
            Vector4 enc = kEncodeMul * v;
            enc.x = Mathf.Frac(enc.x);
            enc.y = Mathf.Frac(enc.y);
            enc.z = Mathf.Frac(enc.z);
            enc.w = Mathf.Frac(enc.w);
            enc -= new Vector4(enc.y, enc.z, enc.w, enc.w) * kEncodeBit;
            return enc;
        }
        protected static float DecodeFloatRGBA(Vector4 enc)
        {
            Vector4 kDecodeDot = new Vector4(1.0f, 1 / 255.0f, 1 / 65025.0f, 1 / 16581375.0f);
            return Vector4.Dot(enc, kDecodeDot);
        }
    }
}
