using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Accounting_System.EntityServiceReference;
using Accounting_System.DataEntryServiceReference;
using Accounting_System.PeriodServiceReference;
using Accounting_System.LimitControlServiceReference;
using Accounting_System.CalculateServiceReference;
using Oleit.AS.Service.DataObject;
using System.Data;

namespace Accounting_System
{
    public partial class TransactionEntry : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {//ToDo
            CheckLimit.CheckPage(Request["menuid"]);
            tx_FromEntity.Attributes.Add("readonly","true");
            tx_ToEntity.Attributes.Add("readonly", "true");
            tx_Amount.Attributes.Add("OnKeyPress", "txtKeyNumber();");
            if (!IsPostBack)
            {
                Session["IsAdd"] = false;
                Session["Rowindex"] = null;
                //tree
                BindTree();
                PeriodServiceClient _pclient = new PeriodServiceClient();
                Period _period = new PeriodCollection(_pclient.GetCurrentPeriod())[0];
                GetData(_period);

            }
        }

        

        private void GetData(Period _period)
        {
            DataEntryServiceClient _client = new DataEntryServiceClient();
            TransactionCollection _collection = new TransactionCollection();
            if(!_period.ID.Equals(0))
                _collection = new TransactionCollection(_client.LoadTransactionByPeriodID(_period.ID));
            else
                _collection = new TransactionCollection(_client.LoadTransaction());
            DataTable dt = new DataTable();
            dt.Columns.Add("id"); dt.Columns.Add("FromEntity"); dt.Columns.Add("ToEntity"); dt.Columns.Add("FromEntityid");
            dt.Columns.Add("ToEntityid"); dt.Columns.Add("Amount"); dt.Columns.Add("Notice"); dt.Columns.Add("NoticeTime");
            dt.Columns.Add("Pay"); dt.Columns.Add("Confirm"); dt.Columns.Add("ConfirmTime"); dt.Columns.Add("Updater");
            dt.Columns.Add("Creator"); dt.Columns.Add("FromCurrency"); dt.Columns.Add("ToCurrency"); dt.Columns.Add("ExchangeRate");
            foreach (Transaction _tran in _collection)
            {
                DataRow newRow = dt.NewRow();
                newRow["id"] = _tran.ID;
                newRow["FromEntity"] = _tran.FromEntity.EntityName;
                newRow["ToEntity"] = _tran.ToEntity.EntityName;
                newRow["FromEntityid"] = _tran.FromEntity.EntityID;
                newRow["ToEntityid"] = _tran.ToEntity.EntityID;
                newRow["Amount"] = string.Format("{0:N2}",_tran.Amount);
                newRow["Notice"] = _tran.NoticeUser.UserName;
                if (!_tran.NoticeTime.ToShortDateString().Equals("0001/1/1"))
                    newRow["NoticeTime"] = _tran.NoticeTime;

                newRow["Pay"] = _tran.IsPay;
                newRow["Confirm"] = _tran.ConfirmUser.UserName;
                if (!_tran.ConfirmTime.ToShortDateString().Equals("0001/1/1"))
                newRow["ConfirmTime"] = _tran.ConfirmTime;
                newRow["Updater"] = _tran.Updater.UserName;
                newRow["Creator"] = _tran.Creator.UserName;

                newRow["FromCurrency"] = _tran.FromCurrency;
                newRow["ToCurrency"] = _tran.ToCurrency;
                newRow["ExchangeRate"] = _tran.ExchangeRate;

                dt.Rows.Add(newRow);
            }
            gv_Transaction.DataSource = dt;
            gv_Transaction.DataBind();
            up_gvGrid.Update();
        }

        private void BindTree()
        {
            List<MyObject> list = new List<MyObject>();
           
            EntityServiceClient _intclient = new EntityServiceClient();
            foreach (Entity entity in new EntityCollection(_intclient.LoadCashMainAndTransaction()))
            {
                list.Add(new MyObject() { Id = entity.EntityID, Name = entity.EntityName, ParentId = entity.ParentID, ToolTip = entity.Currency.CurrencyID });
            }
            //Session["TreeList"] = list;
            BindTree(list, null);
        }


