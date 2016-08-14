using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Tor.Converters;

namespace Tor
{
    /// <summary>
    /// A structure containing information on a host, such as an address and/or port number.
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(HostTypeConverter))]
    public struct Host
    {
        private string address;
        private int port;

        /// <summary>
        /// Initializes a new instance of the <see cref="Host"/> struct.
        /// </summary>
        /// <param name="address">The address of the host.</param>
        public Host(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("A host address cannot be null or white-space", "address");

            this.address = address;
            this.port = -1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Host"/> struct.
        /// </summary>
        /// <param name="address">The address of the host.</param>
        /// <param name="port">The port number open on the host. A value of <c>-1</c> indicates an unspecified port number.</param>
        public Host(string address, int port)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("A host address cannot be null or white-space", "address");
            if (port != -1 && (port <= 0 || short.MaxValue < port))
                throw new ArgumentOutOfRangeException("port", "A port number must fall within an acceptable range");

            this.address = address;
            this.port = port;
        }

        #region Properties

        /// <summary>
        /// Gets or sets the address of the host.
        /// </summary>
        public string Address
        {
            get { return address; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("A host address cannot be null or white-space");

                address = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the host is actually a null reference.
        /// </summary>
        public bool IsNull
        {
            get { return address == null; }
        }

        /// <summary>
        /// Gets or sets the port number open on the host. A value of <c>-1</c> indicates an unspecified port number.
        /// </summary>
        public int Port
        {
            get { return port; }
            set
            {
                if (port != -1 && (port <= 0 || short.MaxValue < port))
                    throw new ArgumentOutOfRangeException("value", "A port number must fall within an acceptable range");

                port = value;
            }
        }

        /// <summary>
        /// Gets a <see cref="Host"/> object instance representing a <c>null</c> value.
        /// </summary>
        public static Host Null
        {
            get { return new Host() { address = null, port = -1 }; }
        }

        #endregion
        
        #region System.Object

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj != null && obj is Host && ((Host)obj).address == address && ((Host)obj).port == port;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + address.GetHashCode();
                hash = hash * 23 + port;
                return hash;
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            if (port == -1)
                return address;
            else
                return string.Format("{0}:{1}", address, port);
        }

        #endregion
    }
}
