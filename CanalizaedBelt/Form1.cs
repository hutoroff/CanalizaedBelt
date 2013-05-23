using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CanalizaedBelt
{
    public partial class Form1 : Form
    {
        private Field f;
        private bool rbc;

        public Form1()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            trackBar1.Visible = false;
            trackBar2.Visible = false;
            checkBox1.Visible = true;
            checkBox2.Visible = true;
            PauseButt.Visible = true;
            BasicRadio.Enabled = false;
            MixedRadio.Enabled = false;
            SlowRadio.Enabled = false;
            int test = getRadioButtonPosition();

            f = new Field(pictureBox1.Width, pictureBox1.Height, trackBar1.Value, trackBar2.Value,getRadioButtonPosition());

            pictureBox1.Refresh();
            timer1.Start();
        }

        private int getRadioButtonPosition()
        {
            if (BasicRadio.Checked)
                return 0;
            if (MixedRadio.Checked)
                return 1;
            if (SlowRadio.Checked)
                return 2;
            return -1;
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            f = null;
            trackBar1.Visible = true;
            trackBar2.Visible = true;
            checkBox1.Visible = false;
            checkBox2.Visible = false;
            PauseButt.Visible = false;
            BasicRadio.Enabled = true;
            MixedRadio.Enabled = true;
            SlowRadio.Enabled = true;
            pictureBox1.Refresh();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            trackBar2.Maximum = (pictureBox1.Height / trackBar1.Value)*4;
            trackBar2.Value = trackBar2.Maximum / 2;
            
            particleLabel.Text = Convert.ToString(trackBar2.Value);
            String res = String.Concat(Convert.ToString(pictureBox1.Width / trackBar1.Value), " x ",
                                       Convert.ToString(pictureBox1.Height / trackBar1.Value));
            fieldLabel.Text = res;
            pictureBox1.Refresh();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            particleLabel.Text = Convert.ToString(trackBar2.Value);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
            if (checkBox1.Checked)
            {
                this.Height = 695;
                timer1.Interval = 300;
            }
            else
            {
                //this.Height = 580;
                //timer1.Interval = 50;
            }
            if (rbc)
            {
                f.Move();
                if (checkBox1.Checked)
                {
                    //textCur1.Text = Convert.ToString(f.countC1);
                    //textCur2.Text = Convert.ToString(f.countC2);
                    //textLast1.Text = Convert.ToString(f.countL1);
                    //textLast2.Text = Convert.ToString(f.countL2);
                    //textTotal1.Text = Convert.ToString(f.countT1);
                    //textTotal2.Text = Convert.ToString(f.countT2);
                }
            }
            else
            {
               
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            int grid_step = trackBar1.Value;
            int kX = pictureBox1.Width / grid_step;
            int kY = pictureBox1.Height / grid_step;

            for (int i = 0; i <= kX; ++i)
            {
                e.Graphics.DrawLine(Pens.LightGray, i * grid_step, 0, i * grid_step, pictureBox1.Height);
            }

            for (int j = 0; j <= kY; ++j)
            {
                e.Graphics.DrawLine(Pens.LightGray, 0, j * grid_step, pictureBox1.Width, j * grid_step);
            }

            //отрисовка границ третей
            Brush b = new SolidBrush(Color.Fuchsia);
            Pen p = new Pen(b, 7);
            e.Graphics.DrawLine(p, (kX / 2) * grid_step, 0, (kX / 2) * grid_step, pictureBox1.Height);

            if (f != null) f.Draw(e.Graphics);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (BasicRadio.Checked)
            {
                rbc = true;
                f = null;
                trackBar2.Maximum = (pictureBox1.Height / trackBar1.Value)*4;
                trackBar2.Value = trackBar2.Maximum / 2;
                particleLabel.Text = trackBar2.Value.ToString();
            }
            else
            {
                //rbc = false;
                //f = null;
                //trackBar1.Maximum = 40;
                //trackBar1.Value = 20;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
                timer1.Interval = 10;
            else
                timer1.Interval = 100;
        }
    }
}
