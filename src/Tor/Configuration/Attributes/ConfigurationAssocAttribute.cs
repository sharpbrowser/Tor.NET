using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tor.Config
{
    /// <summary>
    /// Specifies the associated tor configuration name against an enumerator value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    internal sealed class ConfigurationAssocAttribute : Attribute
    {
        private readonly string name;

        private object defaultValue;
        private Type type;
        private ConfigurationValidation validation;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationAssocAttribute"/> class.
        /// </summary>
        /// <param name="name">The name of the configuration within the tor <c>torrc</c> configuration file.</param>
        public ConfigurationAssocAttribute(string name)
        {
            this.defaultValue = null;
            this.name = name;
            this.type = null;
            this.validation = ConfigurationValidation.None;
        }

        #region Properties

        /// <summary>
        /// Gets or sets the default value of the configuration.
        /// </summary>
        public object Default
        {
            get { return defaultValue; }
            set { defaultValue = value; }
        }

        /// <summary>
        /// Gets the name of the configuration within the tor <c>torrc</c> configuration file.
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Gets or sets the type of value expected for the configuration.
        /// </summary>
        public Type Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// Gets or sets the validation to perform against the configuration value.
        /// </summary>
        public ConfigurationValidation Validation
        {
            get { return validation; }
            set { validation = value; }
        }

        #endregion
    }
}
