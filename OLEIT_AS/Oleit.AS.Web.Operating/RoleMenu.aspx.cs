using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oleit.AS.Service.DataObject;
using Accounting_System.MenuServiceReference;
using Accounting_System.SystemDataServiceReference;
using System.Data;

namespace Accounting_System
{
    public partial class RoleMenu : System.Web.UI.Page
    {
        private FuncMenu[] FMCollection = new FuncMenu[] { };
        private Role[] RoleCollecion = new Role[] { };
        private DataSet RoleMenuRelation = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckLimit.CheckPage(Request["menuid"]);
            if (!IsPostBack)
            {
                genRoleMenuTable();
            }
        }
        private void genRoleMenuTable()
        {
            var _msr = new MenuServiceClient();
            var _sdsr = new SystemDataServiceClient();
            RoleCollecion = _msr.QueryAllRole();
            FMCollection = _msr.QueryAllMenu();
            RoleMenuRelation = _sdsr.QueryRoleMenuRelation();
            
            gvRoleMenu.DataSource = FMCollection;
            foreach (var role in RoleCollecion)
            {
                BoundField _bf = new BoundField();
                _bf.HeaderText = role.RoleName;
                
                
                gvRoleMenu.Columns.Add(_bf);
            }
            
            gvRoleMenu.DataBind();
        }

        protected void gvRoleMenu_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            e.Row.Cells[0].CssClass = "leftColumn";

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var _roleMenu = RoleMenuRelation.Tables[0].AsEnumerable();
                int _menuID = int.Parse(gvRoleMenu.DataKeys[e.Row.RowIndex].Values[0].ToString());
                for (int i = 0; i < RoleCollecion.Count(); i++)
                {
                    CheckBox _cb = new CheckBox();
                    _cb.ID = string.Format("{0}_{1}", _menuID,RoleCollecion[i].ID);
                    _cb.ClientIDMode = ClientIDMode.Static;
                    var _hasRelation = _roleMenu.Where(x => x.Field<int>("Menu_ID") == _menuID && x.Field<int>("Role_ID") == RoleCollecion[i].ID);
                    _cb.Checked = _hasRelation.Any();
                    e.Row.Cells[i+1].Controls.Add(_cb);
                }
            }

        }
    }
}