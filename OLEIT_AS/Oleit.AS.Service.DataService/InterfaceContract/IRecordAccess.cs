using Oleit.AS.Service.DataObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data;

namespace Oleit.AS.Service.DataService
{
    [ServiceContract]
    public interface IRecordAccess
    {
        [OperationContract(Name = "Insert")]
        [WebGet(UriTemplate = "Insert/Record/{record}/JournalCollection/{jcollection}")]
        int Insert(Record record, JournalCollection jcollection);

        [OperationContract(Name = "InsertRecordCollection")]
        [WebGet(UriTemplate = "Insert/RecordCollection/{recordCollection}")]
        void Insert(RecordCollection recordCollection);

        [OperationContract]
        RecordCollection QueryAll();

        [OperationContract(Name = "QueryByrecordID")]
        [WebGet(UriTemplate = "Query/int/{recordID}")]
        RecordCollection Query(int recordID);

        [OperationContract(Name = "QueryBytype")]
        [WebGet(UriTemplate = "Query/RecordType/{type}")]
        RecordCollection Query(RecordType type);

        [OperationContract(Name = "QueryByperiod")]
        [WebGet(UriTemplate = "Query/Period/{period}")]
        RecordCollection Query(Period period);
       
        [OperationContract(Name = "QueryByRecordStatus")]
        [WebGet(UriTemplate = "Query/Period/{period}")]
        RecordCollection Query(RecordStatus status);

        [OperationContract(Name = "QueryByapproveUser")]
        [WebGet(UriTemplate = "Query/User/{approveUser}")]
        RecordCollection Query(User approveUser);

        [OperationContract(Name = "QueryJournal")]
        [WebGet(UriTemplate = "Query/int/{recordID}/string/{equenceNo}")]
        JournalCollection QueryJournal(int recordID, int sequenceNo);

        [OperationContract(Name = "QueryJournal2")]
        [WebGet(UriTemplate = "Query/int/{recordID}/string/{baseCurrency}")]
        JournalCollection QueryJournal(int recordID, string baseCurrency);

        [OperationContract(Name = "QueryRecordByPeriodEntityID")]
        [WebGet(UriTemplate = "Query/int/{entityid}/int/{periodid}")]
        RecordCollection Query(int entityid, int periodid);

        [OperationContract(Name = "Update")]
        [WebGet(UriTemplate = "Update/User/{user}/Record/{record}/JournalCollection/{jcollection}")]
        void Update(User user,Record record,JournalCollection jcollection);

        [OperationContract]
        DataSet LoadWinAndLossLog(int peroidID, int entityID);

        [OperationContract]
        void InsertDeletionLog(JournalCollection jcollection);

        [OperationContract(Name = "UpdateJournal")]
        [WebGet(UriTemplate = "Update/int/{recordID}/Journal/{journal}")]
        void UpdateJournal(Journal journal);

        [OperationContract(Name = "UpdateJournalCollection")]
        [WebGet(UriTemplate = "Update/int/{recordID}/JournalCollection/{journalCollection}")]
        void UpdateJournal(JournalCollection journalCollection);

        [OperationContract(Name = "ChangeStatus")]
        [WebGet(UriTemplate = "Update/int/{recordID}/RecordStatus/{status}")]
        void ChangeStatus(int recordID, RecordStatus status);

        [OperationContract]
        int QueryRecordID(int entityid, int periodid);

        [OperationContract]
        List<decimal> GetjournalSum(int periodId, int typeid, int entityid);
    }
}
