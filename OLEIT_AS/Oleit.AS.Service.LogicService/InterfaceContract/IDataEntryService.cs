using Oleit.AS.Service.DataObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace Oleit.AS.Service.LogicService
{
    [ServiceContract]
    public interface IDataEntryService
    {
        [OperationContract]
        int InsertRecord(Record record, JournalCollection jcollection);

        [OperationContract]
        int SaveRecord(Record record, JournalCollection jcollection);

        [OperationContract]
        int InsertTransfer(Record record, Transfer transfer);

        [OperationContract]
        void UpdateRecord(User user, Record record, JournalCollection jcollection);

        [OperationContract]
        Record LoadRecord(int recordID);

        [OperationContract]
        RecordCollection LoadRecordByPeriodEntityID(int entityid, int periodid);

        [OperationContract]
        RecordCollection GetRecordList();

        [OperationContract]
        void Approve(int recordID);

        [OperationContract]
        void Avoid(int recordID);

        [OperationContract]
        TransactionCollection LoadTransaction();

        [OperationContract]
        TransactionCollection LoadTransactionByID(int _id);

        [OperationContract]
        TransactionCollection LoadTransactionByPeriodID(int _periodid);

        [OperationContract]
        void InsertTransaction(Transaction _transaction);

        [OperationContract]
        void SetNotices(int _id, int _userid);

        [OperationContract]
        void SetConfirm(Transaction transaction);

        [OperationContract]
        void Updatetransaction(Transaction transaction);

        [OperationContract]
        List<decimal> GetjournalSum(int periodId, int typeid, int entityid);

       
    }
}