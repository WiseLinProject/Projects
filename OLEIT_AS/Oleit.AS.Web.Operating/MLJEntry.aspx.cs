using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Accounting_System.PeriodServiceReference;
using Accounting_System.MLJServiceReference;
using Accounting_System.PropertiesServiceReference;
using Accounting_System.EntityServiceReference;
using Oleit.AS.Service.DataObject;
using Accounting_System.MenuServiceReference;
using Accounting_System.LimitControlServiceReference;
using Accounting_System.SystemDataServiceReference;
using System.Text;
using System.Data;
using ClosedXML.Excel;
using System.IO;
using AjaxControlToolkit;

namespace Accounting_System
{
    public partial class MLJEntry : System.Web.UI.Page
    {
        Oleit.AS.Service.DataObject.Period _period;
        List<int> changeList = new List<int>();
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckLimit.CheckPage(Request["menuid"]);
            //Limit and Login not yet
            //Session["UserID"] = 26; 
            //Session["Roleid"] = 5;

            using (LimitControlServiceClient _licli = new LimitControlServiceClient())
            {
                if (_licli.isFunctionAuthorized(SessionData.UserID.ToString(), "7"))
                    btn_Approve.Visible = true;
                else
                    btn_Approve.Visible = false;
                if (_licli.isFunctionAuthorized(SessionData.UserID.ToString(), "8"))
                    IB_Excel.Visible = true;
                else
                    IB_Excel.Visible = false;
            }

            // Page.MaintainScrollPositionOnPostBack = true;
            //btn_Search.Attributes.Add("onclick", ClientScript.GetPostBackEventReference(btn_Search, "click") + ";this.disabled=true; this.value='Wait.....';");

            if (!IsPostBack)
            {
                Session["Rowindex"] = null;
                Session["AllDataTable"] = null;
                if (!IsCustomer())
                {
                    Panel_Type.Visible = false;
                    DDL_Type.SelectedValue = "0";
                }
                else
                {
                    Panel_Type.Visible = true;
                    DDL_Type.SelectedValue = "1";
                }
                //First:Check This Period has reoird ? if not create one
                PeriodServiceClient _pclient = new PeriodServiceClient();
                _period = new PeriodCollection(_pclient.GetCurrentPeriod())[0];
                tx_Period.Text = _period.PeriodNo;
                MLJServiceClient _client = new MLJServiceClient();
                _client.CheckAndAdd(_period.ID, SessionData.UserID);
                GetStatusDDL();
                GetUserDDL();
                GetData();

            }
            //  ScriptManager.RegisterStartupScript(this,typeof(Page),"ScriptDoFocus",SCRIPT_DOFOCUS.Replace("REQUEST_LASTFOCUS", Request["__LASTFOCUS"]),true);
        }
        private bool IsCustomer()
        {
            if (SessionData.RoleID.Exists(delegate(Role _role) { return _role.ID == 5; }))
                return true;
            else
                return false;
        }
        private void GetStatusDDL()
        {
            DDL_Status.Items.Clear();
            DDL_DetailStatus.Items.Clear();
            using (MLJServiceClient _MLJ = new MLJServiceClient())
            {
                DDL_Status.Items.Add(new ListItem("All", "0"));
                StatusColorCollection _coll = new StatusColorCollection(_MLJ.QueryStatusColor());
                foreach (StatusColor _color in _coll)
                {
                    string color = "#" + _color.MLJColor;
                    ListItem _item = new ListItem();
                    _item.Text = _color.StatusType.ToString();
                    _item.Value = _color.MLJStatus.ToString();
                    // _item.Attributes.Add("style", "background-color:" + color + "");                    
                    DDL_Status.Items.Add(_item);
                    DDL_DetailStatus.Items.Add(_item);
                }
            }
        }
        bool RecordConfirm;


