using Oleit.AS.Service.DataObject;
using System;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Data;

namespace Oleit.AS.Service.DataService
{
    [ServiceContract]
    public interface IMenuAccess
    {
        [OperationContract]
        void Insert(Role role, FuncMenu menu);

        //[OperationContract]
        //void Insert(Role role, MenuCollection menucollection);

        //[OperationContract]
        //MenuCollection Query(Role role);

        //[OperationContract(Name = "Query1")]
        //[WebGet(UriTemplate = "Query")]
        //Tuple<MenuCollection, RoleCollection> Query();

        [OperationContract]
        FuncMenuCollection Query(int userId);

        [OperationContract]
        Role QueryRole(int roleID);

        //[OperationContract]
        //void Update(Menu menu);
        [OperationContract]
        FuncMenuCollection QueryAllMenu();

        [OperationContract]
        FuncMenuCollection QueryUserMenu(int userID);

        [OperationContract]
        RoleCollection QueryAllRole();

        [OperationContract]
        DataSet QueryRoleMenuRelation();

        [OperationContract]
        DataSet QueryRoleUserRelation();

        [OperationContract]
        void Update(FuncMenuCollection menuCollection);

        [OperationContract]
        void DeleteRole(int roleID);      

        //[OperationContract]
        //bool SetRoles(User user, Role role);

        [OperationContract]
        bool SetRoles(User user, RoleCollection roleCollection);

        [OperationContract]
        UserCollection QueryRoleUser(int roleID);

        [OperationContract]
        RoleCollection QueryUserRole(int userID);

        [OperationContract]
        void UpdateRole(Role role);

        [OperationContract]
        void InsertRole(string roleName);

        [OperationContract]
        void InsertRoleMenuRelation(int menuID, int roleID);

        [OperationContract]
        void DeleteRoleMenuRelation(int menuID, int roleID);

        [OperationContract]
        void InsertRoleUserRelation(int menuID, int roleID);

        [OperationContract]
        void DeleteRoleUserRelation(int menuID, int roleID);
    }
}
