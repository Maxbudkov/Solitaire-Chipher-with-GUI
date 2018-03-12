using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cryptography;

namespace SolitaireChipher
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Solitaire s = new Solitaire(textBox3.Text);
            if (radioButton1.Checked)
                textBox2.Text = s.Encrypt(textBox1.Text);
            if (radioButton2.Checked)
                textBox2.Text = s.Decrypt(textBox1.Text);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            button1.Text = "Зашифровать";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            button1.Text = "Расшифровать";
        }
    }
}
