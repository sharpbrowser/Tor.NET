using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using Tor.Net;
using System.Diagnostics;

namespace Tor.IO
{
    /// <summary>
    /// A class which connects to a remote address using the tor network service, and provides stream methods for communication.
    /// </summary>
    [DebuggerStepThrough]
    public sealed class Socks5Stream : System.IO.Stream
    {
        private readonly Client client;
        private readonly string host;
        private readonly int port;
        private readonly object synchronize;

        private volatile bool disposed;
        private ForwardSocket socket;
        private NetworkStream stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="Socks5Stream"/> class.
        /// </summary>
        /// <param name="client">The client hosting the SOCKS5 connection.</param>
        /// <param name="host">The remote host address.</param>
        /// <param name="port">The remote port number.</param>
        internal Socks5Stream(Client client, string host, int port)
        {
            this.client = client;
            this.disposed = false;
            this.host = host;
            this.port = port;
            this.socket = null;
            this.synchronize = new object();

            this.Start();
        }

        #region Properties

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports reading.
        /// </summary>
        public override bool CanRead
        {
            get { return true; }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports seeking.
        /// </summary>
        public override bool CanSeek
        {
            get { return false; }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports writing.
        /// </summary>
        public override bool CanWrite
        {
            get { return true; }
        }

        /// <summary>
        /// When overridden in a derived class, gets the length in bytes of the stream.
        /// </summary>
        public override long Length
        {
            get { throw new NotSupportedException(); }
        }

        /// <summary>
        /// When overridden in a derived class, gets or sets the position within the current stream.
        /// </summary>
        public override long Position
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        #endregion

        #region System.IO.Stream

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="T:System.IO.Stream" /> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposed)
                return;

            if (disposing)
            {
                Shutdown();
                disposed = true;
            }
        }

        /// <summary>
        /// When overridden in a derived class, clears all buffers for this stream and causes any buffered data to be written to the underlying device.
        /// </summary>
        public override void Flush()
        {
            if (disposed)
                throw new ObjectDisposedException("this");
            if (stream == null)
                throw new Exception("The stream is not connected");

            stream.Flush();
        }

        /// <summary>
        /// When overridden in a derived class, reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.
        /// </summary>
        /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between <paramref name="offset" /> and (<paramref name="offset" /> + <paramref name="count" /> - 1) replaced by the bytes read from the current source.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> at which to begin storing the data read from the current stream.</param>
        /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
        /// <returns>
        /// The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.
        /// </returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (disposed)
                throw new ObjectDisposedException("this");
            if (stream == null)
                throw new Exception("The stream is not connected");

            return stream.Read(buffer, offset, count);
        }

        /// <summary>
        /// When overridden in a derived class, sets the position within the current stream.
        /// </summary>
        /// <param name="offset">A byte offset relative to the <paramref name="origin" /> parameter.</param>
        /// <param name="origin">A value of type <see cref="T:System.IO.SeekOrigin" /> indicating the reference point used to obtain the new position.</param>
        /// <returns>
        /// The new position within the current stream.
        /// </returns>
        public override long Seek(long offset, System.IO.SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// When overridden in a derived class, sets the length of the current stream.
        /// </summary>
        /// <param name="value">The desired length of the current stream in bytes.</param>
        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// When overridden in a derived class, writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
        /// </summary>
        /// <param name="buffer">An array of bytes. This method copies <paramref name="count" /> bytes from <paramref name="buffer" /> to the current stream.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> at which to begin copying bytes to the current stream.</param>
        /// <param name="count">The number of bytes to be written to the current stream.</param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            if (disposed)
                throw new ObjectDisposedException("this");
            if (stream == null)
                throw new Exception("The stream is not connected");

            stream.Write(buffer, offset, count);
        }

        #endregion

        /// <summary>
        /// Reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.
        /// </summary>
        /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the bytes read from the current source.</param>
        /// <returns>
        /// The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.
        /// </returns>
        public int Read(byte[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");
            if (buffer.Length == 0)
                throw new ArgumentOutOfRangeException("buffer", "The read buffer should be greater than zero in length");

            return Read(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Starts the SOCKS5 stream by connecting to the remote host using the forwarding socket.
        /// </summary>
        private void Start()
        {
            if (host == null)
                throw new InvalidOperationException("The specified host is null");

            if (disposed)
                throw new ObjectDisposedException("this");

            lock (synchronize)
            {
                try
                {
                    if (socket != null)
                        return;

                    string address = host;

                    if (address.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase))
                        address = address.Substring(7);
                    if (address.StartsWith("https://", StringComparison.CurrentCultureIgnoreCase))
                        address = address.Substring(8);

                    socket = new ForwardSocket(client);
                    socket.ProxyAddress = "127.0.0.1";
                    socket.ProxyPort = client.Configuration.SocksPort;
                    socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, 1);
                    socket.Connect(address, port);

                    stream = new NetworkStream(socket, false);
                }
                catch (Exception exception)
                {
                    throw new TorException("An attempt to connect to a SOCKS5 host failed", exception);
                }
                finally
                {
                    if (stream != null)
                    {
                        stream.Dispose();
                        stream = null;
                    }

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

        /// <summary>
        /// Shuts down the SOCKS5 connection and disables streaming.
        /// </summary>
        private void Shutdown()
        {
            lock (synchronize)
            {
                if (stream != null)
                {
                    stream.Dispose();
                    stream = null;
                }

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

        /// <summary>
        /// Writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
        /// </summary>
        /// <param name="buffer">An array of bytes. This method copies <paramref name="count" /> bytes from <paramref name="buffer" /> to the current stream.</param>
        public void Write(byte[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");

            Write(buffer, 0, buffer.Length);
        }
    }
}
