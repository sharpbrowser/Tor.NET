using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Tor
{
    /// <summary>
    /// An enumerator containing the different possible statuses for a stream.
    /// </summary>
    public enum StreamStatus
    {
        /// <summary>
        /// The stream status was not provided.
        /// </summary>
        [Description(null)]
        None,

        /// <summary>
        /// A new request to connect.
        /// </summary>
        [Description("NEW")]
        New,

        /// <summary>
        /// A new request to resolve an address.
        /// </summary>
        [Description("NEWRESOLVE")]
        NewResolve,

        /// <summary>
        /// An address was re-mapped to another.
        /// </summary>
        [Description("REMAP")]
        Remap,

        /// <summary>
        /// A connect cell was sent along a circuit.
        /// </summary>
        [Description("SENTCONNECT")]
        SentConnect,

        /// <summary>
        /// A resolve cell was sent along the circuit.
        /// </summary>
        [Description("SENTRESOLVE")]
        SentResolve,

        /// <summary>
        /// A reply was received and the stream was established.
        /// </summary>
        [Description("SUCCEEDED")]
        Succeeded,

        /// <summary>
        /// The stream failed and cannot be retried.
        /// </summary>
        [Description("FAILED")]
        Failed,

        /// <summary>
        /// The stream is closed.
        /// </summary>
        [Description("CLOSED")]
        Closed,

        /// <summary>
        /// The stream was detached from a circuit, but can be retried.
        /// </summary>
        [Description("DETACHED")]
        Detached,
    }
}
