using System;
using System.Collections.Generic;
using System.Linq;
using Oleit.AS.Service.DataObject;
using Accounting_System.CalculateServiceReference;
using Accounting_System.EntityServiceReference;
using Accounting_System.DataEntryServiceReference;
using Accounting_System.PeriodServiceReference;
using Accounting_System.PropertiesServiceReference;
using Accounting_System.SettleServiceReference;
using System.Text;
using Newtonsoft.Json;

namespace Accounting_System
{
    public partial class AmountTransferAjax : System.Web.UI.Page
    {
        public const string _deleteIcon = "<span title=\"delete\" onclick=\"deleteEntityCheck(this);\" class=\"ui-button-icon-primary ui-icon ui-icon-closethick \"></span>";
        protected void Page_Load(object sender, EventArgs e)
        {
            int _entityId;
            int _recordId;
            string _type = Request["type"];
            string _jsonEntity = Request["json"];
            int.TryParse(Request["entityId"], out _entityId);
            string _firstCheck = Request["first"];
            int.TryParse(Request["recordId"], out _recordId);
            if (_type == "Add")
                addEntity(_entityId, _firstCheck);
            else if (_type == "SubTotal")
            {
                subTotal(_jsonEntity);
            }
            else if (_type == "Save")
            {
                save(_jsonEntity, _entityId);
            }
            else if (_type == "Transfer")
            {
                transfer(_jsonEntity);
            }
            else if (_type == "Confirm")
            {
                confirm(_jsonEntity, _recordId);
            }

        }

        private void addEntity(int entityId, string firstCheck)
        {
            var _sbBefore = new StringBuilder();
            var _sbTransfer = new StringBuilder();
            var _sbResult = new StringBuilder();
            var _csr = new CalculateServiceClient();
            var _wsc = _csr.GetWeeklySummary(entityId);
            if (_wsc.Any())
            {
                try
                {
                    if (firstCheck == "true")
                    {
                        _sbBefore.AppendFormat(
                            "<tr entityId='{0}' id='trBeforeFirst'><td id='entityName{0}'>{1}</td><td id='currency{0}'>{2}</td><td id='er{0}'>{3}</td><td id='beforeBaseAmount{0}'>{4}</td><td id='beforeSGDAmount{0}'>{5}</td><td>{6}</td></tr>",
                            _wsc[0].Entity.EntityID, _wsc[0].Entity.EntityName, _wsc[0].Entity.Currency.CurrencyID,
                            _wsc[0].Entity.ExchangeRate, _wsc[0].BaseBalance, _wsc[0].SGDBalance, _deleteIcon);
                        _sbTransfer.AppendFormat("<tr entityid='{0}' id='trTransferFirst{0}'><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>", _wsc[0].Entity.EntityID);
                        _sbResult.AppendFormat("<tr><td></td><td></td></tr>");
                    }
                    else
                    {
                        _sbBefore.AppendFormat(
                            "<tr entityId='{0}' id='entityId{0}'><td id='entityName{0}'>{1}</td><td id='currency{0}'>{2}</td><td id='er{0}'>{3}</td><td id='beforeBaseAmount{0}'>{4}</td><td id='beforeSGDAmount{0}'>{5}</td><td>{6}</td></tr>",
                            _wsc[0].Entity.EntityID, _wsc[0].Entity.EntityName, _wsc[0].Entity.Currency.CurrencyID,
                            _wsc[0].Entity.ExchangeRate, _wsc[0].BaseBalance, _wsc[0].SGDBalance, _deleteIcon);

                        var _input = string.Format("<input type='text' id='txt{0}' value='{1}' onkeydown='return checkNum(event);' runat='server' />", _wsc[0].Entity.EntityID, _wsc[0].BaseBalance);
                        _sbTransfer.AppendFormat("<tr entityid='{0}' id='transferId{0}'><td id='transferBaseAmount{0}'>{1}</td><td id='transferSGDAmountId{0}'>&nbsp;</td><td id='transferPnLId{0}' >&nbsp;</td></tr>", _wsc[0].Entity.EntityID, _input);
                    }
                    Response.Write(string.Format("{0},{1}", _sbBefore, _sbTransfer));
                }
                catch (Exception)
                {
                    Response.Write("Fail!");
                }
            }
            else
            {
                #region "History"
                /*
                var _esr = new EntityServiceClient();
                var _entity = _esr.LoadEntity2(_entityId)[0];

                try
                {
                    if (_firstCheck == "true")
                    {
                        _sbBefore.AppendFormat(
                            "<tr entityId='{0}' id='trBeforeFirst'><td id='entityName{0}'>{1}</td><td id='currency{0}'>{2}</td><td id='er{0}'>{3}</td><td id='beforeBaseAmount{0}'>0</td><td id='beforeSGDAmount{0}'>0</td><td>{4}</td></tr>",
                            _entity.EntityID, _entity.EntityName, _entity.Currency.CurrencyID,
                            _entity.ExchangeRate, _deleteIcon);
                        _sbTransfer.AppendFormat("<tr entityid='{0}' id='trTransferFirst{0}'><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>", _entity.EntityID);
                    }
                    else
                    {
                        _sbBefore.AppendFormat(
                            "<tr entityId='{0}' id='entityId{0}'><td id='entityName{0}'>{1}</td><td id='currency{0}'>{2}</td><td id='er{0}'>{3}</td><td id='beforeBaseAmount{0}'>0</td><td id='beforeSGDAmount{0}'>0</td><td>{4}</td></tr>",
                            _entity.EntityID, _entity.EntityName, _entity.Currency.CurrencyID,
                            _entity.ExchangeRate, _deleteIcon);

                        var _input = string.Format("<input type='text' id='txt{0}' value='{1}' runat='server' />", _entity.EntityID, 0);
                        _sbTransfer.AppendFormat("<tr entityid='{0}' id='trTransferFirst{0}'><td id='transferBaseAmount{0}'>{1}</td><td id='transferSGDAmountId{0}'>&nbsp;</td><td id='transferPnLId{0}' >&nbsp;</td></tr>", _entity.EntityID, _input);
                    }
                    Response.Write(string.Format("{0},{1}", _sbBefore, _sbTransfer));
                }
                catch (Exception)
                {
                    Response.Write("error");
                }*/
                #endregion
                Response.Write("Fail! No Balance in this Entity!");
            }

        }

