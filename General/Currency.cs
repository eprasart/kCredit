using System;
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
    class Currency
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        // todo: more here
        public string Note { get; set; }
        public string Status { get; set; }
        public string Insert_By { get; set; }
        public DateTime? Insert_At { get; set; }
        public string Change_By { get; set; }
        public DateTime? Change_At { get; set; }
    }

    static class CurrencyFacade
    {
        public static readonly string TableName = "currency";
        private static string Currency;
        private static double RoundUnit; // Temporary store the round unit of the currency
        private static string RoundRule;
        public static string Format;

        public static void LoadSetting(string currency)
        {
            Currency = currency;

            var sql = SqlFacade.SqlSelect(TableName, "round_unit, round_rule, format", "code = :code");
            //RoundUnit =SqlFacade.Connection.ExecuteScalar<double>(sql, new { code = currency });
            var dr = SqlFacade.Connection.ExecuteReader(sql, new { code = currency });
            dr.Read();
            RoundRule = dr["round_rule"].ToString();
            RoundUnit = double.Parse(dr["round_unit"].ToString());
            Format = dr["format"].ToString();
            dr.Close();
        }

        public static double Round(double value)
        {
            double result = 0;
            switch (RoundRule)
            {
                case "R":
                    result = RoundUpDown(value);
                    break;
                case "U":
                    result = RoundUp(value);
                    break;
                case "D":
                    result = RoundDown(value);
                    break;
            }
            return result;
        }

        private static double RoundUp(double value)
        {
            double result = Math.Ceiling(value / RoundUnit) * RoundUnit;
            return result;
        }

        private static double RoundDown(double value)
        {
            double result = Math.Floor(value / RoundUnit) * RoundUnit;
            return result;
        }

        private static double RoundUpDown(double value)
        {
            double result = Math.Round(value / RoundUnit) * RoundUnit;
            return result;
        }

        public static DataTable GetDataTable(string filter = "", string status = "")
        {
            var sql = SqlFacade.SqlSelect(TableName, "id, code, name", "1 = 1");
            if (status.Length == 0)
                sql += " and status <> '" + Constant.RecordStatus_Deleted + "'";
            else
                sql += " and status = '" + status + "'";
            if (filter.Length > 0)
                sql += " and (" + SqlFacade.SqlILike("code, name") + ")";
            sql += "\norder by code\nlimit " + ConfigFacade.Select_Limit;

            var cmd = new NpgsqlCommand(sql);
            if (filter.Length > 0)
                cmd.Parameters.AddWithValue(":filter", "%" + filter + "%");

            return SqlFacade.GetDataTable(cmd);
        }

        public static long Save(Branch m)
        {
            string sql = "";
            if (m.Id == 0)
            {
                m.Insert_By = App.session.Username;
                sql = SqlFacade.SqlInsert(TableName, "code, name, parent_branch, currency, address, province, district, commune, village, note, insert_by", "", true);
                m.Id = SqlFacade.Connection.ExecuteScalar<long>(sql, m);
                CustomerFacade.SaveSrNo(m.Code);
            }
            else
            {
                m.Change_By = App.session.Username;
                sql = SqlFacade.SqlUpdate(TableName, "code, name, parent_branch, currency, address, province, district, commune, village, note, change_by, change_at, change_no", "change_at = now(), change_no = change_no + 1", "id = :id");
                SqlFacade.Connection.Execute(sql, m);
                ReleaseLock(m.Id);  // Unlock
            }
            return m.Id;
        }

        public static Branch Select(long Id)
        {
            var sql = SqlFacade.SqlSelect(TableName, "*", "id = :id");
            return SqlFacade.Connection.Query<Branch>(sql, new { Id }).FirstOrDefault();
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
                bExists = SqlFacade.Connection.ExecuteScalar<bool>(sql, new { Id, Status = Constant.RecordStatus_Deleted, Code });
            }
            catch (Exception ex)
            {
                MessageFacade.Show(MessageFacade.error_query + "\r\n" + ex.Message, LabelFacade.sys_branch, MessageBoxButtons.OK, MessageBoxIcon.Error);
                ErrorLogFacade.Log(ex, "Exists");
            }
            return bExists;
        }

        public static void Export()
        {
            var cols = "*";
            cols = ConfigFacade.Get(Constant.Sql_Export + TableName, cols);
            string sql = SqlFacade.SqlSelect(TableName, cols, "status <> '" + Constant.RecordStatus_Deleted + "'", "code");
            SqlFacade.ExportToCSV(sql);
        }
    }
}