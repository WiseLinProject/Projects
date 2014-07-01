using Oleit.AS.Service.DataObject;
using Oleit.AS.Service.LogicService.EntityAccessReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;


namespace Oleit.AS.Service.LogicService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AccountService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select AccountService.svc or AccountService.svc.cs at the Solution Explorer and start debugging.
    public class AccountService : IAccountService
    {
        public void DoWork()
        {
        }

        public void SetAttributes(User user, int entityID, Account account)
        {
            using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
            {
                _entityAccessClient.SetAttributes(user, entityID, account);
            }
        }

        public void Allocate(string accountName, int entityID)
        {
            using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
            {
                _entityAccessClient.Allocate(accountName, entityID);
            }
        }

        public void UpdateStatus(User user, int entityID, Status status)
        {
            using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
            {
                _entityAccessClient.ChangeStatus(user, entityID, status);
            }
        }
    }
}
