
namespace WPRenderer
{
    public class Texture
    {
        public int width;
        public int height;
        public Color[,] datas;

        public bool wrapRepeat;

        public Texture(int width, int height, bool wrapRepeat = false)
        {
            this.width = width;
            this.height = height;
            this.datas = new Color[width, height];
            this.wrapRepeat = wrapRepeat;
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
            int i = Mathf.Round(u * (width - 1));
            int j = Mathf.Round(v * (height - 1));
            return datas[i, j];
        }

        public Texture SetWrap(bool repeat)
        {
            this.wrapRepeat = repeat;
            return this;
        }
    }
}
