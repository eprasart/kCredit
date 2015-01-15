using System;
using System.Windows.Forms;
using kBit.ERP.SM;
using kBit.ERP.SYS;
using System.Text;
using System.Drawing;

namespace kBit.ERP.GL
{
    public partial class frmAccount : Form
    {
        long Id = 0;
        int RowIndex = 0;   // Current gird selected row
        bool IsExpand = false;
        bool IsDirty = false;
        bool IsIgnore = true;

        frmMsg fMsg = null;

        StringFormat headerCellFormat = new StringFormat()
        {
            // right alignment might actually make more sense for numbers
            Alignment = StringAlignment.Near,
            LineAlignment = StringAlignment.Center
        };

        public frmAccount()
        {
            InitializeComponent();
        }

        private string GetStatus()
        {
            var status = "";
            if (mnuShowA.Checked && !mnuShowI.Checked)
                status = Type.RecordStatus_Active;
            else if (mnuShowI.Checked && !mnuShowA.Checked)
                status = Type.RecordStatus_InActive;
            return status;
        }

        private void RefreshGrid(long seq = 0)
        {
            Cursor = Cursors.WaitCursor;
            //IsIgnore = true;
            if (dgvList.SelectedRows.Count > 0) RowIndex = dgvList.SelectedRows[0].Index;
            try
            {
                dgvList.DataSource = AccountFacade.GetDataTable(txtFind.Text, GetStatus());
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageFacade.Show(MessageFacade.error_retrieve_data + "\r\n" + ex.Message, LabelFacade.sy_location, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            //LoadData();
            Cursor = Cursors.Default;
        }

        private void LockControls(bool l = true)
        {
            if (Id != 0 && l == false)
                txtCode.ReadOnly = true;
            else
                txtCode.ReadOnly = l;
            txtDesc.ReadOnly = l;
            cboNormalBalance.Enabled = !l;
            txtStructureCode.ReadOnly = l;
            txtStructureCodeDesc.ReadOnly = l;
            txtGroup.ReadOnly = l;
            txtFax.ReadOnly = l;
            txtEmail.ReadOnly = l;
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
            if (stat == Type.RecordStatus_Active)
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
            string Code = txtCode.Text.Trim();
            if (Code.Length == 0)
            {
                sMsg.AppendLine(LabelFacade.sy_msg_prefix + MessageFacade.code_not_empty);
                cFocus = txtCode;
            }
            else if (AccountFacade.Exists(Code, Id))
            {
                sMsg.AppendLine(LabelFacade.sy_msg_prefix + MessageFacade.code_already_exists);
                cFocus = txtCode;
            }
            if (cboNormalBalance.SelectedIndex == -1)
            {
                sMsg.AppendLine(LabelFacade.sy_msg_prefix + MessageFacade.location_type_not_empty);
                if (cFocus == null) cFocus = cboNormalBalance;
            }
            if (txtEmail.Text.Length > 0 && !Util.IsEmailValid(txtEmail.Text))
            {
                sMsg.AppendLine(LabelFacade.sy_msg_prefix + MessageFacade.email_not_valid);
                if (cFocus == null) cFocus = txtEmail;
            }
            if (sMsg.Length > 0)
            {
                MessageFacade.Show(this, ref fMsg, sMsg.ToString(), LabelFacade.sy_save);
                //fMsg.Show(sMsg.ToString(), LabelFacade.sy_save, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);                
                cFocus.Focus();
                return false;
            }
            return true;
        }

        private void ClearAllBoxes()
        {
            txtCode.Text = "";
            txtCode.Focus();
            txtDesc.Text = "";
            txtStructureCode.Text = "";
            txtStructureCodeDesc.Text = "";
            txtGroup.Text = "";
            txtFax.Text = "";
            txtEmail.Text = "";
            txtNote.Text = "";
            IsDirty = false;
        }

        private void LoadData()
        {
            var Id = dgvList.Id;
            if (Id != 0)
                try
                {
                    var m = AccountFacade.Select(Id);
                    txtCode.Text = m.Code;
                    txtDesc.Text = m.Description;
                    cboNormalBalance.SelectedIndex = (m.Type != "L" ? 0 : 1);
                    txtStructureCode.Text = m.Address;
                    txtStructureCodeDesc.Text = m.Name;
                    txtGroup.Text = m.Phone;
                    txtFax.Text = m.Fax;
                    txtEmail.Text = m.Email;
                    txtNote.Text = m.Note;
                    SetStatus(m.Status);
                    LockControls();
                    IsDirty = false;
                    SessionLogFacade.Log(Type.Priority_Information, Type.Module_IC_Location, Type.Log_View, "View. Id=" + m.Id + ", Code=" + m.Code);
                }
                catch (Exception ex)
                {
                    MessageFacade.Show(MessageFacade.error_load_record + "\r\n" + ex.Message, LabelFacade.sy_location, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    SYS.ErrorLogFacade.Log(ex);
                }
            else    // when delete all => disable buttons and clear all controls
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
            txtCode.CharacterCasing = cs;
        }

        private void SetSettings()
        {
            try
            {
                SetIconDisplayType(ConfigFacade.sy_toolbar_icon_display_type);
                splitContainer1.SplitterDistance = ConfigFacade.ic_location_splitter_distance;

                SetCodeCasing();
                txtCode.MaxLength = ConfigFacade.sy_code_max_length;
                var lo = ConfigFacade.ic_location_location;
                if (lo != new System.Drawing.Point(-1, -1))
                    Location = lo;
                var si = ConfigFacade.ic_location_size;
                if (si != new System.Drawing.Size(-1, -1))
                    Size = si;
                WindowState = (FormWindowState)ConfigFacade.ic_location_window_state;
            }
            catch (Exception ex)
            {
                ErrorLogFacade.Log(ex, "Set settings");
            }
        }

        private void SetLabels()
        {
            var prefix = "ic_location_";
            btnNew.Text = LabelFacade.sy_button_new;
            btnCopy.Text = LabelFacade.sy_button_copy;
            btnUnlock.Text = LabelFacade.sy_button_unlock;
            btnSave.Text = LabelFacade.sy_button_save;
            btnSaveNew.Text = LabelFacade.sy_button_save_new;
            btnActive.Text = LabelFacade.sy_button_inactive;
            btnDelete.Text = LabelFacade.sy_button_delete;
            btnMode.Text = LabelFacade.sy_button_mode;
            btnExport.Text = LabelFacade.sy_export;
            lblSearch.Text = LabelFacade.sy_search_place_holder;
            btnFind.Text = "     " + LabelFacade.sy_button_find;
            btnClear.Text = "     " + LabelFacade.sy_button_clear;
            btnFilter.Text = "     " + LabelFacade.sy_button_filter;

            colCode.HeaderText = LabelFacade.GetLabel(prefix + "code");
            lblCode.Text = "* " + colCode.HeaderText;
            lblDescription.Text = LabelFacade.GetLabel(prefix + "description");
            colDescription.HeaderText = lblDescription.Text;
            lblGroup.Text = LabelFacade.GetLabel(prefix + "type");
            lblAddress.Text = LabelFacade.GetLabel(prefix + "address");
            colAddress.HeaderText = lblAddress.Text;
            lblStructureCode.Text = LabelFacade.GetLabel(prefix + "name");
            colName.HeaderText = lblStructureCode.Text;
            lblType.Text = LabelFacade.GetLabel(prefix + "phone");
            colPhone.HeaderText = lblType.Text;
            lblPostToAccount.Text = LabelFacade.GetLabel(prefix + "fax");
            colFax.HeaderText = lblPostToAccount.Text;
            lblEmail.Text = LabelFacade.GetLabel(prefix + "email");
            colEmail.HeaderText = lblEmail.Text;
            glbLocation.Caption = LabelFacade.GetLabel(prefix + "location");
            glbContact.Caption = LabelFacade.GetLabel(prefix + "contact");
            glbNote.Caption = LabelFacade.GetLabel(prefix + "note");
        }

        private bool Save()
        {
            if (!IsValidated()) return false;
            Cursor = Cursors.WaitCursor;
            var m = new Account();
            var log = new SessionLog { Module = Type.Module_IC_Location };
            m.Id = Id;
            m.Code = txtCode.Text.Trim();
            m.Description = txtDesc.Text;
            m.Type = cboNormalBalance.Text.Substring(0, 1);
            m.Address = txtStructureCode.Text;
            m.Name = txtStructureCodeDesc.Text;
            m.Phone = txtGroup.Text;
            m.Fax = txtFax.Text;
            m.Email = txtEmail.Text;
            m.Note = txtNote.Text;
            if (m.Id == 0)
            {
                log.Priority = Type.Priority_Information;
                log.Type = Type.Log_Insert;
            }
            else
            {
                log.Priority = Type.Priority_Caution;
                log.Type = Type.Log_Update;
            }
            try
            {
                m.Id = AccountFacade.Save(m);
            }
            catch (Exception ex)
            {
                MessageFacade.Show(MessageFacade.error_save + "\r\n" + ex.Message, LabelFacade.sy_save, MessageBoxButtons.OK, MessageBoxIcon.Error);
                ErrorLogFacade.Log(ex);
            }
            if (dgvList.RowCount > 0) RowIndex = dgvList.CurrentRow.Index;
            RefreshGrid(m.Id);
            LockControls();
            Cursor = Cursors.Default;
            log.Message = "Saved. Id=" + m.Id + ", Code=" + txtCode.Text;
            SessionLogFacade.Log(log);
            IsDirty = false;
            return true;
        }

        private void frmLocationList_Load(object sender, EventArgs e)
        {
            Icon = Properties.Resources.Icon;
            try
            {
                dgvList.ShowLessColumns(true);
                SetSettings();
                SetLabels();
                SessionLogFacade.Log(Type.Priority_Information, Type.Module_IC_Location, Type.Log_Open, "Opened");
                RefreshGrid();
                LoadData();
            }
            catch (Exception ex)
            {
                ErrorLogFacade.Log(ex, "Form_Load");
                MessageFacade.Show(MessageFacade.error_load_form + "\r\n" + ex.Message, LabelFacade.sy_location, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            if (!Privilege.CanAccess(Type.Function_IC_Location, Type.Privilege_New))
            {
                MessageFacade.Show(MessageFacade.privilege_no_access, LabelFacade.sy_new, MessageBoxButtons.OK, MessageBoxIcon.Information);
                SessionLogFacade.Log(Type.Priority_Caution, Type.Module_IC_Location, Type.Log_NoAccess, "New: No access");
                return;
            }
            if (IsExpand) picExpand_Click(sender, e);
            ClearAllBoxes();
            if (dgvList.CurrentRow != null)
                dgvList.CurrentRow.Selected = false;
            Id = 0;
            LockControls(false);

            if (dgvList.CurrentRow != null) RowIndex = dgvList.CurrentRow.Index;
            SessionLogFacade.Log(Type.Priority_Information, Type.Module_IC_Location, Type.Log_New, "New clicked");
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
            SessionLogFacade.Log(Type.Priority_Information, Type.Module_IC_Location, Type.Log_SaveAndNew, "Saved and new. Id=" + dgvList.Id + ", Code=" + txtCode.Text);
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
                var lInfo = AccountFacade.GetLock(Id);
                string msg = "";
                if (lInfo.Locked)
                {
                    msg = string.Format(MessageFacade.delete_locked, lInfo.Lock_By, lInfo.Lock_At);
                    if (!Privilege.CanAccess(Type.Function_IC_Location, "O"))
                    {
                        MessageFacade.Show(msg, LabelFacade.sy_delete, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        SessionLogFacade.Log(Type.Priority_Caution, Type.Module_IC_Location, Type.Log_Delete, "Cannot delete. Currently locked by '" + lInfo.Lock_By + "' since '" + lInfo.Lock_At + "' . Id=" + dgvList.Id + ", Code=" + txtCode.Text);
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
                    AccountFacade.SetStatus(Id, Type.RecordStatus_Deleted);
                }
                catch (Exception ex)
                {
                    MessageFacade.Show(MessageFacade.error_delete + "\r\n" + ex.Message, LabelFacade.sy_delete, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ErrorLogFacade.Log(ex);
                }
                RefreshGrid();
                // log
                SessionLogFacade.Log(Type.Priority_Warning, Type.Module_IC_Location, Type.Log_Delete, "Deleted. Id=" + dgvList.Id + ", Code=" + txtCode.Text);
            }
            catch (Exception ex)
            {
                MessageFacade.Show(MessageFacade.error_delete + "\r\n" + ex.Message, LabelFacade.sy_delete, MessageBoxButtons.OK, MessageBoxIcon.Error);
                ErrorLogFacade.Log(ex);
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (!Privilege.CanAccess(Type.Function_IC_Location, Type.Privilege_New))
            {
                MessageFacade.Show(MessageFacade.privilege_no_access, LabelFacade.sy_copy, MessageBoxButtons.OK, MessageBoxIcon.Information);
                SessionLogFacade.Log(Type.Priority_Information, Type.Module_IC_Location, Type.Log_NoAccess, "Copy: No access");
                return;
            }
            Id = 0;
            if (IsExpand) picExpand_Click(sender, e);
            txtCode.Focus();
            LockControls(false);
            SessionLogFacade.Log(Type.Priority_Information, Type.Module_IC_Location, Type.Log_Copy, "Copy from Id=" + dgvList.Id + "Code=" + txtCode.Text);
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
                splitContainer1.SplitterDistance = ConfigFacade.ic_location_splitter_distance;
                splitContainer1.FixedPanel = FixedPanel.Panel1;
            }
            dgvList.ShowLessColumns(IsExpand);
            IsExpand = !IsExpand;
        }

        private void dgvList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            if (IsExpand) picExpand_Click(sender, e);
            dgvList_SelectionChanged(sender, e);    // reload data since SelectionChanged will not occured on current row
        }

        private void btnActive_Click(object sender, EventArgs e)
        {
            var Id = dgvList.Id;
            if (Id == 0) return;

            string status = btnActive.Text == LabelFacade.sy_button_inactive ? Type.RecordStatus_InActive : Type.RecordStatus_Active;
            // If referenced
            //todo: check if already used in ic_item

            //If locked
            var lInfo = AccountFacade.GetLock(Id);
            if (lInfo.Locked)
            {
                string msg = string.Format(MessageFacade.lock_currently, lInfo.Lock_By, lInfo.Lock_At);
                if (!Privilege.CanAccess(Type.Function_IC_Location, "O"))
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
                AccountFacade.SetStatus(Id, status);
            }
            catch (Exception ex)
            {
                MessageFacade.Show(MessageFacade.error_active_inactive + ex.Message, MessageFacade.active_inactive, MessageBoxButtons.OK, MessageBoxIcon.Error);
                ErrorLogFacade.Log(ex);
            }
            RefreshGrid();
            SessionLogFacade.Log(Type.Priority_Caution, Type.Module_IC_Location, status == Type.RecordStatus_InActive ? Type.Log_Inactive : Type.Log_Active, "Id=" + dgvList.Id + ", Code=" + txtCode.Text);
        }

        private void btnUnlock_Click(object sender, EventArgs e)
        {
            if (!Privilege.CanAccess(Type.Function_IC_Location, Type.Privilege_Update))
            {
                MessageFacade.Show(MessageFacade.privilege_no_access, LabelFacade.sy_button_unlock, MessageBoxButtons.OK, MessageBoxIcon.Information);
                SessionLogFacade.Log(Type.Priority_Information, Type.Module_IC_Location, Type.Log_NoAccess, "Copy: No access");
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
                LockControls(true);
                dgvList.Focus();
                try
                {
                    AccountFacade.ReleaseLock(dgvList.Id);
                }
                catch (Exception ex)
                {
                    MessageFacade.Show(MessageFacade.error_unlock + "\r\n" + ex.Message, LabelFacade.sy_unlock, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ErrorLogFacade.Log(ex);
                    return;
                }
                if (dgvList.CurrentRow != null && !dgvList.CurrentRow.Selected)
                    dgvList.CurrentRow.Selected = true;
                SessionLogFacade.Log(Type.Priority_Information, Type.Module_IC_Location, Type.Log_Unlock, "Unlock cancel. Id=" + dgvList.Id + ", Code=" + txtCode.Text);
                btnUnlock.ToolTipText = "Unlock (Ctrl+L)";
                IsDirty = false;
                return;
            }
            // Unlock
            if (Id == 0) return;
            try
            {
                var lInfo = AccountFacade.GetLock(Id);

                if (lInfo.Locked) // Check if record is locked
                {
                    string msg = string.Format(MessageFacade.lock_currently, lInfo.Lock_By, lInfo.Lock_At);
                    if (!Privilege.CanAccess(Type.Function_IC_Location, "O"))
                    {
                        MessageFacade.Show(msg, LabelFacade.sy_unlock, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else
                        if (MessageFacade.Show(msg + "\r\n" + MessageFacade.lock_override, LabelFacade.sy_unlock, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                            SessionLogFacade.Log(Type.Priority_Caution, Type.Module_IC_Location, Type.Log_Lock, "Override lock. Id=" + dgvList.Id + ", Code=" + txtCode.Text);
                        else
                            return;
                }
                txtDesc.SelectionStart = txtDesc.Text.Length;
                txtDesc.Focus();
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
                AccountFacade.Lock(dgvList.Id, txtCode.Text);
            }
            catch (Exception ex)
            {
                MessageFacade.Show(MessageFacade.error_lock + "\r\n" + ex.Message, LabelFacade.sy_lock, MessageBoxButtons.OK, MessageBoxIcon.Error);
                ErrorLogFacade.Log(ex);
                return;
            }
            SessionLogFacade.Log(Type.Priority_Information, Type.Module_IC_Location, Type.Log_Lock, "Locked. Id=" + dgvList.Id + ", Code=" + txtCode.Text);
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

        private void frmLocationList_FormClosing(object sender, FormClosingEventArgs e)
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
            if (btnUnlock.Text == "Cance&l")
                btnUnlock_Click(null, null);

            // Set config values
            if (!IsExpand)
                ConfigFacade.ic_location_splitter_distance = splitContainer1.SplitterDistance;
            ConfigFacade.ic_location_location = Location;
            ConfigFacade.ic_location_window_state = (int)WindowState;
            if (WindowState == FormWindowState.Normal) ConfigFacade.ic_location_size = Size;
        }

        private void txtCode_Leave(object sender, EventArgs e)
        {
            // Check if entered code already exists
            if (txtCode.ReadOnly) return;
            if (AccountFacade.Exists(txtCode.Text.Trim()))
            {
                MessageFacade.Show(this, ref fMsg, LabelFacade.sy_msg_prefix + MessageFacade.code_already_exists, LabelFacade.sy_location);
            }
        }

        private void btnMode_Click(object sender, EventArgs e)
        {
            splitContainer1.IsSplitterFixed = !IsExpand;
            if (!IsExpand)
            {
                ConfigFacade.ic_location_splitter_distance = splitContainer1.SplitterDistance;
                splitContainer1.SplitterDistance = splitContainer1.Size.Width;
                splitContainer1.FixedPanel = FixedPanel.Panel2;
            }
            else
            {
                splitContainer1.SplitterDistance = ConfigFacade.ic_location_splitter_distance;
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
            AccountFacade.Export();
            Cursor = Cursors.Default;
        }

        private void frmLocationList_Leave(object sender, EventArgs e)
        {

        }

        private void frmLocationList_Deactivate(object sender, EventArgs e)
        {
            //fMsg.TopMost = false;
            //fMsg.Update();
            //fMsg.Activate();
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

        //private void dgvList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        //{
        //e.PaintCells(e.ClipBounds, DataGridViewPaintParts.All);
        //e.PaintHeader(DataGridViewPaintParts.Background | DataGridViewPaintParts.Border | DataGridViewPaintParts.Focus
        //    | DataGridViewPaintParts.SelectionBackground | DataGridViewPaintParts.ContentForeground);

        //var rowIdx = " " + (e.RowIndex + 1).ToString();

        //var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, dgvList.RowHeadersWidth, e.RowBounds.Height);
        //e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, headerCellFormat);         
        //}
    }
}