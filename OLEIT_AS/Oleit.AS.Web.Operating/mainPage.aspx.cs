using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oleit.AS.Service.DataObject;
using Accounting_System.MenuServiceReference;
using System.Text;
using Menu = Oleit.AS.Service.DataObject;

namespace Accounting_System
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int _userID;
            if (Session["UserID"] != null && int.TryParse(Session["UserID"].ToString(), out _userID))
            {
                if (!IsPostBack)
                {
                    menuFunc(_userID);
                }
            }
            else
            {
                Session.Clear();
                Session.Abandon();
                Response.Clear();
                Response.Redirect("Login.aspx");
            }
        }
        private void menuFunc( int userID)
        {
            StringBuilder _sbMenu = new StringBuilder();
            MenuServiceClient _msc = new MenuServiceClient();
            FuncMenuCollection _menuList = new FuncMenuCollection(_msc.QueryUserMenu(userID).GroupBy(x => x.Text).Select(x => x.First()));
            var _menuParentList = _menuList.Where(x => x.ParentID == 0).ToList();
            _sbMenu.Append("<div id='divFuncMenu'>");
            _sbMenu.Append("<ul>");
            for (int i = 0; i < _menuParentList.Count; i++)
            {
                var _menu = _menuParentList[i];
                _sbMenu.Append(buildString(_menuList.Any(x => x.ParentID == _menu.ItemID), i == _menuParentList.Count, _menu, _menuList));
            }
            _sbMenu.Append("</ul></div>");
            ltrFuncMenu.Text = _sbMenu.ToString();
        }
        private string buildString(bool hasChildren, bool lastMenu, FuncMenu _menu, FuncMenuCollection _menuCollection)
        {
            string _class = "";
            StringBuilder _sbString = new StringBuilder();
            if (lastMenu)
                _class = "last";


            if (hasChildren)
            {
                _sbString.AppendFormat("<li class='has-sub {0}'><a href='#'><span>{1}</span></a><ul>", _class,
                                       _menu.Text);
                _sbString.Append(buildSubString(_menu.ItemID, _menuCollection));
                _sbString.Append("</ul></li>");
            }
            else
            {
                _sbString.AppendFormat(
                    "<li ><a id='{0}?menuid={1}'  onclick='changePage(this.id);' href='#' ><span>{2}</span></a></li>",_menu.Path,_menu.ItemID,_menu.Text);
            }


            return _sbString.ToString();
        }

        private string buildSubString(int parentId, FuncMenuCollection _menu)
        {
            StringBuilder _sbSubString = new StringBuilder();
            foreach (var subMenu in _menu.Where(x=>x.ParentID==parentId))
            {
                _sbSubString.AppendFormat("<li ><a id='{0}?menuid={1}' onclick='changePage(this.id);' href='#'><span>{2}</span></a></li>", subMenu.Path,subMenu.ItemID, subMenu.Text);
            }
            return _sbSubString.ToString();
        }
    }
}