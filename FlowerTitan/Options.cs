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
    /// <summary>
    /// FlowerTitan options window/form.
    /// </summary>
    public partial class Options : Form
    {
        private Database.Database db;

        /// <summary>
        /// Default constructor.
        /// </summary>
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
            loadSettings(false);      
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            int[] newSettings = new int[] { (int)numLeftSpacing.Value,
                                            (int)numFrameWH.Value,
                                            (int)numFrameSpacing.Value,
                                            (int)numTopSpacing.Value,
                                            (int)numTopSpacingNumber.Value,
                                            (int)numNumberH.Value,
                                            (int)numNumberW.Value,
                                            (int)numSecondColOffset.Value,
                                            (int)numThirdColOffset.Value,
                                            (int)numSecondRowOffset.Value,
                                            (int)numThirdRowOffset.Value,
                                            (int)numFourthRowOffset.Value
                                            };
            db.SetProperty("leftSpacing", newSettings[0]);
            db.SetProperty("frameWH", newSettings[1]);
            db.SetProperty("frameSpacing", newSettings[2]);
            db.SetProperty("topSpacing", newSettings[3]);
            db.SetProperty("topSpacingNumber", newSettings[4]);
            db.SetProperty("numberH", newSettings[5]);
            db.SetProperty("numberW", newSettings[6]);
            db.SetProperty("secondColOffset", newSettings[7]);
            db.SetProperty("thirdColOffset", newSettings[8]);
            db.SetProperty("secondRowOffset", newSettings[9]);
            db.SetProperty("thirdRowOffset", newSettings[10]);
            db.SetProperty("fourthRowOffset", newSettings[11]);
            db.SetProperty("emptyTreshold", (int)numEmptyTreshold.Value);
            Core.PixelProcessor.EmptyTreshold = (int)numEmptyTreshold.Value;
            Core.PositionController.UpdateSettings(newSettings);

        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            loadSettings(true);
        }

        private void loadSettings(bool reset)
        {
            numLeftSpacing.Value = db.GetProperty("leftSpacing", reset);
            numFrameWH.Value = db.GetProperty("frameWH", reset);
            numFrameSpacing.Value = db.GetProperty("frameSpacing", reset);
            numTopSpacing.Value = db.GetProperty("topSpacing", reset);
            numTopSpacingNumber.Value = db.GetProperty("topSpacingNumber", reset);
            numNumberH.Value = db.GetProperty("numberH", reset);
            numNumberW.Value = db.GetProperty("numberW", reset);
            numSecondColOffset.Value = db.GetProperty("secondColOffset", reset);
            numThirdColOffset.Value = db.GetProperty("thirdColOffset", reset);
            numSecondRowOffset.Value = db.GetProperty("secondRowOffset", reset);
            numThirdRowOffset.Value = db.GetProperty("thirdRowOffset", reset);
            numFourthRowOffset.Value = db.GetProperty("fourthRowOffset", reset);
            numEmptyTreshold.Value = db.GetProperty("emptyTreshold", reset);
        }
    }
}
