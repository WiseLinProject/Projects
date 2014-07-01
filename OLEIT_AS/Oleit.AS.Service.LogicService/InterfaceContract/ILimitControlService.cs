using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Oleit.AS.Service.DataObject;


namespace Oleit.AS.Service.LogicService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ILimitControlService" in both code and config file together.
    [ServiceContract]
    public interface ILimitControlService
    {
        [OperationContract]
        void DoWork();

        [OperationContract]
         List<FuncMenu> getMenuByUserID(string UserID);

         [OperationContract]
        Dictionary<int, string> getUserFunctions(string UserID, string MenuID);

         [OperationContract]
        Dictionary<int, string> getUserFunctionByMenuID(string MenuID);

         [OperationContract]
        bool insertUserFunction(string UserID, string FunctionID);

         [OperationContract]
        bool deleteUserFunction(string UserID, string FunctionID);

         [OperationContract]
         bool insertMenuToRole(String MenuID, String RoleID);

         [OperationContract]
        bool insertRoleToUser(String UserID, String RoleID);

         [OperationContract]
         bool isFunctionAuthorized(String UserID, String FunctionID);

         [OperationContract]
        List<string> getUserRole(string userID);


    }
}
