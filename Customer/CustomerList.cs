﻿using System;
using System.Windows.Forms;
using kCredit.SM;
using kCredit.SYS;
using System.Text;
using System.Drawing;

namespace kCredit
{
    public partial class frmCustomer : Form
    {
        long Id = 0;
        int RowIndex = 0;   // Current gird selected row
        bool IsExpand = false;
        bool IsDirty = false;
        bool IsIgnore = true;

        public bool IsDlg = false; // Show dialog box for selecting one 
        public string CustomerNo = ""; // Selected customer no
        public string FullName = "";    // Customer Full name

        frmMsg fMsg = null;

        StringFormat headerCellFormat = new StringFormat()
        {
            // right alignment might actually make more sense for numbers
            Alignment = StringAlignment.Near,
            LineAlignment = StringAlignment.Center
        };

        public frmCustomer()
        {
            InitializeComponent();
        }

        private string GetStatus()
        {
            var status = "";
            if (mnuShowA.Checked && !mnuShowI.Checked)
                status = Constant.RecordStatus_Active;
            else if (mnuShowI.Checked && !mnuShowA.Checked)
                status = Constant.RecordStatus_InActive;
            return status;
        }

        private void RefreshGrid(long seq = 0)
        {
            Cursor = Cursors.WaitCursor;
            //IsIgnore = true;
            if (dgvList.SelectedRows.Count > 0) RowIndex = dgvList.SelectedRows[0].Index;
            try
            {
                dgvList.DataSource = CustomerFacade.GetDataTable(txtFind.Text, GetStatus());
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageFacade.Show(MessageFacade.error_retrieve_data + "\r\n" + ex.Message, LabelFacade.sy_customer, MessageBoxButtons.OK, MessageBoxIcon.Error);
                ErrorLogFacade.Log(ex);
                return;
            }
            if (dgvList.RowCount > 0)
            {
                if (seq == 0)
                {
                    if (RowIndex >= dgvList.RowCount) RowIndex = dgvList.RowCount - 1;
                    dgvList.CurrentCell = dgvList[1, RowIndex];
                }
                else
                    foreach (DataGridViewRow row in dgvList.Rows)
                        if ((long)row.Cells[0].Value == seq)
                        {
                            Id = (int)seq;
                            dgvList.CurrentCell = dgvList[1, row.Index];
                            break;
                        }
            }
            else
            {
                btnCopy.Enabled = false;
                btnUnlock.Enabled = false;
                btnActive.Enabled = false;
                btnDelete.Enabled = false;
                ClearAllBoxes();
            }
            IsIgnore = false;
            Cursor = Cursors.Default;
        }

        private void LockControls(bool l = true)
        {
            //if (Id != 0 && l == false)
            //    txtNo.ReadOnly = true;
            //else
            //    txtNo.ReadOnly = l;
            txtFirstName.ReadOnly = l;
            txtLastName.ReadOnly = l;
            cboBranch.Enabled = !l;
            cboType.Enabled = !l;
            cboCategory.Enabled = !l;
            cboGender.Enabled = !l;
            dtpDOB.Enabled = !l;
            cboProvince.Enabled = !l;
            cboDistrict.Enabled = !l;
            cboCommune.Enabled = !l;
            cboVillage.Enabled = !l;
            txtAddress.ReadOnly = l;
            cboIDType1.Enabled = !l;
            txtIDValue1.ReadOnly = l;
            cboIDType2.Enabled = !l;
            txtIDValue2.ReadOnly = l;
            cboIDType3.Enabled = !l;
            txtIDValue3.ReadOnly = l;
            cboContactType1.Enabled = !l;
            txtContactValue1.ReadOnly = l;
            cboContactType2.Enabled = !l;
            txtContactValue2.ReadOnly = l;
            cboContactType3.Enabled = !l;
            txtContactValue3.ReadOnly = l;
            cboContactType4.Enabled = !l;
            txtContactValue4.ReadOnly = l;
            txtNote.ReadOnly = l;

            btnNew.Enabled = l;
            btnCopy.Enabled = dgvList.Id != 0 && l;
            btnSave.Enabled = !l;
            btnSaveNew.Enabled = !l;
            btnActive.Enabled = dgvList.Id != 0 && l;
            btnDelete.Enabled = dgvList.Id != 0 && l;
            splitContainer1.Panel1.Enabled = l;
            btnUnlock.Enabled = !l || dgvList.RowCount > 0;
            btnUnlock.Text = l ? LabelFacade.sy_button_unlock : LabelFacade.sy_button_cancel;
            txtFind.ReadOnly = !l;
            btnFind.Enabled = l;
            btnClear.Enabled = l;
            btnFilter.Enabled = l;
            if (fMsg != null && !fMsg.IsDisposed) fMsg.Close();
        }

