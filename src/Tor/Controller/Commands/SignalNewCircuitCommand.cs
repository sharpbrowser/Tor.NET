using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tor.Controller
{
    /// <summary>
    /// A class containing the command to generate a new circuit.
    /// </summary>
    internal sealed class SignalNewCircuitCommand : Command<Response>
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
            if (connection.Write("signal newnym"))
            {
                ConnectionResponse response = connection.Read();
                return new Response(response.Success);
            }

            return new Response(false);
        }

        #endregion
    }
}
