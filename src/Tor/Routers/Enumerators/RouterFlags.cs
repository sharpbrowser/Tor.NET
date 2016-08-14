using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Tor
{
    /// <summary>
    /// An enumerator containing different flags which may be associated with a router.
    /// </summary>
    [Flags]
    public enum RouterFlags : int
    {
        /// <summary>
        /// No flags were associated with the router.
        /// </summary>
        [Description(null)]
        None = 0x0000,

        /// <summary>
        /// The router is a directory authority.
        /// </summary>
        [Description("Authority")]
        Authority = 0x0001,

        /// <summary>
        /// The router is considered useless as an exit node.
        /// </summary>
        [Description("BadExit")]
        BadExit = 0x0002,

        /// <summary>
        /// The router is more useful for building general-purpose exit circuits, rather than router circuits.
        /// </summary>
        [Description("Exit")]
        Exit = 0x0004,

        /// <summary>
        /// The router is suitable for high-bandwidth circuits.
        /// </summary>
        [Description("Fast")]
        Fast = 0x0008,

        /// <summary>
        /// The router is suitable for use as an entry guard.
        /// </summary>
        [Description("Guard")]
        Guard = 0x0010,

        /// <summary>
        /// The router is considered a v2 hidden-service directory.
        /// </summary>
        [Description("HSDir")]
        HSDir = 0x0020,

        /// <summary>
        /// The router's identity-nickname mapping is canonical, and this authority binds names.
        /// </summary>
        [Description("Named")]
        Named = 0x0040,

        /// <summary>
        /// The router is suitable for long-lived circuits.
        /// </summary>
        [Description("Stable")]
        Stable = 0x0080,

        /// <summary>
        /// The router is currently usable.
        /// </summary>
        [Description("Running")]
        Running = 0x0100,

        /// <summary>
        /// The router's name has been bound by another router, and this authority binds names.
        /// </summary>
        [Description("Unnamed")]
        Unnamed = 0x0200,

        /// <summary>
        /// The router has been validated.
        /// </summary>
        [Description("Valid")]
        Valid = 0x0400,

        /// <summary>
        /// The router implements the v2 directory protocol, or higher.
        /// </summary>
        [Description("V2Dir")]
        V2Dir = 0x0800,
    }
}
