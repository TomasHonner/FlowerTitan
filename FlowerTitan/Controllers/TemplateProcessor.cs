using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;

namespace FlowerTitan.Controllers
{
    class TemplateProcessor
    {
        /// <summary>
        /// Bitmap with stored template
        /// </summary>
        private Bitmap _template;

        /// <summary>
        /// List of blosssoms bitmaps created from template
        /// </summary>
        private List<Bitmap> _listOfBlossoms;

        /// <summary>
        /// List of blosssoms bitmaps to draw
        /// </summary>
        private List<Bitmap> _listOfBlossomsToDraw;

        /// <summary>
        /// Variable for temporary blossom from listOfBlossoms
        /// </summary>
        private Bitmap _tempBlossom;

        /// <summary>
        /// Temporary variable for images in graycale
        /// </summary>
        private Image<Gray, Byte> _grayscaleTemp;

        /// <summary>
        /// Temporary variable for images with gaussian filter applied 
        /// </summary>
        private Image<Gray, Byte> _gaussianTemp;

        /// <summary>
        /// Temporary variable for images with edge detection filter applied 
        /// </summary>
        private Image<Gray, Byte> _edgeTemp;

        /// <summary>
        /// Temporary variable for images with hough detection applied 
        /// </summary>
        private Image<Gray, Byte> _houghTemp;


        /// <summary>
        /// Instance of FileController class
        /// </summary>
        FileController _fc;

        /// <summary>
        /// Instance of GrayscaleFilter class
        /// </summary>
        Filters.GrayscaleFilter _gsf;

        /// <summary>
        /// Instance of GaussianFilter class
        /// </summary>
        Filters.GaussianFilter _gf;

        /// <summary>
        /// Instance of EdgeFilter class
        /// </summary>
        Filters.EdgeFilter _ef;

        /// <summary>
        /// Instance of HoughTransform class
        /// </summary>
        Filters.HoughTransform _hough;


        /// <summary>
        /// TemplateProcessor constructor
        /// </summary>
        public TemplateProcessor()
        {
            _fc = new FileController();
            _gsf = new Filters.GrayscaleFilter();
            _gf = new Filters.GaussianFilter();
            _ef = new Filters.EdgeFilter();
            _hough = new Filters.HoughTransform();

            _template = null;
            _listOfBlossoms = null;
            _tempBlossom = null;

            _grayscaleTemp = null;
            _gaussianTemp = null;
            _edgeTemp = null;
            _houghTemp = null;

        }

        /// <summary>
        /// Load template from file
        /// </summary>
        public void loadTemplate(string file)
        {
            _fc.loadTemplate(file);
            _template = _fc.STemplate;
        }

        /// <summary>
        /// Create list of bitmaps of blossoms from template
        /// </summary>
        public void createListOfBlossoms()
        {
            _fc.createBlossomsList();
            _listOfBlossoms = _fc.BlossomsList;
            _listOfBlossomsToDraw = _fc.BlossomsList;
        }

        /// <summary>
        /// Change type Bitmap to typy Image from Emgucv
        /// </summary>
        /// <param name="b">Bitmap</param>
        /// <returns>Image in Emgucv format</returns>
        private Image<Bgr, Byte> bitmapToEmgu(Bitmap b)
        {
            Image<Bgr, Byte> r = new Image<Bgr, byte>(b);
            return r;
        }

        /// <summary>
        /// Applied grayscale filter
        /// </summary>
        private void applyGrayFilter()
        {
            _gsf.SourceImage = bitmapToEmgu(_tempBlossom);
            _gsf.applyFilter();
            _grayscaleTemp = _gsf.ResultImage;
        }

        /// <summary>
        /// Applied gaussian filter
        /// </summary>
        private void applyGaussianFilter()
        {
            _gf.SourceImage = _grayscaleTemp;
            _gf.applyFilter();
            _gaussianTemp = _gf.ResultImage;

        }

        /// <summary>
        /// Applied edge detection filter
        /// </summary>
        private void applyEdgeFilter()
        {
            _ef.SourceImage = _gaussianTemp;
            _ef.applyFilter();
            _edgeTemp = _ef.ResultImage;

        }

        /// <summary>
        /// Applied hough transformation filter
        /// </summary>
        private void applyHoughTransform()
        {
            _hough.SourceImage = _edgeTemp;
            _hough.applyTransform();
            _houghTemp = _hough.ResultImage;

        }

        /// <summary>
        /// Starts processing template
        /// </summary>
        public void startProcessing()
        {

        }



        /// <summary>
        /// Getter setter
        /// </summary>
        public List<Bitmap> ListOfBlossomsToDraw
        {
            get { return _listOfBlossomsToDraw; }
            set { _listOfBlossomsToDraw = value; }
        }
    }
}
