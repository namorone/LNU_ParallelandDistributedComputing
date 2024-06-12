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
    public partial class Thread2 : Form
    {
        
        public Thread2()
        {
            InitializeComponent();
        }

        public Thread thread2;
        Graphics g;
        Graphics fG;
        Bitmap btm;

        bool drawing = true;

        private void Thread2_Load(object sender, EventArgs e)
        {
            Resume2.Enabled = false;
            btm = new Bitmap(200, 200);
            g = Graphics.FromImage(btm);
            fG = CreateGraphics();
            thread2 = new Thread(Draw);
            thread2.IsBackground = true;
            thread2.Start();
        }

        public void Draw()
        {
            Rectangle area = new Rectangle(25, 50, 150, 100);

            Pen pen = new Pen(Brushes.Black, 3);
            PointF img = new PointF(35, 8);

            fG.Clear(Color.White);                      
            

            while (drawing)
            {
                while(area.Width <= 150 && area.Width > 70)
                {
                    g.Clear(Color.LightBlue); ;
                    g.DrawRectangle(pen, area);
                    g.FillRectangle(new SolidBrush(Color.Black), area);
                    fG.DrawImage(btm, img);

                    area.Width -= 2;
                    area.Height -= 2;
                    area.X += 1;
                    area.Y += 1;
                    Thread.Sleep(10);
                }

                while(area.Width < 150 && area.Width >= 70)
                {
                    g.Clear(Color.Orange);
                    g.DrawRectangle(pen, area);
                    g.FillRectangle(new SolidBrush(Color.Black), area);
                    fG.DrawImage(btm, img);

                    area.Width += 2;
                    area.Height += 2;
                    area.X -= 1;
                    area.Y -= 1;
                    Thread.Sleep(10);

                }
            }
        }

        private void Pause2_Click(object sender, EventArgs e)
        {
            thread2.Suspend();
            Pause2.Enabled = false;
            Resume2.Enabled = true;
        }

        private void Resume2_Click(object sender, EventArgs e)
        {
            thread2.Resume();
            Pause2.Enabled = true;
            Resume2.Enabled = false;
        }

        private void Thread2_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                thread2.Abort();
            }
            catch { }
        }
    }
}









    