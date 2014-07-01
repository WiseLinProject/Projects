using System.Collections.Generic;
using Oleit.AS.Service.DataObject;
using Accounting_System.EntityServiceReference;
using Newtonsoft.Json;
using System.Linq;

namespace Accounting_System
{
    public static class JsonEntityFunc
    {
        public static string LoadEntityTree()
        {
            var _esc = new EntityServiceClient();
            var _loadEntity = new EntityCollection(_esc.LoadEntity1());
            //JsonEntityTreeString = _loadEntityTest.SerializeToJson();
            var _jetc = new JsonEntitiesTreeCollection();
            for (int i = _loadEntity.Count - 1; i >= 0; i--)
            {
                var _classType = _loadEntity[i].ParentID == 0
                                     ? _loadEntity[i].EntityType.Equals(EntityType.Cash)
                                           ? "CashMainEntity"
                                           : "MainEntity"
                                     : _loadEntity[i].IsLastLevel == 1
                                           ? _loadEntity[i].IsAccount == 1 ? "Account" : "LastNodeEntity"
                                           : "NodeEntity";
                var _entityType = _loadEntity[i].EntityType == EntityType.PAndL
                                      ? "PnL"
                                      : _loadEntity[i].EntityType == EntityType.Cash
                                            ? "Cash"
                                            : _loadEntity[i].EntityType == EntityType.Expence ? "Exp" : "BadDebt";
                CashEntity _cashEntity;
                if (_loadEntity[i].EntityType.Equals(EntityType.Cash) && _loadEntity[i].ParentID == 0)
                {
                    _cashEntity = _esc.LoadCashEntity2(_loadEntity[i].EntityID)[0];
                }
                else
                {
                    _cashEntity = null;
                }
                string _entityName = string.Format("{0} ({1})", _loadEntity[i].EntityName, _entityType.Replace("PAndL", "P&L"));
                JsonEntitiesTree _entityParent = new JsonEntitiesTree();
                if (_cashEntity != null)
                {
                    _entityParent = new JsonEntitiesTree
                    {
                        #region "Cash Main Entity"
                        attr = new JsonEntitiesAttr
                        {
                            id = _loadEntity[i].EntityID,
                            ParentID = _loadEntity[i].ParentID,
                            @class = _classType,
                            Enable = _loadEntity[i].Enable,
                            EntityType = _entityType,
                            SumType = _loadEntity[i].SumType,
                            Currency = _loadEntity[i].Currency.CurrencyID,
                            ER = _loadEntity[i].ExchangeRate,
                            EntityName = _loadEntity[i].EntityName,
                            ContractNumber = _cashEntity.ContractNumber,
                            TallyName = _cashEntity.TallyName,
                            TallyNumber = _cashEntity.TallyNumber,
                            SettlementName = _cashEntity.SettlementName,
                            SettlementNumber = _cashEntity.SettlementNumber,
                            RecommendedBy = _cashEntity.RecommendedBy,
                            Skype = _cashEntity.Skype,
                            QQ = _cashEntity.QQ,
                            Email = _cashEntity.Email,
                            CreditLimit = _cashEntity.CreditLimit,
                        },
                        data = _entityName,
                        children = subLoadEntityTree(_loadEntity[i].SubEntities)
                        #endregion
                    };
                }
                else
                {
                    _entityParent = new JsonEntitiesTree
                    {
                        #region "Main Entity"
                        attr = new JsonEntitiesAttr
                        {
                            id = _loadEntity[i].EntityID,
                            ParentID = _loadEntity[i].ParentID,
                            @class = _classType,
                            Enable = _loadEntity[i].Enable,
                            EntityType = _entityType,
                            SumType = _loadEntity[i].SumType,
                            Currency = _loadEntity[i].Currency.CurrencyID,
                            ER = _loadEntity[i].ExchangeRate,
                            EntityName = _loadEntity[i].EntityName,
                        },
                        data = _entityName,
                        children = subLoadEntityTree(_loadEntity[i].SubEntities)
                        #endregion
                    };
                }
                _jetc.Add(_entityParent);
            }
            //JsonConvert.SerializeObject(this, Formatting.Indented);
            return JsonConvert.SerializeObject(_jetc);
            //JsonEntityTreeString = "[{\"attr\":{\"id\":\"Lesile_PnL\",\"href\":\"#\",\"class\":\"mainEntity\"},\"data\":\"Lesile(P&L)\",\"children\":[{\"attr\":{\"id\":\"Kevin\",\"class\":\"LastNodeEntity\"},\"data\":\"PropertiesSetting\"}]},{\"attr\":{\"id\":\"178NiuNiu_Cash\",\"href\":\"#\",\"class\":\"mainEntity\"},\"data\":\"178NiuNiu(Cash)\",\"children\":[{\"attr\":{\"id\":\"178NiuNiuA\",\"class\":\"NodeEntity\"},\"data\":\"178NiuNiuA\",\"children\":[{\"attr\":{\"id\":\"8ga12\",\"class\":\"LastNodeEntity\"},\"data\":\"8ga12\"},{\"attr\":{\"id\":\"ah23\",\"class\":\"account\"},\"data\":\"ah23\"}]},{\"attr\":{\"id\":\"178NiuiuB\",\"class\":\"LastNodeEntity\"},\"data\":\"178NiuNiuB\"}]}]";
        }

        public static string PnLEntityTree()
        {
            var _esc = new EntityServiceClient();
            var _loadEntity = new EntityCollection(_esc.LoadEntity1());
            //JsonEntityTreeString = _loadEntityTest.SerializeToJson();
            var _jetc = new JsonEntitiesTreeCollection();
            for (int i = _loadEntity.Count - 1; i >= 0; i--)
            {
                var _classType = _loadEntity[i].ParentID == 0
                                     ? _loadEntity[i].EntityType.Equals(EntityType.Cash)
                                           ? "CashMainEntity"
                                           : "MainEntity"
                                     : _loadEntity[i].IsLastLevel == 1
                                           ? _loadEntity[i].IsAccount == 1 ? "Account" : "LastNodeEntity"
                                           : "NodeEntity";
                var _entityType = _loadEntity[i].EntityType == EntityType.PAndL
                                      ? "PnL"
                                      : _loadEntity[i].EntityType == EntityType.Cash
                                            ? "Cash"
                                            : _loadEntity[i].EntityType == EntityType.Expence ? "Exp" : "BadDebt";

                string _entityName = string.Format("{0} ({1})", _loadEntity[i].EntityName, _entityType.Replace("PAndL", "P&L"));
                JsonEntitiesTree _entityParent = new JsonEntitiesTree();
                
                if (_loadEntity[i].EntityType.Equals(EntityType.PAndL) && _loadEntity[i].ParentID == 0)
                {
                    _entityParent = new JsonEntitiesTree
                    {
                        #region "Main Entity"
                        attr = new JsonEntitiesAttr
                        {
                            id = _loadEntity[i].EntityID,
                            ParentID = _loadEntity[i].ParentID,
                            @class = _classType,
                            Enable = _loadEntity[i].Enable,
                            EntityType = _entityType,
                            SumType = _loadEntity[i].SumType,
                            Currency = _loadEntity[i].Currency.CurrencyID,
                            ER = _loadEntity[i].ExchangeRate,
                            EntityName = _loadEntity[i].EntityName,
                        },
                        data = _entityName,
                        children = subLoadEntityTree(_loadEntity[i].SubEntities)
                        #endregion
                    };
                }
                _jetc.Add(_entityParent);
            }
            //JsonConvert.SerializeObject(this, Formatting.Indented);
            return JsonConvert.SerializeObject(_jetc);
            //JsonEntityTreeString = "[{\"attr\":{\"id\":\"Lesile_PnL\",\"href\":\"#\",\"class\":\"mainEntity\"},\"data\":\"Lesile(P&L)\",\"children\":[{\"attr\":{\"id\":\"Kevin\",\"class\":\"LastNodeEntity\"},\"data\":\"PropertiesSetting\"}]},{\"attr\":{\"id\":\"178NiuNiu_Cash\",\"href\":\"#\",\"class\":\"mainEntity\"},\"data\":\"178NiuNiu(Cash)\",\"children\":[{\"attr\":{\"id\":\"178NiuNiuA\",\"class\":\"NodeEntity\"},\"data\":\"178NiuNiuA\",\"children\":[{\"attr\":{\"id\":\"8ga12\",\"class\":\"LastNodeEntity\"},\"data\":\"8ga12\"},{\"attr\":{\"id\":\"ah23\",\"class\":\"account\"},\"data\":\"ah23\"}]},{\"attr\":{\"id\":\"178NiuiuB\",\"class\":\"LastNodeEntity\"},\"data\":\"178NiuNiuB\"}]}]";
        }

