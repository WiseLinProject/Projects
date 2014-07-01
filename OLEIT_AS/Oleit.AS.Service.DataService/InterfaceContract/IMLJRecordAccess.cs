using Oleit.AS.Service.DataObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Oleit.AS.Service.DataService
{
    [ServiceContract]
    public interface IMLJRecordAccess
    {
        [OperationContract]
        int Insert(MLJRecord record);

        [OperationContract (Name="QueryByRecordID")]
        [WebGet(UriTemplate = "Query/int/{MLJRecordID}")]
        MLJRecordCollection Query(int MLJRecordID);

        [OperationContract (Name = "QueryByPeriodIDEntityName")]
        [WebGet(UriTemplate = "Query/int/{PeriodID}/string/{EntityName}")]
        MLJJournalCollection Query(int PeriodID, string EntityName);

        [OperationContract (Name = "QueryJournalByRecordID")]
        [WebGet(UriTemplate = "Query/int/{MLJRecordID}")]
        MLJJournalCollection QueryJournal(int MLJRecordID);

        [OperationContract (Name = "UpdateRecord")]
        [WebGet(UriTemplate = "Update/MLJRecord/{record}")]
        void Update(MLJRecord record);
        
        [OperationContract (Name = "UpdateJournal")]
        [WebGet(UriTemplate = "Update/MLJJournal/{journal}")]
        void UpdateJournal(MLJJournal journal);

        [OperationContract(Name = "UpdateJournalCollection")]
        [WebGet(UriTemplate = "Update/MLJJournalCollection/{journalCollection}")]
        void UpdateJournal(MLJJournalCollection journalCollection);

        [OperationContract]
        void ChangeStatus(int MLJRecordID, RecordStatus status,int userID);

        [OperationContract]
        void CheckAndAdd(int periodid,int userid);

        [OperationContract]
        StatusColorCollection QueryStatusColor();

        [OperationContract]
        void UpdateColor(StatusColorCollection collection);

        [OperationContract]
        StatusColorCollection QueryOneStatusColor(int statusid);

        [OperationContract]
        void InsertUserRMLJ(int userid, int entityid);

        [OperationContract]
        System.Data.DataSet GetUserMLJEntity();
        
        [OperationContract]
        void UpdateUserMLJ(int userid, EntityCollection collection);

        [OperationContract]
        bool IsApprove(int PeriodID);

        [OperationContract]
        System.Data.DataSet GetAccountStatusLog(int entityid);

        [OperationContract]
        System.Data.DataSet GetMLJLog(int periodId, string entityName);

        [OperationContract]
        decimal GetMLJSum(int periodId, int entityid);

    }
}
