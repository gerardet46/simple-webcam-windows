using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;

namespace webcam
{
    public partial class Form1 : Form
    {
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        { 
            var d = cam.VideoCapabilities;

            if (keyData == Keys.Up)
            {
                this.Width += 5;
                this.Height = (int)(this.Width * (float)d[5].FrameSize.Height / (float)d[5].FrameSize.Width);
                return true;
            }
            if (keyData == Keys.Down)
            {
                this.Width -= 5;
                this.Height = (int)(this.Width * (float)d[5].FrameSize.Height / (float)d[5].FrameSize.Width);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }



        VideoCaptureDevice cam;

        public Form1()
        {
            InitializeComponent();
            var dev = new FilterInfoCollection(FilterCategory.VideoInputDevice)[0];

            cam = new VideoCaptureDevice(dev.MonikerString);
            var d = cam.VideoCapabilities;

            cam.VideoResolution = d[5];

            this.Height = (int)(this.Width * (float)d[5].FrameSize.Height / (float)d[5].FrameSize.Width);

            cam.NewFrame += Cam_NewFrame;
            cam.Start();
        }

        private void Cam_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cam.IsRunning) cam.Stop();
        }

        // per moure-ho
        bool down = false;
        int x, y;
        private void MouseDown(object sender, MouseEventArgs e)
        {
            if (!down)
            {
                down = true;
                x = e.X;
                y = e.Y;
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (down)
            {
                int x1 = e.X;
                int y1 = e.Y;

                this.Left += x1 - x;
                this.Top += y1 - y;
                //x = x1;
                //y = y1;
            }
        }

        private void MouseUp(object sender, MouseEventArgs e)
        {
            down = false;
        }
    }
}