        public static string LoadSubEntitiesTree(int entityID)
        {
            var _esc = new EntityServiceClient();
            var _loadEntity = new EntityCollection(_esc.LoadEntity2(entityID)[0].SubEntities);
            //JsonEntityTreeString = _loadEntityTest.SerializeToJson();
            var _jetc = new JsonEntitiesTreeCollection();
            for (int i = _loadEntity.Count - 1; i >= 0; i--)
            {
                var _classType = _loadEntity[i].ParentID == 0
                                     ? _loadEntity[i].EntityType.Equals(EntityType.Cash)
                                           ? "CashMainEntity"
                                           : "MainEntity"
                                     : _loadEntity[i].IsLastLevel == 1
                                           ? _loadEntity[i].IsAccount == 1 ? "Account" : "LastNodeEntity"
                                           : "NodeEntity";
                var _entityType = _loadEntity[i].EntityType == EntityType.PAndL
                                      ? "PnL"
                                      : _loadEntity[i].EntityType == EntityType.Cash
                                            ? "Cash"
                                            : _loadEntity[i].EntityType == EntityType.Expence ? "Exp" : "BadDebt";
                CashEntity _cashEntity;
                if (_loadEntity[i].EntityType.Equals(EntityType.Cash) && _loadEntity[i].ParentID == 0)
                {
                    _cashEntity = _esc.LoadCashEntity2(_loadEntity[i].EntityID)[0];
                }
                else
                {
                    _cashEntity = null;
                }

                string _entityName = string.Format("{0} ({1})", _loadEntity[i].EntityName, _loadEntity[i].SumType);
                JsonEntitiesTree _entityParent;
                if (_cashEntity != null)
                {
                    _entityParent = new JsonEntitiesTree
                    {
                        #region "Cash Main Entity"
                        attr = new JsonEntitiesAttr
                        {
                            id = _loadEntity[i].EntityID,
                            Islastlevel = 0,
                            IsAccount = 0,
                            ParentID = _loadEntity[i].ParentID,
                            @class = _classType,
                            Enable = _loadEntity[i].Enable,
                            EntityType = _entityType,
                            SumType = _loadEntity[i].SumType,
                            Currency = _loadEntity[i].Currency.CurrencyID,
                            ER = _loadEntity[i].ExchangeRate,
                            EntityName = _loadEntity[i].EntityName,
                            ContractNumber = _cashEntity.ContractNumber,
                            TallyName = _cashEntity.TallyName,
                            TallyNumber = _cashEntity.TallyNumber,
                            SettlementName = _cashEntity.SettlementName,
                            SettlementNumber = _cashEntity.SettlementNumber,
                            RecommendedBy = _cashEntity.RecommendedBy,
                            Skype = _cashEntity.Skype,
                            QQ = _cashEntity.QQ,
                            Email = _cashEntity.Email,
                            CreditLimit = _cashEntity.CreditLimit,
                        },
                        data = _entityName,
                        children = subLoadEntityTree(_loadEntity[i].SubEntities)
                        #endregion
                    };
                }
                else
                {
                    _entityParent = new JsonEntitiesTree
                    {
                        #region "Main Entity"
                        attr = new JsonEntitiesAttr
                        {
                            id = _loadEntity[i].EntityID,
                            Islastlevel = 0,
                            IsAccount = 0,
                            ParentID = _loadEntity[i].ParentID,
                            @class = _classType,
                            Enable = _loadEntity[i].Enable,
                            EntityType = _entityType,
                            SumType = _loadEntity[i].SumType,
                            Currency = _loadEntity[i].Currency.CurrencyID,
                            ER = _loadEntity[i].ExchangeRate,
                            EntityName = _loadEntity[i].EntityName,
                        },
                        data = _entityName,
                        children = subLoadEntityTree(_loadEntity[i].SubEntities)
                        #endregion
                    };
                }
                _jetc.Add(_entityParent);
            }
            //JsonConvert.SerializeObject(this, Formatting.Indented);
            return JsonConvert.SerializeObject(_jetc);
            //JsonEntityTreeString = "[{\"attr\":{\"id\":\"Lesile_PnL\",\"href\":\"#\",\"class\":\"mainEntity\"},\"data\":\"Lesile(P&L)\",\"children\":[{\"attr\":{\"id\":\"Kevin\",\"class\":\"LastNodeEntity\"},\"data\":\"PropertiesSetting\"}]},{\"attr\":{\"id\":\"178NiuNiu_Cash\",\"href\":\"#\",\"class\":\"mainEntity\"},\"data\":\"178NiuNiu(Cash)\",\"children\":[{\"attr\":{\"id\":\"178NiuNiuA\",\"class\":\"NodeEntity\"},\"data\":\"178NiuNiuA\",\"children\":[{\"attr\":{\"id\":\"8ga12\",\"class\":\"LastNodeEntity\"},\"data\":\"8ga12\"},{\"attr\":{\"id\":\"ah23\",\"class\":\"account\"},\"data\":\"ah23\"}]},{\"attr\":{\"id\":\"178NiuiuB\",\"class\":\"LastNodeEntity\"},\"data\":\"178NiuNiuB\"}]}]";
        }

        public static string LoadMainEntityTree(int entityType)
        {
            var _esc = new EntityServiceClient();
            EntityCollection _loadEntity;
            if (entityType > 0)
            {
                EntityType _entityType = entityType == 1 ? EntityType.PAndL :
                    entityType == 2 ? EntityType.Cash :
                    entityType == 3 ? EntityType.Expence :
                    entityType == 4 ? EntityType.BadDebt : EntityType.MLJ;
                 _loadEntity = new EntityCollection(_esc.LoadEntity1().Where(x=>x.EntityType==_entityType));
            }
            else
            {
                 _loadEntity = new EntityCollection(_esc.LoadEntity1().OrderByDescending(x => x.EntityType));
            }
            //JsonEntityTreeString = _loadEntityTest.SerializeToJson();
            var _jetc = new JsonEntitiesTreeCollection();
            for (int i = _loadEntity.Count - 1; i >= 0; i--)
            {
                var _classType = _loadEntity[i].ParentID == 0
                                     ? _loadEntity[i].EntityType.Equals(EntityType.Cash)
                                           ? "CashMainEntity"
                                           : "MainEntity"
                                     : _loadEntity[i].IsLastLevel == 1
                                           ? _loadEntity[i].IsAccount == 1 ? "Account" : "LastNodeEntity"
                                           : "NodeEntity";                
                CashEntity _cashEntity;
                if (_loadEntity[i].EntityType.Equals(EntityType.Cash) && _loadEntity[i].ParentID == 0)
                {
                    _cashEntity = _esc.LoadCashEntity2(_loadEntity[i].EntityID)[0];
                }
                else
                {
                    _cashEntity = null;
                }
                
                string _entityType = string.Format("{0}", _loadEntity[i].EntityType);
                string _entityName = string.Format("{0} ({1})", _loadEntity[i].EntityName, _entityType.Replace("PAndL", "P&L"));
                JsonEntitiesTree _entityParent;
                if (_cashEntity != null)
                {
                    _entityParent = new JsonEntitiesTree
                    {
                        #region "Cash Main Entity"
                        attr = new JsonEntitiesAttr
                        {
                            id = _loadEntity[i].EntityID,
                            Islastlevel = 0,
                            IsAccount = 0,
                            ParentID = _loadEntity[i].ParentID,
                            @class = _classType,
                            Enable = _loadEntity[i].Enable,
                            EntityType = _entityType,
                            SumType = _loadEntity[i].SumType,
                            Currency = _loadEntity[i].Currency.CurrencyID,
                            ER = _loadEntity[i].ExchangeRate,
                            EntityName = _loadEntity[i].EntityName,
                            ContractNumber = _cashEntity.ContractNumber,
                            TallyName = _cashEntity.TallyName,
                            TallyNumber = _cashEntity.TallyNumber,
                            SettlementName = _cashEntity.SettlementName,
                            SettlementNumber = _cashEntity.SettlementNumber,
                            RecommendedBy = _cashEntity.RecommendedBy,
                            Skype = _cashEntity.Skype,
                            QQ = _cashEntity.QQ,
                            Email = _cashEntity.Email,
                            CreditLimit = _cashEntity.CreditLimit,
                        },
                        data = _entityName
                        #endregion
                    };
                }
                else
                {
                    _entityParent = new JsonEntitiesTree
                    {
                        #region "Main Entity"
                        attr = new JsonEntitiesAttr
                        {
                            id = _loadEntity[i].EntityID,
                            Islastlevel = 0,
                            IsAccount = 0,
                            ParentID = _loadEntity[i].ParentID,
                            @class = _classType,
                            Enable = _loadEntity[i].Enable,
                            EntityType = _entityType,
                            SumType = _loadEntity[i].SumType,
                            Currency = _loadEntity[i].Currency.CurrencyID,
                            ER = _loadEntity[i].ExchangeRate,
                            EntityName = _loadEntity[i].EntityName,
                        },
                        data = _entityName
                        #endregion
                    };
                }
                _jetc.Add(_entityParent);
            }
            //JsonConvert.SerializeObject(this, Formatting.Indented);
            return JsonConvert.SerializeObject(_jetc);
            //JsonEntityTreeString = "[{\"attr\":{\"id\":\"Lesile_PnL\",\"href\":\"#\",\"class\":\"mainEntity\"},\"data\":\"Lesile(P&L)\",\"children\":[{\"attr\":{\"id\":\"Kevin\",\"class\":\"LastNodeEntity\"},\"data\":\"PropertiesSetting\"}]},{\"attr\":{\"id\":\"178NiuNiu_Cash\",\"href\":\"#\",\"class\":\"mainEntity\"},\"data\":\"178NiuNiu(Cash)\",\"children\":[{\"attr\":{\"id\":\"178NiuNiuA\",\"class\":\"NodeEntity\"},\"data\":\"178NiuNiuA\",\"children\":[{\"attr\":{\"id\":\"8ga12\",\"class\":\"LastNodeEntity\"},\"data\":\"8ga12\"},{\"attr\":{\"id\":\"ah23\",\"class\":\"account\"},\"data\":\"ah23\"}]},{\"attr\":{\"id\":\"178NiuiuB\",\"class\":\"LastNodeEntity\"},\"data\":\"178NiuNiuB\"}]}]";
        }

