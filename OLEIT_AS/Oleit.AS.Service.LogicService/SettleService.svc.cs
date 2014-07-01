using Oleit.AS.Service.DataObject;
using Oleit.AS.Service.LogicService.PropertyAccessReference;
using Oleit.AS.Service.LogicService.RecordAccessReference;
using Oleit.AS.Service.LogicService.PeriodAccessReference;
using Oleit.AS.Service.LogicService.WeeklySummaryReference;
using Oleit.AS.Service.LogicService.MLJRecordAccessReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Transactions;

namespace Oleit.AS.Service.LogicService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SettleService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select SettleService.svc or SettleService.svc.cs at the Solution Explorer and start debugging.
    public class SettleService : ISettleService
    {
        public static volatile SettleService Instance = new SettleService();

        public void DoWork()
        {
        }

        public void WeeklySummarize(Entity entity)
        {
            throw new NotImplementedException();
        }

        public void TransactionConfirm(WeeklySummaryCollection weeklySummaryCollection)
        {
            foreach (WeeklySummary _week in weeklySummaryCollection)
            {
                using (WeeklySummaryAccessClient _weekAccessClient = new WeeklySummaryAccessClient(EndpointName.WeeklySummaryAccess))
                {
                    //Get this Period WeeklySummary
                    PeriodAccessClient _periodacc = new PeriodAccessClient();
                    Period _lastperiod = new PeriodCollection(_periodacc.Query4(_week.Period.PeriodNo,-1))[0];
                    WeeklySummaryCollection _thisweek = new WeeklySummaryCollection(_weekAccessClient.Query(_week.Period.ID, _week.Entity.EntityID));
                    WeeklySummaryCollection _lastweek = new WeeklySummaryCollection(_weekAccessClient.Query(_lastperiod.ID, _week.Entity.EntityID));
                    if (_thisweek.Count > 0)
                    {
                        decimal _basepre = 0;
                        decimal _basewl = 0;
                        decimal _basetransfer = 0;
                        decimal _basebalance = 0;
                        decimal _basetran = 0;

                        decimal _sgdpre = 0;
                        decimal _sgdwl = 0;
                        decimal _sgdtransfer = 0;
                        decimal _sgdbalance = 0;
                        decimal _sgdtran = 0;

                        if (!_thisweek[0].BasePrevBalance.Equals(""))
                            _basepre = _thisweek[0].BasePrevBalance;
                        if (!_thisweek[0].BaseTransfer.Equals(""))
                            _basetransfer = _thisweek[0].BaseTransfer;
                        if (!_thisweek[0].BaseWinAndLoss.Equals(""))
                            _basewl = _thisweek[0].BaseWinAndLoss;
                        if (!_week.BaseTransaction.Equals(""))
                            _basetran = _thisweek[0].BaseTransaction+_week.BaseTransaction;

                        _basebalance = _basepre + _basetransfer + _basewl + _basetran;

                        if (!_thisweek[0].SGDPrevBalance.Equals(""))
                            _sgdpre = _thisweek[0].SGDPrevBalance;
                        if (!_thisweek[0].SGDTransfer.Equals(""))
                            _sgdtransfer = _thisweek[0].SGDTransfer;
                        if (!_thisweek[0].SGDWinAndLoss.Equals(""))
                            _sgdwl = _week.SGDWinAndLoss;
                        if (!_thisweek[0].SGDTransaction.Equals(""))
                            _sgdtran = _thisweek[0].SGDTransaction + (_week.SGDTransaction/_thisweek[0].ExchangeRate);

                        _sgdbalance = _sgdpre + _sgdtransfer + _sgdwl + _sgdtran;

                        _thisweek[0].BaseTransaction = _basetran;
                        _thisweek[0].BaseBalance = _basebalance;
                        _thisweek[0].SGDBalance = _sgdbalance;
                        _thisweek[0].ConfirmUser = _week.ConfirmUser;
                        _thisweek[0].Status = WeeklySummaryStatus.None;
                        _weekAccessClient.Update1(_thisweek[0]);
                    }
                    else
                    {
                        WeeklySummary _newweek = new WeeklySummary();
                        if (_lastweek.Count > 0)
                        {
                            _newweek.BasePrevTransaction = _lastweek[0].BaseTransaction;
                            _newweek.SGDPrevTransaction = _lastweek[0].SGDTransaction;
                            _newweek.BasePrevBalance = _lastweek[0].BaseBalance;
                            _newweek.SGDPrevBalance = _lastweek[0].SGDBalance;
                        }
                        else
                        {
                            _newweek.BasePrevTransaction = 0;
                            _newweek.SGDPrevTransaction = 0;
                            _newweek.BasePrevBalance = 0;
                            _newweek.SGDPrevBalance = 0;
                        }
                        _newweek.Period.ID = _week.Period.ID;
                        _newweek.Entity = _week.Entity;
                        _newweek.BaseCurrency = _week.BaseCurrency;
                        _newweek.ExchangeRate = _week.ExchangeRate;                        
                        
                        _newweek.BaseWinAndLoss = 0;
                        _newweek.SGDWinAndLoss = 0;
                        _newweek.BaseTransfer = 0;
                        _newweek.SGDTransfer = 0;
                        _newweek.BaseTransaction = _week.BaseTransaction;
                        _newweek.SGDTransaction = 0;
                        _newweek.BaseBalance = _week.BaseTransaction;
                        _newweek.SGDBalance = 0;
                        _newweek.Status = WeeklySummaryStatus.None;
                        
                        _newweek.ConfirmUser = new User();
                        _weekAccessClient.Insert1(_newweek);
                    }
                }              
            }

        }

        public void WinLossConfirm(WeeklySummaryCollection weeklySummaryCollection, int recordID)
        {
            foreach (WeeklySummary _week in weeklySummaryCollection)
            {
                using (WeeklySummaryAccessClient _weekAccessClient = new WeeklySummaryAccessClient(EndpointName.WeeklySummaryAccess))
                {
                    //Get this Period WeeklySummary
                    WeeklySummaryCollection _thisweek = new WeeklySummaryCollection(_weekAccessClient.Query(_week.Period.ID, _week.Entity.EntityID));
                    if (_thisweek.Count > 0)
                    {                        
                        decimal _basepre=0;
                        decimal _basewl = 0;
                        decimal _basetransfer = 0;
                        decimal _basebalance = 0;
                        decimal _basetran = 0;

                        decimal _sgdpre = 0;
                        decimal _sgdwl = 0;
                        decimal _sgdtransfer = 0;
                        decimal _sgdbalance = 0;
                        decimal _sgdtran = 0;

                        if(!_thisweek[0].BasePrevBalance.Equals("")) 
                            _basepre = _thisweek[0].BasePrevBalance;
                        if(!_thisweek[0].BaseTransfer.Equals(""))
                            _basetransfer = _thisweek[0].BaseTransfer;
                        if(!_week.BaseWinAndLoss.Equals(""))         
                            _basewl = _week.BaseWinAndLoss;
                        if (!_week.BaseTransaction.Equals(""))
                            _basetran = _thisweek[0].BaseTransaction;

                        _basebalance = _basepre + _basetransfer + _basewl + _basetran;

                        if (!_thisweek[0].SGDPrevBalance.Equals(""))
                            _sgdpre = _thisweek[0].SGDPrevBalance;
                        if (!_thisweek[0].SGDTransfer.Equals(""))
                            _sgdtransfer = _thisweek[0].SGDTransfer;
                        if (!_week.SGDWinAndLoss.Equals(""))
                            _sgdwl = _week.SGDWinAndLoss;
                        if (!_thisweek[0].SGDTransaction.Equals(""))
                            _sgdtran = _thisweek[0].SGDTransaction;

                        _sgdbalance = _sgdpre + _sgdtransfer + _sgdwl + _sgdtran;

                        //_week.BasePrevBalance = _thisweek[0].BasePrevBalance;
                        //_week.SGDPrevBalance = _thisweek[0].SGDPrevBalance;
                        //_week.BaseTransfer = _thisweek[0].BaseTransfer;
                        _thisweek[0].BaseWinAndLoss = _week.BaseWinAndLoss;
                        _thisweek[0].SGDWinAndLoss = _week.SGDWinAndLoss;

                       // _week.SGDTransfer = 
                        _thisweek[0].BaseCurrency = _week.BaseCurrency;
                        _thisweek[0].ExchangeRate = _week.ExchangeRate;
                        _thisweek[0].BaseBalance = _basebalance;
                        _thisweek[0].SGDBalance = _sgdbalance;
                        _thisweek[0].ConfirmUser = _week.ConfirmUser;
                        _thisweek[0].Status = WeeklySummaryStatus.None;
                        _weekAccessClient.Update1(_thisweek[0]);
                    }
                    else
                    {
                        WeeklySummary _newweek = new WeeklySummary();
                        _newweek.Period.ID = _week.Period.ID;
                        _newweek.Entity = _week.Entity;
                        _newweek.BaseCurrency = _week.BaseCurrency;
                        _newweek.ExchangeRate = _week.ExchangeRate;
                        _newweek.BasePrevBalance = 0;
                        _newweek.SGDPrevBalance = 0;
                        _newweek.BaseWinAndLoss = _week.BaseWinAndLoss;
                        _newweek.SGDWinAndLoss = _week.SGDWinAndLoss;
                        _newweek.BaseTransfer = 0;
                        _newweek.SGDTransfer = 0;
                        _newweek.BaseTransaction = 0;
                        _newweek.SGDTransaction = 0;
                        _newweek.BaseBalance = _week.BaseWinAndLoss;
                        _newweek.SGDBalance = _week.SGDWinAndLoss;
                        _newweek.Status = WeeklySummaryStatus.None;
                        _newweek.BasePrevTransaction = _week.BaseTransaction;
                        _newweek.SGDPrevTransaction = _week.SGDTransaction;
                        _newweek.ConfirmUser = new User();
                        _weekAccessClient.Insert1(_newweek);
                    }
                }
                using (RecordAccessClient _recordclient = new RecordAccessClient(EndpointName.RecordAccess))
                {
                    _recordclient.ChangeStatus(recordID, RecordStatus.Confirm);
                }
            }
        
        }

        public void TransferConfirm(Record record,int userID)
        {
            foreach (Journal _journal in record.JournalCollection)
            {
                using (WeeklySummaryAccessClient _weekAccessClient = new WeeklySummaryAccessClient(EndpointName.WeeklySummaryAccess))
                {
                    //Get this Period WeeklySummary
                    WeeklySummaryCollection _thisweek = new WeeklySummaryCollection(_weekAccessClient.Query(record.Period.ID,_journal.EntityID));
                    if (_thisweek.Count > 0)
                    {
                        decimal _newbasetransfer = 0;
                        decimal _newsgdtransfer = 0;

                        decimal _basepre=0;
                        decimal _basewl = 0;
                        decimal _basetransfer = 0;
                        decimal _basebalance = 0;
                        decimal _basetran = 0;

                        decimal _sgdpre = 0;
                        decimal _sgdwl = 0;
                        decimal _sgdtransfer = 0;
                        decimal _sgdbalance = 0;
                        decimal _sgdtran = 0;

                        if(!_thisweek[0].BasePrevBalance.Equals("")) 
                            _basepre = _thisweek[0].BasePrevBalance;
                        // Journal Transfer
                        if(!_thisweek[0].BaseTransfer.Equals(""))
                            _basetransfer = _thisweek[0].BaseTransfer;
                       
                        _newbasetransfer = _journal.BaseAmount;

                        if (!_thisweek[0].BaseWinAndLoss.Equals(""))
                            _basewl = _thisweek[0].BaseWinAndLoss;
                        if (!_thisweek[0].BaseTransaction.Equals(""))
                            _basetran = _thisweek[0].BaseTransaction;

                        _basebalance = _basepre + _basetransfer + _basewl + _basetran + _newbasetransfer;

                        if (!_thisweek[0].SGDPrevBalance.Equals(""))
                            _sgdpre = _thisweek[0].SGDPrevBalance;
                        // Journal Transfer
                        if (!_thisweek[0].SGDTransfer.Equals(""))
                            _sgdtransfer = _thisweek[0].SGDTransfer;
                        _newsgdtransfer = _journal.SGDAmount;

                        if (!_thisweek[0].SGDWinAndLoss.Equals(""))
                            _sgdwl = _thisweek[0].SGDWinAndLoss;
                        if (!_thisweek[0].SGDTransaction.Equals(""))
                            _sgdtran = _thisweek[0].SGDTransaction;

                        _sgdbalance = _sgdpre + _sgdtransfer + _sgdwl + _sgdtran + _newsgdtransfer;

                      //  _thisweek[0].BasePrevBalance = _thisweek[0].BasePrevBalance;
                        _thisweek[0].SGDTransfer = _journal.SGDAmount + _thisweek[0].SGDTransfer;
                        _thisweek[0].BaseTransfer = _journal.BaseAmount + _thisweek[0].BaseTransfer;

                       //  
                        _thisweek[0].BaseCurrency = _thisweek[0].BaseCurrency;
                        _thisweek[0].ExchangeRate = _thisweek[0].ExchangeRate;
                        _thisweek[0].BaseWinAndLoss = _thisweek[0].BaseWinAndLoss;
                        _thisweek[0].SGDWinAndLoss = _thisweek[0].SGDWinAndLoss;
                        _thisweek[0].BaseBalance = _basebalance;
                        _thisweek[0].SGDBalance = _sgdbalance;
                        _thisweek[0].ConfirmUser.UserID = userID;
                        _thisweek[0].Status = WeeklySummaryStatus.None;
                        _weekAccessClient.Update1(_thisweek[0]);
                    }
                    else
                    {
                        WeeklySummary _newweek = new WeeklySummary();
                        _newweek.Period.ID = record.Period.ID;
                        _newweek.Entity.EntityID = _journal.EntityID;
                        _newweek.BaseCurrency = _journal.BaseCurrency;
                        _newweek.ExchangeRate = _journal.ExchangeRate;
                        _newweek.BasePrevBalance = 0;
                        _newweek.SGDPrevBalance = _journal.SGDAmount;
                        _newweek.BaseWinAndLoss = 0;
                        _newweek.SGDWinAndLoss = 0;
                        _newweek.BaseTransfer = _journal.BaseAmount;
                        _newweek.SGDTransfer = _journal.SGDAmount;
                        _newweek.BaseTransaction = 0;
                        _newweek.SGDTransaction = 0;
                        _newweek.BaseBalance = _journal.BaseAmount;
                        _newweek.SGDBalance = _journal.SGDAmount;
                        _newweek.Status = WeeklySummaryStatus.None;
                        _newweek.BasePrevTransaction = 0;
                        _newweek.SGDPrevTransaction = 0;
                        _newweek.ConfirmUser.UserID = userID;
                        _weekAccessClient.Insert1(_newweek);
                    }
                }
                using (RecordAccessClient _recordclient = new RecordAccessClient(EndpointName.RecordAccess))
                {
                    _recordclient.ChangeStatus(record.RecordID, RecordStatus.Confirm);
                }
            }
        }

        public void TransferPL(Period period)
        {
            using (RecordAccessClient _recordAccessClient = new RecordAccessClient(EndpointName.RecordAccess))
            {
                _recordAccessClient.QueryByperiod(period);               
            }
        }

        public bool CloseEntry(int userid)
        {
            //First: get this period SummaryCollection
            int _periodid = 0;
            Property _pro = new Property();
            Period _current;
            using (PeriodAccessClient _periodAccessClient = new PeriodAccessClient(EndpointName.PeriodAccess))
            {
               _current = PeriodService.Instance.GetCurrentPeriod()[0];
                _periodid = _current.ID;
                _pro.PropertyValue = _current.PeriodNo;
                _pro.PropertyName = "ClosedPeriod";
            }
            PropertiesService.Instance.SetProperty(_pro.PropertyName, _pro);


            //Check MLJ This Period Is Approved
            using (MLJRecordAccessClient _mljclient = new MLJRecordAccessClient(EndpointName.MLJRecordAccess))
            {
                if (!_mljclient.IsApprove(_periodid))
                    return false;
            }

            using (WeeklySummaryAccessClient _weekAccessClient = new WeeklySummaryAccessClient(EndpointName.WeeklySummaryAccess))
            {
                WeeklySummaryCollection _weekCollection = new WeeklySummaryCollection(_weekAccessClient.QuerybyPeriod(_periodid));
                if (!_periodid.Equals(0))
                {
                    foreach (WeeklySummary _week in _weekCollection)
                    {
                        if (_week.Status.Equals(WeeklySummaryStatus.None))
                            return false;
                        //Second: Add Next Period Summary
                        WeeklySummary _newweek = new WeeklySummary();
                        _newweek.Period.ID = _current.ID;
                        _newweek.Entity = _week.Entity;
                        _newweek.BaseCurrency = _week.BaseCurrency;
                        _newweek.ExchangeRate = _week.ExchangeRate;
                        _newweek.BasePrevBalance = _week.BaseBalance;
                        _newweek.SGDPrevBalance = _week.SGDBalance;
                        //_newweek.BaseWinAndLoss = 0;
                        //_newweek.SGDWinAndLoss = 0;
                        //_newweek.BaseTransfer = 0;
                        //_newweek.SGDTransfer = 0;
                        //_newweek.BaseTransaction = 0;
                        //_newweek.SGDTransaction = 0;
                        //_newweek.BaseBalance = 0;
                        //_newweek.SGDBalance = 0;
                        //_newweek.Status = WeeklySummaryStatus.None;
                        _newweek.BasePrevTransaction = _week.BaseTransaction;
                        _newweek.SGDPrevTransaction = _week.SGDTransaction;
                        _weekAccessClient.Insert1(_newweek);
                    }
                }
            }

            //Check and add MLJ
            using (MLJRecordAccessClient _mljclient = new MLJRecordAccessClient(EndpointName.MLJRecordAccess))
            {
                _mljclient.CheckAndAdd(_current.ID, userid);                   
            }


            

            return true;

        }

        public void ReverseClosing()
        {
            //First: get this periodid

            string _preperiod = "";
            Period _current = PeriodService.Instance.GetClosedPeriod()[0];
            using (PeriodAccessClient _periodAccessClient = new PeriodAccessClient(EndpointName.PeriodAccess))
            {
                _preperiod = new PeriodCollection(_periodAccessClient.Query4(_current.PeriodNo, -1))[0].PeriodNo;
            }
            Property _pro = new Property();
            _pro.PropertyValue = _preperiod;
            _pro.PropertyName = "ClosedPeriod";
            PropertiesService.Instance.SetProperty("ClosedPeriod", _pro);
        }
    }
}
