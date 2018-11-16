namespace WindowsFormsDevice
{
    partial class RenderView
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.ChangeMaterial = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ChangeMaterial
            // 
            this.ChangeMaterial.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ChangeMaterial.Location = new System.Drawing.Point(1177, 634);
            this.ChangeMaterial.Name = "ChangeMaterial";
            this.ChangeMaterial.Size = new System.Drawing.Size(75, 35);
            this.ChangeMaterial.TabIndex = 0;
            this.ChangeMaterial.Text = "change material";
            this.ChangeMaterial.UseVisualStyleBackColor = true;
            this.ChangeMaterial.Click += new System.EventHandler(this.ChangeMaterial_Click);
            // 
            // RenderView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.ChangeMaterial);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "RenderView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RenderView";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ChangeMaterial;
    }
}

