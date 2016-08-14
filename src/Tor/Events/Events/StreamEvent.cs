using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tor
{
    /// <summary>
    /// A class containing information regarding a stream status changing.
    /// </summary>
    public sealed class StreamEventArgs : EventArgs
    {
        private readonly Stream stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamEventArgs"/> class.
        /// </summary>
        /// <param name="stream">The stream which was changed.</param>
        public StreamEventArgs(Stream stream)
        {
            this.stream = stream;
        }

        #region Properties

        /// <summary>
        /// Gets the stream which was changed.
        /// </summary>
        public Stream Stream
        {
            get { return stream; }
        }

        #endregion
    }

    /// <summary>
    /// Represents the method that will handle a stream changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="StreamEventArgs"/> instance containing the event data.</param>
    public delegate void StreamEventHandler(object sender, StreamEventArgs e);
}
