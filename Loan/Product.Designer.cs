﻿namespace kCredit
{
    partial class frmProduct
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnNew = new System.Windows.Forms.ToolStripButton();
            this.btnCopy = new System.Windows.Forms.ToolStripButton();
            this.btnUnlock = new System.Windows.Forms.ToolStripButton();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.btnSaveNew = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.btnActive = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnMode = new System.Windows.Forms.ToolStripButton();
            this.btnExport = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lblSearch = new System.Windows.Forms.Label();
            this.btnFind = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnFilter = new System.Windows.Forms.Button();
            this.lblName = new System.Windows.Forms.Label();
            this.lblCode = new System.Windows.Forms.Label();
            this.chkHoliday = new System.Windows.Forms.CheckBox();
            this.chkSunday = new System.Windows.Forms.CheckBox();
            this.chkSaturday = new System.Windows.Forms.CheckBox();
            this.txtNote = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblOnNonWorkingDay = new System.Windows.Forms.Label();
            this.mnuShow = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuShowA = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowI = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.dgvList = new kUI.DataGridView();
            this.txtFind = new kUI.TextBox(this.components);
            this.txtCode = new kUI.TextBox(this.components);
            this.txtName = new kUI.TextBox(this.components);
            this.glbGeneral = new kUI.GroupLabel();
            this.groupLabel1 = new kUI.GroupLabel();
            this.groupLabel2 = new kUI.GroupLabel();
            this.glbNote = new kUI.GroupLabel();
            this.cboTotalRound = new kUI.ComboBox(this.components);
            this.cboInterestRound = new kUI.ComboBox(this.components);
            this.cboPrincipalRound = new kUI.ComboBox(this.components);
            this.cboMethod = new kUI.ComboBox(this.components);
            this.cboMove = new kUI.ComboBox(this.components);
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.mnuShow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.White;
            this.toolStrip1.Font = new System.Drawing.Font("Noto Sans Khmer", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNew,
            this.btnCopy,
            this.btnUnlock,
            this.btnSave,
            this.btnSaveNew,
            this.toolStripSeparator,
            this.btnActive,
            this.btnDelete,
            this.toolStripSeparator1,
            this.btnMode,
            this.btnExport});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(776, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnNew
            // 
            this.btnNew.Image = global::kCredit.Properties.Resources.New;
            this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(55, 22);
            this.btnNew.Text = "&New";
            this.btnNew.ToolTipText = "New (Ctrl+N)";
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Enabled = false;
            this.btnCopy.Image = global::kCredit.Properties.Resources.Copy;
            this.btnCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(60, 22);
            this.btnCopy.Text = "Cop&y";
            this.btnCopy.ToolTipText = "Copy (Ctrl+Y)";
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnUnlock
            // 
            this.btnUnlock.Enabled = false;
            this.btnUnlock.Image = global::kCredit.Properties.Resources.Unlock;
            this.btnUnlock.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUnlock.Name = "btnUnlock";
            this.btnUnlock.Size = new System.Drawing.Size(70, 22);
            this.btnUnlock.Text = "Unl&ock";
            this.btnUnlock.ToolTipText = "Unlock (Ctrl+L)";
            this.btnUnlock.Click += new System.EventHandler(this.btnUnlock_Click);
            // 
            // btnSave
            // 
            this.btnSave.Enabled = false;
            this.btnSave.Image = global::kCredit.Properties.Resources.Save;
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(60, 22);
            this.btnSave.Text = "&Save";
            this.btnSave.ToolTipText = "Save (Ctrl+S)";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnSaveNew
            // 
            this.btnSaveNew.Enabled = false;
            this.btnSaveNew.Image = global::kCredit.Properties.Resources.SaveNew;
            this.btnSaveNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSaveNew.Name = "btnSaveNew";
            this.btnSaveNew.Size = new System.Drawing.Size(116, 22);
            this.btnSaveNew.Text = "Save and Ne&w";
            this.btnSaveNew.ToolTipText = "Save and New (Ctrl+W)";
            this.btnSaveNew.Click += new System.EventHandler(this.btnSaveNew_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // btnActive
            // 
            this.btnActive.Enabled = false;
            this.btnActive.Image = global::kCredit.Properties.Resources.Inactive;
            this.btnActive.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnActive.Name = "btnActive";
            this.btnActive.Size = new System.Drawing.Size(74, 22);
            this.btnActive.Text = "Inactiv&e";
            this.btnActive.ToolTipText = "Active/Inactive (Ctrl+E)";
            this.btnActive.Click += new System.EventHandler(this.btnActive_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Enabled = false;
            this.btnDelete.Image = global::kCredit.Properties.Resources.Recyclebin;
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(68, 22);
            this.btnDelete.Text = "Delete";
            this.btnDelete.ToolTipText = "Delete (Del)";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnMode
            // 
            this.btnMode.Image = global::kCredit.Properties.Resources.Expand;
            this.btnMode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMode.Name = "btnMode";
            this.btnMode.Size = new System.Drawing.Size(88, 22);
            this.btnMode.Text = "List/Detail";
            this.btnMode.ToolTipText = "Toggle between list and detail mode (F9)";
            this.btnMode.Click += new System.EventHandler(this.btnMode_Click);
            // 
            // btnExport
            // 
            this.btnExport.Image = global::kCredit.Properties.Resources.Export;
            this.btnExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(66, 22);
            this.btnExport.Text = "Export";
            this.btnExport.ToolTipText = "Export all data to CSV file (F12)";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lblSearch);
            this.splitContainer1.Panel1.Controls.Add(this.btnFind);
            this.splitContainer1.Panel1.Controls.Add(this.btnClear);
            this.splitContainer1.Panel1.Controls.Add(this.btnFilter);
            this.splitContainer1.Panel1.Controls.Add(this.dgvList);
            this.splitContainer1.Panel1.Controls.Add(this.txtFind);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.txtCode);
            this.splitContainer1.Panel2.Controls.Add(this.lblName);
            this.splitContainer1.Panel2.Controls.Add(this.txtName);
            this.splitContainer1.Panel2.Controls.Add(this.lblCode);
            this.splitContainer1.Panel2.Controls.Add(this.chkHoliday);
            this.splitContainer1.Panel2.Controls.Add(this.glbGeneral);
            this.splitContainer1.Panel2.Controls.Add(this.chkSunday);
            this.splitContainer1.Panel2.Controls.Add(this.chkSaturday);
            this.splitContainer1.Panel2.Controls.Add(this.groupLabel1);
            this.splitContainer1.Panel2.Controls.Add(this.groupLabel2);
            this.splitContainer1.Panel2.Controls.Add(this.glbNote);
            this.splitContainer1.Panel2.Controls.Add(this.txtNote);
            this.splitContainer1.Panel2.Controls.Add(this.cboTotalRound);
            this.splitContainer1.Panel2.Controls.Add(this.cboInterestRound);
            this.splitContainer1.Panel2.Controls.Add(this.cboPrincipalRound);
            this.splitContainer1.Panel2.Controls.Add(this.cboMethod);
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.label17);
            this.splitContainer1.Panel2.Controls.Add(this.cboMove);
            this.splitContainer1.Panel2.Controls.Add(this.label4);
            this.splitContainer1.Panel2.Controls.Add(this.lblOnNonWorkingDay);
            this.splitContainer1.Panel2.Font = new System.Drawing.Font("Noto Sans Khmer", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(8, 4, 8, 8);
            this.splitContainer1.Panel2MinSize = 0;
            this.splitContainer1.Size = new System.Drawing.Size(776, 502);
            this.splitContainer1.SplitterDistance = 228;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 1;
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.lblSearch.Font = new System.Drawing.Font("Noto Sans Khmer", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearch.ForeColor = System.Drawing.Color.DimGray;
            this.lblSearch.Location = new System.Drawing.Point(6, 5);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(51, 17);
            this.lblSearch.TabIndex = 6;
            this.lblSearch.Text = "Search";
            this.lblSearch.Click += new System.EventHandler(this.lblSearch_Click);
            // 
            // btnFind
            // 
            this.btnFind.Image = global::kCredit.Properties.Resources.Search;
            this.btnFind.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFind.Location = new System.Drawing.Point(1, 27);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(68, 23);
            this.btnFind.TabIndex = 5;
            this.btnFind.Text = "     Find";
            this.btnFind.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // btnClear
            // 
            this.btnClear.Image = global::kCredit.Properties.Resources.Brush;
            this.btnClear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClear.Location = new System.Drawing.Point(69, 27);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(68, 23);
            this.btnClear.TabIndex = 5;
            this.btnClear.Text = "     Clear";
            this.btnClear.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnFilter
            // 
            this.btnFilter.Image = global::kCredit.Properties.Resources.Filter;
            this.btnFilter.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFilter.Location = new System.Drawing.Point(137, 27);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(68, 23);
            this.btnFilter.TabIndex = 5;
            this.btnFilter.Text = "     Filter";
            this.btnFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFilter.UseVisualStyleBackColor = true;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // lblName
            // 
            this.lblName.Font = new System.Drawing.Font("Noto Sans Khmer", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.Location = new System.Drawing.Point(12, 76);
            this.lblName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(136, 17);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "Name";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCode
            // 
            this.lblCode.Font = new System.Drawing.Font("Noto Sans Khmer", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCode.Location = new System.Drawing.Point(12, 43);
            this.lblCode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(136, 17);
            this.lblCode.TabIndex = 0;
            this.lblCode.Text = "Code";
            this.lblCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkHoliday
            // 
            this.chkHoliday.AutoSize = true;
            this.chkHoliday.Checked = true;
            this.chkHoliday.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHoliday.Location = new System.Drawing.Point(155, 326);
            this.chkHoliday.Name = "chkHoliday";
            this.chkHoliday.Size = new System.Drawing.Size(74, 21);
            this.chkHoliday.TabIndex = 15;
            this.chkHoliday.Text = "Holiday";
            this.chkHoliday.UseVisualStyleBackColor = true;
            this.chkHoliday.CheckedChanged += new System.EventHandler(this.chkNeverOn_CheckedChanged);
            // 
            // chkSunday
            // 
            this.chkSunday.AutoSize = true;
            this.chkSunday.Checked = true;
            this.chkSunday.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSunday.Location = new System.Drawing.Point(242, 299);
            this.chkSunday.Name = "chkSunday";
            this.chkSunday.Size = new System.Drawing.Size(73, 21);
            this.chkSunday.TabIndex = 14;
            this.chkSunday.Text = "Sunday";
            this.chkSunday.UseVisualStyleBackColor = true;
            this.chkSunday.CheckedChanged += new System.EventHandler(this.chkNeverOn_CheckedChanged);
            // 
            // chkSaturday
            // 
            this.chkSaturday.AutoSize = true;
            this.chkSaturday.Checked = true;
            this.chkSaturday.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSaturday.Location = new System.Drawing.Point(155, 299);
            this.chkSaturday.Name = "chkSaturday";
            this.chkSaturday.Size = new System.Drawing.Size(81, 21);
            this.chkSaturday.TabIndex = 13;
            this.chkSaturday.Text = "Saturday";
            this.chkSaturday.UseVisualStyleBackColor = true;
            this.chkSaturday.CheckedChanged += new System.EventHandler(this.chkNeverOn_CheckedChanged);
            // 
            // txtNote
            // 
            this.txtNote.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNote.Location = new System.Drawing.Point(12, 422);
            this.txtNote.Margin = new System.Windows.Forms.Padding(4);
            this.txtNote.Multiline = true;
            this.txtNote.Name = "txtNote";
            this.txtNote.ReadOnly = true;
            this.txtNote.Size = new System.Drawing.Size(513, 66);
            this.txtNote.TabIndex = 18;
            this.txtNote.TextChanged += new System.EventHandler(this.Dirty_TextChanged);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Noto Sans Khmer", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 238);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(136, 17);
            this.label3.TabIndex = 10;
            this.label3.Text = "Total";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Noto Sans Khmer", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 207);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 17);
            this.label2.TabIndex = 8;
            this.label2.Text = "Interest";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Noto Sans Khmer", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 176);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "Principal";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label17
            // 
            this.label17.Font = new System.Drawing.Font("Noto Sans Khmer", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(12, 107);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(136, 17);
            this.label17.TabIndex = 4;
            this.label17.Text = "Calculation method";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Noto Sans Khmer", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(39, 300);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(109, 17);
            this.label4.TabIndex = 12;
            this.label4.Text = "Never on";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblOnNonWorkingDay
            // 
            this.lblOnNonWorkingDay.Font = new System.Drawing.Font("Noto Sans Khmer", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOnNonWorkingDay.Location = new System.Drawing.Point(9, 356);
            this.lblOnNonWorkingDay.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOnNonWorkingDay.Name = "lblOnNonWorkingDay";
            this.lblOnNonWorkingDay.Size = new System.Drawing.Size(139, 17);
            this.lblOnNonWorkingDay.TabIndex = 16;
            this.lblOnNonWorkingDay.Text = "On non-working day";
            this.lblOnNonWorkingDay.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // mnuShow
            // 
            this.mnuShow.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuShowA,
            this.mnuShowI});
            this.mnuShow.Name = "contextMenuStrip1";
            this.mnuShow.Size = new System.Drawing.Size(148, 48);
            // 
            // mnuShowA
            // 
            this.mnuShowA.Checked = true;
            this.mnuShowA.CheckOnClick = true;
            this.mnuShowA.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mnuShowA.Name = "mnuShowA";
            this.mnuShowA.Size = new System.Drawing.Size(147, 22);
            this.mnuShowA.Text = "Show Active";
            this.mnuShowA.CheckedChanged += new System.EventHandler(this.mnuShow_CheckedChanged);
            // 
            // mnuShowI
            // 
            this.mnuShowI.CheckOnClick = true;
            this.mnuShowI.Name = "mnuShowI";
            this.mnuShowI.Size = new System.Drawing.Size(147, 22);
            this.mnuShowI.Text = "Show Inactive";
            this.mnuShowI.CheckedChanged += new System.EventHandler(this.mnuShow_CheckedChanged);
            // 
            // dgvList
            // 
            this.dgvList.AllowUserToAddRows = false;
            this.dgvList.AllowUserToDeleteRows = false;
            this.dgvList.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Honeydew;
            this.dgvList.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvList.BackgroundColor = System.Drawing.Color.White;
            this.dgvList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Noto Sans Khmer", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.colCode,
            this.colName,
            this.Column2,
            this.Column3,
            this.Column8,
            this.Column4});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Noto Sans Khmer", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.Teal;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvList.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgvList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvList.Location = new System.Drawing.Point(1, 51);
            this.dgvList.Margin = new System.Windows.Forms.Padding(4);
            this.dgvList.MultiSelect = false;
            this.dgvList.Name = "dgvList";
            this.dgvList.ReadOnly = true;
            this.dgvList.RowHeadersVisible = false;
            this.dgvList.RowHeadersWidth = 35;
            this.dgvList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvList.ShowEditingIcon = false;
            this.dgvList.Size = new System.Drawing.Size(223, 447);
            this.dgvList.TabIndex = 1;
            this.dgvList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvList_CellDoubleClick);
            this.dgvList.SelectionChanged += new System.EventHandler(this.dgvList_SelectionChanged);
            this.dgvList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvList_KeyDown);
            // 
            // txtFind
            // 
            this.txtFind.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFind.Font = new System.Drawing.Font("Noto Sans Khmer", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFind.Format = null;
            this.txtFind.Location = new System.Drawing.Point(1, 1);
            this.txtFind.Margin = new System.Windows.Forms.Padding(4);
            this.txtFind.Name = "txtFind";
            this.txtFind.Numeric = false;
            this.txtFind.Size = new System.Drawing.Size(224, 25);
            this.txtFind.TabIndex = 4;
            this.txtFind.TabOnEnter = false;
            this.toolTip1.SetToolTip(this.txtFind, "Enter query to search for");
            this.txtFind.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            this.txtFind.Enter += new System.EventHandler(this.txtFind_Enter);
            this.txtFind.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            this.txtFind.Leave += new System.EventHandler(this.txtFind_Leave);
            // 
            // txtCode
            // 
            this.txtCode.Font = new System.Drawing.Font("Noto Sans Khmer", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCode.Format = null;
            this.txtCode.Location = new System.Drawing.Point(156, 40);
            this.txtCode.Margin = new System.Windows.Forms.Padding(4);
            this.txtCode.Name = "txtCode";
            this.txtCode.Numeric = false;
            this.txtCode.ReadOnly = true;
            this.txtCode.Size = new System.Drawing.Size(230, 25);
            this.txtCode.TabIndex = 1;
            this.txtCode.TabOnEnter = true;
            this.txtCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("Noto Sans Khmer", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtName.Format = null;
            this.txtName.Location = new System.Drawing.Point(156, 73);
            this.txtName.Margin = new System.Windows.Forms.Padding(4);
            this.txtName.Name = "txtName";
            this.txtName.Numeric = false;
            this.txtName.ReadOnly = true;
            this.txtName.Size = new System.Drawing.Size(230, 25);
            this.txtName.TabIndex = 3;
            this.txtName.TabOnEnter = true;
            this.txtName.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip1.SetToolTip(this.txtName, "Default conversion factor");
            // 
            // glbGeneral
            // 
            this.glbGeneral.Caption = "General";
            this.glbGeneral.Font = new System.Drawing.Font("Noto Sans Khmer", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.glbGeneral.Location = new System.Drawing.Point(12, 11);
            this.glbGeneral.Margin = new System.Windows.Forms.Padding(4, 12, 4, 4);
            this.glbGeneral.Name = "glbGeneral";
            this.glbGeneral.Size = new System.Drawing.Size(373, 21);
            this.glbGeneral.TabIndex = 0;
            this.glbGeneral.TabStop = false;
            // 
            // groupLabel1
            // 
            this.groupLabel1.Caption = "Rounding Rule";
            this.groupLabel1.Font = new System.Drawing.Font("Noto Sans Khmer", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupLabel1.Location = new System.Drawing.Point(12, 145);
            this.groupLabel1.Margin = new System.Windows.Forms.Padding(4, 12, 4, 4);
            this.groupLabel1.Name = "groupLabel1";
            this.groupLabel1.Size = new System.Drawing.Size(373, 21);
            this.groupLabel1.TabIndex = 0;
            this.groupLabel1.TabStop = false;
            // 
            // groupLabel2
            // 
            this.groupLabel2.Caption = "Installment Date";
            this.groupLabel2.Font = new System.Drawing.Font("Noto Sans Khmer", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupLabel2.Location = new System.Drawing.Point(12, 275);
            this.groupLabel2.Margin = new System.Windows.Forms.Padding(4, 12, 4, 4);
            this.groupLabel2.Name = "groupLabel2";
            this.groupLabel2.Size = new System.Drawing.Size(373, 21);
            this.groupLabel2.TabIndex = 0;
            this.groupLabel2.TabStop = false;
            // 
            // glbNote
            // 
            this.glbNote.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.glbNote.Caption = "Note";
            this.glbNote.Font = new System.Drawing.Font("Noto Sans Khmer", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.glbNote.Location = new System.Drawing.Point(12, 393);
            this.glbNote.Margin = new System.Windows.Forms.Padding(4, 12, 4, 4);
            this.glbNote.Name = "glbNote";
            this.glbNote.Size = new System.Drawing.Size(497, 21);
            this.glbNote.TabIndex = 28;
            this.glbNote.TabStop = false;
            // 
            // cboTotalRound
            // 
            this.cboTotalRound.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTotalRound.Enabled = false;
            this.cboTotalRound.FormattingEnabled = true;
            this.cboTotalRound.Location = new System.Drawing.Point(155, 235);
            this.cboTotalRound.Name = "cboTotalRound";
            this.cboTotalRound.Size = new System.Drawing.Size(230, 25);
            this.cboTotalRound.TabIndex = 11;
            this.cboTotalRound.TabOnEnter = true;
            this.cboTotalRound.Value = "";
            this.cboTotalRound.TextChanged += new System.EventHandler(this.Dirty_TextChanged);
            // 
            // cboInterestRound
            // 
            this.cboInterestRound.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboInterestRound.Enabled = false;
            this.cboInterestRound.FormattingEnabled = true;
            this.cboInterestRound.Location = new System.Drawing.Point(155, 204);
            this.cboInterestRound.Name = "cboInterestRound";
            this.cboInterestRound.Size = new System.Drawing.Size(230, 25);
            this.cboInterestRound.TabIndex = 9;
            this.cboInterestRound.TabOnEnter = true;
            this.cboInterestRound.Value = "";
            this.cboInterestRound.TextChanged += new System.EventHandler(this.Dirty_TextChanged);
            // 
            // cboPrincipalRound
            // 
            this.cboPrincipalRound.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPrincipalRound.Enabled = false;
            this.cboPrincipalRound.FormattingEnabled = true;
            this.cboPrincipalRound.Location = new System.Drawing.Point(155, 173);
            this.cboPrincipalRound.Name = "cboPrincipalRound";
            this.cboPrincipalRound.Size = new System.Drawing.Size(230, 25);
            this.cboPrincipalRound.TabIndex = 7;
            this.cboPrincipalRound.TabOnEnter = true;
            this.cboPrincipalRound.Value = "";
            this.cboPrincipalRound.TextChanged += new System.EventHandler(this.Dirty_TextChanged);
            // 
            // cboMethod
            // 
            this.cboMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMethod.Enabled = false;
            this.cboMethod.FormattingEnabled = true;
            this.cboMethod.Location = new System.Drawing.Point(155, 105);
            this.cboMethod.Name = "cboMethod";
            this.cboMethod.Size = new System.Drawing.Size(230, 25);
            this.cboMethod.TabIndex = 5;
            this.cboMethod.TabOnEnter = true;
            this.cboMethod.Value = "";
            this.cboMethod.TextChanged += new System.EventHandler(this.Dirty_TextChanged);
            // 
            // cboMove
            // 
            this.cboMove.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMove.Enabled = false;
            this.cboMove.FormattingEnabled = true;
            this.cboMove.Location = new System.Drawing.Point(155, 353);
            this.cboMove.Name = "cboMove";
            this.cboMove.Size = new System.Drawing.Size(230, 25);
            this.cboMove.TabIndex = 17;
            this.cboMove.TabOnEnter = true;
            this.cboMove.Value = "";
            this.cboMove.SelectedIndexChanged += new System.EventHandler(this.cboFrequencyUnit_SelectedIndexChanged);
            this.cboMove.TextChanged += new System.EventHandler(this.Dirty_TextChanged);
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "Id";
            this.Column1.HeaderText = "Id";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Visible = false;
            // 
            // colCode
            // 
            this.colCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colCode.DataPropertyName = "code";
            this.colCode.HeaderText = "Code";
            this.colCode.Name = "colCode";
            this.colCode.ReadOnly = true;
            this.colCode.Width = 66;
            // 
            // colName
            // 
            this.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colName.DataPropertyName = "name";
            dataGridViewCellStyle3.NullValue = null;
            this.colName.DefaultCellStyle = dataGridViewCellStyle3;
            this.colName.HeaderText = "Name";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            this.colName.Width = 70;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column2.DataPropertyName = "calculation_method";
            dataGridViewCellStyle4.NullValue = null;
            this.Column2.DefaultCellStyle = dataGridViewCellStyle4;
            this.Column2.HeaderText = "Calculation Method";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 147;
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column3.DataPropertyName = "principal_round_rule";
            this.Column3.HeaderText = "Priciple";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 78;
            // 
            // Column8
            // 
            this.Column8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column8.DataPropertyName = "interest_round_rule";
            this.Column8.HeaderText = "Interest";
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            this.Column8.Width = 76;
            // 
            // Column4
            // 
            this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column4.DataPropertyName = "total_round_rule";
            this.Column4.HeaderText = "Total";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // frmProduct
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(776, 527);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("Noto Sans Khmer", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmProduct";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Product";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmProductList_FormClosing);
            this.Load += new System.EventHandler(this.frmProductList_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.mnuShow.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnNew;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.ToolStripButton btnCopy;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripButton btnSaveNew;
        private System.Windows.Forms.TextBox txtNote;
        private System.Windows.Forms.ContextMenuStrip mnuShow;
        private System.Windows.Forms.ToolStripButton btnActive;
        private System.Windows.Forms.ToolStripButton btnUnlock;
        private System.Windows.Forms.ToolStripMenuItem mnuShowA;
        private System.Windows.Forms.ToolStripMenuItem mnuShowI;
        private kUI.DataGridView dgvList;
        private kUI.GroupLabel glbGeneral;
        private kUI.GroupLabel glbNote;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripButton btnMode;
        private System.Windows.Forms.ToolStripButton btnExport;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private kUI.TextBox txtFind;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.CheckBox chkHoliday;
        private System.Windows.Forms.CheckBox chkSunday;
        private System.Windows.Forms.CheckBox chkSaturday;
        private System.Windows.Forms.Label lblOnNonWorkingDay;
        private System.Windows.Forms.Label label4;
        private kUI.GroupLabel groupLabel2;
        private kUI.ComboBox cboMove;
        private System.Windows.Forms.Label label17;
        private kUI.ComboBox cboMethod;
        private kUI.GroupLabel groupLabel1;
        private kUI.ComboBox cboPrincipalRound;
        private kUI.ComboBox cboTotalRound;
        private kUI.ComboBox cboInterestRound;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private kUI.TextBox txtName;
        private System.Windows.Forms.Label lblCode;
        private kUI.TextBox txtCode;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
    }
}

