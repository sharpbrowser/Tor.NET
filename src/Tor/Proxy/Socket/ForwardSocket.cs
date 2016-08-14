using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Tor.Proxy
{
    /// <summary>
    /// A class which extends the <see cref="Socket"/> class implementation for automated SOCKS5 protocol support.
    /// </summary>
    internal sealed class ForwardSocket : Socket
    {
        private readonly Client client;

        private Socks5AsyncResult asyncResult;
        private AsyncCallback connectCallback;
        private Socks5Processor processor;
        private string proxyAddress;
        private int proxyPort;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForwardSocket"/> class.
        /// </summary>
        /// <param name="client">The client hosting the proxy connection.</param>
        /// <param name="connection">The connection associated with the originating client connection.</param>
        public ForwardSocket(Client client, Connection connection) : base(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        {
            this.asyncResult = null;
            this.client = client;
            this.connectCallback = null;
            this.processor = null;
            this.proxyAddress = null;
            this.proxyPort = -1;
        }

        #region Properties

        /// <summary>
        /// Gets or sets the address of the SOCKS5 proxy host.
        /// </summary>
        public string ProxyAddress
        {
            get { return proxyAddress; }
            set { proxyAddress = value; }
        }

        /// <summary>
        /// Gets or sets the port number of the SOCKS5 proxy host.
        /// </summary>
        public int ProxyPort
        {
            get { return proxyPort; }
            set { proxyPort = value; }
        }

        #endregion

        #region System.Net.Dns

        /// <summary>
        /// Called when the DNS resolved a host.
        /// </summary>
        /// <param name="ar">The asynchronous result object for the asynchronous method.</param>
        private void OnDnsGetHostEntry(IAsyncResult ar)
        {
            try
            {
                IPHostEntry entry = Dns.EndGetHostEntry(ar);
                DNSResolveState state = (DNSResolveState)ar.AsyncState;

                base.BeginConnect(new IPEndPoint(entry.AddressList[0], state.Port), OnSocketBeginConnect, state.State);
            }
            catch (Exception exception)
            {
                throw new TorException("The fowarding socket failed to resolve a hostname", exception);
            }
        }

        #endregion

        #region System.Net.Sockets.Socket

        /// <summary>
        /// Begins an asynchronous request for a remote host connection.
        /// </summary>
        /// <param name="remoteEP">An <see cref="T:System.Net.EndPoint" /> that represents the remote host.</param>
        /// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate.</param>
        /// <param name="state">An object that contains state information for this request.</param>
        /// <returns>
        /// An <see cref="T:System.IAsyncResult" /> that references the asynchronous connection.
        /// </returns>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Net.SocketPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        /// </PermissionSet>
        public new IAsyncResult BeginConnect(EndPoint remoteEP, AsyncCallback callback, object state)
        {
            if (remoteEP == null)
                throw new ArgumentNullException("remoteEP");
            if (callback == null)
                throw new ArgumentNullException("callback");

            if (proxyAddress == null || proxyPort == -1)
                return base.BeginConnect(remoteEP, callback, state);
            else
            {
                connectCallback = callback;

                processor = new Socks5Processor(this);
                asyncResult = processor.BeginProcessing((IPEndPoint)remoteEP, OnConnectionEstablished);

                return asyncResult;
            }
        }

        /// <summary>
        /// Begins an asynchronous request for a remote host connection.
        /// </summary>
        /// <param name="host">The host address to connect to.</param>
        /// <param name="port">The port number to connect to.</param>
        /// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate.</param>
        /// <param name="state">An object that contains state information for this request.</param>
        /// <returns>
        /// An <see cref="T:System.IAsyncResult" /> that references the asynchronous connection.
        /// </returns>
        public new IAsyncResult BeginConnect(string host, int port, AsyncCallback callback, object state)
        {
            if (host == null)
                throw new ArgumentNullException("host");
            if (callback == null)
                throw new ArgumentNullException("callback");

            connectCallback = callback;

            if (proxyAddress == null || proxyPort == -1)
                return BeginDNSResolve(host, port, callback, state);
            else
            {
                processor = new Socks5Processor(this);
                asyncResult = processor.BeginProcessing(host, port, OnConnectionEstablished);

                return asyncResult;
            }
        }

        /// <summary>
        /// Establishes a connection to a remote host.
        /// </summary>
        /// <param name="remoteEP">An <see cref="T:System.Net.EndPoint" /> that represents the remote device.</param>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Net.SocketPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        /// </PermissionSet>
        public new void Connect(EndPoint remoteEP)
        {
            if (remoteEP == null)
                throw new ArgumentNullException("remoteEP");

            if (proxyAddress == null || proxyPort == -1)
                base.Connect(remoteEP);
            else
            {
                processor = new Socks5Processor(this);
                asyncResult = processor.BeginProcessing((IPEndPoint)remoteEP, OnConnectionEstablished);
                asyncResult.AsyncWaitHandle.WaitOne();
            }
        }

        /// <summary>
        /// Ends a pending asynchronous connection request.
        /// </summary>
        /// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> that stores state information and any user defined data for this asynchronous operation.</param>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
        /// </PermissionSet>
        public new void EndConnect(IAsyncResult asyncResult)
        {
            if (asyncResult == null)
                throw new ArgumentNullException("asyncResult");
            if (asyncResult.IsCompleted)
                return;

            throw new InvalidOperationException("The socket has not yet completed processing, this is an invalid call");
        }

        /// <summary>
        /// Called when the socket has connected following a DNS resolution.
        /// </summary>
        /// <param name="ar">The asynchronous result object for the asynchronous method.</param>
        private void OnSocketBeginConnect(IAsyncResult ar)
        {
            try
            {
                base.EndConnect(ar);

                asyncResult.Set();

                if (connectCallback != null)
                    connectCallback(asyncResult);
            }
            catch (Exception exception)
            {
                throw new TorException("The forwarding socket failed to complete the connect operation", exception);
            }
        }

        #endregion

        /// <summary>
        /// Begins an asynchronous request for a remote host connection.
        /// </summary>
        /// <param name="remoteEP">An <see cref="T:System.Net.EndPoint" /> that represents the remote host.</param>
        /// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate.</param>
        /// <param name="state">An object that contains state information for this request.</param>
        /// <returns>
        /// An <see cref="T:System.IAsyncResult" /> that references the asynchronous connection.
        /// </returns>
        internal IAsyncResult BeginConnectInternal(EndPoint remoteEP, AsyncCallback callback, object state)
        {
            return base.BeginConnect(remoteEP, callback, state);
        }

        /// <summary>
        /// Begins an asynchronous request to resolve the DNS information for a host.
        /// </summary>
        /// <param name="host">The host address to resolve.</param>
        /// <param name="port">The port number of the associated host.</param>
        /// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous request.</returns>
        private IAsyncResult BeginDNSResolve(string host, int port, AsyncCallback callback, object state)
        {
            DNSResolveState dnsState;

            try
            {
                dnsState = new DNSResolveState();
                dnsState.Callback = callback;
                dnsState.Host = host;
                dnsState.Port = port;
                dnsState.State = state;

                asyncResult = new Socks5AsyncResult();
                
                Dns.BeginGetHostEntry(host, OnDnsGetHostEntry, dnsState);

                return asyncResult;
            }
            catch (Exception exception)
            {
                throw new TorException("The forwarding socket failed to resolve a host address", exception);
            }
        }

        /// <summary>
        /// Called when the connection has been established to the proxy provider.
        /// </summary>
        /// <param name="success"><c>true</c> if the connection succeeds; otherwise, <c>false</c>.</param>
        private void OnConnectionEstablished(bool success)
        {
            if (success)
            {
                asyncResult.Set();

                if (connectCallback != null)
                    connectCallback(asyncResult);
            }
            else
            {
                Close();
            }
        }

        /// <summary>
        /// A structure which stores information regarding a DNS resolution request.
        /// </summary>
        private struct DNSResolveState
        {
            public AsyncCallback Callback;
            public string Host;
            public int Port;
            public object State;
        }
    }
}
