using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Windows.Forms;

namespace CSVParser
{
    public partial class Form1 : Form
    {
        string filePath = string.Empty;
        string fileContent = string.Empty;
        Stream fileStream;

        public Form1()
        {
            InitializeComponent();
           
        }
       
        private void button1_Click(object sender, EventArgs e)
        {

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog1.FileName;
                fileStream = openFileDialog1.OpenFile();
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    fileContent = reader.ReadToEnd();
                    richTextBox1.Text = fileContent;
                }


            }
            var lines = File.ReadAllLines(filePath);
            var rows = lines.Count();
            numericUpDown1.Maximum = rows;
            numericUpDown1.Value = rows - 1;
        }

        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            foreach (var headerLine in File.ReadLines(filePath).Take(1))
            {
                foreach (var headerItem in headerLine.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var column = new DataGridViewTextBoxColumn();
                    column.Name = headerItem;
                    column.HeaderText = headerItem;
                    dataGridView1.Columns.Add(column);
                }
            }
            var i = 0;
            
            foreach (var line in File.ReadLines(filePath).Skip(1))
            {
                 if (i < numericUpDown1.Value)
                    dataGridView1.Rows.Add(line.Split(','));
                 else
                    break;
                i++;
            }
        }
        private void dataGridView1_ColumnHeaderMouseDoubleClick_1(object sender, DataGridViewCellMouseEventArgs e)
        {
            richTextBox1.Clear();

            string[] countries = new string[((int)numericUpDown1.Value)];

            int x = 0;
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {

                var ciao = item.Cells[dataGridView1.Columns[e.ColumnIndex].HeaderText].Value;

                if (ciao is not null)
                    countries[x] = ciao.ToString();
                x++;
            }


            Dictionary<string, int> times = new Dictionary<string, int>();
            foreach (var country in countries)
            {                
                if (!times.ContainsKey(country))
                    times[country] = 0; 
                else
                    times[country]++;

            }
            foreach (KeyValuePair<string, int> kvp in times)
            {
                richTextBox1.AppendText(kvp.Key.ToString() + " " + kvp.Value.ToString() + "\n");
            }
            
        }
    }
}