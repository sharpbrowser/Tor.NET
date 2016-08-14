using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace Tor.Net
{
    /// <summary>
    /// A class responsible for mediating the authentication and dispatch process of a SOCKS5 protocol connection.
    /// </summary>
    internal sealed class Socks5Processor
    {
        private readonly Socks5AsyncResult asyncResult;
        private readonly ForwardSocket socket;

        private byte[] buffer;
        private ProcessorCallback callback;
        private string endAddress;
        private IPEndPoint endPoint;
        private int endPort;
        private int finalLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="Socks5Processor"/> class.
        /// </summary>
        /// <param name="socket">The socket which is connected to the proxy network.</param>
        public Socks5Processor(ForwardSocket socket)
        {
            this.asyncResult = new Socks5AsyncResult();
            this.buffer = new byte[512];
            this.endAddress = null;
            this.endPoint = null;
            this.endPort = 0;
            this.finalLength = 0;
            this.socket = socket;
        }

        #region System.Net.Sockets.Socket

        /// <summary>
        /// Called when the forwarded socket has connected to the destination IP address and port.
        /// </summary>
        /// <param name="ar">The asynchronous result object for the asynchronous method.</param>
        private void OnSocketConnect(IAsyncResult ar)
        {
            try
            {
                buffer[0] = 5;
                buffer[1] = 1;
                buffer[2] = 0;

                socket.EndConnect(ar);
                socket.BeginSend(buffer, 0, 3, SocketFlags.None, OnSocketSendHandshake, socket);
            }
            catch
            {
                if (callback != null)
                    callback(false);
            }
        }

        /// <summary>
        /// Called when the socket receives the response to a connect request.
        /// </summary>
        /// <param name="ar">The asynchronous result object for the asynchronous method.</param>
        private void OnSocketReceiveConnect(IAsyncResult ar)
        {
            try
            {
                int received = socket.EndReceive(ar);

                if (received != 5 || buffer[0] != 5 || buffer[1] != 0)
                    throw new Exception();

                int length = 0;

                switch (buffer[3])
                {
                    case 1:
                        length = 5;
                        break;
                    case 3:
                        length = buffer[4] + 2;
                        break;
                    case 4:
                        length = 17;
                        break;
                    default:
                        throw new Exception();
                }

                finalLength = length;

                socket.BeginReceive(buffer, 1, length, SocketFlags.None, OnSocketReceiveConnectFinal, socket);
            }
            catch
            {
                if (callback != null)
                    callback(false);
            }
        }

        /// <summary>
        /// Called when the socket receives the final response to a connect request.
        /// </summary>
        /// <param name="ar">The asynchronous result object for the asynchronous method.</param>
        private void OnSocketReceiveConnectFinal(IAsyncResult ar)
        {
            try
            {
                int received = socket.EndReceive(ar);

                if (finalLength < received)
                    throw new Exception();

                if (finalLength > received)
                {
                    finalLength -= received;
                    socket.BeginReceive(buffer, 0, finalLength, SocketFlags.None, OnSocketReceiveConnectFinal, socket);
                    return;
                }

                if (callback != null)
                    callback(true);
            }
            catch
            {
                if (callback != null)
                    callback(false);
            }
        }

        /// <summary>
        /// Called when the socket receives the response to a handshake retrieve request.
        /// </summary>
        /// <param name="ar">The asynchronous result object for the asynchronous method.</param>
        private void OnSocketReceiveHandshake(IAsyncResult ar)
        {
            try
            {
                int received = socket.EndReceive(ar);

                if (received == 0 || buffer[0] != 5 || buffer[1] != 0)
                    throw new Exception();

                int length = 0;

                if (endPoint != null)
                {
                    buffer[0] = 5;
                    buffer[1] = 1;
                    buffer[2] = 0;
                    buffer[3] = 1;

                    byte[] address = endPoint.Address.GetAddressBytes();
                    byte[] port = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)endPoint.Port));

                    Array.Copy(address, 0, buffer, 4, 4);
                    Array.Copy(port, 0, buffer, 8, 2);

                    length = 10;
                }
                else
                {
                    buffer[0] = 5;
                    buffer[1] = 1;
                    buffer[2] = 0;
                    buffer[3] = 3;
                    buffer[4] = (byte)endAddress.Length;

                    byte[] address = Encoding.ASCII.GetBytes(endAddress);
                    byte[] port = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)endPort));

                    Array.Copy(address, 0, buffer, 5, address.Length);
                    Array.Copy(port, 0, buffer, address.Length + 5, 2);

                    length = address.Length + 7;
                }

                socket.BeginSend(buffer, 0, length, SocketFlags.None, OnSocketSendConnect, socket);
            }
            catch
            {
                if (callback != null)
                    callback(false);
            }
        }

        /// <summary>
        /// Called when the socket completes sending a request to connect to an IP end point.
        /// </summary>
        /// <param name="ar">The asynchronous result object for the asynchronous method.</param>
        private void OnSocketSendConnect(IAsyncResult ar)
        {
            try
            {
                socket.EndSend(ar);
                socket.BeginReceive(buffer, 0, 5, SocketFlags.None, OnSocketReceiveConnect, socket);
            }
            catch
            {
                if (callback != null)
                    callback(false);
            }
        }

        /// <summary>
        /// Called when the socket has completed sending the handshake request.
        /// </summary>
        /// <param name="ar">The asynchronous result object for the asynchronous method.</param>
        private void OnSocketSendHandshake(IAsyncResult ar)
        {
            try
            {
                socket.EndSend(ar);
                socket.BeginReceive(buffer, 0, 2, SocketFlags.None, OnSocketReceiveHandshake, socket);
            }
            catch
            {
                if (callback != null)
                    callback(false);
            }
        }

        #endregion

        /// <summary>
        /// Starts processing the connection by establishing relevant protocol commands and resolving the address.
        /// </summary>
        /// <param name="remoteEP">The remote end-point targetted for connection.</param>
        /// <param name="callback">The method raised once the connection process has completed or failed.</param>
        public Socks5AsyncResult BeginProcessing(IPEndPoint remoteEP, ProcessorCallback callback)
        {
            if (remoteEP == null)
                throw new ArgumentNullException("remoteEP");

            this.callback = callback;
            this.endPoint = remoteEP;

            socket.BeginConnectInternal(new IPEndPoint(IPAddress.Parse(socket.ProxyAddress), socket.ProxyPort), OnSocketConnect, socket);

            return asyncResult;
        }

        /// <summary>
        /// Starts processing the connection by establishing relevant protocol commands and resolving the address.
        /// </summary>
        /// <param name="host">The address of the remote host to connect to.</param>
        /// <param name="port">The port number of the remote host to connect to.</param>
        /// <param name="callback">The method raised once the connection process has completed or failed.</param>
        public Socks5AsyncResult BeginProcessing(string host, int port, ProcessorCallback callback)
        {
            if (host == null)
                throw new ArgumentNullException("host");

            this.callback = callback;
            this.endAddress = host;
            this.endPort = port;

            socket.BeginConnectInternal(new IPEndPoint(IPAddress.Parse(socket.ProxyAddress), socket.ProxyPort), OnSocketConnect, socket);

            return asyncResult;
        }
    }
    
    /// <summary>
    /// A class which contains information regarding the state of a forward processor.
    /// </summary>
    internal sealed class Socks5AsyncResult : IAsyncResult
    {
        private readonly ManualResetEvent asyncWaitHandle;

        private object asyncState;
        private bool completed;

        /// <summary>
        /// Initializes a new instance of the <see cref="Socks5AsyncResult"/> class.
        /// </summary>
        public Socks5AsyncResult()
        {
            this.asyncWaitHandle = new ManualResetEvent(false);
            this.asyncState = null;
            this.completed = false;
        }

        #region Properties

        /// <summary>
        /// Gets a user-defined object that qualifies or contains information about an asynchronous operation.
        /// </summary>
        public object AsyncState
        {
            get { return asyncState; }
        }

        /// <summary>
        /// Gets a <see cref="T:System.Threading.WaitHandle" /> that is used to wait for an asynchronous operation to complete.
        /// </summary>
        public WaitHandle AsyncWaitHandle
        {
            get { return asyncWaitHandle; }
        }

        /// <summary>
        /// Gets a value that indicates whether the asynchronous operation completed synchronously.
        /// </summary>
        public bool CompletedSynchronously
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value that indicates whether the asynchronous operation has completed.
        /// </summary>
        public bool IsCompleted
        {
            get { return completed; }
        }

        #endregion

        /// <summary>
        /// Resets the asynchronous result.
        /// </summary>
        public void Reset()
        {
            completed = false;
            asyncWaitHandle.Reset();
        }

        /// <summary>
        /// Sets the asynchronous result to completed.
        /// </summary>
        public void Set()
        {
            asyncState = null;
            completed = true;

            asyncWaitHandle.Set();
        }
    }
    
    /// <summary>
    /// A delegate event handler which encapsulates the callback method raised once a processor has completed.
    /// </summary>
    /// <param name="success"><c>true</c> if the connection succeeds; otherwise, <c>false</c>.</param>
    internal delegate void ProcessorCallback(bool success);
}
