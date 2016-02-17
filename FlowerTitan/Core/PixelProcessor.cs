using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Windows.Forms;

namespace FlowerTitan.Core
{
    class PixelProcessor
    {
        /// <summary>
        /// Instance of WhitePixelStorage
        /// </summary>
        private WhitePixelsStorage wps;

        /// <summary>
        /// List of WhitePixelStorage objects. One object is one image from the template
        /// </summary>
        private List<WhitePixelsStorage> whitePixelListFinal;

        /// <summary>
        /// Empty image treshold.
        /// </summary>
        private static int emptyTreshold = Database.Database.GetInstance().GetProperty("emptyTreshold");
        public static int EmptyTreshold { set { emptyTreshold = value; } }

       /// <summary>
        /// Start processing array of lines from MainWindow
       /// </summary>
       /// <param name="lines">Array of lines draw by user</param>
       /// <returns>Array of detected lines</returns>
        public MeasuringLines.Line[][] startProcessing(MeasuringLines.Line[][] lines)
        {
            if (lines.Length == 1)
            {
                return processEveryImage(lines, true);
            }
            else
            {
                return processEveryImage(lines, false);
            }
        }

        /// <summary>
        /// Processes every image with default or itself lines according to the isFirstProcessing parameter.
        /// </summary>
        /// <param name="lines">reference lines from first or all images</param>
        /// <param name="isFirstProcessing">determines whether lines are the same for all images or not</param>
        /// <returns>Array of detected lines</returns>
        private MeasuringLines.Line[][] processEveryImage(MeasuringLines.Line[][] lines, bool isFirstProcessing)
        {
            List<MeasuringLines.Line[]> allLines = new List<MeasuringLines.Line[]>();
            for (int i = 1; i < whitePixelListFinal.Count; i++)
            {
                if (!whitePixelListFinal[i - 1].IsEmpty)
                {
                    List<MeasuringLines.Line> imgLines = new List<MeasuringLines.Line>();
                    if (isFirstProcessing)
                    {
                        foreach (MeasuringLines.Line line in lines[0])
                        {
                            processEveryLine(i, imgLines, line);
                        }
                    }
                    else
                    {
                        foreach (MeasuringLines.Line line in lines[i - 1])
                        {
                            processEveryLine(i, imgLines, line);
                        }
                    }
                    allLines.Add(imgLines.ToArray());
                }
                else
                {
                    break;
                }
            }
            return allLines.ToArray();
        }

        /// <summary>
        /// Processes every image's line.
        /// </summary>
        /// <param name="i">image's index</param>
        /// <param name="imgLines">result lines</param>
        /// <param name="line">processed line</param>
        private void processEveryLine(int i, List<MeasuringLines.Line> imgLines, MeasuringLines.Line line)
        {
            List<Point> pointList = line.Points;
            List<Point> rList = new List<Point>();
            foreach (Point p in pointList)
            {
                rList.Add(findNearestPixel(p, i - 1));
            }
            MeasuringLines.Line l = new MeasuringLines.Line();
            l.Points = rList;
            imgLines.Add(l);
        }

        /// <summary>
        /// Find near pixel from edge detection for defined point and insert in Storage
        /// </summary>
        /// <param name="p">Point for near edge search</param>
        /// <param name="index">index of right WhitePixelStoreage in WhitePixelListFinal</param>
        /// <returns>Nearest point to pixel</returns>
        private Point findNearestPixel(Point p, int index)
        {
            Point r = new Point(0, 0);
            WhitePixelsStorage wps = WhitePixelListFinal[index];
            
            List<Point> tempList = wps.AllWhitePixels;
            double minLengthPx = Double.MaxValue;
            foreach (Point pp in tempList)
            {
                double lengthPx;
                float dX = p.X - pp.X;
                float dY = p.Y - pp.Y;
                lengthPx = Math.Sqrt((dX * dX) + (dY * dY));
                if (lengthPx < minLengthPx)
                {
                    r = pp;
                    minLengthPx = lengthPx;
                }
            }
            return r;
        }

        /// <summary>
        /// Find all white pixels in all images from the list
        /// </summary>
        /// <param name="l">List of images with edge detection</param>
        /// <param name="mainWindow">Instance of MainWindow</param>
        public void proccessImageList(List<Image<Gray, Byte>> l, MainWindow mainWindow)
        {
            this.whitePixelListFinal = new List<WhitePixelsStorage>();
            int n = 0;
            if (l.Count > 0)
            {
                foreach(Image<Gray, Byte> i in l)
                {
                    wps = new WhitePixelsStorage();
                    wps.ImageNumber = n;
                    wps.AllWhitePixels = proccessImage(i);
                    n++;
                    if (wps.AllWhitePixels.Count() < emptyTreshold)
                    {
                        wps.IsEmpty = true;
                        WhitePixelListFinal.Add(wps);
                    }
                    else
                    {
                        WhitePixelListFinal.Add(wps);
                    }
                    Action stateChanged = new Action(() =>
                    {
                        mainWindow.toolStripProgressBar.Value += 5;
                    });
                    mainWindow.Invoke(stateChanged);
                }
            }
            else
            {
                MessageBox.Show("Images in the list not found");
            }
        }

        /// <summary>
        /// Find all white points (edges) in image
        /// </summary>
        /// <param name="b">Image to search</param>
        /// <returns>Position of white pixel</returns>
        private List<Point> proccessImage(Image<Gray, Byte> b)
        {
            Bitmap testImage = b.ToBitmap();
            int width = testImage.Width;
            int heigth = testImage.Height;

            List<Point> r = new List<Point>();

            for(int x = 0; x < width; x++)
            {
                for(int y = 0; y < heigth; y++)
                {
                    Color c = testImage.GetPixel(x, y);

                    if(c.ToArgb() == Color.White.ToArgb())
                    {
                        r.Add(new Point(x,y));
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            return r;
        }
        
        
        
        /// <summary>
        /// Getter Setter for WhitePixelListFinal
        /// </summary>
        internal List<WhitePixelsStorage> WhitePixelListFinal
        {
            get { return whitePixelListFinal; }
            set { whitePixelListFinal = value; }
        }
    }
}
