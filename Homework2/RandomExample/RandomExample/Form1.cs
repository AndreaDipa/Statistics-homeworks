using System.Text;
using System.Windows.Forms.VisualStyles;

namespace RandomExample
{
    public partial class Form1 : Form
    {
        Random r = new Random();
        double sum = 0;
        int count = 0;
        int[] C  = new int[10];
       
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Start();

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            double rand = r.Next(0, 100); 
            progressBar1.Value = (Int32)rand;
            richTextBox1.BackColor = Color.FromArgb(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256));
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            int length = 8;
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            richTextBox2.Text = res.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}