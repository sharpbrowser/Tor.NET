using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Tor
{
    /// <summary>
    /// An enumerator containing the different states of an OR connection.
    /// </summary>
    public enum ORStatus
    {
        /// <summary>
        /// No status was not provided or the status could not be determined.
        /// </summary>
        [Description(null)]
        None,

        /// <summary>
        /// The OR connection has been received and is beginning the handshake process.
        /// </summary>
        [Description("NEW")]
        New,

        /// <summary>
        /// The OR connection has been launched and is beginning the client-side handshake process.
        /// </summary>
        [Description("LAUNCHED")]
        Launched,

        /// <summary>
        /// The OR connection has been connected and the handshake is complete.
        /// </summary>
        [Description("CONNECTED")]
        Connected,

        /// <summary>
        /// The OR connection failed.
        /// </summary>
        [Description("FAILED")]
        Failed,

        /// <summary>
        /// The OR connection closed.
        /// </summary>
        [Description("CLOSED")]
        Closed
    }
}
