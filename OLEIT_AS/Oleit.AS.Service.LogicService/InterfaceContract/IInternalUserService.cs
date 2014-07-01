using Oleit.AS.Service.DataObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web;

namespace Oleit.AS.Service.LogicService
{
    [ServiceContract]
    public interface IInternalUserService
    {
        [OperationContract (Name="NewUser")]
        [WebGet(UriTemplate = "Insert/User/{user}")]
        void NewUser(User user);

        [OperationContract(Name = "NewUserCollection")]
        [WebGet(UriTemplate = "Insert/UserCollection/{collection}")]
        void NewUser(UserCollection collection);

        [OperationContract]
        int CheckPassword(string userName, string password);

        [OperationContract]      
        void SetRole(string userName, Role role);

        [OperationContract]       
        bool CheckRole(string userName, Role role);

        [OperationContract]      
        RoleCollection GetAllRoles(string userName);

        [OperationContract]       
        bool DisableUser(int userID);

        [OperationContract]
        void Disable(UserCollection collection);

        [OperationContract]
        void SetRelateEntity(Relation relation);

        [OperationContract]
        RelationCollection GetRelateEntity(int entityID);

        [OperationContract]       
        FuncMenuCollection GetMenu(string userName);

        [OperationContract]
        UserCollection QueryADuser();

        [OperationContract]
        UserCollection QueryAlluser();

    }
}