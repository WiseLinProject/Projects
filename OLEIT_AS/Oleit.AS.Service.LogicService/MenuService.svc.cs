using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Oleit.AS.Service.DataObject;
using Oleit.AS.Service.LogicService.MenuAccessReference;
using System.Data;

namespace Oleit.AS.Service.LogicService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "MenuService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select MenuService.svc or MenuService.svc.cs at the Solution Explorer and start debugging.
    public class MenuService : IMenuService
    {
        public void DoWork()
        {
        }

        

        public RoleCollection QueryAllRole()
        {
            using (MenuAccessClient _menuAccessClient = new MenuAccessClient(EndpointName.MenuAccess))
            {
                RoleCollection _rc = new RoleCollection(_menuAccessClient.QueryAllRole());
                return _rc;
            }
        }

        public UserCollection QueryRoleUser(int roleID)
        {
            using (MenuAccessClient _menuAccessClient = new MenuAccessClient(EndpointName.MenuAccess))
            {
                UserCollection _uc = new UserCollection(_menuAccessClient.QueryRoleUser(roleID));
                return _uc;
            }
        }

        public RoleCollection QueryUserRole(int userID)
        {
            using (MenuAccessClient _menuAccessClient = new MenuAccessClient(EndpointName.MenuAccess))
            {
                RoleCollection _rc = new RoleCollection(_menuAccessClient.QueryUserRole(userID));
                return _rc;
            }
        }

        public Role QueryRole(int roleId)
        {
            using (MenuAccessClient _menuAccessClient = new MenuAccessClient(EndpointName.MenuAccess))
            {
                Role _rc =_menuAccessClient.QueryRole(roleId);
                return _rc;
            }
        }

        public void UpdateRole(Role role)
        {
            using (MenuAccessClient _menuAccessClient = new MenuAccessClient(EndpointName.MenuAccess))
            {
                _menuAccessClient.UpdateRole(role);
            }
        }

        public bool DeleteRole(RoleCollection roleCollection)
        {
            using (MenuAccessClient _menuAccessClient = new MenuAccessClient(EndpointName.MenuAccess))
            {
                bool _hasUser = false;
                foreach (var role in roleCollection)
                {
                    var _userCollection= _menuAccessClient.QueryRoleUser(role.ID);
                    if (_userCollection.Any())
                        _hasUser = true;
                }
                if (_hasUser)
                    return false;
                else
                {
                    try
                    {
                        foreach (var role in roleCollection)
                        {
                            _menuAccessClient.DeleteRole(role.ID);
                        }
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                    return true;
                }
            }
        }

        public bool InsertRole(string roleName)
        {
            using (MenuAccessClient _menuAccessClient = new MenuAccessClient(EndpointName.MenuAccess))
            {
                try
                {
                    _menuAccessClient.InsertRole(roleName);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public FuncMenuCollection QueryAllMenu()
        {
            using (MenuAccessClient _menuAccessClient = new MenuAccessClient(EndpointName.MenuAccess))
            {
                FuncMenuCollection _fmc = new FuncMenuCollection(_menuAccessClient.QueryAllMenu());
                return _fmc;
            }
        }

        public FuncMenuCollection QueryUserMenu(int userID)
        {
            using (MenuAccessClient _menuAccessClient = new MenuAccessClient(EndpointName.MenuAccess))
            {
                FuncMenuCollection _fmc = new FuncMenuCollection(_menuAccessClient.QueryUserMenu(userID));
                return _fmc;
            }
        }

        public void DeleteRoleMenuRelation(int menuID,int roleID)
        {
            using (MenuAccessClient _menuAccessClient = new MenuAccessClient(EndpointName.MenuAccess))
            {
                _menuAccessClient.DeleteRoleMenuRelation(menuID,roleID);
            }
        }

        public void InsertRoleMenuRelation(int menuID, int roleID)
        {
            using (MenuAccessClient _menuAccessClient = new MenuAccessClient(EndpointName.MenuAccess))
            {
                _menuAccessClient.InsertRoleMenuRelation(menuID, roleID);
            }
        }

        public void DeleteRoleUserRelation(int userID, int roleID)
        {
            using (MenuAccessClient _menuAccessClient = new MenuAccessClient(EndpointName.MenuAccess))
            {
                _menuAccessClient.DeleteRoleUserRelation(userID, roleID);
            }
        }

        public void InsertRoleUserRelation(int userID, int roleID)
        {
            using (MenuAccessClient _menuAccessClient = new MenuAccessClient(EndpointName.MenuAccess))
            {
                _menuAccessClient.InsertRoleUserRelation(userID, roleID);
            }
        }


    }
}
