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
    /// <summary>
    /// FlowerTitan main window/form.
    /// </summary>
    public partial class MainWindow : Form
    {
        private bool previousExportState = false;
        private Controllers.TemplateProcessor _tp;
        private MeasuringLines.MeasuringLines measuringLines;
        private Database.Database database;
        //processing thread
        private Thread processingThread;
        private List<Emgu.CV.Image<Emgu.CV.Structure.Bgr, Byte>> blossoms;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public MainWindow()
        {
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

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (measuringLines.IsEnabled)
            {
                previousExportState = buttonExport.Enabled;
                exportDataToolStripMenuItem.Enabled = false;
                buttonExport.Enabled = false;
                buttonStart.Enabled = false;
                buttonStop.Enabled = true;
                timerStatus.Stop();
                toolStripStatusLabelInfo.Text = Properties.Resources.MainWindow_status_processing;
                toolStripProgressBar.Value = 0;
                panel1.Enabled = false;
                panel8.Enabled = false;
                menuStrip.Enabled = false;
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
            /*
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
             */

            //MOCK end

            Action processingDone = new Action(() =>
            {
                buttonStop.Enabled = false;
                changeStatus(Properties.Resources.MainWindow_status_done);
                panel1.Enabled = true;
                panel8.Enabled = true;
                menuStrip.Enabled = true;

                // MOCK: add processed lines to image (for every image with its processed lines)
                //every array has to have the same count of lines and line's points as the reference one
                measuringLines.AddMeasuringLinesToImage(iB2, referenceLines, 2);
                measuringLines.AddMeasuringLinesToImage(iB3, referenceLines, 3);
                measuringLines.AddMeasuringLinesToImage(iB4, referenceLines, 4);
                measuringLines.AddMeasuringLinesToImage(iB5, referenceLines, 5);
                measuringLines.AddMeasuringLinesToImage(iB6, referenceLines, 6);
                measuringLines.AddMeasuringLinesToImage(iB7, referenceLines, 7);
                measuringLines.AddMeasuringLinesToImage(iB8, referenceLines, 8);
                measuringLines.AddMeasuringLinesToImage(iB9, referenceLines, 9);
                measuringLines.AddMeasuringLinesToImage(iB10, referenceLines, 10);
                measuringLines.AddMeasuringLinesToImage(iB11, referenceLines, 11);
                measuringLines.AddMeasuringLinesToImage(iB12, referenceLines, 12);
                //MOCK end

                buttonExport.Enabled = true;
                buttonStart.Enabled = true;
                exportDataToolStripMenuItem.Enabled = true;
                saveTemplateToolStripMenuItem.Enabled = true;
                saveTemplateToolStripMenuItem1.Enabled = true;
                loadTemplateToolStripMenuItem1.Enabled = false;
            });
            this.Invoke(processingDone);
        }

        private void importTemplateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialogImage.InitialDirectory = database.GetOpenFilePath();
            openFileDialogImage.FileName = "";
            if (openFileDialogImage.ShowDialog() == DialogResult.OK)
            {
                //clean all old measuring lines and prepare for a new template
                measuringLines.NewTemplate();
                tBtemplateName.Text = "";
                threadStart(Properties.Resources.MainWindow_status_importing);
                exportDataToolStripMenuItem.Enabled = false;
                buttonExport.Enabled = false;
                buttonStart.Enabled = false;
                buttonStop.Enabled = false;
                processingThread = new Thread(new ThreadStart(this.import));
                processingThread.Start();
                database.SetOpenFilePath(System.IO.Path.GetDirectoryName(openFileDialogImage.FileName));
            }
        }

        private void import()
        {
            _tp.loadTemplate(openFileDialogImage.FileName);
            _tp.createListOfBlossoms();
            this.blossoms = _tp.ListOfBlossomsToDraw;
            //TODO insert bitmap with template id, it is crutial that image mustn't have its frame, otherwise OCR will usually fail
            Bitmap bitmapTempID = new Bitmap(@"E:\IS projekt\test_data\tempIDs\test14.png");
            string tempID = TemplateOCR.TemplateIdOCR.GetInstance().ProcessTemplateID(bitmapTempID);

            Action importingDone = new Action(() =>
            {
                iB1.Image = this.blossoms.ElementAt(0);
                iB2.Image = this.blossoms.ElementAt(1);
                iB3.Image = this.blossoms.ElementAt(2);
                iB4.Image = this.blossoms.ElementAt(3);
                iB5.Image = this.blossoms.ElementAt(4);
                iB6.Image = this.blossoms.ElementAt(5);
                iB7.Image = this.blossoms.ElementAt(6);
                iB8.Image = this.blossoms.ElementAt(7);
                iB9.Image = this.blossoms.ElementAt(8);
                iB10.Image = this.blossoms.ElementAt(9);
                iB11.Image = this.blossoms.ElementAt(10);
                iB12.Image = this.blossoms.ElementAt(11);

                tID.Text = tempID;
                pictureBoxID.Image = bitmapTempID;
                threadDone(Properties.Resources.MainWindow_status_import);
                //enabling measuring lines on the first image
                measuringLines.EnableMeasuringLinesOnFirstImage(iB1, (float)_tp.getDpi());
                buttonStop.Enabled = false;
                buttonExport.Enabled = false;
                buttonStart.Enabled = true;
                exportDataToolStripMenuItem.Enabled = false;
                loadTemplateToolStripMenuItem1.Enabled = true;
                saveTemplateToolStripMenuItem.Enabled = false;
                saveTemplateToolStripMenuItem1.Enabled = true;
                panel1.Visible = true;
            });
            this.Invoke(importingDone);
        }

        private void newTemplateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TemplateGenerator.TemplateGeneratorWindow tGW = new TemplateGenerator.TemplateGeneratorWindow();
            tGW.ShowDialog();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            _tp = new Controllers.TemplateProcessor();
            measuringLines = MeasuringLines.MeasuringLines.GetInstance(this);
            database = Database.Database.GetInstance();
            openFileDialogImage.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
            toolStripStatusLabelInfo.Text = Properties.Resources.MainWindow_status_ready;
            loadTemplateToolStripMenuItem1.Enabled = false;
            saveTemplateToolStripMenuItem.Enabled = false;
            saveTemplateToolStripMenuItem1.Enabled = false;
            exportDataToolStripMenuItem.Enabled = false;
            buttonStart.Enabled = false;
            buttonStop.Enabled = false;
            buttonExport.Enabled = false;
            // Set last window location
            if (Properties.Settings.Default.MainWindowLocation != null)
            {
                this.Location = Properties.Settings.Default.MainWindowLocation;
                trackBarThickness.Value = Properties.Settings.Default.LineThickness;
                trackBarPointSize.Value = Properties.Settings.Default.PointSize;
                buttonColor.BackColor = Properties.Settings.Default.LineColor;
                measuringLines.UpdateSettings();
            }
        }

        private void loadTemplateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (measuringLines.IsEnabled)
            {
                //replace lines with lines from db
                Open o = new Open(Properties.Resources.Open_load_template, true);
                if ((o.ShowDialog() == System.Windows.Forms.DialogResult.OK) && (o.Tag != null))
                {
                    List<int> colors = new List<int>();
                    List<string> names = new List<string>();
                    MeasuringLines.Line[] tempLines = database.LoadAsTemplate((long)o.Tag, colors, names);
                    measuringLines.SetTemplateLines(iB1, tempLines, colors.ToArray(), names.ToArray());
                    changeStatus(Properties.Resources.MainWindow_status_load);
                    saveTemplateToolStripMenuItem.Enabled = false;
                    saveTemplateToolStripMenuItem1.Enabled = true;
                    loadTemplateToolStripMenuItem1.Enabled = true;
                    exportDataToolStripMenuItem.Enabled = false;
                    buttonStart.Enabled = true;
                    buttonStop.Enabled = false;
                    buttonExport.Enabled = false;
                }
            }
        }

        private void saveAsTemplateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (measuringLines.IsEnabled)
            {
                threadStart(Properties.Resources.MainWindow_status_saving);
                processingThread = new Thread(() => this.save(true));
                processingThread.Start();
            }
        }

        private void panel1_MouseEnter(object sender, EventArgs e)
        {
            //enables mouse wheel scrolling
            panel1.Focus();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Open o = new Open(Properties.Resources.Open_load, false);
            if ((o.ShowDialog() == System.Windows.Forms.DialogResult.OK) && (o.Tag != null))
            {
                List<int> colors = new List<int>();
                List<string> names = new List<string>();
                List<byte[]> images = new List<byte[]>();
                List<byte> tempIdImage = new List<byte>();
                string name = "";
                double scale = 0;
                long tempID = 0L;
                MeasuringLines.AllLines[] allLines = database.LoadTemplate((long)o.Tag, colors, names, ref name, ref scale, ref tempID, images, ref tempIdImage);
                measuringLines.SetAllTemplateLines(allLines, colors.ToArray(), names.ToArray(), images.ToArray(), name, scale, tempID, tempIdImage.ToArray());
                changeStatus(Properties.Resources.MainWindow_status_load);
                loadTemplateToolStripMenuItem1.Enabled = true;
                saveTemplateToolStripMenuItem.Enabled = true;
                saveTemplateToolStripMenuItem1.Enabled = true;
                exportDataToolStripMenuItem.Enabled = true;
                buttonStart.Enabled = true;
                buttonStop.Enabled = false;
                buttonExport.Enabled = true;
                panel1.Visible = true;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (measuringLines.IsEnabled)
            {
                threadStart(Properties.Resources.MainWindow_status_saving);
                processingThread = new Thread(() => this.save(false));
                processingThread.Start();
            }
        }

        private void threadStart(string message)
        {
            timerStatus.Stop();
            toolStripStatusLabelInfo.Text = message;
            toolStripProgressBar.Value = 0;
            tableLayoutPanel2.Enabled = false;
            menuStrip.Enabled = false;
        }

        private void save(bool isTemplate)
        {
            database.SaveTemplate(measuringLines.ActiveImagesLines, measuringLines.ActiveImagesImages, isTemplate, tID.Text, tBtemplateName.Text, measuringLines.Scale, measuringLines.Colors, measuringLines.Names, this);

            Action saveDone = new Action(() =>
            {
                threadDone(Properties.Resources.MainWindow_status_save);
            });
            this.Invoke(saveDone);
        }

        private void threadDone(string message)
        {
            changeStatus(message);
            tableLayoutPanel2.Enabled = true;
            menuStrip.Enabled = true;
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
                FlowerTitan.XLSXGenerator.XLSXGenerator.GetInstance().ExportData(tID.Value.ToString(), tBtemplateName.Text, measuringLines.Names, measuringLines.ActiveImagesLines, database.GetXLSPath(), measuringLines.Scale);
                changeStatus(Properties.Resources.MainWindow_status_export);
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            processingThread.Abort();
            measuringLines.ProcessingAborted();
            changeStatus(Properties.Resources.MainWindow_status_abort);
            panel1.Enabled = true;
            panel8.Enabled = true;
            menuStrip.Enabled = true;
            buttonStop.Enabled = false;
            buttonStart.Enabled = true;
            buttonExport.Enabled = previousExportState;
            exportDataToolStripMenuItem.Enabled = previousExportState;
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
            if (processingThread != null)
            {
                if (processingThread.IsAlive) e.Cancel = true;
            }
            Properties.Settings.Default.MainWindowLocation = this.Location;
            Properties.Settings.Default.LineColor = buttonColor.BackColor;
            Properties.Settings.Default.LineThickness = trackBarThickness.Value;
            Properties.Settings.Default.PointSize = trackBarPointSize.Value;
            Properties.Settings.Default.Save();
        }

        private void tID_DoubleClick(object sender, EventArgs e)
        {
            tID.ReadOnly = !tID.ReadOnly;
        }

        /// <summary>
        /// Gets template's scale.
        /// </summary>
        /// <returns>template's scale</returns>
        public double GetTemplateScale()
        {
            return measuringLines.Scale;
        }
    }
}
