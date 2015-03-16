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
        /// FileController constructor
        /// </summary>
        public FileController()
        {
            _sTemplate = null;
            _blossomsList = null;
        }

        /// <summary>
        /// Load template from file
        /// </summary>
        public void loadTemplate()
        {
            try
            {
                OpenFileDialog d = new OpenFileDialog();
                d.Title = "Open template file";
                d.Filter = " jpg files (.jpg)| *.jpg|bmp files (.bmp)| *.bmp| png files (.png)| *.png";
                d.Multiselect = false;

                if(d.ShowDialog() == DialogResult.OK)
                {
                    Bitmap bmp = new Bitmap(d.FileName);
                    STemplate = bmp;
                }
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

                for (int i = 0; i < 6; i++)
                {
                    Rectangle cloneRect = getBlossomPosition(i);
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
        /// <returns>Rectangle with position</returns>
        private Rectangle getBlossomPosition(int i)
        {
            int x = 0, y = 0, w = 0, h = 0;

            switch (i)
            {
                case 0: { x = 1790; y = 1540; w = 80; h = 379; }; break;
            }

            return new Rectangle(x, y, w, h);
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

    }
}