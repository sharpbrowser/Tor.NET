using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tor.Logging
{
    /// <summary>
    /// A class containing the methods and events for interacting with the logging engine within the client.
    /// </summary>
    public sealed class Logging : MarshalByRefObject
    {
        private readonly Client client;

        /// <summary>
        /// Initializes a new instance of the <see cref="Logging"/> class.
        /// </summary>
        /// <param name="client">The client for which this object instance belongs.</param>
        internal Logging(Client client)
        {
            this.client = client;
        }

        #region Events

        /// <summary>
        /// Occurs when a debug message is received.
        /// </summary>
        public event LogEventHandler DebugReceived
        {
            add { client.Events.DebugReceived += value; }
            remove { client.Events.DebugReceived -= value; }
        }

        /// <summary>
        /// Occurs when an error message is received.
        /// </summary>
        public event LogEventHandler ErrorReceived
        {
            add { client.Events.ErrorReceived += value; }
            remove { client.Events.ErrorReceived -= value; }
        }

        /// <summary>
        /// Occurs when an information message is received.
        /// </summary>
        public event LogEventHandler InfoReceived
        {
            add { client.Events.InfoReceived += value; }
            remove { client.Events.InfoReceived -= value; }
        }

        /// <summary>
        /// Occurs when a notice message is received.
        /// </summary>
        public event LogEventHandler NoticeReceived
        {
            add { client.Events.NoticeReceived += value; }
            remove { client.Events.NoticeReceived -= value; }
        }

        /// <summary>
        /// Occurs when a warning message is received.
        /// </summary>
        public event LogEventHandler WarnReceived
        {
            add { client.Events.WarnReceived += value; }
            remove { client.Events.WarnReceived -= value; }
        }

        #endregion
    }
}
