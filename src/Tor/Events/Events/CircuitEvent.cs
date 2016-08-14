using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tor
{
    /// <summary>
    /// A class containing information regarding a circuit status changing.
    /// </summary>
    public sealed class CircuitEventArgs : EventArgs
    {
        private readonly Circuit circuit;

        /// <summary>
        /// Initializes a new instance of the <see cref="CircuitEventArgs"/> class.
        /// </summary>
        /// <param name="circuit">The circuit which was changed.</param>
        public CircuitEventArgs(Circuit circuit)
        {
            this.circuit = circuit;
        }

        #region Properties

        /// <summary>
        /// Gets the circuit which was changed.
        /// </summary>
        public Circuit Circuit
        {
            get { return circuit; }
        }

        #endregion
    }

    /// <summary>
    /// Represents the method that will handle a circuit changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="CircuitEventArgs"/> instance containing the event data.</param>
    public delegate void CircuitEventHandler(object sender, CircuitEventArgs e);
}
