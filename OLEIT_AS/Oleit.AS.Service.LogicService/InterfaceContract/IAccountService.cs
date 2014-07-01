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
    public interface IAccountService
    {
        [OperationContract]       
        void Allocate(string accountName, int entityID);

        [OperationContract]       
        void SetAttributes(User user, int entityID, Account account);

        [OperationContract]       
        void UpdateStatus(User user, int entityID, Status status);
    }
}