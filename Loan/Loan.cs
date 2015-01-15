﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Npgsql;
using Dapper;
using kCredit.SYS;
using kCredit.SM;
using System.Windows.Forms;

namespace kCredit
{
    class Loan
    {
        public long Id { get; set; }
        public string Account_No { get; set; }
        public string Customer_No { get; set; }
        public string Branch_Code { get; set; }
        public string Frequency_Unit { get; set; }
        public int Frequency { get; set; }
        public int Installment_No { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; }
        public double Interest_Rate { get; set; }
        public string Calculation_Method { get; set; }
        public DateTime Disburse_Date { get; set; }
        public DateTime First_Installment_Date { get; set; }
        public DateTime Maturity_Mate { get; set; }
        public string Never_On { get; set; }
        public string Non_Working_Day_Move { get; set; }
        public string Purpose { get; set; }
        public string Payment_Site { get; set; }
        public int Credit_Agent_Id { get; set; }
        public string Account_Status { get; set; }
        public string Note { get; set; }
        public string Status { get; set; }
        public string Insert_By { get; set; }
        public DateTime? Insert_At { get; set; }
        public string Change_By { get; set; }
        public DateTime? Change_At { get; set; }
    }

    static class LoanFacade
    {
        public static readonly string TableName = "loan";

        public static DataTable GetDataTable(string filter = "", string status = "")
        {
            var sql = "id, account_no, customer_no, branch_code, frequency_unit, frequency, installment_no, amount, currency, interest_rate";
            sql = SqlFacade.SqlSelect(TableName, sql, "1 = 1");
            if (status.Length == 0)
                sql += " and status <> '" + Type.RecordStatus_Deleted + "'";
            else
                sql += " and status = '" + status + "'";
            if (filter.Length > 0)
                sql += " and (" + SqlFacade.SqlILike("account_no, customer_no, branch_code") + ")";
            sql += "\norder by account_no\nlimit " + ConfigFacade.sy_select_limit;

            var cmd = new NpgsqlCommand(sql);
            if (filter.Length > 0)
                cmd.Parameters.AddWithValue(":filter", "%" + filter + "%");

            return SqlFacade.GetDataTable(cmd);
        }

        public static long Save(Loan m)
        {
            string sql = "";
            if (m.Id == 0)
            {
                m.Insert_By = App.session.Username;
                m.Branch_Code = App.session.Branch_Code;
                  sql = "account_no, customer_no, branch_code, frequency_unit, frequency, installment_no, amount, currency, interest_rate, calculation_method, " +
                    "disburse_date, first_installment_date, maturity_date, never_on, non_working_day_move, purpose, payment_site, credit_agent_id, " +
                    "note, insert_by";
                sql = SqlFacade.SqlInsert(TableName, sql, "", true);
              
                m.Id = SqlFacade.Connection.ExecuteScalar<long>(sql, m);
            }
            else
            {
                m.Change_By = App.session.Username;
                sql = "frequency_unit, frequency, installment_no, amount, currency, interest_rate, calculation_method, disburse_date, first_installment_date, " +
                    "maturity_date, never_on, non_working_day_move, purpose, payment_site, credit_agent_id, note, change_by, change_at, change_no";
                sql = SqlFacade.SqlUpdate(TableName, sql, "change_at = now(), change_no = change_no + 1", "id = :id");
                SqlFacade.Connection.Execute(sql, m);
                ReleaseLock(m.Id);  // Unlock
            }
            return m.Id;
        }

        public static Loan Select(long Id)
        {
            var sql = SqlFacade.SqlSelect(TableName, "*", "id = :id");
            return SqlFacade.Connection.Query<Loan>(sql, new { Id }).FirstOrDefault();
        }

        public static void SetStatus(long Id, string status)
        {
            var sql = SqlFacade.SqlUpdate(TableName, "status, change_by, change_at", "change_at = now()", "id = :id");
            SqlFacade.Connection.Execute(sql, new { status, Change_By = App.session.Username, Id });
        }

        public static Lock GetLock(long Id)
        {
            return LockFacade.Select(TableName, Id);
        }

        public static void Lock(long Id, string code)
        {
            var m = new Lock { Table_Name = TableName, Lock_Id = Id, Ref = code };
            LockFacade.Save(m);
        }

        public static void ReleaseLock(long Id)
        {
            LockFacade.Delete(TableName, Id);
        }

        public static bool Exists(string account_no, long Id = 0)
        {
            var sql = SqlFacade.SqlExists(TableName, "id <> :id and status <> :status and account_no = :account_no");
            var bExists = false;
            try
            {
                bExists = SqlFacade.Connection.ExecuteScalar<bool>(sql, new { Id, Status = Type.RecordStatus_Deleted, No = account_no });
            }
            catch (Exception ex)
            {
                MessageFacade.Show(MessageFacade.error_query + "\r\n" + ex.Message, LabelFacade.sy_customer, MessageBoxButtons.OK, MessageBoxIcon.Error);
                ErrorLogFacade.Log(ex, "Exists");
            }
            return bExists;
        }

        public static void Export()
        {
            string sql = SqlFacade.SqlSelect(TableName, ConfigFacade.sy_sql_export_customer, "status <> '" + Type.RecordStatus_Deleted + "'", "account_no");
            SqlFacade.ExportToCSV(sql);
        }

        public static void SaveSrNo(string branch_code, long running_no = 1)
        {
            var sql = SqlFacade.SqlInsert("customer_srno", "branch_code, running_no", "");
            SqlFacade.Connection.Execute(sql, new { branch_code, running_no });
        }

        public static void IncrementSrNo(string branch_code)
        {
            var sql = SqlFacade.SqlUpdate("customer_srno", "running_no", "running_no = running_no + 1", "branch_code = :branch_code");
            SqlFacade.Connection.Execute(sql, new { branch_code });
        }

        public static void DecrementSrNo(string branch_code)    // When cancel; //todo: but when multi user???
        {
            var sql = SqlFacade.SqlUpdate("customer_srno", "running_no", "running_no = running_no - 1", "branch_code = :branch_code");
            SqlFacade.Connection.Execute(sql, new { branch_code });
        }

        public static string GetNextSrNo(string branch_code)
        {
            var sql = SqlFacade.SqlSelect("customer_srno", "running_no", "branch_code = :branch_code");
            var lNo = SqlFacade.Connection.ExecuteScalar<long>(sql, new { branch_code });
            if (lNo != 0)
                IncrementSrNo(branch_code);
            else
            {
                SaveSrNo(branch_code, 2);
                lNo = 1;
            }
            var sNo = branch_code + "-" + lNo.ToString(ConfigFacade.sy_customer_no_format);
            return sNo;
        }
    }
}