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
    /// Summary description for UserFunctionInsertHandler
    /// </summary>
    public class UserFunctionInsertHandler : IHttpHandler, IReadOnlySessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            string UserID = context.Request["UserID"];
            string FunctionID = context.Request["FunctionID"];
            SaveUserLimitFunctions(UserID, FunctionID);

        }


        public bool SaveUserLimitFunctions(string UserID, string FunctionID)
        {

            LimitControlServiceClient limitClient = new LimitControlServiceClient();
   
            if (limitClient.insertUserFunction(UserID, FunctionID))
            {
                return true;
            }

            return false;
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