        public static string LoadMainEntityTree(string mainEntityName,int entityType)
        {
            var _esc = new EntityServiceClient();
            EntityCollection _loadEntity;
            if (entityType > 0)
            {
                EntityType _entityType = EntitiesFunc.entityTypeFunc(entityType);
                _loadEntity = new EntityCollection(_esc.LoadEntity1().Where(x => x.EntityName.ToUpper().Contains(mainEntityName.ToUpper()) && x.EntityType == _entityType));
            }
            else
            {
                _loadEntity = new EntityCollection(_esc.LoadEntity1().Where(x => x.EntityName.ToUpper().Contains(mainEntityName.ToUpper())));
            }
            //JsonEntityTreeString = _loadEntityTest.SerializeToJson();
            var _jetc = new JsonEntitiesTreeCollection();
            for (int i = _loadEntity.Count - 1; i >= 0; i--)
            {
                var _classType = _loadEntity[i].ParentID == 0
                                     ? _loadEntity[i].EntityType.Equals(EntityType.Cash)
                                           ? "CashMainEntity"
                                           : "MainEntity"
                                     : _loadEntity[i].IsLastLevel == 1
                                           ? _loadEntity[i].IsAccount == 1 ? "Account" : "LastNodeEntity"
                                           : "NodeEntity";
                CashEntity _cashEntity;
                if (_loadEntity[i].EntityType.Equals(EntityType.Cash) && _loadEntity[i].ParentID == 0)
                {
                    _cashEntity = _esc.LoadCashEntity2(_loadEntity[i].EntityID)[0];
                }
                else
                {
                    _cashEntity = null;
                }
                string _entityType = string.Format("{0}", _loadEntity[i].EntityType);
                string _entityName = string.Format("{0} ({1})", _loadEntity[i].EntityName, _entityType.Replace("PAndL", "P&L"));
                JsonEntitiesTree _entityParent;
                if (_cashEntity != null)
                {
                    _entityParent = new JsonEntitiesTree
                    {
                        #region "Cash Main Entity"
                        attr = new JsonEntitiesAttr
                        {
                            id = _loadEntity[i].EntityID,
                            IsAccount = 0,
                            Islastlevel = 0,
                            ParentID = _loadEntity[i].ParentID,
                            @class = _classType,
                            Enable = _loadEntity[i].Enable,
                            EntityType = _entityType,
                            SumType = _loadEntity[i].SumType,
                            Currency = _loadEntity[i].Currency.CurrencyID,
                            ER = _loadEntity[i].ExchangeRate,
                            EntityName = _loadEntity[i].EntityName,
                            ContractNumber = _cashEntity.ContractNumber,
                            TallyName = _cashEntity.TallyName,
                            TallyNumber = _cashEntity.TallyNumber,
                            SettlementName = _cashEntity.SettlementName,
                            SettlementNumber = _cashEntity.SettlementNumber,
                            RecommendedBy = _cashEntity.RecommendedBy,
                            Skype = _cashEntity.Skype,
                            QQ = _cashEntity.QQ,
                            Email = _cashEntity.Email,
                            CreditLimit = _cashEntity.CreditLimit,
                        },
                        data = _entityName
                        #endregion
                    };
                }
                else
                {
                    _entityParent = new JsonEntitiesTree
                    {
                        #region "Main Entity"
                        attr = new JsonEntitiesAttr
                        {
                            id = _loadEntity[i].EntityID,
                            IsAccount = 0,
                            Islastlevel =0,
                            ParentID = _loadEntity[i].ParentID,
                            @class = _classType,
                            Enable = _loadEntity[i].Enable,
                            EntityType = _entityType,
                            SumType = _loadEntity[i].SumType,
                            Currency = _loadEntity[i].Currency.CurrencyID,
                            ER = _loadEntity[i].ExchangeRate,
                            EntityName = _loadEntity[i].EntityName,
                        },
                        data = _entityName
                        #endregion
                    };
                }
                _jetc.Add(_entityParent);
            }
            //JsonConvert.SerializeObject(this, Formatting.Indented);
            return JsonConvert.SerializeObject(_jetc);
            //JsonEntityTreeString = "[{\"attr\":{\"id\":\"Lesile_PnL\",\"href\":\"#\",\"class\":\"mainEntity\"},\"data\":\"Lesile(P&L)\",\"children\":[{\"attr\":{\"id\":\"Kevin\",\"class\":\"LastNodeEntity\"},\"data\":\"PropertiesSetting\"}]},{\"attr\":{\"id\":\"178NiuNiu_Cash\",\"href\":\"#\",\"class\":\"mainEntity\"},\"data\":\"178NiuNiu(Cash)\",\"children\":[{\"attr\":{\"id\":\"178NiuNiuA\",\"class\":\"NodeEntity\"},\"data\":\"178NiuNiuA\",\"children\":[{\"attr\":{\"id\":\"8ga12\",\"class\":\"LastNodeEntity\"},\"data\":\"8ga12\"},{\"attr\":{\"id\":\"ah23\",\"class\":\"account\"},\"data\":\"ah23\"}]},{\"attr\":{\"id\":\"178NiuiuB\",\"class\":\"LastNodeEntity\"},\"data\":\"178NiuNiuB\"}]}]";
        }

