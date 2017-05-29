using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tor.Events
{
    /// <summary>
    /// A class containing the logic for parsing a circuit-status event.
    /// </summary>
    [EventAssoc(Event.Circuits)]
    internal sealed class CircuitDispatcher : Dispatcher
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
            Circuit circuit = Circuit.FromLine(Client, line);
            if (circuit == null)
                return false;

            Events.OnCircuitChanged(new CircuitEventArgs(circuit));

            return true;
        }

        #endregion
    }
}
