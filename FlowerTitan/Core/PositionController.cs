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

        //cutting settings
        private static int leftSpacing = Database.Database.GetInstance().GetProperty("leftSpacing");
        private static int frameWH = Database.Database.GetInstance().GetProperty("frameWH");
        private static int frameSpacing = Database.Database.GetInstance().GetProperty("frameSpacing");
        private static int topSpacing = Database.Database.GetInstance().GetProperty("topSpacing");
        private static int topSpacingNumber = Database.Database.GetInstance().GetProperty("topSpacingNumber");
        private static int numberH = Database.Database.GetInstance().GetProperty("numberH");
        private static int numberW = Database.Database.GetInstance().GetProperty("numberW");
        private static int secondColOffset = Database.Database.GetInstance().GetProperty("secondColOffset");
        private static int thirdColOffset = Database.Database.GetInstance().GetProperty("thirdColOffset");
        private static int secondRowOffset = Database.Database.GetInstance().GetProperty("secondRowOffset");
        private static int thirdRowOffset = Database.Database.GetInstance().GetProperty("thirdRowOffset");
        private static int fourthRowOffset = Database.Database.GetInstance().GetProperty("fourthRowOffset");

        private int secondCol = 0;
        private int thirdCol = 0;
        private int secondRow = 0;
        private int thirdRow = 0;
        private int fourthRow = 0;

        /// <summary>
        /// Constat for converting inches to mm.
        /// </summary>
        private const double DPI_TO_MM = 25.4;
        
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
        /// Updates cutting settings.
        /// </summary>
        /// <param name="newSettings">new cutting settings</param>
        public static void UpdateSettings(int[] newSettings)
        {
            leftSpacing = newSettings[0];
            frameWH = newSettings[1];
            frameSpacing = newSettings[2];
            topSpacing = newSettings[3];
            topSpacingNumber = newSettings[4];
            numberH = newSettings[5];
            numberW = newSettings[6];
            secondColOffset = newSettings[7];
            thirdColOffset = newSettings[8];
            secondRowOffset = newSettings[9];
            thirdRowOffset = newSettings[10];
            fourthRowOffset = newSettings[11];
        }

        /// <summary>
        /// Returns real frame position in millimeters in template
        /// </summary>
        /// <returns>array with real position of frame</returns>
        private int[] getFramePositon()
        {
            int x = 0;
            int y = 0;
            
            switch (FrameNumber)
            {
                case 0: { x = leftSpacing; y = topSpacing; }; break; // 1
                case 1: { x = secondCol; y = topSpacing; }; break; // 2
                case 2: { x = thirdCol; y = topSpacing; }; break; // 3
                case 3: { x = leftSpacing; y = secondRow; }; break; // 4
                case 4: { x = secondCol; y = secondRow; }; break; // 5
                case 5: { x = thirdCol; y = secondRow; }; break; //6
                case 6: { x = leftSpacing; y = thirdRow; }; break; // 7
                case 7: { x = secondCol; y = thirdRow; }; break; // 8
                case 8: { x = thirdCol; y = thirdRow; }; break; //9
                case 9: { x = leftSpacing; y = fourthRow; }; break; // 10
                case 10: { x = secondCol; y = fourthRow; }; break; // 11 
                case 11: { x = thirdCol; y = fourthRow; }; break; // 12
                case 12: { x = thirdCol + 1; y = topSpacingNumber; }; break; // template number
            }

            int[] position = { x, y };
            return position;
        }

        /// <summary>
        /// Calculationg position of frame from real position
        /// </summary>
        public void calculatePositionOfFrame()
        {
            secondCol = leftSpacing + frameWH + frameSpacing + secondColOffset;
            thirdCol = secondCol + frameWH + frameSpacing + thirdColOffset;
            secondRow = topSpacing + frameWH + frameSpacing + secondRowOffset;
            thirdRow = secondRow + frameWH + frameSpacing + thirdRowOffset;
            fourthRow = thirdRow + frameWH + frameSpacing + fourthRowOffset;

            int[] framePosition = getFramePositon();
            int[] tempFramePosition = new int[4];

            if (FrameNumber == 12)
            {
                tempFramePosition[0] = Convert.ToInt32(calculatePixel(framePosition[0]));
                tempFramePosition[1] = Convert.ToInt32(calculatePixel(framePosition[1]));
                tempFramePosition[2] = Convert.ToInt32(calculatePixel(numberW));
                tempFramePosition[3] = Convert.ToInt32(calculatePixel(numberH));
            }
            else
            {
                tempFramePosition[0] = Convert.ToInt32(calculatePixel(framePosition[0]));
                tempFramePosition[1] = Convert.ToInt32(calculatePixel(framePosition[1]));
                tempFramePosition[2] = Convert.ToInt32(calculatePixel(frameWH));
                tempFramePosition[3] = Convert.ToInt32(calculatePixel(frameWH));
            }

            this.FinalFramePosition = tempFramePosition;
        }


        /// <summary>
        /// Calculating pixel from real position 
        /// </summary>
        /// <param name="mm">Positon in mm</param>
        /// <returns>Positon in pixels</returns>
        private double calculatePixel(int mm)
        {
            // pixels = (mm * dpi) / 25.4
            double pixel = (mm * this.Dpi) / DPI_TO_MM;
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
