using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Text;
using Accounting_System.MenuServiceReference;
using System.IO;

namespace Accounting_System
{
    public class CheckLimit
    {
        public static void CheckPage(object menuID)
        { 
            int _userID;
            int _menuID;
            //StringBuilder _sbLog = new StringBuilder();
            //_sbLog.AppendFormat("userID 11:{0}", System.Web.HttpContext.Current.Session["userID"].ToString());
            //_sbLog.AppendFormat("\r\nmenuID 11:{0}", menuID.ToString());
            //log(_sbLog.ToString(), "error");
            if(System.Web.HttpContext.Current.Session["userID"]==null)
            {
                System.Web.HttpContext.Current.Session.Clear();
                System.Web.HttpContext.Current.Session.Abandon();
                System.Web.HttpContext.Current.Response.Clear();
                StringBuilder _sb = new StringBuilder();
                _sb.Append("<script type='text/javascript'>");
                _sb.Append("alert('Session has expired.');window.parent.location.reload();");
                _sb.Append("</script>");
                //((System.Web.UI.Page)System.Web.HttpContext.Current.Handler).RegisterStartupScript("ReloadParent", _sb.ToString());
                System.Web.HttpContext.Current.Response.Write(_sb.ToString());
                System.Web.HttpContext.Current.Response.End();
            }
            else if ((menuID == null) || !int.TryParse(menuID.ToString(), out _menuID))
            {
                System.Web.HttpContext.Current.Response.Redirect("mainPage.aspx");
            }
            else
            {
                int.TryParse(System.Web.HttpContext.Current.Session["userID"].ToString(), out _userID);                
                //_sbLog.AppendFormat("userID:{0}",_userID);
                //_sbLog.AppendFormat("\r\nmenuID:{0}", menuID);
                var _msr = new MenuServiceClient();
                var _UserMenu = _msr.QueryUserMenu(_userID);
                //_sbLog.AppendFormat("\r\nuserMenu count:{0}", _UserMenu.Count());
                if (_UserMenu.Any())
                {
                    int.TryParse(menuID.ToString(), out _menuID);
                    var _limitUserMenu = _msr.QueryUserMenu(_userID).Select(x => x.ItemID == _menuID);
                    if (!_limitUserMenu.Any())
                    {
                        System.Web.HttpContext.Current.Response.Redirect("mainPage.aspx");
                    }
                }
                else
                {
                    //log(_sbLog.ToString(), "error");
                    StringBuilder _sb = new StringBuilder();
                    _sb.Append("<script type='text/javascript'>");
                    _sb.Append("alert('Please contact administrator to set this user limit.');window.parent.location.href='Logout.aspx';");
                    _sb.Append("</script>");
                    System.Web.HttpContext.Current.Response.Write(_sb.ToString());
                }
            }
        }
        public static void log(string logStr, string logFileName)
        {
            try
            {
                if (!Directory.Exists(@"C:\logAS\"))
                {
                    DirectoryInfo di = Directory.CreateDirectory(@"C:\logAS\");
                }
            }
            catch (Exception)
            {

            }
            using (StreamWriter swr = new StreamWriter(@"C:\logAS\" + logFileName + ".txt", false, System.Text.Encoding.Default))
            {
                swr.Write(logStr);
                swr.Close();
            }
        }
    }
}