        public static string LoadRelationEntityTree()
        {
            var _esc = new EntityServiceClient();
            var _loadEntity = new EntityCollection(_esc.QueryRelationEntities());
            //JsonEntityTreeString = _loadEntityTest.SerializeToJson();
            var _jetc = new JsonEntitiesTreeCollection();
            for (int i = _loadEntity.Count - 1; i >= 0; i--)
            {
                var _classType = _loadEntity[i].ParentID == 0
                                     ? _loadEntity[i].EntityType.Equals(EntityType.Cash)
                                           ? "CashMainEntity"
                                           : "MainEntity"
                                     : _loadEntity[i].IsLastLevel == 1
                                           ? _loadEntity[i].IsAccount == 1 ? "Account" : "LastNodeEntity"
                                           : "NodeEntity";
                CashEntity _cashEntity;
                if (_loadEntity[i].EntityType.Equals(EntityType.Cash) && _loadEntity[i].ParentID == 0)
                {
                    _cashEntity = _esc.LoadCashEntity2(_loadEntity[i].EntityID)[0];
                }
                else
                {
                    _cashEntity = null;
                }
                string _entityType = string.Format("{0}", _loadEntity[i].EntityType);
                string _entityName = string.Format("{0} ({1})", _loadEntity[i].EntityName, _entityType.Replace("PAndL", "P&L"));
                JsonEntitiesTree _entityParent;
                if (_cashEntity != null)
                {
                    _entityParent = new JsonEntitiesTree
                    {
                        #region "Cash Main Entity"
                        attr = new JsonEntitiesAttr
                        {
                            id = _loadEntity[i].EntityID,
                            Islastlevel = 0,
                            IsAccount = 0,
                            ParentID = _loadEntity[i].ParentID,
                            @class = _classType,
                            Enable = _loadEntity[i].Enable,
                            EntityType = _entityType,
                            SumType = _loadEntity[i].SumType,
                            Currency = _loadEntity[i].Currency.CurrencyID,
                            ER = _loadEntity[i].ExchangeRate,
                            EntityName = _loadEntity[i].EntityName,
                            ContractNumber = _cashEntity.ContractNumber,
                            TallyName = _cashEntity.TallyName,
                            TallyNumber = _cashEntity.TallyNumber,
                            SettlementName = _cashEntity.SettlementName,
                            SettlementNumber = _cashEntity.SettlementNumber,
                            RecommendedBy = _cashEntity.RecommendedBy,
                            Skype = _cashEntity.Skype,
                            QQ = _cashEntity.QQ,
                            Email = _cashEntity.Email,
                            CreditLimit = _cashEntity.CreditLimit,
                        },
                        data = _entityName,
                        children = subLoadRelationEntityTree(_loadEntity[i].SubEntities)
                        #endregion
                    };
                }
                else
                {
                    _entityParent = new JsonEntitiesTree
                    {
                        #region "Main Entity"
                        attr = new JsonEntitiesAttr
                        {
                            id = _loadEntity[i].EntityID,
                            Islastlevel = 0,
                            IsAccount = 0,
                            ParentID = _loadEntity[i].ParentID,
                            @class = _classType,
                            Enable = _loadEntity[i].Enable,
                            EntityType = _entityType,
                            SumType = _loadEntity[i].SumType,
                            Currency = _loadEntity[i].Currency.CurrencyID,
                            ER = _loadEntity[i].ExchangeRate,
                            EntityName = _loadEntity[i].EntityName,
                        },
                        data = _entityName,
                        children = subLoadRelationEntityTree(_loadEntity[i].SubEntities)
                        #endregion
                    };
                }
                _jetc.Add(_entityParent);
            }
            //JsonConvert.SerializeObject(this, Formatting.Indented);
            return JsonConvert.SerializeObject(_jetc);
            //JsonEntityTreeString = "[{\"attr\":{\"id\":\"Lesile_PnL\",\"href\":\"#\",\"class\":\"mainEntity\"},\"data\":\"Lesile(P&L)\",\"children\":[{\"attr\":{\"id\":\"Kevin\",\"class\":\"LastNodeEntity\"},\"data\":\"PropertiesSetting\"}]},{\"attr\":{\"id\":\"178NiuNiu_Cash\",\"href\":\"#\",\"class\":\"mainEntity\"},\"data\":\"178NiuNiu(Cash)\",\"children\":[{\"attr\":{\"id\":\"178NiuNiuA\",\"class\":\"NodeEntity\"},\"data\":\"178NiuNiuA\",\"children\":[{\"attr\":{\"id\":\"8ga12\",\"class\":\"LastNodeEntity\"},\"data\":\"8ga12\"},{\"attr\":{\"id\":\"ah23\",\"class\":\"account\"},\"data\":\"ah23\"}]},{\"attr\":{\"id\":\"178NiuiuB\",\"class\":\"LastNodeEntity\"},\"data\":\"178NiuNiuB\"}]}]";
        }

        public static string LoadRelationEntityTree(string mainEntityName,int entityType)
        {
            var _esc = new EntityServiceClient();
            EntityCollection _loadEntity;
            if (mainEntityName != "")
            {
                if (entityType > 0)
                {
                    EntityType _entityType = EntitiesFunc.entityTypeFunc(entityType);
                    _loadEntity = new EntityCollection(_esc.QueryRelationEntities().Where(x => x.EntityName.ToUpper().Contains(mainEntityName.ToUpper()) && x.EntityType == _entityType));
                }
                else
                {
                    _loadEntity = new EntityCollection(_esc.QueryRelationEntities().Where(x => x.EntityName.ToUpper().Contains(mainEntityName.ToUpper())));
                }
            }
            else
            {
                if (entityType > 0)
                {
                    EntityType _entityType = EntitiesFunc.entityTypeFunc(entityType);
                    _loadEntity = new EntityCollection(_esc.QueryRelationEntities().Where(x => x.EntityType == _entityType));
                }
                else
                {
                    _loadEntity = new EntityCollection(_esc.QueryRelationEntities());
                }
            }
            var _jetc = new JsonEntitiesTreeCollection();
            for (int i = _loadEntity.Count - 1; i >= 0; i--)
            {
                var _classType = _loadEntity[i].ParentID == 0
                                     ? _loadEntity[i].EntityType.Equals(EntityType.Cash)
                                           ? "CashMainEntity"
                                           : "MainEntity"
                                     : _loadEntity[i].IsLastLevel == 1
                                           ? _loadEntity[i].IsAccount == 1 ? "Account" : "LastNodeEntity"
                                           : "NodeEntity";
                CashEntity _cashEntity;
                if (_loadEntity[i].EntityType.Equals(EntityType.Cash) && _loadEntity[i].ParentID == 0)
                {
                    _cashEntity = _esc.LoadCashEntity2(_loadEntity[i].EntityID)[0];
                }
                else
                {
                    _cashEntity = null;
                }
                string _entityType = string.Format("{0}", _loadEntity[i].EntityType);
                string _entityName = string.Format("{0} ({1})", _loadEntity[i].EntityName, _entityType.Replace("PAndL", "P&L"));
                JsonEntitiesTree _entityParent;
                if (_cashEntity != null)
                {
                    _entityParent = new JsonEntitiesTree
                    {
                        #region "Cash Main Entity"
                        attr = new JsonEntitiesAttr
                        {
                            id = _loadEntity[i].EntityID,
                            Islastlevel = 0,
                            IsAccount =0,
                            ParentID = _loadEntity[i].ParentID,
                            @class = _classType,
                            Enable = _loadEntity[i].Enable,
                            EntityType = _entityType,
                            SumType = _loadEntity[i].SumType,
                            Currency = _loadEntity[i].Currency.CurrencyID,
                            ER = _loadEntity[i].ExchangeRate,
                            EntityName = _loadEntity[i].EntityName,
                            ContractNumber = _cashEntity.ContractNumber,
                            TallyName = _cashEntity.TallyName,
                            TallyNumber = _cashEntity.TallyNumber,
                            SettlementName = _cashEntity.SettlementName,
                            SettlementNumber = _cashEntity.SettlementNumber,
                            RecommendedBy = _cashEntity.RecommendedBy,
                            Skype = _cashEntity.Skype,
                            QQ = _cashEntity.QQ,
                            Email = _cashEntity.Email,
                            CreditLimit = _cashEntity.CreditLimit,
                        },
                        data = _entityName,
                        children = subLoadRelationEntityTree(_loadEntity[i].SubEntities)
                        #endregion
                    };
                }
                else
                {
                    _entityParent = new JsonEntitiesTree
                    {
                        #region "Main Entity"
                        attr = new JsonEntitiesAttr
                        {
                            id = _loadEntity[i].EntityID,
                            Islastlevel = 0,
                            IsAccount = 0,
                            ParentID = _loadEntity[i].ParentID,
                            @class = _classType,
                            Enable = _loadEntity[i].Enable,
                            EntityType = _entityType,
                            SumType = _loadEntity[i].SumType,
                            Currency = _loadEntity[i].Currency.CurrencyID,
                            ER = _loadEntity[i].ExchangeRate,
                            EntityName = _loadEntity[i].EntityName,
                        },
                        data = _entityName,
                        children = subLoadRelationEntityTree(_loadEntity[i].SubEntities)
                        #endregion
                    };
                }
                _jetc.Add(_entityParent);
            }
            //JsonConvert.SerializeObject(this, Formatting.Indented);
            return JsonConvert.SerializeObject(_jetc);
            //JsonEntityTreeString = "[{\"attr\":{\"id\":\"Lesile_PnL\",\"href\":\"#\",\"class\":\"mainEntity\"},\"data\":\"Lesile(P&L)\",\"children\":[{\"attr\":{\"id\":\"Kevin\",\"class\":\"LastNodeEntity\"},\"data\":\"PropertiesSetting\"}]},{\"attr\":{\"id\":\"178NiuNiu_Cash\",\"href\":\"#\",\"class\":\"mainEntity\"},\"data\":\"178NiuNiu(Cash)\",\"children\":[{\"attr\":{\"id\":\"178NiuNiuA\",\"class\":\"NodeEntity\"},\"data\":\"178NiuNiuA\",\"children\":[{\"attr\":{\"id\":\"8ga12\",\"class\":\"LastNodeEntity\"},\"data\":\"8ga12\"},{\"attr\":{\"id\":\"ah23\",\"class\":\"account\"},\"data\":\"ah23\"}]},{\"attr\":{\"id\":\"178NiuiuB\",\"class\":\"LastNodeEntity\"},\"data\":\"178NiuNiuB\"}]}]";
        }

