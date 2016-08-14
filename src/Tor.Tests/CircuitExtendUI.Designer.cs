namespace Tor.Tests
{
    partial class CircuitExtendUI
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
            this.label1 = new System.Windows.Forms.Label();
            this.circuitLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.routerComboBox = new System.Windows.Forms.ComboBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.extendButton = new System.Windows.Forms.Button();
            this.bandwidthCheck = new System.Windows.Forms.CheckBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.bandwidthNum = new System.Windows.Forms.NumericUpDown();
            this.bandwidthCombo = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.bandwidthNum)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(0, 0, 10, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Selected Circuit";
            // 
            // circuitLabel
            // 
            this.circuitLabel.AutoSize = true;
            this.circuitLabel.Location = new System.Drawing.Point(101, 10);
            this.circuitLabel.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.circuitLabel.Name = "circuitLabel";
            this.circuitLabel.Size = new System.Drawing.Size(13, 13);
            this.circuitLabel.TabIndex = 1;
            this.circuitLabel.Text = "?";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 36);
            this.label2.Margin = new System.Windows.Forms.Padding(0, 0, 10, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Selected Router";
            // 
            // routerComboBox
            // 
            this.routerComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.routerComboBox.FormattingEnabled = true;
            this.routerComboBox.Location = new System.Drawing.Point(104, 33);
            this.routerComboBox.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.routerComboBox.Name = "routerComboBox";
            this.routerComboBox.Size = new System.Drawing.Size(280, 21);
            this.routerComboBox.TabIndex = 3;
            this.toolTip.SetToolTip(this.routerComboBox, "This is the router which will be appended onto the circuit as a new exit node.");
            this.routerComboBox.SelectedIndexChanged += new System.EventHandler(this.OnRouterSelectedIndexChanged);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(10, 98);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(0);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 23);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // extendButton
            // 
            this.extendButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.extendButton.Location = new System.Drawing.Point(306, 95);
            this.extendButton.Name = "extendButton";
            this.extendButton.Size = new System.Drawing.Size(75, 23);
            this.extendButton.TabIndex = 5;
            this.extendButton.Text = "&Extend";
            this.extendButton.UseVisualStyleBackColor = true;
            this.extendButton.Click += new System.EventHandler(this.OnExtendButtonClick);
            // 
            // bandwidthCheck
            // 
            this.bandwidthCheck.AutoSize = true;
            this.bandwidthCheck.Location = new System.Drawing.Point(104, 67);
            this.bandwidthCheck.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.bandwidthCheck.Name = "bandwidthCheck";
            this.bandwidthCheck.Size = new System.Drawing.Size(100, 17);
            this.bandwidthCheck.TabIndex = 6;
            this.bandwidthCheck.Text = "Bandwidth over";
            this.toolTip.SetToolTip(this.bandwidthCheck, "Enables filtering the routers by the bandwidth.");
            this.bandwidthCheck.UseVisualStyleBackColor = true;
            this.bandwidthCheck.CheckedChanged += new System.EventHandler(this.OnBandwidthCheckChanged);
            // 
            // toolTip
            // 
            this.toolTip.AutomaticDelay = 1000;
            this.toolTip.ToolTipTitle = "Information";
            // 
            // bandwidthNum
            // 
            this.bandwidthNum.Enabled = false;
            this.bandwidthNum.Location = new System.Drawing.Point(214, 66);
            this.bandwidthNum.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.bandwidthNum.Maximum = new decimal(new int[] {
            4194304,
            0,
            0,
            0});
            this.bandwidthNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.bandwidthNum.Name = "bandwidthNum";
            this.bandwidthNum.Size = new System.Drawing.Size(60, 20);
            this.bandwidthNum.TabIndex = 7;
            this.bandwidthNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.bandwidthNum.ValueChanged += new System.EventHandler(this.OnBandwidthValueChanged);
            // 
            // bandwidthCombo
            // 
            this.bandwidthCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.bandwidthCombo.Enabled = false;
            this.bandwidthCombo.FormattingEnabled = true;
            this.bandwidthCombo.Items.AddRange(new object[] {
            "B",
            "KB",
            "MB",
            "GB"});
            this.bandwidthCombo.Location = new System.Drawing.Point(284, 65);
            this.bandwidthCombo.Margin = new System.Windows.Forms.Padding(0);
            this.bandwidthCombo.Name = "bandwidthCombo";
            this.bandwidthCombo.Size = new System.Drawing.Size(50, 21);
            this.bandwidthCombo.TabIndex = 8;
            this.bandwidthCombo.SelectedIndexChanged += new System.EventHandler(this.OnBandwidthUnitChanged);
            // 
            // CircuitExtendUI
            // 
            this.AcceptButton = this.extendButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(394, 131);
            this.Controls.Add(this.bandwidthCombo);
            this.Controls.Add(this.bandwidthNum);
            this.Controls.Add(this.bandwidthCheck);
            this.Controls.Add(this.extendButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.routerComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.circuitLabel);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CircuitExtendUI";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Tor Client - Extend Circuit";
            ((System.ComponentModel.ISupportInitialize)(this.bandwidthNum)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label circuitLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox routerComboBox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button extendButton;
        private System.Windows.Forms.CheckBox bandwidthCheck;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.NumericUpDown bandwidthNum;
        private System.Windows.Forms.ComboBox bandwidthCombo;
    }
}