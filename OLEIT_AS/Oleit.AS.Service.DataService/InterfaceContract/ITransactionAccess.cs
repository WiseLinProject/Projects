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
    public interface ITransactionAccess
    {
        [OperationContract]
        TransactionCollection QueryAll();

        [OperationContract]
        TransactionCollection QueryByPeriodid(int _periodid);

        [OperationContract]
        TransactionCollection QueryByID(int _id);

        [OperationContract]
        void InsertTransaction(Transaction _transaction);

        [OperationContract]
        void InsertTransactionCollection(TransactionCollection _collection);

        [OperationContract]
        void SetNotices(int _id, int _userid);

        [OperationContract]
        void SetConfirm(int _id, int _userid, int _periodid);

        [OperationContract]
        void Update(Transaction transaction);


    }
}
