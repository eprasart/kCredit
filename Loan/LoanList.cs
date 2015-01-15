using System;
using System.Windows.Forms;
using kCredit.SM;
using kCredit.SYS;
using System.Text;
using System.Drawing;

namespace kCredit
{
    public partial class frmLoan : Form
    {
        long Id = 0;
        int RowIndex = 0;   // Current gird selected row
        bool IsExpand = false;
        bool IsDirty = false;
        bool IsIgnore = true;

        DateTime dteMaturity;

        frmMsg fMsg = null;

        StringFormat headerCellFormat = new StringFormat()
        {
            // right alignment might actually make more sense for numbers
            Alignment = StringAlignment.Near,
            LineAlignment = StringAlignment.Center
        };

        public frmLoan()
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

        private DateTime GetNextRepaymentDate(DateTime dteCurrent, int nNo = 1)
        {
            string sUnit = cboFrequencyUnit.SelectedValue.ToString();
            DateTime dteRepayment = dteCurrent;
            switch (sUnit)
            {
                case "W":
                    dteRepayment = dteCurrent.AddDays(7 * nNo);
                    break;
                case "M":
                    dteRepayment = dteCurrent.AddMonths(nNo);
                    break;
            }
            return dteRepayment;
        }

        // First installment & Maturity date
        private void CalculateDates()
        {
            int nFrequency = int.Parse(txtFrequency.Text);
            int nInstallmentNo = int.Parse(txtInstallmentNo.Text);

            if (!dtpFirstInstallment.Checked)
            {
                dtpFirstInstallment.Value = GetNextRepaymentDate(dtpDisburse.Value);
                dtpFirstInstallment.Checked = false;
            }
            dteMaturity = GetNextRepaymentDate(dtpFirstInstallment.Value, nInstallmentNo - 1);
            txtMaturity.Text = dteMaturity.ToString("ddd dd-MM-yy");
        }

        private double GetRatePerDay(string sUnit, double Rate)
        {
            double r = 0.0;
            if (Rate > 1) Rate /= 100;
            switch (sUnit)
            {
                case "W":
                    r = Rate / 7;
                    break;
                case "M":
                    r = Rate / 30;
                    break;
            }
            return r;
        }

