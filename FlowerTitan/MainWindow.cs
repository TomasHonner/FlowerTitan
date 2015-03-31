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
    public partial class MainWindow : Form
    {
        Controllers.TemplateProcessor _tp;
        MeasuringLines.MeasuringLines measuringLines;

        public MainWindow()
        {
            _tp = new Controllers.TemplateProcessor();
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutFlowerTitan about = new AboutFlowerTitan();
            about.ShowDialog();
        }

        private void loadTemplateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //clean all old measuring lines and prepare for a new template
            measuringLines.NewTemplate();
            // TODO: load template from DB, enabling images, lines, dialog result
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (measuringLines.IsEnabled)
            {
                //get reference measuring lines, call only once
                MeasuringLines.Line[] referenceLines = measuringLines.GetReferenceMeasuringLines();

                _tp.startProcessing();
                // MOCK: add processed lines to image (for every image with its processed lines)
                //every array has to have the same count of lines and line's points as the reference one
                measuringLines.AddMeasuringLinesToImage(iB2, referenceLines);
                measuringLines.AddMeasuringLinesToImage(iB3, referenceLines);
                measuringLines.AddMeasuringLinesToImage(iB4, referenceLines);
                measuringLines.AddMeasuringLinesToImage(iB5, referenceLines);
                measuringLines.AddMeasuringLinesToImage(iB6, referenceLines);
                measuringLines.AddMeasuringLinesToImage(iB7, referenceLines);
                measuringLines.AddMeasuringLinesToImage(iB8, referenceLines);
                measuringLines.AddMeasuringLinesToImage(iB9, referenceLines);
                measuringLines.AddMeasuringLinesToImage(iB10, referenceLines);
                measuringLines.AddMeasuringLinesToImage(iB11, referenceLines);
                measuringLines.AddMeasuringLinesToImage(iB12, referenceLines);
            }
        }

        private void importTemplateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //clean all old measuring lines and prepare for a new template
            measuringLines.NewTemplate();

            // TODO: dialog result - close button
            _tp.loadTemplate();
            toolStripStatusLabelInfo.Text = "Template loaded.";

            //enabling measuring lines on the first image
            measuringLines.EnableMeasuringLinesOnFirstImage(iB1);
        }

        /// <summary>
        /// Open form with generation's options.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">click event</param>
        private void newTemplateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TemplateGenerator.TemplateGeneratorWindow tGW = new TemplateGenerator.TemplateGeneratorWindow();
            tGW.ShowDialog();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            measuringLines = MeasuringLines.MeasuringLines.GetInstance(this);
        }

        private void saveTemplateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (measuringLines.IsEnabled)
            {
                // TODO: save template to DB,test null measuringLines
            }
        }

        private void panel1_MouseEnter(object sender, EventArgs e)
        {
            //enables mouse wheel scrolling
            panel1.Focus();
        }
    }
}