        private void subTotal(string jsonEntity)
        {
            try
            {
                var _sbBefore = new StringBuilder();//Before
                var _sbTransfer = new StringBuilder();//Transfer
                var _sbResult = new StringBuilder();//Result
                var _sbRecord = new StringBuilder();//Record
                var _amCollection = JsonConvert.DeserializeObject<AMCollection>(jsonEntity);
                var _cs = new CalculateServiceClient();
                var _esr = new EntityServiceClient();

                var firstOrDefault = _amCollection.entity.FirstOrDefault();
                if (firstOrDefault != null)
                {
                    var _entity = _esr.LoadEntity2(firstOrDefault.EntityId);
                    var _transfer = _cs.Subtotal(_amCollection.UserId, _entity[0]);
                    _transfer.ToEntity.SubEntities = new EntityCollection(_esr.QuerySubEntitiesList(_transfer.ToEntity.EntityID));
                    var _record = _transfer.RecordNotInDB.JournalCollection;

                    //Before
                    _sbBefore.AppendFormat(
                        "<tr  entityId='{0}' id='trBeforeFirst{0}'><td>{1}</td><td>{6}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td></tr>",
                        _transfer.ToEntity.EntityID, _transfer.ToEntity.EntityName, _transfer.ToEntity.ExchangeRate,
                        _transfer.BaseBefore, _transfer.SGDBefore, _deleteIcon, _transfer.Currency.CurrencyID);
                    //Transfer
                    _sbTransfer.AppendFormat(
                        "<tr  entityId='{0}' id='trTransferFirst{0}'><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>",
                        _transfer.ToEntity.EntityID);
                    //Result
                    _sbResult.AppendFormat("<tr  entityId='{0}' id='trResultFirst{0}'><td>{1}</td><td>{2}</td><tr>",
                                           _transfer.ToEntity.EntityID, _transfer.BaseResult, _transfer.SGDResult);

                    foreach (var t in _transfer.TransferDetailCollection)
                    {
                        string _inputBox =
                            string.Format(
                                "<input type='text' id='txt{0}' value='{1}' disabled='disabled' runat='server' />",
                                t.Entity.EntityID, t.BaseTransfer);
                        //Before
                        _sbBefore.AppendFormat(
                            "<tr  entityId='{0}' id='entityId{0}'><td id='entityName{0}'>{1}</td><td id='currency{0}'>{2}</td><td id='er{0}'>{3}</td><td id='beforeBaseAmount{0}'>{4}</td><td id='beforeSGDAmount{0}'>{5}</td><td>{6}</td></tr>",
                            t.Entity.EntityID, t.Entity.EntityName, t.BaseCurrency, t.ExchangeRate, t.BaseBefore,
                            t.SGDBefore, _deleteIcon);
                        //Transfer
                        _sbTransfer.AppendFormat(
                            "<tr  entityId='{0}' id='transferId{0}'><td id='transferBaseAmountId{0}'>{1}</td><td id='transferSGDAmountId{0}'>{2}</td><td id='transferPnLId{0}'>{3}</td><tr>",
                            t.Entity.EntityID, _inputBox, t.SGDTransfer, t.ProfitAndLoss);
                        //Result
                        _sbResult.AppendFormat(
                            "<tr  entityId='{0}' id='resultId{0}'><td id='resultBaseAmountId{0}'>{1}</td><td id='resultSGDAmountId{0}'>{2}</td><tr>",
                            t.Entity.EntityID, t.BaseResult, t.SGDResult);
                    }
                    //Record
                    var orDefault = _record.FirstOrDefault();

                    //Record first Jornal
                    if (orDefault != null)
                    {
                        string _entityType = _transfer.ToEntity.EntityType == EntityType.PAndL
                                                 ? "PnL"
                                                 : _transfer.ToEntity.EntityType == EntityType.Cash ? "Cash" : "Exp";
                        string _entityName = _transfer.ToEntity.EntityName;
                        _sbRecord.AppendFormat(
                            "<tr entityId='{0}'  id='trAfterFirst{0}'><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td></tr>",
                            orDefault.EntityID, _entityType, _entityName,orDefault.BaseCurrency, orDefault.ExchangeRate,
                            orDefault.BaseAmount, orDefault.SGDAmount);
                    }
                    //Record Jornals
                    if (_record.Count() > 1)
                    {
                        for (int i = 1; i < _record.Count(); i++)
                        {
                            var _entityCls = _transfer.ToEntity.SubEntities.Where(x => x.EntityID == _record[i].EntityID);
                            string _entityType = _entityCls.First().EntityType == EntityType.PAndL
                                                     ? "PnL"
                                                     : _entityCls.First().EntityType == EntityType.Cash ? "Cash" : "Exp";
                            string _entityName = _entityCls.First().EntityName;
                            _sbRecord.AppendFormat(
                                "<tr entityId='{0}'  id='afterId{0}'><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td></tr>",
                                _record[i].EntityID, _entityType, _entityName, _record[i].BaseCurrency,
                                _entityCls.First().ExchangeRate, _record[i].BaseAmount, _record[i].SGDAmount);
                        }
                    }
                    int _total = (int) _transfer.TransferDetailCollection.Sum(x => x.SGDBefore);
                    int _transferSGDSum = (int) _transfer.TransferDetailCollection.Sum(x => x.SGDTransfer);
                    int _transferPnLSum = (int) _transfer.TransferDetailCollection.Sum(x => x.ProfitAndLoss);
                    int _resultSGDSum = (int) _transfer.TransferDetailCollection.Sum(x => x.ProfitAndLoss);
                    int _subTotal = (int) _record.Sum(x => x.SGDAmount);
                    Response.Write(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", _sbBefore, _sbTransfer,
                                                 _sbResult, _sbRecord, _total, _transferSGDSum, _transferPnLSum,
                                                 _resultSGDSum, _subTotal));
                }
            }
            catch (Exception)
            {
                Response.Write("Fail!");
            }
        }

