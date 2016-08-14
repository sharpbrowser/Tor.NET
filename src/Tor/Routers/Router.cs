using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Net;

namespace Tor
{
    /// <summary>
    /// A class containing information regarding a router within a circuit.
    /// </summary>
    [Serializable]
    public sealed class Router
    {
        private Bytes bandwidth;
        private string digest;
        private int dirPort;
        private RouterFlags flags;
        private string identity;
        private IPAddress ipAddress;
        private string nickname;
        private int orPort;
        private DateTime publication;

        /// <summary>
        /// Initializes a new instance of the <see cref="Router"/> class.
        /// </summary>
        /// <param name="circuit">The circuit containing the router.</param>
        internal Router()
        {
            this.bandwidth = Bytes.Empty;
            this.digest = null;
            this.dirPort = -1;
            this.flags = RouterFlags.None;
            this.identity = null;
            this.ipAddress = null;
            this.nickname = null;
            this.orPort = -1;
            this.publication = DateTime.MinValue;
        }

        #region Properties

        /// <summary>
        /// Gets the bandwidth of the router.
        /// </summary>
        public Bytes Bandwidth
        {
            get { return bandwidth; }
            internal set { bandwidth = value; }
        }

        /// <summary>
        /// Gets the digest of the router. This is the hash of the most recent descriptor, encoded in base64.
        /// </summary>
        public string Digest
        {
            get { return digest; }
            internal set { digest = value; }
        }

        /// <summary>
        /// Gets the current directory port of the router.
        /// </summary>
        public int DIRPort
        {
            get { return dirPort; }
            internal set { dirPort = value; }
        }

        /// <summary>
        /// Gets the flags assigned against the router.
        /// </summary>
        public RouterFlags Flags
        {
            get { return flags; }
            internal set { flags = value; }
        }

        /// <summary>
        /// Gets the encoded hash identity of the router, encoded in base 64 with trailing equal symbols removed.
        /// </summary>
        public string Identity
        {
            get { return identity; }
            internal set { identity = value; }
        }

        /// <summary>
        /// Gets the IP address of the router.
        /// </summary>
        public IPAddress IPAddress
        {
            get { return ipAddress; }
            internal set { ipAddress = value; }
        }

        /// <summary>
        /// Gets the nickname of the router.
        /// </summary>
        public string Nickname
        {
            get { return nickname; }
            internal set { nickname = value; }
        }

        /// <summary>
        /// Gets the current OR port of the router.
        /// </summary>
        public int ORPort
        {
            get { return orPort; }
            internal set { orPort = value; }
        }

        /// <summary>
        /// Gets the publication time of the most recent descriptor.
        /// </summary>
        public DateTime Publication
        {
            get { return publication; }
            internal set { publication = value; }
        }

        #endregion
    }

    /// <summary>
    /// A class containing a read-only collection of <see cref="Router"/> objects.
    /// </summary>
    public sealed class RouterCollection : ReadOnlyCollection<Router>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RouterCollection"/> class.
        /// </summary>
        /// <param name="list">The list of routers.</param>
        internal RouterCollection(IList<Router> list) : base(list)
        {
        }
    }
}
