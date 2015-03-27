using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace FlowerTitan.MeasurementLines
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
        /// Line's color.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Line's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Initialization.
        /// </summary>
        public Line(Color color)
        {
            Points = new List<Point>();
            Color = color;
        }
    }
}
