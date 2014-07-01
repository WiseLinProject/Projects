using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oleit.AS.Service.DataObject;
using Accounting_System.MenuServiceReference;
using Accounting_System.SystemDataServiceReference;
using Accounting_System.InternalUserServiceReference;
using System.Data;

namespace Accounting_System
{
    public partial class RoleUser : System.Web.UI.Page
    {
        private User[] UserCollection = new User[] { };
        private Role[] RoleCollecion = new Role[] { };
        private DataSet RoleUserRelation = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckLimit.CheckPage(Request["menuid"]);
            if (!IsPostBack)
            {
                genRoleUserTable();
            }
        }

        private void genRoleUserTable()
        {
            var _msr = new MenuServiceClient();
            var _sdsr = new SystemDataServiceClient();
            var _iusr = new InternalUserServiceClient();
            RoleCollecion = _msr.QueryAllRole();
            UserCollection = _iusr.QueryAlluser();
            RoleUserRelation = _sdsr.QueryRoleUserRelation();

            gvRoleUser.DataSource = UserCollection;
            foreach (var role in RoleCollecion)
            {
                BoundField _bf = new BoundField();
                _bf.HeaderText = role.RoleName;
                gvRoleUser.Columns.Add(_bf);
            }
            gvRoleUser.DataBind();
        }

        protected void gvRoleUser_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[0].CssClass = "leftColumn";
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var _roleMenu = RoleUserRelation.Tables[0].AsEnumerable();
                int _menuID = int.Parse(gvRoleUser.DataKeys[e.Row.RowIndex].Values[0].ToString());
                for (int i = 0; i < RoleCollecion.Count(); i++)
                {
                    CheckBox _cb = new CheckBox();
                    _cb.ID = string.Format("{0}_{1}", _menuID, RoleCollecion[i].ID);
                    _cb.ClientIDMode = ClientIDMode.Static;
                    var _hasRelation = _roleMenu.Where(x => x.Field<int>("User_ID") == _menuID && x.Field<int>("Role_ID") == RoleCollecion[i].ID);
                    _cb.Checked = _hasRelation.Any();
                    e.Row.Cells[i + 1].Controls.Add(_cb);
                }
            }

        }
    }
}