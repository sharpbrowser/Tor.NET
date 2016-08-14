using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Tor
{
    /// <summary>
    /// A class containing the configurations needed to connect to a remotely hosted tor application executable.
    /// </summary>
    [DebuggerStepThrough]
    [Serializable]
    public sealed class ClientRemoteParams
    {
        private string address;
        private string controlPassword;
        private int controlPort;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientRemoteParams"/> class.
        /// </summary>
        public ClientRemoteParams() : this(null, 9051, "")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientRemoteParams"/> class.
        /// </summary>
        /// <param name="address">The address of the hosted tor application.</param>
        public ClientRemoteParams(string address) : this(address, 9051, "")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientRemoteParams"/> class.
        /// </summary>
        /// <param name="address">The address of the hosted tor application.</param>
        /// <param name="controlPort">The port number which the application will listening on for control connections.</param>
        public ClientRemoteParams(string address, int controlPort) : this(address, controlPort, "")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientRemoteParams"/> class.
        /// </summary>
        /// <param name="address">The address of the hosted tor application.</param>
        /// <param name="controlPort">The port number which the application will listening on for control connections.</param>
        /// <param name="controlPassword">The password used when authenticating with the control connection.</param>
        public ClientRemoteParams(string address, int controlPort, string controlPassword)
        {
            this.address = address;
            this.controlPassword = controlPassword;
            this.controlPort = controlPort;
        }

        #region Properties

        /// <summary>
        /// Gets or sets the address of the hosted tor application.
        /// </summary>
        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        /// <summary>
        /// Gets or sets the password used when authenticating with the tor application on the control connection. A value of
        /// <c>null</c> or blank indicates that no password has been configured.
        /// </summary>
        public string ControlPassword
        {
            get { return controlPassword; }
            set { controlPassword = value; }
        }

        /// <summary>
        /// Gets or sets the port number which the application will be listening on for control connections.
        /// </summary>
        public int ControlPort
        {
            get { return controlPort; }
            set { controlPort = value; }
        }

        #endregion

        /// <summary>
        /// Validates the parameters assigned to the object, and throws relevant exceptions where necessary.
        /// </summary>
        internal void ValidateParameters()
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new TorException("The address cannot be null or white-space");
            if (controlPort <= 0 || short.MaxValue < controlPort)
                throw new TorException("The control port number must be within a valid port range");
        }
    }
}
