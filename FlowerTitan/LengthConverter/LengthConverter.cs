using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FlowerTitan.MeasuringLines;

namespace FlowerTitan.LengthConverter
{
    /// <summary>
    /// Singleton class which handles all lines' length coversions.
    /// </summary>
    class LengthConverter
    {
        //singleton instance
        private static LengthConverter lengthConverter = null;

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

        public float ConvertLineLengthToMM(Line line)
        {

            return 0f;
        }
    }
}
