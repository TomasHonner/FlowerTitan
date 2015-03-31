using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace FlowerTitan.MeasuringLines
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
       /// Initializtion.
       /// </summary>
        public AllLines()
        {
            Lines = new List<Line>();
        }
    }
}
