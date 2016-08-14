using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Tor
{
    /// <summary>
    /// An enumerator containing the possible statuses of a circuit.
    /// </summary>
    public enum CircuitStatus
    {
        /// <summary>
        /// The circuit ID was assigned to a new circuit.
        /// </summary>
        [Description("LAUNCHED")]
        Launched,

        /// <summary>
        /// The circuit has completed all hops and can accept streams.
        /// </summary>
        [Description("BUILT")]
        Built,

        /// <summary>
        /// The circuit has been extended with an additional hop.
        /// </summary>
        [Description("EXTENDED")]
        Extended,

        /// <summary>
        /// The circuit is closed because it was not built.
        /// </summary>
        [Description("FAILED")]
        Failed,

        /// <summary>
        /// The circuit is closed.
        /// </summary>
        [Description("CLOSED")]
        Closed
    }
}
