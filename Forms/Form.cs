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
    public partial class Forms : Form
    {
        public Forms()
        {
            InitializeComponent();
        }

        private void StartThread1_Click(object sender, EventArgs e)
        {
            Thread1 thread1 = new Thread1();
            thread1.Show();
        }

        private void StartThread2_Click(object sender, EventArgs e)
        {
            Thread2 thread2 = new Thread2();
            thread2.Show();
        }

        private void StartThread3_Click(object sender, EventArgs e)
        {
            Thread3 thread3 = new Thread3();
            thread3.Show();
        }

        private void StartThread4_Click(object sender, EventArgs e)
        {
            Thread4 thread4 = new Thread4();
            thread4.Show();
        }
    }
}
