using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Accounting_System.EntityServiceReference;
using Accounting_System.TallyServiceReference;
using Accounting_System.PeriodServiceReference;
using Oleit.AS.Service.DataObject;
using ClosedXML.Excel;
using System.IO;

namespace Accounting_System
{
    public partial class Tally : System.Web.UI.Page
    {
        public string JsonEntityTreeString = "";
        public int UserId;
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckLimit.CheckPage(Request["menuid"]);
            UserId = int.Parse(SessionData.UserID.ToString());
            if (!IsPostBack)
            {
                //JsonTallyTreeString = LoadTallyTree();
                JsonEntityTreeString = JsonEntityFunc.LoadEntityTree();
                loadMainEntity();
                loadPeriod();

            }
        }
        private void loadPeriod()
        {
            var _psr = new PeriodServiceClient();
            var _periodCollection = _psr.GetPeriods().Select(p => p.PeriodNo);
            var _yearCollection = (from p in _periodCollection
                                   select new { Year = p.Substring(0, 4) }).GroupBy(x => x.Year).ToList().Select(x => x.Key);
            ddlYear.DataSource = _yearCollection;
            ddlYear.DataBind();

            string _currencyPeriod = _psr.GetCurrentPeriod()[0].PeriodNo;
            string[] _periodDate = _currencyPeriod.Split('.');
            ddlYear.SelectedValue = _periodDate[0];
            ddlMonth.SelectedValue = _periodDate[1];
            ddlWeek.SelectedValue = _periodDate[2];

        }

        private void loadMainEntity()
        {
            var _esr = new EntityServiceClient();
            var _entityCollection = _esr.LoadEntity1();
            var _mainEntityCollection = (from mec in _entityCollection.Where(x => x.ParentID == 0 && x.EntityType == EntityType.Cash).OrderByDescending(x => x.EntityID)
                                         select new ddlcls
                                         {
                                             Text = mec.EntityName,
                                             Value = mec.EntityID
                                         }).ToList();
            _mainEntityCollection.Insert(0, new ddlcls { Text = "please select", Value = 0 });
            ddlMainEntity.DataSource = _mainEntityCollection;
            ddlMainEntity.DataTextField = "Text";
            ddlMainEntity.DataValueField = "Value";
            ddlMainEntity.DataBind();
        }
        public class ddlcls
        {
            public string Text { get; set; }
            public int Value { get; set; }
        }