        private void GetUserDDL()
        {
            DDL_User.Items.Clear();
            if (!IsCustomer())
            {
                using (MenuServiceClient _menuclient = new MenuServiceClient())
                {
                    UserCollection _userc = new UserCollection(_menuclient.QueryRoleUser(7));
                    DDL_User.DataTextField = "UserName";
                    DDL_User.DataValueField = "UserID";
                    DDL_User.DataSource = _userc.ToList();
                    DDL_User.DataBind();
                    DDL_User.Items.Insert(0, new ListItem("All", "0"));
                    DDL_User.SelectedValue = SessionData.UserID.ToString();
                }
            }
            else
            {
                using (MenuServiceClient _menuclient = new MenuServiceClient())
                {
                    UserCollection _userc1 = new UserCollection(_menuclient.QueryRoleUser(7));
                    UserCollection _userc2 = new UserCollection(_menuclient.QueryRoleUser(5));
                    foreach (User _user in _userc2)
                    {
                        _userc1.Add(_user);
                    }
                    DDL_User.DataTextField = "UserName";
                    DDL_User.DataValueField = "UserID";
                    DDL_User.DataSource = _userc1.ToList();
                    DDL_User.DataBind();
                    DDL_User.Items.Insert(0, new ListItem("All", "100"));
                }
            }
        }
        private void GetData()
        {
            EntityServiceClient _ent = new EntityServiceClient();
            if (tx_Period.Text.Equals(""))
            {
                Library.Alert("Period can't empty!! ");
                tx_Period.Focus();
                return;
            }
            PeriodServiceClient _pclient = new PeriodServiceClient();
            _period = _pclient.DateOfPeriod(tx_Period.Text)[0];
            if (_period == null)
            {
                Library.Alert("Period Error!! ");
                return;
            }
            using (MLJServiceClient _client = new MLJServiceClient())
            {
                // _client.CheckAndAdd(_period.ID, Convert.ToInt32(Session["UserID"]));
                RecordConfirm = false;
                string Name = tx_EntityName.Text;

                MLJJournalCollection _jurc = new MLJJournalCollection(_client.Query(_period.ID, Name));
                System.Data.DataTable dt = new System.Data.DataTable();

                dt.Columns.Add("RecordID"); dt.Columns.Add("ID"); dt.Columns.Add("EntityName"); dt.Columns.Add("Mon");
                dt.Columns.Add("Tue"); dt.Columns.Add("Wed"); dt.Columns.Add("Thu"); dt.Columns.Add("Fri"); dt.Columns.Add("Sat");
                dt.Columns.Add("Sun"); dt.Columns.Add("BaseCurrency"); dt.Columns.Add("ExchangeRate"); dt.Columns.Add("EntryUser");
                dt.Columns.Add("EntityTotal"); dt.Columns.Add("Accountid"); dt.Columns.Add("IsAccount"); dt.Columns.Add("WeekTotal"); dt.Columns.Add("Userid");
                dt.Columns["Mon"].DataType = typeof(int); dt.Columns["Tue"].DataType = typeof(int); dt.Columns["Wed"].DataType = typeof(int);
                dt.Columns["Thu"].DataType = typeof(int); dt.Columns["Fri"].DataType = typeof(int); dt.Columns["Sat"].DataType = typeof(int);
                dt.Columns["Sun"].DataType = typeof(int); dt.Columns.Add("EntityID");

                #region For Account Attribute
                dt.Columns.Add("Company"); dt.Columns.Add("Account_Name"); dt.Columns.Add("Password"); dt.Columns.Add("Betting_Limit");
                dt.Columns.Add("Status"); dt.Columns.Add("Factor"); dt.Columns.Add("Perbet"); dt.Columns.Add("DateOpen");
                dt.Columns.Add("Personnel"); dt.Columns.Add("IP"); dt.Columns.Add("Odds"); dt.Columns.Add("IssuesConditions");
                dt.Columns.Add("RemarksAcc"); dt.Columns.Add("Color");
                #endregion

                int _MonTotal = 0; int _TueTotal = 0; int _WedTotal = 0; int _ThuTotal = 0; int _FriTotal = 0;
                int _SatTotal = 0; int _SunTotal = 0; int WeekTotal = 0; int cusWeekTotal = 0;
                int _cusMonTotal = 0; int _cusTueTotal = 0; int _cusWedTotal = 0; int _cusThuTotal = 0; int _cusFriTotal = 0;
                int _cusSatTotal = 0; int _cusSunTotal = 0;
                int MLJRecoredID = 0;

                int _AllMonTotal = 0; int _AllTueTotal = 0; int _AllWedTotal = 0; int _AllThuTotal = 0; int _AllFriTotal = 0;
                int _AllSatTotal = 0; int _AllSunTotal = 0; int AllWeekTotal = 0;


                foreach (Oleit.AS.Service.DataObject.MLJJournal _jur in _jurc)
                {
                    DataRow newRow = dt.NewRow();

                    MLJRecoredID = _jur.MLJRecordID;
                    newRow["RecordID"] = _jur.MLJRecordID;
                    newRow["ID"] = _jur.SequenceNo;
                    newRow["Userid"] = _jur.UserID;
                    newRow["EntityName"] = _jur.EntityName;
                    newRow["EntityID"] = _jur.EntityID;
                    newRow["Accountid"] = _jur.Account.ID;
                    if (_jur.Account.ID.Equals(0))
                        newRow["IsAccount"] = 0;
                    else
                        newRow["IsAccount"] = 1;
                    //account
                    newRow["Company"] = _jur.Account.Company;
                    newRow["Account_Name"] = _jur.Account.AccountName;
                    newRow["Password"] = _jur.Account.Password;
                    newRow["Betting_Limit"] = _jur.Account.BettingLimit;
                    newRow["Status"] = _jur.Account.Status;
                    newRow["Factor"] = _jur.Account.Factor;
                    newRow["Perbet"] = _jur.Account.Perbet;
                    newRow["DateOpen"] = _jur.Account.DateOpen;
                    newRow["Personnel"] = _jur.Personnel.UserName;
                    newRow["IP"] = _jur.Account.IP;
                    newRow["Odds"] = _jur.Account.Odds;
                    newRow["IssuesConditions"] = _jur.Account.IssuesConditions;
                    newRow["RemarksAcc"] = _jur.Account.RemarksAcc;
                    newRow["Color"] = _jur.Account.Color;
                    newRow["Mon"] = _jur.Mon;
                    newRow["Tue"] = _jur.Tue;
                    newRow["Wed"] = _jur.Wed;
                    newRow["Thu"] = _jur.Thu;
                    newRow["Fri"] = _jur.Fri;
                    newRow["Sat"] = _jur.Sat;
                    newRow["Sun"] = _jur.Sun;
                    newRow["BaseCurrency"] = _jur.BaseCurrency;
                    newRow["ExchangeRate"] = _jur.ExchangeRate;
                    newRow["EntryUser"] = _jur.EntryUser.UserName;
                    newRow["EntityTotal"] = Library.FN((_jur.Mon + _jur.Tue + _jur.Wed + _jur.Thu + _jur.Fri + _jur.Sat + _jur.Sun).ToString());
                    dt.Rows.Add(newRow);
                }
                if (dt.Rows.Count > 0)
                {
                    Session["AllDataTable"] = dt;
                    MLJRecord _mljre = _client.QueryRecordByID(MLJRecoredID);
                    if (_mljre.RecordStatus.Equals(RecordStatus.Confirm))
                        RecordConfirm = true;
                    using (LimitControlServiceClient _licli = new LimitControlServiceClient())
                    {
                        if (!RecordConfirm && _licli.isFunctionAuthorized(SessionData.UserID.ToString(), "7"))
                            btn_Approve.Visible = true;
                        else
                            btn_Approve.Visible = false;
                    }
                    // Filter
                    DataTable _newdt = new DataTable();
                    string _filter = "";
                    if (DDL_Type.SelectedValue.Equals("0") || !IsCustomer())
                        _filter = "IsAccount = '1' ";
                    else if (DDL_Type.SelectedValue.Equals("1"))
                        _filter = "IsAccount = '0' ";
                    if (!DDL_User.SelectedValue.Equals("0") && !DDL_User.SelectedValue.Equals("100"))
                    {
                        if (_filter != "")
                            _filter += "  and Userid = '" + DDL_User.SelectedValue + "'";
                        else
                            _filter += "  Userid = '" + DDL_User.SelectedValue + "' ";
                    }
                    if (!DDL_Status.SelectedValue.Equals("0"))
                    {
                        if (_filter != "")
                            _filter += " and Status = '" + DDL_Status.SelectedItem.Text + "' ";
                        else
                            _filter += " Status = '" + DDL_Status.SelectedItem.Text + "' ";
                    }
                    dt.DefaultView.RowFilter = _filter;
                    dt.DefaultView.Sort = "IsAccount desc,EntityName";


                    _newdt = dt.DefaultView.ToTable();
                    int _index = 0; int _firstcusindex = 0; bool _gocus = false; int _accindex = 0; int _cusindex = 0;
                    foreach (DataRow _newdata in _newdt.Rows)
                    {
                        int _newMon = Convert.ToInt32(_newdata["Mon"]); int _newTue = Convert.ToInt32(_newdata["Tue"]); int _newWed = Convert.ToInt32(_newdata["Wed"]);
                        int _newThu = Convert.ToInt32(_newdata["Thu"]); int _newFri = Convert.ToInt32(_newdata["Fri"]); int _newSat = Convert.ToInt32(_newdata["Sat"]);
                        int _newSun = Convert.ToInt32(_newdata["Sun"]);
                        int _cusnewMon = Convert.ToInt32(_newdata["Mon"]); int _cusnewTue = Convert.ToInt32(_newdata["Tue"]); int _cusnewWed = Convert.ToInt32(_newdata["Wed"]);
                        int _cusnewThu = Convert.ToInt32(_newdata["Thu"]); int _cusnewFri = Convert.ToInt32(_newdata["Fri"]); int _cusnewSat = Convert.ToInt32(_newdata["Sat"]);
                        int _cusnewSun = Convert.ToInt32(_newdata["Sun"]);
                        if (_newdata["IsAccount"].ToString().Equals("1"))
                        {
                            _MonTotal += _newMon;
                            _TueTotal += _newTue;
                            _WedTotal += _newWed;
                            _ThuTotal += _newThu;
                            _FriTotal += _newFri;
                            _SatTotal += _newSat;
                            _SunTotal += _newSun;
                            WeekTotal += _newMon + _newTue + _newWed + _newThu + _newFri + _newSat + _newSun;
                            _accindex += 1;
                        }
                        else
                        {
                            if (!_gocus)
                            {
                                _firstcusindex = _index;
                                _gocus = true;
                            }
                            _cusMonTotal += _cusnewMon;
                            _cusTueTotal += _cusnewTue;
                            _cusWedTotal += _cusnewWed;
                            _cusThuTotal += _cusnewThu;
                            _cusFriTotal += _cusnewFri;
                            _cusSatTotal += _cusnewSat;
                            _cusSunTotal += _cusnewSun;
                            cusWeekTotal += _cusnewMon + _cusnewTue + _cusnewWed + _cusnewThu + _cusnewFri + _cusnewSat + _cusnewSun;
                            _cusindex += 1;
                        }
                        _index += 1;
                    }

                    if (_cusindex > 0 && _accindex > 0)//if (_firstcusindex > 0 && DDL_Type.SelectedValue.Equals("2"))
                    {
                        _AllMonTotal = _MonTotal + _cusMonTotal;
                        _AllTueTotal = _TueTotal + _cusTueTotal;
                        _AllWedTotal = _WedTotal + _cusWedTotal;
                        _AllThuTotal = _ThuTotal + _cusThuTotal;
                        _AllFriTotal = _FriTotal + _cusFriTotal;
                        _AllSatTotal = _SatTotal + _cusSatTotal;
                        _AllSunTotal = _SunTotal + _cusSunTotal;
                        AllWeekTotal = WeekTotal + cusWeekTotal;
                        DataRow _newrow = _newdt.NewRow();
                        _newrow["Mon"] = _MonTotal;
                        _newrow["Tue"] = _TueTotal;
                        _newrow["Wed"] = _WedTotal;
                        _newrow["Thu"] = _ThuTotal;
                        _newrow["Fri"] = _FriTotal;
                        _newrow["Sat"] = _SatTotal;
                        _newrow["Sun"] = _SunTotal;
                        _newrow["EntityTotal"] = Library.FN(WeekTotal.ToString());
                        _newrow["EntityName"] = "Total";
                        _newdt.Rows.InsertAt(_newrow, _firstcusindex);

                        _newrow = _newdt.NewRow();
                        _newrow["Mon"] = _cusMonTotal;
                        _newrow["Tue"] = _cusTueTotal;
                        _newrow["Wed"] = _cusWedTotal;
                        _newrow["Thu"] = _cusThuTotal;
                        _newrow["Fri"] = _cusFriTotal;
                        _newrow["Sat"] = _cusSatTotal;
                        _newrow["Sun"] = _cusSunTotal;
                        _newrow["EntityTotal"] = Library.FN(cusWeekTotal.ToString());
                        _newrow["EntityName"] = "Total";
                        _newdt.Rows.InsertAt(_newrow, dt.Rows.Count + 1);
                    }
                    gv_MLJ.DataSource = _newdt;
                    gv_MLJ.DataBind();

                    if (gv_MLJ.Rows.Count > 0)
                    {
                        gv_MLJ.FooterRow.Cells[0].Text = "Total";
                        if (_cusindex > 0 && _accindex > 0)
                        {
                            gv_MLJ.FooterRow.Cells[1].Text = Library.FN(_AllMonTotal.ToString());
                            gv_MLJ.FooterRow.Cells[2].Text = Library.FN(_AllTueTotal.ToString());
                            gv_MLJ.FooterRow.Cells[3].Text = Library.FN(_AllWedTotal.ToString());
                            gv_MLJ.FooterRow.Cells[4].Text = Library.FN(_AllThuTotal.ToString());
                            gv_MLJ.FooterRow.Cells[5].Text = Library.FN(_AllFriTotal.ToString());
                            gv_MLJ.FooterRow.Cells[6].Text = Library.FN(_AllSatTotal.ToString());
                            gv_MLJ.FooterRow.Cells[7].Text = Library.FN(_AllSunTotal.ToString());
                            gv_MLJ.FooterRow.Cells[8].Text = Library.FN(AllWeekTotal.ToString());
                            // gv_MLJ.FooterStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFC000");
                        }
                        else if (_accindex.Equals(0) && _cusindex > 0)
                        {
                            gv_MLJ.FooterRow.Cells[1].Text = Library.FN(_cusMonTotal.ToString());
                            gv_MLJ.FooterRow.Cells[2].Text = Library.FN(_cusTueTotal.ToString());
                            gv_MLJ.FooterRow.Cells[3].Text = Library.FN(_cusWedTotal.ToString());
                            gv_MLJ.FooterRow.Cells[4].Text = Library.FN(_cusThuTotal.ToString());
                            gv_MLJ.FooterRow.Cells[5].Text = Library.FN(_cusFriTotal.ToString());
                            gv_MLJ.FooterRow.Cells[6].Text = Library.FN(_cusSatTotal.ToString());
                            gv_MLJ.FooterRow.Cells[7].Text = Library.FN(_cusSunTotal.ToString());
                            gv_MLJ.FooterRow.Cells[8].Text = Library.FN(cusWeekTotal.ToString());
                        }
                        else
                        {
                            gv_MLJ.FooterRow.Cells[1].Text = Library.FN(_MonTotal.ToString());
                            gv_MLJ.FooterRow.Cells[2].Text = Library.FN(_TueTotal.ToString());
                            gv_MLJ.FooterRow.Cells[3].Text = Library.FN(_WedTotal.ToString());
                            gv_MLJ.FooterRow.Cells[4].Text = Library.FN(_ThuTotal.ToString());
                            gv_MLJ.FooterRow.Cells[5].Text = Library.FN(_FriTotal.ToString());
                            gv_MLJ.FooterRow.Cells[6].Text = Library.FN(_SatTotal.ToString());
                            gv_MLJ.FooterRow.Cells[7].Text = Library.FN(_SunTotal.ToString());
                            gv_MLJ.FooterRow.Cells[8].Text = Library.FN(WeekTotal.ToString());
                        }
                        //gv_MLJ.FooterRow.Cells[8].BackColor = System.Drawing.ColorTranslator.FromHtml("#fe9801");
                        gv_MLJ.FooterStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#775046");
                        gv_MLJ.FooterRow.HorizontalAlign = HorizontalAlign.Right;
                        if (_firstcusindex > 0)
                        {
                            gv_MLJ.Rows[_firstcusindex].BackColor = System.Drawing.ColorTranslator.FromHtml("#0000");
                            gv_MLJ.Rows[_firstcusindex].ForeColor = System.Drawing.Color.White;
                            gv_MLJ.Rows[_firstcusindex].Cells[8].BackColor = System.Drawing.ColorTranslator.FromHtml("#0000");
                            gv_MLJ.Rows[gv_MLJ.Rows.Count - 1].Cells[8].BackColor = System.Drawing.ColorTranslator.FromHtml("#0000");
                        }
                        //  gv_MLJ.FooterRow.Cells[0].ColumnSpan = 3;
                        //  gv_MLJ.FooterRow.Cells.RemoveAt(8);              
                    }
                    if (Session["Rowindex"] != null)
                        gv_MLJ.Rows[Convert.ToInt32(Session["Rowindex"])].BackColor = System.Drawing.ColorTranslator.FromHtml("#65a7d2");
                    up_gvGrid.Update();
                }
            }

        }