        private void save(string jsonEntity, int entityId)
        {
            var _amCollection = JsonConvert.DeserializeObject<AMCollection>(jsonEntity);
            var _des = new DataEntryServiceClient();
            int periodId = new PeriodServiceClient().GetCurrentPeriod()[0].ID;
            #region "TransferDetail"
            var _transferDetail = from t in _amCollection.entity.Where(x => x.EntityId != entityId)
                                  select new TransferDetail()
                                             {
                                                 BaseBefore = t.BeforeBaseAmount,
                                                 BaseResult = t.ResultBaseAmount,
                                                 BaseTransfer = t.TransferBaseAmount,
                                                 BaseCurrency = t.Currency,
                                                 ProfitAndLoss = t.PnL,
                                                 ExchangeRate = t.ER,
                                                 SGDBefore = t.BeforeSGDAmount,
                                                 SGDResult = t.ResultSGDAmount,
                                                 Entity = new Entity{EntityID = t.EntityId}
                                             };
            #endregion
            TransferDetailCollection _transferDetailCollection = new TransferDetailCollection(_transferDetail);
            var jsonTransferEntity = _amCollection.entity.FirstOrDefault();
            if (jsonTransferEntity != null)
            {
                #region "Tranfer"

                var _transfer = new Transfer
                                    {
                                        BaseBefore = jsonTransferEntity.BeforeBaseAmount,
                                        BaseResult = jsonTransferEntity.ResultBaseAmount,
                                        Currency = new Currency {CurrencyID = jsonTransferEntity.Currency},
                                        ExchangeRate = jsonTransferEntity.ER,
                                        SGDBefore = jsonTransferEntity.BeforeSGDAmount,
                                        TransferDetailCollection = _transferDetailCollection,
                                        ToEntity = new Entity{ EntityID= jsonTransferEntity.EntityId},
                                        SGDResult = jsonTransferEntity.ResultSGDAmount
                                    };

                #endregion

                #region "Journal"

                var _journal = from j in _amCollection.record
                               select new Journal
                                          {
                                              BaseAmount = j.BaseAmount,
                                              BaseCurrency = j.Currency,
                                              EntityID = j.EntityId,
                                              EntryUser = new User {UserID = _amCollection.UserId},
                                              ExchangeRate = j.ER,
                                              SGDAmount = j.SGDAmount,
                                          };

                #endregion

                #region "Record"

                JournalCollection _jc = new JournalCollection(_journal);
                var _record = new Record
                                         {
                                             JournalCollection = _jc,
                                             Period = new Period {ID = periodId},
                                             RecordStatus = RecordStatus.Normal,
                                             Type = RecordType.Transfer
                                         };

                #endregion

                try
                {
                    int _recordId = _des.InsertTransfer(_record, _transfer);
                    Response.Write(string.Format("{0},{1}", "Success!", _recordId));
                }
                catch (Exception)
                {

                    Response.Write("Fail!");
                }
                
            }
        }

