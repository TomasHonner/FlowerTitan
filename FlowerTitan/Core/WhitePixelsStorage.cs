using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FlowerTitan.Core
{
    class WhitePixelsStorage
    {
        /// <summary>
        /// Image number
        /// </summary>
        private int imageNumber;

        /// <summary>
        /// Boolean for is image empty
        /// </summary>
        private bool isEmpty;

        /// <summary>
        /// List of best white points matches 
        /// </summary>
        private List<Point> matchedPixels;

        /// <summary>
        /// List of all white points
        /// </summary>
        private List<Point> allWhitePixels;

        /// <summary>
        /// Class WhitePointsStorage constructor
        /// </summary>
        /// <param name="imageNumber"></param>
        public WhitePixelsStorage()
        {
            ImageNumber = imageNumber;

            this.matchedPixels = new List<Point>();
            this.allWhitePixels = new List<Point>();
            this.isEmpty = false;
        }

        /// <summary>
        /// Add white point to the list
        /// </summary>
        /// <param name="p">Image pixel</param>
        private void addWhitePoint(Point p)
        {
            AllWhitePixels.Add(p);
        }

        /// <summary>
        /// Add best matched pixel to the list
        /// </summary>
        /// <param name="p">Image pixel</param>
        private void addMatchedPoint(Point p)
        {
            matchedPixels.Add(p);
        }

        /// <summary>
        /// Clear list of white points
        /// </summary>
        private void clearWhitePointsList()
        {
            AllWhitePixels.Clear();
        }

        /// <summary>
        /// Clear list of matched points
        /// </summary>
        private void clearMatchedList()
        {
            MatchedPixels.Clear();
        }

        /// <summary>
        /// Getter Setter for ImageNumber
        /// </summary>
        public int ImageNumber
        {
            get { return imageNumber; }
            set { imageNumber = value; }
        }

        /// <summary>
        /// Getter Setter for BestMatchedPoints
        /// </summary>
        public List<Point> MatchedPixels
        {
            get { return matchedPixels; }
            set { matchedPixels = value; }
        }

        /// <summary>
        /// Getter Setter for AllWhitePoints
        /// </summary>
        public List<Point> AllWhitePixels
        {
            get { return allWhitePixels; }
            set { allWhitePixels = value; }
        }

        /// <summary>
        /// Getter Setter for IsEmpty
        /// </summary>
        public bool IsEmpty
        {
            get { return isEmpty; }
            set { isEmpty = value; }
        }

    }
}
