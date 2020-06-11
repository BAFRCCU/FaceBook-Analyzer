using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Viewer
{
    public partial class Messages : Form
    {
        
        public Messages()
        {
            InitializeComponent();
            
        }

        public void SetRichTextBox(string text)
        {
            richTextBox1.Text = text;
           


        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = 0;
            richTextBox1.SelectAll();
            richTextBox1.SelectionBackColor = Color.White;
            label1.Text = "Hits";

            string[] words = textBox1.Text.Split(',');
            foreach (string word in words)
            {
                int startindex = 0;
                int hits = 0;
                while (startindex < richTextBox1.TextLength)
                {
                    int wordstartIndex = richTextBox1.Find(word, startindex, RichTextBoxFinds.None);
                    if (wordstartIndex != -1)
                    {
                        richTextBox1.SelectionStart = wordstartIndex;
                        richTextBox1.SelectionLength = word.Length;
                        richTextBox1.SelectionBackColor = Color.Yellow;

                        hits++;
                    }
                    else
                        break;
                    startindex += wordstartIndex + word.Length;
                }

                label1.Text = hits + " Hits";
            }
        }
    
    }
}
