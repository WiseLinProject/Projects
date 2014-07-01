using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Web.Script.Serialization;
using Oleit.AS.Service.DataObject;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Accounting_System.LimitControlServiceReference;
using Oleit.AS.Service.LogicService;

namespace Accounting_System
{
    /// <summary>
    /// Summary description for UserFunctionHandler
    /// </summary>
    public class UserFunctionHandler : IHttpHandler,IReadOnlySessionState
    {

        public void ProcessRequest(HttpContext context)
        {

      
         string UserID = context.Request["UserID"];
          string FunctionID = context.Request["FunctionID"];
      
         List<FuncMenu> returnMenuList = new List<FuncMenu>();
            
        string TableStr = string.Empty;

        if (UserID != null )
            {
              // TableStr+= "<table id='myTable' class='table table-hover table-bordered table table-condensed' >";
       

               returnMenuList = getData(UserID);
               TableStr += "<tr><td colspan='4' align='center'><input id='btnSaveAll' class='SaveAll' title='Save All' value='' type='button' /></td></tr>";
               TableStr += "<tr><th align='center'>ID</th><th align='center'>Menu Text</th><th align='center'>Functions</th><th align='center'></th></tr>";

               string repeatMenu = string.Empty;
               foreach (var menu in returnMenuList)
               {

                   if (repeatMenu == menu.ItemID.ToString())
                   {
                       continue;
                   }
                   else
                   {
                       repeatMenu = menu.ItemID.ToString();
                   }

                   TableStr += "<tr>";
                   TableStr += "<td align='center'>" + menu.ItemID + "</td>";
                   TableStr += "<td align='left'>&nbsp;&nbsp;" + menu.Text + "</td>";
                   
                   Dictionary<int, string> returnFunctionList = new Dictionary<int, string>();

                   LimitControlServiceClient limitClient = new LimitControlServiceClient();
              
                   returnFunctionList = getFunctionList(menu.ItemID.ToString());


                   TableStr += "<td align='left'>";
                   foreach (var func in returnFunctionList)
                   {
                       
                       if (limitClient.isFunctionAuthorized(UserID, func.Key.ToString()))
                       {
                           TableStr += "&nbsp;&nbsp;<input type='checkbox' checked value='" + func.Key.ToString() + "' />" + func.Value;
                       }
                       else
                       {
                           TableStr += "&nbsp;&nbsp;<input type='checkbox'  value='" + func.Key.ToString() + "' />" + func.Value;
                       }

                       TableStr += "</br>";
                       
                   }
                   TableStr += "</td>";
                   TableStr += "<td align='center'><input id='btnSave' type='button' class='Save02' title='Save' value=''/></td>";
                  


                   TableStr += "</tr>";
               }

               
               // TableStr+="</table>";
           }  
            context.Response.Write(TableStr);

           
        }


     

        public List<FuncMenu> getData(string UserID)
        {
            LimitControlServiceClient menu = new LimitControlServiceClient();
          
            return menu.getMenuByUserID(UserID).ToList();
       
        }
        public Dictionary<int,string> getFunctionList(string MenuID)
        {
            LimitControlServiceClient menu = new LimitControlServiceClient();
            return menu.getUserFunctionByMenuID(MenuID);

        }


        public bool DeleteUserLimitFunctions(string UserID,string FunctionID)
        {
         
            LimitControlServiceClient limitClient = new LimitControlServiceClient();
            if (limitClient.deleteUserFunction(UserID, FunctionID))
            {
                return true;
            }

            return false;
        
        }


        public bool SaveUserLimitFunctions(string UserID,string FunctionID)
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