        private void GenerateSchedule()
        {
            CalculateDates();
            dataGridView1.Rows.Clear();
            dataGridView1.Rows.Add();

            int nInstallmentNo = int.Parse(txtInstallmentNo.Text);
            string sFrequencyUnit = cboFrequencyUnit.SelectedValue.ToString();
            double dAmount = double.Parse(txtAmount.Text);
            double dInterestRate = double.Parse(txtInterestRate.Text);
            double dPrincipalPay = 1.0 * dAmount / nInstallmentNo;
            double dPrincipalPayLast = dAmount - (dPrincipalPay * (nInstallmentNo - 1));
            double dRatePerDay = GetRatePerDay(sFrequencyUnit, dInterestRate);

            DateTime dtePrevious = dtpDisburse.Value;
            DateTime dteFirstInstallment = dtpFirstInstallment.Value;
            DateTime dteRepayment = dteFirstInstallment;

            double lPrincipalOut = dAmount;

            int iDayNum = (int)(dteRepayment - dtePrevious).TotalDays;
            double dInterestCal = lPrincipalOut * iDayNum * dRatePerDay;

            double lInterestPay = dInterestCal;
            double dblTotalInterestCal = dInterestCal;
            double lTotalInterestPay = lInterestPay;
            double lTotalPay = dPrincipalPay + lInterestPay;

            dtePrevious = dteRepayment;
            lPrincipalOut = dAmount - dPrincipalPay;

            dataGridView1.Rows[0].Cells["colNo"].Value = 1;
            dataGridView1.Rows[0].Cells["colDate"].Value = dteRepayment;
            dataGridView1.Rows[0].Cells["colPrin"].Value = dPrincipalPay;
            dataGridView1.Rows[0].Cells["colInt"].Value = dInterestCal;
            dataGridView1.Rows[0].Cells["colTotal"].Value = lTotalPay;

            for (int i = 2; i < nInstallmentNo; i++)
            {
                dteRepayment = GetNextRepaymentDate(dteFirstInstallment, i - 1);
                iDayNum = (int)(dteRepayment - dtePrevious).TotalDays;
                dInterestCal = lPrincipalOut * iDayNum * dRatePerDay;
                dblTotalInterestCal += dInterestCal;

                lInterestPay = dInterestCal;
                lTotalInterestPay = lTotalInterestPay + lInterestPay;
                lPrincipalOut = dAmount - (i * dPrincipalPay);
                dtePrevious = dteRepayment;
                if (i < nInstallmentNo)
                    lTotalPay = lTotalPay + dPrincipalPay + lInterestPay;
                else
                    lTotalPay = lTotalPay + dPrincipalPayLast + lInterestPay;

                dataGridView1.Rows.Add();
                dataGridView1.Rows[i - 1].Cells["colNo"].Value = i;
                dataGridView1.Rows[i - 1].Cells["colDate"].Value = dteRepayment;
                dataGridView1.Rows[i - 1].Cells["colPrin"].Value = dPrincipalPay;
                dataGridView1.Rows[i - 1].Cells["colInt"].Value = dInterestCal;
                dataGridView1.Rows[i - 1].Cells["colTotal"].Value = dPrincipalPay + lInterestPay;
            }
            iDayNum = (int)(dteMaturity - dtePrevious).TotalDays;
            dInterestCal = lPrincipalOut * iDayNum * dRatePerDay;
            dblTotalInterestCal = dblTotalInterestCal + dInterestCal;

            dataGridView1.Rows[nInstallmentNo - 1].Cells["colNo"].Value = nInstallmentNo;
            dataGridView1.Rows[nInstallmentNo - 1].Cells["colDate"].Value = dteMaturity;
            dataGridView1.Rows[nInstallmentNo - 1].Cells["colPrin"].Value = dPrincipalPayLast;
            dataGridView1.Rows[nInstallmentNo - 1].Cells["colInt"].Value = dInterestCal;
            dataGridView1.Rows[nInstallmentNo - 1].Cells["colTotal"].Value = dPrincipalPay + dInterestCal;
        }

