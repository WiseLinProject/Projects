using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oleit.AS.Service.DataObject;

namespace Accounting_System
{
    public partial class RoleEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckLimit.CheckPage(Request["menuid"]);
            //RoleMgmt.aspx
            try
            {
                int _roleId = int.Parse(Request["i"].ToString());
                string _roleName = Request["r"].ToString();
                initEditRole(_roleId, _roleName);
            }
            catch (Exception)
            {

                Response.Redirect("RoleMgmt.aspx");
            }
            
        }

        private void initEditRole(int roleId, string roleName)
        {
            Role _role = new Role {RoleName = roleName, ID = roleId};
            List<Role> _rl = new List<Role>();
            _rl.Add(_role);
            dvRoleEdit.DataSource = _rl.ToList();
            dvRoleEdit.DataBind();
        }
    }
}