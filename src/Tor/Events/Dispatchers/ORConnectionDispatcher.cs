using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tor.Events
{
    /// <summary>
    /// A class containing the logic for dispatching an OR connection changed event.
    /// </summary>
    [EventAssoc(Event.ORConnections)]
    internal sealed class ORConnectionDispatcher : Dispatcher
    {
        #region Tor.Events.Dispatcher

        /// <summary>
        /// Dispatches the event, parsing the content of the line and raising the relevant event.
        /// </summary>
        /// <param name="line">The line which was received from the control connection.</param>
        /// <returns>
        ///   <c>true</c> if the event is parsed and dispatched successfully; otherwise, <c>false</c>.
        /// </returns>
        public override bool Dispatch(string line)
        {
            ORConnection connection = ORConnection.FromLine(line);

            if (connection == null)
                return false;

            Client.Events.OnORConnectionChanged(new ORConnectionEventArgs(connection));
            return true;
        }

        #endregion
    }
}
