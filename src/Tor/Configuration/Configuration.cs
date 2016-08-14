using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tor.Helpers;
using System.ComponentModel;
using Tor.Controller;
using System.Diagnostics;
using Tor.Events;

namespace Tor.Config
{
    /// <summary>
    /// A class containing configuration values supported by the tor application. The configuration values reflect those available
    /// in a corresponding <c>torrc</c> document, and is populated from the application after it has been initialized.
    /// </summary>
    public sealed class Configuration : MarshalByRefObject, INotifyPropertyChanged
    {
        private readonly Client client;
        private readonly Dictionary<ConfigurationNames, object> values;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class.
        /// </summary>
        /// <param name="client">The client for which this object instance belongs.</param>
        internal Configuration(Client client)
        {
            this.client = client;
            this.values = new Dictionary<ConfigurationNames, object>();

            this.SetDefaults();
        }

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether addresses ending in ".foo.exit" on the SocksPort/TransPort/NATDPort should be
        /// converted into the target hostname that exit from the node "foo". This is disabled by default as it can open your request
        /// to modification or manipulation.
        /// </summary>
        public bool AllowDotExit
        {
            get { return (bool)GetValue(ConfigurationNames.AllowDotExit); }
            set { SetValue(ConfigurationNames.AllowDotExit, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether Tor should allow connections to hostnames containing illegal characters.
        /// </summary>
        public bool AllowNonRFC953HostNames
        {
            get { return (bool)GetValue(ConfigurationNames.AllowNonRFC953HostNames); }
            set { SetValue(ConfigurationNames.AllowNonRFC953HostNames, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Tor application should attempt to write to the disk less frequently.
        /// </summary>
        public bool AvoidDiskWrites
        {
            get { return (bool)GetValue(ConfigurationNames.AvoidDiskWrites); }
            set { SetValue(ConfigurationNames.AvoidDiskWrites, value); }
        }

        /// <summary>
        /// Gets or sets the token bandwidth limitation on the current node.
        /// </summary>
        public Bytes BandwidthRate
        {
            get { return (Bytes)GetValue(ConfigurationNames.BandwidthRate); }
            set { SetValue(ConfigurationNames.BandwidthRate, value); }
        }

        /// <summary>
        /// Gets or sets the period (in seconds) that Tor should attempt to construct a circuit.
        /// </summary>
        public int CircuitBuildTimeout
        {
            get { return (int)GetValue(ConfigurationNames.CircuitBuildTimeout); }
            set { SetValue(ConfigurationNames.CircuitBuildTimeout, value); }
        }

        /// <summary>
        /// Gets or sets the period (in seconds) that an idle (never used) circuit should remain open. When the timeout
        /// period is elapsed, the circuit will be closed, and this helps in maintaining open slots in the service. This
        /// will also close down TLS connections.
        /// </summary>
        public int CircuitIdleTimeout
        {
            get { return (int)GetValue(ConfigurationNames.CircuitIdleTimeout); }
            set { SetValue(ConfigurationNames.CircuitIdleTimeout, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether Tor prefers an OR port with an IPv6 address over an IPv4 address if
        /// a given entry node has both.
        /// </summary>
        public bool ClientPreferIPv6ORPort
        {
            get { return (bool)GetValue(ConfigurationNames.ClientPreferIPv6ORPort); }
            set { SetValue(ConfigurationNames.ClientPreferIPv6ORPort, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether Tor might connect to entry nodes using IPv6.
        /// </summary>
        public bool ClientUseIPv6
        {
            get { return (bool)GetValue(ConfigurationNames.ClientUseIPv6); }
            set { SetValue(ConfigurationNames.ClientUseIPv6, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether Tor will inform the kernel to attempt to shrink the buffers for all sockets
        /// to the size specified in the <see cref="ConstrainedSockSize"/> property.
        /// </summary>
        public bool ConstrainedSockets
        {
            get { return (bool)GetValue(ConfigurationNames.ConstrainedSockets); }
            set { SetValue(ConfigurationNames.ConstrainedSockets, value); }
        }

        /// <summary>
        /// Gets or sets the size of the socket buffers when the <see cref="ConstrainedSockets"/> setting is enabled. This value should be between
        /// 2048 and 262144 bytes, and accepts only <c>Bits.B</c> or <c>Bits.KB</c> magnitudes.
        /// </summary>
        public Bytes ConstrainedSockSize
        {
            get { return (Bytes)GetValue(ConfigurationNames.ConstrainedSockSize); }
            set { SetValue(ConfigurationNames.ConstrainedSockSize, value); }
        }

        /// <summary>
        /// Gets or sets the port on which Tor will accept control connections.
        /// </summary>
        public int ControlPort
        {
            get { return (int)GetValue(ConfigurationNames.ControlPort); }
            set { SetValue(ConfigurationNames.ControlPort, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Tor application shouldn't listen for accept any connections other than control connections.
        /// </summary>
        public bool DisableNetwork
        {
            get { return (bool)GetValue(ConfigurationNames.DisableNetwork); }
            set { SetValue(ConfigurationNames.DisableNetwork, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether Tor should not put two servers whose IP addresses are "too close" on the same circuit. Currently,
        /// two addresses are considered "too close" if they line in the same /16 range.
        /// </summary>
        public bool EnforceDistinctSubnets
        {
            get { return (bool)GetValue(ConfigurationNames.EnforceDistinctSubnets); }
            set { SetValue(ConfigurationNames.EnforceDistinctSubnets, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether circuits built by Tor should exclude relays with the
        /// <c>AllowSingleHopExits</c> flag set to <c>true</c>.
        /// </summary>
        public bool ExcludeSingleHopRelays
        {
            get { return (bool)GetValue(ConfigurationNames.ExcludeSingleHopRelays); }
            set { SetValue(ConfigurationNames.ExcludeSingleHopRelays, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether Tor should generate out-going connection requests on typically accepted
        /// port numbers (such as port 80 or 443).
        /// </summary>
        public bool FascistFirewall
        {
            get { return (bool)GetValue(ConfigurationNames.FascistFirewall); }
            set { SetValue(ConfigurationNames.FascistFirewall, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether Tor will not use the public key step for the first hop of creating circuits. A value
        /// of <c>Auto.Auto</c> indicates that the Tor network will take advice from the authorities in the latest consensus on using the feature.
        /// Disabling this feature may cause the circuit building to execute slower.
        /// </summary>
        public Auto FastFirstHopPK
        {
            get { return (Auto)GetValue(ConfigurationNames.FastFirstHopPK); }
            set { SetValue(ConfigurationNames.FastFirstHopPK, value); }
        }

        /// <summary>
        /// Gets or sets the path to a file containing IPv4 GeoIP data.
        /// </summary>
        public string GeoIPFile
        {
            get { return GetValue(ConfigurationNames.GeoIPFile) as string; }
            set { SetValue(ConfigurationNames.GeoIPFile, value); }
        }

        /// <summary>
        /// Gets or sets the path to a file containing IPv6 GeoIP data.
        /// </summary>
        public string GeoIPv6File
        {
            get { return GetValue(ConfigurationNames.GeoIPv6File) as string; }
            set { SetValue(ConfigurationNames.GeoIPv6File, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the tor application should try to use built-in (static) crypto hardware
        /// acceleration when available.
        /// </summary>
        public bool HardwareAcceleration
        {
            get { return (bool)GetValue(ConfigurationNames.HardwareAcceleration); }
            set { SetValue(ConfigurationNames.HardwareAcceleration, value); }
        }

        /// <summary>
        /// Gets or sets the hashed control password used by the control connection for validating connections.
        /// </summary>
        public string HashedControlPassword
        {
            get { return (string)GetValue(ConfigurationNames.HashedControlPassword); }
            set { SetValue(ConfigurationNames.HashedControlPassword, value); }
        }

        /// <summary>
        /// Gets or sets the proxy host through which Tor will make all directory requests. This causes Tor to connect through the proxy
        /// host address and port number, rather than communicating with the directory directly. A value of <c>Host.Null</c> indicates that
        /// no proxy should be used.
        /// </summary>
        public Host HTTPProxy
        {
            get { return (Host)GetValue(ConfigurationNames.HTTPProxy); }
            set { SetValue(ConfigurationNames.HTTPProxy, value); }
        }

        /// <summary>
        /// Gets or sets the credentials to use when authenticating with the <see cref="HTTPProxy"/> host.
        /// </summary>
        public HostAuth HTTPProxyAuthenticator
        {
            get { return (HostAuth)GetValue(ConfigurationNames.HTTPProxyAuthenticator); }
            set { SetValue(ConfigurationNames.HTTPProxyAuthenticator, value); }
        }

        /// <summary>
        /// Gets or sets the proxy host through which Tor will make all SSL directory requests. This causes Tor to connect through the proxy
        /// host address and port number, rather than communicating with the directory directly when handling secure requests.
        /// A value of <c>Host.Null</c> indicates that no proxy should be used.
        /// </summary>
        public Host HTTPSProxy
        {
            get { return (Host)GetValue(ConfigurationNames.HTTPSProxy); }
            set { SetValue(ConfigurationNames.HTTPSProxy, value); }
        }

        /// <summary>
        /// Gets or sets the credentials to use when authenticating with the <see cref="HTTPSProxy"/> host.
        /// </summary>
        public HostAuth HTTPSProxyAuthenticator
        {
            get { return (HostAuth)GetValue(ConfigurationNames.HTTPSProxyAuthenticator); }
            set { SetValue(ConfigurationNames.HTTPSProxyAuthenticator, value); }
        }

        /// <summary>
        /// Gets or sets the interval (in seconds) between packets sent from Tor containing padded keep-alive values. This is used when
        /// running behind a firewall to prevent open connections from being terminated prematurely.
        /// </summary>
        public int KeepAlivePeriod
        {
            get { return (int)GetValue(ConfigurationNames.KeepAlivePeriod); }
            set { SetValue(ConfigurationNames.KeepAlivePeriod, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether <c>CircuitBuildTimeout</c> adaptive learning is enabled.
        /// </summary>
        public bool LearnCircuitBuildTimeout
        {
            get { return (bool)GetValue(ConfigurationNames.LearnCircuitBuildTimeout); }
            set { SetValue(ConfigurationNames.LearnCircuitBuildTimeout, value); }
        }

        /// <summary>
        /// Gets or sets the maximum time (in seconds) in which a circuit can be re-used after the first use. This will allow existing
        /// circuits to be re-used, but will not attach new streams onto these circuits.
        /// </summary>
        public int MaxCircuitDirtiness
        {
            get { return (int)GetValue(ConfigurationNames.MaxCircuitDirtiness); }
            set { SetValue(ConfigurationNames.MaxCircuitDirtiness, value); }
        }

        /// <summary>
        /// Gets or sets the maximum number of circuits to be pending at a time when handling client streams. A circuit is pending
        /// if Tor has begun constructing it, but it has not yet been completely constructed.
        /// </summary>
        public int MaxClientCircuitsPending
        {
            get { return (int)GetValue(ConfigurationNames.MaxClientCircuitsPending); }
            set { SetValue(ConfigurationNames.MaxClientCircuitsPending, value); }
        }

        /// <summary>
        /// Gets or sets the period (in seconds) between Tor considering building a new circuit.
        /// </summary>
        public int NewCircuitPeriod
        {
            get { return (int)GetValue(ConfigurationNames.NewCircuitPeriod); }
            set { SetValue(ConfigurationNames.NewCircuitPeriod, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether Tor, when using an exit node that supports the feature, should try to optimistically send data
        /// to the exit node without waiting for the exit node to report whether the connection was successful. A value of <c>Auto.Auto</c>
        /// indicates that the Tor application should look at the <c>UseOptimisticData</c> parameter in the network status.
        /// </summary>
        public Auto OptimisticData
        {
            get { return (Auto)GetValue(ConfigurationNames.OptimisticData); }
            set { SetValue(ConfigurationNames.OptimisticData, value); }
        }
        
        /// <summary>
        /// Gets or sets a value indicating whether Tor should reject connections which use unsafe variants of the SOCKS protocol.
        /// </summary>
        public bool SafeSocks
        {
            get { return (bool)GetValue(ConfigurationNames.SafeSocks); }
            set { SetValue(ConfigurationNames.SafeSocks, value); }
        }

        /// <summary>
        /// Gets or sets the port number that the Tor application will listen for SOCKS connections upon. A value of zero indicates that
        /// the Tor application should not listen for SOCKS connections.
        /// </summary>
        public int SocksPort
        {
            get { return (int)GetValue(ConfigurationNames.SocksPort); }
            set { SetValue(ConfigurationNames.SocksPort, value); }
        }

        /// <summary>
        /// Gets or sets the time (in seconds) permitted for SOCKS connections to spend hand-shaking, and unattached while
        /// waiting for an appropriate circuit, before it fails.
        /// </summary>
        public int SocksTimeout
        {
            get { return (int)GetValue(ConfigurationNames.SocksTimeout); }
            set { SetValue(ConfigurationNames.SocksTimeout, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether Tor should use micro-descriptors when building circuits. Micro-descriptors are
        /// a smaller version of information needed when building circuits, allowing the client to use less directory information.
        /// </summary>
        public Auto UseMicroDescriptors
        {
            get { return (Auto)GetValue(ConfigurationNames.UseMicroDescriptors); }
            set { SetValue(ConfigurationNames.UseMicroDescriptors, value); }
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Tor.Client

        /// <summary>
        /// Called when a configuration value has changed within the client.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Events.ConfigurationChangedEventArgs"/> instance containing the event data.</param>
        private void OnClientConfigurationChanged(object sender, ConfigurationChangedEventArgs e)
        {
            foreach (KeyValuePair<string, string> v in e.Configurations)
                SetValueDirect(v.Key, v.Value);
        }

        #endregion

        /// <summary>
        /// Gets the value of a configuration.
        /// </summary>
        /// <param name="name">The configuration value to retrieve.</param>
        /// <returns>The value of the configuration.</returns>
        private object GetValue(ConfigurationNames name)
        {
            if (values.ContainsKey(name))
                return values[name];

            return null;
        }

        /// <summary>
        /// Signals the client to save all current values to the <c>torrc</c> file provided on client launch. This signal
        /// will succeed if the <c>torrc</c> was not supplied with the tor instance, but will not save anything.
        /// </summary>
        /// <returns><c>true</c> if the configuration values are saved; otherwise, <c>false</c>.</returns>
        public bool Save()
        {
            SaveConfCommand command = new SaveConfCommand();
            Response response = command.Dispatch(client);
            return response.Success;
        }

        /// <summary>
        /// Sets the value of all configurations to their default values.
        /// </summary>
        private void SetDefaults()
        {
            ConfigurationNames[] names = (ConfigurationNames[])Enum.GetValues(typeof(ConfigurationNames));

            foreach (ConfigurationNames name in names)
            {
                ConfigurationAssocAttribute attribute = ReflectionHelper.GetAttribute<ConfigurationNames, ConfigurationAssocAttribute>(name);

                if (attribute == null)
                    continue;

                values[name] = ReflectionHelper.Convert(attribute.Default, attribute.Type);
            }
        }

        /// <summary>
        /// Sets the value of a configuration.
        /// </summary>
        /// <param name="name">The configuration value to set.</param>
        /// <param name="value">The value to assign to the configuration.</param>
        private void SetValue(ConfigurationNames name, object value)
        {
            ConfigurationAssocAttribute attribute = ReflectionHelper.GetAttribute<ConfigurationNames, ConfigurationAssocAttribute>(name);

            if (attribute == null)
                return;

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

            values[name] = value;

            if (PropertyChanged != null)
                PropertyChanged(client, new PropertyChangedEventArgs(attribute.Name));

            SetConfCommand command = new SetConfCommand(attribute.Name, ReflectionHelper.Convert(value, typeof(string)) as string);
            Response response = command.Dispatch(client);
        }

        /// <summary>
        /// Sets the value of a configuration directly, without raising any <c>setconf</c> commands.
        /// </summary>
        /// <param name="name">The name of the configuration value to set.</param>
        /// <param name="value">The value to assign to the configuration.</param>
        internal void SetValueDirect(string name, string value)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            
            ConfigurationNames association = ReflectionHelper.GetEnumerator<ConfigurationNames, ConfigurationAssocAttribute>(attr => attr.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
            ConfigurationAssocAttribute attribute = ReflectionHelper.GetAttribute<ConfigurationNames, ConfigurationAssocAttribute>(association);

            if (attribute.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
            {
                values[association] = ReflectionHelper.Convert(value, attribute.Type);

                if (PropertyChanged != null)
                    PropertyChanged(client, new PropertyChangedEventArgs(attribute.Name));
            }
        }

        /// <summary>
        /// Starts the configuration listening for configuration changes using the client event handler.
        /// </summary>
        internal void Start()
        {
            this.client.Events.ConfigurationChanged += new Events.ConfigurationChangedEventHandler(OnClientConfigurationChanged);
        }
    }
}
