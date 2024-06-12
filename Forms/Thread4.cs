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
    public partial class Thread4 : Form
    {
        public Thread4()
        {
            InitializeComponent();
        }

        public Thread thread4;
        Graphics g;
        Graphics fG;
        Bitmap btm;

        Random rnd = new Random();


        bool drawing = true;

        private void Thread4_Load(object sender, EventArgs e)
        {
            Resume4.Enabled = false;
            btm = new Bitmap(200, 200);
            g = Graphics.FromImage(btm);
            fG = CreateGraphics();
            thread4 = new Thread(Draw);
            thread4.IsBackground = true;
            thread4.Start();
        }

        public void Draw()
        {
            Rectangle circle = new Rectangle(50, 50, 100, 100);
            PointF img = new PointF(35, 8);

            fG.Clear(Color.White);

            while (drawing)
            {
                g.Clear(Color.Orange);

                Color randomColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));

                Pen pen = new Pen(randomColor, 3);

                g.DrawEllipse(pen, circle);
                g.FillEllipse(new SolidBrush(randomColor), circle);

                fG.DrawImage(btm, img);
                Thread.Sleep(700);
            }
        }


        private void Pause4_Click(object sender, EventArgs e)
        {
            thread4.Suspend();
            Pause4.Enabled = false;
            Resume4.Enabled = true;
        }

        private void Resume4_Click(object sender, EventArgs e)
        {
            thread4.Resume();
            Pause4.Enabled = true;
            Resume4.Enabled = false;
        }       

        private void Thread4_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                thread4.Abort();
            }
            catch { }
        }
    }
}
