﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Accounting_System
{
    public partial class AmountSummarize : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckLimit.CheckPage(Request["menuid"]);
        }
    }
}