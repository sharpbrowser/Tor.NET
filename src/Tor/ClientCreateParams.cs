using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Tor.Config;
using Tor.Helpers;

namespace Tor
{
    /// <summary>
    /// A class containing the configurations needed when launching a tor application executable.
    /// </summary>
    [DebuggerStepThrough]
    [Serializable]
    public sealed class ClientCreateParams
    {
        private string defaultConfigurationFile;
        private string configurationFile;
        private string controlPassword;
        private int controlPort;
        private Dictionary<ConfigurationNames, object> overrides;
        private string path;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientCreateParams"/> class.
        /// </summary>
        public ClientCreateParams() : this(null, 9051, "", null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientCreateParams"/> class.
        /// </summary>
        /// <param name="path">The path to the application executable file.</param>
        public ClientCreateParams(string path) : this(path, 9051, "", null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientCreateParams"/> class.
        /// </summary>
        /// <param name="path">The path to the application executable file.</param>
        /// <param name="configurationFile">The path to the configuration file.</param>
        public ClientCreateParams(string path, string configurationFile) : this(path, 9051, "", configurationFile)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientCreateParams"/> class.
        /// </summary>
        /// <param name="path">The path to the application executable file.</param>
        /// <param name="controlPort">The port number which the application will listening on for control connections.</param>
        public ClientCreateParams(string path, int controlPort) : this(path, controlPort, "", null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientCreateParams"/> class.
        /// </summary>
        /// <param name="path">The path to the application executable file.</param>
        /// <param name="controlPort">The port number which the application will listening on for control connections.</param>
        /// <param name="controlPassword">The password used when authenticating with the control connection.</param>
        public ClientCreateParams(string path, int controlPort, string controlPassword) : this(path, controlPort, controlPassword, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientCreateParams"/> class.
        /// </summary>
        /// <param name="path">The path to the application executable file.</param>
        /// <param name="controlPort">The port number which the application will listening on for control connections.</param>
        /// <param name="controlPassword">The password used when authenticating with the control connection.</param>
        /// <param name="configurationFile">The path to the configuration file.</param>
        public ClientCreateParams(string path, int controlPort, string controlPassword, string configurationFile)
        {
            this.configurationFile = configurationFile;
            this.controlPassword = controlPassword;
            this.controlPort = controlPort;
            this.defaultConfigurationFile = null;
            this.overrides = new Dictionary<ConfigurationNames, object>();
            this.path = path;
        }

        #region Properties

        /// <summary>
        /// Gets or sets the path to the configuration file containing the default values. A value of <c>null</c>
        /// indicates that no default configuration file will be loaded.
        /// </summary>
        public string DefaultConfigurationFile
        {
            get { return defaultConfigurationFile; }
            set { defaultConfigurationFile = value; }
        }

        /// <summary>
        /// Gets or sets the path to the configuration file. A value of <c>null</c> indicates that no
        /// configuration file will be loaded.
        /// </summary>
        public string ConfigurationFile
        {
            get { return configurationFile; }
            set { configurationFile = value; }
        }

        /// <summary>
        /// Gets or sets the password used when authenticating with the tor application on the control connection. A value of
        /// <c>null</c> or blank indicates that no password has been configured.
        /// </summary>
        public string ControlPassword
        {
            get { return controlPassword; }
            set { controlPassword = value; }
        }

        /// <summary>
        /// Gets or sets the port number which the application will be listening on for control connections.
        /// </summary>
        public int ControlPort
        {
            get { return controlPort; }
            set { controlPort = value; }
        }

        /// <summary>
        /// Gets or sets the path to the application executable file.
        /// </summary>
        public string Path
        {
            get { return path; }
            set { path = value; }
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
            ClientCreateParams compare = obj as ClientCreateParams;

            if (compare == null)
                return false;

            return compare.configurationFile == configurationFile &&
                   compare.controlPassword == controlPassword &&
                   compare.controlPort == controlPort &&
                   compare.defaultConfigurationFile == defaultConfigurationFile &&
                   compare.path == path;
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
                hash = hash * 23 + (defaultConfigurationFile != null ? defaultConfigurationFile.GetHashCode() : 0);
                hash = hash * 23 + (configurationFile != null ? configurationFile.GetHashCode() : 0);
                hash = hash * 23 + (controlPassword != null ? controlPassword.GetHashCode() : 0);
                hash = hash * 23 + (controlPort);
                hash = hash * 23 + (path != null ? path.GetHashCode() : 0);
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
            StringBuilder builder;

            builder = new StringBuilder();
            builder.Append("--ignore-missing-torrc ");
            builder.Append("--allow-missing-torrc ");
            builder.AppendFormat("--ControlPort {0}", controlPort);

            /** TEMPORARY: DELETE LATER **/
            builder.AppendFormat(" --SocksPort 9050");

            if (!string.IsNullOrWhiteSpace(configurationFile))
                builder.AppendFormat(" -f \"{0}\"", configurationFile);

            if (!string.IsNullOrWhiteSpace(defaultConfigurationFile))
                builder.AppendFormat(" --defaults-torrc \"{0}\"", defaultConfigurationFile);

            foreach (KeyValuePair<ConfigurationNames, object> over in overrides)
            {
                string value = "";

                ConfigurationAssocAttribute attribute = ReflectionHelper.GetAttribute<ConfigurationNames, ConfigurationAssocAttribute>(over.Key);

                if (attribute == null)
                    continue;

                if (over.Value != null)
                    value = ReflectionHelper.Convert(over.Value, typeof(string)) as string;

                if (value == null)
                    value = "";
                if (value.Contains(" "))
                    value = "\"" + value + "\"";

                builder.AppendFormat(" --{0} {1}", attribute.Name, value);
            }

            return builder.ToString();
        }

        #endregion

        /// <summary>
        /// Sets the value of a configuration supplied with the process arguments. Setting a configuration value within the create
        /// parameters overrides any values stored in the configuration file. The control port can also be set using this method.
        /// </summary>
        /// <param name="configuration">The configuration which should be overridden.</param>
        /// <param name="value">The value of the configuration. This should align with the type specified in the <see cref="Tor.Config.Configuration"/> class.</param>
        public void SetConfig(ConfigurationNames configuration, object value)
        {
            ConfigurationAssocAttribute attribute = ReflectionHelper.GetAttribute<ConfigurationNames, ConfigurationAssocAttribute>(configuration);

            if (attribute == null)
                return;

            if (value != null && !value.GetType().Equals(attribute.Type))
                throw new ArgumentException("The value of this configuration should be of type " + attribute.Type, "value");

            ConfigurationValidation validation = attribute.Validation;

            if (validation != ConfigurationValidation.None)
            {
                if (validation.HasFlag(ConfigurationValidation.NonNull) && value == null)
                    throw new ArgumentNullException("value");

                if (validation.HasFlag(ConfigurationValidation.NonZero) ||
                    validation.HasFlag(ConfigurationValidation.NonNegative) ||
                    validation.HasFlag(ConfigurationValidation.PortRange))
                {
                    int converted = Convert.ToInt32(value);

                    if (validation.HasFlag(ConfigurationValidation.NonZero) && converted == 0)
                        throw new ArgumentOutOfRangeException("value", "The value of this configuration cannot be zero");
                    if (validation.HasFlag(ConfigurationValidation.NonNegative) && converted < 0)
                        throw new ArgumentOutOfRangeException("value", "The value of this configuration cannot be below zero");
                    if (validation.HasFlag(ConfigurationValidation.PortRange) && (converted <= 0 || short.MaxValue < converted))
                        throw new ArgumentOutOfRangeException("value", "The value of this configuration must be within a valid port range");
                }

                if (validation.HasFlag(ConfigurationValidation.SizeDivision))
                {
                    double converted = Convert.ToDouble(value);

                    if (converted % 1024 != 0)
                        throw new ArgumentException("The value of this configuration must be divisible by 1024");
                }
            }

            if (configuration == ConfigurationNames.ControlPort)
                controlPort = Convert.ToInt32(value);
            else
                overrides[configuration] = value;
        }

        /// <summary>
        /// Validates the parameters assigned to the object, and throws relevant exceptions where necessary.
        /// </summary>
        internal void ValidateParameters()
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new TorException("The path cannot be null or white-space");
            if (controlPort <= 0 || short.MaxValue < controlPort)
                throw new TorException("The control port number must be within a valid port range");
        }
    }
}
