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
        private Database.Database database;
        private long autoSaveID = 0;
        //processing thread
        private Thread processingThread;

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
                autoSaveID = database.SaveTemplate(measuringLines.ActiveImagesLines, measuringLines.ActiveImagesImages, false, tID.Text, tBtemplateName.Text, measuringLines.Scale, measuringLines.Colors, measuringLines.Names);
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

                database.DeleteTemplate(autoSaveID);
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
            openFileDialogImage.InitialDirectory = database.GetOpenFilePath();
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
                database.SetOpenFilePath(System.IO.Path.GetDirectoryName(openFileDialogImage.FileName));
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
                //MOCK
                tID.Text = new Random().Next().ToString();
                iB1.Image = new Emgu.CV.Image<Emgu.CV.Structure.Bgr, Byte>(100, 100, new Emgu.CV.Structure.Bgr(Color.Aqua));
                iB2.Image = new Emgu.CV.Image<Emgu.CV.Structure.Bgr, Byte>(100, 100, new Emgu.CV.Structure.Bgr(Color.Blue));
                iB3.Image = new Emgu.CV.Image<Emgu.CV.Structure.Bgr, Byte>(100, 100, new Emgu.CV.Structure.Bgr(Color.Brown));
                iB4.Image = new Emgu.CV.Image<Emgu.CV.Structure.Bgr, Byte>(100, 100, new Emgu.CV.Structure.Bgr(Color.Coral));
                iB5.Image = new Emgu.CV.Image<Emgu.CV.Structure.Bgr, Byte>(100, 100, new Emgu.CV.Structure.Bgr(Color.DarkGreen));
                iB6.Image = new Emgu.CV.Image<Emgu.CV.Structure.Bgr, Byte>(100, 100, new Emgu.CV.Structure.Bgr(Color.DarkOrange));
                iB7.Image = new Emgu.CV.Image<Emgu.CV.Structure.Bgr, Byte>(100, 100, new Emgu.CV.Structure.Bgr(Color.DarkViolet));
                iB8.Image = new Emgu.CV.Image<Emgu.CV.Structure.Bgr, Byte>(100, 100, new Emgu.CV.Structure.Bgr(Color.Firebrick));
                iB9.Image = new Emgu.CV.Image<Emgu.CV.Structure.Bgr, Byte>(100, 100, new Emgu.CV.Structure.Bgr(Color.Gold));
                iB10.Image = new Emgu.CV.Image<Emgu.CV.Structure.Bgr, Byte>(100, 100, new Emgu.CV.Structure.Bgr(Color.Honeydew));
                iB11.Image = new Emgu.CV.Image<Emgu.CV.Structure.Bgr, Byte>(100, 100, new Emgu.CV.Structure.Bgr(Color.LemonChiffon));
                iB12.Image = new Emgu.CV.Image<Emgu.CV.Structure.Bgr, Byte>(100, 100, new Emgu.CV.Structure.Bgr(Color.LightSeaGreen));
                //MOCK end

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
                Open o = new Open(Properties.Resources.Open_load_template, true);
                if ((o.ShowDialog() == System.Windows.Forms.DialogResult.OK) && (o.Tag != null))
                {
                    List<int> colors = new List<int>();
                    List<string> names = new List<string>();
                    MeasuringLines.Line[] tempLines = database.LoadAsTemplate((long)o.Tag, colors, names);
                    measuringLines.SetTemplateLines(iB1, tempLines, colors.ToArray(), names.ToArray());
                    changeStatus(Properties.Resources.MainWindow_status_load);
                }
            }
        }

        private void saveAsTemplateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (measuringLines.IsEnabled)
            {
                database.SaveTemplate(measuringLines.ActiveImagesLines, measuringLines.ActiveImagesImages, true, tID.Text, tBtemplateName.Text, measuringLines.Scale, measuringLines.Colors, measuringLines.Names);
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
            Open o = new Open(Properties.Resources.Open_load_template, false);
            if ((o.ShowDialog() == System.Windows.Forms.DialogResult.OK) && (o.Tag != null))
            {
                List<int> colors = new List<int>();
                List<string> names = new List<string>();
                List<byte[]> images = new List<byte[]>();
                string name = "";
                double scale = 0;
                long tempID = 0L;
                MeasuringLines.AllLines[] allLines = database.LoadTemplate((long)o.Tag, colors, names, ref name, ref scale, ref tempID, images);
                measuringLines.SetAllTemplateLines(allLines, colors.ToArray(), names.ToArray(), images.ToArray(), name, scale, tempID);
                changeStatus(Properties.Resources.MainWindow_status_load);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (measuringLines.IsEnabled)
            {
                database.SaveTemplate(measuringLines.ActiveImagesLines, measuringLines.ActiveImagesImages, false, tID.Text, tBtemplateName.Text, measuringLines.Scale, measuringLines.Colors, measuringLines.Names);
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
            changeStatus(Properties.Resources.MainWindow_status_abort);
            List<int> colors = new List<int>();
            List<string> names = new List<string>();
            List<byte[]> images = new List<byte[]>();
            string name = "";
            double scale = 0;
            long tempID = 0L;
            MeasuringLines.AllLines[] allLines = database.LoadTemplate(autoSaveID, colors, names, ref name, ref scale, ref tempID, images);
            measuringLines.SetAllTemplateLines(allLines, colors.ToArray(), names.ToArray(), images.ToArray(), name, scale, tempID);
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
