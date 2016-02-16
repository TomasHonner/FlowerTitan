using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerTitan.Core
{
    class PositionController
    {

        /// <summary>
        /// Store image DPI
        /// </summary>
        private int dpi;

        /// <summary>
        /// Strore number of frame in template
        /// </summary>
        private int frameNumber;

        /// <summary>
        /// Store calculated frame postion in template
        /// </summary>
        private int[] finalFramePosition;

        
        /// <summary>
        /// PositionController constructor
        /// </summary>
        public PositionController()
        {

        }

        /// <summary>
        /// PositionController constructor
        /// </summary>
        /// <param name="dpi">DPI of image</param>
        /// <param name="frameNumber">Number of frame in template</param>
        public PositionController(int dpi, int frameNumber)
        {
            this.dpi = dpi;
            this.frameNumber = frameNumber;
            this.finalFramePosition = null;

        }

        /// <summary>
        /// Returns real frame position in millimeters in old template
        /// </summary>
        /// <returns>array with real position of frame</returns>
        private double[] getFramePositon()
        {
            double x = 0.0;
            double y = 0.0;

            switch(FrameNumber)
            {
                case 0 : { x = 16.0; y = 40.0; }; break; // 1
                case 1 : { x = 78.0; y = 40.0; }; break; // 2
                case 2:  { x = 140.0; y = 40.0; }; break; // 3
                case 3 : { x = 16.0; y = 100.0; }; break; // 4
                case 4 : { x = 78.0; y = 100.0; }; break; // 5
                case 5 : { x = 140.0; y = 100.0; }; break; //6
                case 6 : { x = 16.0; y = 162.0; }; break; // 7
                case 7 : { x = 78.0; y = 162.0; }; break; // 8
                case 8 : { x = 140.0; y = 162.0; }; break; //9
                case 9 : { x = 16.0; y = 224.0; }; break; // 10
                case 10 : { x = 78.0; y = 224.0; }; break; // 11 
                case 11 : { x = 140.0; y = 224.0; }; break; // 12
                case 12: { x = 138.0; y = 20.0; }; break; // template number
            }

            double[] position = { x, y };
            return position;
        }

        /// <summary>
        /// Returns real frame position in millimeters in new template
        /// </summary>
        /// <returns>array with real position of frame</returns>
        private double[] getFramePositonNewTemplate()
        {
            double x = 0.0;
            double y = 0.0;

            switch (FrameNumber)
            {
                case 0: { x = 13.0; y = 35.0; }; break; // 1
                case 1: { x = 78.0; y = 35.0; }; break; // 2
                case 2: { x = 142.0; y = 35.0; }; break; // 3
                case 3: { x = 13.0; y = 100.0; }; break; // 4
                case 4: { x = 78.0; y = 100.0; }; break; // 5
                case 5: { x = 142.0; y = 100.0; }; break; //6
                case 6: { x = 13.0; y = 163.0; }; break; // 7
                case 7: { x = 78.0; y = 163.0; }; break; // 8
                case 8: { x = 142.0; y = 163.0; }; break; //9
                case 9: { x = 13.0; y = 228.0; }; break; // 10
                case 10: { x = 78.0; y = 228.0; }; break; // 11 
                case 11: { x = 142.0; y = 228.0; }; break; // 12
                case 12: { x = 142.0; y = 13.0; }; break; // template number
            }

            double[] position = { x, y };
            return position;
        }

        /// <summary>
        /// Calculationg position of frame from real position
        /// </summary>
        public void calculatePositionOfFrame()
        {
            double[] framePosition = getFramePositonNewTemplate();
            int[] tempFramePosition = new int[4];

            double width = 56.0; 
            double heigth = 56.0;
            double numberFrameHeigth = 11.0;

            if (FrameNumber == 12)
            {
                tempFramePosition[0] = Convert.ToInt32(calculatePixel(framePosition[0]));
                tempFramePosition[1] = Convert.ToInt32(calculatePixel(framePosition[1]));
                tempFramePosition[2] = Convert.ToInt32(calculatePixel(width));
                tempFramePosition[3] = Convert.ToInt32(calculatePixel(numberFrameHeigth));
            }
            else
            {
                tempFramePosition[0] = Convert.ToInt32(calculatePixel(framePosition[0]));
                tempFramePosition[1] = Convert.ToInt32(calculatePixel(framePosition[1]));
                tempFramePosition[2] = Convert.ToInt32(calculatePixel(width));
                tempFramePosition[3] = Convert.ToInt32(calculatePixel(heigth));
            }

            this.FinalFramePosition = tempFramePosition;
        }


        /// <summary>
        /// Calculating pixel from real position 
        /// </summary>
        /// <param name="mm">Positon in mm</param>
        /// <returns>Positon in pixels</returns>
        private double calculatePixel(double mm)
        {
            // pixels = (mm * dpi) / 25.4
            double pixel = (mm * this.Dpi) / 25.4;
            return pixel;
        }

        /// <summary>
        /// Getter and setter for dpi
        /// </summary>
        public int Dpi
        {
            get { return dpi; }
            set { dpi = value; }
        }

        /// <summary>
        /// Getter and setter for frameNumber
        /// </summary>
        public int FrameNumber
        {
            get { return frameNumber; }
            set { frameNumber = value; }
        }

        /// <summary>
        /// Getter and setter for framePosition
        /// </summary>
        public int[] FinalFramePosition
        {
            get { return finalFramePosition; }
            set { finalFramePosition = value; }
        }

    }
}
