using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Tor
{
    /// <summary>
    /// An enumerator containing the possible values specified against the BUILD_FLAGS parameter.
    /// </summary>
    [Flags]
    public enum CircuitBuildFlags : int
    {
        /// <summary>
        /// No build flags were specified.
        /// </summary>
        [Description(null)]
        None = 0x000,

        /// <summary>
        /// The circuit is a one hop circuit used to fetch directory information.
        /// </summary>
        [Description("ONEHOP_TUNNEL")]
        OneHopTunnel = 0x001,

        /// <summary>
        /// The circuit will not be used for client traffic.
        /// </summary>
        [Description("IS_INTERNAL")]
        IsInternal = 0x002,

        /// <summary>
        /// The circuit only includes high capacity relays.
        /// </summary>
        [Description("NEED_CAPACITY")]
        NeedCapacity = 0x004,

        /// <summary>
        /// The circuit only includes relays with high uptime.
        /// </summary>
        [Description("NEED_UPTIME")]
        NeedUpTime = 0x008,
    }
}