        protected void btn_Search_Click(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(1000);
            GetData();
        }

        protected void Tx_OnTextChanged(object sender, EventArgs e)
        {
            //TextBox _Tx = ((TextBox)sender);
            //int index = (_Tx.NamingContainer as GridViewRow).RowIndex;
            //int ID = Convert.ToInt32(gv_MLJ.DataKeys[index].Values[0].ToString());
            //changeList = (List<int>)Session["changeList"];
            //if(changeList==null)
            //    changeList = new List<int>() ;
            //changeList.Add(index);
            //Session["stringList"] = changeList;
            //if (_Tx.Text.Equals(""))
            //{
            //    Alert("Format error , please check your value or type 0 !");
            //    _Tx.Focus();
            //    return;
            //}
            //try
            //{
            //    if (!_Tx.Text.Equals(""))
            //    {
            //        Convert.ToDecimal(_Tx.Text);
            //    }
            //}
            //catch
            //{
            //    Alert("Format error , please check your value!");
            //    _Tx.Focus();
            //    return;
            //}
            //Update this journal
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchCustomers(string prefixText, int count)
        {//TODO 權限設定
            List<string> _entityName = new List<string>();
            PropertiesServiceClient _proclient = new PropertiesServiceClient();
            int _MLJEntityID = Convert.ToInt32((_proclient.GetPropertyValue2("MLJEntity"))[0].PropertyValue);
            EntityServiceClient _entityclient = new EntityServiceClient();
            EntityCollection _accountcollection = new EntityCollection(_entityclient.QueryAllMLJ(_MLJEntityID));
            var list = _accountcollection.Where(e => e.EntityName.ToUpper().StartsWith(prefixText) || e.EntityName.ToLower().StartsWith(prefixText));
            foreach (Entity _entity in list)
            {
                _entityName.Add(_entity.EntityName);
            }
            return _entityName;

        }

        protected void gv_MLJ_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                #region GetDataKeys
                string _id = gv_MLJ.DataKeys[e.Row.RowIndex].Values[0].ToString();
                string _status = gv_MLJ.DataKeys[e.Row.RowIndex].Values[2].ToString();
                string _Company = gv_MLJ.DataKeys[e.Row.RowIndex].Values[3].ToString();
                string _Account_Name = gv_MLJ.DataKeys[e.Row.RowIndex].Values[4].ToString();
                string _Password = gv_MLJ.DataKeys[e.Row.RowIndex].Values[5].ToString();
                string _Betting_Limit = gv_MLJ.DataKeys[e.Row.RowIndex].Values[6].ToString();
                string _Factor = gv_MLJ.DataKeys[e.Row.RowIndex].Values[7].ToString();
                string _Perbet = gv_MLJ.DataKeys[e.Row.RowIndex].Values[8].ToString();
                string _DateOpen = gv_MLJ.DataKeys[e.Row.RowIndex].Values[9].ToString();
                string _Personnel = gv_MLJ.DataKeys[e.Row.RowIndex].Values[10].ToString();
                string _IP = gv_MLJ.DataKeys[e.Row.RowIndex].Values[11].ToString();
                string _Odds = gv_MLJ.DataKeys[e.Row.RowIndex].Values[12].ToString();
                string _IssuesConditions = gv_MLJ.DataKeys[e.Row.RowIndex].Values[13].ToString();
                string _RemarksAcc = gv_MLJ.DataKeys[e.Row.RowIndex].Values[14].ToString();
                string _EntityName = gv_MLJ.DataKeys[e.Row.RowIndex].Values[15].ToString();
                string _Accountid = gv_MLJ.DataKeys[e.Row.RowIndex].Values[16].ToString();
                string _Color = gv_MLJ.DataKeys[e.Row.RowIndex].Values[17].ToString();
                #endregion

                #region Control
                TextBox _Tx_Mon = (TextBox)e.Row.Cells[1].FindControl("Tx_Mon");
                TextBox _Tx_Tue = (TextBox)e.Row.Cells[2].FindControl("Tx_Tue");
                TextBox _Tx_Wed = (TextBox)e.Row.Cells[3].FindControl("Tx_Wed");
                TextBox _Tx_Thu = (TextBox)e.Row.Cells[4].FindControl("Tx_Thu");
                TextBox _Tx_Fri = (TextBox)e.Row.Cells[5].FindControl("Tx_Fri");
                TextBox _Tx_Sat = (TextBox)e.Row.Cells[6].FindControl("Tx_Sat");
                TextBox _Tx_Sun = (TextBox)e.Row.Cells[7].FindControl("Tx_Sun");
                Label _Lb_Mon = (Label)e.Row.Cells[1].FindControl("Lb_Mon");
                Label _Lb_Tue = (Label)e.Row.Cells[2].FindControl("Lb_Tue");
                Label _Lb_Wed = (Label)e.Row.Cells[3].FindControl("Lb_Wed");
                Label _Lb_Thu = (Label)e.Row.Cells[4].FindControl("Lb_Thu");
                Label _Lb_Fri = (Label)e.Row.Cells[5].FindControl("Lb_Fri");
                Label _Lb_Sat = (Label)e.Row.Cells[6].FindControl("Lb_Sat");
                Label _Lb_Sun = (Label)e.Row.Cells[7].FindControl("Lb_Sun");

                LinkButton _LB_Tooltip = (LinkButton)e.Row.Cells[0].FindControl("LB_Tooltip");
                Literal _Literal_ToolTip = (Literal)e.Row.Cells[0].FindControl("Literal_ToolTip");
                #endregion


                if (RecordConfirm)
                {
                    _Tx_Mon.Enabled = false; _Tx_Tue.Enabled = false; _Tx_Wed.Enabled = false; _Tx_Thu.Enabled = false;
                    _Tx_Fri.Enabled = false; _Tx_Sat.Enabled = false; _Tx_Sun.Enabled = false;
                }
                //_Tx_Mon.Attributes.Add("onfocus", "try{document.getElementById('__LASTFOCUS').value=this.id}catch(e) {}");
                //_Tx_Tue.Attributes.Add("onfocus", "try{document.getElementById('__LASTFOCUS').value=this.id}catch(e) {}");
                //_Tx_Wed.Attributes.Add("onfocus", "try{document.getElementById('__LASTFOCUS').value=this.id}catch(e) {}");
                //_Tx_Thu.Attributes.Add("onfocus", "try{document.getElementById('__LASTFOCUS').value=this.id}catch(e) {}");
                //_Tx_Fri.Attributes.Add("onfocus", "try{document.getElementById('__LASTFOCUS').value=this.id}catch(e) {}");
                //_Tx_Sat.Attributes.Add("onfocus", "try{document.getElementById('__LASTFOCUS').value=this.id}catch(e) {}");
                //_Tx_Sun.Attributes.Add("onfocus", "try{document.getElementById('__LASTFOCUS').value=this.id}catch(e) {}");

                if (_EntityName.Equals("Total"))
                {
                    _Tx_Mon.Visible = false; _Tx_Tue.Visible = false; _Tx_Wed.Visible = false; _Tx_Thu.Visible = false;
                    _Tx_Fri.Visible = false; _Tx_Sat.Visible = false; _Tx_Sun.Visible = false;

                    _Lb_Mon.Visible = true; _Lb_Tue.Visible = true; _Lb_Wed.Visible = true; _Lb_Thu.Visible = true;
                    _Lb_Fri.Visible = true; _Lb_Sat.Visible = true; _Lb_Sun.Visible = true;
                    e.Row.BackColor = System.Drawing.Color.Black;
                    e.Row.ForeColor = System.Drawing.Color.White;
                    e.Row.Cells[0].Text = _EntityName;
                    // e.Row.Cells[0].ColumnSpan = 3;
                    //e.Row.Cells.RemoveAt(1);
                    //e.Row.Cells.RemoveAt(1);
                }
                if (!_Accountid.Equals("0") && !_Accountid.Equals(""))
                {
                    #region For Different Status
                    string _colorh = "#" + _Color;
                    e.Row.Cells[0].BackColor = System.Drawing.ColorTranslator.FromHtml(_colorh);
                    #endregion

                    #region For Tooltips
                    StringBuilder _tooltips = new StringBuilder();
                    _tooltips.Append("<a class=\"tooltip\" href=\"#\" style=\"text-decoration:none\">");
                    _tooltips.Append(_EntityName);
                    _tooltips.Append("<span class=\"custom info\"><img src=\"img\\Info.png\" alt=\"Information\" height=\"48\" width=\"48\" />");
                    _tooltips.AppendFormat("<em>{0}</em>", _EntityName);
                    _tooltips.Append("<div>");
                    _tooltips.AppendFormat("<li>Status：{0}</li>", _status);
                    _tooltips.AppendFormat("<li>Company：{0}</li>", _Company);
                    //_tooltips.AppendFormat("<li>Account_Name：{0}</li>", _Account_Name);
                    _tooltips.AppendFormat("<li>Password：{0}</li>", _Password);
                    _tooltips.AppendFormat("<li>Factor：{0}</li>", _Factor);
                    _tooltips.AppendFormat("<li>Per bet：{0}</li>", _Perbet);
                    _tooltips.AppendFormat("<li>Credit：{0}</li>", _Betting_Limit);
                    _tooltips.AppendFormat("<li>DateOpen：{0}</li>", _DateOpen);
                    _tooltips.AppendFormat("<li>Personnel：{0}</li>", _Personnel);
                    _tooltips.AppendFormat("<li>IP：{0}</li>", _IP);
                    _tooltips.AppendFormat("<li>Odds：{0}</li>", _Odds);
                    _tooltips.AppendFormat("<li>Issues/Conditions：{0}</li>", _IssuesConditions);
                    _tooltips.AppendFormat("<li>Remarks for Accounting：{0}</li>", _RemarksAcc);
                    _tooltips.Append("</div>");
                    _tooltips.Append("</span></a>");
                    _Literal_ToolTip.Text = _tooltips.ToString();

                    //_LB_Tooltip.CommandName = "ShowDetail";

                    #endregion
                }
                else
                {
                    _Literal_ToolTip.Text = _EntityName;
                    //e.Row.Cells[0].Text = _EntityName;
                    // e.Row.Cells[0].ColumnSpan = 3;
                    // e.Row.Cells.RemoveAt(1);
                    //e.Row.Cells.RemoveAt(1);
                }
            }
        }

        protected void btn_Approve_Click(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(1500);
            if (gv_MLJ.Rows.Count > 0)
            {
                int RecordID = Convert.ToInt32(gv_MLJ.DataKeys[0].Values[1].ToString());
                using (MLJServiceClient _client = new MLJServiceClient())
                {
                    _client.Approve(RecordID, SessionData.UserID);
                }
                GetData();
            }
        }

        protected void btn_Save_Click(object sender, EventArgs e)
        {
            if (gv_MLJ.Rows.Count.Equals(0))
            {
                Library.Alert("No data !!");
                return;
            }
            System.Threading.Thread.Sleep(1000);

            for (int i = 0; i < gv_MLJ.Rows.Count; i++)
            {
                if (!gv_MLJ.Rows[i].Cells[0].Text.Equals("Total"))
                {
                    int ID = Convert.ToInt32(gv_MLJ.DataKeys[i].Values[0].ToString());

                    DataTable dt = (DataTable)Session["AllDataTable"];

                    int _Mon = 0; int _Tue = 0; int _Wed = 0; int _Thu = 0; int _Fri = 0;
                    int _Sat = 0; int _Sun = 0;
                    int _oldMon = 0; int _oldTue = 0; int _oldWed = 0; int _oldThu = 0; int _oldFri = 0;
                    int _oldSat = 0; int _oldSun = 0;

                    TextBox _Tx_Mon = ((TextBox)gv_MLJ.Rows[i].Cells[1].FindControl("Tx_Mon"));
                    TextBox _Tx_Tue = ((TextBox)gv_MLJ.Rows[i].Cells[2].FindControl("Tx_Tue"));
                    TextBox _Tx_Wed = ((TextBox)gv_MLJ.Rows[i].Cells[3].FindControl("Tx_Wed"));
                    TextBox _Tx_Thu = ((TextBox)gv_MLJ.Rows[i].Cells[4].FindControl("Tx_Thu"));
                    TextBox _Tx_Fri = ((TextBox)gv_MLJ.Rows[i].Cells[5].FindControl("Tx_Fri"));
                    TextBox _Tx_Sat = ((TextBox)gv_MLJ.Rows[i].Cells[6].FindControl("Tx_Sat"));
                    TextBox _Tx_Sun = ((TextBox)gv_MLJ.Rows[i].Cells[7].FindControl("Tx_Sun"));

                    _Mon = (_Tx_Mon.Text.Equals("")) ? 0 : int.Parse(_Tx_Mon.Text.Replace(",", ""));
                    _Tue = (_Tx_Tue.Text.Equals("")) ? 0 : int.Parse(_Tx_Tue.Text.Replace(",", ""));
                    _Wed = (_Tx_Wed.Text.Equals("")) ? 0 : int.Parse(_Tx_Wed.Text.Replace(",", ""));
                    _Thu = (_Tx_Thu.Text.Equals("")) ? 0 : int.Parse(_Tx_Thu.Text.Replace(",", ""));
                    _Fri = (_Tx_Fri.Text.Equals("")) ? 0 : int.Parse(_Tx_Fri.Text.Replace(",", ""));
                    _Sat = (_Tx_Sat.Text.Equals("")) ? 0 : int.Parse(_Tx_Sat.Text.Replace(",", ""));
                    _Sun = (_Tx_Sun.Text.Equals("")) ? 0 : int.Parse(_Tx_Sun.Text.Replace(",", ""));
                    dt.DefaultView.RowFilter = "ID=" + ID + "";
                    DataTable oldDt = dt.DefaultView.ToTable();
                    _oldMon = Convert.ToInt32(oldDt.Rows[0]["Mon"]); _oldTue = Convert.ToInt32(oldDt.Rows[0]["Tue"]); _oldWed = Convert.ToInt32(oldDt.Rows[0]["Wed"]);
                    _oldThu = Convert.ToInt32(oldDt.Rows[0]["Thu"]); _oldFri = Convert.ToInt32(oldDt.Rows[0]["Fri"]); _oldSat = Convert.ToInt32(oldDt.Rows[0]["Sat"]);
                    _oldSun = Convert.ToInt32(oldDt.Rows[0]["Sun"]);
                    int _Batch_ID = Convert.ToInt32(oldDt.Rows[0]["RecordID"]);
                    int _Entity_ID = Convert.ToInt32(oldDt.Rows[0]["EntityID"]);
                    string _Base_Currency = oldDt.Rows[0]["BaseCurrency"].ToString();
                    decimal _Exchange_Rate = Convert.ToDecimal(oldDt.Rows[0]["ExchangeRate"]);

                    if (_Mon != _oldMon || _Tue != _oldTue || _Wed != _oldWed || _Thu != _oldThu || _Fri != _oldFri || _Sat != _oldSat || _Sun != _oldSun)
                    {
                        using (MLJServiceClient _client = new MLJServiceClient())
                        {
                            MLJJournal _jur = new MLJJournal();
                            _jur.SequenceNo = ID;
                            _jur.MLJRecordID = _Batch_ID;
                            _jur.EntityID = _Entity_ID;
                            _jur.BaseCurrency = _Base_Currency;
                            _jur.ExchangeRate = _Exchange_Rate;
                            _jur.Mon = _Mon;
                            _jur.Tue = _Tue;
                            _jur.Wed = _Wed;
                            _jur.Thu = _Thu;
                            _jur.Fri = _Fri;
                            _jur.Sat = _Sat;
                            _jur.Sun = _Sun;
                            User _user = new User();
                            _user.UserID = SessionData.UserID;
                            _jur.EntryUser = _user;
                            _client.UpdateJournal(_jur);
                        }
                    }
                    //switch (_Tx.ID)
                    //{
                    //    case "tx_Mon":
                    //        ((TextBox)gv_MLJ.Rows[index].Cells[2].FindControl("tx_Tue")).Focus();
                    //        break;
                    //    case "tx_Tue":
                    //        ((TextBox)gv_MLJ.Rows[index].Cells[3].FindControl("tx_Wed")).Focus();
                    //        break;
                    //    case "tx_Wed":
                    //        ((TextBox)gv_MLJ.Rows[index].Cells[4].FindControl("tx_Thu")).Focus();
                    //        break;
                    //    case "tx_Thu":
                    //        ((TextBox)gv_MLJ.Rows[index].Cells[5].FindControl("tx_Fri")).Focus();
                    //        break;
                    //    case "tx_Fri":
                    //        ((TextBox)gv_MLJ.Rows[index].Cells[6].FindControl("tx_Sat")).Focus();
                    //        break;
                    //    case "tx_Sat":
                    //        ((TextBox)gv_MLJ.Rows[index].Cells[7].FindControl("tx_Sun")).Focus();
                    //        break;
                    //}
                }
            }
            Library.Alert("Save Completed!!!");
            GetData();
        }

        protected void IB_Excel_Click(object sender, ImageClickEventArgs e)
        {
            GetData();
            if (Session["AllDataTable"] != null)
            {
                DataTable dt = new DataTable();
                dt = (DataTable)Session["AllDataTable"];
                dt.DefaultView.Sort = "IsAccount desc,EntityName";
                if (dt.Rows.Count > 0)
                {
                    var wb = new XLWorkbook();
                    var ws = wb.Worksheets.Add("MLJ");

                    #region Header

                    ws.Cell(1, 1).Value = "Username"; ws.Cell(1, 2).Value = "Code (MLJ)"; ws.Cell(1, 3).Value = "Code (Conf)"; ws.Cell(1, 4).Value = "Factor";
                    ws.Cell(1, 5).Value = "Per bet"; ws.Cell(1, 6).Value = "Credit"; ws.Cell(1, 7).Value = "Web"; ws.Cell(1, 8).Value = "Mon";
                    ws.Cell(1, 9).Value = "Tue"; ws.Cell(1, 10).Value = "Wed"; ws.Cell(1, 11).Value = "Thu"; ws.Cell(1, 12).Value = "Fri";
                    ws.Cell(1, 13).Value = "Sat"; ws.Cell(1, 14).Value = "Sun"; ws.Cell(1, 15).Value = "Total"; ws.Cell(1, 16).Value = "Date Open";
                    ws.Cell(1, 17).Value = "Personnel"; ws.Cell(1, 18).Value = "IP"; ws.Cell(1, 19).Value = "Odds"; ws.Cell(1, 20).Value = "Issues / Conditions";
                    ws.Cell(1, 21).Value = "Remarks for Accounting";

                    var rngTable = ws.Range("A1:U1");
                    rngTable.Style.Fill.BackgroundColor = XLColor.FromHtml("#775046");
                    rngTable.Style.Font.FontSize = 14;

                    #endregion

                    int _lastrow = 0;

                    foreach (DataRow _newdata in dt.Rows)
                    {
                        if (_newdata["IsAccount"].ToString().Equals("1"))
                            _lastrow += 1;
                    }

                    DataRow _newrow = dt.NewRow();
                    _newrow["EntityName"] = "Total";
                    dt.Rows.InsertAt(_newrow, _lastrow);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string _entityName = dt.Rows[i]["EntityName"].ToString();
                        ws.Cell(2 + i, 1).Value = _entityName;
                        ws.Cell(2 + i, 1).Style.Font.FontSize = 16;
                        ws.Cell(2 + i, 1).Style.Font.FontName = "Arabic Typesetting";

                        ws.Cell(2 + i, 8).Value = dt.Rows[i]["Mon"].ToString();
                        ws.Cell(2 + i, 9).Value = dt.Rows[i]["Tue"].ToString();
                        ws.Cell(2 + i, 10).Value = dt.Rows[i]["Wed"].ToString();
                        ws.Cell(2 + i, 11).Value = dt.Rows[i]["Thu"].ToString();
                        ws.Cell(2 + i, 12).Value = dt.Rows[i]["Fri"].ToString();
                        ws.Cell(2 + i, 13).Value = dt.Rows[i]["Sat"].ToString();
                        ws.Cell(2 + i, 14).Value = dt.Rows[i]["Sun"].ToString();
                        ws.Cell(2 + i, 15).FormulaA1 = string.Format("SUM(H{0}: N{1})", 2 + i, 2 + i);
                        #region Account Attribute
                        if (dt.Rows[i]["IsAccount"].ToString().Equals("1"))
                        {
                            string _color = "#" + dt.Rows[i]["Color"].ToString();
                            ws.Cell(2 + i, 1).Style.Fill.BackgroundColor = XLColor.FromHtml(_color);
                            ws.Cell(2 + i, 4).Value = dt.Rows[i]["Factor"].ToString();
                            ws.Cell(2 + i, 5).Value = dt.Rows[i]["Perbet"].ToString();
                            ws.Cell(2 + i, 5).Style.NumberFormat.Format = "#,##0";
                            ws.Cell(2 + i, 6).Value = dt.Rows[i]["Betting_Limit"].ToString();
                            ws.Cell(2 + i, 6).Style.NumberFormat.Format = "#,##0";
                            ws.Cell(2 + i, 7).Value = dt.Rows[i]["Company"].ToString();
                            ws.Cell(2 + i, 7).Style.Fill.BackgroundColor = XLColor.FromHtml("#92D050");
                            ws.Cell(2 + i, 16).Value = dt.Rows[i]["DateOpen"].ToString();
                            ws.Cell(2 + i, 17).Value = dt.Rows[i]["Personnel"].ToString();
                            ws.Cell(2 + i, 18).Value = dt.Rows[i]["IP"].ToString();
                            ws.Cell(2 + i, 19).Value = dt.Rows[i]["Odds"].ToString();
                            ws.Cell(2 + i, 20).Value = dt.Rows[i]["IssuesConditions"].ToString();
                            ws.Cell(2 + i, 21).Value = dt.Rows[i]["RemarksAcc"].ToString();
                        }
                        else
                        {
                            ws.Cell(2 + i, 2).Style.Fill.BackgroundColor = XLColor.FromHtml("#808080");
                            ws.Cell(2 + i, 3).Style.Fill.BackgroundColor = XLColor.FromHtml("#808080");
                            ws.Cell(2 + i, 4).Style.Fill.BackgroundColor = XLColor.FromHtml("#808080");
                            ws.Cell(2 + i, 5).Style.Fill.BackgroundColor = XLColor.FromHtml("#808080");
                            ws.Cell(2 + i, 6).Style.Fill.BackgroundColor = XLColor.FromHtml("#808080");
                            ws.Cell(2 + i, 7).Style.Fill.BackgroundColor = XLColor.FromHtml("#808080");
                        }
                        if (_entityName.Equals("Total"))
                        {
                            ws.Row(2 + i).Style.Fill.BackgroundColor = XLColor.Black;
                            ws.Row(2 + i).Style.Font.FontColor = XLColor.White;
                            ws.Row(2 + i).Style.Font.FontSize = 13;
                            ws.Row(2 + i).Style.Font.FontName = "Arial";
                            ws.Cell(2 + i, 15).Value = "";
                            ws.Cell(2 + i, 17).FormulaA1 = string.Format("SUM(H{0}: O{1})", 2, 2 + i);
                        }
                        #endregion
                    }

                    #region Style

                    #region Last Total

                    ws.Row(dt.Rows.Count + 2).InsertRowsBelow(1);
                    ws.Cell(dt.Rows.Count + 2, 1).Value = "Total";
                    ws.Row(dt.Rows.Count + 2).Style.Font.FontSize = 13;
                    ws.Row(dt.Rows.Count + 2).Style.Font.FontName = "Arial";
                    ws.Row(dt.Rows.Count + 2).Style.Fill.BackgroundColor = XLColor.FromHtml("#775046");
                    ws.Cell(dt.Rows.Count + 2, 8).FormulaA1 = string.Format("SUM(H{0}: H{1})", 2, dt.Rows.Count + 1);
                    ws.Cell(dt.Rows.Count + 2, 9).FormulaA1 = string.Format("SUM(I{0}: I{1})", 2, dt.Rows.Count + 1);
                    ws.Cell(dt.Rows.Count + 2, 10).FormulaA1 = string.Format("SUM(J{0}: J{1})", 2, dt.Rows.Count + 1);
                    ws.Cell(dt.Rows.Count + 2, 11).FormulaA1 = string.Format("SUM(K{0}: K{1})", 2, dt.Rows.Count + 1);
                    ws.Cell(dt.Rows.Count + 2, 12).FormulaA1 = string.Format("SUM(L{0}: L{1})", 2, dt.Rows.Count + 1);
                    ws.Cell(dt.Rows.Count + 2, 13).FormulaA1 = string.Format("SUM(M{0}: M{1})", 2, dt.Rows.Count + 1);
                    ws.Cell(dt.Rows.Count + 2, 14).FormulaA1 = string.Format("SUM(N{0}: N{1})", 2, dt.Rows.Count + 1);
                    ws.Cell(dt.Rows.Count + 2, 15).FormulaA1 = string.Format("SUM(O{0}: O{1})", 2, dt.Rows.Count + 1);

                    #endregion

                    //Filter 

                    ws.Column(17).SetAutoFilter();

                    var rngTable2 = ws.Range("A1:U" + dt.Rows.Count + 1);
                    ws.Rows().AdjustToContents();
                    ws.Columns().AdjustToContents();
                    rngTable2.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                    rngTable2.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    rngTable2.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    #endregion

                    // Prepare the response
                    HttpResponse httpResponse = Response;
                    httpResponse.Clear();
                    httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    httpResponse.AddHeader("content-disposition", "attachment;filename=\"MLJ.xlsx\"");

                    // Flush the workbook to the Response.OutputStream
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        wb.SaveAs(memoryStream);
                        memoryStream.WriteTo(httpResponse.OutputStream);
                        memoryStream.Close();
                    }

                    httpResponse.End();
                }
            }
        }

        protected void DDL_Status_SelectedIndexChanged(object sender, EventArgs e)
        {
            //using(MLJServiceClient _client = new MLJServiceClient())
            //{
            //    StatusColorCollection _coll = new StatusColorCollection(_client.QueryStatusColor());
            //    foreach (StatusColor _color in _coll)
            //    {
            //        if (_color.MLJStatus.ToString().Equals(DDL_Status.SelectedItem.Value))
            //        {
            //            string color = "#" + _color.MLJColor;
            //            DDL_Status.BackColor = System.Drawing.ColorTranslator.FromHtml(color);
            //        }

            //    }
            //}

        }

        protected void gv_MLJ_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // GridViewRow row = ((LinkButton)e.CommandSource).Parent.Parent as GridViewRow;
            GridViewRow row = ((ImageButton)e.CommandSource).Parent.Parent as GridViewRow;
            string _Status = gv_MLJ.DataKeys[row.RowIndex].Values[2].ToString();
            string _Remark = gv_MLJ.DataKeys[row.RowIndex].Values[14].ToString();
            string _EntityName = gv_MLJ.DataKeys[row.RowIndex].Values[15].ToString();
            string _EntityID = gv_MLJ.DataKeys[row.RowIndex].Values[18].ToString();
            int _MLJRecoredID = Convert.ToInt32(gv_MLJ.DataKeys[row.RowIndex].Values[1].ToString());
            int Period = 0;
            if (e.CommandName.Equals("ShowDetail"))
            {
                Session["Rowindex"] = row.RowIndex;
                DDL_DetailStatus.SelectedItem.Text = _Status;
                Tx_DetailRemark.Text = _Remark;
                LB_DetailName.Text = _EntityName;
                LB_ID.Text = _EntityID;
                //AccountStatus
                using (MLJServiceClient _client = new MLJServiceClient())
                {
                    MLJRecord _mljre = _client.QueryRecordByID(_MLJRecoredID);
                    Period = _mljre.Period.ID;
                }
                using (SystemDataServiceClient _sysclient = new SystemDataServiceClient())
                {
                    DataTable _statusDt = _sysclient.GetAccountStatusLog(Convert.ToInt32(_EntityID)).Tables[0];
                    gv_StatusRecord.DataSource = _statusDt;
                    gv_StatusRecord.DataBind();

                    DataTable _accDt = _sysclient.GetMLJLog(Period, _EntityName).Tables[0];
                    gv_AccountRecord.DataSource = _accDt;
                    gv_AccountRecord.DataBind();
                }

                up_Edit.Update();
                mp1.Show();
            }
            GetData();
        }

        protected void IB_Close_Click(object sender, ImageClickEventArgs e)
        {
            mp1.Hide();
            GetData();
        }

        protected void btn_SaveAccount_Click(object sender, EventArgs e)
        {
            using (EntityServiceClient _client = new EntityServiceClient())
            {
                Entity _Entity = new EntityCollection(_client.LoadEntity2(Convert.ToInt32(LB_ID.Text)))[0];
                Account _Account = new AccountCollection(_client.LoadAccount2(Convert.ToInt32(LB_ID.Text)))[0];
                _Account.Status = (Status)(Convert.ToInt32(DDL_DetailStatus.SelectedValue));
                _Account.RemarksAcc = Tx_DetailRemark.Text;
                _client.SaveEntity3(SessionData.UserID, _Entity, _Account);
                Library.Alert("Save Completed!!");
            }
            mp1.Hide();
            GetData();
        }

    }
}