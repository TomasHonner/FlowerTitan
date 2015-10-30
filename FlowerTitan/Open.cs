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
    public partial class Open : Form
    {

        private Database.Database database;
        private bool isTemplate;
        List<long> ids = new List<long>();
        List<DateTime> dates = new List<DateTime>();
        List<string> names = new List<string>();
        List<long> tempIDs = new List<long>();

        public Open(string title, bool isTemplate)
        {
            InitializeComponent();
            this.Text = title;
            database = Database.Database.GetInstance();
            this.isTemplate = isTemplate;
        }

        private void Open_Load(object sender, EventArgs e)
        {
            database.LoadTemplates(ids, dates, names, tempIDs, isTemplate);
            int i = 0;
            foreach (long id in ids)
            {
                DataGridViewRow dr = new DataGridViewRow();
                dr.CreateCells(dataGridView1);
                dr.Cells[0].Value = tempIDs[i];
                dr.Cells[1].Value = names[i];
                dr.Cells[2].Value = dates[i].ToString("yyyy-MM-dd HH:mm:ss");
                dataGridView1.Rows.Add(dr);
                i++;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                this.Tag = ids[tempIDs.IndexOf((long)dataGridView1.CurrentRow.Cells[0].Value)];
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                long id = ids[tempIDs.IndexOf((long)dataGridView1.CurrentRow.Cells[0].Value)];
                dataGridView1.Rows.RemoveAt(dataGridView1.CurrentCell.RowIndex);
                database.DeleteTemplate(id);
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            button1_Click(sender, e);
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
