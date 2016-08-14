using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Tor.Events
{
    /// <summary>
    /// An enumerator containing the list of events which can be monitored in the tor service.
    /// </summary>
    public enum Event
    {
        /// <summary>
        /// An event raised when the circuit status is changed.
        /// </summary>
        [Description("CIRC")]
        Circuits,

        /// <summary>
        /// An event raised when a stream status is changed.
        /// </summary>
        [Description("STREAM")]
        Streams,

        /// <summary>
        /// An event raised when an OR connection status is changed.
        /// </summary>
        [Description("ORCONN")]
        ORConnections,

        /// <summary>
        /// An event raised when the bandwidth used within the last second has changed.
        /// </summary>
        [Description("BW")]
        Bandwidth,

        /// <summary>
        /// An event raised when new descriptors are available.
        /// </summary>
        [Description("NEWDESC")]
        NewDescriptors,

        /// <summary>
        /// An event raised when a new address mapping is registered in the tor address map cache.
        /// </summary>
        [Description("ADDRMAP")]
        AddressMapping,
    }
}
