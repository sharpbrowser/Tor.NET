using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tor.Helpers;
using System.ComponentModel;

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
            string target;
            ORStatus status;
            string[] parts = StringHelper.GetAll(line, ' ');

            if (parts.Length < 2)
                return false;

            target = parts[0];
            status = ReflectionHelper.GetEnumerator<ORStatus, DescriptionAttribute>(attr => parts[1].Equals(attr.Description, StringComparison.CurrentCultureIgnoreCase));

            ORConnection connection = new ORConnection();
            connection.Status = status;
            connection.Target = target;

            for (int i = 2; i < parts.Length; i++)
            {
                string data = parts[i].Trim();

                if (!data.Contains("="))
                    continue;

                string[] values = data.Split(new[] { '=' }, 2);

                if (values.Length < 2)
                    continue;

                string name = values[0].Trim();
                string value = values[1].Trim();

                if ("REASON".Equals(name, StringComparison.CurrentCultureIgnoreCase))
                {
                    connection.Reason = ReflectionHelper.GetEnumerator<ORReason, DescriptionAttribute>(attr => value.Equals(attr.Description, StringComparison.CurrentCultureIgnoreCase));
                    continue;
                }

                if ("NCIRCS".Equals(name, StringComparison.CurrentCultureIgnoreCase))
                {
                    int circuits;

                    if (int.TryParse(value, out circuits))
                        connection.CircuitCount = circuits;

                    continue;
                }

                if ("ID".Equals(name, StringComparison.CurrentCultureIgnoreCase))
                {
                    int id;

                    if (int.TryParse(value, out id))
                        connection.ID = id;

                    continue;
                }
            }

            Client.Events.OnORConnectionChanged(new ORConnectionEventArgs(connection));
            return true;
        }

        #endregion
    }
}
