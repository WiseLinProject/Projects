using Oleit.AS.Service.DataObject;
using Oleit.AS.Service.LogicService.EntityAccessReference;
using Oleit.AS.Service.LogicService.PeriodAccessReference;
using Oleit.AS.Service.LogicService.WeeklySummaryReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Diagnostics;


namespace Oleit.AS.Service.LogicService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "EntityService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select EntityService.svc or EntityService.svc.cs at the Solution Explorer and start debugging.
    public class EntityService : IEntityService
    {
        public static volatile EntityService Instance = new EntityService();

        public void DoWork()
        {
        }

        public void NewEntity(Entity entity)
        {
            using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
            {
                _entityAccessClient.Insert1(entity);
            }
        }

        public void NewEntity(Entity entity, CashEntity cashEntity)
        {
            using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
            {
                _entityAccessClient.Insert2(entity, cashEntity);
            }
        }

        public void NewEntity(Entity entity, Account account)
        {
            using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
            {
                _entityAccessClient.Insert3(entity, account);
            }
        }

        private Entity FindParent(EntityCollection entityCollection, int parentID)
        {
            foreach (Entity _entity in entityCollection)
            {
                if (_entity.EntityID == parentID)
                {
                    return _entity;
                }
                else if (_entity.SubEntities.Count == 0)
                {
                    continue;
                }
                else
                {
                    Entity _parent = FindParent(_entity.SubEntities, parentID);

                    if (_parent != null)
                    {
                        return _parent;
                    }
                }
            }

            return null;
        }

        public EntityCollection LoadEntity()
        {
            EntityCollection _entityCollection = new EntityCollection();

            using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
            {
                _entityCollection = new EntityCollection(_entityAccessClient.Query1());
            }

            int _count = _entityCollection.Count - 1;

            for (int i = _count; i >= 0; i--)
            {
                int _parentID = _entityCollection[i].ParentID;

                if (_parentID == 0)
                {
                    continue;
                }

                Entity _parent = FindParent(_entityCollection, _parentID);
                IEnumerable<Entity> _children = _entityCollection.Where(Entity => (Entity.ParentID == _parentID));

                _parent.SubEntities.AddRange(_children);
                _entityCollection.RemoveAll(Entity => (Entity.ParentID == _parentID));

                i = _entityCollection.Count;
            }

            return _entityCollection;
        }

        public EntityCollection LoadAll()
        {
            using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
            {
                return new EntityCollection(_entityAccessClient.Query1());
            }
        }

        public EntityCollection LoadCashMainAndTransaction()
        {
            using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
            {
                EntityCollection _All = new EntityCollection();
                EntityCollection _Cash = new EntityCollection(_entityAccessClient.QueryMainCash());                
                foreach (Entity _entity in _Cash)
                {
                    _All.Add(_entity);
                    EntityCollection _Tran = new EntityCollection(_entityAccessClient.QueryTransactionList(_entity.EntityID));
                    foreach (Entity _entitytran in _Tran)
                    {
                        _All.Add(_entitytran);
                    }
                }
                return _All;
            }
        }

        public EntityCollection QueryAllMLJ(int entityID)
        {
            using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
            {
                return new EntityCollection(_entityAccessClient.QueryAllMLJ(entityID));
            }
        }

        public Entity FindEntity(int entityID, EntityCollection entityCollection)
        {
            foreach (Entity _entity in entityCollection)
            {
                if (_entity.EntityID == entityID)
                {
                    return _entity;
                }
                else if (_entity.SubEntities.Count == 0)
                {
                    continue;
                }
                else
                {
                    Entity _target = FindEntity(entityID, _entity.SubEntities);

                    if (_target != null)
                    {
                        return _target;
                    }
                }
            }

            return null;
        }

        public EntityCollection LoadEntity(int entityID)
        {
            //using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
            //{
            //    return new EntityCollection(_entityAccessClient.Query2(entityID));
            //}

            EntityCollection _entityTree = LoadEntity();

            EntityCollection _result = new EntityCollection();

            Entity _target = FindEntity(entityID, _entityTree);

            if (_target != null)
            {
                _result.Add(_target);
            }

            return _result;
        }

        public EntityCollection LoadEntity(int[] entityIDs)
        {
            EntityCollection _entityCollection = LoadEntity();

            EntityCollection _result = new EntityCollection();

            foreach (int _entityID in entityIDs)
            {
                Entity _target = FindEntity(_entityID, _entityCollection);

                if (_target != null)
                {
                    _result.Add(_target);
                }
            }

            return _result;
        }

        public EntityCollection LoadEntity(string entityName)
        {
            using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
            {
                return new EntityCollection(_entityAccessClient.Query3(entityName));
            }
        }

        public EntityCollection QueryRelationEntities()
        {
            using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
            {
                return new EntityCollection(_entityAccessClient.QueryRelationEntities());
            }
        }

        public EntityCollection LoadSingleEntityList(int entityID)
        {
            using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
            {
                EntityCollection _ecEntities = new EntityCollection(_entityAccessClient.QuerySubEntitiesList(entityID));
                _ecEntities.Add(_entityAccessClient.Query2(entityID)[0]);
                return _ecEntities;
            }
        }

        public EntityCollection QuerySubEntitiesList(int entityID)
        {
            using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
            {
                EntityCollection _ecEntities = new EntityCollection(_entityAccessClient.QuerySubEntitiesList(entityID));
                return _ecEntities;
            }
        }

        private void LoadAccountEntities(EntityCollection source, EntityCollection storage)
        {
            foreach (Entity _entity in source)
            {
                if (_entity.IsAccount == 1)
                {
                    storage.Add(_entity);
                }

                LoadAccountEntities(_entity.SubEntities, storage);
            }
        }

        public EntityCollection LoadAccountEntities(int parentID)
        {
            EntityCollection _entityParent = LoadEntity(parentID);
            EntityCollection _storage = new EntityCollection();

            if (_entityParent.Count == 0)
            {
                return _storage;
            }

            LoadAccountEntities(_entityParent, _storage);

            return _storage;
        }

        public Entity QueryMainEntity(int entityID)
        {
            using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
            {
                return _entityAccessClient.QueryMainEntity(entityID);
            }
        }

        private void LoadLastLevelEntities(EntityCollection source, EntityCollection storage)
        {
            foreach (Entity _entity in source)
            {
                if (_entity.IsLastLevel == 1)
                {
                    storage.Add(_entity);
                }

                LoadLastLevelEntities(_entity.SubEntities, storage);
            }
        }

        public EntityCollection LoadLastLevelEntities(int parentID)
        {
            EntityCollection _entityParent = LoadEntity(parentID);
            EntityCollection _storage = new EntityCollection();

            if (_entityParent.Count == 0)
            {
                return _storage;
            }

            LoadLastLevelEntities(_entityParent, _storage);

            return _storage;
        }

        public CashEntityCollection LoadCashEntity()
        {
            using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
            {
                return new CashEntityCollection(_entityAccessClient.QueryCashEntity1());
            }
        }

        public CashEntityCollection LoadCashEntity(int entityID)
        {
            using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
            {
                return new CashEntityCollection(_entityAccessClient.QueryCashEntity2(entityID));
            }
        }

        public CashEntityCollection LoadCashEntity(string contractNumber)
        {
            using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
            {
                return new CashEntityCollection(_entityAccessClient.QueryCashEntity3(contractNumber));
            }
        }

        public AccountCollection LoadAccount()
        {
            using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
            {
                return new AccountCollection(_entityAccessClient.QueryAccount1());
            }
        }

        public AccountCollection LoadAccount(int entityID)
        {
            using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
            {
                return new AccountCollection(_entityAccessClient.QueryAccount2(entityID));
            }
        }

        public AccountCollection LoadAccount(string accountName)
        {
            using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
            {
                return new AccountCollection(_entityAccessClient.QueryAccount3(accountName));
            }
        }

        public EntityCollection LoadSubtotalEntity()
        {
            using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
            {
                return new EntityCollection(_entityAccessClient.QuerySumEntity1(SumType.Subtotal));
            }
        }

        public EntityCollection QueryParentSubTotalEntity(int entityID)
        {
            using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
            {
                return new EntityCollection(_entityAccessClient.QueryParentSubTotalEntity(entityID));
            }
        }

        public EntityCollection LoadTransactionEntity()
        {
            using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
            {
                return new EntityCollection(_entityAccessClient.QuerySumEntity1(SumType.Transaction));
            }
        }

        public EntityCollection LoadTransactionEntity(int entityID)
        {
            using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
            {
                return new EntityCollection(_entityAccessClient.QuerySumEntity2(entityID, SumType.Transaction));
            }
        }


        public void SaveEntity(Entity entity)
        {
            using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
            {
                _entityAccessClient.Update1(entity);
            }
        }

        public void SaveEntity(Entity entity, CashEntity cashEntity)
        {
            using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
            {
                _entityAccessClient.Update2(entity, cashEntity);
            }
        }

        public void SaveEntity(int userID, Entity entity, Account account)
        {
            AccountCollection _accountCollection = LoadAccount(entity.EntityID);

            if (_accountCollection.Count == 0)
            {
                using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
                {
                    account.EntityID = entity.EntityID;

                    _entityAccessClient.Insert4(account);
                    SaveEntity(entity);
                }
            }
            else//
            {
                using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
                {
                    account.ID = _entityAccessClient.QueryAccount2(account.EntityID)[0].ID;
                    _entityAccessClient.Update3(userID, entity, account);
                }
            }
        }

        private void ChangeEntityType(EntityCollection entityCollection, EntityType type)
        {
            if (type != EntityType.BadDebt)
            {
                throw new NotSupportedException("if (type != EntityType.BadDebt)");
            }

            foreach (Entity _entity in entityCollection)
            {
                ChangeEntityType(_entity.SubEntities, type);

                _entity.EntityType = type;

                SaveEntity(_entity);
            }
        }

        public void ChangeEntityType(int entityID, EntityType type)
        {
            if (type != EntityType.BadDebt)
            {
                throw new NotSupportedException("if (type != EntityType.BadDebt)");
            }

            Entity _entity = LoadEntity(entityID)[0];

            _entity.EntityType = type;

            SaveEntity(_entity);

            ChangeEntityType(_entity.SubEntities, type);

            //if (_entity.ParentID == 0)
            //{
            //    foreach (Entity _sub in _entity.SubEntities)
            //    {
            //        if (_sub.SumType != SumType.Transaction)
            //        {
            //            continue;
            //        }

            //        _sub.EntityType = type;

            //        SaveEntity(_sub);
            //    }
            //}
            //else if (_entity.SumType == SumType.Transaction)
            //{
            //    _entity.EntityType = type;

            //    SaveEntity(_entity);
            //}

            //return;
        }

        //public void NewRelation(int entityID, int targetEntityID, Relation relation)
        //{
        //    Entity _entity = null;
        //    Entity _parentEntity = null;
        //    Entity _targetEntity = null;
        //    string _entityName = "";

        //    using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
        //    {
        //        _entity = new EntityCollection(_entityAccessClient.Query2(entityID))[0];
        //        _parentEntity = new EntityCollection(_entityAccessClient.Query2(_entity.ParentID))[0];
        //        _targetEntity = new EntityCollection(_entityAccessClient.Query2(targetEntityID))[0];
        //    }
        //    decimal _exchangeRate = _targetEntity.ExchangeRate;
        //    string _currencyID = _targetEntity.Currency.CurrencyID;
        //    if ((relation.Description != RelationDescription.Allocate) && (relation.Description != RelationDescription.Position))
        //    {
        //        relation.TargetEntity = _targetEntity;
        //        relation.Entity = _entity;

        //        using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
        //        {
        //            _entityAccessClient.SetRelateEntity(relation);
        //        }

        //        return;
        //    }

        //    Account _oriAccount = LoadAccount(entityID)[0];

        //    Entity _node = new Entity()
        //    {
        //        ParentID = _targetEntity.EntityID,
        //        Enable = 1,
        //        EntityName = _parentEntity.EntityName,
        //        EntityType = _targetEntity.EntityType,
        //        //Currency = new Currency() { CurrencyID = _targetEntity.Currency.CurrencyID },
        //        ExchangeRate = _targetEntity.ExchangeRate,
        //    };

        //    using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
        //    {
        //        _node.EntityID = _entityAccessClient.Insert1(_node);
        //    }

        //    if (_node.EntityID <= 0)
        //    {
        //        throw new MethodAccessException("if (_node.EntityID <= 0)");
        //    }

        //    if (relation.Description == RelationDescription.Allocate)
        //    {
        //        _entityName = string.Format("{0}", _targetEntity.EntityName);
        //        _exchangeRate = 1;
        //        _currencyID="";
        //    }
        //    else if (relation.Description == RelationDescription.Position)
        //        _entityName = string.Format("Position from {0}", _targetEntity.EntityName);
        //    else if (relation.Description == RelationDescription.Position)
        //        _entityName = string.Format("{0} Comm", _targetEntity.EntityName);
        //    else
        //    {
        //        _entityName = string.Format("{0}", _targetEntity.EntityName);
        //    }

        //    Entity _accountEntity = new Entity()
        //    {
        //        ParentID = _node.EntityID,
        //        IsLastLevel = 1,
        //        //IsAccount = 1,
        //        Enable = 1,
        //        EntityName = _entityName,
        //        EntityType = _targetEntity.EntityType,
        //        Currency = new Currency { CurrencyID = _currencyID },
        //        ExchangeRate = _targetEntity.ExchangeRate,
        //    };

        //    using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
        //    {
        //        _accountEntity.EntityID = _entityAccessClient.Insert1(_accountEntity);
        //    }

        //    if (_accountEntity.EntityID <= 0)
        //    {
        //        throw new MethodAccessException("if (_accountEntity.EntityID <= 0)");
        //    }

        //    //Account _newAccount = new Account()
        //    //{
        //    //    EntityID = _accountEntity.EntityID,
        //    //    Company = _oriAccount.Company,
        //    //    AccountName = _oriAccount.AccountName,
        //    //    Password = _oriAccount.Password,
        //    //    AccountType = _oriAccount.AccountType,
        //    //    BettingLimit = _oriAccount.BettingLimit,
        //    //    Status = _oriAccount.Status,
        //    //};

        //    //using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
        //    //{
        //    //    _entityAccessClient.Insert4(_newAccount);
        //    //}

        //    relation.TargetEntity = _accountEntity;
        //    relation.Entity = _entity;

        //    using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
        //    {
        //        _entityAccessClient.SetRelateEntity(relation);
        //    }
        //}

        public Entity FindParent(int parentID, SumType sumType)
        {
            Entity _entity = null;

            using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
            {
                _entity = _entityAccessClient.Query2(parentID)[0];
            }

            if (_entity.SumType == sumType)
            {
                return _entity;
            }

            return FindParent(_entity.ParentID, sumType);
        }

        public Entity FindChild(EntityCollection entityCollection, string childName)
        {
            foreach (Entity _entity in entityCollection)
            {
                if (_entity.EntityName.Equals(childName, StringComparison.OrdinalIgnoreCase))
                {
                    return _entity;
                }

                Entity _child = FindChild(_entity.SubEntities, childName);

                if (_child != null)
                {
                    return _child;
                }
            }

            return null;
        }

        public Entity FindLastLevel(EntityCollection entityCollection, string childName)
        {
            foreach (Entity _entity in entityCollection)
            {
                if (_entity.EntityName.Equals(childName, StringComparison.OrdinalIgnoreCase) && (_entity.IsLastLevel == 1) && (_entity.IsAccount == 0))
                {
                    return _entity;
                }

                Entity _child = FindChild(_entity.SubEntities, childName);

                if (_child != null)
                {
                    return _child;
                }
            }

            return null;
        }

        public EntityCollection FindChildren(EntityCollection entityCollection, SumType sumType)
        {
            EntityCollection _result = new EntityCollection();

            foreach (Entity _entity in entityCollection)
            {
                if (_entity.SumType == sumType)
                {
                    _result.Add(_entity);

                    continue;
                }

                _result.AddRange(FindChildren(_entity.SubEntities, sumType));
            }

            return _result;
        }

        //1. AccountEntityID, TransactionEntityID, relation
        //2. SubtotalEntityName -> false to new subtotal entity. (cmx1 comm)
        //3. AccountEntityName -> true to return.
        //4. false to new Account and Entity. (cmx1 comm) * 2
        //5. SetRelateEntity(relation);
        public void NewRelation(Relation relation)
        {
            const string _posionName = "Position from ";
            const string _commisionName = " comm";

            switch (relation.Description)
            {
                case RelationDescription.Allocate:
                    break;

                case RelationDescription.Position:
                    break;

                case RelationDescription.Commission:
                    break;

                case RelationDescription.PAndLSum:
                    break;

                default:
                    throw new NotSupportedException(string.Format("switch ({0})", relation.Description));
            }

            if (relation.Description != RelationDescription.PAndLSum)
            {
                Entity _transactionEntity = (relation.TargetEntity = LoadEntity(relation.TargetEntity.EntityID)[0]);
                EntityCollection _transactionSubtotal = FindChildren(_transactionEntity.SubEntities, SumType.Subtotal);
                EntityCollection _transactionSuper = FindChildren(_transactionSubtotal, SumType.Super);
                EntityCollection _transactionMaster = FindChildren(_transactionSuper, SumType.Master);
                EntityCollection _transactionAgent = FindChildren(_transactionMaster, SumType.Agent);
                Entity _relationEntity = (relation.Entity = LoadEntity(relation.Entity.EntityID)[0]);
                Entity _relationParent = FindParent(_relationEntity.ParentID, SumType.Subtotal);

                Entity _subtotalChild = null;
                Entity _lastLevel = null;

                switch (relation.Description)
                {
                    case RelationDescription.Allocate:
                        {
                            //checkExistEntityAllocate(_transactionEntity, _relationEntity, relation.Description);

                            foreach (Entity _subEntity in _transactionSubtotal)
                            {
                                _subtotalChild = FindChild(_subEntity.SubEntities, _relationParent.EntityName);

                                if (_subtotalChild != null)
                                {
                                    break;
                                }
                            }

                            if (_subtotalChild != null)
                            {
                                break;
                            }
                            
                            _subtotalChild = new Entity()
                            {
                                ParentID = _transactionEntity.EntityID,
                                Enable = 1,
                                EntityName = _relationParent.EntityName,
                                EntityType = _transactionEntity.EntityType,
                                Currency = new Currency() { CurrencyID = _relationParent.Currency.CurrencyID },
                                ExchangeRate = _relationParent.ExchangeRate,
                                SumType = SumType.Subtotal,
                            };
                        }
                        break;

                    case RelationDescription.Position:
                        {
                            foreach (Entity _subtotal in _transactionSubtotal)
                            {
                                _subtotalChild = FindChild(_subtotal.SubEntities, string.Format("{0}{1}", _posionName, _relationParent.EntityName));

                                if (_subtotalChild != null)
                                {
                                    break;
                                }
                            }

                            if (_subtotalChild != null)
                            {
                                break;
                            }

                            _subtotalChild = new Entity()
                            {
                                ParentID = _transactionEntity.EntityID,
                                Enable = 1,
                                EntityName = string.Format("{0}{1}", _posionName, _relationParent.EntityName),
                                EntityType = _transactionEntity.EntityType,
                                Currency = new Currency() { CurrencyID = _relationParent.Currency.CurrencyID },
                                ExchangeRate = _relationParent.ExchangeRate,
                                SumType = SumType.Subtotal,
                            };
                        }
                        break;

                    case RelationDescription.Commission:
                        {
                            foreach (Entity _subtotal in _transactionSubtotal)
                            {
                                _subtotalChild = FindChild(_subtotal.SubEntities, string.Format("{0}{1}", _relationParent.EntityName, _commisionName));

                                if (_subtotalChild != null)
                                {
                                    break;
                                }
                            }

                            if (_subtotalChild != null)
                            {
                                break;
                            }

                            _subtotalChild = new Entity()
                            {
                                ParentID = _transactionEntity.EntityID,
                                Enable = 1,
                                EntityName = string.Format("{0}{1}", _relationParent.EntityName, _commisionName),
                                EntityType = _transactionEntity.EntityType,
                                Currency = new Currency() { CurrencyID = _relationParent.Currency.CurrencyID },
                                ExchangeRate = _relationParent.ExchangeRate,
                                SumType = SumType.Subtotal,
                            };
                        }
                        break;
                }

                if (_subtotalChild.EntityID == 0)
                {
                    using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
                    {
                        _subtotalChild.EntityID = _entityAccessClient.Insert1(_subtotalChild);
                        relation.TargetEntity = _subtotalChild;
                    }
                }

                switch (relation.Description)
                {
                    case RelationDescription.Allocate:
                        {
                            _lastLevel = FindLastLevel(_subtotalChild.SubEntities, _relationEntity.EntityName);

                            if (_lastLevel != null)
                            {
                                break;
                            }

                            _lastLevel = new Entity()
                            {
                                ParentID = _subtotalChild.EntityID,
                                IsLastLevel = 1,
                                Enable = 1,
                                EntityName = _relationEntity.EntityName,
                                EntityType = _transactionEntity.EntityType,
                                Currency = new Currency() { CurrencyID = _relationEntity.Currency.CurrencyID },
                                ExchangeRate = _relationEntity.ExchangeRate,
                                SumType = SumType.Not,
                            };
                        }
                        break;

                    case RelationDescription.Position:
                        {
                            _lastLevel = FindLastLevel(_subtotalChild.SubEntities, string.Format("{0}{1}", _posionName, _relationEntity.EntityName));

                            if (_lastLevel != null)
                            {
                                break;
                            }

                            _lastLevel = new Entity()
                            {
                                ParentID = _subtotalChild.EntityID,
                                IsLastLevel = 1,
                                Enable = 1,
                                EntityName = string.Format("{0}{1}", _posionName, _relationEntity.EntityName),
                                EntityType = _transactionEntity.EntityType,
                                Currency = new Currency() { CurrencyID = _relationEntity.Currency.CurrencyID },
                                ExchangeRate = _relationEntity.ExchangeRate,
                                SumType = SumType.Not,
                            };
                        }
                        break;

                    case RelationDescription.Commission:
                        {
                            _lastLevel = FindLastLevel(_subtotalChild.SubEntities, string.Format("{0}{1}", _relationEntity.EntityName, _commisionName));

                            if (_lastLevel != null)
                            {
                                break;
                            }

                            _lastLevel = new Entity()
                            {
                                ParentID = _subtotalChild.EntityID,
                                IsLastLevel = 1,
                                Enable = 1,
                                EntityName = string.Format("{0}{1}", _relationEntity.EntityName, _commisionName),
                                EntityType = _transactionEntity.EntityType,
                                Currency = new Currency() { CurrencyID = _relationEntity.Currency.CurrencyID },
                                ExchangeRate = _relationEntity.ExchangeRate,
                                SumType = SumType.Not,
                            };
                        }
                        break;
                }

                if (_lastLevel.EntityID == 0)
                {
                    using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
                    {
                        _lastLevel.EntityID = _entityAccessClient.Insert1(_lastLevel);
                        relation.TargetEntity = _lastLevel;
                    }
                }

                switch (relation.Description)
                {
                    case RelationDescription.Commission:
                        {
                            //_lastLevel = FindChild(_relationEntity.SubEntities, string.Format("{0}{1}", _relationEntity.EntityName, _commisionName));

                            //if (_lastLevel != null)
                            //{
                            //    break;
                            //}

                            _lastLevel = new Entity()
                            {
                                ParentID = _relationEntity.ParentID,
                                IsLastLevel = 1,
                                Enable = 1,
                                EntityName = string.Format("{0}{1}", _relationEntity.EntityName, _commisionName),
                                EntityType = _relationEntity.EntityType,
                                Currency = new Currency() { CurrencyID = _relationEntity.Currency.CurrencyID },
                                ExchangeRate = _relationEntity.ExchangeRate,
                                SumType = SumType.Not,
                            };

                            using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
                            {
                                _lastLevel.EntityID = _entityAccessClient.Insert1(_lastLevel);
                            }
                        }
                        break;
                }
            }

            using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
            {
                _entityAccessClient.SetRelateEntity(relation);
            }
        }

        public string QuerySingleBranchEntity(int entityID)
        {
            RelationCollection relationCollection = new RelationCollection(GetTallyRelationEntity(entityID));
            EntityCollection _newEC = new EntityCollection();
            EntityCollection _entityCollection = new EntityCollection(querySingleParentLine(_newEC,relationCollection[0].Entity.EntityID));
            CalculateService _cs = new CalculateService();
            for (int _i = _entityCollection.Count-1; _i >= 0; _i--)
            {

                int _parentID = _entityCollection[_i].ParentID;

                if (_parentID == 0)
                {
                    continue;
                }
                
                WeeklySummaryCollection _weeklySummary = _cs.GetWeeklySummary(_entityCollection[_i].EntityID);
                _entityCollection[_i].EntityName = string.Format("{0} ({1})", _entityCollection[_i].EntityName, _weeklySummary[0].BaseBalance);

                Entity _parent = FindParent(_entityCollection, _parentID);
                var _children = new EntityCollection(_entityCollection.Where(Entity => (Entity.ParentID == _parentID)));

                _parent.SubEntities=_children;
                _entityCollection.RemoveAll(Entity => (Entity.ParentID == _parentID));

                _i = _entityCollection.Count;
            }
            return EntityTree.LoadPnLEntityTree(_entityCollection);
        }

        private EntityCollection querySingleParentLine(EntityCollection entityCollection, int entityID)
        {            
            Entity _entity = LoadEntity(entityID)[0];
            entityCollection.Add(_entity);
            if (_entity.ParentID != 0)
                querySingleParentLine(entityCollection, _entity.ParentID);
            return entityCollection;
        }

        private SumType subSumType(SumType sumType)
        {
            switch (sumType)
            {
                case SumType.Transaction:
                    return SumType.Subtotal;
                case SumType.Subtotal:
                    return SumType.Super;
                case SumType.Super:
                    return SumType.Master;
                case SumType.Master:
                    return SumType.Agent;
                case SumType.Agent:
                    return SumType.Not;
                default:
                    return sumType;
            }
        }

        private void checkExistEntityAllocate(Entity targetEntity, Entity fromEntity, RelationDescription description)
        {
            SumType _subSumType = subSumType(targetEntity.SumType);
            EntityCollection _entityCollection = FindChildren(targetEntity.SubEntities, _subSumType);
            Entity _fromEntitySpecifyParent = FindParent(fromEntity.ParentID, _subSumType);
            string _entityName = "";
            Entity _subChild = null;
            switch (description)
            {
                case RelationDescription.Allocate:
                    _entityName = _fromEntitySpecifyParent.EntityName;                    
                    break;
                case RelationDescription.Position:
                    _entityName = string.Format("Position from {0}", _fromEntitySpecifyParent.EntityName);
                    break;
                case RelationDescription.Commission:
                    _entityName = string.Format("{0} comm", _fromEntitySpecifyParent.EntityName);
                    break;
            }

            foreach (Entity _subEntity in _entityCollection)
            {
                _subChild = FindChild(_subEntity.SubEntities, _fromEntitySpecifyParent.EntityName);

                if (_subChild != null)
                {
                    break;
                }
            }

            if (_subChild != null)
            {
                _subChild = new Entity()
                {
                    ParentID = targetEntity.EntityID,
                    Enable = 1,
                    EntityName = _entityName,
                    EntityType = targetEntity.EntityType,
                    Currency = new Currency() { CurrencyID = _fromEntitySpecifyParent.Currency.CurrencyID },
                    ExchangeRate = _fromEntitySpecifyParent.ExchangeRate,
                    SumType = _subSumType,
                };
            }

            if (_subChild.EntityID == 0)
            {
                using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
                {
                    _subChild.EntityID = _entityAccessClient.Insert1(_subChild);
                }
            }
        }
        
        public void SetRelation(int entityID, Relation relation)
        {
            //EntityCollection _entityCollection = LoadEntity2(entityID);

            //foreach (Entity _entity in _entityCollection)
            //{
            //    Relation _relation = _entity.RelationCollection.FirstOrDefault(Relation => (Relation.TargetEntity == relation.TargetEntity));

            //    if (_relation == null)
            //    {
            //        _entity.RelationCollection.Add(_relation);
            //    }
            //    else
            //    {
            //        _relation.Numeric = relation.Numeric;
            //    }
            //    using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
            //    {
            //        _entityAccessClient.SetRelateEntity(_relation);
            //    }
            //}
            using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
            {
                _entityAccessClient.SetRelateEntity(relation);
            }
        }

        public void SetCurrencyAndRate(int entityID, string currencyID, decimal rate)
        {//OK
            //EntityCollection _entityCollection = LoadEntity2(entityID);

            //foreach (Entity _entity in _entityCollection)
            //{
            //    _entity.Currency.CurrencyID = currencyID;
            //    _entity.ExchangeRate = rate;

            //    SaveEntity(_entity);
            //}
            using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
            {
                EntityCollection _entityCollection = LoadEntity(entityID);
                
                if (_entityCollection[0].SumType == SumType.Subtotal)
                {
                    EntityCollection _entitySubEntities = _entityCollection[0].SubEntities;
                    foreach (var sub in _entitySubEntities)
                    {
                        Currency _subCurrency = new Currency {CurrencyID = currencyID};
                        _entityAccessClient.SetRate(sub.EntityID, rate, _subCurrency);
                        foreach (var sub2 in sub.SubEntities)
                            SetCurrencyAndRate(sub2.EntityID, currencyID, rate);
                    }
                }
                _entityCollection[0].EntityName = rate.ToString();
                _entityCollection[0].Currency = new Currency { CurrencyID = currencyID };
                _entityCollection[0].ExchangeRate = rate;
                _entityAccessClient.Update1(_entityCollection[0]);
            }
        }

        public RelationCollection GetRelateEntity(int entityID)
        {
            using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
            {
                return new RelationCollection(_entityAccessClient.GetRelateEntity(entityID));
            }
        }

        public RelationCollection GetTallyRelationEntity(int entityID)
        {
            using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
            {
                return new RelationCollection(_entityAccessClient.GetTallyRelationEntity(entityID));
            }
        }


        public void ChangeRate(int entityID, decimal rate)
        {
            EntityCollection _entityCollection = LoadEntity(entityID);

            foreach (Entity _entity in _entityCollection)
            {
                _entity.ExchangeRate = rate;

                SaveEntity(_entity);
            }
        }

        public void ChangeStatus(User user,int entityID,Status status)
        {
            using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
            {
                _entityAccessClient.ChangeStatus(user, entityID, status);
            }
        }

        public EntityCollection GetEntryEntities(string userName)
        {
            //throw new NotImplementedException(); //TODO:
            return new EntityCollection();
        }

        private void DisableOrEnable(EntityCollection entityCollection, int enable)
        {
            foreach (Entity _entity in entityCollection)
            {
                DisableOrEnable(_entity.SubEntities, enable);

                using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
                {
                    if (enable == 0)
                    {
                        _entityAccessClient.Disable(_entity.EntityID);
                    }
                    else if (enable == 1)
                    {
                        _entityAccessClient.Enable(_entity.EntityID);
                    }
                }

                RelationCollection _relationCollection = EntityService.Instance.RelationEntityWandL(_entity.EntityID);

                foreach (Relation _relation in _relationCollection)
                {
                    RemoveRelation(_entity.EntityID, _relation.TargetEntity.EntityID);
                }
            }
        }

        public void Disable(int entityID)
        {
            DisableOrEnable(LoadEntity(entityID), 0);
        }

        public void Enable(int entityID)
        {
            DisableOrEnable(LoadEntity(entityID), 1);
        }

        public RelationCollection RelationEntity(int _entityID)
        {
            using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
            {
                //First Get Relation Table
                RelationCollection _recollection = new RelationCollection(_entityAccessClient.GetRelateEntity(_entityID));
                foreach (Relation _re in _recollection)
                {
                    _re.Entity = new EntityCollection(_entityAccessClient.QueryRelation(_re.TargetEntity.EntityID))[0];
                }
                return _recollection;
            }
        }

        public RelationCollection RelationEntityWandL(int _entityID)
        {
            using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
            {       
                RelationCollection _recollection = new RelationCollection(_entityAccessClient.GetRelateEntityWandL(_entityID));                
                return _recollection;
            }
        }

        public void RemoveRelation(int entityID, int targetEntityID)
        {
            using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
            {
                _entityAccessClient.RemoveRelation(entityID,targetEntityID);
            }
        }

       
        public EntityCollection CheckTransactionTransfer(int entityID)
        {
            using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
            {              
                int _periodID = PeriodService.Instance.GetCurrentPeriod()[0].ID;
                EntityCollection _ecollection  = new EntityCollection();
                foreach (Entity _entity in new EntityCollection(_entityAccessClient.QueryAllSub1(entityID)))
                {
                    WeeklySummaryAccessClient _client = new WeeklySummaryAccessClient(EndpointName.WeeklySummaryAccess);
                    if (!_client.IsWeeklyConfirm(_periodID, _entity.EntityID))
                        _ecollection.Add(_entity);                    
                }
                return _ecollection;
            }
        }

    }
}
