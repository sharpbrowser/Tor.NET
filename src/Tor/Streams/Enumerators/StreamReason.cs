using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Tor
{
    /// <summary>
    /// An enumerator containing the reasons for a stream's failed, closed or detached events.
    /// </summary>
    public enum StreamReason : int
    {
        /// <summary>
        /// No reason was provided.
        /// </summary>
        [Description(null)]
        None = 0,

        /// <summary>
        /// A miscellaneous error occurred.
        /// </summary>
        [Description("MISC")]
        Misc = 1,

        /// <summary>
        /// The stream failed to resolve an address.
        /// </summary>
        [Description("RESOLVEFAILED")]
        ResolveFailed = 2,

        /// <summary>
        /// The stream connect attempt was refused.
        /// </summary>
        [Description("CONNECTREFUSED")]
        ConnectRefused = 3,

        /// <summary>
        /// The exit policy failed.
        /// </summary>
        [Description("EXITPOLICY")]
        ExitPolicy = 4,

        /// <summary>
        /// The stream was destroyed.
        /// </summary>
        [Description("DESTROY")]
        Destroy = 5,

        /// <summary>
        /// The stream closed normally.
        /// </summary>
        [Description("DONE")]
        Done = 6,

        /// <summary>
        /// The stream timed out.
        /// </summary>
        [Description("TIMEOUT")]
        Timeout = 7,

        /// <summary>
        /// There is no route to the host.
        /// </summary>
        [Description("NOROUTE")]
        NoRoute = 8,

        /// <summary>
        /// The server is hibernating.
        /// </summary>
        [Description("HIBERNATING")]
        Hibernating = 9,

        /// <summary>
        /// An internal error occurred.
        /// </summary>
        [Description("INTERNAL")]
        Internal = 10,

        /// <summary>
        /// The server ran out of resources.
        /// </summary>
        [Description("RESOURCELIMIT")]
        ResourceLimit = 11,

        /// <summary>
        /// The connection was reset.
        /// </summary>
        [Description("CONNRESET")]
        ConnReset = 12,

        /// <summary>
        /// There was a tor protocol error.
        /// </summary>
        [Description("TORPROTOCOL")]
        TorProtocol = 13,

        /// <summary>
        /// The stream was not accessing a directory.
        /// </summary>
        [Description("NOTDIRECTORY")]
        NotDirectory = 14,

        /// <summary>
        /// A relay-end cell was received.
        /// </summary>
        [Description("END")]
        End = -1,

        /// <summary>
        /// The client tried to connect to a private address.
        /// </summary>
        [Description("PRIVATE_ADDR")]
        PrivateAddr = -1,
    }
}
