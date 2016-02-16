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
        private Core.TemplateProcessor tp;
        private MeasuringLines.MeasuringLines measuringLines;
        private Database.Database database;
        private Emgu.CV.UI.ImageBox[] allImages;
        //processing thread
        private Thread processingThread;

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
            int a, b;
            a = treshold1.Value;
            b = treshold2.Value;
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
                processingThread = new Thread(() => this.process(a, b));
                processingThread.Start();
                panelProcessing.Enabled = true;
                buttonImages.Enabled = true;
                buttonEdges.Enabled = true;
            }
        }

        private void process(int treshold, int tresholdLinking)
        {
            int c = 1;
            if (!measuringLines.IsFirstProcessing) c = 12;
            List<MeasuringLines.Line[]> referenceLines = new List<MeasuringLines.Line[]>();
            for (int i = 0; i < c; i++)
            {
                referenceLines.Add(measuringLines.GetReferenceMeasuringLines(i));
            }
            MeasuringLines.Line[][] resultLines = tp.startProcessing(treshold, tresholdLinking, referenceLines.ToArray(), this);
            
            Action processingDone = new Action(() =>
            {
                buttonStop.Enabled = false;
                changeStatus(Properties.Resources.MainWindow_status_done);
                panel1.Enabled = true;
                panel8.Enabled = true;
                menuStrip.Enabled = true;

                int counter = 0;
                foreach (MeasuringLines.Line[] lines in resultLines)
                {
                    measuringLines.AddMeasuringLinesToImage(allImages[counter], lines, (counter + 1));
                    counter++;
                }

                buttonExport.Enabled = true;
                buttonStart.Enabled = true;
                exportDataToolStripMenuItem.Enabled = true;
                saveTemplateToolStripMenuItem.Enabled = true;
                saveTemplateToolStripMenuItem1.Enabled = true;
                loadTemplateToolStripMenuItem1.Enabled = false;
            });
            this.Invoke(processingDone);
        }

        private void drawBlossoms()
        {
            for (int i = 0; i < tp.ListOfBlossomsToDraw.Count - 1; i++)
            {
                allImages[i].Image = tp.ListOfBlossomsToDraw[i];
            }
        }

        private void drawEdges()
        {
            for (int i = 0; i < tp.ListOfEdgesToDraw.Count - 1; i++)
            {
                allImages[i].Image = tp.ListOfEdgesToDraw[i];
            }
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
                int width = iB1.Width;
                int height = iB1.Height;
                processingThread = new Thread(() => this.import(width, height));
                processingThread.Start();
                database.SetOpenFilePath(System.IO.Path.GetDirectoryName(openFileDialogImage.FileName));
            }
        }

        private void import(int width, int height)
        {
            tp.loadTemplate(openFileDialogImage.FileName, this);
            tp.createListOfBlossoms(width, height, this);
            //set template id image and get number
            Emgu.CV.Image<Emgu.CV.Structure.Bgr, Byte> emguCVImage = tp.ListOfBlossomsToDraw[tp.ListOfBlossomsToDraw.Count - 1];
            string tempID = TemplateOCR.TemplateIdOCR.GetInstance().ProcessTemplateID(emguCVImage);

            Action importingDone = new Action(() =>
            {
                drawBlossoms();
                tID.Text = tempID;
                imageBoxID.Image = emguCVImage;
                threadDone(Properties.Resources.MainWindow_status_import);
                //enabling measuring lines on the first image
                measuringLines.EnableMeasuringLinesOnFirstImage(iB1, (float)tp.getDpi());
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
            tp = new Core.TemplateProcessor();
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
            buttonEdges.Enabled = false;
            buttonImages.Enabled = false;
            allImages = new Emgu.CV.UI.ImageBox[] { iB1, iB2, iB3, iB4, iB5, iB6, iB7, iB8, iB9, iB10, iB11, iB12 };
            // Set last window location
            if (Properties.Settings.Default.MainWindowLocation != null)
            {
                this.Location = Properties.Settings.Default.MainWindowLocation;
                this.Size = Properties.Settings.Default.MainWindowSize;
                this.WindowState = Properties.Settings.Default.MainWindowState;
                trackBarThickness.Value = Properties.Settings.Default.LineThickness;
                trackBarPointSize.Value = Properties.Settings.Default.PointSize;
                buttonColor.BackColor = Properties.Settings.Default.LineColor;
                treshold1.Value = Properties.Settings.Default.Treshold1;
                treshold2.Value = Properties.Settings.Default.Treshold2;
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
                    buttonImages.Enabled = false;
                    buttonEdges.Enabled = false;
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
                buttonImages.Enabled = false;
                buttonEdges.Enabled = false;
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
            if (this.WindowState == FormWindowState.Maximized)
            {
                Properties.Settings.Default.MainWindowState = this.WindowState;
            }
            else
            {
                Properties.Settings.Default.MainWindowState = FormWindowState.Normal;
                Properties.Settings.Default.MainWindowLocation = this.Location;
                Properties.Settings.Default.MainWindowSize = this.Size;
            }
            Properties.Settings.Default.LineColor = buttonColor.BackColor;
            Properties.Settings.Default.LineThickness = trackBarThickness.Value;
            Properties.Settings.Default.PointSize = trackBarPointSize.Value;
            Properties.Settings.Default.Treshold1 = treshold1.Value;
            Properties.Settings.Default.Treshold2 = treshold2.Value;
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

        private void panel6_MouseEnter(object sender, EventArgs e)
        {
            panel6.Focus();
        }

        private void panel9_Paint(object sender, PaintEventArgs e)
        {
            int left = iB1.Location.X;
            int top = iB1.Location.Y;
            int offset = iB2.Location.X - iB1.Location.X;
            Pen pen = new Pen(Color.Black, 12);
            Rectangle rec = new Rectangle(imageBoxID.Location.X, imageBoxID.Location.Y, imageBoxID.Width, imageBoxID.Height);
            e.Graphics.DrawRectangle(pen, rec);
            for (int row = 0; row < 4; row++)
            {
                for (int column = 0; column < 3; column++)
                {
                    rec = new Rectangle((left + (column * offset)), (top + (row * offset)), iB1.Width, iB1.Height);
                    e.Graphics.DrawRectangle(pen, rec);
                }
            }
            pen.Dispose();
        }

        private void buttonImages_Click(object sender, EventArgs e)
        {
            drawBlossoms();
        }

        private void buttonEdges_Click(object sender, EventArgs e)
        {
            drawEdges();
        }
    }
}
