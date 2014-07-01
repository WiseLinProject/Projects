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
    public interface ICurrencyAccess
    {
        [OperationContract(Name = "Insert1")]
        [WebGet(UriTemplate = "Insert/Currency/{currency}")]
        void Insert(Currency currency);

        [OperationContract(Name = "Insert2")]
        [WebGet(UriTemplate = "Insert/CurrencyCollection/{currencyCollection}")]
        void Insert(CurrencyCollection currencyCollection);

        [OperationContract]
        CurrencyCollection Query(string currencyID);

        [OperationContract]
        CurrencyCollection QueryAll();

        [OperationContract]
        void Delete(Currency currency);
    }
}
