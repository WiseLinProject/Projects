using Oleit.AS.Service.DataObject;
using Oleit.AS.Service.LogicService.CurrencyAccessReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Oleit.AS.Service.LogicService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CurrencyService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select CurrencyService.svc or CurrencyService.svc.cs at the Solution Explorer and start debugging.
    public class CurrencyService : ICurrencyService
    {
        public void DoWork()
        {
        }

        public void NewCurrency(Currency currency)
        {
            try
            {
                using (CurrencyAccessClient _currencyAccessClient = new CurrencyAccessClient(EndpointName.CurrencyAccess))
                {
                    _currencyAccessClient.Insert1(currency);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CurrencyCollection AllCurrency()
        {
            try
            {
                using (CurrencyAccessClient _currencyAccessClient = new CurrencyAccessClient(EndpointName.CurrencyAccess))
                {
                    return new CurrencyCollection(_currencyAccessClient.QueryAll());
                }
            }
            catch (Exception)
            {
                
                throw;
            }
           
        }

        public void DelCurrency(Currency currency)
        {
            try
            {
                using (CurrencyAccessClient _currencyAccessClient = new CurrencyAccessClient(EndpointName.CurrencyAccess))
                {
                    _currencyAccessClient.Delete(currency);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
