using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Tor.Controller
{
    /// <summary>
    /// A class containing the command to retrieve configuration values from a client.
    /// </summary>
    internal sealed class GetConfCommand : Command<GetConfResponse>
    {
        private readonly ReadOnlyCollection<string> configurations;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetConfCommand"/> class.
        /// </summary>
        /// <param name="configurations">The configurations which should be retrieved from the tor application.</param>
        public GetConfCommand(List<string> configurations)
        {
            this.configurations = configurations.AsReadOnly();
        }

        #region Tor.Controller.Command<>

        /// <summary>
        /// Dispatches the command to the client control port and produces a <typeparamref name="T" /> response result.
        /// </summary>
        /// <param name="connection">The control connection where the command should be dispatched.</param>
        /// <returns>
        /// A <typeparamref name="T" /> object instance containing the response data.
        /// </returns>
        protected override GetConfResponse Dispatch(Connection connection)
        {
            StringBuilder builder = new StringBuilder("getconf");

            foreach (string name in configurations)
            {
                builder.Append(' ');
                builder.Append(name);
            }

            if (connection.Write(builder.ToString()))
            {
                ConnectionResponse response = connection.Read();

                if (!response.Success)
                    return new GetConfResponse(false, null);

                ResponsePairs values = new ResponsePairs(response.Responses.Count);

                foreach (string value in response.Responses)
                {
                    string[] parts = value.Split(new[] { '=' }, 2);
                    string name = parts[0].Trim();

                    if (parts.Length != 2)
                        values[name] = null;
                    else
                        values[name] = parts[1].Trim();
                }

                return new GetConfResponse(true, values);
            }

            return new GetConfResponse(false, null);
        }

        #endregion
    }

    /// <summary>
    /// A class containing a collection of configuration value responses.
    /// </summary>
    internal sealed class GetConfResponse : Response
    {
        private readonly ResponsePairs values;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetConfResponse"/> class.
        /// </summary>
        /// <param name="success">A value indicating whether the command was received and processed successfully.</param>
        /// <param name="values">The values received from the tor control connection.</param>
        public GetConfResponse(bool success, ResponsePairs values) : base(success)
        {
            this.values = values;
        }

        #region Properties

        /// <summary>
        /// Gets the values received from the control connection.
        /// </summary>
        public ResponsePairs Values
        {
            get { return values; }
        }

        #endregion
    }
}
