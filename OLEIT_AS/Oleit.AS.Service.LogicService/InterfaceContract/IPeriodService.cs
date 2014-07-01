using Oleit.AS.Service.DataObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace Oleit.AS.Service.LogicService
{
    [ServiceContract]
    public interface IPeriodService
    {
        [OperationContract]
        PeriodCollection SetPeriod(int year);

        [OperationContract]
        PeriodCollection GetPeriods();

        [OperationContract]
        PeriodCollection PeriodByDate(DateTime dateTime);

        [OperationContract]
        PeriodCollection DateOfPeriod(string periodNo);

        [OperationContract]
        PeriodCollection GetNextorLast(string PeriodNo, int flag);

        [OperationContract]
        PeriodCollection GetClosedPeriod();

        [OperationContract]
        PeriodCollection GetCurrentPeriod();

    }
}