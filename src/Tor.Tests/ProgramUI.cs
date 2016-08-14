using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using Tor.IO;
using System.IO;
using System.Threading;
using Tor.Config;
using System.Reflection;

namespace Tor.Tests
{
    /// <summary>
    /// A class representing the user interface of the application.
    /// </summary>
    public partial class ProgramUI : Form
    {
        private const int PROGRESS_DISABLED = -1;
        private const int PROGRESS_INDETERMINATE = -2;

        private RouterCollection allRouters;
        private Client client;
        private CircuitCollection circuits;
        private volatile bool closing;
        private ORConnectionCollection connections;
        private StreamCollection streams;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgramUI"/> class.
        /// </summary>
        public ProgramUI()
        {
            InitializeComponent();
            InitializeFonts();

            allRouters = null;
            closing = false;
            browserControl.CanGoBackChanged += (s, e) => backButton.Enabled = browserControl.CanGoBack;
            browserControl.CanGoForwardChanged += (s, e) => forwardButton.Enabled = browserControl.CanGoForward;
            browserControl.StatusTextChanged += new EventHandler(OnBrowserControlStatusTextChanged);
            browserControl.ProgressChanged += (s, e) => SetStatusProgress((int)Math.Floor(e.CurrentProgress * 100.00 / e.MaximumProgress));
            routerList.MouseDoubleClick += (s, e) =>
            {
                int index = routerList.SelectedIndex;
                if (index < 0 || allRouters.Count <= index)
                    return;
                Router router = allRouters[index];
                if (router == null)
                    return;
                string countryCode = client.Status.GetCountryCode(router);
                MessageBox.Show(
                    "Router information\n" +
                    "---------------------------\n" +
                    "Nickname: " + router.Nickname + "\n" +
                    "Identity: " + router.Identity + "\n" +
                    "IP Address: " + router.IPAddress + "\n" +
                    "Digest: " + router.Digest + "\n" +
                    "Published: " + (router.Publication == DateTime.MinValue ? "Unknown" : Convert.ToString(router.Publication)) + "\n" +
                    "Bandwidth: " + (router.Bandwidth) + "\n" +
                    "Country: " + (countryCode != null ? countryCode : "Unknown") + "\n" +
                    "---------------------------\n" +
                    "Flags:\n" +
                    router.Flags.ToString(),
                    "Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            };

            backButton.Click += (s, e) => browserControl.GoBack();
            forwardButton.Click += (s, e) => browserControl.GoForward();
        }

        #region System.Windows.Forms.Form
        
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitializeTor();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.FormClosing" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.FormClosingEventArgs" /> that contains the event data.</param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (client != null && client.IsRunning)
            {
                closing = true;
                client.Status.BandwidthChanged -= OnClientBandwidthChanged;
                client.Status.CircuitsChanged -= OnClientCircuitsChanged;
                client.Dispose();

                e.Cancel = true;
            }
        }

        #endregion

        #region Tor.Client

        /// <summary>
        /// Called when the bandwidth values within the client are changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="BandwidthEventArgs"/> instance containing the event data.</param>
        private void OnClientBandwidthChanged(object sender, BandwidthEventArgs e)
        {
            if (closing)
                return;
            
            Invoke((Action)delegate
            {
                if (e.Downloaded.Value == 0 && e.Uploaded.Value == 0)
                    bandwidthLabel.Text = "";
                else
                    bandwidthLabel.Text = string.Format("Down: {0}/s, Up: {1}/s", e.Downloaded, e.Uploaded);
            });
        }

        /// <summary>
        /// Called when a circuit has changed within the client.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnClientCircuitsChanged(object sender, EventArgs e)
        {
            if (closing)
                return;

            circuits = client.Status.Circuits;

            Invoke((Action)delegate
            {
                circuitTree.BeginUpdate();

                List<TreeNode> removals = new List<TreeNode>();

                foreach (TreeNode n in circuitTree.Nodes)
                    removals.Add(n);

                foreach (Circuit circuit in circuits)
                {
                    bool added = false;
                    TreeNode node = null;

                    if (!showClosedCheckBox.Checked)
                        if (circuit.Status == CircuitStatus.Closed || circuit.Status == CircuitStatus.Failed)
                            continue;

                    foreach (TreeNode existingNode in circuitTree.Nodes)
                        if (((Circuit)existingNode.Tag).ID == circuit.ID)
                        {
                            node = existingNode;
                            break;
                        }

                    string text = string.Format("Circuit #{0} [{1}] ({2})", circuit.ID, circuit.Status, circuit.Routers.Count);
                    string tooltip = string.Format("Created: {0}\nBuild Flags: {1}", circuit.TimeCreated, circuit.BuildFlags);
                    
                    if (node == null)
                    {
                        node = new TreeNode(text);
                        node.ContextMenuStrip = circuitMenuStrip;
                        node.Tag = circuit;
                        node.ToolTipText = tooltip;
                        added = true;
                    }
                    else
                    {
                        node.Text = text;
                        node.ToolTipText = tooltip;
                        node.Nodes.Clear();

                        removals.Remove(node);
                    }

                    foreach (Router router in circuit.Routers)
                        node.Nodes.Add(string.Format("{0} [{1}] ({2}/s)", router.Nickname, router.IPAddress, router.Bandwidth));

                    if (added)
                        circuitTree.Nodes.Add(node);
                }

                foreach (TreeNode remove in removals)
                    circuitTree.Nodes.Remove(remove);

                circuitTree.EndUpdate();
            });
        }

        /// <summary>
        /// Called when an OR connection has changed within the client.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnClientConnectionsChanged(object sender, EventArgs e)
        {
            if (closing)
                return;

            connections = client.Status.ORConnections;

            Invoke((Action)delegate
            {
                connectionTree.BeginUpdate();

                List<TreeNode> removals = new List<TreeNode>();

                foreach (TreeNode n in connectionTree.Nodes)
                    removals.Add(n);

                foreach (ORConnection connection in connections)
                {
                    bool added = false;
                    TreeNode node = null;

                    if (!showClosedCheckBox.Checked)
                        if (connection.Status == ORStatus.Closed || connection.Status == ORStatus.Failed)
                            continue;

                    foreach (TreeNode existingNode in connectionTree.Nodes)
                    {
                        ORConnection existing = (ORConnection)existingNode.Tag;

                        if (connection.ID != 0 && connection.ID == existing.ID)
                        {
                            node = existingNode;
                            break;
                        }
                        if (connection.Target.Equals(existing.Target, StringComparison.CurrentCultureIgnoreCase))
                        {
                            node = existingNode;
                            break;
                        }
                    }

                    string text = string.Format("Connection #{0} [{1}] ({2})", connection.ID, connection.Status, connection.Target);

                    if (node == null)
                    {
                        node = new TreeNode(text);
                        node.Tag = connection;
                        added = true;
                    }
                    else
                    {
                        node.Text = text;
                        node.Nodes.Clear();

                        removals.Remove(node);
                    }
                    
                    if (added)
                        connectionTree.Nodes.Add(node);
                }

                foreach (TreeNode remove in removals)
                    connectionTree.Nodes.Remove(remove);

                connectionTree.EndUpdate();
            });
        }

        /// <summary>
        /// Called when the tor client has been shutdown.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnClientShutdown(object sender, EventArgs e)
        {
            if (!closing)
            {
                MessageBox.Show("The tor client has been terminated without warning", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            client = null;

            Invoke((Action)delegate { Close(); });
        }

        /// <summary>
        /// Called when a stream has changed within the client.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnClientStreamsChanged(object sender, EventArgs e)
        {
            if (closing)
                return;

            streams = client.Status.Streams;

            Invoke((Action)delegate
            {
                streamsTree.BeginUpdate();

                List<TreeNode> removals = new List<TreeNode>();

                foreach (TreeNode n in streamsTree.Nodes)
                    removals.Add(n);

                foreach (Stream stream in streams)
                {
                    bool added = false;
                    TreeNode node = null;

                    if (!showClosedCheckBox.Checked)
                        if (stream.Status == StreamStatus.Failed || stream.Status == StreamStatus.Closed)
                            continue;

                    foreach (TreeNode existingNode in streamsTree.Nodes)
                        if (((Stream)existingNode.Tag).ID == stream.ID)
                        {
                            node = existingNode;
                            break;
                        }

                    Circuit circuit = null;

                    if (stream.CircuitID > 0)
                        circuit = circuits.Where(c => c.ID == stream.CircuitID).FirstOrDefault();

                    string text = string.Format("Stream #{0} [{1}] ({2}, {3})", stream.ID, stream.Status, stream.Target, circuit == null ? "detached" : "circuit #" + circuit.ID);
                    string tooltip = string.Format("Purpose: {0}", stream.Purpose);

                    if (node == null)
                    {
                        node = new TreeNode(text);
                        node.ContextMenuStrip = streamMenuStrip;
                        node.Tag = stream;
                        node.ToolTipText = tooltip;
                        added = true;
                    }
                    else
                    {
                        node.Text = text;
                        node.ToolTipText = tooltip;
                        node.Nodes.Clear();

                        removals.Remove(node);
                    }
                    
                    if (added)
                        streamsTree.Nodes.Add(node);
                }

                foreach (TreeNode remove in removals)
                    streamsTree.Nodes.Remove(remove);

                streamsTree.EndUpdate();
            });
        }

        #endregion

        /// <summary>
        /// Initializes the fonts of the user interface.
        /// </summary>
        private void InitializeFonts()
        {
            Font = SystemFonts.DialogFont;
            circuitMenuStrip.Font = Font;
            statusBar.Font = Font;
        }

        /// <summary>
        /// Initializes the tor client.
        /// </summary>
        private void InitializeTor()
        {
            Process[] previous = Process.GetProcessesByName("tor");
            
            SetStatusProgress(PROGRESS_INDETERMINATE);

            if (previous != null && previous.Length > 0)
            {
                SetStatusText("Killing previous tor instances..");

                foreach (Process process in previous)
                    process.Kill();
            }
            
            SetStatusText("Creating the tor client..");

            ClientCreateParams createParameters = new ClientCreateParams();
            createParameters.ConfigurationFile = ConfigurationManager.AppSettings["torConfigurationFile"];
            createParameters.ControlPassword = ConfigurationManager.AppSettings["torControlPassword"];
            createParameters.ControlPort = Convert.ToInt32(ConfigurationManager.AppSettings["torControlPort"]);
            createParameters.DefaultConfigurationFile = ConfigurationManager.AppSettings["torDefaultConfigurationFile"];
            createParameters.Path = ConfigurationManager.AppSettings["torPath"];

            createParameters.SetConfig(ConfigurationNames.AvoidDiskWrites, true);
            createParameters.SetConfig(ConfigurationNames.GeoIPFile, Path.Combine(Environment.CurrentDirectory, @"Tor\Data\Tor\geoip"));
            createParameters.SetConfig(ConfigurationNames.GeoIPv6File, Path.Combine(Environment.CurrentDirectory, @"Tor\Data\Tor\geoip6"));

            client = Client.Create(createParameters);

            if (!client.IsRunning)
            {
                SetStatusProgress(PROGRESS_DISABLED);
                SetStatusText("The tor client could not be created");
                return;
            }
            
            client.Status.BandwidthChanged += OnClientBandwidthChanged;
            client.Status.CircuitsChanged += OnClientCircuitsChanged;
            client.Status.ORConnectionsChanged += OnClientConnectionsChanged;
            client.Status.StreamsChanged += OnClientStreamsChanged;
            client.Configuration.PropertyChanged += (s, e) => { Invoke((Action)delegate { configGrid.Refresh(); }); };
            client.Shutdown += new EventHandler(OnClientShutdown);
            
            if (!Program.SetConnectionProxy(string.Format("127.0.0.1:{0}", client.Proxy.Port)))
                MessageBox.Show("The application could not set the default connection proxy. The browser control is not using the tor service as a proxy!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            SetStatusProgress(PROGRESS_DISABLED);
            SetStatusText("Ready");

            configGrid.SelectedObject = client.Configuration;

            SetStatusText("Downloading routers");
            SetStatusProgress(PROGRESS_INDETERMINATE);

            ThreadPool.QueueUserWorkItem(state =>
            {
                allRouters = client.Status.GetAllRouters();

                if (allRouters == null)
                {
                    SetStatusText("Could not download routers");
                    SetStatusProgress(PROGRESS_DISABLED);
                }
                else
                {
                    Invoke((Action)delegate
                    {
                        routerList.BeginUpdate();

                        foreach (Router router in allRouters)
                            routerList.Items.Add(string.Format("{0} [{1}] ({2}/s)", router.Nickname, router.IPAddress, router.Bandwidth));

                        routerList.EndUpdate();
                    });

                    SetStatusText("Ready");
                    SetStatusProgress(PROGRESS_DISABLED);
                }
            });
        }

        /// <summary>
        /// Called when the user presses a key in the text box.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PreviewKeyDownEventArgs"/> instance containing the event data.</param>
        private void OnAddressTextBockPreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    string addr = addressTextBox.Text;

                    if (!addr.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase) &&
                        !addr.StartsWith("https://", StringComparison.CurrentCultureIgnoreCase))
                        addr = "http://" + addr;

                    Uri uri = new Uri(addr);
                    browserControl.Navigate(uri);
                }
                catch
                {
                    MessageBox.Show("Please check the address value", "Error");
                }
            }
        }

