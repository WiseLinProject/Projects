using Oleit.AS.Service.DataObject;
using Oleit.AS.Service.LogicService.RecordAccessReference;
using Oleit.AS.Service.LogicService.TransferAccessReference;
using Oleit.AS.Service.LogicService.TransactionAccessReference;
using Oleit.AS.Service.LogicService.EntityAccessReference;
using Oleit.AS.Service.LogicService.WeeklySummaryReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;


namespace Oleit.AS.Service.LogicService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DataEntryService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select DataEntryService.svc or DataEntryService.svc.cs at the Solution Explorer and start debugging.
    public class DataEntryService : IDataEntryService
    {
        public static volatile DataEntryService Instance = new DataEntryService();

        public void DoWork()
        {
        }

        public void UpdateRecord(User user,Record record,JournalCollection jcollection)
        {
            using (RecordAccessClient _recordAccessClient = new RecordAccessClient(EndpointName.RecordAccess))
            {
                _recordAccessClient.Update(user, record, jcollection.ToArray());
            }
        }

        public int SaveRecord(Record record, JournalCollection jcollection)
        {
            //log record
            var _logRecord = LoadRecordByPeriodEntityID(record.EntityID, record.Period.ID);
            if (_logRecord.Any())
            {
                JournalCollection _delJournalCollection= new JournalCollection();
                JournalCollection _updJournalCollection = new JournalCollection();
                var _newJournal = jcollection;
                foreach (var _journal in _logRecord[0].JournalCollection)
                {
                    if (_newJournal.Any(x => _journal.EntityID == x.EntityID && _journal.BaseAmount == x.BaseAmount))
                        continue;
                    else if (_newJournal.Any(x => _journal.EntityID == x.EntityID))
                        _updJournalCollection.Add(_newJournal.First(x => x.EntityID == _journal.EntityID));
                    else
                        _delJournalCollection.Add(_journal);
                }

                #region "Update function"
                if (_updJournalCollection.Any())
                {
                    using (RecordAccessClient _recordAccessClient = new RecordAccessClient(EndpointName.RecordAccess))
                    {
                        try
                        {
                            _recordAccessClient.UpdateJournalCollection(_updJournalCollection.ToArray());
                        }
                        catch(Exception ex)
                        {
                            return -1;
                        }
                    }
                }
                #endregion

                #region "Delete function"
                if (_delJournalCollection.Any())
                {
                    using (RecordAccessClient _recordAccessClient = new RecordAccessClient(EndpointName.RecordAccess))
                    {
                        try
                        {
                            _recordAccessClient.InsertDeletionLog(_delJournalCollection.ToArray());
                        }
                        catch (Exception ex)
                        {
                            return -1;
                        }
                    }
                }
                #endregion
                
                return record.RecordID;
            }
            else
            {
                using (RecordAccessClient _recordAccessClient = new RecordAccessClient(EndpointName.RecordAccess))
                {
                    return _recordAccessClient.Insert(record, jcollection.ToArray());
                }
            }
        }

        public int InsertRecord(Record record, JournalCollection jcollection)
        {
            using (RecordAccessClient _recordAccessClient = new RecordAccessClient(EndpointName.RecordAccess))
            {
                return _recordAccessClient.Insert(record, jcollection.ToArray());
            }
        }

        public int InsertTransfer(Record record,  Transfer transfer)
        {
            using (TransferAccessClient _transferAccessClient = new TransferAccessClient(EndpointName.TransferAccess))
            {
                return _transferAccessClient.Insert(record, transfer);
            }
        }

        public Record LoadRecord(int recordID)
        {
            using (RecordAccessClient _recordAccessClient = new RecordAccessClient(EndpointName.RecordAccess))
            {
                return _recordAccessClient.QueryByrecordID(recordID)[0];
            }
        }
        public RecordCollection LoadRecordByPeriodEntityID(int entityid, int periodid)
        {
            using (RecordAccessClient _recordAccessClient = new RecordAccessClient(EndpointName.RecordAccess))
            {
                return new RecordCollection(_recordAccessClient.QueryRecordByPeriodEntityID(entityid,periodid));
            }
        }

        public RecordCollection GetRecordList()
        {
            using (RecordAccessClient _recordAccessClient = new RecordAccessClient(EndpointName.RecordAccess))
            {
                return new RecordCollection(_recordAccessClient.QueryAll());
            }
        }

        public void Approve(int recordID)
        {
            using (RecordAccessClient _recordAccessClient = new RecordAccessClient(EndpointName.RecordAccess))
            {
                _recordAccessClient.ChangeStatus(recordID, RecordStatus.Confirm);
            }
        }

        public void Avoid(int recordID)
        {
            using (RecordAccessClient _recordAccessClient = new RecordAccessClient(EndpointName.RecordAccess))
            {
                _recordAccessClient.ChangeStatus(recordID, RecordStatus.Avoid);
            }
        }

        public TransactionCollection LoadTransaction()
        {
            using (TransactionAccessClient _tran = new TransactionAccessClient(EndpointName.TransactionAccess))
            {
                return new TransactionCollection(_tran.QueryAll());
            }
        }

        public TransactionCollection LoadTransactionByID(int _id)
        {
            using (TransactionAccessClient _tran = new TransactionAccessClient(EndpointName.TransactionAccess))
            {
                return new TransactionCollection(_tran.QueryByID(_id));
            }
        }

        public TransactionCollection LoadTransactionByPeriodID(int _periodid)
        {
            using (TransactionAccessClient _tran = new TransactionAccessClient(EndpointName.TransactionAccess))
            {
                return new TransactionCollection(_tran.QueryByPeriodid(_periodid));
            }
        }

        public void InsertTransaction(Transaction _transaction)
        {
            using (TransactionAccessClient _tran = new TransactionAccessClient(EndpointName.TransactionAccess))
            {
                _tran.InsertTransaction(_transaction);
            }
        }

        public void InsertTransactionCollection(TransactionCollection _collection)
        {
            using (TransactionAccessClient _tran = new TransactionAccessClient(EndpointName.TransactionAccess))
            {
                _tran.InsertTransactionCollection(_collection.ToArray());
            }
        }

        public void SetNotices(int _id, int _userid)
        {
            using (TransactionAccessClient _tran = new TransactionAccessClient(EndpointName.TransactionAccess))
            {
                _tran.SetNotices(_id, _userid);
            }
        }

        public void SetConfirm(Transaction transaction)
        {
            using (TransactionAccessClient _tran = new TransactionAccessClient(EndpointName.TransactionAccess))
            {
                SettleService _settle = new SettleService();
                WeeklySummaryCollection _weekco = new WeeklySummaryCollection();
                //From Entity
                WeeklySummary _weekFrom = new WeeklySummary();
                _weekFrom.ExchangeRate = transaction.ExchangeRate;
                _weekFrom.Period = transaction.Period;                
                _weekFrom.Entity = transaction.FromEntity;
                _weekFrom.BaseTransaction = -(transaction.Amount);
                _weekFrom.BaseCurrency = transaction.FromCurrency;
                //To Entity
                WeeklySummary _weekTo = new WeeklySummary();
                _weekTo.ExchangeRate = transaction.ExchangeRate;
                _weekTo.Period = transaction.Period;               
                _weekTo.Entity = transaction.ToEntity;
                _weekTo.BaseTransaction = transaction.To_Amount;
                _weekTo.BaseCurrency = transaction.ToCurrency;
                _weekco.Add(_weekFrom);
                _weekco.Add(_weekTo);

                _settle.TransactionConfirm(_weekco);
                
                _tran.SetConfirm(transaction.ID, transaction.Updater.UserID, transaction.Period.ID);
            }
        }

        public void Updatetransaction(Transaction transaction)
        {
            using (TransactionAccessClient _tran = new TransactionAccessClient(EndpointName.TransactionAccess))
            {
                _tran.Update(transaction);
            }
        }

        public List<decimal> GetjournalSum(int periodId, int typeid, int entityid)
        {
            using (RecordAccessClient _Client = new RecordAccessClient(EndpointName.RecordAccess))
            {                
                if (typeid.Equals(2))
                {
                   // Entity _entity = 
                    EntityAccessClient _enClient = new EntityAccessClient(EndpointName.EntityAccess);
                    EntityCollection _accountcollection = new EntityCollection(_enClient.QueryAllSubEntity(entityid));
                    List<decimal> _allandsub = new List<decimal>();
                    decimal _base = 0;
                    decimal _sgd = 0;
                    _accountcollection.Add(_enClient.Query2(entityid)[0]);
                    foreach (Entity _entity in _accountcollection)
                    {
                        if (_Client.GetjournalSum(periodId, typeid, _entity.EntityID).ToList().Count > 0)
                        {
                            _base += _Client.GetjournalSum(periodId, typeid, _entity.EntityID).ToList()[0];
                            _sgd += _Client.GetjournalSum(periodId, typeid, _entity.EntityID).ToList()[1];
                        }
                    }
                    _allandsub.Add(_base);
                    _allandsub.Add(_sgd);
                    return _allandsub.ToList();
                }
                else
                {
                    return _Client.GetjournalSum(periodId, typeid, entityid).ToList();
                }
            }
        }

    }
}
