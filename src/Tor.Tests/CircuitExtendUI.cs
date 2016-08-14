using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Tor.Tests
{
    /// <summary>
    /// A class representing a dialog shown when extending a circuit.
    /// </summary>
    public partial class CircuitExtendUI : Form
    {
        private Circuit circuit;
        private RouterCollection routers;
        private IList<Router> selectionRouters;
        private Router selectedRouter;
        private int updating;

        /// <summary>
        /// Initializes a new instance of the <see cref="CircuitExtendUI"/> class.
        /// </summary>
        public CircuitExtendUI()
        {
            InitializeComponent();
            Font = SystemFonts.DialogFont;
        }

        #region Properties

        /// <summary>
        /// Gets or sets the circuit targetted by the extension form.
        /// </summary>
        public Circuit Circuit
        {
            get { return circuit; }
            set
            {
                if (circuit != value)
                {
                    circuit = value;
                    circuitLabel.Text = (circuit == null ? "" : string.Format("Circuit #{0}", circuit.ID));
                }
            }
        }

        /// <summary>
        /// Gets or sets the collection of routers which can be extended onto circuit.
        /// </summary>
        public RouterCollection Routers
        {
            get { return routers; }
            set
            {
                if (routers != value)
                {
                    routers = value;
                    selectionRouters = value;
                    routerComboBox.Items.Clear();

                    if (routers != null)
                    {
                        routerComboBox.BeginUpdate();
                        foreach (Router router in routers)
                            routerComboBox.Items.Add(string.Format("{0} [{1}] ({2}/s)", router.IPAddress, router.Nickname, router.Bandwidth));
                        routerComboBox.EndUpdate();
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the selected router.
        /// </summary>
        public Router SelectedRouter
        {
            get { return selectedRouter; }
            set
            {
                if (selectedRouter != value)
                {
                    selectedRouter = value;
                    routerComboBox.SelectedItem = selectedRouter;
                }
            }
        }

        #endregion

        /// <summary>
        /// Called when the user changes the bandwidth selection.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnBandwidthCheckChanged(object sender, EventArgs e)
        {
            bandwidthCombo.Enabled = bandwidthCheck.Checked;
            bandwidthNum.Enabled = bandwidthCheck.Checked;
            UpdateFilter();
        }

        /// <summary>
        /// Called when the user changes the bandwidth units.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnBandwidthUnitChanged(object sender, EventArgs e)
        {
            if (bandwidthCombo.SelectedIndex == -1)
            {
                bandwidthCombo.SelectedIndex = 0;
                return;
            }
            UpdateFilter();
        }

        /// <summary>
        /// Called when the user changes the bandwidth parameter.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnBandwidthValueChanged(object sender, EventArgs e)
        {
            UpdateFilter();
        }

        /// <summary>
        /// Called when the user clicks to extend the circuit.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnExtendButtonClick(object sender, EventArgs e)
        {
            if (selectedRouter == null)
            {
                MessageBox.Show("You must select a router before attempting to extend the circuit", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Called when the user chooses a router.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnRouterSelectedIndexChanged(object sender, EventArgs e)
        {
            if (routerComboBox.SelectedIndex == -1)
                selectedRouter = null;
            else
                selectedRouter = selectionRouters[routerComboBox.SelectedIndex];
        }

        /// <summary>
        /// Updates the list of routers based on a selection of filtering options.
        /// </summary>
        private void UpdateFilter()
        {
            if (Interlocked.CompareExchange(ref updating, 1, 0) == 0)
            {
                routerComboBox.Enabled = false;

                bool isChecked = bandwidthCheck.Checked;
                double checkValue = (double)bandwidthNum.Value;
                int units = bandwidthCombo.SelectedIndex;
                string previousSelection = routerComboBox.SelectedText;

                ThreadPool.QueueUserWorkItem(state =>
                {
                    bool refreshList = false;

                    if (!isChecked)
                    {
                        if (selectionRouters != routers)
                        {
                            selectionRouters = routers;
                            refreshList = true;
                        }
                    }
                    else
                    {
                        if (units == -1)
                        {
                            if (selectionRouters != routers)
                            {
                                selectionRouters = routers;
                                refreshList = true;
                            }
                        }
                        else
                        {
                            Bits b = (Bits)units;
                            List<Router> matched = routers.Where(r => (r.Bandwidth.Units == b) ? r.Bandwidth.Value > checkValue : r.Bandwidth.ToUnit(b).Value > checkValue).ToList();
                            selectionRouters = matched;
                            refreshList = true;
                        }
                    }

                    if (refreshList)
                    {
                        Invoke((Action)delegate
                        {
                            routerComboBox.Items.Clear();
                            routerComboBox.BeginUpdate();
                            foreach (Router router in selectionRouters)
                                routerComboBox.Items.Add(string.Format("{0} [{1}] ({2}/s)", router.Nickname, router.IPAddress, router.Bandwidth));
                            routerComboBox.EndUpdate();
                            routerComboBox.SelectedText = previousSelection;
                        });
                    }

                    Invoke((Action)delegate { routerComboBox.Enabled = true; });

                    Interlocked.CompareExchange(ref updating, 0, 1);
                });
            }
        }
    }
}
