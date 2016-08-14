using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Tor
{
    /// <summary>
    /// An enumerator containing the possible values for a field which uses the REASON parameter.
    /// </summary>
    public enum CircuitReason
    {
        /// <summary>
        /// No reason was provided.
        /// </summary>
        [Description(null)]
        None,

        /// <summary>
        /// There was a violation in the Tor protocol.
        /// </summary>
        [Description("TORPROTOCOL")]
        TorProtocol,

        /// <summary>
        /// There was an internal error.
        /// </summary>
        [Description("INTERNAL")]
        Internal,

        /// <summary>
        /// Requested by the client via a TRUNCATE command.
        /// </summary>
        [Description("REQUESTED")]
        Requested,

        /// <summary>
        /// The relay is currently hibernating.
        /// </summary>
        [Description("HIBERNATING")]
        Hibernating,

        /// <summary>
        /// The relay is out of memory, sockets, or circuit IDs.
        /// </summary>
        [Description("RESOURCELIMIT")]
        ResourceLimit,

        /// <summary>
        /// Unable to contact the relay.
        /// </summary>
        [Description("CONNECTFAILED")]
        ConnectFailed,

        /// <summary>
        /// The relay had the wrong OR identification.
        /// </summary>
        [Description("OR_IDENTITY")]
        ORIdentity,

        /// <summary>
        /// The connection failed after being established.
        /// </summary>
        [Description("OR_CONN_CLOSED")]
        ORConnectionClosed,

        /// <summary>
        /// The circuit has expired.
        /// </summary>
        [Description("FINISHED")]
        Finished,

        /// <summary>
        /// The circuit construction timed out.
        /// </summary>
        [Description("TIMEOUT")]
        Timeout,

        /// <summary>
        /// The circuit was unexpectedly closed.
        /// </summary>
        [Description("DESTROYED")]
        Destroyed,

        /// <summary>
        /// There are not enough relays to make a circuit.
        /// </summary>
        [Description("NOPATH")]
        NoPath,

        /// <summary>
        /// The requested hidden service does not exist.
        /// </summary>
        [Description("NOSUCHSERVICE")]
        NoSuchService,

        /// <summary>
        /// The circuit construction timed out, except that the circuit was left open for measurement purposes.
        /// </summary>
        [Description("MEASUREMENT_EXPIRED")]
        MeasurementExpired,
    }
}
