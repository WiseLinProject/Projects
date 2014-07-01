using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oleit.AS.Service.DataObject;
using Accounting_System.MenuServiceReference;
using Accounting_System.InternalUserServiceReference;

namespace Accounting_System
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserID"] != null)
                {
                    Response.Redirect("mainPage.aspx"); 
                }
            } 
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            var _iusr = new InternalUserServiceClient();
            var _msr = new MenuServiceClient();
            int _userID=_iusr.CheckPassword(txtUsername.Value,txtPassword.Value);
            if(_userID!=-1)
            {


                RoleCollection _rc = new RoleCollection(_msr.QueryUserRole(_userID));
                SessionData.InitUserData(_userID, txtUsername.Value.ToString(),_rc );
                Response.Redirect("mainPage.aspx");
            }
            else
            {
                ltrFailText.Text = string.Format("<span style='color:red;font-weight:bold'>Invalid Username or Password!</span>");
            }
        }
    }
}