        //public static string LoadEntityTree(int relation)
        //{
        //    var _esc = new EntityServiceClient();
        //    var _loadEntity = new EntityCollection(_esc.LoadEntity1());
        //    //JsonEntityTreeString = _loadEntityTest.SerializeToJson();
        //    var _jetc = new JsonEntitiesTreeCollection();
        //    for (int i = _loadEntity.Count - 1; i >= 0; i--)
        //    {
        //        var _classType = _loadEntity[i].ParentID == 0
        //                             ? _loadEntity[i].EntityType.Equals(EntityType.Cash)
        //                                   ? "CashMainEntity"
        //                                   : "MainEntity"
        //                             : _loadEntity[i].IsLastLevel == 1
        //                                   ? _loadEntity[i].IsAccount == 1 ? "Account" : "LastNodeEntity"
        //                                   : "NodeEntity";
        //        var _entityType = _loadEntity[i].EntityType == EntityType.PAndL
        //                              ? "PnL"
        //                              : _loadEntity[i].EntityType == EntityType.Cash
        //                                    ? "Cash"
        //                                    : _loadEntity[i].EntityType == EntityType.Expence ? "Exp" : "BadDebt";
        //        CashEntity _cashEntity;
        //        if (_loadEntity[i].EntityType.Equals(EntityType.Cash) && _loadEntity[i].ParentID == 0)
        //        {
        //            _cashEntity = _esc.LoadCashEntity2(_loadEntity[i].EntityID)[0];
        //        }
        //        else
        //        {
        //            _cashEntity = null;
        //        }

        //        string _entityName = string.Format("{0} ({1})", _loadEntity[i].EntityName, _entityType);
        //        JsonEntitiesTree _entityParent;
        //        if (_cashEntity != null)
        //        {
        //            _entityParent = new JsonEntitiesTree
        //            {
        //                #region "Cash Main Entity"
        //                attr = new JsonEntitiesAttr
        //                {
        //                    id = _loadEntity[i].EntityID,
        //                    ParentID = _loadEntity[i].ParentID,
        //                    @class = _classType,
        //                    Enable = _loadEntity[i].Enable,
        //                    EntityType = _entityType,
        //                    SumType = _loadEntity[i].SumType,
        //                    Currency = _loadEntity[i].Currency.CurrencyID,
        //                    ER = _loadEntity[i].ExchangeRate,
        //                    EntityName = _loadEntity[i].EntityName,
        //                    ContractNumber = _cashEntity.ContractNumber,
        //                    TallyName = _cashEntity.TallyName,
        //                    TallyNumber = _cashEntity.TallyNumber,
        //                    SettlementName = _cashEntity.SettlementName,
        //                    SettlementNumber = _cashEntity.SettlementNumber,
        //                    RecommendedBy = _cashEntity.RecommendedBy,
        //                    Skype = _cashEntity.Skype,
        //                    QQ = _cashEntity.QQ,
        //                    Email = _cashEntity.Email,
        //                    CreditLimit = _cashEntity.CreditLimit,
        //                },
        //                data = _entityName,
        //                children = subLoadEntityTree(_loadEntity[i].SubEntities,0)
        //                #endregion
        //            };
        //        }
        //        else
        //        {
        //            _entityParent = new JsonEntitiesTree
        //            {
        //                #region "Main Entity"
        //                attr = new JsonEntitiesAttr
        //                {
        //                    id = _loadEntity[i].EntityID,
        //                    ParentID = _loadEntity[i].ParentID,
        //                    @class = _classType,
        //                    Enable = _loadEntity[i].Enable,
        //                    EntityType = _entityType,
        //                    SumType = _loadEntity[i].SumType,
        //                    Currency = _loadEntity[i].Currency.CurrencyID,
        //                    ER = _loadEntity[i].ExchangeRate,
        //                    EntityName = _loadEntity[i].EntityName,
        //                },
        //                data = _entityName,
        //                children = subLoadEntityTree(_loadEntity[i].SubEntities,0)
        //                #endregion
        //            };
        //        }
        //        _jetc.Add(_entityParent);
        //    }
        //    //JsonConvert.SerializeObject(this, Formatting.Indented);
        //    return JsonConvert.SerializeObject(_jetc);
        //    //JsonEntityTreeString = "[{\"attr\":{\"id\":\"Lesile_PnL\",\"href\":\"#\",\"class\":\"mainEntity\"},\"data\":\"Lesile(P&L)\",\"children\":[{\"attr\":{\"id\":\"Kevin\",\"class\":\"LastNodeEntity\"},\"data\":\"PropertiesSetting\"}]},{\"attr\":{\"id\":\"178NiuNiu_Cash\",\"href\":\"#\",\"class\":\"mainEntity\"},\"data\":\"178NiuNiu(Cash)\",\"children\":[{\"attr\":{\"id\":\"178NiuNiuA\",\"class\":\"NodeEntity\"},\"data\":\"178NiuNiuA\",\"children\":[{\"attr\":{\"id\":\"8ga12\",\"class\":\"LastNodeEntity\"},\"data\":\"8ga12\"},{\"attr\":{\"id\":\"ah23\",\"class\":\"account\"},\"data\":\"ah23\"}]},{\"attr\":{\"id\":\"178NiuiuB\",\"class\":\"LastNodeEntity\"},\"data\":\"178NiuNiuB\"}]}]";
        //}

