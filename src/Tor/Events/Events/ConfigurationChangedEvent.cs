using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tor.Events
{
    /// <summary>
    /// A class containing information regarding configuration values changing.
    /// </summary>
    public sealed class ConfigurationChangedEventArgs : EventArgs
    {
        private readonly Dictionary<string, string> configurations;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationChangedEventArgs"/> class.
        /// </summary>
        /// <param name="configurations">The configurations which were changed.</param>
        internal ConfigurationChangedEventArgs(Dictionary<string, string> configurations)
        {
            this.configurations = configurations;
        }

        #region Properties

        /// <summary>
        /// Gets the configurations which were changed.
        /// </summary>
        public Dictionary<string, string> Configurations
        {
            get { return configurations; }
        }

        #endregion
    }

    /// <summary>
    /// Represents the method that will handle a configuration changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="ConfigurationChangedEventArgs"/> instance containing the event data.</param>
    public delegate void ConfigurationChangedEventHandler(object sender, ConfigurationChangedEventArgs e);
}