        private void SetStatus(string stat)
        {
            if (stat == Constant.RecordStatus_Active)
            {
                if (btnActive.Text == LabelFacade.sy_button_inactive) return;
                btnActive.Text = LabelFacade.sy_button_inactive;
                if (btnActive.Image != Properties.Resources.Inactive)
                    btnActive.Image = Properties.Resources.Inactive;
            }
            else
            {
                if (btnActive.Text == LabelFacade.sy_button_active) return;
                btnActive.Text = LabelFacade.sy_button_active;
                if (btnActive.Image != Properties.Resources.Active)
                    btnActive.Image = Properties.Resources.Active;
            }
        }

        private bool IsValidated()
        {
            var sMsg = new StringBuilder();
            Control cFocus = null;
            string No = txtCustomerNo.Text.Trim();
            if (No.Length == 0)
            {
                sMsg.AppendLine(LabelFacade.sy_msg_prefix + MessageFacade.code_not_empty);
                cFocus = txtCustomerNo;
            }
            else if (CustomerFacade.Exists(No, Id))
            {
                sMsg.AppendLine(LabelFacade.sy_msg_prefix + MessageFacade.code_already_exists);
                cFocus = txtCustomerNo;
            }
            if (txtFirstName.Text.Trim().Length == 0)
            {
                sMsg.AppendLine(LabelFacade.sy_msg_prefix + MessageFacade.code_already_exists);
                cFocus = txtFirstName;
            }
            if (sMsg.Length > 0)
            {
                MessageFacade.Show(this, ref fMsg, sMsg.ToString(), LabelFacade.sy_save);
                cFocus.Focus();
                return false;
            }
            return true;
        }

        private void ClearAllBoxes()
        {
            txtCustomerNo.Text = "";
            txtCustomerNo.Focus();
            txtLastName.Text = "";
            txtFirstName.Text = "";
            txtAddress.Text = "";
            txtNote.Text = "";
            IsDirty = false;
        }

