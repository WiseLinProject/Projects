using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oleit.AS.Service.DataObject;
using Accounting_System.MenuServiceReference;

namespace Accounting_System
{
    public partial class RoleAjax : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] != null)
            {
                int _roleId;
                int.TryParse(Request["RoleId"], out _roleId);

                int _menuId;
                int.TryParse(Request["MenuId"], out _menuId);

                int _userId;
                int.TryParse(Request["UserId"], out _userId);

                string _roleName = Request["RoleName"];
                string _roleIdAry = Request["roleIdAry"];
                string _type = Request["Type"];
                string _checked = Request["Checked"];
                if (_type.Equals("update", StringComparison.OrdinalIgnoreCase))
                    updateRoleName(_roleId, _roleName);
                else if (_type.Equals("delete", StringComparison.OrdinalIgnoreCase))
                    deleteRole(_roleIdAry);
                else if (_type.Equals("insert", StringComparison.OrdinalIgnoreCase))
                    addRole(_roleName);
                else if (_type.Equals("rolemenu", StringComparison.OrdinalIgnoreCase))
                {
                    updateRoleMenuRelation(_menuId, _roleId, _checked);
                }
                else if (_type.Equals("roleuser", StringComparison.OrdinalIgnoreCase))
                {
                    updateRoleUserRelation(_userId, _roleId, _checked);
                }
                else
                {
                    Response.Write("There is no limit here.");
                }
            }
            else
            {
                Response.Write("Session has expired.");
            }
        }

        private void updateRoleName(int roleId,string roleName)
        {
            var _msr = new MenuServiceClient();
            try
            {
                _msr.UpdateRole(new Role { ID = roleId, RoleName = roleName });
                Response.Write("Success");
            }
            catch (Exception)
            {

                Response.Write("Fail");
            }
        }

        private void deleteRole(string roleIdAry )
        {
            string _roleIDStr = roleIdAry;
            string[] _roleIDAry = _roleIDStr.Split(',');
            var _roleCollection = from r in _roleIDAry
                                  select new Role
                                  {
                                      ID = int.Parse(r)
                                  };
            MenuServiceClient _msc = new MenuServiceClient();
            bool _result = _msc.DeleteRole(_roleCollection.ToArray());
            if(_result)
                Response.Write("Success");
            else
            {
                Response.Write("There are still users in selected Roles.");
            }
        }

        private void addRole(string roleName)
        {
            MenuServiceClient _msc = new MenuServiceClient();
            bool _result = _msc.InsertRole(roleName);
            if(_result)
                Response.Write("Success");
            else
            {
                Response.Write("Fail");
            }
        }

        private void updateRoleMenuRelation(int menuID , int roleID,string trueFalse)
        {
            var _msr = new MenuServiceClient();
            if(trueFalse.Equals("true"))
            {
                _msr.InsertRoleMenuRelation(menuID,roleID);
            }
            else
            {
                _msr.DeleteRoleMenuRelation(menuID, roleID);
            }
        }

        private void updateRoleUserRelation(int userID, int roleID, string trueFalse)
        {
            var _msr = new MenuServiceClient();
            if (trueFalse.Equals("true"))
            {
                _msr.InsertRoleUserRelation(userID, roleID);
            }
            else
            {
                _msr.DeleteRoleUserRelation(userID, roleID);
            }
        }

    }
}