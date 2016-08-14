using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tor.Controller
{
    /// <summary>
    /// A class containing the command to close an existing circuit.
    /// </summary>
    internal sealed class CloseCircuitCommand : Command<Response>
    {
        private readonly Circuit circuit;

        /// <summary>
        /// Initializes a new instance of the <see cref="CloseCircuitCommand"/> class.
        /// </summary>
        /// <param name="circuit">The circuit which should be closed.</param>
        public CloseCircuitCommand(Circuit circuit)
        {
            this.circuit = circuit;
        }

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
            if (circuit == null || circuit.Status == CircuitStatus.Closed)
                return new Response(false);

            if (connection.Write("closecircuit {0}", circuit.ID))
            {
                ConnectionResponse response = connection.Read();
                return new Response(response.Success);
            }

            return new Response(false);
        }

        #endregion
    }
}