        public static string LoadEntityTree(EntityCollection entityCollection, WeeklySummary[] wc)
        {
            var _esc = new EntityServiceClient();
            //JsonEntityTreeString = _loadEntityTest.SerializeToJson();
            var _jetc = new JsonEntitiesTreeCollection();
            for (int i = entityCollection.Count - 1; i >= 0; i--)
            {
                var _classType = entityCollection[i].ParentID == 0
                                     ? entityCollection[i].EntityType.Equals(EntityType.Cash)
                                           ? "CashMainEntity"
                                           : "MainEntity"
                                     : entityCollection[i].IsLastLevel == 1
                                           ? entityCollection[i].IsAccount == 1 ? "Account" : "LastNodeEntity"
                                           : "NodeEntity";
                CashEntity _cashEntity;
                if (entityCollection[i].EntityType.Equals(EntityType.Cash) && entityCollection[i].ParentID == 0)
                {
                    _cashEntity = _esc.LoadCashEntity2(entityCollection[i].EntityID)[0];
                }
                else
                {
                    _cashEntity = null;
                }
                string _entityType = string.Format("{0}", entityCollection[i].EntityType);
                string _entityName = string.Format("{0} ({1})", entityCollection[i].EntityName, _entityType.Replace("PAndL", "P&L"));
                JsonEntitiesTree _entityParent;
                if (_cashEntity != null)
                {
                    _entityParent = new JsonEntitiesTree
                    {
                        #region "Cash Main Entity"
                        attr = new JsonEntitiesAttr
                        {
                            id = entityCollection[i].EntityID,
                            Islastlevel = 0,
                            IsAccount = 0,
                            ParentID = entityCollection[i].ParentID,
                            @class = _classType,
                            Enable = entityCollection[i].Enable,
                            EntityType = _entityType,
                            SumType = entityCollection[i].SumType,
                            Currency = entityCollection[i].Currency.CurrencyID,
                            ER = entityCollection[i].ExchangeRate,
                            EntityName = entityCollection[i].EntityName,
                            ContractNumber = _cashEntity.ContractNumber,
                            TallyName = _cashEntity.TallyName,
                            TallyNumber = _cashEntity.TallyNumber,
                            SettlementName = _cashEntity.SettlementName,
                            SettlementNumber = _cashEntity.SettlementNumber,
                            RecommendedBy = _cashEntity.RecommendedBy,
                            Skype = _cashEntity.Skype,
                            QQ = _cashEntity.QQ,
                            Email = _cashEntity.Email,
                            CreditLimit = _cashEntity.CreditLimit,
                        },
                        data = _entityName,
                        children = subLoadEntityTree(entityCollection[i].SubEntities, wc, entityCollection[i].EntityType)
                        #endregion
                    };
                }
                else
                {
                    _entityParent = new JsonEntitiesTree
                    {
                        #region "Main Entity"
                        attr = new JsonEntitiesAttr
                        {
                            id = entityCollection[i].EntityID,
                            Islastlevel = 0,
                            IsAccount = 0,
                            ParentID = entityCollection[i].ParentID,
                            @class = _classType,
                            Enable = entityCollection[i].Enable,
                            EntityType = _entityType,
                            SumType = entityCollection[i].SumType,
                            Currency = entityCollection[i].Currency.CurrencyID,
                            ER = entityCollection[i].ExchangeRate,
                            EntityName = entityCollection[i].EntityName,
                        },
                        data = _entityName,
                        children = subLoadEntityTree(entityCollection[i].SubEntities, wc, entityCollection[i].EntityType)
                        #endregion
                    };
                }
                _jetc.Add(_entityParent);
            }
            //JsonConvert.SerializeObject(this, Formatting.Indented);
            return JsonConvert.SerializeObject(_jetc);
            //JsonEntityTreeString = "[{\"attr\":{\"id\":\"Lesile_PnL\",\"href\":\"#\",\"class\":\"mainEntity\"},\"data\":\"Lesile(P&L)\",\"children\":[{\"attr\":{\"id\":\"Kevin\",\"class\":\"LastNodeEntity\"},\"data\":\"PropertiesSetting\"}]},{\"attr\":{\"id\":\"178NiuNiu_Cash\",\"href\":\"#\",\"class\":\"mainEntity\"},\"data\":\"178NiuNiu(Cash)\",\"children\":[{\"attr\":{\"id\":\"178NiuNiuA\",\"class\":\"NodeEntity\"},\"data\":\"178NiuNiuA\",\"children\":[{\"attr\":{\"id\":\"8ga12\",\"class\":\"LastNodeEntity\"},\"data\":\"8ga12\"},{\"attr\":{\"id\":\"ah23\",\"class\":\"account\"},\"data\":\"ah23\"}]},{\"attr\":{\"id\":\"178NiuiuB\",\"class\":\"LastNodeEntity\"},\"data\":\"178NiuNiuB\"}]}]";
        }

        public static string LoadPnLEntityTree(EntityCollection entityCollection, WeeklySummary[] wc)
        {
            var _esc = new EntityServiceClient();
            //JsonEntityTreeString = _loadEntityTest.SerializeToJson();
            var _jetc = new JsonEntitiesTreeCollection();
            for (int i = entityCollection.Count - 1; i >= 0; i--)
            {
                var _classType = entityCollection[i].ParentID == 0
                                     ? entityCollection[i].EntityType.Equals(EntityType.Cash)
                                           ? "CashMainEntity"
                                           : "MainEntity"
                                     : entityCollection[i].IsLastLevel == 1
                                           ? entityCollection[i].IsAccount == 1 ? "Account" : "LastNodeEntity"
                                           : "NodeEntity";
                CashEntity _cashEntity;
                if (entityCollection[i].EntityType.Equals(EntityType.Cash) && entityCollection[i].ParentID == 0)
                {
                    _cashEntity = _esc.LoadCashEntity2(entityCollection[i].EntityID)[0];
                }
                else
                {
                    _cashEntity = null;
                }
                string _entityType = string.Format("{0}", entityCollection[i].EntityType);
                string _entityName = string.Format("{0} ({1})", entityCollection[i].EntityName, _entityType.Replace("PAndL", "P&L"));
                JsonEntitiesTree _entityParent;
                if (_cashEntity != null)
                {
                    _entityParent = new JsonEntitiesTree
                    {
                        #region "Cash Main Entity"
                        attr = new JsonEntitiesAttr
                        {
                            id = entityCollection[i].EntityID,
                            Islastlevel = 0,
                            IsAccount = 0,
                            ParentID = entityCollection[i].ParentID,
                            @class = _classType,
                            Enable = entityCollection[i].Enable,
                            EntityType = _entityType,
                            SumType = entityCollection[i].SumType,
                            Currency = entityCollection[i].Currency.CurrencyID,
                            ER = entityCollection[i].ExchangeRate,
                            EntityName = entityCollection[i].EntityName,
                            ContractNumber = _cashEntity.ContractNumber,
                            TallyName = _cashEntity.TallyName,
                            TallyNumber = _cashEntity.TallyNumber,
                            SettlementName = _cashEntity.SettlementName,
                            SettlementNumber = _cashEntity.SettlementNumber,
                            RecommendedBy = _cashEntity.RecommendedBy,
                            Skype = _cashEntity.Skype,
                            QQ = _cashEntity.QQ,
                            Email = _cashEntity.Email,
                            CreditLimit = _cashEntity.CreditLimit,
                        },
                        data = _entityName,
                        children = subLoadEntityTree(entityCollection[i].SubEntities, wc, entityCollection[i].EntityType)
                        #endregion
                    };
                }
                else
                {
                    _entityParent = new JsonEntitiesTree
                    {
                        #region "Main Entity"
                        attr = new JsonEntitiesAttr
                        {
                            id = entityCollection[i].EntityID,
                            Islastlevel = 0,
                            IsAccount = 0,
                            ParentID = entityCollection[i].ParentID,
                            @class = _classType,
                            Enable = entityCollection[i].Enable,
                            EntityType = _entityType,
                            SumType = entityCollection[i].SumType,
                            Currency = entityCollection[i].Currency.CurrencyID,
                            ER = entityCollection[i].ExchangeRate,
                            EntityName = entityCollection[i].EntityName,
                        },
                        data = _entityName,
                        children = subLoadEntityTree(entityCollection[i].SubEntities, wc, entityCollection[i].EntityType)
                        #endregion
                    };
                }
                _jetc.Add(_entityParent);
            }
            //JsonConvert.SerializeObject(this, Formatting.Indented);
            return JsonConvert.SerializeObject(_jetc);
            //JsonEntityTreeString = "[{\"attr\":{\"id\":\"Lesile_PnL\",\"href\":\"#\",\"class\":\"mainEntity\"},\"data\":\"Lesile(P&L)\",\"children\":[{\"attr\":{\"id\":\"Kevin\",\"class\":\"LastNodeEntity\"},\"data\":\"PropertiesSetting\"}]},{\"attr\":{\"id\":\"178NiuNiu_Cash\",\"href\":\"#\",\"class\":\"mainEntity\"},\"data\":\"178NiuNiu(Cash)\",\"children\":[{\"attr\":{\"id\":\"178NiuNiuA\",\"class\":\"NodeEntity\"},\"data\":\"178NiuNiuA\",\"children\":[{\"attr\":{\"id\":\"8ga12\",\"class\":\"LastNodeEntity\"},\"data\":\"8ga12\"},{\"attr\":{\"id\":\"ah23\",\"class\":\"account\"},\"data\":\"ah23\"}]},{\"attr\":{\"id\":\"178NiuiuB\",\"class\":\"LastNodeEntity\"},\"data\":\"178NiuNiuB\"}]}]";
        }

