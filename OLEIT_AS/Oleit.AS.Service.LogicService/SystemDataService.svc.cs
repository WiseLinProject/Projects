using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Oleit.AS.Service.LogicService.MLJRecordAccessReference;
using Oleit.AS.Service.LogicService.MenuAccessReference;
using Oleit.AS.Service.LogicService.RecordAccessReference;
using Oleit.AS.Service.LogicService.PeriodAccessReference;

namespace Oleit.AS.Service.LogicService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SystemDataService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select SystemDataService.svc or SystemDataService.svc.cs at the Solution Explorer and start debugging.
    public class SystemDataService : ISystemDataService
    {
        public System.Data.DataSet GetUserMLJEntity()
        {
            using (MLJRecordAccessClient _MLJAccessClient = new MLJRecordAccessClient(EndpointName.MLJRecordAccess))
            {
                return _MLJAccessClient.GetUserMLJEntity();
            }
        }

        public System.Data.DataSet LoadWinAndLossLog(int entityID)
        {
            int _currentPeroid = PeriodService.Instance.GetCurrentPeriod()[0].ID;
            using (RecordAccessClient _recordAccessClient = new RecordAccessClient(EndpointName.RecordAccess))
            {
                return _recordAccessClient.LoadWinAndLossLog(_currentPeroid, entityID);
            }
        }

        public System.Data.DataSet QueryRoleMenuRelation()
        {
            using (MenuAccessClient _menuAccessClient = new MenuAccessClient(EndpointName.MenuAccess))
            {
                return _menuAccessClient.QueryRoleMenuRelation();
            }
        }

        public System.Data.DataSet QueryRoleUserRelation()
        {
            using (MenuAccessClient _menuAccessClient = new MenuAccessClient(EndpointName.MenuAccess))
            {
                return _menuAccessClient.QueryRoleUserRelation();
            }
        }

        public System.Data.DataSet GetAccountStatusLog(int entityid)
        {
            using (MLJRecordAccessClient _MLJAccessClient = new MLJRecordAccessClient(EndpointName.MLJRecordAccess))
            {
                return _MLJAccessClient.GetAccountStatusLog(entityid);
            }
        }

        public System.Data.DataSet GetMLJLog(int periodId, string entityName)
        {
            using (MLJRecordAccessClient _MLJAccessClient = new MLJRecordAccessClient(EndpointName.MLJRecordAccess))
            {
                return _MLJAccessClient.GetMLJLog(periodId, entityName);
            }
        }

    }
}
