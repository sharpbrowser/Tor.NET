using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tor.Controller
{
    /// <summary>
    /// A class containing methods for performing control operations against the tor application.
    /// </summary>
    public sealed class Control : MarshalByRefObject
    {
        private readonly Client client;

        /// <summary>
        /// Initializes a new instance of the <see cref="Control"/> class.
        /// </summary>
        /// <param name="client">The client for which this object instance belongs.</param>
        internal Control(Client client)
        {
            this.client = client;
        }

        /// <summary>
        /// Cleans the current circuits in the tor application by requesting new circuits be generated.
        /// </summary>
        public bool CleanCircuits()
        {
            return Command<Response>.DispatchAndReturn<SignalNewCircuitCommand>(client);
        }

        /// <summary>
        /// Clears the client-side cache of IP addresses for hostnames.
        /// </summary>
        /// <returns></returns>
        public bool ClearDNSCache()
        {
            return Command<Response>.DispatchAndReturn<SignalClearDNSCacheCommand>(client);
        }

        /// <summary>
        /// Closes an existing circuit within the tor service.
        /// </summary>
        /// <param name="circuit">The circuit which should be closed.</param>
        /// <returns><c>true</c> if the circuit was closed successfully; otherwise, <c>false</c>.</returns>
        public bool CloseCircuit(Circuit circuit)
        {
            if (circuit == null)
                throw new ArgumentNullException("circuit");
            if (circuit.ID == 0)
                throw new ArgumentException("The circuit has an invalid ID", "circuit");

            CloseCircuitCommand command = new CloseCircuitCommand(circuit);
            Response response = command.Dispatch(client);
            return response.Success;
        }

        /// <summary>
        /// Closes an existing stream within the tor service.
        /// </summary>
        /// <param name="stream">The stream which should be closed.</param>
        /// <param name="reason">The reason for the stream being closed.</param>
        /// <returns><c>true</c> if the stream was closed successfully; otherwise, <c>false</c>.</returns>
        public bool CloseStream(Stream stream, StreamReason reason)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (stream.ID == 0)
                throw new ArgumentException("The stream has an invalid ID", "stream");
            if (reason == StreamReason.None || reason == StreamReason.End || reason == StreamReason.PrivateAddr)
                throw new ArgumentOutOfRangeException("reason", "The reason for closure cannot be None, End or PrivateAddr");

            CloseStreamCommand command = new CloseStreamCommand(stream, reason);
            Response response = command.Dispatch(client);
            return response.Success;
        }

        /// <summary>
        /// Creates a new circuit within the tor service, and allow tor to select the routers.
        /// </summary>
        /// <returns><c>true</c> if the circuit is created successfully; otherwise, <c>false</c>.</returns>
        public bool CreateCircuit()
        {
            CreateCircuitCommand command = new CreateCircuitCommand();
            CreateCircuitResponse response = command.Dispatch(client);
            return response.Success && response.CircuitID >= 0;
        }

        /// <summary>
        /// Creates a new circuit within the tor service comprised of a series of specified routers.
        /// </summary>
        /// <returns><c>true</c> if the circuit is created successfully; otherwise, <c>false</c>.</returns>
        public bool CreateCircuit(params string[] routers)
        {
            CreateCircuitCommand command = new CreateCircuitCommand(routers);
            CreateCircuitResponse response = command.Dispatch(client);
            return response.Success && response.CircuitID >= 0;
        }

        /// <summary>
        /// Extends an existing circuit by attaching a new router onto the path.
        /// </summary>
        /// <param name="circuit">The circuit which should be extended.</param>
        /// <param name="routers">The list of router identities or nicknames to extend onto the circuit.</param>
        /// <returns><c>true</c> if the circuit was extended successfully; otherwise, <c>false</c>.</returns>
        public bool ExtendCircuit(Circuit circuit, params string[] routers)
        {
            if (circuit == null)
                throw new ArgumentNullException("circuit");
            if (circuit.ID == 0)
                throw new ArgumentException("The circuit has an invalid ID", "circuit");
            if (routers.Length == 0)
                throw new ArgumentOutOfRangeException("routers", "At least one router should be supplied with the extend");

            ExtendCircuitCommand command = new ExtendCircuitCommand(circuit);
            command.Routers.AddRange(routers);

            Response response = command.Dispatch(client);
            return response.Success;
        }
    }
}
