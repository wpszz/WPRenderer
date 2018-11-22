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
    }
}
