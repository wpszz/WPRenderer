using System;

namespace WPRenderer
{
    public static class FPS
    {
        static double lastFrameTime = 0;
        static double lastFps = 0;
        static int fpsFrameCount = 0;

        public static void Draw(IDevice device)
        {
            double curFrameTime = TimeHelper.GetSystemTime();
            fpsFrameCount++;
            if (curFrameTime - lastFrameTime >= 1.0f)
            {
                lastFps = fpsFrameCount / (curFrameTime - lastFrameTime);
                lastFrameTime = curFrameTime;
                fpsFrameCount = 0;
            }

            string text = string.Format("FPS:{0:0.00}", lastFps);

            device.DrawText(text, 0, 0, Color.yellow);
        }
    }
}
