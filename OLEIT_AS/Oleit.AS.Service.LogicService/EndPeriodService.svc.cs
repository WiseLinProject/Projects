using System;
using Oleit.AS.Service.DataObject;
using Oleit.AS.Service.LogicService.EndPeriodAccessReference;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Oleit.AS.Service.LogicService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "EndPeriodService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select EndPeriodService.svc or EndPeriodService.svc.cs at the Solution Explorer and start debugging.
    public class EndPeriodService : IEndPeriodService
    {
        public static volatile EndPeriodService Instance = new EndPeriodService();

        public void DoWork()
        {
        }

        public EndPeriodCollection GetEndPeriodRate(Currency Currency)
        {
            int _periodID =  PeriodService.Instance.GetCurrentPeriod()[0].ID;
            EndPeriod _endPeriod = new EndPeriod
            {
                Period_ID = _periodID,
                Currency = new Currency { CurrencyID = Currency.CurrencyID }
            };
            using (EndPeriodAccessClient _endPeriodAccessClient = new EndPeriodAccessClient(EndpointName.EndPeriodAccess))
            {
                return new EndPeriodCollection(_endPeriodAccessClient.Query(_endPeriod));
            }
        }

        public bool InsertEndPeriod(EndPeriod endPeriod)
        {
            var _endPeriodCollection = GetEndPeriodRate(endPeriod.Currency);
            if (_endPeriodCollection.Any())
                return false;
            else
            {
                try
                {
                    using (EndPeriodAccessClient _endPeriodAccessClient = new EndPeriodAccessClient(EndpointName.EndPeriodAccess))
                    {
                        _endPeriodAccessClient.Insert(endPeriod);
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }

    }
}
