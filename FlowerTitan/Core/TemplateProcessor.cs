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

namespace FlowerTitan.Core
{
    class TemplateProcessor
    {
        /// <summary>
        /// Bitmap with stored template
        /// </summary>
        private Bitmap template;

        /// <summary>
        /// List of blosssoms bitmaps created from template
        /// </summary>
        private List<Bitmap> listOfBlossoms;

        /// <summary>
        /// List of blosssoms bitmaps to draw
        /// </summary>
        private List<Image<Bgr, Byte>> listOfBlossomsToDraw;

        /// <summary>
        /// List of Blossoms after edge detection
        /// </summary>
        private List<Image<Gray, Byte>> listOfBlossomsWithEdges;

        /// <summary>
        /// List of images from edge detector to draw
        /// </summary>
        private List<Image<Gray, Byte>> listOfEdgesToDraw;

        /// <summary>
        /// List of blossoms in grayscale
        /// </summary>
        private List<Image<Gray, Byte>> listOfGrayscaleBlossoms;

        /// <summary>
        /// Variable for temporary blossom from listOfBlossoms
        /// </summary>
        private Bitmap tempBlossom;

        /// <summary>
        /// Variable for temporary list of blossoms
        /// </summary>
        private List<Image<Bgr, Byte>> tempListOfBlossoms;



        /// <summary>
        /// Temporary variable for images in graycale
        /// </summary>
        private Image<Gray, Byte> grayscaleTemp;

        /// <summary>
        /// Temporary variable for images with gaussian filter applied 
        /// </summary>
        private Image<Gray, Byte> gaussianTemp;

        /// <summary>
        /// Temporary variable for images with edge detection filter applied 
        /// </summary>
        private Image<Gray, Byte> edgeTemp;

        /// <summary>
        /// Temporary variable for images with hough detection applied 
        /// </summary>
        private Image<Gray, Byte> houghTemp;


        /// <summary>
        /// Instance of FileController class
        /// </summary>
        FileController fc;

        /// <summary>
        /// Instance of GrayscaleFilter class
        /// </summary>
        Filters.GrayscaleFilter gsf;

        /// <summary>
        /// Instance of GaussianFilter class
        /// </summary>
        Filters.GaussianFilter gf;

        /// <summary>
        /// Instance of EdgeFilter class
        /// </summary>
        Filters.EdgeFilter ef;

        /// <summary>
        /// Instance of HoughTransform class
        /// </summary>
        Filters.HoughTransform hough;

        /// <summary>
        /// Instance of PixelProcessor class
        /// </summary>
        PixelProcessor pp;

        /// <summary>
        /// Stores new Dpi value after image resize.
        /// </summary>
        private double newDpi = 0;


        /// <summary>
        /// TemplateProcessor constructor
        /// </summary>
        public TemplateProcessor()
        {
            fc = new FileController();
            gsf = new Filters.GrayscaleFilter();
            gf = new Filters.GaussianFilter();
            ef = new Filters.EdgeFilter();
            hough = new Filters.HoughTransform();
            pp = new PixelProcessor();

            template = null;
            listOfBlossoms = null;
            listOfBlossomsWithEdges = null;
            listOfEdgesToDraw = null;
            tempBlossom = null;

        }

        /// <summary>
        /// Load template from file
        /// </summary>
        /// <param name="file">Image file</param>
        /// <param name="mainWindow">Instance of MainWindow</param>
        public void loadTemplate(string file, MainWindow mainWindow)
        {
            Bitmap tmp;
            fc.loadTemplate(file, mainWindow);
            template = fc.STemplate;
        }

        /// <summary>
        /// Create list of bitmaps of blossoms from template
        /// </summary>
        /// <param name="width">Width of created image</param>
        /// <param name="height">Height of created image</param>
        /// <param name="mainWindow">Instance of MainWindow</param>
        public void createListOfBlossoms(int width, int height, MainWindow mainWindow)
        {
            fc.createBlossomsList();
            Action stateChanged = new Action(() =>
            {
                mainWindow.toolStripProgressBar.Value = 70;
            });
            mainWindow.Invoke(stateChanged);
            listOfBlossoms = fc.BlossomsList;
            convertListOfBlossoms(width, height);
            listOfBlossomsToDraw = tempListOfBlossoms;
            Action stateChanged2 = new Action(() =>
            {
                mainWindow.toolStripProgressBar.Value = 100;
            });
            mainWindow.Invoke(stateChanged2);
        }

