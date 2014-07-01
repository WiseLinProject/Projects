using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;

namespace Accounting_System
{
    public class Library
    {
        /// <summary>
        /// Add comma thousands separator
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string FN(string num)
        {
            bool IsMiner = false;
            try
            {
                if (Convert.ToDouble(num) < 0)
                    IsMiner = true;
            }
            catch
            {

            }
            if (num == null) throw new ArgumentNullException("num");
            string newstr = string.Empty;
            Regex r = new Regex(@"(\d+?)(\d{3})*(\.\d+|$)");
            Match m = r.Match(num);
            newstr += m.Groups[1].Value;
            for (int i = 0; i < m.Groups[2].Captures.Count; i++)
            {
                newstr += "," + m.Groups[2].Captures[i].Value;
            }
            newstr += m.Groups[3].Value;
            if (IsMiner)
                newstr = "-" + newstr;
            return newstr;
        }
        public static void Alert(string message)
        {
            string script = string.Format("alert('{0}');", HttpUtility.HtmlEncode(message.Replace("\n", "\\n")));
            JavaScrpit("window_aler", script);
        }
        public static void Confirm(string message)
        {
            string script = string.Format("confirm('{0}');", HttpUtility.HtmlEncode(message.Replace("\n", "\\n")));
            JavaScrpit("window_confirm", script);
        }
        public static void JavaScrpit(string name, string script)
        {
            System.Web.UI.Page currentPage = (System.Web.UI.Page)HttpContext.Current.Handler;
            ScriptManager.RegisterClientScriptBlock(currentPage, typeof(Page), name, script, true);
        }

    }
}