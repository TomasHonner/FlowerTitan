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
    public partial class Options : Form
    {
        private Database.Database db;

        public Options()
        {
            InitializeComponent();
        }

        private void buttonSetPath_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialogXLSPath.ShowDialog() == DialogResult.OK)
            {
                db.SetXLSPath(folderBrowserDialogXLSPath.SelectedPath);
                textBoxXLSPath.Text = folderBrowserDialogXLSPath.SelectedPath;
            }
        }

        private void Options_Load(object sender, EventArgs e)
        {
            db = Database.Database.GetInstance();
            textBoxXLSPath.Text = db.GetXLSPath();
        }
    }
}
