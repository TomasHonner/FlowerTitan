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
        public float Thickness { get; set; }

        /// <summary>
        /// Pointer's size, size of an ellipse,circle which is used for line's moving.
        /// </summary>
        public Size Size { get; set; }

        /// <summary>
        /// Counts how many lines were created. It is used for an intial line's name.
        /// </summary>
        public int LinesCounter { get; set; }

       /// <summary>
       /// Initializtion.
       /// </summary>
       /// <param name="thickness">Lines' thickness.</param>
       /// <param name="pointSize">Draging points' size.</param>
        public AllLines(float thickness, int pointSize)
        {
            Lines = new List<Line>();
            LinesCounter = 0;
            Thickness = thickness;
            Size = new Size(pointSize, pointSize);
        }
    }
}
