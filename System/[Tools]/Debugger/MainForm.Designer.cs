namespace Tools.Debugger
{
    partial class MainForm
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
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem(new string[] {
            "index",
            "u32",
            "0x123456"}, -1);
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem(new string[] {
            "EAX",
            "0x00001234"}, -1);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem(new string[] {
            "0x00180060",
            "System::Interface::WindowManager::Loop",
            "0x1D4"}, -1);
            this.TabsPanel = new System.Windows.Forms.Panel();
            this.Tabs = new System.Windows.Forms.TabControl();
            this.CodePanel = new System.Windows.Forms.TabPage();
            this.VariablesPanel = new System.Windows.Forms.Panel();
            this.VariableList = new System.Windows.Forms.ListView();
            this.VariableNameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.VariableValueColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.VariableTypeColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.VariableDataColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.VariablesLabel = new System.Windows.Forms.Label();
            this.MemoryPanel = new System.Windows.Forms.TabPage();
            this.MemoryViewPanel = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.MemoryBox = new System.Windows.Forms.RichTextBox();
            this.MemoryLabel = new System.Windows.Forms.Label();
            this.RegistersPanel = new System.Windows.Forms.Panel();
            this.RegisterList = new System.Windows.Forms.ListView();
            this.RegisterNameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.RegisterValueColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label4 = new System.Windows.Forms.Label();
            this.MainMenu = new System.Windows.Forms.ToolStrip();
            this.ContinueButton = new System.Windows.Forms.ToolStripButton();
            this.BreakButton = new System.Windows.Forms.ToolStripButton();
            this.StepGdbButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.StartVMButton = new System.Windows.Forms.ToolStripButton();
            this.StopVMButton = new System.Windows.Forms.ToolStripButton();
            this.RestartVMButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.StepLineButton = new System.Windows.Forms.ToolStripButton();
            this.StepOverButton = new System.Windows.Forms.ToolStripButton();
            this.CallstackPanel = new System.Windows.Forms.Panel();
            this.FrameList = new System.Windows.Forms.ListView();
            this.FramePointerColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.FrameFunctionColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.FrameOffsetColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CallstackLabel = new System.Windows.Forms.Label();
            this.ConsolePanel = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.DebuggerWorker = new System.ComponentModel.BackgroundWorker();
            this.TabsPanel.SuspendLayout();
            this.Tabs.SuspendLayout();
            this.CodePanel.SuspendLayout();
            this.VariablesPanel.SuspendLayout();
            this.MemoryPanel.SuspendLayout();
            this.MemoryViewPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.RegistersPanel.SuspendLayout();
            this.MainMenu.SuspendLayout();
            this.CallstackPanel.SuspendLayout();
            this.ConsolePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // TabsPanel
            // 
            this.TabsPanel.BackColor = System.Drawing.SystemColors.Control;
            this.TabsPanel.Controls.Add(this.Tabs);
            this.TabsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabsPanel.Location = new System.Drawing.Point(0, 231);
            this.TabsPanel.Name = "TabsPanel";
            this.TabsPanel.Padding = new System.Windows.Forms.Padding(2);
            this.TabsPanel.Size = new System.Drawing.Size(464, 266);
            this.TabsPanel.TabIndex = 0;
            // 
            // Tabs
            // 
            this.Tabs.Controls.Add(this.CodePanel);
            this.Tabs.Controls.Add(this.MemoryPanel);
            this.Tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tabs.Location = new System.Drawing.Point(2, 2);
            this.Tabs.Name = "Tabs";
            this.Tabs.SelectedIndex = 0;
            this.Tabs.Size = new System.Drawing.Size(460, 262);
            this.Tabs.TabIndex = 1;
            // 
            // CodePanel
            // 
            this.CodePanel.Controls.Add(this.VariablesPanel);
            this.CodePanel.Location = new System.Drawing.Point(4, 22);
            this.CodePanel.Name = "CodePanel";
            this.CodePanel.Size = new System.Drawing.Size(452, 236);
            this.CodePanel.TabIndex = 1;
            this.CodePanel.Text = "Variables";
            this.CodePanel.UseVisualStyleBackColor = true;
            // 
            // VariablesPanel
            // 
            this.VariablesPanel.BackColor = System.Drawing.SystemColors.Control;
            this.VariablesPanel.Controls.Add(this.VariableList);
            this.VariablesPanel.Controls.Add(this.VariablesLabel);
            this.VariablesPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VariablesPanel.Location = new System.Drawing.Point(0, 0);
            this.VariablesPanel.Name = "VariablesPanel";
            this.VariablesPanel.Padding = new System.Windows.Forms.Padding(2, 22, 2, 2);
            this.VariablesPanel.Size = new System.Drawing.Size(452, 236);
            this.VariablesPanel.TabIndex = 4;
            // 
            // VariableList
            // 
            this.VariableList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.VariableNameColumn,
            this.VariableValueColumn,
            this.VariableTypeColumn,
            this.VariableDataColumn});
            this.VariableList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VariableList.FullRowSelect = true;
            this.VariableList.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem4});
            this.VariableList.Location = new System.Drawing.Point(2, 22);
            this.VariableList.Name = "VariableList";
            this.VariableList.Size = new System.Drawing.Size(448, 212);
            this.VariableList.TabIndex = 2;
            this.VariableList.UseCompatibleStateImageBehavior = false;
            this.VariableList.View = System.Windows.Forms.View.Details;
            this.VariableList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.VariableList_MouseDoubleClick);
            // 
            // VariableNameColumn
            // 
            this.VariableNameColumn.Text = "Name";
            this.VariableNameColumn.Width = 96;
            // 
            // VariableValueColumn
            // 
            this.VariableValueColumn.Text = "Value";
            this.VariableValueColumn.Width = 80;
            // 
            // VariableTypeColumn
            // 
            this.VariableTypeColumn.Text = "Type";
            this.VariableTypeColumn.Width = 144;
            // 
            // VariableDataColumn
            // 
            this.VariableDataColumn.Text = "Data";
            this.VariableDataColumn.Width = 96;
            // 
            // VariablesLabel
            // 
            this.VariablesLabel.AutoSize = true;
            this.VariablesLabel.Location = new System.Drawing.Point(3, 5);
            this.VariablesLabel.Name = "VariablesLabel";
            this.VariablesLabel.Size = new System.Drawing.Size(50, 13);
            this.VariablesLabel.TabIndex = 1;
            this.VariablesLabel.Text = "Variables";
            // 
            // MemoryPanel
            // 
            this.MemoryPanel.Controls.Add(this.MemoryViewPanel);
            this.MemoryPanel.Controls.Add(this.RegistersPanel);
            this.MemoryPanel.Location = new System.Drawing.Point(4, 22);
            this.MemoryPanel.Name = "MemoryPanel";
            this.MemoryPanel.Size = new System.Drawing.Size(452, 98);
            this.MemoryPanel.TabIndex = 0;
            this.MemoryPanel.Text = "Memory";
            this.MemoryPanel.UseVisualStyleBackColor = true;
            // 
            // MemoryViewPanel
            // 
            this.MemoryViewPanel.BackColor = System.Drawing.SystemColors.Control;
            this.MemoryViewPanel.Controls.Add(this.panel1);
            this.MemoryViewPanel.Controls.Add(this.MemoryLabel);
            this.MemoryViewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MemoryViewPanel.Location = new System.Drawing.Point(0, 0);
            this.MemoryViewPanel.Name = "MemoryViewPanel";
            this.MemoryViewPanel.Padding = new System.Windows.Forms.Padding(2, 22, 2, 2);
            this.MemoryViewPanel.Size = new System.Drawing.Size(322, 98);
            this.MemoryViewPanel.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Controls.Add(this.MemoryBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(2, 22);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(1);
            this.panel1.Size = new System.Drawing.Size(318, 74);
            this.panel1.TabIndex = 2;
            // 
            // MemoryBox
            // 
            this.MemoryBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.MemoryBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MemoryBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MemoryBox.Location = new System.Drawing.Point(1, 1);
            this.MemoryBox.Name = "MemoryBox";
            this.MemoryBox.ReadOnly = true;
            this.MemoryBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.MemoryBox.Size = new System.Drawing.Size(316, 72);
            this.MemoryBox.TabIndex = 3;
            this.MemoryBox.Text = "00180070  00 12 22 34 44 56 78 45  abc..dsf";
            // 
            // MemoryLabel
            // 
            this.MemoryLabel.AutoSize = true;
            this.MemoryLabel.Location = new System.Drawing.Point(3, 5);
            this.MemoryLabel.Name = "MemoryLabel";
            this.MemoryLabel.Size = new System.Drawing.Size(44, 13);
            this.MemoryLabel.TabIndex = 1;
            this.MemoryLabel.Text = "Memory";
            // 
            // RegistersPanel
            // 
            this.RegistersPanel.BackColor = System.Drawing.SystemColors.Control;
            this.RegistersPanel.Controls.Add(this.RegisterList);
            this.RegistersPanel.Controls.Add(this.label4);
            this.RegistersPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.RegistersPanel.Location = new System.Drawing.Point(322, 0);
            this.RegistersPanel.Name = "RegistersPanel";
            this.RegistersPanel.Padding = new System.Windows.Forms.Padding(2, 22, 2, 2);
            this.RegistersPanel.Size = new System.Drawing.Size(130, 98);
            this.RegistersPanel.TabIndex = 1;
            // 
            // RegisterList
            // 
            this.RegisterList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.RegisterNameColumn,
            this.RegisterValueColumn});
            this.RegisterList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RegisterList.FullRowSelect = true;
            this.RegisterList.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem5});
            this.RegisterList.Location = new System.Drawing.Point(2, 22);
            this.RegisterList.Name = "RegisterList";
            this.RegisterList.Size = new System.Drawing.Size(126, 74);
            this.RegisterList.TabIndex = 2;
            this.RegisterList.UseCompatibleStateImageBehavior = false;
            this.RegisterList.View = System.Windows.Forms.View.Details;
            this.RegisterList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.RegisterList_MouseDoubleClick);
            // 
            // RegisterNameColumn
            // 
            this.RegisterNameColumn.Text = "Name";
            this.RegisterNameColumn.Width = 42;
            // 
            // RegisterValueColumn
            // 
            this.RegisterValueColumn.Text = "Value";
            this.RegisterValueColumn.Width = 80;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Registers";
            // 
            // MainMenu
            // 
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ContinueButton,
            this.BreakButton,
            this.StepGdbButton,
            this.toolStripSeparator1,
            this.StartVMButton,
            this.StopVMButton,
            this.RestartVMButton,
            this.toolStripSeparator4,
            this.StepLineButton,
            this.StepOverButton});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(464, 25);
            this.MainMenu.TabIndex = 1;
            this.MainMenu.Text = "toolStrip1";
            // 
            // ContinueButton
            // 
            this.ContinueButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ContinueButton.Enabled = false;
            this.ContinueButton.Image = ((System.Drawing.Image)(resources.GetObject("ContinueButton.Image")));
            this.ContinueButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ContinueButton.Name = "ContinueButton";
            this.ContinueButton.Size = new System.Drawing.Size(23, 22);
            this.ContinueButton.Text = "Continue";
            this.ContinueButton.Click += new System.EventHandler(this.ContinueButton_Click);
            // 
            // BreakButton
            // 
            this.BreakButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BreakButton.Enabled = false;
            this.BreakButton.Image = ((System.Drawing.Image)(resources.GetObject("BreakButton.Image")));
            this.BreakButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BreakButton.Name = "BreakButton";
            this.BreakButton.Size = new System.Drawing.Size(23, 22);
            this.BreakButton.Text = "Break";
            this.BreakButton.Click += new System.EventHandler(this.BreakButton_Click);
            // 
            // StepGdbButton
            // 
            this.StepGdbButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.StepGdbButton.Enabled = false;
            this.StepGdbButton.Image = ((System.Drawing.Image)(resources.GetObject("StepGdbButton.Image")));
            this.StepGdbButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.StepGdbButton.Name = "StepGdbButton";
            this.StepGdbButton.Size = new System.Drawing.Size(23, 22);
            this.StepGdbButton.Text = "Step one cycle";
            this.StepGdbButton.Click += new System.EventHandler(this.StepGdbButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // StartVMButton
            // 
            this.StartVMButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.StartVMButton.Image = ((System.Drawing.Image)(resources.GetObject("StartVMButton.Image")));
            this.StartVMButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.StartVMButton.Name = "StartVMButton";
            this.StartVMButton.Size = new System.Drawing.Size(23, 22);
            this.StartVMButton.Text = "Start VM";
            this.StartVMButton.Click += new System.EventHandler(this.StartVMButton_Click);
            // 
            // StopVMButton
            // 
            this.StopVMButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.StopVMButton.Enabled = false;
            this.StopVMButton.Image = ((System.Drawing.Image)(resources.GetObject("StopVMButton.Image")));
            this.StopVMButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.StopVMButton.Name = "StopVMButton";
            this.StopVMButton.Size = new System.Drawing.Size(23, 22);
            this.StopVMButton.Text = "Stop VM";
            this.StopVMButton.Click += new System.EventHandler(this.StopVMButton_Click);
            // 
            // RestartVMButton
            // 
            this.RestartVMButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RestartVMButton.Enabled = false;
            this.RestartVMButton.Image = ((System.Drawing.Image)(resources.GetObject("RestartVMButton.Image")));
            this.RestartVMButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RestartVMButton.Name = "RestartVMButton";
            this.RestartVMButton.Size = new System.Drawing.Size(23, 22);
            this.RestartVMButton.Text = "Restart VM";
            this.RestartVMButton.Click += new System.EventHandler(this.RestartVMButton_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // StepLineButton
            // 
            this.StepLineButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.StepLineButton.Enabled = false;
            this.StepLineButton.Image = ((System.Drawing.Image)(resources.GetObject("StepLineButton.Image")));
            this.StepLineButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.StepLineButton.Name = "StepLineButton";
            this.StepLineButton.Size = new System.Drawing.Size(23, 22);
            this.StepLineButton.Text = "Step one line";
            this.StepLineButton.Click += new System.EventHandler(this.StepLineButton_Click);
            // 
            // StepOverButton
            // 
            this.StepOverButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.StepOverButton.Enabled = false;
            this.StepOverButton.Image = ((System.Drawing.Image)(resources.GetObject("StepOverButton.Image")));
            this.StepOverButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.StepOverButton.Name = "StepOverButton";
            this.StepOverButton.Size = new System.Drawing.Size(23, 22);
            this.StepOverButton.Text = "Step over";
            this.StepOverButton.Click += new System.EventHandler(this.StepOverButton_Click);
            // 
            // CallstackPanel
            // 
            this.CallstackPanel.BackColor = System.Drawing.SystemColors.Control;
            this.CallstackPanel.Controls.Add(this.FrameList);
            this.CallstackPanel.Controls.Add(this.CallstackLabel);
            this.CallstackPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.CallstackPanel.Location = new System.Drawing.Point(0, 25);
            this.CallstackPanel.Name = "CallstackPanel";
            this.CallstackPanel.Padding = new System.Windows.Forms.Padding(2, 22, 2, 2);
            this.CallstackPanel.Size = new System.Drawing.Size(464, 206);
            this.CallstackPanel.TabIndex = 3;
            // 
            // FrameList
            // 
            this.FrameList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.FramePointerColumn,
            this.FrameFunctionColumn,
            this.FrameOffsetColumn});
            this.FrameList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FrameList.FullRowSelect = true;
            this.FrameList.HideSelection = false;
            this.FrameList.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem6});
            this.FrameList.Location = new System.Drawing.Point(2, 22);
            this.FrameList.Name = "FrameList";
            this.FrameList.Size = new System.Drawing.Size(460, 182);
            this.FrameList.TabIndex = 1;
            this.FrameList.UseCompatibleStateImageBehavior = false;
            this.FrameList.View = System.Windows.Forms.View.Details;
            this.FrameList.SelectedIndexChanged += new System.EventHandler(this.FrameList_SelectedIndexChanged);
            this.FrameList.DoubleClick += new System.EventHandler(this.FrameList_DoubleClick);
            // 
            // FramePointerColumn
            // 
            this.FramePointerColumn.Text = "Pointer";
            this.FramePointerColumn.Width = 80;
            // 
            // FrameFunctionColumn
            // 
            this.FrameFunctionColumn.Text = "Function";
            this.FrameFunctionColumn.Width = 240;
            // 
            // FrameOffsetColumn
            // 
            this.FrameOffsetColumn.Text = "Offset";
            this.FrameOffsetColumn.Width = 56;
            // 
            // CallstackLabel
            // 
            this.CallstackLabel.AutoSize = true;
            this.CallstackLabel.Location = new System.Drawing.Point(3, 5);
            this.CallstackLabel.Name = "CallstackLabel";
            this.CallstackLabel.Size = new System.Drawing.Size(50, 13);
            this.CallstackLabel.TabIndex = 0;
            this.CallstackLabel.Text = "Callstack";
            // 
            // ConsolePanel
            // 
            this.ConsolePanel.BackColor = System.Drawing.SystemColors.Control;
            this.ConsolePanel.Controls.Add(this.label3);
            this.ConsolePanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ConsolePanel.Location = new System.Drawing.Point(0, 497);
            this.ConsolePanel.Name = "ConsolePanel";
            this.ConsolePanel.Padding = new System.Windows.Forms.Padding(2, 22, 2, 2);
            this.ConsolePanel.Size = new System.Drawing.Size(464, 244);
            this.ConsolePanel.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Console";
            // 
            // DebuggerWorker
            // 
            this.DebuggerWorker.WorkerReportsProgress = true;
            this.DebuggerWorker.WorkerSupportsCancellation = true;
            this.DebuggerWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.DebuggerWorker_DoWork);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 741);
            this.Controls.Add(this.TabsPanel);
            this.Controls.Add(this.ConsolePanel);
            this.Controls.Add(this.CallstackPanel);
            this.Controls.Add(this.MainMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Debugger";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.TabsPanel.ResumeLayout(false);
            this.Tabs.ResumeLayout(false);
            this.CodePanel.ResumeLayout(false);
            this.VariablesPanel.ResumeLayout(false);
            this.VariablesPanel.PerformLayout();
            this.MemoryPanel.ResumeLayout(false);
            this.MemoryViewPanel.ResumeLayout(false);
            this.MemoryViewPanel.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.RegistersPanel.ResumeLayout(false);
            this.RegistersPanel.PerformLayout();
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.CallstackPanel.ResumeLayout(false);
            this.CallstackPanel.PerformLayout();
            this.ConsolePanel.ResumeLayout(false);
            this.ConsolePanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel TabsPanel;
        private System.Windows.Forms.ToolStrip MainMenu;
        private System.Windows.Forms.ToolStripButton ContinueButton;
        private System.Windows.Forms.ToolStripButton BreakButton;
        private System.Windows.Forms.ToolStripButton StepGdbButton;
        private System.Windows.Forms.ToolStripButton StopVMButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton RestartVMButton;
        private System.Windows.Forms.Panel CallstackPanel;
        private System.Windows.Forms.ListView FrameList;
        private System.Windows.Forms.ColumnHeader FramePointerColumn;
        private System.Windows.Forms.ColumnHeader FrameFunctionColumn;
        private System.Windows.Forms.ColumnHeader FrameOffsetColumn;
        private System.Windows.Forms.Label CallstackLabel;
        private System.Windows.Forms.Panel ConsolePanel;
        private System.Windows.Forms.Label label3;
        private System.ComponentModel.BackgroundWorker DebuggerWorker;
        private System.Windows.Forms.TabControl Tabs;
        private System.Windows.Forms.TabPage MemoryPanel;
        private System.Windows.Forms.TabPage CodePanel;
        private System.Windows.Forms.Panel RegistersPanel;
        private System.Windows.Forms.ListView RegisterList;
        private System.Windows.Forms.ColumnHeader RegisterNameColumn;
        private System.Windows.Forms.ColumnHeader RegisterValueColumn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolStripButton StartVMButton;
        private System.Windows.Forms.Panel VariablesPanel;
        private System.Windows.Forms.ListView VariableList;
        private System.Windows.Forms.ColumnHeader VariableNameColumn;
        private System.Windows.Forms.ColumnHeader VariableTypeColumn;
        private System.Windows.Forms.ColumnHeader VariableValueColumn;
        private System.Windows.Forms.Label VariablesLabel;
        private System.Windows.Forms.ColumnHeader VariableDataColumn;
        private System.Windows.Forms.Panel MemoryViewPanel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RichTextBox MemoryBox;
        private System.Windows.Forms.Label MemoryLabel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton StepLineButton;
        private System.Windows.Forms.ToolStripButton StepOverButton;
    }
}