        private static JsonEntitiesTreeCollection subLoadEntityTree(EntityCollection SubEntities)
        {
            if (SubEntities == null)
                return null;
            var _jetc = new JsonEntitiesTreeCollection();

            for (int i = SubEntities.Count - 1; i >= 0; i--)
            {
                Account _account;
                if (SubEntities[i].IsAccount == 1)
                {
                    var _esc = new EntityServiceClient();
                    _account = _esc.LoadAccount2(SubEntities[i].EntityID)[0];
                }
                else
                {
                    _account = null;
                }
                var _classType = SubEntities[i].ParentID == 0
                                     ? SubEntities[i].EntityType.Equals(EntityType.Cash)
                                           ? "CashMainEntity"
                                           : "MainEntity"
                                     : SubEntities[i].IsLastLevel == 1
                                           ? SubEntities[i].IsAccount == 1 ? "Account" : "LastNodeEntity"
                                           : "NodeEntity";
                //var _entityType = SubEntities[i].EntityType == EntityType.PAndL
                //                      ? "PnL"
                //                      : SubEntities[i].EntityType == EntityType.Cash ? "Cash" : SubEntities[i].EntityType == EntityType.Expence? "Exp":"BadDebt";
                string _entityType = string.Format("{0}", SubEntities[i].EntityType);
                string _entityName = string.Format("{0} ({1})", SubEntities[i].EntityName, SubEntities[i].SumType == SumType.Not ? SubEntities[i].IsAccount == 1 ? "Account" : "Node" : SubEntities[i].SumType.ToString());
                if (_account != null)
                {
                    var _entityParent = new JsonEntitiesTree
                    {
                        #region "Account"
                        attr = new JsonEntitiesAttr
                        {
                            id = SubEntities[i].EntityID,
                            Islastlevel = 1,
                            IsAccount = 1,
                            ParentID = SubEntities[i].ParentID,
                            @class = _classType,
                            Enable = SubEntities[i].Enable,
                            EntityType = _entityType,
                            EntityName = SubEntities[i].EntityName,
                            SumType = SubEntities[i].SumType,
                            Currency = SubEntities[i].Currency.CurrencyID,
                            ER = SubEntities[i].ExchangeRate,
                            Company = _account.Company,
                            AccountID = _account.ID,
                            AccountName = _account.AccountName,
                            AccountType = _account.AccountType,
                            Password = _account.Password,
                            BettingLimit = _account.BettingLimit,
                            Status = _account.Status,
                            DateOpen = _account.DateOpen,
                            Personnel = _account.Personnel,
                            IP=_account.IP,
                            Odds=_account.Odds,
                            IssuesConditions=_account.IssuesConditions,
                            Remarks=_account.RemarksAcc,
                            Factor=_account.Factor,
                            PerBet = _account.Perbet
                        },
                        data = _entityName,
                        children = subLoadEntityTree(SubEntities[i].SubEntities)
                        #endregion
                    };
                    _jetc.Add(_entityParent);
                }
                else
                {
                    var _entityParent = new JsonEntitiesTree
                    {
                        #region "Node Entity"
                        attr = new JsonEntitiesAttr
                        {
                            id = SubEntities[i].EntityID,
                            IsAccount = SubEntities[i].IsAccount,
                            Islastlevel = SubEntities[i].IsLastLevel,
                            ParentID = SubEntities[i].ParentID,
                            @class = _classType,
                            Enable = SubEntities[i].Enable,
                            EntityType = _entityType,
                            EntityName = SubEntities[i].EntityName,
                            SumType = SubEntities[i].SumType,
                            Currency = SubEntities[i].Currency.CurrencyID,
                            ER = SubEntities[i].ExchangeRate,
                        },
                        data = _entityName,
                        children = subLoadEntityTree(SubEntities[i].SubEntities)
                        #endregion
                    };
                    _jetc.Add(_entityParent);
                }
            }
            return _jetc;
        }

        private static JsonEntitiesTreeCollection subLoadRelationEntityTree(EntityCollection SubEntities)
        {
            if (SubEntities == null)
                return null;
            var _jetc = new JsonEntitiesTreeCollection();

            for (int i = SubEntities.Count - 1; i >= 0; i--)
            {
                Account _account;
                if (SubEntities[i].IsAccount == 1)
                {
                    var _esc = new EntityServiceClient();
                    _account = _esc.LoadAccount2(SubEntities[i].EntityID)[0];
                }
                else
                {
                    _account = null;
                }
                var _classType = SubEntities[i].ParentID == 0
                                     ? SubEntities[i].EntityType.Equals(EntityType.Cash)
                                           ? "CashMainEntity"
                                           : "MainEntity"
                                     : SubEntities[i].IsLastLevel == 1
                                           ? SubEntities[i].IsAccount == 1 ? "Account" : "LastNodeEntity"
                                           : "NodeEntity";
                //var _entityType = SubEntities[i].EntityType == EntityType.PAndL
                //                      ? "PnL"
                //                      : SubEntities[i].EntityType == EntityType.Cash ? "Cash" : SubEntities[i].EntityType == EntityType.Expence? "Exp":"BadDebt";
                string _entityType = string.Format("{0}", SubEntities[i].EntityType);
                string _entityName = string.Format("{0} ({1})", SubEntities[i].EntityName, SubEntities[i].SumType == SumType.Not ? SubEntities[i].IsAccount == 1 ? "Account" : "Node" : SubEntities[i].SumType.ToString());
                var _entityParent = new JsonEntitiesTree
                                        {
                                            #region "Node Entity"
                                            attr = new JsonEntitiesAttr
                                                       {
                                                           id = SubEntities[i].EntityID,
                                                           Islastlevel = SubEntities[i].IsLastLevel,
                                                           IsAccount = SubEntities[i].IsAccount,
                                                           ParentID = SubEntities[i].ParentID,
                                                           @class = _classType,
                                                           Enable = SubEntities[i].Enable,
                                                           EntityType = _entityType,
                                                           EntityName = SubEntities[i].EntityName,
                                                           SumType = SubEntities[i].SumType,
                                                           Currency = SubEntities[i].Currency.CurrencyID,
                                                           ER = SubEntities[i].ExchangeRate,
                                                       },
                                            data = _entityName
                                            #endregion
                                        };
                _jetc.Add(_entityParent);
            }
            return _jetc;
        }

