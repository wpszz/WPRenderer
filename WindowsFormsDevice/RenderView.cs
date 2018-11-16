﻿using System;
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
        Mesh mesh = Mesh.CreateCube();
        Material material = new MaterialSpecular(LoadTexture("szz.jpg").SetWrap(true), Color.white, new Vector4(4, 4, 0f, 0))
            .SetSpecColor(Color.yellow).SetSpecular(2.5f).SetGloss(1.5f);

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

            float pitch = -60 + TimeHelper.GetTimeSinceStartup() * 25;
            float yaw = TimeHelper.GetTimeSinceStartup() * 25;
            float roll = 0;
            Matrix4x4 M = Matrix4x4.TRS(new Vector3(0, 0, 2.5f), Quaternion.Euler(pitch, yaw, roll), Vector3.one);
            Matrix4x4 V = camera.WorldToCameraMatrix();
            Matrix4x4 P = camera.ProjectionMatrix();
            GpuProgram.DrawCall(device, mesh, material, M, V, P);
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

        private void ChangeMaterial_Click(object sender, EventArgs e)
        {
            if (materials == null)
            {
                materials = new Material[]
                {
                    material,
                    new MaterialDiffuse(material.mainTexture, Color.yellow, new Vector4(1, 1, 0, 0)),
                    new MaterialTiling(material.mainTexture, Color.white, new Vector4(8, 8, 0, 0)),
                    new Material(material.mainTexture, Color.green),
                    new MaterialDiffuse(LoadTexture("grid.jpg"), Color.white, new Vector4(1, 1, 0, 0)),
                    new MaterialEffect(material.mainTexture, Color.white)
                };
            }
            changeMaterialIndex++;
            if (changeMaterialIndex >= materials.Length)
                changeMaterialIndex = 0;
            material = materials[changeMaterialIndex];
        }
    }
}
