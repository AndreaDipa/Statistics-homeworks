using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace MyVeryFirstCSharpProgram
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            chart1.Series["Data"].Points.AddXY("1", 0);
            chart1.Series["Data"].Points[0].Color = Color.Red;
            chart1.Series["Data"].Points.AddXY("2", 0);
            chart1.Series["Data"].Points[1].Color = Color.Blue;
            chart1.Series["Data"].Points.AddXY("3", 0);
            chart1.Series["Data"].Points[2].Color = Color.Green;
            chart1.Series["Data"].Points.AddXY("4", 0);
            chart1.Series["Data"].Points[3].Color = Color.Purple;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "Hello World!";
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            richTextBox1.Text = "The mouse entered button1!";
        }
        private void button1_MouseLeave(object sender, EventArgs e)
        {
            richTextBox1.Text = "The mouse left button1!";

        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (progressBar1.Value < 100)
            {
                progressBar1.Value += 10;
                if (progressBar1.Value == 10)
                    button3.Enabled = true;
                else if (progressBar1.Value == 100)
                    button2.Enabled = false;

            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (progressBar1.Value > 0)
            {
                progressBar1.Value -= 10;
                if (progressBar1.Value == 90)
                    button2.Enabled = true;
                else if (progressBar1.Value == 0)
                    button3.Enabled = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            chart1.Series["Data"].Points[0].YValues[0] += 1;
            chart1.ChartAreas[0].RecalculateAxesScale();
            chart1.Refresh();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            chart1.Series["Data"].Points[0].YValues[0] -= 1;
            chart1.ChartAreas[0].RecalculateAxesScale();
            chart1.Refresh();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            chart1.Series["Data"].Points[1].YValues[0] += 1;
            chart1.ChartAreas[0].RecalculateAxesScale();
            chart1.Refresh();
        }
        private void button7_Click(object sender, EventArgs e)
        {
            chart1.Series["Data"].Points[1].YValues[0] -= 1;
            chart1.ChartAreas[0].RecalculateAxesScale();
            chart1.Refresh();
        }
        private void button8_Click(object sender, EventArgs e)
        {
            chart1.Series["Data"].Points[2].YValues[0] += 1;
            chart1.ChartAreas[0].RecalculateAxesScale();
            chart1.Refresh();
        }
        private void button9_Click(object sender, EventArgs e)
        {
            chart1.Series["Data"].Points[2].YValues[0] -= 1;
            chart1.ChartAreas[0].RecalculateAxesScale();
            chart1.Refresh();
        }
        private void button10_Click(object sender, EventArgs e)
        {
            chart1.Series["Data"].Points[3].YValues[0] -= 1;
            chart1.ChartAreas[0].RecalculateAxesScale();
            chart1.Refresh();
        }
        private void button11_Click(object sender, EventArgs e)
        {
            chart1.Series["Data"].Points[3].YValues[0] += 1;
            chart1.ChartAreas[0].RecalculateAxesScale();
            chart1.Refresh();
        }
    }
}