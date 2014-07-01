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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IMenuService" in both code and config file together.
    [ServiceContract]
    public interface IMenuService
    {
        [OperationContract]
        void DoWork();

        [OperationContract]
        RoleCollection QueryAllRole();

        [OperationContract]
        Role QueryRole(int roleId);

        [OperationContract]
        void UpdateRole(Role role);

        [OperationContract]
        UserCollection QueryRoleUser(int roleID);

        [OperationContract]
        RoleCollection QueryUserRole(int userID);

        [OperationContract]
        bool DeleteRole(RoleCollection roleCollection);

        [OperationContract]
        bool InsertRole(string roleName);

        [OperationContract]
        FuncMenuCollection QueryAllMenu();

        [OperationContract]
        FuncMenuCollection QueryUserMenu(int userID);

        [OperationContract]
        void DeleteRoleMenuRelation(int menuID, int roleID);

        [OperationContract]
        void InsertRoleMenuRelation(int menuID, int roleID);

        [OperationContract]
        void DeleteRoleUserRelation(int userID, int roleID);

        [OperationContract]
        void InsertRoleUserRelation(int userID, int roleID);

    }

}
