using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

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
