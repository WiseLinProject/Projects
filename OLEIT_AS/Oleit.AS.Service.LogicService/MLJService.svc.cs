using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Oleit.AS.Service.DataObject;
using Oleit.AS.Service.LogicService.MLJRecordAccessReference;
using Oleit.AS.Service.LogicService.RecordAccessReference;

namespace Oleit.AS.Service.LogicService
{

    public class MLJService : IMLJService
    {
        public void CheckAndAdd(int PeriodID, int userID)
        {
            using (MLJRecordAccessClient _MLJAccessClient = new MLJRecordAccessClient(EndpointName.MLJRecordAccess))
            {
                _MLJAccessClient.CheckAndAdd(PeriodID, userID);
            }
        }

        public MLJJournalCollection Query(int PeriodID, string EntityName)
        {
            using (MLJRecordAccessClient _MLJAccessClient = new MLJRecordAccessClient(EndpointName.MLJRecordAccess))
            {
                return new MLJJournalCollection(_MLJAccessClient.QueryByPeriodIDEntityName(PeriodID, EntityName));
            }
        }

        public MLJRecord QueryRecordByID(int RecordID)
        {
            using (MLJRecordAccessClient _MLJAccessClient = new MLJRecordAccessClient(EndpointName.MLJRecordAccess))
            {
                return new MLJRecordCollection(_MLJAccessClient.QueryByRecordID(RecordID))[0];
            }
        }

        public StatusColorCollection QueryStatusColor()
        {
            using (MLJRecordAccessClient _MLJAccessClient = new MLJRecordAccessClient(EndpointName.MLJRecordAccess))
            {
                return new StatusColorCollection(_MLJAccessClient.QueryStatusColor());
            }
        }

        public void UpdateJournal(MLJJournal journal)
        {
            using (MLJRecordAccessClient _MLJAccessClient = new MLJRecordAccessClient(EndpointName.MLJRecordAccess))
            {
                _MLJAccessClient.UpdateJournal(journal);
            }
        }

        public void Approve(int MLJRecordID, int userID)
        {
            using (MLJRecordAccessClient _MLJAccessClient = new MLJRecordAccessClient(EndpointName.MLJRecordAccess))
            {
                //First get all JournalCollection in this period
                MLJRecord _MLJ = new MLJRecordCollection(_MLJAccessClient.QueryByRecordID(MLJRecordID))[0];
                //New Record
                Record _re = new Record();
                _re.Period = _MLJ.Period;
                _re.RecordStatus = RecordStatus.Normal;
                _re.Type = RecordType.WinAndLoss;
                foreach (MLJJournal _MLJjur in _MLJ.MLJJournalCollection)
                {
                    Journal _jur = new Journal();
                    decimal _baseamount = _MLJjur.Mon + _MLJjur.Tue + _MLJjur.Wed + _MLJjur.Thu + _MLJjur.Fri + _MLJjur.Sat + _MLJjur.Sun;
                    _jur.BaseAmount = _baseamount;
                    _jur.BaseCurrency = _MLJjur.BaseCurrency;
                    _jur.ExchangeRate = _MLJjur.ExchangeRate;
                    _jur.SGDAmount = _baseamount * _MLJjur.ExchangeRate;
                    _jur.EntityID = _MLJjur.EntityID;
                    _jur.EntryUser.UserID = userID;
                    _re.JournalCollection.Add(_jur);
                }
                //Second insert Record
                using (RecordAccessClient _RecordClient = new RecordAccessClient(EndpointName.RecordAccess))
                {
                    _RecordClient.Insert(_re, _re.JournalCollection.ToArray());
                }
                //Third Change MLJ_Record Status 
                _MLJAccessClient.ChangeStatus(MLJRecordID, RecordStatus.Confirm, userID);
            }
        }

        public void UpdateColor(StatusColorCollection collection)
        {
            using (MLJRecordAccessClient _MLJAccessClient = new MLJRecordAccessClient(EndpointName.MLJRecordAccess))
            {
                _MLJAccessClient.UpdateColor(collection.ToArray());
            }
        }

        public void UpdateUserMLJ(int userid, EntityCollection collection)
        {
            using (MLJRecordAccessClient _MLJAccessClient = new MLJRecordAccessClient(EndpointName.MLJRecordAccess))
            {
                _MLJAccessClient.UpdateUserMLJ(userid,collection.ToArray());
            }
        }

        public decimal GetMLJSum(int periodId, int entityid)
        {
            using (MLJRecordAccessClient _MLJAccessClient = new MLJRecordAccessClient(EndpointName.MLJRecordAccess))
            {
                return _MLJAccessClient.GetMLJSum(periodId, entityid);
            }
        }
    }
}