        protected void imgbtnExcel_Click(object sender, ImageClickEventArgs e)
        {

            var _esr = new EntityServiceClient();
            var _entityCollection = _esr.LoadEntity1();
            var _mainEntityCollection = _entityCollection.Where(x => x.ParentID == 0 && x.EntityType == EntityType.Cash).OrderByDescending(x => x.EntityID);

            string _sheetName = string.Format("{0}_{1}_{2}_{3}", "Tally", "Cash", "Period", DateTime.Now.ToString("yyyyMMddHHmm"));
            string _fileName = string.Format("{0}_{1}_{2}_{3}", "Tally", "Cash", "Period", DateTime.Now.ToString("yyyyMMddHHmm"));
            string _path = string.Format("{0}excel\\{1}.xlsx", Server.MapPath("~"), _fileName);
            var _xlWB = new XLWorkbook();
            var _ws = _xlWB.Worksheets.Add(_sheetName);

            var _psr = new PeriodServiceClient();
            string _period = string.Format("{0}.{1}.{2}", ddlYear.SelectedValue, ddlMonth.SelectedValue, ddlWeek.SelectedValue);

            _ws.Cell(1, 1).Value = string.Format("Cash Tally Report - Period {0}", _period);
            _ws.Cell(2, 1).Value = string.Format("Report Time: {0}", DateTime.Now.ToString("yyyy/MM/dd HH:mm"));

            _ws.Cell(3, 2).Value = "Currency";
            _ws.Cell(3, 3).Value = "Rate";
            _ws.Cell(3, 4).Value = "Previous Balance";
            _ws.Cell(3, 5).Value = "Win & Loss";
            _ws.Cell(3, 6).Value = "Transaction";
            _ws.Cell(3, 7).Value = "Final Balance(SGD)";
            _ws.Cell(3, 8).Value = "Final Balance(Currency)";
            _ws.Cell(3, 9).Value = "Checked";

            int _i = 4;
            var _tsr = new TallyServiceClient();
            foreach (var _entity in _mainEntityCollection)
            {
                var _loadEntity = _tsr.LoadEntity(_entity.EntityID);
                if (_loadEntity.m_Item1.Where(x => x.SubEntities.Count() == 0).Any())
                    continue;
                EntityCollection _ec = new EntityCollection();
                EntityCollection _ecTally = new EntityCollection(_loadEntity.m_Item1);
                var _newTallyCollection = EntitiesFunc.entityCollectioin(_ec, _ecTally);
                _ws.Row(_i).Style.Border.TopBorder = XLBorderStyleValues.Thick;
                foreach (var _tallyEntity in _newTallyCollection)
                {
                    _ws.Cell(_i, 1).Value = _tallyEntity.EntityName;
                    var _selectEntity = _loadEntity.m_Item2.Where(x => x.Entity.EntityID == _tallyEntity.EntityID);
                    if (_tallyEntity.ParentID != 0)
                    {
                        _ws.Cell(_i, 2).Value = _tallyEntity.Currency.CurrencyID;//Currency
                        _ws.Cell(_i, 3).Value = _tallyEntity.ExchangeRate;//Rate
                        _ws.Cell(_i, 3).Style.NumberFormat.Format = "0.000";
                        _ws.Cell(_i, 4).Value = _selectEntity.Single().BasePrevBalance;//Previous Balance
                        _ws.Cell(_i, 4).Style.NumberFormat.Format = "#,##0";
                        _ws.Cell(_i, 5).Value = _selectEntity.Single().BaseWinAndLoss;//Win & Loss
                        _ws.Cell(_i, 5).Style.NumberFormat.Format = "#,##0";
                        _ws.Cell(_i, 6).Value = _selectEntity.Single().BaseTransaction;//Transaction
                        _ws.Cell(_i, 6).Style.NumberFormat.Format = "#,##0";
                        _ws.Cell(_i, 7).Value = _selectEntity.Single().SGDBalance;//Final Balance(SGD)
                        _ws.Cell(_i, 7).Style.NumberFormat.Format = "#,##0";
                        _ws.Cell(_i, 8).Value = _selectEntity.Single().BaseBalance;//Final Balance(Currency)
                        _ws.Cell(_i, 8).Style.NumberFormat.Format = "#,##0";
                        _ws.Cell(_i, 9).Value = _selectEntity.Any(x => x.Status == WeeklySummaryStatus.Confirm) ? "V" : "";//Checked
                        _ws.Row(_i).Style.Font.FontSize = 12;
                    }
                    else
                    {
                        _ws.Cell(_i, 1).Style.Font.Bold = true;
                        _ws.Row(_i).Style.Font.FontSize = 14;
                    }
                    _i++;
                }
                _ws.Row(_i - 1).Style.Border.BottomBorder = XLBorderStyleValues.Thick;
                _i++;
            }

            _ws.SheetView.FreezeRows(3);
            _ws.Column("I").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            _ws.Column(1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            _ws.Row(3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            _ws.Row(3).Style.Font.FontSize = 14;
            _ws.Row(3).Style.Font.Bold = true;
            _ws.Rows().AdjustToContents();
            _ws.Columns().AdjustToContents();

            // Prepare the response
            HttpResponse httpResponse = Response;
            httpResponse.Clear();
            httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            httpResponse.AddHeader("content-disposition", string.Format("attachment;filename=\"{0}.xlsx\"", _fileName));

            // Flush the workbook to the Response.OutputStream
            using (MemoryStream memoryStream = new MemoryStream())
            {
                _xlWB.SaveAs(memoryStream);
                memoryStream.WriteTo(httpResponse.OutputStream);
                memoryStream.Close();
            }
            httpResponse.End();
        }

        
    }
}