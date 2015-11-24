using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlowerTitan
{
    /// <summary>
    /// FlowerTitan open/delete from DB window/form.
    /// </summary>
    public partial class Open : Form
    {

        private Database.Database database;
        private bool isTemplate;
        private Thread processingThread;
        List<long> ids = new List<long>();
        List<DateTime> dates = new List<DateTime>();
        List<string> names = new List<string>();
        List<long> tempIDs = new List<long>();

        /// <summary>
        /// Parametrized constructor.
        /// </summary>
        /// <param name="title">Window title.</param>
        /// <param name="isTemplate">Determines whether templates or template templates from DB should be loaded.</param>
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
            dataGridView1.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (processingThread != null)
            {
                if (processingThread.IsAlive) return;
            }
            if (dataGridView1.Rows.Count > 0)
            {
                this.Tag = ids[tempIDs.IndexOf((long)dataGridView1.CurrentRow.Cells[0].Value)];
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                if (processingThread != null)
                {
                    if (processingThread.IsAlive) return;
                }
                int rowID = dataGridView1.CurrentCell.RowIndex;
                long id = ids[tempIDs.IndexOf((long)dataGridView1.CurrentRow.Cells[0].Value)];
                DataGridViewRow dr = new DataGridViewRow();
                dr.CreateCells(dataGridView1);
                dr.Cells[0].Value = Properties.Resources.Open_status_deleting;
                dr.Cells[1].Value = "";
                dr.Cells[2].Value = "";
                dataGridView1.Rows.RemoveAt(rowID);
                dataGridView1.Rows.Insert(rowID, dr);
                processingThread = new Thread(() => this.delete(id, rowID));
                processingThread.Start();
            }
            dataGridView1.Focus();
        }

        private void delete(long id, int rowID)
        {
            database.DeleteTemplate(id);

            Action deleteDone = new Action(() =>
            {
                dataGridView1.Rows.RemoveAt(rowID);
            });
            this.Invoke(deleteDone);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            button1_Click(sender, e);
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void Open_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (processingThread != null)
            {
                if (processingThread.IsAlive) e.Cancel = true;
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Delete:
                    button3_Click(this, new EventArgs());
                    break;
                case Keys.Escape:
                    this.Close();
                    break;
            }
        }

        /// <summary>
        /// Capture form press.
        /// </summary>
        /// <param name="msg">message</param>
        /// <param name="keyData">keyData</param>
        /// <returns>whether key press was handeled.</returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (dataGridView1.Focused)
            {
                if (keyData == Keys.Enter)
                {
                    button1_Click(this, new EventArgs());
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    return true;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
