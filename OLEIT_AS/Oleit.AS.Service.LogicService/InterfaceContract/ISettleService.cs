using Oleit.AS.Service.DataObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace Oleit.AS.Service.LogicService
{
    [ServiceContract]
    public interface ISettleService
    {
        [OperationContract]
        void WeeklySummarize(Entity entity);
       
        [OperationContract]
        void WinLossConfirm(WeeklySummaryCollection weeklySummaryCollection, int recordID);

        [OperationContract]
        bool CloseEntry(int userid);

        [OperationContract]
        void ReverseClosing();

        [OperationContract]
        void TransferPL(Period period);

        [OperationContract]
        void TransferConfirm(Record roecord,int userID);

        [OperationContract]
        void TransactionConfirm(WeeklySummaryCollection weeklySummaryCollection);

        
    }
}