using OpenTK;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

using OpenTK.Graphics.OpenGL;

namespace Chernov_tomogram_visualizer
{
    public partial class Form1 : Form
    {
        Bin bin = new Bin();
        bool loaded = false;
        View view = new View();
        int currentLayer;
        bool needReload = false;
        int check_observer_1 = 0;
        int check_observer_2 = 0;
        int minValue = 0;
        int widthValue = 255;

        int FrameCount;
        DateTime NextFPSUpdate = DateTime.Now.AddSeconds(1);

        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Application.Idle += Application_Idle;
        }

        void displayFPS()
        {
            if (DateTime.Now >= NextFPSUpdate)
            {
                this.Text = String.Format("CT Visualizer (fps={0})", FrameCount);
                NextFPSUpdate = DateTime.Now.AddSeconds(1);
                FrameCount = 0;
            }
            FrameCount++;
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string str = dialog.FileName;
                bin.readBIN(str);
                trackBar1.Maximum = Bin.Z - 1;
                view.SetupView(glControl1.Width, glControl1.Height);
                loaded = true;
                glControl1.Invalidate();
            }
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (checkBox1.Checked)
            {
                if (loaded)
                {
                    view.DrawQuadsStrip(currentLayer, minValue, widthValue);
                    glControl1.SwapBuffers();
                }
            }

            else if (checkBox2.Checked)
            {
                if (loaded)
                {
                    if (needReload)
                    {
                        view.generateTextureImage(currentLayer, minValue, widthValue);
                        view.Load2DTexture();
                        needReload = false;
                    }
                    view.DrawQuadsStrip(currentLayer, minValue, widthValue);
                    glControl1.SwapBuffers();
                }
            }
            else
            {
                if (loaded)
                {
                    MessageBox.Show("Пожалуйста, выберите тип текстуры.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    loaded = false;
                }
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            currentLayer = trackBar1.Value;
            needReload = true;
        }

        void Application_Idle(object sender, EventArgs e)
        {
            while (glControl1.IsIdle)
            {
                displayFPS();
                glControl1.Invalidate();
            }
        }
        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                check_observer_2 = 0;
                checkBox2.Checked = false;
                check_observer_1 = 1;
            }
            else
            {
                if (check_observer_1 == 1)
                {
                    checkBox1.Checked = true;
                }
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                check_observer_1 = 0;
                checkBox1.Checked = false;
                check_observer_2 = 1;
            }
            else
            {
                if (check_observer_2 == 1)
                {
                    checkBox2.Checked = true;
                }
            }
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            minValue = trackBar2.Value;
            if (checkBox2.Checked)
            {
                needReload = true; 
            }
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            widthValue = trackBar3.Value;
            if (checkBox2.Checked)
            {
                needReload = true;
            }
        }
    }
}
