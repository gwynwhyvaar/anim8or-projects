using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SkiesOfAzurya.Forms
{
    public partial class Splash : Form
    {
        private Timer SplashTimer;

        public Splash()
        {
            InitializeComponent();
        }

        private void Splash_Load(object sender, EventArgs e)
        {
            SplashTimer = new Timer();
            SplashTimer.Interval = 5000;
            SplashTimer.Enabled = true;
            SplashTimer.Tick += new EventHandler(SplashTimer_Tick);
            SplashTimer.Start();
        }

        void SplashTimer_Tick(object sender, EventArgs e)
        {
            SplashTimer.Stop();
            this.Close();
        }
    }
}
