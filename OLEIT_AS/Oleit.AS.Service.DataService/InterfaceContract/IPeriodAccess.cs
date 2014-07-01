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
    public interface IPeriodAccess
    {
        [OperationContract(Name = "Set1")]
        [WebGet(UriTemplate = "Set/Period/{period}")]
        void Set(Period period);

        [OperationContract(Name = "Set2")]
        [WebGet(UriTemplate = "Set/PeriodCollection/{periodCollection}")]
        void Set(PeriodCollection periodCollection);

        [OperationContract(Name = "Query1")]
        [WebGet(UriTemplate = "Query/string/{periodNo}")]
        PeriodCollection Query(string periodNo);

        [OperationContract(Name = "Query2")]
        [WebGet(UriTemplate = "Query/DateTime/{startDate}")]
        PeriodCollection Query(DateTime startDate);

        [OperationContract(Name = "Query3")]
        [WebGet(UriTemplate = "Query/int/{id}")]
        PeriodCollection Query(int id);

        [OperationContract]
        PeriodCollection QueryAll();

        [OperationContract(Name = "Query4")]
        [WebGet(UriTemplate = "Query/string/{Period}/int/{flag}")]
        PeriodCollection Query(string Period, int flag);
    }
}
