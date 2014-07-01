using Oleit.AS.Service.DataObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Oleit.AS.Service.DataService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IWeeklySummaryAccess" in both code and config file together.
    [ServiceContract]
    public interface IWeeklySummaryAccess
    {
        [OperationContract(Name = "Insert1")]
        [WebGet(UriTemplate = "Insert/WeeklySummary/{weeklySummary}")]
        void Insert(WeeklySummary weeklySummary);

        [OperationContract(Name = "Insert2")]
        [WebGet(UriTemplate = "Insert/WeeklySummaryCollection/{weeklySummaryCollection}")]
        void Insert(WeeklySummaryCollection weeklySummaryCollection);

        [OperationContract(Name = "Query1")]
        [WebGet(UriTemplate = "Query/int/{entityID}")]
        WeeklySummaryCollection Query(int entityID);

        [OperationContract]
        [WebGet(UriTemplate = "Query/int/{periodID}/int/{entityID}")]
        WeeklySummaryCollection Query(int periodID, int entityID);

        [OperationContract]       
        WeeklySummaryCollection QuerybyPeriod(int periodID);

        [OperationContract]
        WeeklySummaryCollection QueryAll();

        [OperationContract(Name = "Query2")]
        [WebGet(UriTemplate = "Query/int/{periodID}/EntityCollection/{entitycoll}")]
        WeeklySummaryCollection Query(int periodID, int[] entity);

        [OperationContract(Name = "Update1")]
        [WebGet(UriTemplate = "Update/WeeklySummary/{weeklySummary}")]
        void Update(WeeklySummary weeklySummary);

        [OperationContract(Name = "Update2")]
        [WebGet(UriTemplate = "Update/WeeklySummaryCollection/{weeklySummaryCollection}")]
        void Update(WeeklySummaryCollection weeklySummaryCollection);

        [OperationContract]
        void SubTotalConfirm(WeeklySummaryCollection weeklySummaryCollection);

        [OperationContract]
        void WinLossConfirm(WeeklySummaryCollection weeklySummaryCollection);

        [OperationContract]
        bool IsWeeklyConfirm(int periodID, int entityID);
    }
}
