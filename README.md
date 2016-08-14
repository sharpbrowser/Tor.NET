# Tor.NET
A managed library to use the Tor network for SOCKS5 communications. All credits go to Chris Copeland. Originally posted on [CodeProject](http://www.codeproject.com/Articles/1072864/Tor-NET-A-managed-Tor-network-library).

# What is Tor?
Tor is a sophisticated security system that enables users from across the world to access content over the internet securely and privately. Tor uses a system of relays to transmit information without any single router having full knowledge of the client, the request or the destination server. It uses multiple layers of encryption.

# Installation and usage

1. Navigate to the [Tor download page](https://www.torproject.org/download/download.html.en).
2. Expand the "Microsoft Windows" option.
3. Download the "Expert Bundle"
4. Install the expert bundle to a known location. These files are all that are necessary to begin using the Tor network with this library.
5. Windows Vista, 7, 8, and 10 may require additional privileges to launch a new process. If this is the case, ensure that the [application manifest file specifies that the process will require elevated privileges](https://msdn.microsoft.com/en-us/library/bb756929.aspx)

# Code Samples

## Launching a new Tor process

	ClientCreateParams createParams = new ClientCreateParams();
	createParams.ConfigurationFile = "/path/to/config/file";
	createParams.DefaultConfigurationFile = "/path/to/default/config/file";
	createParams.ControlPassword = ""; // Tor does not use a control password by default, so this can be null or blank
	createParams.ControlPort = 9051;
	createParams.Path = "/path/to/tor/exe";

	Client client = Client.Create(createParams);
	
## Using the HTTP Proxy

	client.Proxy.Port = 9989;

	HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://domain.com");

	if (client.Proxy.IsRunning)
		request.Proxy = client.Proxy.WebProxy;

	using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
	{
		if (response.StatusCode == StatusCode.OK)
			; // success
	}
	
## Using the SOCKS5 Proxy

	using (Stream stream = client.GetStream("12.34.56.789", 1234))
	{
		stream.Write(..);
		stream.Read(..);
	}
	
## API

The Client class contains a Controller property which provides some methods for controlling elements of the service. The methods available in the class are designed to automate the connection to the control port, and the dispatch of commands. These methods also perform validation on the parameters provided.

In additional to being able to control the process, the client operates event monitoring in the background to synchronize circuit, OR connection, and stream information. This works by opening a constant connection to the control port and dispatching a "setevents" command. This informs the Tor process to report events which occur in the system, such as when a circuit is built or closed, or when connections or streams change.

In order to benefit from real-time updates, event handlers can be registered against events in the  Status property of the Client class. The events which can be monitored are as follows:

### client.Controller.CleanCircuits()

This informs the service that new requests should be routed through new circuits. This prevents new requests from re-using older circuits which have been used before. This also performs a clear of the client-side DNS cache.

### client.Controller.CleanDNSCache()

This causes the service to clear the client-side DNS cache for hostnames.

### client.Controller.CloseCircuit(Circuit)

This closes a circuit if it has not yet been closed, and has not failed. A closed circuit can no longer be used by Tor to generate OR connections or communicate streams.

### client.Controller.CloseStream(Stream, StreamReason)

This closes a stream if it has not yet been closed, and has not failed. This is the equivelant of terminating a socket connection.

### client.Controller.CreateCircuit()

This informs the service that a new circuit should be built, and that the service should be responsible for choosing which routers to use.

### client.Controller.CreateCircuit(string[])

This functions the same as the  CreateCircuit() method, however the method accepts an array containing nicknames or fingerprints of routers which it should use when building the circuit.

### client.Controller.ExtendCircuit(Circuit, string[])

This functions near the same as the the CreateCircuit(string[]) method, except that the routers specified in the argument list will be appended onto the end of an existing circuit.

### client.BandwidthChanged (event)

This is raised whenever the download and update rates, estimated for within the last second, have changed within the Tor service. When requests are being dispatched across a connection these values reflect the number of bytes downloaded and uploaded, on average, since the last event.

### client.CircuitsChanged (event)

This is raised whenever circuits have been altered in the Tor service. When the event is raised, this signals that the  Circuits property contains new information.

### client.ORConnectionsChanged (event)

This is raised whenever OR connections have been altered in the Tor service. When the event is raised, this signals that the  ORConnections property contains new information.

### client.StreamsChanged (event)

This is raised whenever streams have been altered in the Tor service. When the event is raised, this signals that the  Streams property contains new information.

The Status property also provides basic properties and methods for retrieving the current status of the service. For instance, the  GetAllRouters() method is an expensive function which downloads a complete register of all routers which the Tor process cares to know about.

The properties all perform requests to the control port, so execution may not be instantaneous. If the Tor client cannot connect to the control port of the Tor process, then these values may not be representative of the actual values in the process.

There are events which have not been implemented which could easily be integrated into the library. For instance, the "ADDRMAP" event is raised when an address has been resolved.