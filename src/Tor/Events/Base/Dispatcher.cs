using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tor.Events
{
    /// <summary>
    /// A class containing the skeleton structure for processing an event response received from a control connection.
    /// </summary>
    internal abstract class Dispatcher
    {
        private Client client;
        private Event currentEvent;
        private Events events;

        #region Properties

        /// <summary>
        /// Gets or sets the client which owns the dispatcher.
        /// </summary>
        public Client Client
        {
            get { return client; }
            set { client = value; }
        }

        /// <summary>
        /// Gets or sets the event being processed by this dispatcher.
        /// </summary>
        public Event CurrentEvent
        {
            get { return currentEvent; }
            set { currentEvent = value; }
        }

        /// <summary>
        /// Gets or sets the event handler object instance.
        /// </summary>
        public Events Events
        {
            get { return events; }
            set { events = value; }
        }

        #endregion

        /// <summary>
        /// Dispatches the event, parsing the content of the line and raising the relevant event.
        /// </summary>
        /// <param name="line">The line which was received from the control connection.</param>
        /// <returns><c>true</c> if the event is parsed and dispatched successfully; otherwise, <c>false</c>.</returns>
        public abstract bool Dispatch(string line);
    }
}
