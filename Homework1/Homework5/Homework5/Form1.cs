using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Homework5
{
    public partial class Form1 : Form
    {
        Bitmap b;
        Graphics g;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(b);

            Rectangle r = new Rectangle(20, 20, 100, 100);
            Pen blackPen = new Pen(Color.Black, 3);

            g.DrawRectangle(blackPen, r);
            pictureBox1.Image = b;
        }
    }
}
