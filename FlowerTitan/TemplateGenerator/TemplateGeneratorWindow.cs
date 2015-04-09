using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlowerTitan.TemplateGenerator
{
    /// <summary>
    /// Partial class of TemplateGeneratorWindow which calls TemplateGenerator functions.
    /// </summary>
    public partial class TemplateGeneratorWindow : Form
    {
        //holds instance of TemplateGenerator
        private TemplateGenerator templateGenerator;

        /// <summary>
        /// Constructor.
        /// </summary>
        public TemplateGeneratorWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Window load event handler.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">load event</param>
        private void TemplateGeneratorWindow_Load(object sender, EventArgs e)
        {
            //get instance of TemplateGenerator class
            templateGenerator = TemplateGenerator.GetInstance();
            //save dialog settings
            saveFileDialog.DefaultExt = "pdf";
            saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
        }

        /// <summary>
        /// Determines whether template will be printed or stored in user's chosen location.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">click event</param>
        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            //print
            if (radioButtonPrint.Checked)
            {
                //generate pdf
                PdfSharp.Pdf.PdfDocument pdf = generatePdf();
                //save to temp
                templateGenerator.SavePdf(pdf, System.IO.Path.GetTempPath() + "template.pdf");
            }
            else
            {
                //save dialog
                saveFileDialog.FileName = "template";
                //get recently used path from db
                saveFileDialog.InitialDirectory = templateGenerator.GetSaveFilePath();
                if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    //generate pdf
                    PdfSharp.Pdf.PdfDocument pdf = generatePdf();
                    //save pdf to user's chosen loacation
                    templateGenerator.SavePdf(pdf, saveFileDialog.FileName);
                    //save used path to db
                    templateGenerator.SetSaveFilePath(System.IO.Path.GetDirectoryName(saveFileDialog.FileName));
                }
            }
        }

        /// <summary>
        /// Calls pdf generation.
        /// </summary>
        /// <returns>generated pdf</returns>
        private PdfSharp.Pdf.PdfDocument generatePdf()
        {
            Cursor.Current = Cursors.WaitCursor;
            PdfSharp.Pdf.PdfDocument pdf = templateGenerator.GenerateTemplate((int)numericUpDownNumber.Value);
            Cursor.Current = Cursors.Arrow;
            return pdf;
        }

        /// <summary>
        /// Closes the form.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">click event</param>
        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
