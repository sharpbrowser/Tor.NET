using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using Tor.Helpers;

namespace Tor.Events
{
    /// <summary>
    /// A class which provides monitoring for events occuring within the tor service.
    /// </summary>
    public sealed class Events : IDisposable
    {
        private readonly static string EOL = "\r\n";

        private readonly Client client;
        private readonly List<Event> events;
        private readonly object synchronize;

        private StringBuilder backlog;
        private byte[] buffer;
        private volatile bool disposed;
        private volatile bool enabled;
        private Socket socket;

        /// <summary>
        /// Initializes a new instance of the <see cref="Events"/> class.
        /// </summary>
        /// <param name="client">The client for which this object instance belongs.</param>
        internal Events(Client client)
        {
            this.backlog = new StringBuilder();
            this.buffer = new byte[256];
            this.client = client;
            this.disposed = false;
            this.enabled = false;
            this.events = new List<Event>();
            this.socket = null;
            this.synchronize = new object();
        }

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the client should monitor for interested events within the tor service.
        /// </summary>
        public bool Enabled
        {
            get
            {
                return enabled;
            }
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
        }

        #endregion

        #region System.Net.Sockets.Socket

        /// <summary>
        /// Called when the socket connects to the control port.
        /// </summary>
        /// <param name="ar">The asynchronous result object for the asynchronous method.</param>
        private void OnSocketConnect(IAsyncResult ar)
        {
            try
            {
                socket.EndConnect(ar);
                socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, OnSocketReceive, null);
            }
            catch { }
        }

        /// <summary>
        /// Called when the socket receives data from the control port.
        /// </summary>
        /// <param name="ar">The asynchronous result object for the asynchronous method.</param>
        private void OnSocketReceive(IAsyncResult ar)
        {
            try
            {
                int received = socket.EndReceive(ar);

                if (received <= 0)
                    return;

                SetEventResponse(Encoding.ASCII.GetString(buffer, 0, received));
                
                socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, OnSocketReceive, null);
            }
            catch { }
        }

        /// <summary>
        /// Called when the socket completes sending a list of interested events.
        /// </summary>
        /// <param name="ar">The asynchronous result object for the asynchronous method.</param>
        private void OnSocketSendEvents(IAsyncResult ar)
        {
            try
            {
                socket.EndSend(ar);
            }
            catch { }
        }

        #endregion

        /// <summary>
        /// Signals to the control connection to begin monitoring for interested events.
        /// </summary>
        private void SetEvents()
        {
            if (disposed)
                throw new ObjectDisposedException("this");

            lock (synchronize)
            {
                if (socket == null || !enabled)
                    return;

                StringBuilder builder = new StringBuilder("SETEVENTS");

                foreach (Event interested in events)
                {
                    string name = ReflectionHelper.GetDescription<Event>(interested);

                    builder.Append(' ');
                    builder.Append(name);
                }
                
                string command = builder.Append("\r\n").ToString();
                byte[] buffer = Encoding.ASCII.GetBytes(command);

                socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, OnSocketSendEvents, null);
            }
        }

        /// <summary>
        /// Sets the current response data received from the control port, which is event information.
        /// </summary>
        /// <param name="response">The response received from the control port.</param>
        private void SetEventResponse(string response)
        {
            if (response == null)
                throw new ArgumentNullException("response");
            if (response.Length == 0)
                return;

            if (backlog.Length > 0)
                response = new StringBuilder(backlog.Length + response.Length).Append(backlog).Append(response).ToString();

            int index = 0;
            int length = response.Length;

            if (response.Contains(EOL))
            {
                for (int next = response.IndexOf(EOL); 0 <= next; next = response.IndexOf(EOL, next))
                {
                    if (next + EOL.Length == length)
                    {
                        index = length;
                        break;
                    }

                    string line = response.Substring(index, next);

                    index = next + EOL.Length;
                }
            }

            if (backlog.Length > 0 && index == length)
                backlog = new StringBuilder();

            if (index < length)
                backlog = new StringBuilder(response.Substring(index));
        }

        /// <summary>
        /// Starts the process of monitoring for events by launching a control connection.
        /// </summary>
        private void Start()
        {
            if (disposed)
                throw new ObjectDisposedException("this");

            lock (synchronize)
            {
                if (socket != null || !enabled)
                    return;

                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, 1);
                socket.BeginConnect("127.0.0.1", client.Configuration.ControlPort, OnSocketConnect, null);
            }
        }

        /// <summary>
        /// Shuts down the listener socket and releases all resources associated with it.
        /// </summary>
        private void Shutdown()
        {
            lock (synchronize)
            {
                if (socket != null)
                {
                    try
                    {
                        socket.Shutdown(SocketShutdown.Both);
                    }
                    catch { }

                    socket.Dispose();
                    socket = null;
                }
            }
        }
    }
}
