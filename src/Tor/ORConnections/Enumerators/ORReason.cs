using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Tor
{
    /// <summary>
    /// An enumerator containing the different reasons for a <c>ORStatus.Closed</c> or <c>ORStatus.Failed</c> state.
    /// </summary>
    public enum ORReason
    {
        /// <summary>
        /// The reason was not provided or could not be determined.
        /// </summary>
        [Description(null)]
        None,

        /// <summary>
        /// The OR connection has shut down cleanly.
        /// </summary>
        [Description("DONE")]
        Done,

        /// <summary>
        /// The OR connection received an <c>ECONNREFUSED</c> when connecting to the target OR.
        /// </summary>
        [Description("CONNECTREFUSED")]
        ConnectRefused,

        /// <summary>
        /// The OR connection connected to the OR, but the identity was not what was expected.
        /// </summary>
        [Description("IDENTITY")]
        Identity,

        /// <summary>
        /// The OR connection received an <c>ECONNRESET</c> or similar IO error from OR.
        /// </summary>
        [Description("CONNECTRESET")]
        ConnectReset,

        /// <summary>
        /// The OR connection received an <c>ETIMEOUT</c> or similar IO eerror from the OR, or the connection
        /// has been terminated for being open for too long.
        /// </summary>
        [Description("TIMEOUT")]
        Timeout,

        /// <summary>
        /// The OR connection received an <c>ENOTCONN</c>, <c>ENETUNREACH</c>, <c>ENETDOWN</c>, <c>EHOSTUNREACH</c> or
        /// similar error while connecting to the OR.
        /// </summary>
        [Description("NOROUTE")]
        NoRoute,

        /// <summary>
        /// The OR connection received a different IO error.
        /// </summary>
        [Description("IOERROR")]
        IOError,

        /// <summary>
        /// The OR connection does not have enough system resources to connect to the OR.
        /// </summary>
        [Description("RESOURCELIMIT")]
        ResourceLimit,

        /// <summary>
        /// No pluggable transport was available.
        /// </summary>
        [Description("PT_MISSING")]
        PTMissing,

        /// <summary>
        /// The OR connection closed for some other reason.
        /// </summary>
        [Description("MISC")]
        Misc
    }
}
