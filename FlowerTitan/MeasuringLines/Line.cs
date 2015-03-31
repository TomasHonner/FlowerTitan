using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace FlowerTitan.MeasuringLines
{
    /// <summary>
    /// Particular line with its points.
    /// </summary>
    public class Line
    {
        /// <summary>
        /// Holds all line's points.
        /// </summary>
        public List<Point> Points { get; set; }

        /// <summary>
        /// Initialization.
        /// </summary>
        public Line()
        {
            Points = new List<Point>();
        }
    }
}
