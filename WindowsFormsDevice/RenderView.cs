using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WPRenderer;

namespace WindowsFormsDevice
{
    public partial class RenderView : Form
    {
        Timer timer;

        IDevice device;

        const int DeviceWidth = 800;
        const int DeviceHeight = 480;
        Camera camera = new Camera(new Vector3(0, 0, 0), new Vector3(0, 0, 1), Vector3.up, (float)DeviceWidth / DeviceHeight);
        Light light = new Light(new Vector3(1, 0, 0), Color.white, 1f);
        Mesh meshCube = Mesh.CreateCube();
        Mesh meshPanel = Mesh.CreatePanel();
        Material material = new MaterialSpecular(LoadTexture("szz.jpg").SetWrapMode(true).SetLerpMode(true), Color.white, new Vector4(4, 4, 0f, 0))
            .SetSpecColor(Color.yellow).SetSpecular(2.5f).SetGloss(1.5f);
        Texture texGrid = LoadTexture("grid.jpg");
        Texture texNormal = LoadTexture("normal.jpg");
        Texture texPng = LoadTexture("timg.png");

        public RenderView()
        {
            InitializeComponent();

            timer = new Timer();
            timer.Interval = 10;
            timer.Tick += Tick;
            timer.Start();

            device = new WindowsFormsDevice(this);

            GpuProgram.Initialize(device, DeviceWidth, DeviceHeight);

            CenterToScreen();

            RefreshMaterialNameLabel();
        }

        void Tick(object sender, EventArgs e)
        {
            Render();
            FPS.Draw(device);
            device.WaitForPresent();
        }

        void Render()
        {
            GpuProgram.Clear(device, new Color(49 / 255.0f, 77 / 255.0f, 121 / 255.0f));
            GpuProgram.SetDirectionLight(light);
            GpuProgram.SetCamera(camera);

            //NormalizedDevice.DrawLine(device, new Vertex(new Vector3(0, 0), Color.red, new Vector2(0, 0)),
            //    new Vertex(new Vector3(0.5f, 0.5f), Color.green, new Vector2(1, 1)));

            //NormalizedDevice.DrawTriangle(device, new Vertex(new Vector3(0, 0.25f), Color.red, new Vector2(0, 0)),
            //    new Vertex(new Vector3(-0.25f, -0.25f), Color.green, new Vector2(0, 0)),
            //    new Vertex(new Vector3(0.25f, -0.45f), Color.yellow, new Vector2(0, 0)));

            Matrix4x4 M, V, P;
            float x, y, z;
            float pitch, yaw, roll;
            float curTime = TimeHelper.GetTimeSinceStartup();

            V = camera.WorldToCameraMatrix();
            P = camera.ProjectionMatrix();

            x = -1f;
            y = Mathf.Repeat(curTime * 1, 6) - 3f;
            z = 4.5f;
            pitch = -90;
            yaw = curTime * 45;
            roll = 0;
            M = Matrix4x4.TRS(new Vector3(x, y, z), Quaternion.Euler(pitch, yaw, roll), Vector3.one);
            GpuProgram.DrawCall(device, meshPanel, material, M, V, P);

            x = 0;
            y = 0;
            z = 2.5f;
            pitch = -60 + curTime * 25;
            yaw = curTime * 25;
            roll = 0;
            M = Matrix4x4.TRS(new Vector3(x, y, z), Quaternion.Euler(pitch, yaw, roll), Vector3.one);
            GpuProgram.DrawCall(device, meshCube, material, M, V, P);
        }

        static Texture LoadTexture(string path)
        {
            System.IO.Stream stream = System.IO.File.OpenRead(path);
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(stream);
            Texture texture = new Texture(bitmap.Width, bitmap.Height);
            for (int i = 0; i < texture.width; i++)
                for (int j = 0; j < texture.height; j++)
                {
                    System.Drawing.Color c = bitmap.GetPixel(i, j);
                    texture.datas[i, j] = new Color(c.R / 255f, c.G / 255f, c.B / 255f, c.A / 255f);
                }
            bitmap.Dispose();
            stream.Dispose();
            return texture;
        }

        //====================================================================================

        Material[] materials;
        int changeMaterialIndex = 0;
        private void InitializeMaterials()
        {
            if (materials == null)
            {
                materials = new Material[]
                {
                    material.SetName("Specular"),
                    new Material(material.mainTexture, Color.green).SetName("Unlit(Green)"),
                    new MaterialDiffuse(material.mainTexture, Color.yellow, new Vector4(1, 1, 0, 0)).SetName("Diffuse(Yellow)"),
                    new MaterialTiling(material.mainTexture, Color.white, new Vector4(8, 8, 0, 0)).SetName("Tiling"),
                    new MaterialDiffuse(texGrid, Color.white, new Vector4(1, 1, 0, 0)).SetName("Diffuse(Grid)"),
                    new MaterialEffect(material.mainTexture, Color.white).SetName("Effect"),
                    new MaterialGray(material.mainTexture, Color.white).SetName("Gray Output"),
                    new MaterialNormal(material.mainTexture, Color.white).SetName("Normal Output"),
                    new MaterialBumpSelf(material.mainTexture, Color.gray).SetName("Bump self"),
                    new Material(texNormal, Color.white).SetName("Normal"),
                    new MaterialBump(material.mainTexture, Color.gray, texNormal).SetName("Bump"),
                    new Material(material.mainTexture, Color.white).SetZTestEnable(false).SetName("ZTest Off"),
                    new Material(material.mainTexture, Color.white).SetZWriteEnable(false).SetName("ZWrite Off"),
                    new Material(texPng, Color.white).SetBlendEnable(true).SetZWriteEnable(false).SetName("Transparent"),
                    new Material(texPng, Color.white).SetBlendEnable(true).SetZWriteEnable(false)
                        .SetBlendFactors(BlendMode.SrcAlpha, BlendMode.One).SetName("Blend(Additive)"),
                };
            }
        }

        private Material GetMaterial(int index)
        {
            InitializeMaterials();

            if (index >= 0 && index < materials.Length)
                return materials[index];
            return material;
        }

        private void ChangeMaterial_Click(object sender, EventArgs e)
        {
            InitializeMaterials();

            changeMaterialIndex++;
            if (changeMaterialIndex >= materials.Length)
                changeMaterialIndex = 0;
            material = materials[changeMaterialIndex];

            RefreshMaterialNameLabel();
        }

        private void RefreshMaterialNameLabel()
        {
            InitializeMaterials();

            MaterialName.Text = material.name;
        }
    }
}
