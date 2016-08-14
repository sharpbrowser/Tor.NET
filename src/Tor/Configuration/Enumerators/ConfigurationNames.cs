using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tor.Config
{
    /// <summary>
    /// An enumerator containing a list of configurations which can be assigned and retrieved.
    /// </summary>
    public enum ConfigurationNames
    {
        [ConfigurationAssoc("AllowDotExit", Default = false, Type = typeof(bool))]
        AllowDotExit,

        [ConfigurationAssoc("AllowNonRFC953HostNames", Default = false, Type = typeof(bool))]
        AllowNonRFC953HostNames,

        [ConfigurationAssoc("AvoidDiskWrites", Default = false, Type = typeof(bool))]
        AvoidDiskWrites,

        [ConfigurationAssoc("BandwidthRate", Default = 1099511627776.0, Type = typeof(Bytes), Validation = ConfigurationValidation.NonNull | ConfigurationValidation.SizeDivision)]
        BandwidthRate,

        [ConfigurationAssoc("CircuitBuildTimeout", Default = 60, Type = typeof(int), Validation = ConfigurationValidation.Positive)]
        CircuitBuildTimeout,

        [ConfigurationAssoc("CircuitIdleTimeout", Default = 3600, Type = typeof(int), Validation = ConfigurationValidation.Positive)]
        CircuitIdleTimeout,

        [ConfigurationAssoc("ClientPreferIPv6ORPort", Default = false, Type = typeof(bool))]
        ClientPreferIPv6ORPort,

        [ConfigurationAssoc("ClientUseIPv6", Default = false, Type = typeof(bool))]
        ClientUseIPv6,

        [ConfigurationAssoc("ControlPort", Default = 9051, Type = typeof(int), Validation = ConfigurationValidation.PortRange)]
        ControlPort,

        [ConfigurationAssoc("ConstrainedSockets", Default = false, Type = typeof(bool))]
        ConstrainedSockets,

        [ConfigurationAssoc("ConstrainedSockSize", Default = 8388608.0, Type = typeof(Bytes), Validation = ConfigurationValidation.NonNull | ConfigurationValidation.SizeDivision)]
        ConstrainedSockSize,

        [ConfigurationAssoc("DisableNetwork", Default = false, Type = typeof(bool))]
        DisableNetwork,

        [ConfigurationAssoc("EnforceDistinctSubnets", Default = true, Type = typeof(bool))]
        EnforceDistinctSubnets,

        [ConfigurationAssoc("ExcludeSingleHopRelays", Default = true, Type = typeof(bool))]
        ExcludeSingleHopRelays,

        [ConfigurationAssoc("FascistFirewall", Default = false, Type = typeof(bool))]
        FascistFirewall,

        [ConfigurationAssoc("FastFirstHopPK", Default = Auto.Auto, Type = typeof(Auto))]
        FastFirstHopPK,

        [ConfigurationAssoc("GeoIPFile", Default = null, Type = typeof(string))]
        GeoIPFile,

        [ConfigurationAssoc("GeoIPv6File", Default = null, Type = typeof(string))]
        GeoIPv6File,

        [ConfigurationAssoc("HardwareAccel", Default = false, Type = typeof(bool))]
        HardwareAcceleration,

        [ConfigurationAssoc("HashedControlPassword", Default = "", Type = typeof(string), Validation = ConfigurationValidation.NonNull)]
        HashedControlPassword,

        [ConfigurationAssoc("HTTPProxy", Type = typeof(Host))]
        HTTPProxy,

        [ConfigurationAssoc("HTTPProxyAuthenticator", Type = typeof(HostAuth))]
        HTTPProxyAuthenticator,

        [ConfigurationAssoc("HTTPSProxy", Type = typeof(Host))]
        HTTPSProxy,

        [ConfigurationAssoc("HTTPSProxyAuthenticator", Type = typeof(HostAuth))]
        HTTPSProxyAuthenticator,
        
        [ConfigurationAssoc("KeepalivePeriod", Default = 300, Type = typeof(int), Validation = ConfigurationValidation.Positive)]
        KeepAlivePeriod,

        [ConfigurationAssoc("LearnCircuitBuildTimeout", Default = true, Type = typeof(bool))]
        LearnCircuitBuildTimeout,

        [ConfigurationAssoc("MaxCircuitDirtiness", Default = 600, Type = typeof(int), Validation = ConfigurationValidation.Positive)]
        MaxCircuitDirtiness,

        [ConfigurationAssoc("MaxClientCircuitsPending", Default = 32, Type = typeof(int), Validation = ConfigurationValidation.Positive)]
        MaxClientCircuitsPending,

        [ConfigurationAssoc("NewCircuitPeriod", Default = 30, Type = typeof(int), Validation = ConfigurationValidation.Positive)]
        NewCircuitPeriod,

        [ConfigurationAssoc("OptimisticData", Default = Auto.Auto, Type = typeof(Auto))]
        OptimisticData,

        [ConfigurationAssoc("SafeSocks", Default = false, Type = typeof(bool))]
        SafeSocks,

        [ConfigurationAssoc("SocksPort", Default = 9051, Type = typeof(int), Validation = ConfigurationValidation.PortRange)]
        SocksPort,

        [ConfigurationAssoc("SocksTimeout", Default = 120, Type = typeof(int), Validation = ConfigurationValidation.Positive)]
        SocksTimeout,

        [ConfigurationAssoc("UseMicrodescriptors", Default = Auto.Auto, Type = typeof(Auto))]
        UseMicroDescriptors
    }
}
