using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Tor.Helpers;
using System.ComponentModel;

namespace Tor
{
    /// <summary>
    /// A class containing information about an active or inactive stream within the tor network service.
    /// </summary>
    public sealed class Stream : MarshalByRefObject
    {
        private readonly Client client;
        private readonly int id;
        private readonly Host target;

        private int circuitID;
        private StreamPurpose purpose;
        private StreamReason reason;
        private StreamStatus status;

        /// <summary>
        /// Initializes a new instance of the <see cref="Stream"/> class.
        /// </summary>
        /// <param name="client">The client for which the stream belongs.</param>
        /// <param name="id">The unique identifier of the stream within the tor session.</param>
        /// <param name="target">The target of the stream.</param>
        internal Stream(Client client, int id, Host target)
        {
            this.circuitID = 0;
            this.client = client;
            this.id = id;
            this.purpose = StreamPurpose.None;
            this.reason = StreamReason.None;
            this.status = StreamStatus.None;
            this.target = target;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Stream"/> class.
        /// </summary>
        /// <param name="client">The client for which the stream belongs.</param>
        /// <param name="line">The line which was received from the control connection.</param>
        /// <returns>
        ///   <c>Stream</c> the new instance.
        /// </returns>
        public static Stream FromLine(Client client, string line)
        {
            int streamID;
            int circuitID;
            int port;
            StreamStatus status;
            Host target;
            string[] parts = StringHelper.GetAll(line, ' ');

            if (parts.Length < 4)
                return null;

            if ("Tor_internal".Equals(parts[3], StringComparison.CurrentCultureIgnoreCase))
                return null;

            if (!int.TryParse(parts[0], out streamID))
                return null;

            if (!int.TryParse(parts[2], out circuitID))
                return null;

            string[] targetParts = parts[3].Split(new[] { ':' }, 2);

            if (targetParts.Length < 2)
                return null;

            if (!int.TryParse(targetParts[1], out port))
                return null;

            status = ReflectionHelper.GetEnumerator<StreamStatus, DescriptionAttribute>(attr => parts[1].Equals(attr.Description, StringComparison.CurrentCultureIgnoreCase));
            target = new Host(targetParts[0], port);

            Stream stream = new Stream(client, streamID, target);
            stream.CircuitID = circuitID;
            stream.Status = status;

            for (int i = 4; i < parts.Length; i++)
            {
                string data = parts[i].Trim();

                if (!data.Contains("="))
                    continue;

                string[] values = data.Split(new[] { '=' }, 2);

                if (values.Length < 2)
                    continue;

                string name = values[0].Trim();
                string value = values[1].Trim();

                if ("REASON".Equals(name, StringComparison.CurrentCultureIgnoreCase))
                {
                    stream.Reason = ReflectionHelper.GetEnumerator<StreamReason, DescriptionAttribute>(attr => value.Equals(attr.Description, StringComparison.CurrentCultureIgnoreCase));
                    continue;
                }

                if ("PURPOSE".Equals(name, StringComparison.CurrentCultureIgnoreCase))
                {
                    stream.Purpose = ReflectionHelper.GetEnumerator<StreamPurpose, DescriptionAttribute>(attr => value.Equals(attr.Description, StringComparison.CurrentCultureIgnoreCase));
                    continue;
                }
            }

            return stream;
        }

        #region Properties

        /// <summary>
        /// Gets the unique identifier of the circuit which owns this stream. This will be zero if the stream has been detached.
        /// </summary>
        public int CircuitID
        {
            get { return circuitID; }
            internal set { circuitID = value; }
        }

        /// <summary>
        /// Gets the unique identifier of the stream in the tor session.
        /// </summary>
        public int ID
        {
            get { return id; }
        }

        /// <summary>
        /// Gets the purpose for the stream. This will be <c>StreamPurpose.None</c> unless extended events are enabled.
        /// </summary>
        public StreamPurpose Purpose
        {
            get { return purpose; }
            internal set { purpose = value; }
        }

        /// <summary>
        /// Gets the reason for the stream being closed, failed or detached. This will be <c>StreamReason.None</c> until the stream
        /// has reached either of the aforementioned states.
        /// </summary>
        public StreamReason Reason
        {
            get { return reason; }
            internal set { reason = value; }
        }

        /// <summary>
        /// Gets the status of the stream.
        /// </summary>
        public StreamStatus Status
        {
            get { return status; }
            internal set { status = value; }
        }

        /// <summary>
        /// Gets the target of the stream.
        /// </summary>
        public Host Target
        {
            get { return target; }
        }

        #endregion

        /// <summary>
        /// Closes the stream.
        /// </summary>
        /// <returns><c>true</c> if the stream is closed successfully; otherwise, <c>false</c>.</returns>
        public bool Close()
        {
            return client.Controller.CloseStream(this, StreamReason.Misc);
        }
    }

    /// <summary>
    /// A class containing a read-only collection of <see cref="Stream"/> objects.
    /// </summary>
    public sealed class StreamCollection : ReadOnlyCollection<Stream>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamCollection"/> class.
        /// </summary>
        /// <param name="list">The list of streams.</param>
        internal StreamCollection(IList<Stream> list) : base(list)
        {
        }
    }
}
