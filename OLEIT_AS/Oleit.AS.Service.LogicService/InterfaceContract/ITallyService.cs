using Oleit.AS.Service.DataObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Oleit.AS.Service.LogicService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITallyService" in both code and config file together.
    [ServiceContract]
    public interface ITallyService
    {
        [OperationContract]
        void DoWork();

        [OperationContract]
        Tuple<EntityCollection, WeeklySummaryCollection> LoadEntity(int entityID);

        [OperationContract]
        WeeklySummaryCollection Confirm(int confirmUserID, int entityID);

        [OperationContract]
        WeeklySummaryCollection ExcelConfirm(int entityID, WeeklySummaryCollection wsc);
    }
}
