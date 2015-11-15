using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace FlowerTitan.Controllers
{
    class FileController
    {
        /// <summary>
        /// Store loaded template
        /// </summary>
        private Bitmap _sTemplate;

        /// <summary>
        /// Store blossoms bitmaps in a list
        /// </summary>
        private List<Bitmap> _blossomsList;

        /// <summary>
        /// Store DPI of image
        /// </summary>
        private int _dpi;

        
        /// <summary>
        /// FileController constructor
        /// </summary>
        public FileController()
        {
            _sTemplate = null;
            _blossomsList = null;
            _dpi = 0;
            
        }

        /// <summary>
        /// FileController constructor 
        /// </summary>
        /// <param name="imagePath">Path to image</param>
        public FileController(string imagePath)
        {
            _sTemplate = null;
            _blossomsList = null;
            _dpi = 0;

        }

        /// <summary>
        /// Load template from file
        /// </summary>
        public void loadTemplate(string file)
        {
            try
            {
                Bitmap bmp = new Bitmap(file);
                Dpi = Convert.ToInt32(bmp.HorizontalResolution);
                STemplate = bmp;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        /// <summary>
        /// Create list of blossoms bitmaps from template
        /// </summary>
        /// <returns>List of bitmaps with blossoms</returns>
        public void createBlossomsList()
        {
            List<Bitmap> temp = new List<Bitmap>();
            try
            {

                for (int i = 0; i < 13; i++)
                {
                    Rectangle cloneRect = getBlossomPosition(i, Dpi);
                    System.Drawing.Imaging.PixelFormat format = _sTemplate.PixelFormat;
                    Bitmap cloneBitmap = _sTemplate.Clone(cloneRect, format);
                    temp.Add(cloneBitmap);
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            _blossomsList =  temp;

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
            get { return _sTemplate; }
            set { _sTemplate = value; }
        }

        /// <summary>
        /// Getter and setter
        /// </summary>
        public List<Bitmap> BlossomsList
        {
            get { return _blossomsList; }
            set { _blossomsList = value; }
        }

        /// <summary>
        /// Getter and setter
        /// </summary>
        public int Dpi
        {
            get { return _dpi; }
            set { _dpi = value; }
        }

    }
}