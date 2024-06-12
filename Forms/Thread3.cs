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
    public partial class Thread3 : Form
    {
        public Thread3()
        {
            InitializeComponent();
        }

        public Thread thread3;
        Graphics g;
        Graphics fG;
        Bitmap btm;

        bool drawing = true;

        private void Thread3_Load(object sender, EventArgs e)
        {
            Resume3.Enabled = false;
            btm = new Bitmap(275, 200);
            g = Graphics.FromImage(btm);
            fG = CreateGraphics();
            thread3 = new Thread(Draw);
            thread3.IsBackground = true;
            thread3.Start();
        }

        public void Draw()
        {
            Rectangle area = new Rectangle(25, 50, 150, 100);

            Pen pen = new Pen(Brushes.Black, 3);
            PointF img = new PointF(0, 8);

            fG.Clear(Color.White);

            float x1 = 0;
            float y1 = 0;

            float y2 = 0;

            float yEx = 100;
            float eF = 10;

            float x = 0;
            g.Clear(Color.Orange);

            int count = 0;

            while (drawing)
            {
                y2 = (float)Math.Sin(x);
                fG.DrawImage(btm, img);


                g.DrawLine(pen, x1 * eF, y1 * eF + yEx, x * eF, y2 * eF + yEx);

                x1 = x;
                y1 = y2;

                x += 0.2f;
                count++;

                if (x * eF >= 275)
                {
                    x = x1 = y1 = y2 = count = 0;
                    g.Clear(Color.Orange);
                }
                Thread.Sleep(10);
            }
        }

        private void Pause3_Click(object sender, EventArgs e)
        {
            thread3.Suspend();
            Pause3.Enabled = false;
            Resume3.Enabled = true;
        }

        private void Resume3_Click(object sender, EventArgs e)
        {
            thread3.Resume();
            Pause3.Enabled = true;
            Resume3.Enabled = false;
        }

        private void Thread3_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                thread3.Abort();
            }
            catch { }
        }

        
    }
}