        private static JsonEntitiesTreeCollection subLoadEntityTree(EntityCollection SubEntities,WeeklySummary[] _ws,EntityType entityType)
        {
            if (SubEntities == null)
                return null;
            var _jetc = new JsonEntitiesTreeCollection();

            for (int i = SubEntities.Count - 1; i >= 0; i--)
            {
                Account _account;
                if (SubEntities[i].IsAccount == 1)
                {
                    var _esc = new EntityServiceClient();
                    _account = _esc.LoadAccount2(SubEntities[i].EntityID)[0];
                }
                else
                {
                    _account = null;
                }
                var _classType = SubEntities[i].ParentID == 0
                                     ? SubEntities[i].EntityType.Equals(EntityType.Cash)
                                           ? "CashMainEntity"
                                           : "MainEntity"
                                     : SubEntities[i].IsLastLevel == 1
                                           ? SubEntities[i].IsAccount == 1 ? "Account" : "LastNodeEntity"
                                           : "NodeEntity";
                //var _entityType = SubEntities[i].EntityType == EntityType.PAndL
                //                      ? "PnL"
                //                      : SubEntities[i].EntityType == EntityType.Cash ? "Cash" : SubEntities[i].EntityType == EntityType.Expence? "Exp":"BadDebt";
                string _entityType = string.Format("{0}", SubEntities[i].EntityType);
                string _entityName = string.Format("{0} ({1})", SubEntities[i].EntityName, SubEntities[i].SumType == SumType.Not ? SubEntities[i].IsAccount == 1 ? "Account" : "Node" : SubEntities[i].SumType.ToString());
                var _wsToEntity = _ws.First(x => x.Entity.EntityID == SubEntities[i].EntityID);
                decimal _preBalance = _wsToEntity.BasePrevBalance;
                decimal _winAndLoss = _wsToEntity.BaseWinAndLoss;
                decimal _BaseTransfer = _wsToEntity.BaseTransfer;
                decimal _transaction = _wsToEntity.BaseTransaction;
                decimal _balance = _wsToEntity.BaseBalance;
                string _preBalanceInput = string.Format("<input type='text' id='preBalance{0}' disabled='disabled' value='{1}' />", SubEntities[i].EntityID, _preBalance.ToString());
                string _winAndLossInput = string.Format("<input type='text' id='winAndLoss{0}' disabled='disabled' value='{1}' />", SubEntities[i].EntityID, _winAndLoss.ToString());
                string _BaseTransferInput = string.Format("<input type='text' id='BaseTransfer{0}' disabled='disabled' value='{1}' />", SubEntities[i].EntityID, _BaseTransfer.ToString());
                string _transactionInput = string.Format("<input type='text' id='transaction{0}' disabled='disabled' value='{1}' />", SubEntities[i].EntityID, _transaction.ToString());
                string _balanceInput = string.Format("<input type='text' id='balance{0}' disabled='disabled' value='{1}' />", SubEntities[i].EntityID, _balance.ToString());

                string _cbConfirm="";
                if (_wsToEntity.Status == 0)
                    _cbConfirm = string.Format("<input type='checkbox' id='cbConfirm{0}' name='confirm' onclick='confirmCheck(this.id)' title='Confirm' />", SubEntities[i].EntityID);
                else
                {
                    _cbConfirm =
                        string.Format(
                            "<input type='checkbox' id='cbConfirm{0}' name='confirm' onclick='confirmCheck(this.id)' title='Confirm' disabled checked />",
                            SubEntities[i].EntityID);
                }
                string _dataStr;
                if (SubEntities[i].SumType == SumType.Transaction)
                    _dataStr = string.Format("<table id='tbTransaction'><tr style='height:20px'><td></td><td>PreBalance</td><td>WinAndLoss</td><td>Transaction</td><td>Balance</td><td></td></tr><tr style='height:20px'><td style='border:0px'>{0} </td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td style='border:0px;width:50px;'>{5}</td></tr></table>"
                        , _entityName, _preBalanceInput, _winAndLossInput, _transactionInput, _balanceInput, _cbConfirm);
                //_dataStr = string.Format("{0} PreBalance:{1}, WinAndLoss:{2}, Transaction:{3}, Balance{4}  {5}",
                //                       _entityName, _preBalanceInput, _winAndLossInput, _transactionInput, _balanceInput,
                //                      _btnConfirm);
                else if (SubEntities[i].SumType == SumType.Subtotal)
                {
                    _dataStr = string.Format("<table id='tbSubtotal'><tr style='height:20px'><td></td><td>WinAndLoss</td><td>Transfer</td><td>Balance</td><td></td></tr><tr style='height:20px'><td style='border:0px'>{0} </td><td>{1}</td><td>{2}</td><td>{3}</td><td style='border:0px;width:50px;'>{4}</td></tr></table>"
                        , _entityName, _winAndLossInput, _BaseTransferInput, _balanceInput, _cbConfirm);
                }
                else
                {
                    
                    _dataStr = string.Format("{0}<table class='tbAcc'><tr><td style='width:300px'>{1}</td><td>{2}</td><td>{3}</td></tr></table>",
                        "", _entityName, _winAndLossInput, _cbConfirm);
                }
                if (_account != null)
                {
                    
                    var _entityParent = new JsonEntitiesTree
                    {
                        #region "Account"
                        attr = new JsonEntitiesAttr
                        {
                            id = SubEntities[i].EntityID,
                            IsAccount = SubEntities[i].IsAccount,
                            Islastlevel = SubEntities[i].IsLastLevel,
                            ParentID = SubEntities[i].ParentID,
                            @class = _classType,
                            Enable = SubEntities[i].Enable,
                            EntityType = _entityType,
                            EntityName = SubEntities[i].EntityName,
                            SumType = SubEntities[i].SumType,
                            Currency = SubEntities[i].Currency.CurrencyID,
                            ER = SubEntities[i].ExchangeRate,
                            Company = _account.Company,
                            AccountID = _account.ID,
                            AccountName = _account.AccountName,
                            AccountType = _account.AccountType,
                            Password = _account.Password,
                            BettingLimit = _account.BettingLimit,
                            Status = _account.Status,
                            PreBalance = _preBalance,
                            WinAndLoss = _winAndLoss,
                            Transaction = _transaction,
                            Balance = _balance,
                            DateOpen = _account.DateOpen,
                            Personnel = _account.Personnel,
                            IP = _account.IP,
                            Odds = _account.Odds,
                            IssuesConditions = _account.IssuesConditions,
                            Remarks = _account.RemarksAcc,
                            Factor = _account.Factor,
                            PerBet = _account.Perbet
                        },
                        data = _dataStr,
                        children = subLoadEntityTree(SubEntities[i].SubEntities,_ws,SubEntities[i].EntityType)
                        #endregion
                    };
                    _jetc.Add(_entityParent);
                }
                else
                {
                    var _entityParent = new JsonEntitiesTree
                    {
                        #region "Node Entity"
                        attr = new JsonEntitiesAttr
                        {
                            id = SubEntities[i].EntityID,
                            IsAccount = SubEntities[i].IsAccount,
                            Islastlevel = SubEntities[i].IsLastLevel,
                            ParentID = SubEntities[i].ParentID,
                            @class = _classType,
                            Enable = SubEntities[i].Enable,
                            EntityType = _entityType,
                            EntityName = SubEntities[i].EntityName,
                            SumType = SubEntities[i].SumType,
                            Currency = SubEntities[i].Currency.CurrencyID,
                            ER = SubEntities[i].ExchangeRate,
                            PreBalance = _wsToEntity.BasePrevBalance,
                            WinAndLoss = _wsToEntity.BaseWinAndLoss,
                            Transaction = _wsToEntity.BaseTransaction,
                            Balance = _wsToEntity.BaseBalance
                        },                        
                        data = _dataStr,
                        children = subLoadEntityTree(SubEntities[i].SubEntities, _ws,SubEntities[i].EntityType)
                        #endregion
                    };
                    if (SubEntities[i].IsLastLevel == 1 && SubEntities[i].IsAccount != 1 && SubEntities[i].EntityType == EntityType.PAndL)
                        _entityParent.attr.onclick =string.Format("openCashTree({0})",SubEntities[i].EntityID);
                    _jetc.Add(_entityParent);
                }
            }
            return _jetc;
        }

        
    }

    public class JsonEntitiesTree
    {
        public JsonEntitiesAttr attr;
        public string data;
        public JsonEntitiesTreeCollection children;
    }

    public class JsonEntitiesAttr
    {
        public int id { set; get; }
        public int ParentID { set; get; }//required first level 0
        public string @class { set; get; }
        public int Enable { set; get; }//required 0 or 1
        public string EntityName { set; get; }//required
        public string EntityType { set; get; }//required query the enum
        public string Currency { set; get; }
        public decimal ER { set; get; } //Exchange Rate
        public SumType SumType { get; set; }//required query the enum
        public decimal PreBalance = -1;
        public decimal WinAndLoss = -1;
        public decimal Transaction = -1;
        public decimal Balance = -1;
        public string onclick { get; set; }
        public int Islastlevel { set; get; }
        public int IsAccount { set; get; }
        #region "Account attributes"
        public int AccountID { set; get; }
        public int Company { set; get; }
        public string AccountName { set; get; }
        public string Password { set; get; }
        public AccountType AccountType { set; get; }
        public decimal BettingLimit { set; get; }
        public Status Status { set; get; }
        public string DateOpen { set; get; }
        public string Personnel { set; get; }
        public string IP { set; get; }
        public string Odds { set; get; }
        public string IssuesConditions { set; get; }
        public string Remarks { set; get; }
        public string Factor { set; get; }
        public decimal PerBet { set; get; }
        #endregion
        #region "Cash attributes"
        public string ContractNumber { set; get; }
        public string TallyName { set; get; }
        public string TallyNumber { set; get; }
        public string SettlementName { set; get; }
        public string SettlementNumber { set; get; }
        public string RecommendedBy { set; get; }
        public string Skype { set; get; }
        public string QQ { set; get; }
        public string Email { set; get; }
        public decimal CreditLimit { set; get; }
        #endregion
        public JsonEntitiesTreeCollection children;
        public JsonEntitiesAttr()
        {
            children=new JsonEntitiesTreeCollection();
        }
    }
    public class JsonEntitiesTreeCollection : List<JsonEntitiesTree>
    {
        public JsonEntitiesTreeCollection()
        {
        }

        public JsonEntitiesTreeCollection(IEnumerable<JsonEntitiesTree> collection)
            : base(collection)
        {
        }

        public JsonEntitiesTreeCollection(int capacity)
            : base(capacity)
        {
        }
    }
}