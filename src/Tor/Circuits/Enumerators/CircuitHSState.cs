using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Tor
{
    /// <summary>
    /// An enumerator containing the possible values specified against the HS_STATE parameter. The HS_STATE indicates the
    /// different states that a hidden service circuit may have.
    /// </summary>
    public enum CircuitHSState
    {
        /// <summary>
        /// No hidden service state was provided.
        /// </summary>
        [Description(null)]
        None,

        /// <summary>
        /// The client-side hidden service is connecting to the introductory point.
        /// </summary>
        [Description("HSCI_CONNECTING")]
        HSCIConnecting,

        /// <summary>
        /// The client-side hidden service has sent INTRODUCE1 and is awaiting a reply.
        /// </summary>
        [Description("HSCI_INTRO_SENT")]
        HSCIIntroSent,

        /// <summary>
        /// The client-side hidden service has received a reply and the circuit is closing.
        /// </summary>
        [Description("HSCI_DONE")]
        HSCIDone,

        /// <summary>
        /// The client-side hidden service is connecting to the rendezvous point.
        /// </summary>
        [Description("HSCR_CONNECTING")]
        HSCRConnecting,

        /// <summary>
        /// The client-side hidden servicce has established connection to the rendezvous point and is awaiting an introduction.
        /// </summary>
        [Description("HSCR_ESTABLISHED_IDLE")]
        HSCREstablishedIdle,

        /// <summary>
        /// The client-side hidden service has received an introduction and is awaiting a rend.
        /// </summary>
        [Description("HSCR_ESTABLISHED_WAITING")]
        HSCREstablishedWaiting,

        /// <summary>
        /// The client-side hidden service is connected to the hidden service.
        /// </summary>
        [Description("HSCR_JOINED")]
        HSCRJoined,

        /// <summary>
        /// The server-side hidden service is connecting to the introductory point.
        /// </summary>
        [Description("HSSI_CONNECTING")]
        HSSIConnecting,

        /// <summary>
        /// The server-side hidden service has established connection to the introductory point.
        /// </summary>
        [Description("HSSI_ESTABLISHED")]
        HSSIEstablished,

        /// <summary>
        /// The server-side hidden service is connecting to the rendezvous point.
        /// </summary>
        [Description("HSSR_CONNECTING")]
        HSSRConnecting,

        /// <summary>
        /// The server-side hidden service has established connection to the rendezvous point.
        /// </summary>
        [Description("HSSR_JOINED")]
        HSSRJoined,
    }
}
