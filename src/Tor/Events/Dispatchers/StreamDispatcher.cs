using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tor.Events
{
    /// <summary>
    /// A class containing the logic for processing a stream-status event.
    /// </summary>
    [EventAssoc(Event.Streams)]
    internal sealed class StreamDispatcher : Dispatcher
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
            Stream stream = Stream.FromLine(Client, line);

            if (stream == null)
                return false;

            Events.OnStreamChanged(new StreamEventArgs(stream));
            return true;
        }

        #endregion
    }
}
