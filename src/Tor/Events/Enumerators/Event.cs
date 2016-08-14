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
    [Flags]
    public enum Event : int
    {
        /// <summary>
        /// The event was unrecognised.
        /// </summary>
        [Description(null)]
        Unknown = 0x000,

        /// <summary>
        /// An event raised when the circuit status is changed.
        /// </summary>
        [Description("CIRC")]
        Circuits = 0x001,

        /// <summary>
        /// An event raised when a stream status is changed.
        /// </summary>
        [Description("STREAM")]
        Streams = 0x002,

        /// <summary>
        /// An event raised when an OR connection status is changed.
        /// </summary>
        [Description("ORCONN")]
        ORConnections = 0x004,

        /// <summary>
        /// An event raised when the bandwidth used within the last second has changed.
        /// </summary>
        [Description("BW")]
        Bandwidth = 0x008,

        /// <summary>
        /// An event raised when the value of a configuration has changed.
        /// </summary>
        [Description("CONF_CHANGED")]
        ConfigChanged = 0x010,

        /// <summary>
        /// An event raised when a debug message is produced.
        /// </summary>
        [Description("DEBUG")]
        LogDebug = 0x020,

        /// <summary>
        /// An event raised when an information message is produced.
        /// </summary>
        [Description("INFO")]
        LogInfo = 0x040,

        /// <summary>
        /// An event raised when a notice message is produced.
        /// </summary>
        [Description("NOTICE")]
        LogNotice = 0x080,

        /// <summary>
        /// An event raised when a warning message is produced.
        /// </summary>
        [Description("WARN")]
        LogWarn = 0x100,

        /// <summary>
        /// An event raised when an error message is produced.
        /// </summary>
        [Description("ERR")]
        LogError = 0x200,
    }
}
