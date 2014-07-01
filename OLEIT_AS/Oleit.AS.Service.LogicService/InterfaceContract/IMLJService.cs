using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Oleit.AS.Service.DataObject;

namespace Oleit.AS.Service.LogicService
{
   
    [ServiceContract]
    public interface IMLJService
    {
        [OperationContract]
        void CheckAndAdd(int PeriodID, int userID);

        [OperationContract]
        MLJJournalCollection Query(int PeriodID, string EntityName);

        [OperationContract]
        void UpdateJournal(MLJJournal journal);

        [OperationContract]
        void Approve(int MLJRecordID, int userID);

        [OperationContract]
        MLJRecord QueryRecordByID(int RecordID);

        [OperationContract]
        StatusColorCollection QueryStatusColor();

        [OperationContract]
        void UpdateColor(StatusColorCollection collection);

        [OperationContract]
        void UpdateUserMLJ(int userid, EntityCollection collection);

        [OperationContract]
        decimal GetMLJSum(int periodId, int entityid);

    }
}
