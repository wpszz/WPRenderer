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
            this.MaterialName = new System.Windows.Forms.Label();
            this.yawBar = new System.Windows.Forms.TrackBar();
            this.pitchBar = new System.Windows.Forms.TrackBar();
            this.manualRotation = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.yawBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pitchBar)).BeginInit();
            this.SuspendLayout();
            // 
            // ChangeMaterial
            // 
            this.ChangeMaterial.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ChangeMaterial.Location = new System.Drawing.Point(710, 407);
            this.ChangeMaterial.Name = "ChangeMaterial";
            this.ChangeMaterial.Size = new System.Drawing.Size(75, 35);
            this.ChangeMaterial.TabIndex = 0;
            this.ChangeMaterial.Text = "change material";
            this.ChangeMaterial.UseVisualStyleBackColor = true;
            this.ChangeMaterial.Click += new System.EventHandler(this.ChangeMaterial_Click);
            // 
            // MaterialName
            // 
            this.MaterialName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.MaterialName.AutoSize = true;
            this.MaterialName.Location = new System.Drawing.Point(623, 427);
            this.MaterialName.Name = "MaterialName";
            this.MaterialName.Size = new System.Drawing.Size(47, 12);
            this.MaterialName.TabIndex = 1;
            this.MaterialName.Text = "unnamed";
            this.MaterialName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // yawBar
            // 
            this.yawBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.yawBar.AutoSize = false;
            this.yawBar.Location = new System.Drawing.Point(0, 417);
            this.yawBar.Maximum = 180;
            this.yawBar.Minimum = -180;
            this.yawBar.Name = "yawBar";
            this.yawBar.Size = new System.Drawing.Size(250, 24);
            this.yawBar.TabIndex = 2;
            this.yawBar.TickStyle = System.Windows.Forms.TickStyle.None;
            // 
            // pitchBar
            // 
            this.pitchBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pitchBar.AutoSize = false;
            this.pitchBar.Location = new System.Drawing.Point(0, 169);
            this.pitchBar.Maximum = 180;
            this.pitchBar.Minimum = -180;
            this.pitchBar.Name = "pitchBar";
            this.pitchBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.pitchBar.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.pitchBar.Size = new System.Drawing.Size(24, 250);
            this.pitchBar.TabIndex = 3;
            this.pitchBar.TickStyle = System.Windows.Forms.TickStyle.None;
            // 
            // manualRotation
            // 
            this.manualRotation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.manualRotation.AutoSize = true;
            this.manualRotation.Location = new System.Drawing.Point(250, 425);
            this.manualRotation.Name = "manualRotation";
            this.manualRotation.Size = new System.Drawing.Size(114, 16);
            this.manualRotation.TabIndex = 4;
            this.manualRotation.Text = "manual rotation";
            this.manualRotation.UseVisualStyleBackColor = true;
            // 
            // RenderView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 441);
            this.Controls.Add(this.manualRotation);
            this.Controls.Add(this.pitchBar);
            this.Controls.Add(this.yawBar);
            this.Controls.Add(this.MaterialName);
            this.Controls.Add(this.ChangeMaterial);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "RenderView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RenderView";
            ((System.ComponentModel.ISupportInitialize)(this.yawBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pitchBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ChangeMaterial;
        private System.Windows.Forms.Label MaterialName;
        private System.Windows.Forms.TrackBar yawBar;
        private System.Windows.Forms.TrackBar pitchBar;
        private System.Windows.Forms.CheckBox manualRotation;
    }
}

