using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Threading;

namespace FlowerTitan
{
    public partial class MainWindow : Form
    {
        private Controllers.TemplateProcessor _tp;
        private MeasuringLines.MeasuringLines measuringLines;
        //processing thread
        private Thread processingThread;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //test saved changes
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutFlowerTitan about = new AboutFlowerTitan();
            about.ShowDialog();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (measuringLines.IsEnabled)
            {
                //save to db
                tableLayoutPanel2.Enabled = false;
                menuStrip.Enabled = false;
                buttonExport.Enabled = false;
                buttonStart.Enabled = false;
                buttonStop.Enabled = true;
                timerStatus.Stop();
                toolStripStatusLabelInfo.Text = Properties.Resources.MainWindow_status_processing;
                toolStripProgressBar.Value = 0;
                processingThread = new Thread(new ThreadStart(this.process));
                processingThread.Start();
            }
        }

        private void process()
        {
            //get reference measuring lines, call only once
            MeasuringLines.Line[] referenceLines = measuringLines.GetReferenceMeasuringLines();
            _tp.startProcessing();

            //MOCK
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(1000);
                Action stateChanged = new Action(() =>
                {
                    toolStripProgressBar.Value = (i + 1) * 10;
                });
                this.Invoke(stateChanged);
                Thread.Sleep(1000);
            }
            //MOCK end

            Action processingDone = new Action(() =>
            {
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
                //MOCK end

                changeStatus(Properties.Resources.MainWindow_status_done);
                tableLayoutPanel2.Enabled = true;
                menuStrip.Enabled = true;
                buttonStop.Enabled = false;
                buttonExport.Enabled = true;
                buttonStart.Enabled = true;
            });
            this.Invoke(processingDone);
        }

        private void importTemplateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Database.Database db = Database.Database.GetInstance();
            openFileDialogImage.InitialDirectory = db.GetOpenFilePath();
            openFileDialogImage.FileName = "";
            if (openFileDialogImage.ShowDialog() == DialogResult.OK)
            {
                //clean all old measuring lines and prepare for a new template
                measuringLines.NewTemplate();
                tableLayoutPanel2.Enabled = false;
                menuStrip.Enabled = false;
                buttonExport.Enabled = false;
                buttonStart.Enabled = false;
                buttonStop.Enabled = false;
                timerStatus.Stop();
                toolStripStatusLabelInfo.Text = Properties.Resources.MainWindow_status_importing;
                toolStripProgressBar.Value = 0;
                processingThread = new Thread(new ThreadStart(this.import));
                processingThread.Start();
                db.SetOpenFilePath(System.IO.Path.GetDirectoryName(openFileDialogImage.FileName));
            }
        }

        private void import()
        {
            _tp.loadTemplate(openFileDialogImage.FileName);
            //MOCK
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(100);
                Action stateChanged = new Action(() =>
                {
                    toolStripProgressBar.Value = (i + 1) * 10;
                });
                this.Invoke(stateChanged);
                Thread.Sleep(100);
            }
            //MOCK end

            Action importingDone = new Action(() =>
            {
                //TODO: put images into boxes

                changeStatus(Properties.Resources.MainWindow_status_import);
                //enabling measuring lines on the first image
                measuringLines.EnableMeasuringLinesOnFirstImage(iB1, 100f);
                tableLayoutPanel2.Enabled = true;
                menuStrip.Enabled = true;
                buttonStop.Enabled = false;
                buttonExport.Enabled = true;
                buttonStart.Enabled = true;
            });
            this.Invoke(importingDone);
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
            _tp = new Controllers.TemplateProcessor();
            measuringLines = MeasuringLines.MeasuringLines.GetInstance(this);
            openFileDialogImage.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
            toolStripStatusLabelInfo.Text = Properties.Resources.MainWindow_status_ready;
            // Set last window location
            if (Properties.Settings.Default.MainWindowLocation != null)
            {
                this.Location = Properties.Settings.Default.MainWindowLocation;
            }
        }

        private void loadTemplateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (measuringLines.IsEnabled)
            {
                //replace lines with lines from db
                changeStatus(Properties.Resources.MainWindow_status_load);
            }
        }

        private void saveAsTemplateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (measuringLines.IsEnabled)
            {
                //save lines from first image to db
                changeStatus(Properties.Resources.MainWindow_status_save);
            }
        }

        private void panel1_MouseEnter(object sender, EventArgs e)
        {
            //enables mouse wheel scrolling
            panel1.Focus();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //clean all old measuring lines and prepare for a new template
            measuringLines.NewTemplate();
            // TODO: load template from DB, enabling images, lines, dialog result, enable measuring lines
            changeStatus(Properties.Resources.MainWindow_status_load);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (measuringLines.IsEnabled)
            {
                // TODO: save template to DB
                changeStatus(Properties.Resources.MainWindow_status_save);
            }
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //clean if new
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //clean if new
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Options o = new Options();
            o.ShowDialog();
        }

        private void linesControlsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            imageToolTip.Show(Properties.Resources.MeasuringLines_help, panel1, panel1.Location, 10000);
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            if (measuringLines.IsEnabled)
            {
                //save to db
                //get from db
                //generate
                changeStatus(Properties.Resources.MainWindow_status_export);
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            processingThread.Abort();
            //load from db
            changeStatus(Properties.Resources.MainWindow_status_abort);
            tableLayoutPanel2.Enabled = true;
            menuStrip.Enabled = true;
            buttonStop.Enabled = false;
            buttonExport.Enabled = true;
            buttonStart.Enabled = true;
        }

        private void createReportToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void timerStatus_Tick(object sender, EventArgs e)
        {
            timerStatus.Stop();
            toolStripStatusLabelInfo.Text = Properties.Resources.MainWindow_status_ready;
            toolStripProgressBar.Value = 0;
        }

        private void changeStatus(string status)
        {
            timerStatus.Stop();
            toolStripStatusLabelInfo.Text = status;
            toolStripProgressBar.Value = 100;
            timerStatus.Start();
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            //save window location
            Properties.Settings.Default.MainWindowLocation = this.Location;
            Properties.Settings.Default.Save();
        }
    }
}