        private void confirm(string jsonEntity, int recordId)
        {
            var _amCollection = JsonConvert.DeserializeObject<AMCollection>(jsonEntity);
            int _periodId = new PeriodServiceClient().GetCurrentPeriod()[0].ID;
            var _dsr = new DataEntryServiceClient();
            var _ssc = new SettleServiceClient();

            var _record = _dsr.LoadRecord(recordId);
            _ssc.TransferConfirm(_record,SessionData.UserID);
            try
            {

                Response.Write("Success!");
            }
            catch (Exception)
            {
                Response.Write("Fail!");
            }
            //#region "Weekly Summary"
            //var _ws = from w in _amCollection.record
            //          select new WeeklySummary
            //                     {
            //                         BaseTransfer = w.BaseAmount,
            //                         SGDTransfer = w.SGDAmount,
            //                         BaseCurrency = w.Currency,
            //                         Entity = new Entity {EntityID = w.EntityId},
            //                         ConfirmUser = new User {UserID = _amCollection.UserId},
            //                         ExchangeRate = w.ER,
            //                         Period = new Period{ID = _periodId}
            //                     };
            //#endregion
            //WeeklySummaryCollection _wsc = new WeeklySummaryCollection(_ws);

            //using (CalculateServiceClient _calculateService = new CalculateServiceClient())
            //{
            //    foreach (JsonTransferEntity _jsonTransferEntity in _amCollection.entity.Reverse())
            //    {
            //        WeeklySummary _weeklySummary = _calculateService.GetWeeklySummary(_jsonTransferEntity.EntityId)[0];

            //        _weeklySummary.BaseTransfer = _jsonTransferEntity.TransferBaseAmount;
            //        _weeklySummary.SGDTransfer = _jsonTransferEntity.TransferSGDAmount;

            //        _weeklySummary.BaseBalance = _jsonTransferEntity.ResultBaseAmount;
            //        _weeklySummary.SGDBalance = _jsonTransferEntity.ResultSGDAmount;

            //        _wsc.Insert(0, _weeklySummary);
            //    }
            //}by yang
            
        }

