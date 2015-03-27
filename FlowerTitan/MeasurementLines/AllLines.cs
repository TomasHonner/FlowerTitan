using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace FlowerTitan.MeasurementLines
{
    /// <summary>
    /// Holds all lines on an image.
    /// </summary>
    public class AllLines
    {
        /// <summary>
        /// Holds all image's lines.
        /// </summary>
        public List<Line> Lines { get; set; }

        /// <summary>
        /// Line's and point's thickness.
        /// </summary>
        private float thickness;
        public float Thickness { get { return this.thickness; } }

        /// <summary>
        /// Pointer's size, size of an ellipse,circle which is used for line's moving.
        /// </summary>
        private Size size;
        public Size Size { get { return this.size; } }

        /// <summary>
        /// Counts how many lines were created. It is used for an intial line's name.
        /// </summary>
        public int LinesCounter { get; set; }

        /// <summary>
        /// Initialization.
        /// </summary>
        public AllLines()
        {
            Lines = new List<Line>();
            LinesCounter = 0;
            //default values
            thickness = 3f;
            size = new Size(7, 7);
        }
    }
}
