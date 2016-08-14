using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tor
{
    /// <summary>
    /// A class containing information regarding an OR connection event.
    /// </summary>
    [Serializable]
    public sealed class ORConnectionEventArgs : EventArgs
    {
        private readonly ORConnection connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="ORConnectionEventArgs"/> class.
        /// </summary>
        /// <param name="connection">The connection which was changed.</param>
        public ORConnectionEventArgs(ORConnection connection)
        {
            this.connection = connection;
        }

        #region Properties

        /// <summary>
        /// Gets the connection which was changed.
        /// </summary>
        public ORConnection Connection
        {
            get { return connection; }
        }

        #endregion
    }

    /// <summary>
    /// Represents the method that will handle an OR connection changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="ORConnectionEventArgs"/> instance containing the event data.</param>
    public delegate void ORConnectionEventHandler(object sender, ORConnectionEventArgs e);
}
