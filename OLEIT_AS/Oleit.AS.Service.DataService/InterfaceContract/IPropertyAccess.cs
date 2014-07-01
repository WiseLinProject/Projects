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
    public interface IPropertyAccess
    {
        [OperationContract(Name = "Insert1")]
        [WebGet(UriTemplate = "Insert/Property/{property}")]
        void Insert(Property property);

        [OperationContract(Name = "Insert2")]
        [WebGet(UriTemplate = "Insert/PropertyCollection/{propertyCollection}")]
        void Insert(PropertyCollection propertyCollection);

        [OperationContract(Name = "Query1")]
        [WebGet(UriTemplate = "Query/int/{propertyID}")]
        PropertyCollection Query(int propertyID);

        [OperationContract(Name = "Query2")]
        [WebGet(UriTemplate = "Query/string/{propertyName}")]
        PropertyCollection Query(string propertyName);

        [OperationContract]
        PropertyCollection QueryAll();

        [OperationContract(Name = "Update1")]
        [WebGet(UriTemplate = "Update/Property/{property}")]
        void Update(string propertyKey,Property property);

        [OperationContract(Name = "Update2")]
        [WebGet(UriTemplate = "Update/PropertyCollection/{propertyCollection}")]
        void Update(PropertyCollection propertyCollection);
    }
}
