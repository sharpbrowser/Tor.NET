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
    /// A class containing the command to retrieve all relevant router statuses.
    /// </summary>
    internal sealed class GetAllRouterStatusCommand : Command<GetAllRouterStatusResponse>
    {
        #region Tor.Controller.Command<>

        /// <summary>
        /// Dispatches the command to the client control port and produces a <typeparamref name="T" /> response result.
        /// </summary>
        /// <param name="connection">The control connection where the command should be dispatched.</param>
        /// <returns>
        /// A <typeparamref name="T" /> object instance containing the response data.
        /// </returns>
        protected override GetAllRouterStatusResponse Dispatch(Connection connection)
        {
            if (connection.Write("getinfo ns/all"))
            {
                ConnectionResponse response = connection.Read();

                if (!response.Success || !response.Responses[0].StartsWith("ns/all", StringComparison.CurrentCultureIgnoreCase))
                    return new GetAllRouterStatusResponse(false);

                List<Router> routers = new List<Router>();
                Router router = null;

                for (int i = 1, count = response.Responses.Count; i < count; i++)
                {
                    string line = response.Responses[i].Trim();

                    if (string.IsNullOrWhiteSpace(line) || ".".Equals(line))
                        continue;

                    if (line.StartsWith("r"))
                    {
                        if (router != null)
                        {
                            routers.Add(router);
                            router = null;
                        }

                        string[] values = line.Split(' ');

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

                    if (line.StartsWith("s") && router != null)
                    {
                        string[] values = line.Split(' ');

                        for (int j = 1, length = values.Length; j < length; j++)
                        {
                            RouterFlags flag = ReflectionHelper.GetEnumerator<RouterFlags, DescriptionAttribute>(attr => values[j].Equals(attr.Description, StringComparison.CurrentCultureIgnoreCase));

                            if (flag != RouterFlags.None)
                                router.Flags |= flag;
                        }

                        continue;
                    }

                    if (line.StartsWith("w") && router != null)
                    {
                        string[] values = line.Split(' ');

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

                if (router != null)
                {
                    routers.Add(router);
                    router = null;
                }

                return new GetAllRouterStatusResponse(true, routers);
            }

            return new GetAllRouterStatusResponse(false);
        }

        #endregion
    }

    /// <summary>
    /// A class containing the response information from a <c>getinfo ns/all</c> command.
    /// </summary>
    internal sealed class GetAllRouterStatusResponse : Response
    {
        private readonly RouterCollection routers;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllRouterStatusResponse"/> class.
        /// </summary>
        /// <param name="success">A value indicating whether the command was received and processed successfully.</param>
        public GetAllRouterStatusResponse(bool success) : base(success)
        {
            this.routers = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllRouterStatusResponse"/> class.
        /// </summary>
        /// <param name="success">A value indicating whether the command was received and processed successfully.</param>
        /// <param name="routers">The collection of routers.</param>
        public GetAllRouterStatusResponse(bool success, IList<Router> routers) : base(success)
        {
            this.routers = new RouterCollection(routers);
        }

        #region Properties

        /// <summary>
        /// Gets a read-only collection containing the router status information.
        /// </summary>
        public RouterCollection Routers
        {
            get { return routers; }
        }

        #endregion
    }
}