        private void LoadData()
        {
            var Id = dgvList.Id;
            if (Id != 0)
                try
                {
                    var m = CustomerFacade.Select(Id);
                    txtCustomerNo.Text = m.Customer_No;
                    txtFirstName.Text = m.First_Name;
                    txtLastName.Text = m.Last_Name;
                    cboGender.SelectedValue = m.Gender;
                    dtpDOB.Value = m.Date_of_Birth;
                    cboType.SelectedValue = m.Type;
                    cboCategory.SelectedValue = m.Category;
                    cboBranch.SelectedValue = m.Branch_Code;
                    cboIDType1.SelectedValue = m.Id_Type1;
                    txtIDValue1.Text = m.Id_Value1;
                    cboIDType2.SelectedValue = m.Id_Type2;
                    txtIDValue2.Text = m.Id_Value2;
                    cboIDType3.SelectedValue = m.Id_Type3;
                    txtIDValue3.Text = m.Id_Value3;
                    cboContactType1.SelectedValue = m.Contact_Type1;
                    txtContactValue1.Text = m.Contact_Value1;
                    cboContactType2.SelectedValue = m.Contact_Type2;
                    txtContactValue2.Text = m.Contact_Value2;
                    cboContactType3.SelectedValue = m.Contact_Type3;
                    txtContactValue3.Text = m.Contact_Value3;
                    cboContactType4.SelectedValue = m.Contact_Type4;
                    txtContactValue4.Text = m.Contact_Value4;
                    txtAddress.Text = m.Address;
                    cboProvince.SelectedValue = m.Province;
                    cboDistrict.SelectedValue = m.District;
                    cboCommune.SelectedValue = m.Commune;
                    cboVillage.SelectedValue = m.Village;
                    txtNote.Text = m.Note;
                    SetStatus(m.Status);
                    LockControls();
                    IsDirty = false;
                    SessionLogFacade.Log(Constant.Priority_Information, Constant.Module_Branch, Constant.Log_View, "View. Id=" + m.Id + ", No=" + m.Customer_No);
                }
                catch (Exception ex)
                {
                    MessageFacade.Show(MessageFacade.error_load_record + "\r\n" + ex.Message, LabelFacade.sy_customer, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ErrorLogFacade.Log(ex);
                }
            else    // when grid is empty => disable buttons and clear all controls
            {
                if (dgvList.RowCount == 0)
                {
                    btnUnlock.Enabled = false;
                    ClearAllBoxes();
                }
            }
        }

        private void SetIconDisplayType(string type)
        {
            ToolStripItemDisplayStyle ds;
            switch (type)
            {
                case "I":
                    ds = ToolStripItemDisplayStyle.Image;
                    break;
                case "T":
                    ds = ToolStripItemDisplayStyle.Text;
                    break;
                default:
                    ds = ToolStripItemDisplayStyle.ImageAndText;
                    break;
            }
            if (ds == ToolStripItemDisplayStyle.ImageAndText) return;   // If IT=ImageAndText, then do nothing (the designer already take care this)
            foreach (var c in toolStrip1.Items)
            {
                if (c is ToolStripButton)
                    ((ToolStripButton)c).DisplayStyle = ds;
            }
        }

        private void SetCodeCasing()
        {
            CharacterCasing cs;
            switch (ConfigFacade.sy_code_casing)
            {
                case "U":
                    cs = CharacterCasing.Upper;
                    break;
                case "L":
                    cs = CharacterCasing.Lower;
                    break;
                default:
                    cs = CharacterCasing.Normal;
                    break;
            }
            txtCustomerNo.CharacterCasing = cs;
        }

        private void SetSettings()
        {
            try
            {
                SetIconDisplayType(ConfigFacade.sy_toolbar_icon_display_type);
                splitContainer1.SplitterDistance = ConfigFacade.ic_unit_measure_splitter_distance;

                SetCodeCasing();
                txtCustomerNo.MaxLength = ConfigFacade.sy_code_max_length;
                Util.SetFormState(this);
            }
            catch (Exception ex)
            {
                ErrorLogFacade.Log(ex, "Set settings");
            }
        }

        private void SetLabels()
        {
            var prefix = "customer_";
            btnNew.Text = LabelFacade.sy_button_new ?? btnNew.Text;
            btnCopy.Text = LabelFacade.sy_button_copy ?? btnCopy.Text;
            btnUnlock.Text = LabelFacade.sy_button_unlock ?? btnUnlock.Text;
            btnSave.Text = LabelFacade.sy_button_save ?? btnSave.Text;
            btnSaveNew.Text = LabelFacade.sy_button_save_new ?? btnSaveNew.Text;
            btnActive.Text = LabelFacade.sy_button_inactive ?? btnActive.Text;
            btnDelete.Text = LabelFacade.sy_button_delete ?? btnDelete.Text;
            btnMode.Text = LabelFacade.sy_button_mode ?? btnMode.Text;
            btnExport.Text = LabelFacade.sy_export ?? btnExport.Text;
            lblSearch.Text = LabelFacade.sy_search_place_holder ?? lblSearch.Text;
            btnFind.Text = "     " + (LabelFacade.sy_button_find ?? btnFind.Text.Replace(" ", ""));
            btnClear.Text = "     " + (LabelFacade.sy_button_clear ?? btnClear.Text.Replace(" ", ""));
            btnFilter.Text = "     " + (LabelFacade.sy_button_filter ?? btnFilter.Text.Replace(" ", ""));

            colCustomerNo.HeaderText = LabelFacade.GetLabel(prefix + "code") ?? colCustomerNo.HeaderText;
            lblCode.Text = colCustomerNo.HeaderText;
            lblName.Text = LabelFacade.GetLabel(prefix + "default_factor") ?? lblName.Text;
            glbGeneral.Caption = LabelFacade.GetLabel(prefix + "general") ?? glbGeneral.Caption;
            glbNote.Caption = LabelFacade.GetLabel(prefix + "note") ?? glbNote.Caption;
            //todo: load the rest
        }

        private bool Save()
        {
            if (!IsValidated()) return false;
            Cursor = Cursors.WaitCursor;
            var m = new Customer();
            var log = new SessionLog { Module = Constant.Module_Branch };
            m.Id = Id;
            m.Customer_No = txtCustomerNo.Text.Trim();
            m.First_Name = txtFirstName.Text;
            m.Last_Name = txtLastName.Text;
            m.Gender = cboGender.SelectedValue.ToString();
            m.Date_of_Birth = dtpDOB.Value;
            m.Type = cboType.SelectedValue.ToString();
            m.Category = cboCategory.SelectedValue.ToString();
            m.Branch_Code = cboBranch.SelectedValue.ToString();
            m.Id_Type1 = cboIDType1.SelectedValue.ToString();
            m.Id_Value1 = txtIDValue1.Text;
            m.Id_Type2 = cboIDType2.SelectedValue.ToString();
            m.Id_Value2 = txtIDValue2.Text;
            m.Id_Type3 = cboIDType3.SelectedValue.ToString();
            m.Id_Value3 = txtIDValue3.Text;
            m.Contact_Type1 = cboContactType1.SelectedValue.ToString();
            m.Contact_Value1 = txtContactValue1.Text;
            m.Contact_Type2 = cboContactType2.SelectedValue.ToString();
            m.Contact_Value2 = txtContactValue2.Text;
            m.Contact_Type3 = cboContactType3.SelectedValue.ToString();
            m.Contact_Value3 = txtContactValue3.Text;
            m.Contact_Type4 = cboContactType4.SelectedValue.ToString();
            m.Contact_Value4 = txtContactValue4.Text;
            m.Address = txtAddress.Text;
            m.Province = cboProvince.SelectedValue.ToString();
            m.District = cboDistrict.SelectedValue.ToString();
            m.Commune = cboCommune.SelectedValue.ToString();
            m.Village = cboVillage.SelectedValue.ToString();
            m.Note = txtNote.Text;
            if (m.Id == 0)
            {
                log.Priority = Constant.Priority_Information;
                log.Type = Constant.Log_Insert;
            }
            else
            {
                log.Priority = Constant.Priority_Caution;
                log.Type = Constant.Log_Update;
            }
            try
            {
                m.Id = CustomerFacade.Save(m);
            }
            catch (Exception ex)
            {
                MessageFacade.Show(MessageFacade.error_save + "\r\n" + ex.Message, LabelFacade.sy_save, MessageBoxButtons.OK, MessageBoxIcon.Error);
                ErrorLogFacade.Log(ex);
            }
            if (dgvList.CurrentRow != null) RowIndex = dgvList.CurrentRow.Index;
            RefreshGrid(m.Id);
            LockControls();
            Cursor = Cursors.Default;
            log.Message = "Saved. Id=" + m.Id + ", Code=" + txtCustomerNo.Text;
            SessionLogFacade.Log(log);
            IsDirty = false;
            return true;
        }

        private void frmCustomerList_Load(object sender, EventArgs e)
        {
            Icon = Properties.Resources.Icon;
            try
            {
                dgvList.ShowLessColumns(true);
                SetSettings();
                SetLabels();
                Data.LoadBranch(cboBranch, false);
                Data.LoadList(cboType, "customer");
                Data.LoadList(cboCategory, "customer_category");
                Data.LoadList(cboGender, "gender");
                Data.LoadList(cboIDType1, "id");
                Data.LoadList(cboIDType2, "id");
                Data.LoadList(cboIDType3, "id");
                Data.LoadList(cboContactType1, "contact");
                Data.LoadList(cboContactType2, "contact");
                Data.LoadList(cboContactType3, "contact");
                Data.LoadList(cboContactType4, "contact");
                Data.LoadRegional(cboProvince, "'P', 'M'"); // Province and Municipality                

                SessionLogFacade.Log(Constant.Priority_Information, Constant.Module_Branch, Constant.Log_Open, "Opened");
                RefreshGrid();

                LoadData();
            }
            catch (Exception ex)
            {
                ErrorLogFacade.Log(ex, "Form_Load");
                MessageFacade.Show(MessageFacade.error_load_form + "\r\n" + ex.Message, LabelFacade.sy_customer, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            // ShowDialog; Select a customer
            btnSelect.Visible = IsDlg;
            if (IsDlg)
            {
                btnMode_Click(null, null);
                toolStrip1.Refresh();
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            if (!Privilege.CanAccess(Constant.Function_IC_Unit_Measure, Constant.Privilege_New))
            {
                MessageFacade.Show(MessageFacade.privilege_no_access, LabelFacade.sy_new, MessageBoxButtons.OK, MessageBoxIcon.Information);
                SessionLogFacade.Log(Constant.Priority_Caution, Constant.Module_Branch, Constant.Log_NoAccess, "New: No access");
                return;
            }
            if (IsExpand) picExpand_Click(sender, e);
            ClearAllBoxes();
            if (dgvList.CurrentRow != null)
                dgvList.CurrentRow.Selected = false;
            Id = 0;
            LockControls(false);
            cboType.Focus();
            cboBranch_SelectedIndexChanged(null, null);
            if (dgvList.CurrentRow != null) RowIndex = dgvList.CurrentRow.Index;
            SessionLogFacade.Log(Constant.Priority_Information, Constant.Module_Branch, Constant.Log_New, "New clicked");
            IsDirty = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnFind_Click(null, null);
            }
        }

        private void dgvList_SelectionChanged(object sender, EventArgs e)
        {
            if (IsIgnore) return;
            LoadData();
        }

        private void btnSaveNew_Click(object sender, EventArgs e)
        {
            SessionLogFacade.Log(Constant.Priority_Information, Constant.Module_Branch, Constant.Log_SaveAndNew, "Saved and new. Id=" + dgvList.Id + ", Code=" + txtCustomerNo.Text);
            btnSave_Click(sender, e);
            if (btnSaveNew.Enabled) return;
            btnNew_Click(sender, e);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var Id = dgvList.Id;
                if (Id == 0) return;
                // If referenced
                //todo: check if exist in ic_item
                // If locked
                var lInfo = CustomerFacade.GetLock(Id);
                string msg = "";
                if (lInfo.Locked)
                {
                    msg = string.Format(MessageFacade.delete_locked, lInfo.Lock_By, lInfo.Lock_At);
                    if (!Privilege.CanAccess(Constant.Function_IC_Unit_Measure, "O"))
                    {
                        MessageFacade.Show(msg, LabelFacade.sy_delete, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        SessionLogFacade.Log(Constant.Priority_Caution, Constant.Module_Branch, Constant.Log_Delete, "Cannot delete. Currently locked by '" + lInfo.Lock_By + "' since '" + lInfo.Lock_At + "' . Id=" + dgvList.Id + ", Code=" + txtCustomerNo.Text);
                        return;
                    }
                }
                // Delete
                msg = MessageFacade.delete_confirmation;
                if (lInfo.Locked) msg = string.Format(MessageFacade.lock_currently, lInfo.Lock_By, lInfo.Lock_At) + "'\n" + msg;
                if (MessageFacade.Show(msg, LabelFacade.sy_delete, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
                    return;
                try
                {
                    CustomerFacade.SetStatus(Id, Constant.RecordStatus_Deleted);
                }
                catch (Exception ex)
                {
                    MessageFacade.Show(MessageFacade.error_delete + "\r\n" + ex.Message, LabelFacade.sy_delete, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ErrorLogFacade.Log(ex);
                }
                RefreshGrid();
                // log
                SessionLogFacade.Log(Constant.Priority_Warning, Constant.Module_Branch, Constant.Log_Delete, "Deleted. Id=" + dgvList.Id + ", Code=" + txtCustomerNo.Text);
            }
            catch (Exception ex)
            {
                MessageFacade.Show(MessageFacade.error_delete + "\r\n" + ex.Message, LabelFacade.sy_delete, MessageBoxButtons.OK, MessageBoxIcon.Error);
                ErrorLogFacade.Log(ex);
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (!Privilege.CanAccess(Constant.Function_IC_Unit_Measure, Constant.Privilege_New))
            {
                MessageFacade.Show(MessageFacade.privilege_no_access, LabelFacade.sy_copy, MessageBoxButtons.OK, MessageBoxIcon.Information);
                SessionLogFacade.Log(Constant.Priority_Information, Constant.Module_Branch, Constant.Log_NoAccess, "Copy: No access");
                return;
            }
            Id = 0;
            if (IsExpand) picExpand_Click(sender, e);
            txtCustomerNo.Focus();
            LockControls(false);
            cboBranch_SelectedIndexChanged(null, null);
            SessionLogFacade.Log(Constant.Priority_Information, Constant.Module_Branch, Constant.Log_Copy, "Copy from Id=" + dgvList.Id + "Code=" + txtCustomerNo.Text);
            IsDirty = false;
        }

        private void picExpand_Click(object sender, EventArgs e)
        {
            splitContainer1.IsSplitterFixed = !IsExpand;
            if (!IsExpand)
            {
                splitContainer1.SplitterDistance = splitContainer1.Size.Width;
                splitContainer1.FixedPanel = FixedPanel.Panel2;
            }
            else
            {
                splitContainer1.SplitterDistance = ConfigFacade.ic_unit_measure_splitter_distance;
                splitContainer1.FixedPanel = FixedPanel.Panel1;
            }
            dgvList.ShowLessColumns(IsExpand);
            IsExpand = !IsExpand;
        }

        private void dgvList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            if (!IsDlg)
            {
                if (IsExpand) picExpand_Click(sender, e);
                dgvList_SelectionChanged(sender, e);    // reload data since SelectionChanged will not occured on current row
            }
            else
                btnSelect_Click(null, null);
        }

        private void btnActive_Click(object sender, EventArgs e)
        {
            var Id = dgvList.Id;
            if (Id == 0) return;

            string status = btnActive.Text == LabelFacade.sy_button_inactive ? Constant.RecordStatus_InActive : Constant.RecordStatus_Active;
            // If referenced
            //todo: check if already used in ic_item

            //If locked
            var lInfo = CustomerFacade.GetLock(Id);
            if (lInfo.Locked)
            {
                string msg = string.Format(MessageFacade.lock_currently, lInfo.Lock_By, lInfo.Lock_At);
                if (!Privilege.CanAccess(Constant.Function_IC_Unit_Measure, "O"))
                {
                    MessageFacade.Show(msg, MessageFacade.active_inactive, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                    if (MessageFacade.Show(msg + "\r\n" + MessageFacade.proceed_confirmation, MessageFacade.active_inactive, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
                        return;
            }
            try
            {
                CustomerFacade.SetStatus(Id, status);
            }
            catch (Exception ex)
            {
                MessageFacade.Show(MessageFacade.error_active_inactive + ex.Message, MessageFacade.active_inactive, MessageBoxButtons.OK, MessageBoxIcon.Error);
                ErrorLogFacade.Log(ex);
            }
            RefreshGrid();
            SessionLogFacade.Log(Constant.Priority_Caution, Constant.Module_Branch, status == Constant.RecordStatus_InActive ? Constant.Log_Inactive : Constant.Log_Active, "Id=" + dgvList.Id + ", Code=" + txtCustomerNo.Text);
        }

        private void btnUnlock_Click(object sender, EventArgs e)
        {
            if (!Privilege.CanAccess(Constant.Function_IC_Unit_Measure, Constant.Privilege_Update))
            {
                MessageFacade.Show(MessageFacade.privilege_no_access, LabelFacade.sy_button_unlock, MessageBoxButtons.OK, MessageBoxIcon.Information);
                SessionLogFacade.Log(Constant.Priority_Information, Constant.Module_Branch, Constant.Log_NoAccess, "Copy: No access");
                return;
            }
            if (IsExpand) picExpand_Click(sender, e);
            Id = dgvList.Id;
            // Cancel
            if (btnUnlock.Text == LabelFacade.sy_button_cancel)
            {
                if (IsDirty)
                {
                    var result = MessageFacade.Show(MessageFacade.save_confirmation, LabelFacade.sy_button_cancel, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (result == System.Windows.Forms.DialogResult.Yes) // Save then close
                        btnSave_Click(null, null);
                    else if (result == System.Windows.Forms.DialogResult.No)
                        LoadData(); // Load original back if changes (dirty)
                    else if (result == System.Windows.Forms.DialogResult.Cancel)
                        return;
                }
                CustomerFacade.DecrementSrNo(cboBranch.SelectedValue.ToString());
                LockControls(true);
                dgvList.Focus();
                try
                {
                    CustomerFacade.ReleaseLock(dgvList.Id);
                }
                catch (Exception ex)
                {
                    MessageFacade.Show(MessageFacade.error_unlock + "\r\n" + ex.Message, LabelFacade.sy_unlock, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ErrorLogFacade.Log(ex);
                    return;
                }
                if (dgvList.CurrentRow != null && !dgvList.CurrentRow.Selected)
                    dgvList.CurrentRow.Selected = true;
                SessionLogFacade.Log(Constant.Priority_Information, Constant.Module_Branch, Constant.Log_Unlock, "Unlock cancel. Id=" + dgvList.Id + ", Code=" + txtCustomerNo.Text);
                btnUnlock.ToolTipText = "Unlock (Ctrl+L)";
                IsDirty = false;
                return;
            }
            // Unlock
            if (Id == 0) return;
            try
            {
                var lInfo = CustomerFacade.GetLock(Id);

                if (lInfo.Locked) // Check if record is locked
                {
                    string msg = string.Format(MessageFacade.lock_currently, lInfo.Lock_By, lInfo.Lock_At);
                    if (!Privilege.CanAccess(Constant.Function_IC_Unit_Measure, "O"))
                    {
                        MessageFacade.Show(msg, LabelFacade.sy_unlock, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else
                        if (MessageFacade.Show(msg + "\r\n" + MessageFacade.lock_override, LabelFacade.sy_unlock, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                            SessionLogFacade.Log(Constant.Priority_Caution, Constant.Module_Branch, Constant.Log_Lock, "Override lock. Id=" + dgvList.Id + ", Code=" + txtCustomerNo.Text);
                        else
                            return;
                }
                txtAddress.SelectionStart = txtAddress.Text.Length;
                txtAddress.Focus();
                LockControls(false);
            }
            catch (Exception ex)
            {
                MessageFacade.Show(MessageFacade.error_unlock + "\r\n" + ex.Message, LabelFacade.sy_unlock, MessageBoxButtons.OK, MessageBoxIcon.Error);
                ErrorLogFacade.Log(ex);
                return;
            }
            try
            {
                CustomerFacade.Lock(dgvList.Id, txtCustomerNo.Text);
            }
            catch (Exception ex)
            {
                MessageFacade.Show(MessageFacade.error_lock + "\r\n" + ex.Message, LabelFacade.sy_lock, MessageBoxButtons.OK, MessageBoxIcon.Error);
                ErrorLogFacade.Log(ex);
                return;
            }
            SessionLogFacade.Log(Constant.Priority_Information, Constant.Module_Branch, Constant.Log_Lock, "Locked. Id=" + dgvList.Id + ", Code=" + txtCustomerNo.Text);
            btnUnlock.ToolTipText = "Cancel (Esc or Ctrl+L)";
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Control | Keys.N:
                    if (btnNew.Enabled) btnNew_Click(null, null);
                    break;
                case Keys.Control | Keys.Y:
                    if (btnCopy.Enabled) btnCopy_Click(null, null);
                    break;
                case Keys.Control | Keys.L:
                    if (btnUnlock.Enabled) btnUnlock_Click(null, null);
                    break;
                case Keys.Escape:
                    if (btnUnlock.Text.StartsWith("C")) btnUnlock_Click(null, null);    // Cancel
                    break;
                case Keys.Control | Keys.S:
                    if (btnSave.Enabled) btnSave_Click(null, null);
                    break;
                case Keys.Control | Keys.W:
                    if (btnSaveNew.Enabled) btnSaveNew_Click(null, null);
                    break;
                case Keys.Control | Keys.E:
                    if (btnActive.Enabled) btnActive_Click(null, null);
                    break;
                case Keys.Control | Keys.F:
                    if (!txtFind.ReadOnly) txtFind.Focus();
                    break;
                case Keys.F3:
                case Keys.F5:
                    if (btnFind.Enabled) btnFind_Click(null, null);
                    break;
                case Keys.F4:
                    if (btnClear.Enabled) btnClear_Click(null, null);
                    break;
                case Keys.F8:
                    if (btnFilter.Enabled) btnFilter_Click(null, null);
                    break;
                case Keys.F9:
                    if (btnMode.Enabled) btnMode_Click(null, null);
                    break;
                case Keys.F12:
                    if (btnExport.Enabled) btnExport_Click(null, null);
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (txtFind.Text.Length == 0) btnFind_Click(null, null);
        }

        private void mnuShow_CheckedChanged(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            if (!mnuShowA.Checked && !mnuShowI.Checked)
                mnuShowA.Checked = true;
            RefreshGrid();
            Cursor = Cursors.Default;
        }

        private void Dirty_TextChanged(object sender, EventArgs e)
        {
            IsDirty = true;
        }

        private void frmUnitMeasureList_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsDirty)
            {
                switch (MessageFacade.Show(MessageFacade.save_confirmation, LabelFacade.sy_close, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                {
                    case System.Windows.Forms.DialogResult.Yes: // Save then close
                        if (!Save())
                            e.Cancel = true;
                        break;
                    case System.Windows.Forms.DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }
            if (e.Cancel) return;
            IsDirty = false;
            if (btnUnlock.Text == LabelFacade.sy_button_cancel)
                btnUnlock_Click(null, null);
            if (!IsExpand)
                ConfigFacade.Set(Name + Constant.Splitter_Distance, splitContainer1.SplitterDistance);
            Util.SaveFormSate(this);    
        }

        private void txtCode_Leave(object sender, EventArgs e)
        {
            //// Check if entered code already exists
            //if (txtNo.ReadOnly) return;
            //if (CustomerFacade.Exists(txtNo.Text.Trim()))
            //{
            //    MessageFacade.Show(this, ref fMsg, LabelFacade.sy_msg_prefix + MessageFacade.code_already_exists, LabelFacade.sy_customer);
            //}
        }

        private void btnMode_Click(object sender, EventArgs e)
        {
            splitContainer1.IsSplitterFixed = !IsExpand;
            if (!IsExpand)
            {
                ConfigFacade.ic_unit_measure_splitter_distance = splitContainer1.SplitterDistance;
                splitContainer1.SplitterDistance = splitContainer1.Size.Width;
                splitContainer1.FixedPanel = FixedPanel.Panel2;
            }
            else
            {
                splitContainer1.SplitterDistance = ConfigFacade.ic_unit_measure_splitter_distance;
                splitContainer1.FixedPanel = FixedPanel.Panel1;
            }
            dgvList.ShowLessColumns(IsExpand);
            IsExpand = !IsExpand;
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            mnuShow.Show(btnFilter, 0, 27);
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            RefreshGrid();
            txtFind.Focus();
            Cursor = Cursors.Default;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtFind.Clear();
            txtFind.Focus();
        }

        private void dgvList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Delete) return;
            if (btnDelete.Enabled) btnDelete_Click(null, null);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            Application.DoEvents();
            CustomerFacade.Export();
            Cursor = Cursors.Default;
        }

        private void lblSearch_Click(object sender, EventArgs e)
        {
            txtFind.Focus();
        }

        private void txtFind_Enter(object sender, EventArgs e)
        {
            lblSearch.Visible = false;
        }

        private void txtFind_Leave(object sender, EventArgs e)
        {
            lblSearch.Visible = (txtFind.Text.Length == 0);
        }

        private void cboProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            Data.LoadRegional(cboDistrict, "'D'", cboProvince.SelectedValue);
            cboDistrict.SelectedIndex = -1;
        }

        private void cboDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            Data.LoadRegional(cboCommune, "'C'", cboDistrict.SelectedValue);
            cboCommune.SelectedIndex = -1;
        }

        private void cboCommune_SelectedIndexChanged(object sender, EventArgs e)
        {
            Data.LoadRegional(cboVillage, "'V'", cboCommune.SelectedValue);
            cboVillage.SelectedIndex = -1;
        }

        private void cboType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cboBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboBranch.SelectedIndex == -1 || btnNew.Enabled) return;
            txtCustomerNo.Text = CustomerFacade.GetNextCustomerNo(cboBranch.SelectedValue.ToString()); //todo: Format No; from table
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            CustomerNo = dgvList.CurrentRow.Cells["colCustomerNo"].Value.ToString();
            FullName = dgvList.CurrentRow.Cells["colName"].Value.ToString();
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}