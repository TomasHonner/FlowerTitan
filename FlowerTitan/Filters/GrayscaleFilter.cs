using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;

namespace FlowerTitan.Filters
{
    class GrayscaleFilter
    {
        /// <summary>
        /// Variable for source image
        /// </summary>
        private Image<Bgr, Byte> _sourceImage;

        /// <summary>
        /// Variable for converted image
        /// </summary>
        private Image<Gray, Byte> _resultImage;

        /// <summary>
        /// GrayscaleFilter constructor
        /// </summary>
        public GrayscaleFilter()
        {

        }

        /// <summary>
        /// Apply filter on image
        /// </summary>
        public void applyFilter()
        {
            try
            {
                _resultImage = _sourceImage.Convert<Gray, Byte>();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        /// <summary>
        /// Getter setter
        /// </summary>
        public Image<Bgr, Byte> SourceImage
        {
            get { return _sourceImage; }
            set { _sourceImage = value; }
        }

        /// <summary>
        /// Getter setter
        /// </summary>
        public Image<Gray, Byte> ResultImage
        {
            get { return _resultImage; }
            set { _resultImage = value; }
        }
    }
}
