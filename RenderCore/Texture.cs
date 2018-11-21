
namespace WPRenderer
{
    public class Texture
    {
        public int width;
        public int height;
        public Color[,] datas;

        // clamp or repeat
        public bool wrapRepeat;
        // point or bilinear
        public bool lerpLinear;

        public Texture(int width, int height, bool wrapRepeat = false, bool lerpLinear = false)
        {
            this.width = width;
            this.height = height;
            this.datas = new Color[width, height];
            this.wrapRepeat = wrapRepeat;
            this.lerpLinear = lerpLinear;
        }

        public Color Sample(float u, float v)
        {
            if (wrapRepeat)
            {
                u = Mathf.Repeat(u, 1f);
                v = Mathf.Repeat(v, 1f);
            }
            else
            {
                u = Mathf.Clamp01(u);
                v = Mathf.Clamp01(v);
            }

            if (lerpLinear)
            {
                float i = u * (width - 1);
                float j = v * (height - 1);
                int iA = Mathf.FloorToInt(i);
                int jA = Mathf.FloorToInt(j);
                int iB = Mathf.CeilToInt(i);
                int jB = Mathf.CeilToInt(j);

                Color leftTop = datas[iA, jA];
                Color rightTop = datas[iB, jA];
                Color leftBottom = datas[iA, jB];
                Color rightBottom = datas[iB, jB];

                Color top = Color.Lerp(leftTop, rightTop, i - iA);
                Color bottom = Color.Lerp(leftBottom, rightBottom, i - iA);

                return Color.Lerp(top, bottom, j - jA);
            }
            else
            {
                int i = Mathf.Round(u * (width - 1));
                int j = Mathf.Round(v * (height - 1));
                return datas[i, j];
            }
        }

        public Texture SetWrapMode(bool repeat)
        {
            this.wrapRepeat = repeat;
            return this;
        }

        public Texture SetLerpMode(bool linear)
        {
            this.lerpLinear = linear;
            return this;
        }
    }
}
