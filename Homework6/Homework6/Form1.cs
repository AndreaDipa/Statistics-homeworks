using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using System.Net.Sockets;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Homework6
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Bitmap b;
        Graphics g;

        int x_down;
        int y_down;

        int x_mouse;
        int y_mouse;

        int r_width;
        int r_height;

        Rectangle r;

        bool drag = false;
        bool resizing = false;
        double[] pop = new double[447];
        double pop_mean = 0;
        double pop_var = 0;
        double[][] samples;
        Random rand = new Random();
        double[] means;
        double[] variances;
        double variance_of_variances;
        double mean_of_means;

        private void button1_Click(object sender, EventArgs e)
        {
            b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(b);

            r = new Rectangle(20, 20, 500, 300);

            g.DrawRectangle(Pens.Black, r);
            g.FillRectangle(Brushes.Black, r);
            pictureBox1.Image = b;
        }
        private double knuth_mean(double[] values)
        {
            int count = 1;
            double mean = 0;
            int size = values.Length;
            for (int i = 0; i < size; i++)
            {
                mean = mean + ((values[i] - mean) / (double)count);
                count++;
            }
            return mean;
        }
        public double welford_variance(double[] values)
        {
            double mean = 0;
            double oldM;
            double variance = 0;
            int count = 1;
            int size = values.Length;

            for (int j = 0; j < size; j++)
            {
                double val = values[j];
                oldM = mean;
                mean = mean + (val - mean) / count;
                variance = variance + (val - mean) * (val - oldM);
                count++;
            }

            variance = variance / (values.Length);

            return (variance);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            var filePath = String.Empty;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog1.FileName;
            }
            foreach (var headerLine in File.ReadLines(filePath).Take(1))
            {
                foreach (var headerItem in headerLine.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var column = new DataGridViewTextBoxColumn();
                    column.Name = headerItem;
                    column.HeaderText = headerItem;
                }
            }

            //population mean and variance
            var i = 0;
            foreach (var line in File.ReadLines(filePath).Skip(1))
            {
                var length = line.Split(',')[5].Replace('"', ' ').Trim();
                pop[i] = Int32.Parse(length);
                i++;
            }
            pop_mean = knuth_mean(pop);
            pop_var = welford_variance(pop);
            var numsamples = (int)numericUpDown1.Value;

            var samplesize = (int)numericUpDown2.Value;
            samples = new double[numsamples][];

            for (i = 0; i < numsamples; i++)
            {
                double[] single_sample = new double[samplesize];

                for (int j = 0; j < samplesize; j++)
                {
                    int ri = rand.Next(0, pop.Length);

                    single_sample[j] = (double)pop[ri];
                }
                samples[i] = single_sample;
            }
            means = new double[numsamples];
            variances = new double[numsamples];
            for (int x = 0; x < numsamples; x++)
            {
                means[x] = knuth_mean(samples[x]);
                variances[x] = welford_variance(samples[x]);

            }
            mean_of_means = knuth_mean(means);
            variance_of_variances = welford_variance(variances);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (r.Contains(e.X, e.Y))
            {
                x_mouse = e.X;
                y_mouse = e.Y;

                x_down = r.X;
                y_down = r.Y;

                r_width = r.Width;
                r_height = r.Height;

                if (e.Button == MouseButtons.Left)
                {
                    drag = true;
                }
                else if (e.Button == MouseButtons.Right)
                {
                    resizing = true;
                }
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            drag = false;
            resizing = false;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            int delta_x = e.X - x_mouse;
            int delta_y = e.Y - y_mouse;

            if (r != null)
            {
                if (drag)
                {

                    r.X = x_down + delta_x;
                    r.Y = y_down + delta_y;

                    redraw(r, g);
                }
                else if (resizing)
                {
                    r.Width = r_width + delta_x;
                    r.Height = r_height + delta_y;

                    redraw(r, g);
                }
            }
        }
        private void redraw(Rectangle r, Graphics g)
        {
            g.Clear(Color.White);

            g.DrawRectangle(Pens.Black, r);
            g.FillRectangle(Brushes.Black, r);
            if (checkBox1.Checked == true)
                drawHistoMeans();
            if (checkBox2.Checked == true)
                drawHistoVariances();
            if (checkBox3.Checked == true)
                drawHistoMEANS();
            if (checkBox4.Checked == true)
                drawHistoVARIANCES();
            pictureBox1.Image = b;
        }
        private void drawHistoMeans()
        {
            double maxvalue = means.Max();


            int space_height = r.Bottom - r.Top - 20;
            int space_width = r.Right - r.Left - 20;

            int num_intervals = means.Length;

            int histrect_width_hor = space_height / num_intervals;
            int histrect_width_ver = space_width / num_intervals;

            int startY = r.Y;
            int startX = r.X;
            foreach (double k in means)
            {
                int rect_height_hor = (int)(((double)k / (double)maxvalue) * ((double)space_width));
                int rect_height_ver = (int)(((double)k / (double)maxvalue) * ((double)space_height));

                Rectangle hist_rect_hor = new Rectangle(r.Left - rect_height_hor, startY, rect_height_hor, histrect_width_hor);
                Rectangle hist_rect_ver = new Rectangle(startX, r.Bottom, histrect_width_ver, rect_height_ver);

                g.FillRectangle(Brushes.Green, hist_rect_hor);
                g.DrawRectangle(Pens.White, hist_rect_hor);
                g.FillRectangle(Brushes.Green, hist_rect_ver);
                g.DrawRectangle(Pens.White, hist_rect_ver);

                startY += histrect_width_hor;
                startX += histrect_width_ver;

            }

            pictureBox1.Image = b;


        }
        private void drawHistoVariances()
        {
            double maxvalue = variances.Max();


            int space_height = r.Bottom - r.Top - 20;
            int space_width = r.Right - r.Left - 20;

            int num_intervals = variances.Length;

            int histrect_width_hor = space_height / num_intervals;
            int histrect_width_ver = space_width / num_intervals;

            int startY = r.Y;
            int startX = r.X;
            foreach (double k in variances)
            {
                int rect_height_hor = (int)(((double)k / (double)maxvalue) * ((double)space_width));
                int rect_height_ver = (int)(((double)k / (double)maxvalue) * ((double)space_height));

                Rectangle hist_rect_hor = new Rectangle(r.Left - rect_height_hor, startY, rect_height_hor, histrect_width_hor);
                Rectangle hist_rect_ver = new Rectangle(startX, r.Bottom, histrect_width_ver, rect_height_ver);

                g.FillRectangle(Brushes.Green, hist_rect_hor);
                g.DrawRectangle(Pens.White, hist_rect_hor);
                g.FillRectangle(Brushes.Green, hist_rect_ver);
                g.DrawRectangle(Pens.White, hist_rect_ver);

                startY += histrect_width_hor;
                startX += histrect_width_ver;

            }

            pictureBox1.Image = b;

        }
        private void drawHistoMEANS()
        {
            double maxvalue;
            if (pop_mean >= mean_of_means)
                maxvalue = pop_mean;
            else maxvalue = mean_of_means;


            int space_height = r.Bottom - r.Top - 20;
            int space_width = r.Right - r.Left - 20;



            int histrect_width_hor = space_height / 2;
            int histrect_width_ver = space_width / 2;

            int startY = r.Y;
            int startX = r.X;
            double[] means = new double[2];
            means[0] = pop_mean;
            means[1] = mean_of_means;
            foreach (double k in means)
            {
                int rect_height_hor = (int)(((double)k / (double)maxvalue) * ((double)space_width));
                int rect_height_ver = (int)(((double)k / (double)maxvalue) * ((double)space_height));

                Rectangle hist_rect_hor = new Rectangle(r.Left - rect_height_hor, startY, rect_height_hor, histrect_width_hor);
                Rectangle hist_rect_ver = new Rectangle(startX, r.Bottom, histrect_width_ver, rect_height_ver);

                g.FillRectangle(Brushes.Green, hist_rect_hor);
                g.DrawRectangle(Pens.White, hist_rect_hor);
                g.FillRectangle(Brushes.Green, hist_rect_ver);
                g.DrawRectangle(Pens.White, hist_rect_ver);

                startY += histrect_width_hor;
                startX += histrect_width_ver;

            }

            pictureBox1.Image = b;


        }
        private void drawHistoVARIANCES()
        {
            double maxvalue;
            if (pop_var >= variance_of_variances)
                maxvalue = pop_var;
            else maxvalue = variance_of_variances;


            int space_height = r.Bottom - r.Top - 20;
            int space_width = r.Right - r.Left - 20;



            int histrect_width_hor = space_height / 2;
            int histrect_width_ver = space_width / 2;

            int startY = r.Y;
            int startX = r.X;
            double[] variances = new double[2];
            variances[0] = pop_var;
            variances[1] = variance_of_variances;
            foreach (double k in variances)
            {
                int rect_height_hor = (int)(((double)k / (double)maxvalue) * ((double)space_width));
                int rect_height_ver = (int)(((double)k / (double)maxvalue) * ((double)space_height));

                Rectangle hist_rect_hor = new Rectangle(r.Left - rect_height_hor, startY, rect_height_hor, histrect_width_hor);
                Rectangle hist_rect_ver = new Rectangle(startX, r.Bottom, histrect_width_ver, rect_height_ver);

                g.FillRectangle(Brushes.Green, hist_rect_hor);
                g.DrawRectangle(Pens.White, hist_rect_hor);
                g.FillRectangle(Brushes.Green, hist_rect_ver);
                g.DrawRectangle(Pens.White, hist_rect_ver);

                startY += histrect_width_hor;
                startX += histrect_width_ver;

            }

            pictureBox1.Image = b;


        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
                drawHistoMeans();
            if (checkBox1.Checked == false)
                redraw(r, g);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
                drawHistoVariances();
            if (checkBox2.Checked == false)
                redraw(r, g);
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            if (checkBox3.Checked == true)
            {
                drawHistoMEANS();
                richTextBox1.AppendText("Mean of means: " + mean_of_means.ToString() + "\n" + "Population mean: " + pop_mean.ToString() + "\n");

            }
            if (checkBox3.Checked == false)
                redraw(r, g);

        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            richTextBox1.Clear();

            if (checkBox4.Checked == true)
            {
                drawHistoVARIANCES();
                richTextBox1.AppendText("Variance of variances: " + variance_of_variances.ToString() + "\n" + "Population variance: " + pop_var.ToString() + "\n");

            }
            if (checkBox4.Checked == false)
                redraw(r, g);
        }
    }
    
}
