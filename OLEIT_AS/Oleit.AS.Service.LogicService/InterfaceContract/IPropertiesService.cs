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
    public interface IPropertiesService
    {
        [OperationContract (Name="Insert")]
        [WebGet(UriTemplate = "Insert/string/{propertyName}/string/{propertyValue}")]
        void NewProperty(Property property);

        [OperationContract(Name = "InsertCollection")]
        [WebGet(UriTemplate = "Insert/PropertyCollection/{propertyCollection}")]
        void NewProperty(PropertyCollection propertyCollection);

        [OperationContract(Name = "SetPropertyValue1")]
        [WebGet(UriTemplate = "Update/int/{propertyID}/string/{propertyValue}")]
        void SetPropertyValue(int propertyID, string propertyValue);

        [OperationContract]
        void SetProperty(string propertyKey,Property perty);
        
        [OperationContract(Name = "SetPropertyValue2")]
        [WebGet(UriTemplate = "Update/string/{propertyName}/string/{propertyValue}")]
        void SetPropertyValue(string propertyName, string propertyValue);

        [OperationContract(Name = "GetPropertyValue1")]
        [WebGet(UriTemplate = "Query/int/{propertyID}")]
        PropertyCollection GetPropertyValue(int propertyID);

        [OperationContract(Name = "GetPropertyValue2")]
        [WebGet(UriTemplate = "Query/string/{propertyName}")]
        PropertyCollection GetPropertyValue(string propertyName);

        [OperationContract]
        PropertyCollection GetProperty(string propertyName);

        [OperationContract]
        PropertyCollection GetAllProperties();
    }
}