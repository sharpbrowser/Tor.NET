using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tor.Config
{
    /// <summary>
    /// An enumerator containing the different validation methods to perform against a configuration value.
    /// </summary>
    [Flags]
    internal enum ConfigurationValidation : int
    {
        /// <summary>
        /// No validation should be performed.
        /// </summary>
        None = 0x000,

        /// <summary>
        /// Ensure that the value is not <c>null</c>.
        /// </summary>
        NonNull = 0x001,

        /// <summary>
        /// Ensure that the value is non-negative.
        /// </summary>
        NonNegative = 0x002,

        /// <summary>
        /// Ensure that the value is non-zero.
        /// </summary>
        NonZero = 0x004,

        /// <summary>
        /// Ensure that the value falls within the range of valid port numbers.
        /// </summary>
        PortRange = 0x008,

        /// <summary>
        /// Ensure that the value is divisible by 1024.
        /// </summary>
        SizeDivision = 0x010,

        /// <summary>
        /// Ensure that the value is positive and non-zero.
        /// </summary>
        Positive = NonNegative | NonZero
    }
}
