using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kCredit
{
    class Validator
    {
        public static frmMsg fMsg = null;
        StringBuilder Msg = new StringBuilder();
        Control cFocus = null;
        public static Form frm = null;
        string codePrefix = "";

        public Validator(Form f, string codePre)
        {
            frm = f;
            codePrefix = codePre + "_";
        }

        public void Add(Control c, string messageCode)
        {
            Msg.AppendLine(LabelFacade.sy_msg_prefix + MessageFacade.GetMessage(codePrefix + messageCode));
            if (cFocus == null) cFocus = c;
        }

        public bool Show()
        {
            if (Msg.Length > 0)
            {
                MessageFacade.Show(frm, ref fMsg, Msg.ToString(), LabelFacade.sys_save);
                cFocus.Focus();
                return false;
            }
            return true;
        }

        public static void Close(Form f)
        {
            if (frm == f && fMsg != null) fMsg.Close();
        }
    }
}
