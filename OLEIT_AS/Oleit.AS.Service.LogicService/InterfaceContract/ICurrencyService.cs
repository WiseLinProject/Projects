using Oleit.AS.Service.DataObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace Oleit.AS.Service.LogicService
{
    [ServiceContract]
    public interface ICurrencyService
    {
        [OperationContract]
        void NewCurrency(Currency currency);

        [OperationContract]
        void DelCurrency(Currency currency);

        [OperationContract]
        CurrencyCollection AllCurrency();
    }
}