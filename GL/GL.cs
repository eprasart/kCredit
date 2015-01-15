using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Npgsql;
using Dapper;
using kCredit.SYS;
using kCredit.SM;
using System.Windows.Forms;

namespace kCredit.GL
{
    class Account
    {
        public long Id { get; set; }
        public string Branch_Code { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }
        public String Status { get; set; }
        public string Insert_By { get; set; }
        public DateTime? Insert_At { get; set; }
        public string Change_By { get; set; }
        public DateTime? Change_At { get; set; }
    }

    static class AccountFacade
    {
        public static readonly string TableName = "ic_location";

        public static DataTable GetDataTable(string filter = "", string status = "")
        {
            var sql = SqlFacade.SqlSelect(TableName, "id, branch_code, code, description, type, name, phone, fax, email, address", "1 = 1");
            if (status.Length == 0)
                sql += " and status <> '" + Type.RecordStatus_Deleted + "'";
            else
                sql += " and status = '" + status + "'";
            if (filter.Length > 0)
                sql += " and (" + SqlFacade.SqlILike("code, description, phone, fax, email, address, note") + ")";
            sql += "\norder by code\nlimit " + ConfigFacade.sy_select_limit;

            var cmd = new NpgsqlCommand(sql);
            if (filter.Length > 0)
                cmd.Parameters.AddWithValue(":filter", "%" + filter + "%");

            return SqlFacade.GetDataTable(cmd);
        }

        public static long Save(Account m)
        {
            string sql = "";
            m.Branch_Code = App.session.Branch_Code;
            if (m.Id == 0)
            {
                m.Insert_By = App.session.Username;
                sql = SqlFacade.SqlInsert(TableName, "branch_code, code, description, type, address, name, phone, fax, email, note, insert_by", "", true);
                m.Id = SqlFacade.Connection.ExecuteScalar<long>(sql, m);
            }
            else
            {
                m.Change_By = App.session.Username;
                sql = SqlFacade.SqlUpdate(TableName, "branch_code, code, description, type, address, name, phone, fax, email, note, change_by, change_at, change_no", "change_at = now(), change_no = change_no + 1", "id = :id");
                SqlFacade.Connection.Execute(sql, m);
                ReleaseLock(m.Id);  // Unlock
            }
            return m.Id;
        }

        public static Account Select(long Id)
        {
            var sql = SqlFacade.SqlSelect(TableName, "*", "id = :id");
            return SqlFacade.Connection.Query<Account>(sql, new { Id }).FirstOrDefault();
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

        public static bool Exists(string Code, long Id = 0)
        {
            var sql = SqlFacade.SqlExists(TableName, "id <> :id and status <> :status and code = :code");
            var bExists = false;
            try
            {
                bExists = SqlFacade.Connection.ExecuteScalar<bool>(sql, new { Id, Status = Type.RecordStatus_Deleted, Code });
            }
            catch (Exception ex)
            {
                MessageFacade.Show(MessageFacade.error_query + "\r\n" + ex.Message, LabelFacade.sy_location, MessageBoxButtons.OK, MessageBoxIcon.Error); 
                ErrorLogFacade.Log(ex, "Exists");                
            }
            return bExists;
        }

        public static void Export()
        {
            string sql = SqlFacade.SqlSelect(TableName, "id \"Id\", branch_code \"Branch Code\", code \"Code\", description \"Description\", type \"Type\", address \"Address\", name \"Contact Name\", phone \"Phone\", fax \"Fax\", " +
                "email \"Email\", note \"Note\", status \"Status\", insert_by \"Inserted By\", insert_at \"Inserted At\", change_by \"Changed By\", change_at \"Changed At\"",
                "status <> '" + Type.RecordStatus_Deleted + "'", "code");
            SqlFacade.ExportToCSV(sql);
        }
    }
}