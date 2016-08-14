using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Tor.Helpers;
using System.ComponentModel;

namespace Tor.Controller
{
    /// <summary>
    /// A class containing the command to retrieve router status information.
    /// </summary>
    internal sealed class GetRouterStatusCommand : Command<GetRouterStatusResponse>
    {
        private string identity;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetRouterStatusCommand"/> class.
        /// </summary>
        public GetRouterStatusCommand() : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetRouterStatusCommand"/> class.
        /// </summary>
        /// <param name="identity">The router identity to retrieve status information for.</param>
        public GetRouterStatusCommand(string identity)
        {
            this.identity = identity;
        }

        #region Properties

        /// <summary>
        /// Gets or sets the identity of the router.
        /// </summary>
        public string Identity
        {
            get { return identity; }
        }

        #endregion

        #region Tor.Controller.Command<>

        /// <summary>
        /// Dispatches the command to the client control port and produces a <typeparamref name="T" /> response result.
        /// </summary>
        /// <param name="connection"></param>
        /// <returns>
        /// A <typeparamref name="T" /> object instance containing the response data.
        /// </returns>
        protected override GetRouterStatusResponse Dispatch(Connection connection)
        {
            if (identity == null)
                return new GetRouterStatusResponse(false, null);

            string request = string.Format("ns/id/{0}", identity);

            if (connection.Write("getinfo {0}", request))
            {
                ConnectionResponse response = connection.Read();

                if (!response.Success || !response.Responses[0].StartsWith(request, StringComparison.CurrentCultureIgnoreCase))
                    return new GetRouterStatusResponse(false, null);

                Router router = null;

                foreach (string line in response.Responses)
                {
                    string stripped = line.Trim();

                    if (string.IsNullOrWhiteSpace(stripped))
                        continue;

                    if (stripped.StartsWith("r"))
                    {
                        string[] values = stripped.Split(' ');

                        if (values.Length < 9)
                            continue;

                        DateTime publication = DateTime.MinValue;

                        if (!DateTime.TryParse(string.Format("{0} {1}", values[4], values[5]), out publication))
                            publication = DateTime.MinValue;

                        int orPort = 0;

                        if (!int.TryParse(values[7], out orPort))
                            orPort = 0;

                        int dirPort = 0;

                        if (!int.TryParse(values[8], out dirPort))
                            dirPort = 0;

                        IPAddress ipAddress = null;

                        if (!IPAddress.TryParse(values[6], out ipAddress))
                            ipAddress = null;

                        router = new Router();
                        router.Digest = values[3];
                        router.DIRPort = dirPort;
                        router.Identity = values[2];
                        router.IPAddress = ipAddress;
                        router.Nickname = values[1];
                        router.ORPort = orPort;
                        router.Publication = publication;
                        continue;
                    }

                    if (stripped.StartsWith("s") && router != null)
                    {
                        string[] values = stripped.Split(' ');

                        for (int i = 1, length = values.Length; i < length; i++)
                        {
                            RouterFlags flag = ReflectionHelper.GetEnumerator<RouterFlags, DescriptionAttribute>(attr => values[i].Equals(attr.Description, StringComparison.CurrentCultureIgnoreCase));

                            if (flag != RouterFlags.None)
                                router.Flags |= flag;
                        }

                        continue;
                    }

                    if (stripped.StartsWith("w") && router != null)
                    {
                        string[] values = stripped.Split(' ');

                        if (values.Length < 2 || !values[1].StartsWith("bandwidth=", StringComparison.CurrentCultureIgnoreCase))
                            continue;

                        string[] value = values[1].Split(new[] { '=' }, 2);

                        if (value.Length < 2)
                            continue;

                        int bandwidth;

                        if (int.TryParse(value[1].Trim(), out bandwidth))
                            router.Bandwidth = new Bytes((double)bandwidth, Bits.KB).Normalize();
                    }
                }

                return new GetRouterStatusResponse(true, router);
            }

            return new GetRouterStatusResponse(false, null);
        }

        #endregion
    }

    /// <summary>
    /// A class containing the response information from a <c>getinfo ns/id/?</c> command.
    /// </summary>
    internal sealed class GetRouterStatusResponse : Response
    {
        private readonly Router router;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetRouterStatusResponse"/> class.
        /// </summary>
        /// <param name="success">A value indicating whether the command was received and processed successfully.</param>
        /// <param name="router">The router information retrieved from the command.</param>
        public GetRouterStatusResponse(bool success, Router router) : base(success)
        {
            this.router = router;
        }

        #region Properties

        /// <summary>
        /// Gets the router information retrieved from the control connection.
        /// </summary>
        public Router Router
        {
            get { return router; }
        }

        #endregion
    }
}
