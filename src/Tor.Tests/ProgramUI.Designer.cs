namespace Tor.Tests
{
    partial class ProgramUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.bandwidthLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.containerPanel = new System.Windows.Forms.Panel();
            this.browserContainerPanel = new System.Windows.Forms.Panel();
            this.forwardButton = new System.Windows.Forms.Button();
            this.newCircuitsButton = new System.Windows.Forms.Button();
            this.addressTextBox = new System.Windows.Forms.TextBox();
            this.browserPanel = new System.Windows.Forms.Panel();
            this.browserControl = new System.Windows.Forms.WebBrowser();
            this.refreshButton = new System.Windows.Forms.Button();
            this.backButton = new System.Windows.Forms.Button();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.showClosedCheckBox = new System.Windows.Forms.CheckBox();
            this.connectionTree = new System.Windows.Forms.TreeView();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.circuitTree = new System.Windows.Forms.TreeView();
            this.streamsTree = new System.Windows.Forms.TreeView();
            this.label2 = new System.Windows.Forms.Label();
            this.newCircuitButton = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.configGrid = new System.Windows.Forms.PropertyGrid();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.routerList = new System.Windows.Forms.ListBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.circuitMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.extendCircuitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.closeCircuitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.streamMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.closeStreamToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusBar.SuspendLayout();
            this.containerPanel.SuspendLayout();
            this.browserContainerPanel.SuspendLayout();
            this.browserPanel.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.circuitMenuStrip.SuspendLayout();
            this.streamMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusBar
            // 
            this.statusBar.AutoSize = false;
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.statusProgress,
            this.bandwidthLabel});
            this.statusBar.Location = new System.Drawing.Point(0, 655);
            this.statusBar.Name = "statusBar";
            this.statusBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
            this.statusBar.Size = new System.Drawing.Size(1008, 24);
            this.statusBar.TabIndex = 0;
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(610, 21);
            this.statusLabel.Spring = true;
            this.statusLabel.Text = "Loading";
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // statusProgress
            // 
            this.statusProgress.Name = "statusProgress";
            this.statusProgress.Size = new System.Drawing.Size(100, 20);
            this.statusProgress.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.statusProgress.Visible = false;
            // 
            // bandwidthLabel
            // 
            this.bandwidthLabel.AutoSize = false;
            this.bandwidthLabel.Name = "bandwidthLabel";
            this.bandwidthLabel.Size = new System.Drawing.Size(250, 21);
            // 
            // containerPanel
            // 
            this.containerPanel.Controls.Add(this.browserContainerPanel);
            this.containerPanel.Controls.Add(this.splitter1);
            this.containerPanel.Controls.Add(this.tabControl);
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerPanel.Location = new System.Drawing.Point(0, 0);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Padding = new System.Windows.Forms.Padding(10);
            this.containerPanel.Size = new System.Drawing.Size(1008, 655);
            this.containerPanel.TabIndex = 1;
            // 
            // browserContainerPanel
            // 
            this.browserContainerPanel.Controls.Add(this.forwardButton);
            this.browserContainerPanel.Controls.Add(this.newCircuitsButton);
            this.browserContainerPanel.Controls.Add(this.addressTextBox);
            this.browserContainerPanel.Controls.Add(this.browserPanel);
            this.browserContainerPanel.Controls.Add(this.refreshButton);
            this.browserContainerPanel.Controls.Add(this.backButton);
            this.browserContainerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.browserContainerPanel.Location = new System.Drawing.Point(280, 10);
            this.browserContainerPanel.Margin = new System.Windows.Forms.Padding(0);
            this.browserContainerPanel.Name = "browserContainerPanel";
            this.browserContainerPanel.Size = new System.Drawing.Size(718, 635);
            this.browserContainerPanel.TabIndex = 13;
            // 
            // forwardButton
            // 
            this.forwardButton.Enabled = false;
            this.forwardButton.Image = global::Tor.Tests.Properties.Resources.right_round_16;
            this.forwardButton.Location = new System.Drawing.Point(26, 0);
            this.forwardButton.Margin = new System.Windows.Forms.Padding(0, 0, 3, 6);
            this.forwardButton.Name = "forwardButton";
            this.forwardButton.Size = new System.Drawing.Size(23, 23);
            this.forwardButton.TabIndex = 8;
            this.toolTip.SetToolTip(this.forwardButton, "Navigation forward");
            this.forwardButton.UseVisualStyleBackColor = true;
            // 
            // newCircuitsButton
            // 
            this.newCircuitsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.newCircuitsButton.Location = new System.Drawing.Point(0, 612);
            this.newCircuitsButton.Margin = new System.Windows.Forms.Padding(0);
            this.newCircuitsButton.Name = "newCircuitsButton";
            this.newCircuitsButton.Size = new System.Drawing.Size(117, 23);
            this.newCircuitsButton.TabIndex = 2;
            this.newCircuitsButton.Text = "&Enforce New Circuits";
            this.toolTip.SetToolTip(this.newCircuitsButton, "Sends a request to the tor service to generate new circuits.");
            this.newCircuitsButton.UseVisualStyleBackColor = true;
            this.newCircuitsButton.Click += new System.EventHandler(this.OnNewCircuitsButtonClick);
            // 
            // addressTextBox
            // 
            this.addressTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.addressTextBox.Location = new System.Drawing.Point(82, 2);
            this.addressTextBox.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.addressTextBox.Name = "addressTextBox";
            this.addressTextBox.Size = new System.Drawing.Size(636, 20);
            this.addressTextBox.TabIndex = 6;
            this.addressTextBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.OnAddressTextBockPreviewKeyDown);
            // 
            // browserPanel
            // 
            this.browserPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.browserPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.browserPanel.Controls.Add(this.browserControl);
            this.browserPanel.Location = new System.Drawing.Point(0, 28);
            this.browserPanel.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.browserPanel.Name = "browserPanel";
            this.browserPanel.Size = new System.Drawing.Size(718, 578);
            this.browserPanel.TabIndex = 5;
            // 
            // browserControl
            // 
            this.browserControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.browserControl.Location = new System.Drawing.Point(0, 0);
            this.browserControl.Margin = new System.Windows.Forms.Padding(2);
            this.browserControl.MinimumSize = new System.Drawing.Size(13, 13);
            this.browserControl.Name = "browserControl";
            this.browserControl.ScriptErrorsSuppressed = true;
            this.browserControl.Size = new System.Drawing.Size(716, 576);
            this.browserControl.TabIndex = 4;
            this.browserControl.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.OnBrowserControlNavigated);
            this.browserControl.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.OnBrowserControlNavigating);
            // 
            // refreshButton
            // 
            this.refreshButton.Image = global::Tor.Tests.Properties.Resources.refresh_16;
            this.refreshButton.Location = new System.Drawing.Point(52, 0);
            this.refreshButton.Margin = new System.Windows.Forms.Padding(0, 0, 7, 6);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(23, 23);
            this.refreshButton.TabIndex = 11;
            this.toolTip.SetToolTip(this.refreshButton, "Refresh the page");
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.OnRefreshButtonClick);
            // 
            // backButton
            // 
            this.backButton.Enabled = false;
            this.backButton.Image = global::Tor.Tests.Properties.Resources.left_round_16;
            this.backButton.Location = new System.Drawing.Point(0, 0);
            this.backButton.Margin = new System.Windows.Forms.Padding(0, 0, 3, 6);
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(23, 23);
            this.backButton.TabIndex = 7;
            this.toolTip.SetToolTip(this.backButton, "Navigate backward");
            this.backButton.UseVisualStyleBackColor = true;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(277, 10);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 635);
            this.splitter1.TabIndex = 12;
            this.splitter1.TabStop = false;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.tabPage2);
            this.tabControl.Controls.Add(this.tabPage3);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Left;
            this.tabControl.Location = new System.Drawing.Point(10, 10);
            this.tabControl.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(267, 635);
            this.tabControl.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.showClosedCheckBox);
            this.tabPage1.Controls.Add(this.connectionTree);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.circuitTree);
            this.tabPage1.Controls.Add(this.streamsTree);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.newCircuitButton);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(10);
            this.tabPage1.Size = new System.Drawing.Size(259, 609);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Status";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // showClosedCheckBox
            // 
            this.showClosedCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.showClosedCheckBox.AutoSize = true;
            this.showClosedCheckBox.Location = new System.Drawing.Point(13, 582);
            this.showClosedCheckBox.Margin = new System.Windows.Forms.Padding(0);
            this.showClosedCheckBox.Name = "showClosedCheckBox";
            this.showClosedCheckBox.Size = new System.Drawing.Size(117, 17);
            this.showClosedCheckBox.TabIndex = 13;
            this.showClosedCheckBox.Text = "Show closed/failed";
            this.showClosedCheckBox.UseVisualStyleBackColor = true;
            this.showClosedCheckBox.CheckedChanged += new System.EventHandler(this.OnShowClosedCheckChanged);
            // 
            // connectionTree
            // 
            this.connectionTree.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.connectionTree.FullRowSelect = true;
            this.connectionTree.ItemHeight = 20;
            this.connectionTree.Location = new System.Drawing.Point(13, 220);
            this.connectionTree.Margin = new System.Windows.Forms.Padding(0, 0, 10, 13);
            this.connectionTree.Name = "connectionTree";
            this.connectionTree.Size = new System.Drawing.Size(239, 150);
            this.connectionTree.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 202);
            this.label3.Margin = new System.Windows.Forms.Padding(0, 0, 10, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Current Connections";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 15);
            this.label1.Margin = new System.Windows.Forms.Padding(0, 0, 10, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Current Circuits";
            // 
            // circuitTree
            // 
            this.circuitTree.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.circuitTree.FullRowSelect = true;
            this.circuitTree.ItemHeight = 20;
            this.circuitTree.Location = new System.Drawing.Point(10, 39);
            this.circuitTree.Margin = new System.Windows.Forms.Padding(0, 0, 10, 13);
            this.circuitTree.Name = "circuitTree";
            this.circuitTree.Size = new System.Drawing.Size(239, 150);
            this.circuitTree.TabIndex = 1;
            // 
            // streamsTree
            // 
            this.streamsTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.streamsTree.FullRowSelect = true;
            this.streamsTree.ItemHeight = 20;
            this.streamsTree.Location = new System.Drawing.Point(13, 400);
            this.streamsTree.Margin = new System.Windows.Forms.Padding(0, 0, 10, 13);
            this.streamsTree.Name = "streamsTree";
            this.streamsTree.Size = new System.Drawing.Size(239, 170);
            this.streamsTree.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 382);
            this.label2.Margin = new System.Windows.Forms.Padding(0, 0, 10, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Current Streams";
            // 
            // newCircuitButton
            // 
            this.newCircuitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.newCircuitButton.Image = global::Tor.Tests.Properties.Resources.plus_16;
            this.newCircuitButton.Location = new System.Drawing.Point(225, 10);
            this.newCircuitButton.Margin = new System.Windows.Forms.Padding(0, 0, 7, 6);
            this.newCircuitButton.Name = "newCircuitButton";
            this.newCircuitButton.Size = new System.Drawing.Size(23, 23);
            this.newCircuitButton.TabIndex = 3;
            this.toolTip.SetToolTip(this.newCircuitButton, "Create a new circuit");
            this.newCircuitButton.UseVisualStyleBackColor = true;
            this.newCircuitButton.Click += new System.EventHandler(this.OnNewCircuitButtonClick);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.configGrid);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(10);
            this.tabPage2.Size = new System.Drawing.Size(259, 589);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Configuration";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // configGrid
            // 
            this.configGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.configGrid.Location = new System.Drawing.Point(10, 10);
            this.configGrid.Margin = new System.Windows.Forms.Padding(0);
            this.configGrid.Name = "configGrid";
            this.configGrid.Size = new System.Drawing.Size(209, 630);
            this.configGrid.TabIndex = 12;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.routerList);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(10);
            this.tabPage3.Size = new System.Drawing.Size(259, 589);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Routers";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // routerList
            // 
            this.routerList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.routerList.FormattingEnabled = true;
            this.routerList.Location = new System.Drawing.Point(13, 13);
            this.routerList.Name = "routerList";
            this.routerList.Size = new System.Drawing.Size(203, 628);
            this.routerList.TabIndex = 12;
            // 
            // toolTip
            // 
            this.toolTip.AutomaticDelay = 1000;
            this.toolTip.ToolTipTitle = "Information";
            // 
            // circuitMenuStrip
            // 
            this.circuitMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.extendCircuitToolStripMenuItem,
            this.toolStripMenuItem1,
            this.closeCircuitToolStripMenuItem});
            this.circuitMenuStrip.Name = "circuitMenuStrip";
            this.circuitMenuStrip.Size = new System.Drawing.Size(148, 54);
            // 
            // extendCircuitToolStripMenuItem
            // 
            this.extendCircuitToolStripMenuItem.Name = "extendCircuitToolStripMenuItem";
            this.extendCircuitToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.extendCircuitToolStripMenuItem.Text = "E&xtend Circuit";
            this.extendCircuitToolStripMenuItem.Click += new System.EventHandler(this.OnExtendCircuitMenuItemClick);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(144, 6);
            // 
            // closeCircuitToolStripMenuItem
            // 
            this.closeCircuitToolStripMenuItem.Name = "closeCircuitToolStripMenuItem";
            this.closeCircuitToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.closeCircuitToolStripMenuItem.Text = "&Close Circuit";
            this.closeCircuitToolStripMenuItem.Click += new System.EventHandler(this.OnCloseCircuitMenuItemClick);
            // 
            // streamMenuStrip
            // 
            this.streamMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeStreamToolStripMenuItem});
            this.streamMenuStrip.Name = "streamMenuStrip";
            this.streamMenuStrip.Size = new System.Drawing.Size(144, 26);
            // 
            // closeStreamToolStripMenuItem
            // 
            this.closeStreamToolStripMenuItem.Name = "closeStreamToolStripMenuItem";
            this.closeStreamToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.closeStreamToolStripMenuItem.Text = "&Close Stream";
            this.closeStreamToolStripMenuItem.Click += new System.EventHandler(this.OnCloseStreamMenuItemClick);
            // 
            // ProgramUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 679);
            this.Controls.Add(this.containerPanel);
            this.Controls.Add(this.statusBar);
            this.Name = "ProgramUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tor Client";
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.containerPanel.ResumeLayout(false);
            this.browserContainerPanel.ResumeLayout(false);
            this.browserContainerPanel.PerformLayout();
            this.browserPanel.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.circuitMenuStrip.ResumeLayout(false);
            this.streamMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.ToolStripProgressBar statusProgress;
        private System.Windows.Forms.Panel containerPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TreeView circuitTree;
        private System.Windows.Forms.Button newCircuitsButton;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ToolStripStatusLabel bandwidthLabel;
        private System.Windows.Forms.Button newCircuitButton;
        private System.Windows.Forms.ContextMenuStrip circuitMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem closeCircuitToolStripMenuItem;
        private System.Windows.Forms.Panel browserPanel;
        private System.Windows.Forms.WebBrowser browserControl;
        private System.Windows.Forms.TextBox addressTextBox;
        private System.Windows.Forms.Button backButton;
        private System.Windows.Forms.Button forwardButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TreeView streamsTree;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.ContextMenuStrip streamMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem closeStreamToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.PropertyGrid configGrid;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ListBox routerList;
        private System.Windows.Forms.ToolStripMenuItem extendCircuitToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel browserContainerPanel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TreeView connectionTree;
        private System.Windows.Forms.CheckBox showClosedCheckBox;
    }
}

