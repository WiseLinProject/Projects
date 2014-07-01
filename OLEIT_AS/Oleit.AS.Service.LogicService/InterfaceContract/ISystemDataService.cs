using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Oleit.AS.Service.LogicService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISystemDataService" in both code and config file together.
    [ServiceContract]
    public interface ISystemDataService
    {
        [OperationContract]
        System.Data.DataSet GetUserMLJEntity();

        [OperationContract]
        System.Data.DataSet LoadWinAndLossLog(int entityID);

        [OperationContract]
        System.Data.DataSet QueryRoleMenuRelation();

        [OperationContract]
        System.Data.DataSet QueryRoleUserRelation();

        [OperationContract]
        System.Data.DataSet GetAccountStatusLog(int entityid);

        [OperationContract]
        System.Data.DataSet GetMLJLog(int periodId, string entityName);
    }
}