        private void transfer(string jsonEntity)
        {
            var _sbBefore = new StringBuilder();
            var _sbTransfer = new StringBuilder();
            var _sbResult = new StringBuilder();
            var _sbAfter = new StringBuilder();
            var _amCollection = JsonConvert.DeserializeObject<AMCollection>(jsonEntity);
            var _cs = new CalculateServiceClient();
            var _esr = new EntityServiceClient();
            
            var firstOrDefault = _amCollection.entity.FirstOrDefault();

             EntityCollection _entityNotConfirm = new EntityCollection();
            if (firstOrDefault.SumType == 1)
               _entityNotConfirm=  new EntityCollection(_esr.CheckTransactionTransfer(firstOrDefault.EntityId));

            bool _notConfirm = false;
            StringBuilder _notConfirmStr = new StringBuilder();
            if (_entityNotConfirm.Count > 0)
            {
                _notConfirmStr.Append("Fail!,<span style='color:red;font-weight:bold;'>These following SubTotal Entities are not confirmed.</span><br/><br/>");
                foreach (var _entity in _entityNotConfirm)
                {
                    if (_amCollection.entity.Any(x=>x.EntityId==_entity.EntityID))
                    {
                        _notConfirm = true;
                        _notConfirmStr.AppendFormat("{0}<br/>", _entity.EntityName);
                    }
                }
                _notConfirmStr.Append("<br/><span style='color:red;font-weight:bold;'>Please confirm them first.</span>");
            }

            if (_notConfirm)
            {
                Response.Write(_notConfirmStr.ToString());
            }
            else
            {
                if (firstOrDefault != null)
                {
                    var _entityIdAry = _amCollection.entity.Select(x => x.EntityId).ToArray();
                    var _entityCollection = _esr.LoadEntity4(_entityIdAry);

                    var _baseTransfer = _amCollection.entity.Select(x => x.TransferBaseAmount);

                    var _transferResult = _cs.Transfer(_amCollection.UserId, _entityCollection.ToArray(), _baseTransfer.ToArray());

                    var _transfer = _transferResult.m_Item1;
                    var _exchangeDiff = _transferResult.m_Item2;

                    var _record = _transfer.RecordNotInDB.JournalCollection;

                    _sbBefore.AppendFormat(
                        "<tr  entityId='{0}' id='trBeforeFirst{0}'><td>{1}</td><td>{6}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td></tr>",
                        _transfer.ToEntity.EntityID, _transfer.ToEntity.EntityName, _transfer.ToEntity.ExchangeRate,
                        _transfer.BaseBefore, _transfer.SGDBefore, _deleteIcon, _transfer.Currency.CurrencyID);
                    _sbTransfer.AppendFormat(
                        "<tr  entityId='{0}' id='trTransferFirst{0}'><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>",
                        _transfer.ToEntity.EntityID);
                    _sbResult.AppendFormat("<tr  entityId='{0}' id='trResultFirst{0}'><td>{1}</td><td>{2}</td><tr>",
                                           _transfer.ToEntity.EntityID, _transfer.BaseResult, _transfer.SGDResult);

                    foreach (var t in _transfer.TransferDetailCollection)
                    {
                        string _inputBox =
                            string.Format(
                                "<input type='text' id='txt{0}' value='{1}' disabled='disabled' runat='server' />",
                                t.Entity.EntityID, t.BaseTransfer);
                        _sbBefore.AppendFormat(
                            "<tr  entityId='{0}' id='entityId{0}'><td id='entityName{0}'>{1}</td><td id='currency{0}'>{2}</td><td id='er{0}'>{3}</td><td id='beforeBaseAmount{0}'>{4}</td><td id='beforeSGDAmount{0}'>{5}</td><td>{6}</td></tr>",
                            t.Entity.EntityID, t.Entity.EntityName, t.BaseCurrency, t.ExchangeRate, t.BaseBefore,
                            t.SGDBefore, _deleteIcon);
                        _sbTransfer.AppendFormat(
                            "<tr  entityId='{0}' id='transferId{0}'><td id='transferBaseAmountId{0}'>{1}</td><td id='transferSGDAmountId{0}'>{2}</td><td id='transferPnLId{0}'>{3}</td><tr>",
                            t.Entity.EntityID, _inputBox, t.SGDTransfer, t.ProfitAndLoss);
                        _sbResult.AppendFormat(
                            "<tr  entityId='{0}' id='resultId{0}'><td id='resultBaseAmountId{0}'>{1}</td><td id='resultSGDAmountId{0}'>{2}</td><tr>",
                            t.Entity.EntityID, t.BaseResult, t.SGDResult);
                    }
                    var orDefault = _record.FirstOrDefault();

                    if (orDefault != null)
                    {
                        string _entityType = _transfer.ToEntity.EntityType == EntityType.PAndL
                                                 ? "PnL"
                                                 : _transfer.ToEntity.EntityType == EntityType.Cash ? "Cash" : "Exp";
                        string _entityName = _transfer.ToEntity.EntityName;
                        _sbAfter.AppendFormat(
                            "<tr entityId='{0}'  id='trAfterFirst{0}'><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td></tr>",
                            orDefault.EntityID, _entityType, _entityName, orDefault.BaseCurrency, orDefault.ExchangeRate,
                            orDefault.BaseAmount, orDefault.SGDAmount);
                    }
                    if (_record.Count() > 1)
                    {
                        for (int i = 1; i < _record.Count(); i++)
                        {
                            Entity[] _entityCls = new Entity[] { };
                            if (i != _record.Count() - 1)
                            {

                                _entityCls = _entityCollection.Where(x => x.EntityID == _record[i].EntityID).ToArray();
                            }
                            else
                            {
                                _entityCls = new Entity[] { _exchangeDiff };
                            }
                            string _entityType = _entityCls.First().EntityType == EntityType.PAndL
                                                     ? "PnL"
                                                     : _entityCls.First().EntityType == EntityType.Cash ? "Cash" : "Exp";
                            string _entityName = _entityCls.First().EntityName;

                            _sbAfter.AppendFormat(
                                "<tr entityId='{0}'  id='afterId{0}'><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td></tr>",
                                _record[i].EntityID, _entityType, _entityName, _record[i].BaseCurrency,
                                _entityCls.First().ExchangeRate, _record[i].BaseAmount, _record[i].SGDAmount);
                        }
                    }
                    int _total = (int)_transfer.TransferDetailCollection.Sum(x => x.SGDBefore) + (int)_amCollection.entity.First().BeforeSGDAmount;
                    int _transferSGDSum = (int)_transfer.TransferDetailCollection.Sum(x => x.SGDTransfer);
                    int _transferPnLSum = (int)_transfer.TransferDetailCollection.Sum(x => x.ProfitAndLoss);
                    int _resultSGDSum = (int)_transfer.TransferDetailCollection.Sum(x => x.SGDResult) + (int)_amCollection.entity.First().ResultSGDAmount;
                    int _subTotal = (int)_record.Sum(x => x.SGDAmount);
                    Response.Write(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", _sbBefore, _sbTransfer, _sbResult,
                                                 _sbAfter, _total, _transferSGDSum, _transferPnLSum, _resultSGDSum,
                                                 _subTotal));
                }
            }
        }

        public class AMCollection
        {
            public IEnumerable<JsonTransferEntity> entity { get; set; }
            public IEnumerable<JsonRecord> record { get; set; }
            public int UserId { get; set; }
        }

        public class JsonTransferEntity
        {
            public int EntityId { get; set; }
            public int SumType { get; set; }
            public string EntityType { get; set; }
            public string Currency { get; set; }
            public decimal ER { get; set; }
            public decimal BeforeBaseAmount { get; set; }
            public decimal BeforeSGDAmount { get; set; }
            public decimal TransferBaseAmount { get; set; }
            public decimal TransferSGDAmount { get; set; }
            public decimal ResultBaseAmount { get; set; }
            public decimal ResultSGDAmount { get; set; }
            public decimal PnL { get; set; }
        }

        public class JsonRecord
        {
            public int EntityId { get; set; }
            public string Currency { get; set; }
            public decimal ER { get; set; }
            public decimal BaseAmount { get; set; }
            public decimal SGDAmount { get; set; }
        }
    }
}