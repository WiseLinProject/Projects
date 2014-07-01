using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Accounting_System.CurrencyServiceReference;
using Oleit.AS.Service.DataObject;

namespace Accounting_System
{
    public partial class CurrencyMgmt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckLimit.CheckPage(Request["menuid"]);
            if (!IsPostBack)
            {
                loadCurrency();
            }
        }
        /// <summary>
        /// Initial Currency repeater
        /// </summary>
        private void loadCurrency()
        {
            var _csc = new CurrencyServiceClient();
            rptCurrency.DataSource = _csc.AllCurrency().ToList();
            rptCurrency.DataBind();
        }

        /// <summary>
        /// Add a new Currency
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            //txtCurrencyName
            var _csc = new CurrencyServiceClient();
            var _newCurrency = new Currency {CurrencyID = txtCurrencyName.Value};
            _csc.NewCurrency(_newCurrency);
            loadCurrency();
            Page.ClientScript.RegisterStartupScript(GetType(), "Success string", "Alert('Add Success !');", true);
        }
    }
}