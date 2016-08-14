using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tor.Controller
{
    /// <summary>
    /// A class containing the command used to set the value of a configuration.
    /// </summary>
    internal sealed class SetConfCommand : Command<Response>
    {
        private readonly string name;
        private readonly string value;

        /// <summary>
        /// Initializes a new instance of the <see cref="SetConfCommand"/> class.
        /// </summary>
        /// <param name="name">The name of the configuration to set.</param>
        /// <param name="value">The value of the configuration.</param>
        public SetConfCommand(string name, string value)
        {
            this.name = name;
            this.value = value;
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
            if (name == null || value == null)
                return new Response(false);

            if (connection.Write("setconf {0}={1}", name, value.Contains(" ") ? string.Format("\"{0}\"", value) : value))
            {
                ConnectionResponse response = connection.Read();
                return new Response(response.Success);
            }

            return new Response(false);
        }

        #endregion
    }
}