        /// <summary>
        /// Convert list of bitmaps to Emgu.CV.Image format
        /// </summary>
        /// <param name="width">>Width of created image</param>
        /// <param name="height">Height of created image</param>
        public void convertListOfBlossoms(int width, int height)
        {
            Bitmap ima;
            Image<Bgr, Byte> imb;
            Image<Bgr, Byte> imc;
            tempListOfBlossoms = new List<Image<Bgr,byte>>();

           
                for (int i = 0; i < 13; i++)
                {
                    ima = listOfBlossoms.ElementAt(i);
                    imb = bitmapToEmgu(ima);
                    imc = resizeImage(imb, width, height);
                    tempListOfBlossoms.Add(imc);
                }
                //DPI rescale formula [DPI/(original size/new size)])
                newDpi = listOfBlossoms[0].HorizontalResolution / ((double)listOfBlossoms[0].Width / (double)tempListOfBlossoms[0].Width);
            
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
        /// Resize image to form window
        /// </summary>
        /// <param name="i">Image</param>
        /// <param name="width">>Width of created image</param>
        /// <param name="height">Height of created image</param>
        /// <returns>Resized image</returns>
        private Image<Bgr, Byte> resizeImage(Image<Bgr, Byte> i, int width, int height)
        {
            Image<Bgr, Byte> r = i.Resize(width, height, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR, true);
            return r;
        }

        /// <summary>
        /// Applied grayscale filter
        /// </summary>
        private void applyGrayFilter()
        {
            List<Image<Bgr, Byte>> temp = ListOfBlossomsToDraw;
            ListOfGrayscaleBlossoms = new List<Image<Gray, Byte>>();
            foreach(Image<Bgr, Byte> image in temp)
            {
                gsf.SourceImage = image;
                gsf.applyFilter();
                ListOfGrayscaleBlossoms.Add(gsf.ResultImage);
            }  
        }

        /// <summary>
        /// Applied gaussian filter
        /// </summary>
        private void applyGaussianFilter()
        {
            gf.SourceImage = grayscaleTemp;
            gf.applyFilter();
            gaussianTemp = gf.ResultImage;

        }

        /// <summary>
        /// Applied edge detection filter
        /// </summary>
        /// <param name="treshold">Treshold for edge detection</param>
        /// <param name="tresholdLinking">TresholdLinking for edge detection</param>
        private void applyEdgeFilter(int treshold, int tresholdLinking)
        {
            List<Image<Gray, Byte>> temp = ListOfGrayscaleBlossoms;
            ListOfBlossomsWithEdges = new List<Image<Gray, Byte>>();
            ListOfEdgesToDraw = new List<Image<Gray, Byte>>();
            foreach (Image<Gray, Byte> image in temp)
            {
                ef.SourceImage = image;
                ef.applyFilter(treshold, tresholdLinking);
                ListOfBlossomsWithEdges.Add(ef.ResultImage);
                ListOfEdgesToDraw.Add(ef.ResultImage);
            }  

        }

        /// <summary>
        /// Applied hough transformation filter
        /// </summary>
        private void applyHoughTransform()
        {
            hough.SourceImage = edgeTemp;
            hough.applyTransform();
            houghTemp = hough.ResultImage;

        }

        /// <summary>
        /// Starts processing template
        /// </summary>
        /// <param name="treshold">Treshold</param>
        /// <param name="tresholdLinking">TresholdLinking</param>
        /// <param name="referenceLines">Array of lines draw  by user</param>
        /// <param name="mainWindow">Instance of MainWindow</param>
        /// <returns>Array of lines after image processing to draw</returns>
        public MeasuringLines.Line[][] startProcessing(int treshold, int tresholdLinking, MeasuringLines.Line[][] referenceLines, MainWindow mainWindow)
        {
            applyGrayFilter();
            Action stateChanged1 = new Action(() =>
            {
                mainWindow.toolStripProgressBar.Value = 10;
            });
            mainWindow.Invoke(stateChanged1);
            applyEdgeFilter(treshold, tresholdLinking);
            Action stateChanged3 = new Action(() =>
            {
                mainWindow.toolStripProgressBar.Value = 30;
            });
            mainWindow.Invoke(stateChanged3);
            pp.proccessImageList(ListOfBlossomsWithEdges, mainWindow);
            MeasuringLines.Line[][] allLines = pp.startProcessing(referenceLines);
            Action stateChanged2 = new Action(() =>
            {
                mainWindow.toolStripProgressBar.Value = 100;
            });
            mainWindow.Invoke(stateChanged2);
            return allLines;
        }

        /// <summary>
        /// Returns DPI of image
        /// </summary>
        /// <returns>DPI</returns>
        public double getDpi()
        {
            return newDpi;
        }



        /// <summary>
        /// Getter setter ListOfBlossomsToDraw
        /// </summary>
        public List<Image<Bgr, Byte>> ListOfBlossomsToDraw
        {
            get { return listOfBlossomsToDraw; }
            set { listOfBlossomsToDraw = value; }
        }

        /// <summary>
        /// Getter Setter ListOfEdgesToDraw
        /// </summary>
        public List<Image<Gray, Byte>> ListOfEdgesToDraw
        {
            get { return listOfEdgesToDraw; }
            set { listOfEdgesToDraw = value; }
        }

        /// <summary>
        /// Getter Setter ListOfBlossomsWithEdges
        /// </summary>
        public List<Image<Gray, Byte>> ListOfBlossomsWithEdges
        {
            get { return listOfBlossomsWithEdges; }
            set { listOfBlossomsWithEdges = value; }
        }

        /// <summary>
        /// Getter Setter ListOfGrayscaleBlossoms
        /// </summary>
        public List<Image<Gray, Byte>> ListOfGrayscaleBlossoms
        {
            get { return listOfGrayscaleBlossoms; }
            set { listOfGrayscaleBlossoms = value; }
        }
    }
}
