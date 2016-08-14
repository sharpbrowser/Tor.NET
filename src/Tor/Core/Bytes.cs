using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tor.Helpers;
using System.ComponentModel;
using Tor.Converters;

namespace Tor
{
    /// <summary>
    /// A structure containing byte information, in a specified magnitude.
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(BytesTypeConverter))]
    public struct Bytes
    {
        /// <summary>
        /// Gets an empty <see cref="Bytes"/> structure.
        /// </summary>
        public static readonly Bytes Empty = new Bytes();

        private Bits units;
        private double value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Bytes"/> struct.
        /// </summary>
        /// <param name="value">The value in bytes.</param>
        public Bytes(double value)
        {
            this.units = Bits.B;
            this.value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bytes"/> struct.
        /// </summary>
        /// <param name="value">The value relative to the specified units.</param>
        /// <param name="units">The units of the bytes.</param>
        public Bytes(double value, Bits units)
        {
            this.units = units;
            this.value = value;
        }

        #region Properties

        /// <summary>
        /// Gets or sets the units of the bytes.
        /// </summary>
        public Bits Units
        {
            get { return units; }
            set { units = value; }
        }

        /// <summary>
        /// Gets or sets the value of bytes, relative to the units.
        /// </summary>
        public double Value
        {
            get { return value; }
            set { this.value = value; }
        }

        #endregion
        
        #region System.Object

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj != null && obj is Bytes && ((Bytes)obj).units == units && ((Bytes)obj).value == value;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Convert.ToInt32(units);
                hash = hash * 23 + Convert.ToInt32(value);
                return hash;
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            string unit = "bytes";

            switch (units)
            {
                case Bits.KB:
                    unit = "KBytes";
                    break;
                case Bits.MB:
                    unit = "MBytes";
                    break;
                case Bits.GB:
                    unit = "GBytes";
                    break;
                case Bits.TB:
                    unit = "TBytes";
                    break;
            }

            return string.Format("{0} {1}", Convert.ToInt32(value), unit);
        }

        #endregion

        /// <summary>
        /// Normalizes the bytes by dividing the value into the nearest acceptable unit.
        /// </summary>
        /// <returns>A <see cref="Bytes"/> struct containing the normalized values.</returns>
        public Bytes Normalize()
        {
            if (units == Bits.TB)
                return this;
            if (value == 0.00)
                return this;

            int max = (int)Bits.TB;
            int unit = (int)units;
            double absolute = Math.Abs(value);

            while (1024 <= absolute && unit < max)
            {
                absolute = Math.Round(absolute / 1024.00, 4);
                unit++;
            }

            if (value < 0.00)
                absolute *= -1.0;

            return new Bytes(absolute, (Bits)unit);
        }

        /// <summary>
        /// Converts the units of the bytes into another unit.
        /// </summary>
        /// <param name="unit">The units to convert the bytes to.</param>
        /// <returns>A <see cref="Bytes"/> structure containing the converted value.</returns>
        public Bytes ToUnit(Bits unit)
        {
            if (units == unit)
                return this;
            if (value == 0.00)
                return new Bytes(0.00, unit);

            int from = 10 - (int)units;
            int to = 10 - (int)unit;
            double absolute = Math.Abs(value);

            if (from < to)
            {
                while (from < to)
                {
                    absolute = Math.Round(absolute * 1024.00, 4);
                    from++;
                }
            }
            else
            {
                while (to < from)
                {
                    absolute = Math.Round(absolute / 1024.00, 4);
                    from--;
                }
            }

            if (value < 0.00)
                absolute *= -1.0;

            return new Bytes(absolute, unit);
        }
    }
}
