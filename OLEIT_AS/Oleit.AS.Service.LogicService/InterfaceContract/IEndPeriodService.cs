using System;
using System.Collections.Generic;
using Oleit.AS.Service.DataObject;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Oleit.AS.Service.LogicService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IEndPeriodService" in both code and config file together.
    [ServiceContract]
    public interface IEndPeriodService
    {
        [OperationContract]
        EndPeriodCollection GetEndPeriodRate(Currency Currency);

        [OperationContract]
        bool InsertEndPeriod(EndPeriod endPeriod);
    }
}
