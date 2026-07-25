namespace SkiesOfAzurya.Forms
{
    partial class Splash
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Splash));
            this.SplashImageBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.SplashImageBox)).BeginInit();
            this.SuspendLayout();
            // 
            // SplashImageBox
            // 
            this.SplashImageBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplashImageBox.Image = ((System.Drawing.Image)(resources.GetObject("SplashImageBox.Image")));
            this.SplashImageBox.Location = new System.Drawing.Point(0, 0);
            this.SplashImageBox.Name = "SplashImageBox";
            this.SplashImageBox.Size = new System.Drawing.Size(442, 307);
            this.SplashImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.SplashImageBox.TabIndex = 0;
            this.SplashImageBox.TabStop = false;
            // 
            // Splash
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(442, 307);
            this.Controls.Add(this.SplashImageBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Splash";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Splash";
            this.Load += new System.EventHandler(this.Splash_Load);
            ((System.ComponentModel.ISupportInitialize)(this.SplashImageBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox SplashImageBox;
    }
}