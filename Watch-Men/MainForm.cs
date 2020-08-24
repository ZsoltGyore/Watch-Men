using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Watch_Men
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void órákToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Orak form = new Orak();
            form.ShowDialog();
        }

        private void ékszerekToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Ekszerek form = new Ekszerek();
            form.ShowDialog();
        }

        private void kiegészítőkToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Kiegeszitok form = new Kiegeszitok();
            form.ShowDialog();
        }

        private void ügyfelekToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Ugyfelek form = new Ugyfelek();
            form.ShowDialog();
        }

        private void névjegyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Nevjegy form = new Nevjegy();
            form.ShowDialog();
        }

        private void kilépésToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Biztosan kilép a programból?", "Kilép?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Biztosan kilép a programból?", "Kilép?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
