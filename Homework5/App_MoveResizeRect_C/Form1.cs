using System.Diagnostics.Metrics;
using System.IO;
using System.Windows.Forms;

namespace App_MoveResizeRect_C
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
        Dictionary<string, int> times = new Dictionary<string, int>();

        private void button1_Click(object sender, EventArgs e)
        {
            b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(b);

            r = new Rectangle(20, 20, 500, 300);

            g.DrawRectangle(Pens.Black, r);
            g.FillRectangle(Brushes.Black, r);
            pictureBox1.Image = b;
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
                drawHisto();
            pictureBox1.Image = b;
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


            foreach (var line in File.ReadLines(filePath).Skip(1))
            {
                var protocol = line.Split(',')[4];
                if (!times.ContainsKey(protocol))
                    times[protocol] = 0;
                else
                    times[protocol]++;
            }
        }
        private void drawHisto()
        {

            int maxvalue = times.Values.Max();

            int space_height = r.Bottom - r.Top - 20;
            int space_width = r.Right - r.Left - 20;

            int num_intervals = times.Keys.Count;
            int histrect_width_hor = space_height / num_intervals;
            int histrect_width_ver = space_width / num_intervals;

            int startY = r.Y;
            int startX = r.X;
            foreach (KeyValuePair<String, int> k in times)
            {
                int rect_height_hor = (int)(((double)k.Value / (double)maxvalue) * ((double)space_width));
                int rect_height_ver = (int)(((double)k.Value / (double)maxvalue) * ((double)space_height));

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
                drawHisto();
            if (checkBox1.Checked == false)
                redraw(r, g);

        }
    }
}