using System;
using Accounting_System.PeriodServiceReference;
using Accounting_System.SettleServiceReference;


namespace Accounting_System
{
    public partial class Settle : System.Web.UI.Page
    {
        public int _userID;
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckLimit.CheckPage(Request["menuid"]);
            if(!IsPostBack)
            {
                loadPeriod();
                int.TryParse(Session["UserID"].ToString(), out _userID);
            }
        }

        private void loadPeriod()
        {
            try
            {
                var _psc = new PeriodServiceClient();
                string test = _psc.GetClosedPeriod()[0].PeriodNo;
                string _currentPeriod = _psc.GetCurrentPeriod()[0].PeriodNo;
                lblCurrentPeriod.Text = _currentPeriod;
            }
            catch
            {

            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            var _ssc = new SettleServiceClient();
            _ssc.CloseEntry(int.Parse(Session["UserID"].ToString()));
            loadPeriod();

        }

        protected void btnReverse_Click(object sender, EventArgs e)
        {
            var _ssc = new SettleServiceClient();
            _ssc.ReverseClosing();
            loadPeriod();
        }
    }
}