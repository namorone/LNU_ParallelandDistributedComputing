using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Forms
{
    public partial class Thread1 : Form
    {
        public Thread1()
        {
            InitializeComponent();
        } 

        public Thread thread1;
        Graphics g;
        Graphics fG;
        Bitmap btm;

        bool drawing = true;        

        private void Thread1_Load(object sender, EventArgs e)
        {
            Resume1.Enabled = false;
            btm = new Bitmap(200, 200);
            g = Graphics.FromImage(btm);
            fG = CreateGraphics();
            thread1 = new Thread(new ThreadStart(Draw));
            thread1.IsBackground = true;
            thread1.Start();
        }

        public void Draw()
        {
            float angle = 0;
            PointF org = new PointF(50, 50);
            float rad = 50;
            Pen pen = new Pen(Brushes.Black, 3);
            RectangleF bigCircle = new RectangleF(50, 50, 100, 100);
            RectangleF smallCircle = new RectangleF(0, 0, 10, 10);

            PointF loc = PointF.Empty;
            PointF img = new PointF(35, 8);

            fG.Clear(Color.White);

            while(drawing)
            {
                g.Clear(Color.Orange);

                g.DrawEllipse(pen, bigCircle);
                loc = CirclePoint(rad, angle, org);

                smallCircle.X = loc.X - (smallCircle.Width / 2) + bigCircle.X;
                smallCircle.Y = loc.Y - (smallCircle.Height / 2) + bigCircle.Y;

                g.DrawEllipse(pen, smallCircle);
                g.FillEllipse(new SolidBrush(Color.Black), smallCircle);


                fG.DrawImage(btm, img);

                if(angle < 360)
                {
                    angle += 0.5f;
                }
                else
                {
                    angle = 0;
                   // Thread.Sleep(1000);
                }
            }

        }

        public PointF CirclePoint (float radius, float angleInDegrees, PointF origin)
        {
            float x = (float)(radius * Math.Cos(angleInDegrees * Math.PI / 180F)) + origin.X;
            float y = (float)(radius * Math.Sin(angleInDegrees * Math.PI / 180F)) + origin.Y;

            return new PointF(x, y);
        }

        private void Pause1_Click(object sender, EventArgs e)
        {
            thread1.Suspend();
            Pause1.Enabled = false;
            Resume1.Enabled = true;
        }

        private void Resume1_Click(object sender, EventArgs e)
        {
            thread1.Resume();
            Pause1.Enabled = true;
            Resume1.Enabled = false;
        }

        private void Thread1_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                thread1.Abort();
            }
            catch { }

        }
    }
}
