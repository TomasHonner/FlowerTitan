using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace FlowerTitan.Core
{
    class FileController
    {
        /// <summary>
        /// Store loaded template
        /// </summary>
        private Bitmap sTemplate;

        /// <summary>
        /// Store blossoms bitmaps in a list
        /// </summary>
        private List<Bitmap> blossomsList;

        /// <summary>
        /// Store DPI of image old image
        /// </summary>
        private int dpi;
        
        /// <summary>
        /// FileController constructor
        /// </summary>
        public FileController()
        {
            sTemplate = null;
            blossomsList = null;
            dpi = 0;
            
        }

        /// <summary>
        /// FileController constructor 
        /// </summary>
        /// <param name="imagePath">Path to image</param>
        public FileController(string imagePath)
        {
            sTemplate = null;
            blossomsList = null;
            dpi = 0;

        }

        /// <summary>
        /// Load template from file
        /// </summary>
        /// <param name="file">Path to file</param>
        /// <param name="mainWindow">Instance of MainWindow</param>
        public void loadTemplate(string file, MainWindow mainWindow)
        {
            Bitmap bmp = new Bitmap(file);
            Dpi = Convert.ToInt32(bmp.HorizontalResolution);
            STemplate = bmp;
            Action stateChanged = new Action(() =>
            {
                mainWindow.toolStripProgressBar.Value = 30;
            });
            mainWindow.Invoke(stateChanged);
        }

        /// <summary>
        /// Create list of blossoms bitmaps from template
        /// </summary>
        /// <returns>List of bitmaps with blossoms</returns>
        public void createBlossomsList()
        {
            List<Bitmap> temp = new List<Bitmap>();
            
            for (int i = 0; i < 13; i++)
            {
                Rectangle cloneRect = getBlossomPosition(i, Dpi);
                System.Drawing.Imaging.PixelFormat format = sTemplate.PixelFormat;
                Bitmap cloneBitmap = sTemplate.Clone(cloneRect, format);
                temp.Add(cloneBitmap);
            }
            blossomsList =  temp;

        }

        /// <summary>
        /// Return position of each blossom on template
        /// </summary>
        /// <param name="i">Identifier for window with blossom</param>
        /// <param name="dpi">DPI resolution of Image</param>
        /// <returns>Rectangle with position</returns>
        private Rectangle getBlossomPosition(int i, int dpi)
        {
            PositionController p = new PositionController(dpi,i);
            p.calculatePositionOfFrame();
            int[] r = p.FinalFramePosition;

            return new Rectangle(r[0], r[1], r[2], r[3]);
        }


        /// <summary>
        /// Getter and setter
        /// </summary>
        public Bitmap STemplate
        {
            get { return sTemplate; }
            set { sTemplate = value; }
        }

        /// <summary>
        /// Getter and setter for BlossomsList
        /// </summary>
        public List<Bitmap> BlossomsList
        {
            get { return blossomsList; }
            set { blossomsList = value; }
        }

        /// <summary>
        /// Getter and setter for Dpi
        /// </summary>
        public int Dpi
        {
            get { return dpi; }
            set { dpi = value; }
        }

    }
}