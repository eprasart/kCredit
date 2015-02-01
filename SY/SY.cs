using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Npgsql;
using Dapper;
using System.Drawing;
using System.Windows.Forms;
using System.Linq.Expressions;

namespace kCredit
{
    public static class SY
    {

    }

    class ConfigItem
    {
        const string TableName = "sy_config";
        private string _value;

        public long Id { private get; set; }
        public string Code { get; set; }
        public string Username { get; set; }
        public bool Changed { private get; set; }

        public ConfigItem()
        { }

        public ConfigItem(string code, string username, string defaultValue)
        {
            Code = code;
            Username = username;
            _value = defaultValue;    // Not "Value = defaultValue" to avoid "Changed = True"
            Get();
            //Changed = false;
        }

        public string Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    Changed = true;
                }
            }
        }

        public void Save()
        {
            if (!Changed) return;
            var sql = SqlFacade.SqlUpdate(TableName, "value", "value = :value", "id = :id");
            Id = SqlFacade.Connection.ExecuteScalar<long>(sql, new { Value, Id });
        }

        private void Get()
        {
            var sWhere = "code ~* :code";
            if (Username.Length > 0)
                sWhere = "username ~* :username and " + sWhere;
            var sql = SqlFacade.SqlSelect(TableName, "id, value", sWhere);

            ConfigItem result = null;
            if (Username.Length > 0)
                result = SqlFacade.Connection.Query<ConfigItem>(sql, new { Username, Code }).FirstOrDefault();
            else
                result = SqlFacade.Connection.Query<ConfigItem>(sql, new { Code }).FirstOrDefault();
            if (result == null)
                Add();
            else
            {
                Id = result.Id;
                _value = result.Value;
            }
        }

        private void Add()
        {
            var sql = SqlFacade.SqlInsert(TableName, "username, code, value", "", true);
            Id = SqlFacade.Connection.ExecuteScalar<long>(sql, new { Username, Code, Value });
        }
    }

    class Config
    {
        const string TableName = "sy_config";

        private string _value;

        private bool Changed { get; set; }

        public long Id { get; set; }
        public string Username { get; set; }
        public string Code { get; set; }

        public string Value
        {
            get { return _value; }
            set
            {
                if (_value != value && value != null)
                {
                    _value = value;
                    Changed = true;
                }
            }
        }

        public string Note { get; set; }
        public String Status { get; set; }

        public Config() { }

        public Config(string username, string code, string defaultValue, string note)
        {
            Username = username;
            Code = code.Substring(1); // skip the _
            Value = defaultValue;
            Note = note;
            Get();
            Changed = false;
        }

        public override string ToString()
        {
            return Value;
        }

        public int ValueInt
        {
            get { return int.Parse(Value); }
        }

        public bool ValueBool
        {
            get { return Value == "Y" || Value == "T" ? true : false; }
        }

        public Point ValuePoint
        {
            get
            {
                if (Value == "") return new Point(-1, -1);
                Value = Util.RemoveCharacters(Value.ToUpper(), "{}XY= "); // X Y
                string[] coords = Value.Split(',');
                return new Point(int.Parse(coords[0]), int.Parse(coords[1]));
            }
        }

        public Size ValueSize
        {
            get
            {
                if (Value == "") return new Size(-1, -1);
                Value = Util.RemoveCharacters(Value.ToUpper(), "{}WIDTHEG= ");    // WIDTH HEIGH
                string[] coords = Value.Split(',');
                return new Size(int.Parse(coords[0]), int.Parse(coords[1]));
            }
        }

        private void Add()
        {
            var sql = SqlFacade.SqlInsert(TableName, "username, code, value, note", "", true);
            Id = SqlFacade.Connection.ExecuteScalar<long>(sql, new { Username, Code, Value, Note });
        }

        private void Get()
        {
            var sWhere = "code ~* :code";
            if (Username.Length > 0)
                sWhere = "username ~* :username and " + sWhere;
            var sql = SqlFacade.SqlSelect(TableName, "id, value as value", sWhere);

            Config result = null;
            if (Username.Length > 0)
                result = SqlFacade.Connection.Query<Config>(sql, new { Username, Code }).FirstOrDefault();
            else
                result = SqlFacade.Connection.Query<Config>(sql, new { Code }).FirstOrDefault();
            if (result == null)
                Add();
            else
            {
                Id = result.Id;
                Value = result.Value;
            }
        }

        public void Save()
        {
            if (!Changed) return;
            var sql = SqlFacade.SqlUpdate(TableName, "value", "", "id = :id");
            try
            {
                SqlFacade.Connection.Execute(sql, new { Value, Id });
            }
            catch (Exception ex)
            {
                ErrorLogFacade.LogToFile(ex, "sql='" + sql + "')");
            }
        }
    }

    static class ConfigFacade
    {
        const string TableName = "sy_config";
        static Dictionary<string, ConfigItem> configList = new Dictionary<string, ConfigItem>();

        static string Username = App.session.Username;

        public static string Language = Get(Constant.Language, "ENG");
        public static string Select_Limit = Get(Constant.Select_Limit, "1000");
        public static string Code_Casing = Get(Constant.Code_Casing, "N");
        public static int Code_Max_Length = GetInt(Constant.Code_Max_Length, "15");

        public static string Toolbar_Icon_Display_Type = Get(Constant.Toolbar_Icon, Username, "IT");
        public static string Export_Delimiter = Get(Constant.Code_Max_Length, ",");
        public static bool Export_Open_File_After = Get(Constant.Code_Max_Length, "Y") == "Y";

        public static void LoadConfig()
        {

        }

        private static void Add(string username, string code, string value, string note = "")
        {
            var sql = SqlFacade.SqlInsert(TableName, "username, code, value, note", "", true);
            SqlFacade.Connection.ExecuteScalar<long>(sql, new { username, code, value, note });
        }

        public static string Get(string code, string username, string defaultValue = "")
        {
            ConfigItem s;
            if (!configList.TryGetValue(code, out s))
            {
                s = new ConfigItem(code, username, defaultValue);
                configList.Add(code, s);
            }
            return s.Value;
        }

        public static void Set(string code, string value)
        {
            if (configList.ContainsKey(code))
                if (configList[code].Value != value)
                    configList[code].Value = value;
        }

        public static void Set(string code, int value)
        {
            Set(code, value.ToString());
        }

        public static string Get(string code, string defaultValue = "")
        {
            return Get(code, "", defaultValue);
        }

        public static string GetUpper(string code, string defaultValue = "")
        {
            return Get(code, defaultValue).ToUpper();
        }

        public static int GetInt(string code, string defaultValue = "0")
        {
            return int.Parse(Get(code, defaultValue));
        }

        public static int GetSplitterDistance(string frmName, string defaultValue = "230")
        {
            return int.Parse(Get(frmName + Constant.Splitter_Distance, Username, defaultValue));
        }

        public static FormWindowState GetWindowState(string code, string defaultValue = "")
        {
            return (FormWindowState)int.Parse(Get(code, App.session.Username, defaultValue));
        }

        public static void Set(string code, FormWindowState value)
        {
            string s = ((int)value).ToString();
            Set(code, s);
        }

        public static Size GetSize(string code, string defaultValue = "")
        {
            string s = Get(code, App.session.Username, defaultValue);
            int width = -1, height = -1;
            if (s.Contains(","))
            {
                string[] dims = s.Split(',');
                int.TryParse(dims[0], out width);
                int.TryParse(dims[1], out height);
            }
            return new System.Drawing.Size(width, height);
        }

        public static void Set(string code, Size value)
        {
            string s = string.Format("{0}, {1}", value.Width, value.Height);
            Set(code, s);
        }

        public static Point GetPoint(string code, string defaultValue = "")
        {
            string s = Get(code, App.session.Username, defaultValue);
            int x = -1, y = -1;
            if (s.Contains(","))
            {
                string[] dims = s.Split(',');
                int.TryParse(dims[0], out x);
                int.TryParse(dims[1], out y);
            }
            return new System.Drawing.Point(x, y);
        }

        public static void Set(string code, Point value)
        {
            string s = string.Format("{0}, {1}", value.X, value.Y);
            Set(code, s);
        }

        // Save [only] changes to database
        public static void Save()
        {
            foreach (KeyValuePair<string, ConfigItem> p in configList)
            {
                p.Value.Save();
            }
        }
    }

    class LabelFacade
    {
        const string TableName = "sy_label";

        public static readonly string sy_msg_prefix = "- ";

        public static string sy_location;
        public static string sy_customer;
        public static string sy_branch;

        public static string sy_cancel;
        public static string sy_close;
        public static string sy_copy;
        public static string sy_delete;

        public static string sy_lock;
        public static string sy_new;
        public static string sy_save;
        public static string sy_unlock;

        public static string sy_button_new;
        public static string sy_button_copy;
        public static string sy_button_cancel;
        public static string sy_button_unlock;
        public static string sy_button_save;
        public static string sy_button_save_new;
        public static string sy_button_active;
        public static string sy_button_inactive;
        public static string sy_button_delete;
        public static string sy_button_mode;
        public static string sy_export;

        public static string sy_button_find;
        public static string sy_button_clear;
        public static string sy_button_filter;

        // Message Box Buttons
        public static string sy_button_abort;
        public static string sy_button_retry;
        public static string sy_button_ignore;
        public static string sy_button_ok;
        public static string sy_button_yes;
        public static string sy_button_no;
        public static string sy_search_place_holder;

        public static void LoadSystemLabel()
        {
            //todo: recall when switching a language

            sy_location = GetLabel(Util.GetMemberName(() => sy_location));
            sy_customer = GetLabel(Util.GetMemberName(() => sy_customer));

            // sy
            sy_cancel = GetLabel(Util.GetMemberName(() => sy_cancel));
            sy_close = GetLabel(Util.GetMemberName(() => sy_close));
            sy_copy = GetLabel(Util.GetMemberName(() => sy_copy));
            sy_delete = GetLabel(Util.GetMemberName(() => sy_delete));
            sy_lock = GetLabel(Util.GetMemberName(() => sy_lock));
            sy_new = GetLabel(Util.GetMemberName(() => sy_new));
            sy_save = GetLabel(Util.GetMemberName(() => sy_save));
            sy_unlock = GetLabel(Util.GetMemberName(() => sy_unlock));
            sy_search_place_holder = GetLabel(Util.GetMemberName(() => sy_search_place_holder));

            // Buttons            
            sy_button_new = GetLabel(Util.GetMemberName(() => sy_button_new));
            sy_button_copy = GetLabel(Util.GetMemberName(() => sy_button_copy));
            sy_button_cancel = GetLabel(Util.GetMemberName(() => sy_button_cancel));
            sy_button_unlock = GetLabel(Util.GetMemberName(() => sy_button_unlock));
            sy_button_save = GetLabel(Util.GetMemberName(() => sy_button_save));
            sy_button_save_new = GetLabel(Util.GetMemberName(() => sy_button_save_new));
            sy_button_active = GetLabel(Util.GetMemberName(() => sy_button_active));
            sy_button_inactive = GetLabel(Util.GetMemberName(() => sy_button_inactive));
            sy_button_delete = GetLabel(Util.GetMemberName(() => sy_button_delete));
            sy_button_mode = GetLabel(Util.GetMemberName(() => sy_button_mode));
            sy_export = GetLabel(Util.GetMemberName(() => sy_export));

            sy_button_find = GetLabel(Util.GetMemberName(() => sy_button_find));
            sy_button_clear = GetLabel(Util.GetMemberName(() => sy_button_clear));
            sy_button_filter = GetLabel(Util.GetMemberName(() => sy_button_filter));

            // Message Box Buttons
            sy_button_abort = GetLabel(Util.GetMemberName(() => sy_button_abort));
            sy_button_retry = GetLabel(Util.GetMemberName(() => sy_button_retry));
            sy_button_ignore = GetLabel(Util.GetMemberName(() => sy_button_ignore));
            sy_button_ok = GetLabel(Util.GetMemberName(() => sy_button_ok));
            sy_button_yes = GetLabel(Util.GetMemberName(() => sy_button_yes));
            sy_button_no = GetLabel(Util.GetMemberName(() => sy_button_no));
        }

        public static string GetLabel(string code)
        {
            var language = ConfigFacade.Language;
            var sql = SqlFacade.SqlSelect(TableName, "value", "code = lower(:code) and language = :language");
            var label = SqlFacade.Connection.ExecuteScalar<string>(sql, new { code, language });
            if (label == null)
                ErrorLogFacade.Log("Label: code=" + code + " not exist");
            return label;
        }
    }

    class MessageFacade
    {
        const string TableName = "sy_message";

        public static string active_inactive;
        public static string error_active_inactive;
        public static string error_retrieve_data;

        public static string delete_confirmation;
        public static string error_delete;
        public static string delete_locked;
        public static string lock_currently;
        public static string error_lock;
        public static string lock_override;
        public static string privilege_no_access;
        public static string proceed_confirmation;
        public static string error_load_record;
        public static string save_confirmation;
        public static string error_save;
        public static string error_unlock;
        public static string error_load_form;
        public static string error_query;
        public static string error_export;

        public static string code_already_exists;
        public static string code_not_empty;

        public static string email_not_valid;
        public static string location_type_not_empty;

        public static string export_exporting;
        public static string export_opening;
        public static string file_being_used_try_again;

        public static DialogResult Show(string msg, string title = "", MessageBoxButtons buttons = MessageBoxButtons.OK,
            MessageBoxIcon icon = MessageBoxIcon.Information, MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1)
        {
            var fMsg = new frmMsg(msg, title, buttons, icon, defaultButton);
            fMsg.Text = title;
            DialogResult dResult = DialogResult.OK;
            if (buttons == MessageBoxButtons.OK && icon == MessageBoxIcon.Information)
                fMsg.Show();
            else
                dResult = fMsg.ShowDialog();
            return dResult;
        }

        public static void Show(IWin32Window owner, ref frmMsg fMsg, string msg, string title)
        {
            if (fMsg == null || fMsg.IsDisposed)
            {
                fMsg = new frmMsg(msg, title);
                fMsg.Show(owner);
            }
            else
            {
                fMsg.Title = title;
                fMsg.Message = msg;
            }
            //if (fMsg.Visible == false) fMsg.Visible = true;
        }

        public static void LoadSystemMessage()
        {
            //todo: reload when language changed
            active_inactive = GetMessage(Util.GetMemberName(() => active_inactive));
            error_active_inactive = GetMessage(Util.GetMemberName(() => error_active_inactive));
            error_retrieve_data = GetMessage(Util.GetMemberName(() => error_retrieve_data));
            delete_confirmation = GetMessage(Util.GetMemberName(() => delete_confirmation));
            error_delete = GetMessage(Util.GetMemberName(() => error_delete));
            delete_locked = GetMessage(Util.GetMemberName(() => delete_locked));
            lock_currently = GetMessage(Util.GetMemberName(() => lock_currently));
            error_lock = GetMessage(Util.GetMemberName(() => error_lock));
            lock_override = GetMessage(Util.GetMemberName(() => lock_override));
            privilege_no_access = GetMessage(Util.GetMemberName(() => privilege_no_access));
            proceed_confirmation = GetMessage(Util.GetMemberName(() => proceed_confirmation));
            error_load_record = GetMessage(Util.GetMemberName(() => error_load_record));
            save_confirmation = GetMessage(Util.GetMemberName(() => save_confirmation));
            error_save = GetMessage(Util.GetMemberName(() => error_save));
            error_unlock = GetMessage(Util.GetMemberName(() => error_unlock));
            error_load_form = GetMessage(Util.GetMemberName(() => error_load_form));
            error_query = GetMessage(Util.GetMemberName(() => error_query));
            error_export = GetMessage(Util.GetMemberName(() => error_export));

            code_already_exists = GetMessage(Util.GetMemberName(() => code_already_exists));
            code_not_empty = GetMessage(Util.GetMemberName(() => code_not_empty));

            email_not_valid = GetMessage(Util.GetMemberName(() => email_not_valid));
            location_type_not_empty = GetMessage(Util.GetMemberName(() => location_type_not_empty));

            export_exporting = GetMessage(Util.GetMemberName(() => export_exporting));
            export_opening = GetMessage(Util.GetMemberName(() => export_opening));
            file_being_used_try_again = Util.EscapeNewLine(GetMessage(Util.GetMemberName(() => file_being_used_try_again)));
        }

        public static string GetMessage(string code)
        {
            var language = ConfigFacade.Language;
            var sql = SqlFacade.SqlSelect(TableName, "value", "code = lower(:code) and language = :language");
            var message = SqlFacade.Connection.ExecuteScalar<string>(sql, new { code, language });
            if (message == null)
            {
                ErrorLogFacade.Log("Message: code=" + code + " not exist");
                message = code;
            }
            return message;
        }
    }
}