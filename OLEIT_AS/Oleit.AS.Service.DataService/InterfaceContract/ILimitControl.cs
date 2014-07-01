using Oleit.AS.Service.DataObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Oleit.AS.Service.DataService
{
    [ServiceContract]
    public interface ILimitControl
    {
        [OperationContract]
        EntityCollection GetEntities(User user);

        [OperationContract]
        AccountCollection GetAccounts(User user);

        [OperationContract]
        FuncMenuCollection GetMenu(User user);

        [OperationContract]
        FuncMenuCollection GetMenuByUserID(String UserID);

        [OperationContract]
        SortedList<int, string> getUserFunctions(String UserID, String MenuID);

        [OperationContract]
        SortedList<int,string> getUserFunctionByMenuID(String MenuID);

        [OperationContract]
        bool insertUserFunction(String UserID, String FunctionID);

        [OperationContract]
        bool insertMenuToRole(String MenuID, String RoleID);


        [OperationContract]
        bool insertRoleToUser(String UserID, String RoleID);


        [OperationContract]
        bool deleteUserFunction(String UserID, String FunctionID);

        [OperationContract]
        List<string> getUserRole(string userID);

        [OperationContract]
        bool isFunctionAuthorized(String UserID, String FunctionID);
        
        [OperationContract]
        bool CheckLimit(User user, int itemID);
    }
}
