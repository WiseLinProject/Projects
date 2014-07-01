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
    public interface IEntityAccess
    {
        [OperationContract(Name = "Insert1")]
        [WebGet(UriTemplate = "Insert/Entity/{entity}")]
        int Insert(Entity entity);

        [OperationContract(Name = "Insert2")]
        [WebGet(UriTemplate = "Insert/Entity/{entity}/CashEntity/{cashEntity}")]
        void Insert(Entity entity, CashEntity cashEntity);

        [OperationContract(Name = "Insert3")]
        [WebGet(UriTemplate = "Insert/Entity/{entity}/Account/{account}")]
        void Insert(Entity entity, Account account);

        [OperationContract(Name = "Insert4")]
        [WebGet(UriTemplate = "Insert/Account/{account}")]
        void Insert(Account account);

        [OperationContract]
        EntityCollection QueryRelationEntities();

        [OperationContract]
        Entity QueryMainEntity(int entityID);

        [OperationContract(Name = "Query1")]
        [WebGet(UriTemplate = "Query")]
        EntityCollection Query();

        [OperationContract(Name = "Query2")]
        [WebGet(UriTemplate = "Query/int/{entityID}")]
        EntityCollection Query(int entityID);

        [OperationContract(Name = "Query3")]
        [WebGet(UriTemplate = "Query/string/{entityName}")]
        EntityCollection Query(string entityName);

        [OperationContract(Name = "Query4")]
        [WebGet(UriTemplate = "Query/EntityType/{_type}")]
        EntityCollection Query(EntityType _type);

        [OperationContract(Name = "QueryCashEntity1")]
        [WebGet(UriTemplate = "QueryCashEntity")]
        CashEntityCollection QueryCashEntity();

        [OperationContract(Name = "QueryCashEntity2")]
        [WebGet(UriTemplate = "QueryCashEntity/int/{entityID}")]
        CashEntityCollection QueryCashEntity(int entityID);

        [OperationContract(Name = "QueryCashEntity3")]
        [WebGet(UriTemplate = "QueryCashEntity/string/{contractNumber}")]
        CashEntityCollection QueryCashEntity(string contractNumber);

        [OperationContract(Name = "QueryAccount1")]
        [WebGet(UriTemplate = "QueryAccount")]
        AccountCollection QueryAccount();

        [OperationContract(Name = "QueryAccount2")]
        [WebGet(UriTemplate = "QueryAccount/int/{entityID}")]
        AccountCollection QueryAccount(int entityID);

        [OperationContract(Name = "QueryAccount3")]
        [WebGet(UriTemplate = "QueryAccount/string/{accountName}")]
        AccountCollection QueryAccount(string accountName);

        [OperationContract(Name = "QuerySumEntity1")]
        [WebGet(UriTemplate = "QuerySumTypeEntity/SumType/{sumType}")]
        EntityCollection QuerySumTypeEntity(SumType sumType);

        [OperationContract(Name = "QuerySumEntity2")]
        [WebGet(UriTemplate = "QuerySumTypeEntity/int/{entityID}/SumType/{sumType}")]
        EntityCollection QuerySumTypeEntity(int entityID, SumType sumType);

        [OperationContract]
        EntityCollection QuerySubEntitiesList(int entityID);

        [OperationContract]
        EntityCollection QueryParentSubTotalEntity(int entityID);

        [OperationContract]
        EntityCollection QueryParentTransactionEntity(int entityID);

        [OperationContract(Name = "QueryAllSub1")]
        [WebGet(UriTemplate = "QueryAllSub/int/{entityID}")]
        EntityCollection QueryAllSub(int entityID);

        [OperationContract(Name = "QueryAllSub2")]
        [WebGet(UriTemplate = "QueryAllSub/string/{entityName}")]
        EntityCollection QueryAllSub(string entityName);

        [OperationContract(Name = "Update1")]
        [WebGet(UriTemplate = "Update/Entity/{entity}")]
        void Update(Entity entity);

        [OperationContract(Name = "Update2")]
        [WebGet(UriTemplate = "Update/Entity/{entity}/CashEntity/{cashEntity}")]
        void Update(Entity entity, CashEntity cashEntity);

        [OperationContract(Name = "Update3")]
        [WebGet(UriTemplate = "Update/int/{userID}/Entity/{entity}/Account/{account}")]
        void Update(int userID, Entity entity, Account account);

        [OperationContract]
        void Disable(int entityID);

        [OperationContract]
        void Enable(int entityID);

        [OperationContract]
        void RemoveRelation(int entityID, int targetEntityID);

        [OperationContract]
        void SetRate(int entityID, decimal exchangeRate,Currency currency);

        [OperationContract]
        void ChangeStatus(User user, int entityID, Status status);

        [OperationContract]
        void SetAttributes(User user, int entityID, Account account);

        [OperationContract]
        void Allocate(string accountname, int entityID);

        [OperationContract]
        void SetRelateEntity(Relation relation);

        [OperationContract]
        RelationCollection GetRelateEntity(int entityID);

        [OperationContract]
        RelationCollection GetTallyRelationEntity(int entityID);

        [OperationContract]
        RelationCollection GetTargetRelation(int targetEntityID);

        [OperationContract]
        EntityCollection QueryRelation(int _entityID);

        [OperationContract]
        EntityCollection QueryAllMLJ(int entityID);

        [OperationContract]
        RelationCollection GetRelateEntityWandL(int entityID);

        [OperationContract]
        EntityCollection QueryAllSubEntity(int entityID);

        [OperationContract]
        EntityCollection QueryMainCash();

        [OperationContract]
        EntityCollection QueryTransactionList(int entityID);

    }
}