        private void BindTree(IEnumerable<MyObject> list, TreeNode parentNode)
        {
            var nodes = list.Where(x => parentNode == null ? x.ParentId == 0 : x.ParentId == int.Parse(parentNode.Value));
            //var nodes = list;
            foreach (var node in nodes)
            {
                TreeNode newNode = new TreeNode(node.Name, node.Id.ToString());
                newNode.ToolTip = node.ToolTip;
                newNode.ImageToolTip = node.ParentId.ToString();
                if (parentNode == null)
                {
                    Tree_FromEntity.Nodes.Add(newNode);                    
                }
                else
                {
                    parentNode.ChildNodes.Add(newNode);
                }
                BindTree(list, newNode);
            }
            foreach (var node in nodes)
            {
                TreeNode newNode = new TreeNode(node.Name, node.Id.ToString());
                newNode.ToolTip = node.ToolTip;
                newNode.ImageToolTip = node.ParentId.ToString();
                if (parentNode == null)
                {                    
                    Tree_ToEntity.Nodes.Add(newNode);
                }
                else
                {
                    parentNode.ChildNodes.Add(newNode);
                }
                BindTree(list, newNode);
            }
        }

        public class MyObject
        {
            public int Id;
            public int ParentId;
            public string Name;
            public string ToolTip;
            
        }
        protected void btn_showFrom_Click(object sender, EventArgs e)
        {
            Panel_From.Visible = true;
            Panel_To.Visible = false;
        }
        
        protected void btn_showTo_Click(object sender, EventArgs e)
        {
            Panel_To.Visible = true;
            Panel_From.Visible = false;
        }

        protected void Tree_FromEntity_SelectedNodeChanged(object sender, EventArgs e)
        {
            if (Tree_FromEntity.SelectedNode.ImageToolTip.Equals("0"))
                return;
            tx_FromEntity.Text = Tree_FromEntity.SelectedNode.Text;
            lb_FromEntityID.Text = Tree_FromEntity.SelectedNode.Value;
            CalculateServiceClient _client = new CalculateServiceClient();
            WeeklySummary _week = new WeeklySummaryCollection(_client.GetWeeklySummary(Convert.ToInt32(Tree_FromEntity.SelectedNode.Value)))[0];           
            lb_FromCurrency.Text = _week.BaseCurrency;
            tx_FromAmount.Text = ((int)_week.BaseBalance).ToString();
            int _toAmount = 0; 
            try
            {
                _toAmount =(int)System.Math.Round(_week.BaseBalance * _week.ExchangeRate,1, MidpointRounding.AwayFromZero);
            }
            catch 
            {
                //Library.Alert(" Please Check Number ");
                _toAmount = 0;
            }
            tx_Amount.Text = _toAmount.ToString();
            Panel_From.Visible = false;
            CheckExchange();
        }

        protected void Tree_ToEntity_SelectedNodeChanged(object sender, EventArgs e)
        {
            if (Tree_ToEntity.SelectedNode.ImageToolTip.Equals("0"))
                return;
            tx_ToEntity.Text = Tree_ToEntity.SelectedNode.Text;
            lb_ToEntityID.Text = Tree_ToEntity.SelectedNode.Value;
            //CalculateServiceClient _client = new CalculateServiceClient();
            //WeeklySummary _week = new WeeklySummaryCollection(_client.GetWeeklySummary(Convert.ToInt32(Tree_ToEntity.SelectedNode.Value)))[0];
            //lb_ToCurrency.Text = _week.BaseCurrency;

            Panel_To.Visible = false;
            CheckExchange();
        }

        private void CheckExchange()
        {
            if (!tx_FromEntity.Text.Equals("") && !tx_ToEntity.Text.Equals(""))
            {
                if (!lb_FromCurrency.Text.Equals(lb_ToCurrency.Text))
                {
                    Panel_ExchangeRate.Visible = true;
                    if (tx_ExchangeRate.Text.Equals(""))
                    {
                        Alert("Please Enter Exchange Rate !!! ");
                        return;
                    }
                    try
                    {
                        Convert.ToDecimal(tx_ExchangeRate.Text);
                    }
                    catch
                    {
                        Alert("Please Correct Exchange Rate !!! ");
                        return;
                    }

                }
                else
                {
                    tx_ExchangeRate.Text = "1";
                    Panel_ExchangeRate.Visible = false;
                }
            }
        }
        
        protected void gv_Transaction_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            GridViewRow row = (GridViewRow)((Control)e.CommandSource).NamingContainer;
            int id = Convert.ToInt32(gv_Transaction.DataKeys[row.RowIndex].Values[0]);
            string _FromEntityID =gv_Transaction.DataKeys[row.RowIndex].Values[1].ToString();
            string _ToEntityID = gv_Transaction.DataKeys[row.RowIndex].Values[2].ToString();
            
