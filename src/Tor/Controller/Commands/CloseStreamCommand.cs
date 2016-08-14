using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tor.Helpers;

namespace Tor.Controller
{
    /// <summary>
    /// A class containing the command to close an existing stream.
    /// </summary>
    internal sealed class CloseStreamCommand : Command<Response>
    {
        private readonly StreamReason reason;
        private readonly Stream stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="CloseStreamCommand"/> class.
        /// </summary>
        /// <param name="stream">The stream which should be closed.</param>
        /// <param name="reason">The reason for the stream being closed.</param>
        public CloseStreamCommand(Stream stream, StreamReason reason)
        {
            this.reason = reason;
            this.stream = stream;
        }

        #region Tor.Controller.Command<>

        /// <summary>
        /// Dispatches the command to the client control port and produces a <typeparamref name="T" /> response result.
        /// </summary>
        /// <param name="connection">The control connection where the command should be dispatched.</param>
        /// <returns>
        /// A <typeparamref name="T" /> object instance containing the response data.
        /// </returns>
        protected override Response Dispatch(Connection connection)
        {
            if (stream == null || stream.ID <= 0)
                return new Response(false);
            if (stream.Status == StreamStatus.Failed || stream.Status == StreamStatus.Closed)
                return new Response(false);
            if (reason == StreamReason.None || reason == StreamReason.PrivateAddr || reason == StreamReason.End)
                return new Response(false);
            
            if (connection.Write("closestream {0} {1}", stream.ID, (int)reason))
            {
                ConnectionResponse response = connection.Read();
                return new Response(response.Success);
            }

            return new Response(false);
        }

        #endregion
    }
}
