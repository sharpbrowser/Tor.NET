using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Tor.Config;
using System.IO;
using Tor.Controller;
using Tor.Helpers;
using Tor.IO;
using System.Threading;

namespace Tor
{
    /// <summary>
    /// A class linked to a running tor application process, and provides methods and properties for interacting with the tor service.
    /// </summary>
    public sealed class Client : MarshalByRefObject, IDisposable
    {
        private readonly static Version minimumSupportedVersion = new Version(0, 2, 0, 9);

        private readonly ClientCreateParams createParams;
        private readonly ClientRemoteParams remoteParams;
        private readonly object synchronize;

        private Configuration configuration;
        private Control controller;
        private Events.Events events;
        private volatile bool disposed;
        private Logging.Logging logging;
        private Process process;
        private Proxy.Proxy proxy;
        private Status.Status status;

        /// <summary>
        /// Initializes a new instance of the <see cref="Client"/> class.
        /// </summary>
        /// <param name="createParams">The parameters used when creating the client.</param>
        private Client(ClientCreateParams createParams)
        {
            this.createParams = createParams;
            this.disposed = false;
            this.process = null;
            this.remoteParams = null;
            this.synchronize = new object();

            this.Start();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Client"/> class.
        /// </summary>
        /// <param name="remoateParams">The parameters used when connecting to the client.</param>
        private Client(ClientRemoteParams remoteParams)
        {
            this.createParams = null;
            this.disposed = false;
            this.process = null;
            this.remoteParams = remoteParams;
            this.synchronize = new object();

            this.Start();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="Client"/> class.
        /// </summary>
        ~Client()
        {
            Dispose(false);
        }

        #region Properties

        /// <summary>
        /// Gets an object containing configuration values associated with the tor application.
        /// </summary>
        public Configuration Configuration
        {
            get { return configuration; }
        }

        /// <summary>
        /// Gets an object which can be used for performing control operations against the tor application.
        /// </summary>
        public Control Controller
        {
            get { return controller; }
        }

        /// <summary>
        /// Gets a value indicating whether the client is configured to a remote tor application.
        /// </summary>
        public bool IsRemote
        {
            get { return remoteParams != null; }
        }

        /// <summary>
        /// Gets a value indicating whether the tor application is still running for this client.
        /// </summary>
        public bool IsRunning
        {
            get { if (IsRemote) return true; lock (synchronize) return process != null && !process.HasExited; }
        }

        /// <summary>
        /// Gets an object which can be used to receive log messages from the tor client.
        /// </summary>
        public Logging.Logging Logging
        {
            get { return logging; }
        }

        /// <summary>
        /// Gets an object which manages the hosted HTTP proxy and can be used to create an <see cref="IWebProxy"/> object instance.
        /// </summary>
        public Proxy.Proxy Proxy
        {
            get { return proxy; }
        }

        /// <summary>
        /// Gets an object which provides methods and properties for determining the status of the tor network service.
        /// </summary>
        public Status.Status Status
        {
            get { return status; }
        }

        /// <summary>
        /// Gets an object which can be used to monitor for events within the tor service.
        /// </summary>
        internal Events.Events Events
        {
            get { return events; }
        }

        /// <summary>
        /// Gets the minimum supported tor version number. The version number is checked after being launched or connected, and will raise an
        /// exception if the minimum version number is not satisfied.
        /// </summary>
        public static Version MinimumSupportedVersion
        {
            get { return minimumSupportedVersion; }
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the client has been shutdown, either from manual shutdown or by forcible means.
        /// </summary>
        public event EventHandler Shutdown;

        #endregion

        #region System.Diagnostics.Process

        /// <summary>
        /// Called when the process has exited.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnHandleProcessExited(object sender, EventArgs e)
        {
            Stop(true);
        }

        #endregion

        #region System.IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        private void Dispose(bool disposing)
        {
            if (disposed)
                return;

            Stop(false);
            disposed = true;
        }

        #endregion

        /// <summary>
        /// Creates a new <see cref="Client"/> object instance and attempts to launch the tor application executable.
        /// </summary>
        /// <param name="createParams">The parameters used when creating the client.</param>
        /// <returns>A <see cref="Client"/> object instance.</returns>
        public static Client Create(ClientCreateParams createParams)
        {
            if (createParams == null)
                throw new ArgumentNullException("createParams");

            return new Client(createParams);
        }

        /// <summary>
        /// Creates a new <see cref="Client"/> object instance configured to connect to a remotely hosted tor application executable.
        /// </summary>
        /// <param name="remoateParams">The parameters used when connecting to the client.</param>
        /// <returns>A <see cref="Client"/> object instance.</returns>
        public static Client CreateForRemote(ClientRemoteParams remoteParams)
        {
            if (remoteParams == null)
                throw new ArgumentNullException("remoteParams");

            return new Client(remoteParams);
        }

        /// <summary>
        /// Gets the address hosting the tor application.
        /// </summary>
        /// <returns>A <see cref="System.String"/> containing the host address.</returns>
        internal string GetClientAddress()
        {
            if (IsRemote)
                return remoteParams.Address;
            else
                return "127.0.0.1";
        }

        /// <summary>
        /// Gets the configurations from the tor application by dispatching the <c>getconf</c> command.
        /// </summary>
        private void GetClientConfigurations()
        {
            List<string> configurations = ReflectionHelper.GetEnumeratorAttributes<ConfigurationNames, ConfigurationAssocAttribute, string>(attr => attr.Name);
            GetConfCommand command = new GetConfCommand(configurations);
            GetConfResponse response = command.Dispatch(this);

            if (!response.Success)
                throw new TorException("The client failed to retrieve configuration values from the tor application (check your control port and password)");

            foreach (KeyValuePair<string, string> value in response.Values)
            {
                string key = value.Key;
                string val = value.Value;
                
                configuration.SetValueDirect(key, val);
            }

            Version version = status.Version;
            Version empty = new Version();

            if (empty < version && version < minimumSupportedVersion)
            {
                Dispose();
                throw new TorException("This version of tor is not supported, please use version " + minimumSupportedVersion + " or higher");
            }
        }

        /// <summary>
        /// Gets the control password to use in the control connection of the hosted tor application, based on
        /// the parameters supplied.
        /// </summary>
        /// <returns>A <see cref="System.String"/> containing the control port password.</returns>
        internal string GetControlPassword()
        {
            if (IsRemote)
                return remoteParams.ControlPassword ?? "";
            else
                return createParams.ControlPassword ?? "";
        }

        /// <summary>
        /// Gets the control port of the hosted tor application based on the parameters supplied.
        /// </summary>
        /// <returns>A <see cref="System.Int32"/> containing the control port number.</returns>
        internal int GetControlPort()
        {
            if (IsRemote)
                return remoteParams.ControlPort;
            else
                return createParams.ControlPort;
        }

        /// <summary>
        /// Gets a <see cref="System.IO.Stream"/> for the running client. This method will establish a connection to the specified
        /// host address and port number, and function as an intermediary for communications.
        /// </summary>
        /// <returns>A <see cref="System.IO.Stream"/> object instance connected to the specified host address and port through the tor network.</returns>
        public System.IO.Stream GetStream(string host, int port)
        {
            return new Socks5Stream(this, host, port);
        }

        /// <summary>
        /// Starts the tor application executable using the provided creation parameters.
        /// </summary>
        private void Start()
        {
            if (createParams != null)
                createParams.ValidateParameters();
            else
                remoteParams.ValidateParameters();

            if (createParams != null)
            {
                lock (synchronize)
                {
                    ProcessStartInfo psi;

                    if (process != null && !process.HasExited)
                        return;

                    psi = new ProcessStartInfo(createParams.Path);
                    psi.Arguments = createParams.ToString();
                    psi.CreateNoWindow = true;
                    psi.UseShellExecute = false;
                    psi.WindowStyle = ProcessWindowStyle.Hidden;
                    psi.WorkingDirectory = Path.GetDirectoryName(createParams.Path);

                    try
                    {
                        process = new Process();
                        process.EnableRaisingEvents = true;
                        process.Exited += new EventHandler(OnHandleProcessExited);
                        process.StartInfo = psi;

                        if (!process.Start())
                        {
                            process.Dispose();
                            process = null;

                            throw new TorException("The tor application process failed to launch");
                        }
                    }
                    catch (Exception exception)
                    {
                        throw new TorException("The tor application process failed to launch", exception);
                    }
                }
            }

            Thread.Sleep(500);

            lock (synchronize)
            {
                configuration = new Configuration(this);
                controller = new Control(this);
                logging = new Logging.Logging(this);
                proxy = new Proxy.Proxy(this);
                status = new Status.Status(this);

                events = new Events.Events(this);
                events.Start((Action)delegate
                {
                    configuration.Start();
                    status.Start();

                    GetClientConfigurations();
                });
            }
        }
        
        /// <summary>
        /// Shuts down the tor application process and releases the associated components of the class.
        /// </summary>
        /// <param name="exited">A value indicating whether the shutdown is being performed after the process has exited.</param>
        private void Stop(bool exited)
        {
            if (disposed && !exited)
                return;

            lock (synchronize)
            {
                if (!IsRemote && process != null)
                {
                    if (exited)
                    {
                        process.Dispose();
                        process = null;
                    }
                    else
                    {
                        SignalHaltCommand command = new SignalHaltCommand();
                        Response response = command.Dispatch(this);

                        if (response.Success)
                            return;

                        process.Kill();
                        process.Dispose();
                        process = null;
                    }
                }

                if (proxy != null)
                {
                    proxy.Dispose();
                    proxy = null;
                }

                if (events != null)
                {
                    events.Dispose();
                    events = null;
                }

                configuration = null;
                controller = null;
                logging = null;
                status = null;

                if (Shutdown != null)
                    Shutdown(this, EventArgs.Empty);
            }
        }
    }
}
