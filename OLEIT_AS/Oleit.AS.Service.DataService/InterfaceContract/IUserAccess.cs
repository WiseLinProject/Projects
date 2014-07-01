using Oleit.AS.Service.DataObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Oleit.AS.Service.DataService
{
    [ServiceContract]
    public interface IUserAccess
    {
        [OperationContract(Name = "Insertuser")]
        [WebGet(UriTemplate = "Insert/User/{user}")]
        void Insert(User user);

        [OperationContract(Name = "InsertuserCollection")]
        [WebGet(UriTemplate = "Insert/UserCollection/{userCollection}")]
        void Insert(UserCollection userCollection);

        [OperationContract(Name = "QueryuserID")]
        [WebGet(UriTemplate = "Query/int/{userID}")]
        UserCollection Query(int userID);

        [OperationContract(Name = "QueryuserName")]
        [WebGet(UriTemplate = "Query/string/{userName}")]
        UserCollection Query(string userName);

        [OperationContract]
        UserCollection QueryAll();

        [OperationContract(Name = "Updateuser")]
        [WebGet(UriTemplate = "Update/Entity/{entity}")]
        void Update(User user);

        [OperationContract(Name = "UpdateuserCollection")]
        [WebGet(UriTemplate = "Update/UserCollection/{userCollection}")]
        void Update(UserCollection userCollection);

        [OperationContract]
        void Disable(UserCollection userCollection);
       
        [OperationContract]
        bool DisableUser(int userID);

        [OperationContract]       
        void ChangePassword(string userName, string password);

        [OperationContract(Name = "UpdateRoles")]
        [WebGet(UriTemplate = "Update/string/{userName}/Role/{role}")]
        void UpdateRoles(string userName, Role role);

        [OperationContract(Name = "UpdaterolesCollection")]
        [WebGet(UriTemplate = "Update/string/{userName}/Role/{role}")]
        void UpdateRoles(string userName, RoleCollection roleCollection);

        [OperationContract]     
        RoleCollection QueryRoles(string userName, string roleName);

        [OperationContract]
        bool CheckRole(string userName, Role role);

        [OperationContract]
        RoleCollection GetAllRoles(string userName);

        [OperationContract]
        FuncMenuCollection GetMenu(string userName);

        [OperationContract]
        bool CheckPassword(string userName,string password);

    }
}
