using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tor.Controller;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Tor.Status
{
    /// <summary>
    /// A class containing methods and properties for reading the status of the tor network service.
    /// </summary>
    [DebuggerStepThrough]
    public sealed class Status : MarshalByRefObject
    {
        private const int MaximumRecords = 30;

        private readonly List<Circuit> circuits;
        private readonly Client client;
        private readonly List<ORConnection> orConnections;
        private readonly List<Stream> streams;
        private readonly object synchronizeCircuits;
        private readonly object synchronizeORConnections;
        private readonly object synchronizeStreams;

        /// <summary>
        /// Initializes a new instance of the <see cref="Status"/> class.
        /// </summary>
        /// <param name="client">The client for which this object instance belongs.</param>
        internal Status(Client client)
        {
            this.circuits = new List<Circuit>();
            this.client = client;
            this.orConnections = new List<ORConnection>();
            this.streams = new List<Stream>();
            this.synchronizeCircuits = new object();
            this.synchronizeORConnections = new object();
            this.synchronizeStreams = new object();
        }

        #region Properties

        /// <summary>
        /// Gets the current circuits configured against the tor client.
        /// </summary>
        public CircuitCollection Circuits
        {
            get { lock (synchronizeCircuits) return new CircuitCollection(circuits); }
        }

        /// <summary>
        /// Gets a value indicating whether the tor software service is dormant. A value of <c>true</c> indicates that
        /// the tor network has not been active for a while, or is dormant for some other reason.
        /// </summary>
        public bool IsDormant
        {
            get { return PropertyGetIsDormant(); }
        }

        /// <summary>
        /// Gets the current OR connections configured against the tor client.
        /// </summary>
        public ORConnectionCollection ORConnections
        {
            get { lock (synchronizeORConnections) return new ORConnectionCollection(orConnections); }
        }

        /// <summary>
        /// Gets the current streams configured against the tor client.
        /// </summary>
        public StreamCollection Streams
        {
            get { lock (synchronizeStreams) return new StreamCollection(streams); }
        }

        /// <summary>
        /// Gets an approximation of the total bytes downloaded by the tor software.
        /// </summary>
        public Bytes TotalBytesDownloaded
        {
            get { return PropertyGetTotalBytesDownloaded(); }
        }

        /// <summary>
        /// Gets an approximation of the total bytes uploaded by the tor software.
        /// </summary>
        public Bytes TotalBytesUploaded
        {
            get { return PropertyGetTotalBytesUploaded(); }
        }

        /// <summary>
        /// Gets the version number of the running tor application associated with this client.
        /// </summary>
        public Version Version
        {
            get { return PropertyGetVersion(); }
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the bandwidth download and upload values change within the tor service. These values are a report of the
        /// download and upload rates within the last second.
        /// </summary>
        public event BandwidthEventHandler BandwidthChanged
        {
            add { client.Events.BandwidthChanged += value; }
            remove { client.Events.BandwidthChanged -= value; }
        }

        /// <summary>
        /// Occurs when the circuits have been updated.
        /// </summary>
        public event EventHandler CircuitsChanged;

        /// <summary>
        /// Occurs when the OR connections have been updated.
        /// </summary>
        public event EventHandler ORConnectionsChanged;

        /// <summary>
        /// Occurs when the streams have been updated.
        /// </summary>
        public event EventHandler StreamsChanged;

        #endregion

        #region Tor.Events.Events

        /// <summary>
        /// Called when a circuit changes within the tor service.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="CircuitEventArgs"/> instance containing the event data.</param>
        private void OnCircuitChanged(object sender, CircuitEventArgs e)
        {
            if (e.Circuit == null)
                return;

            lock (synchronizeCircuits)
            {
                Circuit existing = circuits.Where(c => c.ID == e.Circuit.ID).FirstOrDefault();

                if (existing == null)
                {
                    if (Status.MaximumRecords <= circuits.Count)
                    {
                        Circuit removal = circuits.Where(c => c.Status == CircuitStatus.Closed || c.Status == CircuitStatus.Failed).FirstOrDefault();

                        if (removal == null)
                            return;

                        circuits.Remove(removal);
                    }

                    existing = e.Circuit;
                    existing.GetRouters();
                    circuits.Add(existing);
                }
                else
                {
                    bool update = existing.Paths.Count != e.Circuit.Paths.Count || e.Circuit.Status == CircuitStatus.Extended;

                    existing.BuildFlags = e.Circuit.BuildFlags;
                    existing.HSState = e.Circuit.HSState;
                    existing.Paths = e.Circuit.Paths;
                    existing.Purpose = e.Circuit.Purpose;
                    existing.Reason = e.Circuit.Reason;
                    existing.Status = e.Circuit.Status;
                    existing.TimeCreated = e.Circuit.TimeCreated;

                    if (update)
                        existing.GetRouters();
                }
            }

            if (CircuitsChanged != null)
                CircuitsChanged(client, EventArgs.Empty);
        }

        /// <summary>
        /// Called when an OR connection changes within the tor service.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ORConnectionEventArgs"/> instance containing the event data.</param>
        private void OnORConnectionChanged(object sender, ORConnectionEventArgs e)
        {
            if (e.Connection == null)
                return;

            lock (synchronizeORConnections)
            {
                ORConnection existing;

                if (e.Connection.ID != 0)
                    existing = orConnections.Where(o => o.ID == e.Connection.ID).FirstOrDefault();
                else
                    existing = orConnections.Where(o => o.Target.Equals(e.Connection.Target, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

                if (existing == null)
                {
                    if (Status.MaximumRecords <= orConnections.Count)
                    {
                        ORConnection removal = orConnections.Where(o => o.Status == ORStatus.Closed || o.Status == ORStatus.Failed).FirstOrDefault();

                        if (removal == null)
                            return;

                        orConnections.Remove(removal);
                    }

                    existing = e.Connection;
                    orConnections.Add(existing);
                }
                else
                {
                    existing.CircuitCount = e.Connection.CircuitCount;
                    existing.ID = e.Connection.ID;
                    existing.Reason = e.Connection.Reason;
                    existing.Status = e.Connection.Status;
                    existing.Target = e.Connection.Target;
                }
            }

            if (ORConnectionsChanged != null)
                ORConnectionsChanged(client, EventArgs.Empty);
        }

        /// <summary>
        /// Called when a stream changes within the tor service.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="StreamEventArgs"/> instance containing the event data.</param>
        private void OnStreamChanged(object sender, StreamEventArgs e)
        {
            if (e.Stream == null)
                return;

            lock (synchronizeStreams)
            {
                Stream existing = streams.Where(s => s.ID == e.Stream.ID).FirstOrDefault();

                if (existing == null)
                {
                    if (Status.MaximumRecords <= streams.Count)
                    {
                        Stream removal = streams.Where(s => s.Status == StreamStatus.Closed || s.Status == StreamStatus.Failed).FirstOrDefault();

                        if (removal == null)
                            return;

                        streams.Remove(removal);
                    }

                    existing = e.Stream;
                    streams.Add(existing);
                }
                else
                {
                    existing.CircuitID = e.Stream.CircuitID;
                    existing.Purpose = e.Stream.Purpose;
                    existing.Reason = e.Stream.Reason;
                    existing.Status = e.Stream.Status;
                }
            }

            if (StreamsChanged != null)
                StreamsChanged(client, EventArgs.Empty);
        }

        #endregion

        /// <summary>
        /// Gets a read-only collection of all ORs which the tor application has an opinion about. This method can be time, memory
        /// and CPU intensive, so should be used infrequently.
        /// </summary>
        /// <returns>A <see cref="RouterCollection"/> object instance containing the router information; otherwise, <c>null</c> if the request fails.</returns>
        public RouterCollection GetAllRouters()
        {
            GetAllRouterStatusCommand command = new GetAllRouterStatusCommand();
            GetAllRouterStatusResponse response = command.Dispatch(client);

            if (response.Success)
                return response.Routers;

            return null;
        }

        /// <summary>
        /// Gets a country code for an IP address. This method will not work unless a <c>geoip</c> and/or <c>geoip6</c> file has been supplied.
        /// </summary>
        /// <param name="ipAddress">The IP address which should be resolved.</param>
        /// <returns>A <see cref="System.String"/> containing the country code; otherwise, <c>null</c> if the country code could not be resolved.</returns>
        public string GetCountryCode(string ipAddress)
        {
            if (ipAddress == null)
                throw new ArgumentNullException("router");

            GetInfoCommand command = new GetInfoCommand(string.Format("ip-to-country/{0}", ipAddress));
            GetInfoResponse response = command.Dispatch(client);

            if (response.Success)
            {
                string[] values = response.Values[0].Split(new[] { '=' }, 2);

                if (values.Length == 2)
                    return values[1].Trim();
            }

            return null;
        }

        /// <summary>
        /// Gets a country code for a router within the tor network. This method will not work unless a <c>geoip</c> and/or <c>geoip6</c> file has been supplied.
        /// </summary>
        /// <param name="router">The router to retrieve the country code for.</param>
        /// <returns>A <see cref="System.String"/> containing the country code; otherwise, <c>null</c> if the country code could not be resolved.</returns>
        public string GetCountryCode(Router router)
        {
            if (router == null)
                throw new ArgumentNullException("router");

            string address = router.IPAddress.ToString();

            GetInfoCommand command = new GetInfoCommand(string.Format("ip-to-country/{0}", address));
            GetInfoResponse response = command.Dispatch(client);

            if (response.Success)
            {
                string[] values = response.Values[0].Split(new[] { '=' }, 2);

                if (values.Length == 2)
                    return values[1].Trim();

                return values[0].Trim().ToUpper();
            }

            return null;
        }
                
        /// <summary>
        /// Gets a value indicating whether the tor software service is dormant.
        /// </summary>
        /// <returns><c>true</c> if the tor software service is dormant; otherwise, <c>false</c>.</returns>
        private bool PropertyGetIsDormant()
        {
            GetInfoCommand command = new GetInfoCommand("dormant");
            GetInfoResponse response = command.Dispatch(client);

            if (!response.Success)
                return false;

            int value;

            if (!int.TryParse(response.Values[0], out value))
                return false;

            return value != 0;
        }

        /// <summary>
        /// Gets an approximation of the total bytes downloaded by the tor software.
        /// </summary>
        /// <returns>A <see cref="Bytes"/> object instance containing the estimated number of bytes.</returns>
        private Bytes PropertyGetTotalBytesDownloaded()
        {
            GetInfoCommand command = new GetInfoCommand("traffic/read");
            GetInfoResponse response = command.Dispatch(client);

            if (!response.Success)
                return Bytes.Empty;

            double value;

            if (!double.TryParse(response.Values[0], out value))
                return Bytes.Empty;

            return new Bytes(value).Normalize();
        }

        /// <summary>
        /// Gets an approximation of the total bytes uploaded by the tor software.
        /// </summary>
        /// <returns>A <see cref="Bytes"/> object instance containing the estimated number of bytes.</returns>
        private Bytes PropertyGetTotalBytesUploaded()
        {
            GetInfoCommand command = new GetInfoCommand("traffic/written");
            GetInfoResponse response = command.Dispatch(client);

            if (!response.Success)
                return Bytes.Empty;

            double value;

            if (!double.TryParse(response.Values[0], out value))
                return Bytes.Empty;

            return new Bytes(value).Normalize();
        }

        /// <summary>
        /// Gets the version of the running tor application.
        /// </summary>
        /// <returns>A <see cref="Version"/> object instance containing the version.</returns>
        private Version PropertyGetVersion()
        {
            GetInfoCommand command = new GetInfoCommand("version");
            GetInfoResponse response = command.Dispatch(client);

            if (!response.Success)
                return new Version();

            Regex pattern = new Regex(@"(?<major>\d{1,})\.(?<minor>\d{1,})\.(?<build>\d{1,})\.(?<revision>\d{1,})(?:$|\s)");
            Match match = pattern.Match(response.Values[0]);

            if (match.Success)
                return new Version(
                    Convert.ToInt32(match.Groups["major"].Value),
                    Convert.ToInt32(match.Groups["minor"].Value),
                    Convert.ToInt32(match.Groups["build"].Value),
                    Convert.ToInt32(match.Groups["revision"].Value)
                );
            
            return new Version();
        }

        /// <summary>
        /// Starts the status controller listening for changes within the tor client.
        /// </summary>
        internal void Start()
        {
            this.client.Events.CircuitChanged += new CircuitEventHandler(OnCircuitChanged);
            this.client.Events.ORConnectionChanged += new ORConnectionEventHandler(OnORConnectionChanged);
            this.client.Events.StreamChanged += new StreamEventHandler(OnStreamChanged);
        }
    }
}
