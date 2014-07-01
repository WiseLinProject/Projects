using Oleit.AS.Service.DataObject;
using Oleit.AS.Service.LogicService.PeriodAccessReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Oleit.AS.Service.LogicService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PeriodService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select PeriodService.svc or PeriodService.svc.cs at the Solution Explorer and start debugging.
    public class PeriodService : IPeriodService
    {
        public static volatile PeriodService Instance = new PeriodService();
      
        public void DoWork()
        {
        }

        public PeriodCollection SetPeriod(int year)
        {
            if (year >= 9999)
            {
                return GetPeriods();
            }

            Period _lastPeriod = GetPeriods().OrderByDescending(Period => Period.StartDate).First();

            string[] mSplits = _lastPeriod.PeriodNo.Split('.');

            int _year = int.Parse(mSplits[0]);
            int _month = int.Parse(mSplits[1]);
            int _week = int.Parse(mSplits[2]);

            if (_year >= 9999)
            {
                return GetPeriods();
            }

            PeriodCollection _periodCollection = new PeriodCollection();

            while (_year <= year) //while (_year < 9999)
            {
                Period _period = new Period()
                {
                    StartDate = _lastPeriod.StartDate.AddDays(7),
                    EndDate = _lastPeriod.EndDate.AddDays(7),
                };

                _week++;

                if (_week >= 5)
                {
                    //if ((_month == 13) && (_period.StartDate.Year == _lastPeriod.StartDate.Year))
                    //{ }
                    //else
                    //{
                        _week = 1;
                        _month++;

                        if (_month >= 14)
                        {
                            _month = 1;
                            _year++;
                        }
                    //}
                }

                _period.PeriodNo = string.Format("{0}.{1}.{2}", _year, _month, _week);

                if (_year <= year)
                {
                    _periodCollection.Add(_lastPeriod = _period);
                }
            }

            using (PeriodAccessClient _periodAccessClient = new PeriodAccessClient(EndpointName.PeriodAccess))
            {
                _periodAccessClient.Set2(_periodCollection.ToArray());
            }

            return GetPeriods();
        }

        public PeriodCollection GetPeriods()
        {
            using (PeriodAccessClient _periodAccessClient = new PeriodAccessClient(EndpointName.PeriodAccess))
            {
                return new PeriodCollection(_periodAccessClient.QueryAll());
            }
        }

        public PeriodCollection GetNextorLast(string PeriodNo,int flag)
        {
            using (PeriodAccessClient _periodAccessClient = new PeriodAccessClient(EndpointName.PeriodAccess))
            {
                return new PeriodCollection(_periodAccessClient.Query4(PeriodNo,flag));
            }
        }

        public PeriodCollection PeriodByDate(DateTime dateTime)
        {
            PeriodCollection _periodCollection = null;

            using (PeriodAccessClient _periodAccessClient = new PeriodAccessClient(EndpointName.PeriodAccess))
            {
                _periodCollection = new PeriodCollection(_periodAccessClient.QueryAll());
            }

            int _count = _periodCollection.Count;

            for (int i = (_count - 1); i >= 0; i--)
            {
                if (!((_periodCollection[i].StartDate >= dateTime) && (_periodCollection[i].EndDate <= dateTime)))
                {
                    _periodCollection.RemoveAt(i);
                }
            }

            return _periodCollection;
        }

        public PeriodCollection DateOfPeriod(string periodNo)
        {
            using (PeriodAccessClient _periodAccessClient = new PeriodAccessClient(EndpointName.PeriodAccess))
            {
                return new PeriodCollection(_periodAccessClient.Query1(periodNo));
            }
        }

        public PeriodCollection GetClosedPeriod()
        {
            using (PeriodAccessClient _periodAccessClient = new PeriodAccessClient(EndpointName.PeriodAccess))
            {
                return new PeriodCollection(_periodAccessClient.Query1(PropertiesService.Instance.GetProperty(SpecialProperty.ClosedPeriod)[0].PropertyValue));
            }
        }

        public PeriodCollection GetCurrentPeriod()
        {
            using (PeriodAccessClient _periodAccessClient = new PeriodAccessClient(EndpointName.PeriodAccess))
            {
                return new PeriodCollection(GetNextorLast(GetClosedPeriod()[0].PeriodNo, 1));
            }
        }

    }
}
