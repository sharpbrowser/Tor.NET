using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Tor.Controller;
using System.Net;
using Tor.Helpers;
using System.ComponentModel;

namespace Tor
{
    /// <summary>
    /// A class containing information regarding a circuit within the tor service.
    /// </summary>
    public sealed class Circuit : MarshalByRefObject
    {
        private readonly Client client;
        private readonly int id;
        private readonly object synchronize;

        private CircuitBuildFlags buildFlags;
        private CircuitHSState hsState;
        private List<string> paths;
        private CircuitPurpose purpose;
        private CircuitReason reason;
        private RouterCollection routers;
        private CircuitStatus status;
        private DateTime timeCreated;

        /// <summary>
        /// Initializes a new instance of the <see cref="Circuit"/> class.
        /// </summary>
        /// <param name="client">The client for which the circuit belongs.</param>
        /// <param name="id">The unique identifier of the circuit within the tor session.</param>
        internal Circuit(Client client, int id)
        {
            this.buildFlags = CircuitBuildFlags.None;
            this.client = client;
            this.hsState = CircuitHSState.None;
            this.id = id;
            this.paths = new List<string>();
            this.purpose = CircuitPurpose.None;
            this.reason = CircuitReason.None;
            this.synchronize = new object();
            this.timeCreated = DateTime.MinValue;
        }

        #region Properties

        /// <summary>
        /// Gets the build flags associated with the circuit.
        /// </summary>
        public CircuitBuildFlags BuildFlags
        {
            get { return buildFlags; }
            internal set { buildFlags = value; }
        }

        /// <summary>
        /// Gets the hidden-service state of the circuit.
        /// </summary>
        public CircuitHSState HSState
        {
            get { return hsState; }
            internal set { hsState = value; }
        }

        /// <summary>
        /// Gets the unique identifier of the circuit in the tor session.
        /// </summary>
        public int ID
        {
            get { return id; }
        }

        /// <summary>
        /// Gets the purpose of the circuit.
        /// </summary>
        public CircuitPurpose Purpose
        {
            get { return purpose; }
            internal set { purpose = value; }
        }

        /// <summary>
        /// Gets the reason associated with the circuit, usually assigned upon closed or failed events.
        /// </summary>
        public CircuitReason Reason
        {
            get { return reason; }
            internal set { reason = value; }
        }

        /// <summary>
        /// Gets the routers associated with the circuit.
        /// </summary>
        public RouterCollection Routers
        {
            get { return routers; }
        }

        /// <summary>
        /// Gets the status of the circuit.
        /// </summary>
        public CircuitStatus Status
        {
            get { return status; }
            internal set { status = value; }
        }

        /// <summary>
        /// Gets the date and time the circuit was created.
        /// </summary>
        public DateTime TimeCreated
        {
            get { return timeCreated; }
            internal set { timeCreated = value; }
        }

        /// <summary>
        /// Gets or sets the collection containing the paths associated with the circuit.
        /// </summary>
        internal List<string> Paths
        {
            get { lock (synchronize) return paths; }
            set { lock (synchronize) paths = value; }
        }
        
        #endregion

        /// <summary>
        /// Sends a request to the associated tor client to close the circuit.
        /// </summary>
        /// <returns><c>true</c> if the circuit is closed successfully; otherwise, <c>false</c>.</returns>
        public bool Close()
        {
            return client.Controller.CloseCircuit(this);
        }

        /// <summary>
        /// Sends a request to the associated tor client to extend the circuit.
        /// </summary>
        /// <param name="routers">The list of identities or nicknames to extend onto this circuit.</param>
        /// <returns><c>true</c> if the circuit is extended successfully; otherwise, <c>false</c>.</returns>
        public bool Extend(params string[] routers)
        {
            return client.Controller.ExtendCircuit(this, routers);
        }

        /// <summary>
        /// Sends a request to the associated tor client to extend the circuit.
        /// </summary>
        /// <param name="routers">The list of routers to extend onto this circuit.</param>
        /// <returns><c>true</c> if the circuit is extended successfully; otherwise, <c>false</c>.</returns>
        public bool Extend(params Router[] routers)
        {
            string[] nicknames = new string[routers.Length];

            for (int i = 0, length = routers.Length; i < length; i++)
                nicknames[i] = routers[i].Nickname;

            return client.Controller.ExtendCircuit(this, nicknames);
        }

        /// <summary>
        /// Gets the routers associated with the circuit.
        /// </summary>
        /// <returns>A <see cref="RouterCollection"/> object instance.</returns>
        internal RouterCollection GetRouters()
        {
            lock (synchronize)
            {
                List<Router> routers = new List<Router>();

                if (paths == null || paths.Count == 0)
                {
                    this.routers = new RouterCollection(routers);
                    return this.routers;
                }

                foreach (string path in paths)
                {
                    string trimmed = path;

                    if (trimmed == null)
                        continue;

                    if (trimmed.StartsWith("$"))
                        trimmed = trimmed.Substring(1);
                    if (trimmed.Contains("~"))
                        trimmed = trimmed.Substring(0, trimmed.IndexOf("~"));

                    if (string.IsNullOrWhiteSpace(trimmed))
                        continue;

                    GetRouterStatusCommand command = new GetRouterStatusCommand(trimmed);
                    GetRouterStatusResponse response = command.Dispatch(client);

                    if (response.Success && response.Router != null)
                        routers.Add(response.Router);
                }

                this.routers = new RouterCollection(routers);
                return this.routers;
            }
        }
    }

    /// <summary>
    /// A class containing a read-only collection of <see cref="Circuit"/> objects.
    /// </summary>
    public sealed class CircuitCollection : ReadOnlyCollection<Circuit>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CircuitCollection"/> class.
        /// </summary>
        /// <param name="list">The list of circuits.</param>
        internal CircuitCollection(IList<Circuit> list) : base(list)
        {
        }
    }
}
