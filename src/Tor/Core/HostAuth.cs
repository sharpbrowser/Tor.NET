using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Tor.Converters;

namespace Tor
{
    /// <summary>
    /// A structure containing credential information for a host connection.
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(HostAuthTypeConverter))]
    public struct HostAuth
    {
        private string password;
        private string username;

        /// <summary>
        /// Initializes a new instance of the <see cref="HostAuth"/> struct.
        /// </summary>
        /// <param name="username">The username of the credentials.</param>
        /// <param name="password">The password of the credentials. A value of <c>null</c> indicates no password.</param>
        public HostAuth(string username, string password)
        {
            this.password = password;
            this.username = username;
        }

        #region Properties

        /// <summary>
        /// Gets a value indicating whether the host authentication is actually a null reference.
        /// </summary>
        public bool IsNull
        {
            get { return username == null && password == null; }
        }

        /// <summary>
        /// Gets or sets the password used when authenticating. A value of <c>null</c> indicates no password.
        /// </summary>
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        /// <summary>
        /// Gets or sets the username used when authenticating.
        /// </summary>
        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        /// <summary>
        /// Gets a <see cref="HostAuth"/> object instance representing a <c>null</c> value.
        /// </summary>
        public static HostAuth Null
        {
            get { return new HostAuth() { username = null, password = null }; }
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
            return obj != null && obj is HostAuth && ((HostAuth)obj).password == password && ((HostAuth)obj).username == username;
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
                hash = hash * 23 + (username != null ? username.GetHashCode() : 0);
                hash = hash * 23 + (password != null ? password.GetHashCode() : 0);
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
            if (username == null && password == null)
                return "";
            if (password == null)
                return username;

            return string.Format("{0}:{1}", username, password);
        }

        #endregion
    }
}
