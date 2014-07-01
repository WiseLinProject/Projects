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
    public interface IEntityService
    {
        [OperationContract(Name = "NewEntity1")]
        [WebGet(UriTemplate = "NewEntity/Entity/{entity}")]
        void NewEntity(Entity entity);

        [OperationContract(Name = "NewEntity2")]
        [WebGet(UriTemplate = "NewEntity/Entity/{entity}/CashEntity/{cashEntity}")]
        void NewEntity(Entity entity, CashEntity cashEntity);

        [OperationContract(Name = "NewEntity3")]
        [WebGet(UriTemplate = "NewEntity/Entity/{entity}/Account/{account}")]
        void NewEntity(Entity entity, Account account);

        [OperationContract(Name = "LoadEntity1")]
        [WebGet(UriTemplate = "LoadEntity")]
        EntityCollection LoadEntity();

        [OperationContract(Name = "LoadAll")]       
        EntityCollection LoadAll();

        [OperationContract(Name = "LoadEntity2")]
        [WebGet(UriTemplate = "LoadEntity/int/{entityID}")]
        EntityCollection LoadEntity(int entityID);

        [OperationContract(Name = "LoadEntity3")]
        [WebGet(UriTemplate = "LoadEntity/string/{entityName}")]
        EntityCollection LoadEntity(string entityName);

        [OperationContract(Name = "LoadEntity4")]
        [WebGet(UriTemplate = "LoadEntity/ints/{entityIDs}")]
        EntityCollection LoadEntity(int[] entityIDs);

        [OperationContract]
        EntityCollection LoadAccountEntities(int parentID);

        [OperationContract]
        EntityCollection LoadLastLevelEntities(int parentID);

        [OperationContract]
        EntityCollection QueryRelationEntities();

        [OperationContract]
        EntityCollection LoadSingleEntityList(int entityID);

        [OperationContract]
        EntityCollection QuerySubEntitiesList(int entityID);

        [OperationContract]
        string QuerySingleBranchEntity(int entityID);

        [OperationContract(Name = "LoadCashEntity1")]
        [WebGet(UriTemplate = "LoadCashEntity")]
        CashEntityCollection LoadCashEntity();

        [OperationContract]
        Entity QueryMainEntity(int entityID);

        [OperationContract(Name = "LoadCashEntity2")]
        [WebGet(UriTemplate = "LoadCashEntity/int/{entityID}")]
        CashEntityCollection LoadCashEntity(int entityID);

        [OperationContract(Name = "LoadCashEntity3")]
        [WebGet(UriTemplate = "LoadCashEntity/string/{contractNumber}")]
        CashEntityCollection LoadCashEntity(string contractNumber);

        [OperationContract(Name = "LoadAccount1")]
        [WebGet(UriTemplate = "LoadAccount")]
        AccountCollection LoadAccount();

        [OperationContract(Name = "LoadAccount2")]
        [WebGet(UriTemplate = "LoadAccount/int/{entityID}")]
        AccountCollection LoadAccount(int entityID);

        [OperationContract(Name = "LoadAccount3")]
        [WebGet(UriTemplate = "LoadAccount/string/{accountName}")]
        AccountCollection LoadAccount(string accountName);

        [OperationContract(Name = "LoadSubtotalEntity1")]
        [WebGet(UriTemplate = "NewEntity")]
        EntityCollection LoadSubtotalEntity();

        [OperationContract]
        EntityCollection QueryParentSubTotalEntity(int entityID);

        [OperationContract(Name = "LoadTransactionEntity1")]
        [WebGet(UriTemplate = "Load")]
        EntityCollection LoadTransactionEntity();

        [OperationContract(Name = "LoadTransactionEntity2")]
        [WebGet(UriTemplate = "NewEntity/int/{entityID}")]
        EntityCollection LoadTransactionEntity(int entityID);


        [OperationContract(Name = "SaveEntity1")]
        [WebGet(UriTemplate = "SaveEntity/Entity/{entity}")]
        void SaveEntity(Entity entity);

        [OperationContract(Name = "SaveEntity2")]
        [WebGet(UriTemplate = "SaveEntity/Entity/{entity}/CashEntity/{cashEntity}")]
        void SaveEntity(Entity entity, CashEntity cashEntity);

        [OperationContract(Name = "SaveEntity3")]
        [WebGet(UriTemplate = "SaveEntity/Entity/{entity}/Account/{account}")]
        void SaveEntity(int userID, Entity entity, Account account);

        [OperationContract]
        void ChangeEntityType(int entityID, EntityType type);

        //[OperationContract]
        //void NewRelation(int entityID, int targetEntityID, Relation relation);

        [OperationContract]
        void NewRelation(Relation relation);

        [OperationContract]
        void SetRelation(int entityID, Relation relation);

        [OperationContract]
        void SetCurrencyAndRate(int entityID, string currencyID, decimal rate);

        [OperationContract]
        void ChangeRate(int entityID, decimal rate);

        [OperationContract]
        void Disable(int entityID);

        [OperationContract]
        void Enable(int entityID);

        [OperationContract]
        void RemoveRelation(int entityID, int targetEntityID);

        [OperationContract]
        EntityCollection GetEntryEntities(string userName);

        [OperationContract]
        RelationCollection GetRelateEntity(int entityID);

        [OperationContract]
        void ChangeStatus(User user, int entityID, Status status);

        [OperationContract]
        RelationCollection RelationEntity(int _entityID);

        [OperationContract]
        RelationCollection RelationEntityWandL(int _entityID);

        [OperationContract]
        EntityCollection QueryAllMLJ(int entityID);

        [OperationContract]
        EntityCollection LoadCashMainAndTransaction();

        [OperationContract]
        EntityCollection CheckTransactionTransfer(int entityID);

    }
}