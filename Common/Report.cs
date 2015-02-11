using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace kCredit
{
    public partial class frmReport : Form
    {
        //Fields
        private string _FileName;

        //Property
        public string FileName
        {
            get
            {
                return System.IO.Path.Combine(Application.StartupPath, "Report", _FileName);
            }
            set
            {
                _FileName = value;
            }
        }
        
        public DataTable ReportSource { get; set; }

        public void PreviewReport()
        {
            this.Cursor = Cursors.WaitCursor;
            rptViewer.LocalReport.ReportPath = FileName;
            ReportDataSource rds = new ReportDataSource("DataSet1", ReportSource);
            //rds.Name = "DataSet1";
            //rds.Value = ReportSource;
            rptViewer.LocalReport.DataSources.Add(rds);
            rptViewer.RefreshReport();
            //WindowState = FormWindowState.Maximized;
            this.Show();
            this.Cursor = Cursors.Default;
        }

        //if report consist of parameter(s) use this method to add
        public void SetParameters(params ReportParameter[] rptParam)
        {
            rptViewer.LocalReport.ReportPath = FileName;

            foreach (ReportParameter p in rptParam)
            {
                rptViewer.LocalReport.SetParameters(p);
            }
        }

        public frmReport()
        {
            InitializeComponent();
        }
        public frmReport(string Title):this()
        {
            Text = Title;
        }

        private void frmReport_KeyDown(object sender, KeyEventArgs e)
        {
            // Ctrl+P => Print
            if (e.Control && e.KeyCode == Keys.P)
            {
                this.rptViewer.PrintDialog();
            }
        }

        private void frmReport_Load(object sender, EventArgs e)
        {

        }

    }
}
