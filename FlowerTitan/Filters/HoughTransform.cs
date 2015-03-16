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
    class HoughTransform
    {
        /// <summary>
        /// Variable for source image
        /// </summary>
        private Image<Gray, Byte> _sourceImage;

        /// <summary>
        /// Variable for image with inserted lines
        /// </summary>
        private Image<Gray, Byte> _linesImage;

        /// <summary>
        /// Variable for converted image
        /// </summary>
        private Image<Gray, Byte> _resultImage;


        /// <summary>
        /// HoughTransform constructor
        /// </summary>
        public HoughTransform()
        {

        }

        /// <summary>
        /// Apply Hough lines detector
        /// </summary>
        public void applyTransform()
        {
            try
            {
                _linesImage = _sourceImage.CopyBlank();
                LineSegment2D[] lines = _sourceImage.HoughLinesBinary(1, Math.PI / 180, 1, 10, 5)[0];
                foreach (LineSegment2D line in lines)
                {
                    _linesImage.Draw(line, new Gray(200), 5);
                }
                _resultImage = _linesImage;
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
