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
        static string Username = App.session.Username;

        static Config _sy_customer_no_format = new Config("", Util.GetMemberName(() => _sy_customer_no_format), "0000", "Customer number format");
        static Config _sy_select_limit = new Config("", Util.GetMemberName(() => _sy_select_limit), "1000", "Maximum number of row [1000] display in data grid");
        static Config _sy_toolbar_icon_display_type = new Config(Username, Util.GetMemberName(() => _sy_toolbar_icon_display_type), "IT", "Icon display type. [IT]=ImageAndText, I=Image, T=Text");
        static Config _sy_export_delimiter = new Config(Username, Util.GetMemberName(() => _sy_export_delimiter), ",", "Export delimiter [,]");
        static Config _sy_export_open_file_after = new Config(Username, Util.GetMemberName(() => _sy_export_open_file_after), "Y", "Open file after export. [Y]=Yes or N=No");
        static Config _sy_code_casing = new Config("", Util.GetMemberName(() => _sy_code_casing), "U", "Code character casing. [U]=Upper, L=Lower or N=Normal");
        static Config _sy_code_max_length = new Config("", Util.GetMemberName(() => _sy_code_max_length), "15", "Maximum length of code [15]");
        static Config _sy_language = new Config(Username, Util.GetMemberName(() => _sy_language), "ENG", "Language. e.g ENG or KHM");

        static Config _ic_location_spitter_distance = new Config(Username, Util.GetMemberName(() => _ic_location_spitter_distance), "207", "Data grid splitter distance [228]");
        static Config _ic_location_window_state = new Config(Username, Util.GetMemberName(() => _ic_location_window_state), "0", "Window state. Normal, Maximize and Minimize");
        static Config _ic_location_location = new Config(Username, Util.GetMemberName(() => _ic_location_location), "-1, -1", "Window location");
        static Config _ic_location_size = new Config(Username, Util.GetMemberName(() => _ic_location_size), "1024, 601", "Form size [1024, 601]");

        static Config _ic_category_spitter_distance = new Config(Username, Util.GetMemberName(() => _ic_category_spitter_distance), "207", "Data grid splitter distance [228]");
        static Config _ic_category_window_state = new Config(Username, Util.GetMemberName(() => _ic_category_window_state), "0", "Window state. Normal, Maximize and Minimize");
        static Config _ic_category_location = new Config(Username, Util.GetMemberName(() => _ic_category_location), "-1, -1", "Window location");
        static Config _ic_category_size = new Config(Username, Util.GetMemberName(() => _ic_category_size), "1024, 601", "Form size [1024, 601]");

        static Config _ic_unit_measure_spitter_distance = new Config(Username, Util.GetMemberName(() => _ic_unit_measure_spitter_distance), "207", "Data grid splitter distance [228]");
        static Config _ic_unit_measure_window_state = new Config(Username, Util.GetMemberName(() => _ic_unit_measure_window_state), "0", "Window state. Normal, Maximize and Minimize");
        static Config _ic_unit_measure_location = new Config(Username, Util.GetMemberName(() => _ic_unit_measure_location), "-1, -1", "Window location");
        static Config _ic_unit_measure_size = new Config(Username, Util.GetMemberName(() => _ic_unit_measure_size), "1024, 601", "Form size [1024, 601]");

        // SQL export
        static Config _sy_sql_export_location = new Config("", Util.GetMemberName(() => _sy_sql_export_location),
            "id \"Id\", branch_code \"Branch Code\", code \"Code\", description \"Description\", type \"Type\", address \"Address\", name \"Contact Name\", phone \"Phone\", fax \"Fax\", " +
            "email \"Email\", note \"Note\", status \"Status\", insert_by \"Inserted By\", insert_at \"Inserted At\", change_by \"Changed By\", change_at \"Changed At\"", "");
        static Config _sy_sql_export_customer = new Config("", Util.GetMemberName(() => _sy_sql_export_customer),
            "id \"Id\", code \"Code\", description \"Description\", note \"Note\", status \"Status\", insert_by \"Inserted By\", insert_at \"Inserted At\", change_by \"Changed By\", change_at \"Changed At\"", "");
        static Config _sy_sql_export_branch = new Config("", Util.GetMemberName(() => _sy_sql_export_branch),
                    "*", "");


        public static string sy_customer_no_format
        {
            get { return _sy_customer_no_format.Value; }
            set { _sy_customer_no_format.Value = value; }
        }

        public static int sy_select_limit
        {
            get { return _sy_select_limit.ValueInt; }
            set { _sy_select_limit.Value = value.ToString(); }
        }

        public static string sy_toolbar_icon_display_type
        {
            get { return _sy_toolbar_icon_display_type.Value; }
            set { _sy_toolbar_icon_display_type.Value = value; }
        }

        public static string sy_export_delimiter
        {
            get { return _sy_export_delimiter.Value; }
            set { _sy_export_delimiter.Value = value; }
        }

        public static bool sy_export_open_file_after
        {
            get { return _sy_export_open_file_after.ValueBool; }
            set { _sy_export_open_file_after.Value = value == true ? "Y" : "N"; }
        }

        public static string sy_code_casing
        {
            get { return _sy_code_casing.Value; }
            set { _sy_export_open_file_after.Value = value; }
        }

        public static int sy_code_max_length
        {
            get { return _sy_code_max_length.ValueInt; }
            set { _sy_code_max_length.Value = value.ToString(); }
        }

        public static string sy_language
        {
            get { return _sy_language.Value; }
            set { _sy_language.Value = value; }
        }

        // ic_location
        public static int ic_location_splitter_distance
        {
            get { return _ic_location_spitter_distance.ValueInt; }
            set { _ic_location_spitter_distance.Value = value.ToString(); }
        }

        public static int ic_location_window_state
        {
            get { return _ic_location_window_state.ValueInt; }
            set { _ic_location_window_state.Value = value.ToString(); }
        }

        public static Point ic_location_location
        {
            get { return _ic_location_location.ValuePoint; }
            set
            {
                var val = Util.RemoveCharacters(value.ToString(), "{}XY= ");
                _ic_location_location.Value = val;
            }
        }

        public static Size ic_location_size
        {
            get { return _ic_location_size.ValueSize; }
            set
            {
                var val = Util.RemoveCharacters(value.ToString().ToUpper(), "{}WIDTHEG= ");
                _ic_location_size.Value = val;
            }
        }

        // ic_category
        public static int ic_category_splitter_distance
        {
            get { return _ic_category_spitter_distance.ValueInt; }
            set { _ic_category_spitter_distance.Value = value.ToString(); }
        }

        public static int ic_category_window_state
        {
            get { return _ic_category_window_state.ValueInt; }
            set { _ic_category_window_state.Value = value.ToString(); }
        }

        public static Point ic_category_location
        {
            get { return _ic_category_location.ValuePoint; }
            set
            {
                var val = Util.RemoveCharacters(value.ToString(), "{}XY= ");
                _ic_category_location.Value = val;
            }
        }

        public static Size ic_category_size
        {
            get { return _ic_category_size.ValueSize; }
            set
            {
                var val = Util.RemoveCharacters(value.ToString().ToUpper(), "{}WIDTHEG= ");
                _ic_category_size.Value = val;
            }
        }

        // ic_unit_measure
        public static int ic_unit_measure_splitter_distance
        {
            get { return _ic_unit_measure_spitter_distance.ValueInt; }
            set { _ic_unit_measure_spitter_distance.Value = value.ToString(); }
        }

        public static int ic_unit_measure_window_state
        {
            get { return _ic_unit_measure_window_state.ValueInt; }
            set { _ic_unit_measure_window_state.Value = value.ToString(); }
        }

        public static Point ic_unit_measure_location
        {
            get { return _ic_unit_measure_location.ValuePoint; }
            set
            {
                var val = Util.RemoveCharacters(value.ToString(), "{}XY= ");
                _ic_unit_measure_location.Value = val;
            }
        }

        public static Size ic_unit_measure_size
        {
            get { return _ic_unit_measure_size.ValueSize; }
            set
            {
                var val = Util.RemoveCharacters(value.ToString().ToUpper(), "{}WIDTHEG= ");
                _ic_unit_measure_size.Value = val;
            }
        }

        // SQL Export
        public static string sy_sql_export_customer
        {
            get { return _sy_sql_export_customer.Value; }
            set { _sy_sql_export_customer.Value = value; }
        }

        public static string sy_sql_export_location
        {
            get { return _sy_sql_export_location.Value; }
            set { _sy_sql_export_location.Value = value; }
        }

        public static string sy_sql_export_branch
        {
            get { return _sy_sql_export_branch.Value; }
            set { _sy_sql_export_branch.Value = value; }
        }

        // Save configs back to table
        public static void SaveAll()
        {
            _sy_select_limit.Save();
            _sy_toolbar_icon_display_type.Save();
            _sy_export_delimiter.Save();
            _sy_export_open_file_after.Save();
            _sy_code_casing.Save();

            _ic_location_spitter_distance.Save();
            _ic_location_window_state.Save();
            _ic_location_location.Save();
            _ic_location_size.Save();

            _ic_category_spitter_distance.Save();
            _ic_category_window_state.Save();
            _ic_category_location.Save();
            _ic_category_size.Save();

            _ic_unit_measure_spitter_distance.Save();
            _ic_unit_measure_window_state.Save();
            _ic_unit_measure_location.Save();
            _ic_unit_measure_size.Save();
            //todo: repeated; should loop thru all auto.
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
            var language = ConfigFacade.sy_language;
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
            var language = ConfigFacade.sy_language;
            var sql = SqlFacade.SqlSelect(TableName, "value", "code = lower(:code) and language = :language");
            var message = SqlFacade.Connection.ExecuteScalar<string>(sql, new { code, language });
            if (message == null)
                ErrorLogFacade.Log("Message: code=" + code + " not exist");
            return message;
        }
    }
}