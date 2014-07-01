using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Accounting_System.LimitControlServiceReference;
using Oleit.AS.Service.LogicService;

namespace Accounting_System
{
    /// <summary>
    /// Summary description for UserFunctionCloneHandler
    /// </summary>
    public class UserFunctionCloneHandler : IHttpHandler, IReadOnlySessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            string UserID = context.Request["UserID"];
            string SourcID = context.Request["SourcID"]; 
            string FunctionID = context.Request["FunctionID"];
            string MenuID = context.Request["MenuID"];
            SaveUserLimitFunctions(UserID, SourcID, MenuID, FunctionID);

        }


        public bool SaveUserLimitFunctions(string UserID, string SourcID, string MenuID, string FunctionID)
        {



            LimitControlServiceClient limitClient = new LimitControlServiceClient();


            string[] Roles = limitClient.getUserRole(SourcID);
            for (int i = 0; i < Roles.Length; i++)
            {
                limitClient.insertRoleToUser(UserID,Roles[i]);
           
            }

            return true;
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}