        /// <summary>
        /// Called when the browser control is navigating to a location.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="WebBrowserNavigatingEventArgs"/> instance containing the event data.</param>
        private void OnBrowserControlNavigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            SetStatusProgress(PROGRESS_INDETERMINATE);
        }

        /// <summary>
        /// Called when the browser control has finished navigation.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="WebBrowserNavigatedEventArgs"/> instance containing the event data.</param>
        private void OnBrowserControlNavigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            SetStatusProgress(PROGRESS_DISABLED);
            Invoke((Action)delegate { addressTextBox.Text = browserControl.Url.ToString(); });
        }

        /// <summary>
        /// Called when the browser control status text changes.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnBrowserControlStatusTextChanged(object sender, EventArgs e)
        {
            SetStatusText(browserControl.StatusText);
        }

        /// <summary>
        /// Called when the user chooses to close a circuit.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnCloseCircuitMenuItemClick(object sender, EventArgs e)
        {
            TreeNode selection = circuitTree.SelectedNode;

            if (selection == null)
                return;

            Circuit circuit = selection.Tag as Circuit;

            if (circuit == null || circuit.Status == CircuitStatus.Failed || circuit.Status == CircuitStatus.Closed)
                return;

            circuit.Close();
        }

        /// <summary>
        /// Called when the user chooses to close a stream.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnCloseStreamMenuItemClick(object sender, EventArgs e)
        {
            TreeNode selection = streamsTree.SelectedNode;

            if (selection == null)
                return;

            Stream stream = selection.Tag as Stream;

            if (stream == null || stream.Status == StreamStatus.Closed || stream.Status == StreamStatus.Failed)
                return;

            stream.Close();
        }

        /// <summary>
        /// Called when the user chooses to extend a circuit with a new router.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnExtendCircuitMenuItemClick(object sender, EventArgs e)
        {
            TreeNode selection = circuitTree.SelectedNode;

            if (selection == null)
                return;

            Circuit circuit = selection.Tag as Circuit;

            if (circuit == null || circuit.Status == CircuitStatus.Failed || circuit.Status == CircuitStatus.Closed)
                return;

            CircuitExtendUI ui = new CircuitExtendUI();
            ui.Circuit = circuit;
            ui.Routers = allRouters;

            if (ui.ShowDialog(this) == DialogResult.OK)
            {
                if (!circuits.Any(c => c.ID == ui.Circuit.ID))
                {
                    MessageBox.Show("The selected circuit is no longer active", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Router router = ui.SelectedRouter;

                if (router == null)
                    return;
                
                if (circuit.Extend(router))
                    SetStatusText("Circuit extended");
                else
                    SetStatusText("Circuit extend failed");
            }
        }

        /// <summary>
        /// Called when the user selects to create a new circuit.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnNewCircuitButtonClick(object sender, EventArgs e)
        {
            if (client.IsRunning)
                client.Controller.CreateCircuit();
        }
        
        /// <summary>
        /// Called when the user requests that new circuits be generated.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnNewCircuitsButtonClick(object sender, EventArgs e)
        {
            if (client.IsRunning)
                client.Controller.CleanCircuits();
        }

        /// <summary>
        /// Called when the user chooses to refresh the web-browser control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnRefreshButtonClick(object sender, EventArgs e)
        {
            if (browserControl.Url != null)
                browserControl.Refresh(WebBrowserRefreshOption.Completely);
        }

        /// <summary>
        /// Called when the user chooses to show or hide closed/failed values.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnShowClosedCheckChanged(object sender, EventArgs e)
        {
            OnClientCircuitsChanged(client, EventArgs.Empty);
            OnClientConnectionsChanged(client, EventArgs.Empty);
            OnClientStreamsChanged(client, EventArgs.Empty);
        }
        
        /// <summary>
        /// Sets the value of the status progress bar within the status bar.
        /// </summary>
        /// <param name="value">The value to set the progress bar to.</param>
        private void SetStatusProgress(int value)
        {
            if (closing)
                return;

            Invoke((Action)delegate
            {
                if (value == PROGRESS_DISABLED)
                    statusProgress.Visible = false;
                else if (value == PROGRESS_INDETERMINATE)
                {
                    statusProgress.Visible = true;
                    statusProgress.Style = ProgressBarStyle.Marquee;
                }
                else
                {
                    statusProgress.Visible = true;
                    statusProgress.Value = Math.Min(100, Math.Max(0, value));
                }
            });
        }

        /// <summary>
        /// Sets the value of the status label within the status bar.
        /// </summary>
        /// <param name="message">The message to set the status bar text to.</param>
        private void SetStatusText(string message)
        {
            if (closing)
                return;

            Invoke((Action)delegate { statusLabel.Text = message; });
        }
    }
}
