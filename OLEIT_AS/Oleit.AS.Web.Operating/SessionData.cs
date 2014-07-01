using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oleit.AS.Service.DataObject;

namespace Accounting_System
{
    public class SessionData
    {        
        #region        
        /// <summary>
        /// Initial User
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="UserAccount"></param>
        /// <param name="RoleID"></param>
        public static void InitUserData(int UserID, string UserAccount, RoleCollection RoleID)
        {
            System.Web.HttpContext.Current.Session["UserID"] = UserID;
            System.Web.HttpContext.Current.Session["UserAccount"] = UserAccount;
            System.Web.HttpContext.Current.Session["RoleID"] = RoleID;
         
        }
        /// <summary>
        /// LogOut
        /// </summary>
        public static void Logout()
        {
            System.Web.HttpContext.Current.Session.Abandon();
        }

        #endregion

        #region 
        /// <summary>
        /// UserID
        /// </summary>
        public static int UserID
        {
            get
            {
                return (int)System.Web.HttpContext.Current.Session["UserID"];
            }
        }
        /// <summary>
        /// UserAccount
        /// </summary>
        public static string UserAccount
        {
            get
            {
                return (string)System.Web.HttpContext.Current.Session["UserAccount"];
            }
        }

        /// <summary>
        /// RoleID
        /// </summary>
        public static RoleCollection RoleID
        {
            get
            {
                return (RoleCollection)System.Web.HttpContext.Current.Session["RoleID"];
            }
        }
        #endregion

    }
}