using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FlowerTitan.MeasuringLines;
using System.Drawing;

namespace FlowerTitan.LengthConverter
{
    /// <summary>
    /// Singleton class which handles all lines' length coversions.
    /// </summary>
    class LengthConverter
    {
        //singleton instance
        private static LengthConverter lengthConverter = null;
        //dpi to mm convert ratio
        private const double DPI_TO_MM = 25.4;

        /// <summary>
        /// Private constructor.
        /// </summary>
        private LengthConverter() { }

        /// <summary>
        /// Returns singleton instance.
        /// </summary>
        /// <returns>LengthConverter instance.</returns>
        public static LengthConverter GetInstance()
        {
            if (lengthConverter == null)
            {
                lengthConverter = new LengthConverter();
            }
            return lengthConverter;
        }

        /// <summary>
        /// Converts line length to mm.
        /// </summary>
        /// <param name="line">line to convert</param>
        /// <param name="scale">converting scale</param>
        /// <returns></returns>
        public double ConvertLineLengthToMM(Line line, double scale)
        {
            if (line.Points.Count == 1) return 0f;
            int to = line.Points.Count - 1;
            double lengthPx = 0;
            for (int i = 0; i < to; i++)
            {
                int dX = line.Points[i].X - line.Points[i + 1].X;
                int dY = line.Points[i].Y - line.Points[i + 1].Y;
                lengthPx += Math.Sqrt((dX * dX) + (dY * dY));
            }
            double a = convertPxToMm(lengthPx, scale);
            return a;
        }

        private double convertPxToMm(double lengthPx, double scale)
        {
            return (lengthPx * DPI_TO_MM) / scale;
        }
    }
}
