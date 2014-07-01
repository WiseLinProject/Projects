using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Oleit.AS.Service.DataObject;
using Oleit.AS.Service.LogicService.LimitControlAccessReference;

namespace Oleit.AS.Service.LogicService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "LimitControlService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select LimitControlService.svc or LimitControlService.svc.cs at the Solution Explorer and start debugging.
    public class LimitControlService : ILimitControlService
    {
        public void DoWork()
        {
        }

        public List<FuncMenu> getMenuByUserID(string UserID)
        {
            LimitControlClient limitClient = new LimitControlClient();
            return limitClient.GetMenuByUserID(UserID).ToList();
        }

        public Dictionary<int, string> getUserFunctions(string UserID, string MenuID)
        {
            LimitControlClient limitClient = new LimitControlClient();
            return limitClient.getUserFunctions(UserID, MenuID);        
        }

        public Dictionary<int, string> getUserFunctionByMenuID(string MenuID)
        {
            LimitControlClient limitClient = new LimitControlClient();
            return limitClient.getUserFunctionByMenuID(MenuID);    
        }

        public bool insertUserFunction(string UserID, string FunctionID)
        {
            LimitControlClient limitClient = new LimitControlClient();
            return  limitClient.insertUserFunction(UserID, FunctionID);
        }

        public bool deleteUserFunction(string UserID, string FunctionID)
        { 
           LimitControlClient limitClient = new LimitControlClient();
           return  limitClient.deleteUserFunction(UserID, FunctionID);
        }

        public bool insertMenuToRole(String MenuID, String RoleID)
        {
            LimitControlClient limitClient = new LimitControlClient();
            return limitClient.insertMenuToRole(MenuID, RoleID);
        }

        public bool insertRoleToUser(String UserID, String RoleID)
        {
            LimitControlClient limitClient = new LimitControlClient();
            return limitClient.insertRoleToUser(UserID, RoleID);
        }

        public bool isFunctionAuthorized(String UserID, String FunctionID)
        {
            LimitControlClient limitClient = new LimitControlClient();
            return limitClient.isFunctionAuthorized(UserID, FunctionID);
        
        }

        public List<string> getUserRole(string userID)
        {
            LimitControlClient limitClient = new LimitControlClient();
            return limitClient.getUserRole(userID).ToList();
        
        }



    }
}
