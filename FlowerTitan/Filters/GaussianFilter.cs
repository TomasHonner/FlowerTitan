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
    class GaussianFilter
    {
        /// <summary>
        /// Variable for source image
        /// </summary>
        private Image<Gray, Byte> _sourceImage;

        /// <summary>
        /// Variable for converted image
        /// </summary>
        private Image<Gray, Byte> _resultImage;


        /// <summary>
        /// GaussianFilter constructor
        /// </summary>
        public GaussianFilter()
        {
          
        }

        /// <summary>
        /// Aply filter on image
        /// </summary>
        public void applyFilter()
        {
            try
            {
                _resultImage = _sourceImage.SmoothGaussian(1);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        /// <summary>
        /// Getter setter
        /// </summary>
        public Image<Gray, Byte> SourceImage
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
