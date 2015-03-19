using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlowerTitan
{
    public partial class Form1 : Form
    {
        Controllers.TemplateProcessor _tp;

        public Form1()
        {
            _tp = new Controllers.TemplateProcessor();
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About a = new About();
            a.ShowDialog();
            ///-
        }

        private void loadTemplateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _tp.loadTemplate();
            MessageBox.Show("Template loaded");
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            _tp.startProcessing();
        }
    }
}
