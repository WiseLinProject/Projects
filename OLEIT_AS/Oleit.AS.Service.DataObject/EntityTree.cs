using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Oleit.AS.Service.DataObject
{
    public class EntityTree
    {
        public static string LoadPnLEntityTree(EntityCollection entityCollection)
        {
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
                var _entityType = entityCollection[i].EntityType == EntityType.PAndL
                                      ? "PnL"
                                      : entityCollection[i].EntityType == EntityType.Cash
                                            ? "Cash"
                                            : entityCollection[i].EntityType == EntityType.Expence ? "Exp" : "BadDebt";
                string _entityName;
                if(entityCollection[i].SumType==SumType.Not)
                    _entityName = string.Format("{0} ({1})", entityCollection[i].EntityName, entityCollection[i].SumType);
                else
                    _entityName = string.Format("{0} ({1})", entityCollection[i].EntityName, entityCollection[i].EntityType);
                JsonEntitiesTree _entityParent;

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
                    children = subLoadEntityTree(entityCollection[i].SubEntities)
                    #endregion
                };

                _jetc.Add(_entityParent);
            }
            return JsonConvert.SerializeObject(_jetc);
        }

        private static JsonEntitiesTreeCollection subLoadEntityTree(EntityCollection SubEntities)
        {
            if (SubEntities == null)
                return null;
            var _jetc = new JsonEntitiesTreeCollection();

            for (int i = SubEntities.Count - 1; i >= 0; i--)
            {
                var _classType = SubEntities[i].ParentID == 0
                                     ? SubEntities[i].EntityType.Equals(EntityType.Cash)
                                           ? "CashMainEntity"
                                           : "MainEntity"
                                     : SubEntities[i].IsLastLevel == 1
                                           ? SubEntities[i].IsAccount == 1 ? "Account" : "LastNodeEntity"
                                           : "NodeEntity";
                string _entityType = string.Format("{0}", SubEntities[i].EntityType);
                string _entityName = string.Format("{0} ({1})", SubEntities[i].EntityName, SubEntities[i].SumType == SumType.Not ? SubEntities[i].IsAccount == 1 ? "Account" : "Node" : SubEntities[i].SumType.ToString());
                if (SubEntities[i].IsAccount ==1)
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
                            //Company = _account.Company,
                            //AccountID = _account.ID,
                            //AccountName = _account.AccountName,
                            //AccountType = _account.AccountType,
                            //Password = _account.Password,
                            //BettingLimit = _account.BettingLimit,
                            //Status = _account.Status,
                            //DateOpen = _account.DateOpen,
                            //Personnel = _account.Personnel,
                            //IP = _account.IP,
                            //Odds = _account.Odds,
                            //IssuesConditions = _account.IssuesConditions,
                            //Remarks = _account.RemarksAcc,
                            //Factor = _account.Factor,
                            //PerBet = _account.Perbet
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