            using (DataEntryServiceClient _client = new DataEntryServiceClient())
            {
                PeriodServiceClient _pclient = new PeriodServiceClient();
                Period _period = new PeriodCollection(_pclient.GetCurrentPeriod())[0];
                if (e.CommandName.Equals("Btn_Notice"))
                {
                    _client.SetNotices(id, Convert.ToInt32(Session["Userid"]));
                    GetData(_period);
                   
                }
                else if (e.CommandName.Equals("Btn_Confirm"))
                {
                    Transaction _tran = new TransactionCollection(_client.LoadTransactionByID(id))[0];
                    _tran.Updater.UserID = Convert.ToInt32(Session["Userid"]);
                    _tran.Period = _period;
                    _client.SetConfirm(_tran);
                    GetData(_period);
                    
                }
                else if (e.CommandName.Equals("Btn_Edit"))
                {
                    Session["IsAdd"] = false;                    
                    tx_FromEntity.Text = gv_Transaction.Rows[row.RowIndex].Cells[0].Text ;
                    tx_ToEntity.Text = gv_Transaction.Rows[row.RowIndex].Cells[2].Text;
                    tx_Amount.Text = gv_Transaction.Rows[row.RowIndex].Cells[5].Text;
                    lb_FromEntityID.Text = _FromEntityID;
                    lb_ToEntityID.Text = _ToEntityID;
                    lb_FromCurrency.Text = gv_Transaction.Rows[row.RowIndex].Cells[1].Text;
                    lb_ToCurrency.Text = gv_Transaction.Rows[row.RowIndex].Cells[3].Text;
                    tx_ExchangeRate.Text = gv_Transaction.Rows[row.RowIndex].Cells[4].Text;
                    btn_Confirm.Text = "";
                    lb_ID.Text = id.ToString();
                    up_Edit.Update();
                    Session["Rowindex"] = row.RowIndex;
                    mp1.Show();

                }
                gv_Transaction.Rows[row.RowIndex].BackColor = System.Drawing.ColorTranslator.FromHtml("#023e91");
            }
        }

        protected void gv_Transaction_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string _pay = gv_Transaction.DataKeys[e.Row.RowIndex].Values[3].ToString();
                if (_pay.Equals("Y"))
                {
                    Button _btn_Edit = (Button)e.Row.FindControl("Btn_Edit");
                    Button _btn_Notice = (Button)e.Row.FindControl("Btn_Notice");
                    Button _btn_Confirm = (Button)e.Row.FindControl("Btn_Confirm");
                    _btn_Edit.Visible = false;
                    _btn_Notice.Visible = false;
                    _btn_Confirm.Visible = false;
                }
                if (e.Row.Cells[6].Text.Equals("&nbsp;"))
                {
                    Button _btn_Confirm = (Button)e.Row.FindControl("Btn_Confirm");
                    _btn_Confirm.Visible = false;
                }
            }
        }
        
        protected void btn_Add_Click(object sender, EventArgs e)
        {            
            tx_FromEntity.Text = "";
            tx_ToEntity.Text = "";
            tx_Amount.Text = "";
            lb_FromCurrency.Text = "";
            lb_ToCurrency.Text = "";
            tx_ExchangeRate.Text = "";
            Session["IsAdd"] = true;
            up_Edit.Update();
            mp1.Show();
        }

        protected void btn_Confirm_Click(object sender, EventArgs e)
        {
            CheckExchange();
            if (tx_FromEntity.Text.Equals("") || tx_ToEntity.Text.Equals(""))
            {
                Alert(" You have to select [ From cloumn ] or [ To Entity ] . ");
                return;
            }
            if (tx_Amount.Text.Equals(""))
            {
                Alert(" You have to key in To Amount . ");
                return;
            }
            if (tx_FromAmount.Text.Equals(""))
            {
                Alert(" You have to key in From Amount . ");
                return;
            }
            string regex = "^[0-9]{0,5}$|^[0-9]{0,5}\\.[0-9]{0,2}$ ";
            System.Text.RegularExpressions.RegexOptions options = ((System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace | System.Text.RegularExpressions.RegexOptions.Multiline)
            | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(regex, options);
            if (!reg.IsMatch(tx_Amount.Text))
            {
                Alert(" Please check Amount column . ");
                return;
            }
            using (DataEntryServiceClient _client = new DataEntryServiceClient())
            {
                PeriodServiceClient _pclient = new PeriodServiceClient();
                Period _period = new PeriodCollection(_pclient.GetCurrentPeriod())[0];

                int _FromEntityID = Convert.ToInt32(lb_FromEntityID.Text);
                int _ToEntityID = Convert.ToInt32(lb_ToEntityID.Text);
                decimal _Amount = Convert.ToDecimal(tx_Amount.Text);
                decimal _FromAmount = Convert.ToDecimal(tx_FromAmount.Text);
                if ((bool)Session["IsAdd"])
                {
                    Transaction _tran = new Transaction();
                    _tran.Period.ID = _period.ID;
                    _tran.IsPay = IsPay.N;
                    _tran.Creator.UserID = Convert.ToInt32(Session["Userid"]);
                    _tran.Amount = _FromAmount;
                    _tran.FromEntity.EntityID = _FromEntityID;
                    _tran.ToEntity.EntityID = _ToEntityID;
                    _tran.FromCurrency = lb_FromCurrency.Text;
                    _tran.ToCurrency = lb_ToCurrency.Text;
                    _tran.ExchangeRate = Convert.ToDecimal(tx_ExchangeRate.Text);
                    _tran.To_Amount = _Amount;
                    _client.InsertTransaction(_tran);
                    mp1.Hide();
                    GetData(_period);
                }
                else
                {
                    Transaction _tran = new TransactionCollection(_client.LoadTransactionByID(Convert.ToInt32(lb_ID.Text)))[0];
                    _tran.FromEntity.EntityID = _FromEntityID;
                    _tran.ToEntity.EntityID = _ToEntityID;
                    _tran.Amount = _FromAmount;
                    _tran.To_Amount = _Amount;
                    _tran.FromCurrency = lb_FromCurrency.Text;
                    _tran.ToCurrency = lb_ToCurrency.Text;
                    _tran.ExchangeRate = Convert.ToDecimal(tx_ExchangeRate.Text);
                    _client.Updatetransaction(_tran);
                    mp1.Hide();
                    GetData(_period);
                   
                }
            }
            if(Session["Rowindex"]!=null)
                gv_Transaction.Rows[Convert.ToInt32(Session["Rowindex"])].BackColor = System.Drawing.ColorTranslator.FromHtml("#023e91");
        }
        private void Alert(string message)
        {
            string script = string.Format("alert('{0}');", HttpUtility.HtmlEncode(message.Replace("\n", "\\n")));
            JavaScrpit("window_aler", script);
        }
        private void JavaScrpit(string name, string script)
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), name, script, true);
        }

        protected void btn_Cancel_Click(object sender, EventArgs e)
        {
            PeriodServiceClient _pclient = new PeriodServiceClient();
            Period _period = new PeriodCollection(_pclient.GetCurrentPeriod())[0];
            mp1.Hide();
            GetData(_period);
            //gv_Transaction.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
            up_Edit.Update();
            if (Session["Rowindex"] != null)
                gv_Transaction.Rows[Convert.ToInt32(Session["Rowindex"])].BackColor = System.Drawing.ColorTranslator.FromHtml("#023e91");
        }

        protected void btn_Search_Click(object sender, EventArgs e)
        {
            Period _period = new Period();
            if(tx_Period.Text.Equals(""))
                GetData(_period);
            else
            {
                PeriodServiceClient _pclient = new PeriodServiceClient();
                try
                {
                    _period = new PeriodCollection(_pclient.DateOfPeriod(tx_Period.Text))[0];
                    GetData(_period);
                }
                catch (Exception)
                {
                    throw;                
                }
                
            }
        }

        protected void Btn_FromEntityFilter_Click(object sender, EventArgs e)
        {
           // BindTree((List<MyObject>)Session["TreeList"], null);
        }

        protected void Btn_ToEntityFilter_Click(object sender, EventArgs e)
        {
           // BindTree((List<MyObject>)Session["TreeList"], null);
        }

        protected void tx_FromAmount_TextChanged(object sender, EventArgs e)
        {
            SetToAmount();
        }
        protected void tx_ExchangeRate_TextChanged(object sender, EventArgs e)
        {
            SetToAmount();
        }
        private void SetToAmount()
        {
            int _toAmount = 0; int _fromAmount = 0; decimal _rate = 0;
            try
            {
                _rate = Convert.ToDecimal(tx_ExchangeRate.Text);
                _fromAmount = Convert.ToInt32(tx_FromAmount.Text);
                _toAmount = (int)System.Math.Round(_fromAmount * _rate, 1, MidpointRounding.AwayFromZero);
            }
            catch
            {
                Library.Alert(" Please Check Exchange Rate or From Amount ");
                _toAmount = 0;
            }
            tx_Amount.Text = _toAmount.ToString();
        }
        
    }
}