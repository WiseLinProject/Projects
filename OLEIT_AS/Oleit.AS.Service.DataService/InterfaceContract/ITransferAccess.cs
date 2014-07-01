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
    public interface ITransferAccess
    {
        [OperationContract(Name = "Insert")]
        [WebGet(UriTemplate = "Insert/Record/{record}/Transfer/{transfer}")]
        int Insert(Record record, Transfer transfer);

        [OperationContract(Name = "InsertDetail")]
        [WebGet(UriTemplate = "Insert/TransferDetail/{transferdetail}")]
        void Insert(TransferDetail transferdetail);

        [OperationContract(Name = "Update")]
        [WebGet(UriTemplate = "Update/Transfer/{transfer}")]
        void Update(Transfer transfer);

        [OperationContract(Name = "UpdateDetail")]
        [WebGet(UriTemplate = "Update/TransferDetail/{transferdetail}")]
        void Update(TransferDetail transferdetail);

        [OperationContract(Name = "UpdateDetailCollection")]
        [WebGet(UriTemplate = "Update/TransferDetailCollection/{transferdetailcollection}")]
        void Update(TransferDetailCollection transferdetailcollection);

        [OperationContract(Name = "Query")]
        [WebGet(UriTemplate = "Query/int/{recordID}")]
        TransferCollection Query(int recordID);

        [OperationContract(Name = "QueryDetailCollection")]
        [WebGet(UriTemplate = "Query/int/{recordID}")]
        TransferDetailCollection QueryDetailCollection(int recordID);

        [OperationContract(Name = "QueryCollectionByToEntityID")]
        [WebGet(UriTemplate = "Query/int/{EntityID}")]
        TransferCollection QueryByToEntityID(int EntityID);

        [OperationContract(Name = "QueryDetailCollectionByToEntityID")]
        [WebGet(UriTemplate = "Query/int/{EntityID}")]
        TransferDetailCollection QueryDetailCollectionToEntityID(int EntityID);

        [OperationContract]
        TransferDetailCollection QueryByEntityID(int EntityID);

    }
}
