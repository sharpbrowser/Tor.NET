using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Tor
{
    /// <summary>
    /// An enumerator containing the possible values specified against the PURPOSE parameter.
    /// </summary>
    public enum CircuitPurpose
    {
        /// <summary>
        /// No purpose parameter was specified.
        /// </summary>
        [Description(null)]
        None,

        /// <summary>
        /// The circuit is intended for traffic or fetching directory information.
        /// </summary>
        [Description("GENERAL")]
        General,

        /// <summary>
        /// The circuit is a client-side introduction point for a hidden service circuit.
        /// </summary>
        [Description("HS_CLIENT_INTRO")]
        HSClientIntro,

        /// <summary>
        /// The circuit is a client-side hidden service rendezvous circuit.
        /// </summary>
        [Description("HS_CLIENT_REND")]
        HSClientRend,

        /// <summary>
        /// The circuit is a server-side introduction point for a hidden service circuit.
        /// </summary>
        [Description("HS_SERVICE_INTRO")]
        HSServiceIntro,

        /// <summary>
        /// The circuit is a server-side hidden service rendezvous circuit.
        /// </summary>
        [Description("HS_SERVICE_REND")]
        HSServiceRend,

        /// <summary>
        /// The circuit is a test circuit to verify that the service can be used as a relay.
        /// </summary>
        [Description("TESTING")]
        Testing,

        /// <summary>
        /// The circuit was built by a controller.
        /// </summary>
        [Description("CONTROLLER")]
        Controller,

        /// <summary>
        /// The circuit was built to measure the time taken.
        /// </summary>
        [Description("MEASURE_TIMEOUT")]
        MeasureTimeout,
    }
}
