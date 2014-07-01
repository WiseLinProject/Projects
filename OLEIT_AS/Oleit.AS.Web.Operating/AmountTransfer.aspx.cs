using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Accounting_System.EntityServiceReference;
using Accounting_System.CurrencyServiceReference;
using Oleit.AS.Service.DataObject;

namespace Accounting_System
{
    public partial class AmountTransfer : System.Web.UI.Page
    {
        public string JsonEntityTreeString = "";
        public int UserId;
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckLimit.CheckPage(Request["menuid"]);
            if (!IsPostBack)
            {
                JsonEntityTreeString = JsonEntityFunc.LoadEntityTree();
                UserId = int.Parse(SessionData.UserID.ToString());
            }
        }
    }
}