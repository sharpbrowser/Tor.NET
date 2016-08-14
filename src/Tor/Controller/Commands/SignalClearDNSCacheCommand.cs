using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tor.Controller
{
    /// <summary>
    /// A class containing the command to clear client-side cached IP addresses for hostnames.
    /// </summary>
    internal sealed class SignalClearDNSCacheCommand : Command<Response>
    {
        #region Tor.Controller.Command<>

        /// <summary>
        /// Dispatches the command to the client control port and produces a <typeparamref name="T" /> response result.
        /// </summary>
        /// <param name="connection">The control connection where the command should be dispatched.</param>
        /// <returns>
        /// A <typeparamref name="T" /> object instance containing the response data.
        /// </returns>
        protected override Response Dispatch(Connection connection)
        {
            if (connection.Write("signal cleardnscache"))
            {
                ConnectionResponse response = connection.Read();
                return new Response(response.Success);
            }

            return new Response(false);
        }

        #endregion
    }
}
