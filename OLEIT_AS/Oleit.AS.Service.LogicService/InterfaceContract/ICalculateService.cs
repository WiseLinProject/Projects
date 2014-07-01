using Oleit.AS.Service.DataObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web;

namespace Oleit.AS.Service.LogicService
{
    [ServiceContract]
    public interface ICalculateService
    {
        [OperationContract]
        JournalCollection AutoJournal(JournalCollection pAndLEntities);

        [OperationContract]
        WeeklySummaryCollection GetWeeklySummary(int entityID); //TODO:

        [OperationContract]
        Dictionary<int, bool> HasWeeklySummary(Entity entity); //TODO:

        [OperationContract]
        Transfer Subtotal(int entryUserID, Entity entity);

        [OperationContract]
        Tuple<Transfer, Entity> Transfer(int entryUserID, EntityCollection entityCollection, decimal[] baseTransfer);

        [OperationContract]
        Tuple<Transfer, Entity> ExcelTransfer(EntityCollection entityCollection, decimal[] baseTransfer, WeeklySummaryCollection wsc);
    }
}