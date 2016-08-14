using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;

namespace Tor.Proxy
{
    /// <summary>
    /// A class containing a reference to a connection proxy client.
    /// </summary>
    internal sealed class Connection : IDisposable
    {
        private readonly Client client;

        private volatile bool disposed;
        private ConnectionDisposedCallback disposedCallback;
        private Dictionary<string, string> headers;
        private string host;
        private string http;
        private string method;
        private int port;
        private byte[] post;
        private Socket socket;

        /// <summary>
        /// Initializes a new instance of the <see cref="Connection"/> class.
        /// </summary>
        /// <param name="client">The client hosting the proxy .</param>
        /// <param name="socket">The socket belonging to the connection.</param>
        /// <param name="disposeCallback">A callback method raised when the connection is disposed.</param>
        public Connection(Client client, Socket socket, ConnectionDisposedCallback disposeCallback)
        {
            this.client = client;
            this.disposed = false;
            this.disposedCallback = disposeCallback;
            this.headers = null;
            this.host = null;
            this.http = "HTTP/1.1";
            this.method = null;
            this.port = 80;
            this.post = null;
            this.socket = socket;

            this.GetHeaderData();
        }

        #region Properties

        /// <summary>
        /// Gets the header values provided with the request.
        /// </summary>
        public Dictionary<string, string> Headers
        {
            get { return headers; }
        }

        /// <summary>
        /// Gets the target host of the HTTP request.
        /// </summary>
        public string Host
        {
            get { return host; }
        }

        /// <summary>
        /// Gets the HTTP version sent with the request.
        /// </summary>
        public string HTTP
        {
            get { return http; }
        }

        /// <summary>
        /// Gets the method requested for the HTTP request (GET, POST, PUT, DELETE, CONNECT).
        /// </summary>
        public string Method
        {
            get { return method; }
        }

        /// <summary>
        /// Gets the target port number of the HTTP request.
        /// </summary>
        public int Port
        {
            get { return port; }
        }

        /// <summary>
        /// Gets the POST data.
        /// </summary>
        public byte[] Post
        {
            get { return post; }
        }

        /// <summary>
        /// Gets the socket connected to the proxy client.
        /// </summary>
        public Socket Socket
        {
            get { return socket; }
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

            if (disposing)
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

                disposed = true;

                if (disposedCallback != null)
                    disposedCallback(this);
            }
        }

        #endregion
        
        /// <summary>
        /// Gets the header block which was dispatched with the original socket request.
        /// </summary>
        /// <returns>A <see cref="System.String"/> containing the header data.</returns>
        public string GetHeader()
        {
            StringBuilder header = new StringBuilder();

            header.Append(method);
            header.Append("\r\n");

            foreach (KeyValuePair<string, string> value in headers)
            {
                if (value.Key.StartsWith("proxy", StringComparison.CurrentCultureIgnoreCase))
                    continue;

                header.AppendFormat("{0}: {1}\r\n", value.Key, value.Value);
            }

            return header.Append("\r\n").ToString();
        }
        
        /// <summary>
        /// Process the connection request by reading the header information from the HTTP request, and connecting to the tor server
        /// and dispatching the request to the relevant host.
        /// </summary>
        private void GetHeaderData()
        {
            try
            {
                StringBuilder builder = new StringBuilder();

                using (StreamReader reader = new StreamReader(new NetworkStream(socket, false)))
                {
                    for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
                    {
                        builder.Append(line);
                        builder.Append("\r\n");

                        if (line.Trim().Length == 0)
                            break;
                    }
                }

                using (StringReader reader = new StringReader(builder.ToString()))
                {
                    method = reader.ReadLine();

                    if (method == null)
                        throw new InvalidOperationException("The proxy connection did not supply a valid HTTP header");

                    headers = new Dictionary<string, string>();

                    for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
                    {
                        string trimmed = line.Trim();

                        if (trimmed.Length == 0)
                            break;

                        string[] parts = trimmed.Split(new[] { ':' }, 2);

                        if (parts.Length == 1)
                            continue;

                        headers[parts[0].Trim()] = parts[1].Trim();
                    }

                    if (method.StartsWith("POST", StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (!headers.ContainsKey("Content-Length"))
                            throw new InvalidOperationException("The proxy connection is a POST method but contains no content length");

                        long contentLength = long.Parse(headers["Content-Length"]);

                        using (MemoryStream memory = new MemoryStream())
                        {
                            long read = 0;
                            byte[] buffer = new byte[512];

                            while (contentLength > read)
                            {
                                int received = socket.Receive(buffer, 0, buffer.Length, SocketFlags.None);

                                if (received <= 0)
                                    throw new InvalidOperationException("The proxy connection was terminated while reading POST data");

                                memory.Write(buffer, 0, received);
                                read += received;
                            }

                            post = memory.ToArray();
                        }
                    }

                    if (method.StartsWith("CONNECT", StringComparison.CurrentCultureIgnoreCase))
                    {
                        string[] connectTargets = method.Split(' ');

                        if (connectTargets.Length < 3)
                            throw new InvalidOperationException("The proxy connection supplied a CONNECT command with insufficient parameters");

                        http = connectTargets[2];

                        string connectTarget = connectTargets[1];
                        string[] connectParams = connectTarget.Split(':');

                        if (connectParams.Length == 2)
                        {
                            host = connectParams[0];
                            port = int.Parse(connectParams[1]);
                        }
                        else
                        {
                            host = connectParams[0];
                            port = 443;
                        }
                    }
                    else
                    {
                        if (!headers.ContainsKey("Host"))
                            throw new InvalidOperationException("The proxy connection did not supply a connection host");

                        string connectTarget = headers["Host"];
                        string[] connectParams = connectTarget.Split(':');

                        if (connectParams.Length == 1)
                            host = connectParams[0];
                        else
                        {
                            host = connectParams[0];
                            port = int.Parse(connectParams[1]);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                throw new TorException("The proxy connection failed to process", exception);
            }
        }

        /// <summary>
        /// Writes a buffer of data to the connected client socket.
        /// </summary>
        /// <param name="data">The data to send to the client socket.</param>
        public void Write(string data)
        {
            Write(Encoding.ASCII.GetBytes(data));
        }

        /// <summary>
        /// Writes a buffer of data to the connected client socket.
        /// </summary>
        /// <param name="data">The data to send to the client socket.</param>
        /// <param name="parameters">An optional list of parameters to format into the data.</param>
        public void Write(string data, params object[] parameters)
        {
            data = string.Format(data, parameters);
            Write(Encoding.ASCII.GetBytes(data));
        }

        /// <summary>
        /// Writes a buffer of data to the connected client socket.
        /// </summary>
        /// <param name="buffer">The data to send to the client socket.</param>
        public void Write(byte[] buffer)
        {
            socket.Send(buffer, 0, buffer.Length, SocketFlags.None);
        }
    }

    /// <summary>
    /// A delegate event handler representing a method raised when a connection is disposed.
    /// </summary>
    /// <param name="connection">The connection which was disposed.</param>
    internal delegate void ConnectionDisposedCallback(Connection connection);
}