        private void RefreshGrid(long seq = 0)
        {
            Cursor = Cursors.WaitCursor;
            //IsIgnore = true;
            if (dgvList.SelectedRows.Count > 0) RowIndex = dgvList.SelectedRows[0].Index;
            try
            {
                dgvList.DataSource = LoanFacade.GetDataTable(txtFind.Text, GetStatus());
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
            //LoadData();
            Cursor = Cursors.Default;
        }

        private void LockControls(bool l = true)
        {
            //if (Id != 0 && l == false)
            //    txtNo.ReadOnly = true;
            //else
            //    txtNo.ReadOnly = l;
            txtFrequency.ReadOnly = l;
            txtFullName.ReadOnly = l;
            cboFrequencyUnit.Enabled = !l;
            cboPurpose.Enabled = !l;
            cboPaymentSite.Enabled = !l;
            cboAgent.Enabled = !l;
            cboCurrency.Enabled = !l;
            cboMethod.Enabled = !l;
            dtpDisburse.Enabled = !l;
            dtpFirstInstallment.Enabled = !l;
            txtInstallmentNo.ReadOnly = l;
            txtAmount.ReadOnly = l;
            txtInterestRate.ReadOnly = l;
            chkSaturday.Enabled = !l;
            chkSunday.Enabled = !l;
            chkHoliday.Enabled = !l;
            chkNeverOn_CheckedChanged(null, null);
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
            string No = txtAccountNo.Text.Trim();
            if (No.Length == 0)
            {
                sMsg.AppendLine(LabelFacade.sy_msg_prefix + MessageFacade.code_not_empty);
                cFocus = txtAccountNo;
            }
            else if (LoanFacade.Exists(No, Id))
            {
                sMsg.AppendLine(LabelFacade.sy_msg_prefix + MessageFacade.code_already_exists);
                cFocus = txtAccountNo;
            }
            if (txtFrequency.Text.Trim().Length == 0)
            {
                sMsg.AppendLine(LabelFacade.sy_msg_prefix + MessageFacade.code_already_exists);
                cFocus = txtFrequency;
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
            txtAccountNo.Text = "";
            txtAccountNo.Focus();
            txtFrequency.Text = "1";
            txtInstallmentNo.Text = "";
            txtAmount.Text = "";
            txtInterestRate.Text = "";
            dtpDisburse.Value = DateTime.Today;
            dtpFirstInstallment.Checked = false;
            txtMaturity.Text = "";
            //txtAccountNo.Text=    //todo: get new acc.
            txtCustomerNo.Text = "";
            txtFullName.Text = "";
            cboPaymentSite.SelectedIndex = -1;
            cboAgent.SelectedIndex = -1;
            txtAccountStatus.Text = "Active";
            chkSaturday.Checked = true;
            chkSunday.Checked = true;
            chkHoliday.Checked = true;
            cboMove.SelectedIndex = 0;
            txtNote.Text = "";
            IsDirty = false;
        }

        private void LoadData()
        {
            var Id = dgvList.Id;
            if (Id != 0)
                try
                {
                    var m = LoanFacade.Select(Id);
                    txtAccountNo.Text = m.Account_No;
                    cboFrequencyUnit.SelectedValue = m.Frequency_Unit;
                    txtFrequency.Text = m.Frequency.ToString();
                    txtInstallmentNo.Text = m.Installment_No.ToString();
                    txtAmount.Text = m.Amount.ToString();
                    cboCurrency.SelectedValue = m.Currency;
                    txtInterestRate.Text = m.Interest_Rate.ToString();
                    cboMethod.SelectedValue = m.Calculation_Method;
                    dtpDisburse.Value = m.Disburse_Date;
                    CalculateDates();
                    dtpFirstInstallment.Checked = (dtpFirstInstallment.Value != GetNextRepaymentDate(dtpDisburse.Value));
                    txtAccountNo.Text = m.Account_No;
                    txtCustomerNo.Text = m.Customer_No;
                    //txtFullName.Text = m   //todo: fullname                                        
                    cboPurpose.SelectedValue = m.Purpose;
                    cboPaymentSite.SelectedValue = m.Payment_Site;
                    cboAgent.SelectedValue = m.Credit_Agent_Id;
                    txtAccountStatus.Text = m.Account_Status;
                    chkSaturday.Checked = (m.Non_Working_Day_Move.Contains("6"));
                    chkSunday.Checked = (m.Non_Working_Day_Move.Contains("0"));
                    chkHoliday.Checked = (m.Non_Working_Day_Move.Contains("H"));
                    cboMove.SelectedValue = m.Non_Working_Day_Move;
                    txtNote.Text = m.Note;
                    SetStatus(m.Status);
                    LockControls();
                    IsDirty = false;
                    SessionLogFacade.Log(Type.Priority_Information, Type.Module_Branch, Type.Log_View, "View. Id=" + m.Id + ", Account No.=" + m.Account_No);
                }
                catch (Exception ex)
                {
                    MessageFacade.Show(MessageFacade.error_load_record + "\r\n" + ex.Message, LabelFacade.sy_customer, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ErrorLogFacade.Log(ex);
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
            txtAccountNo.CharacterCasing = cs;
        }

        private void SetSettings()
        {
            try
            {
                SetIconDisplayType(ConfigFacade.sy_toolbar_icon_display_type);
                splitContainer1.SplitterDistance = ConfigFacade.ic_unit_measure_splitter_distance;

                SetCodeCasing();
                //txtAccountNo.MaxLength = ConfigFacade.sy_code_max_length;

                //todo: implement this
                //var lo = ConfigFacade.ic_unit_measure_location;
                //if (lo != new System.Drawing.Point(-1, -1))
                //    Location = lo;
                //var si = ConfigFacade.ic_unit_measure_size;
                //if (si != new System.Drawing.Size(-1, -1))
                //    Size = si;
                //WindowState = (FormWindowState)ConfigFacade.ic_unit_measure_window_state;
            }
            catch (Exception ex)
            {
                ErrorLogFacade.Log(ex, "Set settings");
            }
        }

        private void SetLabels()
        {
            var prefix = "loan_";
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

            colCode.HeaderText = LabelFacade.GetLabel(prefix + "code") ?? colCode.HeaderText;

            lblName.Text = LabelFacade.GetLabel(prefix + "default_factor") ?? lblName.Text;
            //colDescription.HeaderText = LabelFacade.GetLabel(prefix + "description") ?? lblDescription.Text;
            //lblDescription.Text = colDescription.HeaderText;
            glbGeneral.Caption = LabelFacade.GetLabel(prefix + "general") ?? glbGeneral.Caption;
            glbNote.Caption = LabelFacade.GetLabel(prefix + "note") ?? glbNote.Caption;
        }

        private bool Save()
        {
            if (!IsValidated()) return false;
            Cursor = Cursors.WaitCursor;
            var m = new Loan();
            var log = new SessionLog { Module = Type.Module_Branch };
            m.Id = Id;
            m.Frequency_Unit = cboFrequencyUnit.SelectedValue.ToString();
            m.Frequency = int.Parse(txtFrequency.Text);
            m.Installment_No = int.Parse(txtInstallmentNo.Text);
            m.Amount = double.Parse(txtAmount.Text);
            m.Currency = cboCurrency.SelectedValue.ToString();
            m.Interest_Rate = double.Parse(txtInterestRate.Text);
            m.Calculation_Method = cboMethod.SelectedValue.ToString();
            m.Disburse_Date = dtpDisburse.Value;
            m.First_Installment_Date = dtpFirstInstallment.Value;
            m.Maturity_Mate = dteMaturity;
            m.Account_No = txtAccountNo.Text;
            m.Customer_No = txtCustomerNo.Text;
            m.Purpose = cboPurpose.SelectedValue.ToString();
            m.Payment_Site = cboPaymentSite.SelectedValue.ToString();
            m.Credit_Agent_Id = int.Parse(cboAgent.SelectedValue.ToString());
            string sNeverOn = "";
            if (chkSaturday.Checked) sNeverOn = "6";
            if (chkSunday.Checked) sNeverOn += "0";
            if (chkHoliday.Checked) sNeverOn += "H";
            m.Non_Working_Day_Move = cboMove.SelectedValue.ToString();
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
                m.Id = LoanFacade.Save(m);
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
            log.Message = "Saved. Id=" + m.Id + ", Code=" + txtAccountNo.Text;
            SessionLogFacade.Log(log);
            IsDirty = false;
            return true;
        }

        private void frmUnitMeasureList_Load(object sender, EventArgs e)
        {
            Icon = Properties.Resources.Icon;
            try
            {
                dgvList.ShowLessColumns(true);
                SetSettings();
                SetLabels();
                Data.LoadList(cboFrequencyUnit, "frequency_unit");
                Data.LoadList(cboMethod, "calculation_method");
                Data.LoadList(cboPurpose, "loan_purpose");
                Data.LoadList(cboPaymentSite, "payment_site");
                Data.LoadAgent(cboAgent);
                Data.LoadList(cboMove, "non_working_day_move");
                Data.LoadCurrency(cboCurrency);

                SessionLogFacade.Log(Type.Priority_Information, Type.Module_Branch, Type.Log_Open, "Opened");
                RefreshGrid();
                LoadData();
            }
            catch (Exception ex)
            {
                ErrorLogFacade.Log(ex, "Form_Load");
                MessageFacade.Show(MessageFacade.error_load_form + "\r\n" + ex.Message, LabelFacade.sy_customer, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            if (!Privilege.CanAccess(Type.Function_IC_Unit_Measure, Type.Privilege_New))
            {
                MessageFacade.Show(MessageFacade.privilege_no_access, LabelFacade.sy_new, MessageBoxButtons.OK, MessageBoxIcon.Information);
                SessionLogFacade.Log(Type.Priority_Caution, Type.Module_Branch, Type.Log_NoAccess, "New: No access");
                return;
            }
            if (IsExpand) picExpand_Click(sender, e);
            ClearAllBoxes();
            if (dgvList.CurrentRow != null)
                dgvList.CurrentRow.Selected = false;
            Id = 0;
            LockControls(false);
            cboPurpose.Focus();
            cboBranch_SelectedIndexChanged(null, null);
            if (dgvList.CurrentRow != null) RowIndex = dgvList.CurrentRow.Index;
            SessionLogFacade.Log(Type.Priority_Information, Type.Module_Branch, Type.Log_New, "New clicked");
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
            SessionLogFacade.Log(Type.Priority_Information, Type.Module_Branch, Type.Log_SaveAndNew, "Saved and new. Id=" + dgvList.Id + ", Code=" + txtAccountNo.Text);
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
                var lInfo = LoanFacade.GetLock(Id);
                string msg = "";
                if (lInfo.Locked)
                {
                    msg = string.Format(MessageFacade.delete_locked, lInfo.Lock_By, lInfo.Lock_At);
                    if (!Privilege.CanAccess(Type.Function_IC_Unit_Measure, "O"))
                    {
                        MessageFacade.Show(msg, LabelFacade.sy_delete, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        SessionLogFacade.Log(Type.Priority_Caution, Type.Module_Branch, Type.Log_Delete, "Cannot delete. Currently locked by '" + lInfo.Lock_By + "' since '" + lInfo.Lock_At + "' . Id=" + dgvList.Id + ", Code=" + txtAccountNo.Text);
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
                    LoanFacade.SetStatus(Id, Type.RecordStatus_Deleted);
                }
                catch (Exception ex)
                {
                    MessageFacade.Show(MessageFacade.error_delete + "\r\n" + ex.Message, LabelFacade.sy_delete, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ErrorLogFacade.Log(ex);
                }
                RefreshGrid();
                // log
                SessionLogFacade.Log(Type.Priority_Warning, Type.Module_Branch, Type.Log_Delete, "Deleted. Id=" + dgvList.Id + ", Code=" + txtAccountNo.Text);
            }
            catch (Exception ex)
            {
                MessageFacade.Show(MessageFacade.error_delete + "\r\n" + ex.Message, LabelFacade.sy_delete, MessageBoxButtons.OK, MessageBoxIcon.Error);
                ErrorLogFacade.Log(ex);
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (!Privilege.CanAccess(Type.Function_IC_Unit_Measure, Type.Privilege_New))
            {
                MessageFacade.Show(MessageFacade.privilege_no_access, LabelFacade.sy_copy, MessageBoxButtons.OK, MessageBoxIcon.Information);
                SessionLogFacade.Log(Type.Priority_Information, Type.Module_Branch, Type.Log_NoAccess, "Copy: No access");
                return;
            }
            Id = 0;
            if (IsExpand) picExpand_Click(sender, e);
            txtAccountNo.Focus();
            LockControls(false);
            cboBranch_SelectedIndexChanged(null, null);
            SessionLogFacade.Log(Type.Priority_Information, Type.Module_Branch, Type.Log_Copy, "Copy from Id=" + dgvList.Id + "Code=" + txtAccountNo.Text);
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
            var lInfo = LoanFacade.GetLock(Id);
            if (lInfo.Locked)
            {
                string msg = string.Format(MessageFacade.lock_currently, lInfo.Lock_By, lInfo.Lock_At);
                if (!Privilege.CanAccess(Type.Function_IC_Unit_Measure, "O"))
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
                LoanFacade.SetStatus(Id, status);
            }
            catch (Exception ex)
            {
                MessageFacade.Show(MessageFacade.error_active_inactive + ex.Message, MessageFacade.active_inactive, MessageBoxButtons.OK, MessageBoxIcon.Error);
                ErrorLogFacade.Log(ex);
            }
            RefreshGrid();
            SessionLogFacade.Log(Type.Priority_Caution, Type.Module_Branch, status == Type.RecordStatus_InActive ? Type.Log_Inactive : Type.Log_Active, "Id=" + dgvList.Id + ", Code=" + txtAccountNo.Text);
        }

        private void btnUnlock_Click(object sender, EventArgs e)
        {
            if (!Privilege.CanAccess(Type.Function_IC_Unit_Measure, Type.Privilege_Update))
            {
                MessageFacade.Show(MessageFacade.privilege_no_access, LabelFacade.sy_button_unlock, MessageBoxButtons.OK, MessageBoxIcon.Information);
                SessionLogFacade.Log(Type.Priority_Information, Type.Module_Branch, Type.Log_NoAccess, "Copy: No access");
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
                LoanFacade.DecrementSrNo(cboFrequencyUnit.SelectedValue.ToString());
                LockControls(true);
                dgvList.Focus();
                try
                {
                    LoanFacade.ReleaseLock(dgvList.Id);
                }
                catch (Exception ex)
                {
                    MessageFacade.Show(MessageFacade.error_unlock + "\r\n" + ex.Message, LabelFacade.sy_unlock, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ErrorLogFacade.Log(ex);
                    return;
                }
                if (dgvList.CurrentRow != null && !dgvList.CurrentRow.Selected)
                    dgvList.CurrentRow.Selected = true;
                SessionLogFacade.Log(Type.Priority_Information, Type.Module_Branch, Type.Log_Unlock, "Unlock cancel. Id=" + dgvList.Id + ", Code=" + txtAccountNo.Text);
                btnUnlock.ToolTipText = "Unlock (Ctrl+L)";
                IsDirty = false;
                return;
            }
            // Unlock
            if (Id == 0) return;
            try
            {
                var lInfo = LoanFacade.GetLock(Id);

                if (lInfo.Locked) // Check if record is locked
                {
                    string msg = string.Format(MessageFacade.lock_currently, lInfo.Lock_By, lInfo.Lock_At);
                    if (!Privilege.CanAccess(Type.Function_IC_Unit_Measure, "O"))
                    {
                        MessageFacade.Show(msg, LabelFacade.sy_unlock, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else
                        if (MessageFacade.Show(msg + "\r\n" + MessageFacade.lock_override, LabelFacade.sy_unlock, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                            SessionLogFacade.Log(Type.Priority_Caution, Type.Module_Branch, Type.Log_Lock, "Override lock. Id=" + dgvList.Id + ", Code=" + txtAccountNo.Text);
                        else
                            return;
                }
                txtMaturity.SelectionStart = txtMaturity.Text.Length;
                txtMaturity.Focus();
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
                LoanFacade.Lock(dgvList.Id, txtAccountNo.Text);
            }
            catch (Exception ex)
            {
                MessageFacade.Show(MessageFacade.error_lock + "\r\n" + ex.Message, LabelFacade.sy_lock, MessageBoxButtons.OK, MessageBoxIcon.Error);
                ErrorLogFacade.Log(ex);
                return;
            }
            SessionLogFacade.Log(Type.Priority_Information, Type.Module_Branch, Type.Log_Lock, "Locked. Id=" + dgvList.Id + ", Code=" + txtAccountNo.Text);
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
            //todo: work on this
            // Set config values
            //if (!IsExpand)
            //    ConfigFacade.ic_unit_measure_splitter_distance = splitContainer1.SplitterDistance;
            //ConfigFacade.ic_unit_measure_location = Location;
            //ConfigFacade.ic_unit_measure_window_state = (int)WindowState;
            //if (WindowState == FormWindowState.Normal) ConfigFacade.ic_unit_measure_size = Size;
        }

        private void txtCode_Leave(object sender, EventArgs e)
        {
            //// Check if entered code already exists
            //if (txtNo.ReadOnly) return;
            //if (LoanFacade.Exists(txtNo.Text.Trim()))
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
            LoanFacade.Export();
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

        private void cboBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboFrequencyUnit.SelectedIndex == -1 || btnNew.Enabled) return;
            txtAccountNo.Text = LoanFacade.GetNextSrNo(cboFrequencyUnit.SelectedValue.ToString()); //todo: Format No; from table
        }

        private void txtMaturity_Enter(object sender, EventArgs e)
        {
            CalculateDates();
        }

        private void btnSchedule_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
            GenerateSchedule();
        }

        private void chkNeverOn_CheckedChanged(object sender, EventArgs e)
        {
            bool b = (!chkSaturday.Checked && !chkSunday.Checked && !chkHoliday.Checked);
            cboMove.Enabled = !b;
            if (!chkHoliday.Enabled)
                cboMove.Enabled = false;
            else
                lblOnNonWorkingDay.Enabled = !b;
            //if (!cboMove.Enabled) cboMove.SelectedValue = "";
        }
    }
}