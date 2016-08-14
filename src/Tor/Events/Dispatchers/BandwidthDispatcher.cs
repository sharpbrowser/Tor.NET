using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tor.Helpers;

namespace Tor.Events
{
    /// <summary>
    /// A class containing the logic for parsing a bandwidth event.
    /// </summary>
    [EventAssoc(Event.Bandwidth)]
    internal sealed class BandwidthDispatcher : Dispatcher
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
            string[] parts = StringHelper.GetAll(line, ' ');

            if (parts.Length < 2)
                return false;

            double downloaded;
            double uploaded;

            if (!double.TryParse(parts[0], out downloaded))
                return false;

            if (!double.TryParse(parts[1], out uploaded))
                return false;

            Events.OnBandwidthChanged(new BandwidthEventArgs(new Bytes(downloaded).Normalize(), new Bytes(uploaded).Normalize()));
            return true;
        }

        #endregion
    }
}
