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

        public MainWindow()
        {
            _tp = new Controllers.TemplateProcessor();
            InitializeComponent();
            //calls function in partial class MeasurementLines.cs
            prepareMeasuringLines(iB1);
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
            prepareForNewTemplate();
            // TODO: dialog result - close button
            _tp.loadTemplate();
            MessageBox.Show("Template loaded");
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            _tp.startProcessing();
            //mock start - get measuring lines and do image processing
            //for every image getMeasuring lines again - it returns deep copy of object
            //every image has to have the same count of lines and points as the very first one
            iB2.Tag = getMeasuringLines();//currently it is the same as on the first image
            iB3.Tag = getMeasuringLines();
            iB4.Tag = getMeasuringLines();
            //mock end
            //add event handlers to the other images
            //call ONLY after adding measuring lines to image tag
            //it has to be called only once
            addHandlersToImage(iB2);
            addHandlersToImage(iB3);
            addHandlersToImage(iB4);
        }
    }
}
