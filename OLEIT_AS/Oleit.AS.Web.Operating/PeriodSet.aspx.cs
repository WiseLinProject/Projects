using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Accounting_System.PeriodServiceReference;
using Oleit.AS.Service.DataObject;

namespace Accounting_System
{
    public partial class PeriodSet : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckLimit.CheckPage(Request["menuid"]);
            if (!IsPostBack)
            {
                loadPeriod();
            }
        }

        /// <summary>
        /// Initial Period repeater
        /// </summary>
        private void loadPeriod()
        {
            var _psr = new PeriodServiceClient();
            rptPeriod.DataSource = _psr.GetPeriods().ToList();
            rptPeriod.DataBind();
        }

        /// <summary>
        /// Set Period
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSet_Click(object sender, EventArgs e)
        {
            var _psr = new PeriodServiceClient();
            int _yesrValue = int.Parse(yearSet.Text);
            _psr.SetPeriod(_yesrValue);
            loadPeriod();
            Page.ClientScript.RegisterStartupScript(GetType(), "Success string", "Alert('Set Success !');", true);
        }
    }
}