using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tor
{
    /// <summary>
    /// A class containing information regarding bandwidth values changing.
    /// </summary>
    [Serializable]
    public sealed class BandwidthEventArgs : EventArgs
    {
        private readonly Bytes downloaded;
        private readonly Bytes uploaded;

        /// <summary>
        /// Initializes a new instance of the <see cref="BandwidthEventArgs"/> class.
        /// </summary>
        /// <param name="downloaded">The bytes downloaded.</param>
        /// <param name="uploaded">The bytes uploaded.</param>
        public BandwidthEventArgs(Bytes downloaded, Bytes uploaded)
        {
            this.downloaded = downloaded;
            this.uploaded = uploaded;
        }

        #region Properties

        /// <summary>
        /// Gets the bytes downloaded.
        /// </summary>
        public Bytes Downloaded
        {
            get { return downloaded; }
        }

        /// <summary>
        /// Gets the bytes uploaded.
        /// </summary>
        public Bytes Uploaded
        {
            get { return uploaded; }
        }

        #endregion
    }

    /// <summary>
    /// Represents the method that will handle a bandwidth changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="BandwidthEventArgs"/> instance containing the event data.</param>
    public delegate void BandwidthEventHandler(object sender, BandwidthEventArgs e);
}
