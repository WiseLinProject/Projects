using System;
using System.Linq;
using Oleit.AS.Service.DataObject;
using Accounting_System.MenuServiceReference;


namespace Accounting_System
{
    public partial class RoleMgmt : System.Web.UI.Page
    {
        private int _userID;
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckLimit.CheckPage(Request["menuid"]);
            if (!IsPostBack)
            {
                loadRole();
            }
        }

        private void loadRole()
        {
            MenuServiceClient _msc = new MenuServiceClient();
            gvRole.DataSource = _msc.QueryAllRole().Where(x=>x.ID!=1);
            gvRole.DataBind();
        }

        //protected void btnDelete_Click(object sender, EventArgs e)
        //{
        //    string _roleIDStr = hfRoleIDAry.Value;
        //    string[] _roleIDAry = _roleIDStr.Split(',');
        //    var _roleCollection = from r in _roleIDAry
        //                          select new Role
        //                                     {
        //                                         ID = int.Parse(r)
        //                                     };
        //    MenuServiceClient _msc = new MenuServiceClient();
        //    bool _result = _msc.DeleteRole(_roleCollection.ToArray());
        //    loadRole();
        //    Page.ClientScript.RegisterStartupScript(GetType(), "Result string", "Alert('"+_result+"!');", true);
            
        //}

        protected void btnAddNewRole_Click(object sender, EventArgs e)
        {
            MenuServiceClient _msc = new MenuServiceClient();
            bool _result = _msc.InsertRole(txtNewRoleName.Value);
            loadRole();
            Page.ClientScript.RegisterStartupScript(GetType(), "Result string", "Alert('" + _result + "!');", true);
        }
    }
}