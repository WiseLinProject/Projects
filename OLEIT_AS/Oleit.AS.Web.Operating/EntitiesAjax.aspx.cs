using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Accounting_System.EntityServiceReference;
using Oleit.AS.Service.DataObject;
using System.Text;
using Newtonsoft.Json;
using Accounting_System.EndPeriodServiceReference;

namespace Accounting_System
{
    public partial class EntitiesAjax : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] != null)
            {
                int _entityID;
                int.TryParse(Request["entityID"], out _entityID);

                string _type = Request["type"];
                //_entityID = 185;
                if (_type.Equals("load", StringComparison.OrdinalIgnoreCase))
                    genEntities(_entityID);
                else if (_type.Equals("er", StringComparison.OrdinalIgnoreCase))
                {
                    string _currency = Request["currency"];
                    decimal _er;
                    decimal.TryParse(Request["er"].ToString(), out _er);
                    setER(_entityID, _currency, _er);
                }
                else if (_type.Equals("insert", StringComparison.OrdinalIgnoreCase))
                {
                    string _jsonEntityString = Request["json"];
                    insertNodeEntity(_jsonEntityString);

                }
                else if (_type.Equals("remove", StringComparison.OrdinalIgnoreCase))
                {
                    removeEntity(_entityID);
                }
                else if (_type.Equals("update", StringComparison.OrdinalIgnoreCase))
                {
                    string _jsonEntityString = Request["json"];
                    updateEntity(_jsonEntityString);
                }
                else if (_type.Equals("search", StringComparison.OrdinalIgnoreCase))
                {
                    int _entityType = int.Parse(Request["entityType"]);
                    string _entityName = Request["entityName"];
                    searchRelationEntities(_entityName, _entityType);
                }
                else if (_type.Equals("baddebt", StringComparison.OrdinalIgnoreCase))
                {
                    setBadDebt(_entityID);
                }
                else if (_type.Equals("loadSubEntities", StringComparison.OrdinalIgnoreCase))
                    loadSubEntities(_entityID);
            }
            else
            {
                Response.Write("Session has expired.");
            }
        }

        private void setBadDebt(int entityID)
        {
            var _esr = new EntityServiceClient();
            try
            {
                _esr.ChangeEntityType(entityID, EntityType.BadDebt);
                Response.Write("Success!");
            }
            catch
            {
                Response.Write("Fail!");
            }
        }

        private void searchRelationEntities(string entityName, int entityType)
        {
            string _jsonRelationTree =  JsonEntityFunc.LoadRelationEntityTree(entityName,entityType);
            Response.Write(_jsonRelationTree);
        }

        private void updateEntity(string jsonEntityString)
        {
            JsonEntities _jsonEntity = JsonConvert.DeserializeObject<JsonEntities>(jsonEntityString);
            #region "Entity"
            Entity _entity = new Entity
            {
                EntityID = _jsonEntity.EntityID,
                EntityName = _jsonEntity.EntityName,
                ParentID = _jsonEntity.ParentID,
                EntityType = EntitiesFunc.entityTypeFunc(_jsonEntity.EntityType),
                IsAccount = _jsonEntity.IsAccount ? 1 : 0,
                IsLastLevel = _jsonEntity.IsLastLevel ? 1 : 0,
                SumType = EntitiesFunc.sumTypeFunc(_jsonEntity.SumType),
                Currency = new Currency { CurrencyID = _jsonEntity.Currency },
                ExchangeRate = _jsonEntity.ER
            };
            #endregion
            try
            {
                if (!_jsonEntity.IsAccount)
                {//no account                
                    EntitiesFunc.entityUpdate(_entity);
                }
                else
                {//has account
                    #region "Account"
                    Account _account = new Account
                    {
                        AccountName = _jsonEntity.Account.AccountName,
                        Company = _jsonEntity.Account.Company,
                        Password = _jsonEntity.Account.Password,
                        AccountType = EntitiesFunc.accoutnTypeFunc(_jsonEntity.Account.AccountType),
                        BettingLimit = _jsonEntity.Account.BettingLimit,
                        DateOpen = _jsonEntity.Account.DateOpen,
                        Factor = _jsonEntity.Account.Factor,
                        Personnel = _jsonEntity.Account.Personnel,
                        IP = _jsonEntity.Account.IP,
                        Odds = _jsonEntity.Account.Odds,
                        IssuesConditions = _jsonEntity.Account.IssuesConditions,
                        RemarksAcc = _jsonEntity.Account.RemarksAcc,
                        Perbet = _jsonEntity.Account.Perbet,
                        EntityID = _jsonEntity.EntityID
                    };
                    #endregion
                    EntitiesFunc.entityUpdate(_entity, _account);
                }
                Response.Write("Success!");
            }
            catch
            {
                Response.Write("Fail!");
            }
        }

        private void removeEntity(int EntityID)
        {
            var _esr = new EntityServiceClient();
            try
            {
                _esr.Disable(EntityID);
                Response.Write("Success!");
            }
            catch
            {
                Response.Write("Fail!");
            }
        }

        private void insertNodeEntity(string jsonEntityString)
        {
            
            JsonEntities _jsonEntity = JsonConvert.DeserializeObject<JsonEntities>(jsonEntityString);
            if (checkSameTransaction(_jsonEntity.ParentID,_jsonEntity.EntityName) && (_jsonEntity.SumType==1 || _jsonEntity.SumType==2))
            {
                Response.Write(string.Format("There are two {0} {1} Entities!", _jsonEntity.EntityName,EntitiesFunc.sumTypeFunc(_jsonEntity.SumType)));
            }
            else
            {
                var _epsr = new EndPeriodServiceClient();
                var _esr = new EntityServiceClient();
                decimal _exchangeRate;
                Currency _currency;
                if(_jsonEntity.SumType==1)
                {
                    _currency = new Currency{CurrencyID =_jsonEntity.EntityName};
                    _exchangeRate = _epsr.GetEndPeriodRate(new Currency { CurrencyID = _jsonEntity.EntityName })[0].ExchangeRate;

                }
                else if (_jsonEntity.SumType == 2)
                {
                    _currency = _esr.LoadEntity2(_jsonEntity.ParentID)[0].Currency;
                    _exchangeRate = decimal.Parse(_jsonEntity.EntityName);
                }
                else if (_jsonEntity.ParentID == 0)
                {
                    _currency = null;
                    _exchangeRate = 1;
                }
                else
                {
                    var _parentSubTotalEntity= _esr.QueryParentSubTotalEntity(_jsonEntity.ParentID)[0];
                    _exchangeRate = _parentSubTotalEntity.ExchangeRate;
                    _currency = _parentSubTotalEntity.Currency;
                }
                
                #region "Entity"
                Entity _entity = new Entity
                {
                    EntityName = _jsonEntity.EntityName,
                    EntityType = EntitiesFunc.entityTypeFunc(_jsonEntity.EntityType),
                    IsAccount = _jsonEntity.IsAccount ? 1 : 0,
                    IsLastLevel = _jsonEntity.IsLastLevel ? 1 : 0,
                    ParentID = _jsonEntity.ParentID,
                    ExchangeRate = _exchangeRate,
                    Currency = _currency,
                    SumType = EntitiesFunc.sumTypeFunc(_jsonEntity.SumType)
                };
                #endregion
                try
                {
                    if (!_jsonEntity.IsAccount)
                    {//no account                
                        EntitiesFunc.entityInsert(_entity);
                    }
                    else
                    {//has account
                        #region "Account"
                        Account _account = new Account
                        {
                            AccountName = _jsonEntity.Account.AccountName,
                            Company = _jsonEntity.Account.Company,
                            Password = _jsonEntity.Account.Password,
                            AccountType = EntitiesFunc.accoutnTypeFunc(_jsonEntity.Account.AccountType),
                            BettingLimit = _jsonEntity.Account.BettingLimit,
                            DateOpen = _jsonEntity.Account.DateOpen,
                            Factor = _jsonEntity.Account.Factor,
                            Personnel = _jsonEntity.Account.Personnel,
                            IP = _jsonEntity.Account.IP,
                            Odds = _jsonEntity.Account.Odds,
                            IssuesConditions = _jsonEntity.Account.IssuesConditions,
                            RemarksAcc = _jsonEntity.Account.RemarksAcc,
                            Perbet = _jsonEntity.Account.Perbet
                        };
                        #endregion
                        EntitiesFunc.entityInsert(_entity, _account);
                    }
                    Response.Write("Success!");
                }
                catch
                {
                    Response.Write("Fail!");
                }
            }
        }

        private bool checkSameTransaction(int entityID,string entityName)
        {
            var esc = new EntityServiceClient();
            EntityCollection _ec = new EntityCollection();
            EntityCollection _ecs = new EntityCollection(esc.LoadEntity2(entityID));
            var _newTallyCollection = EntitiesFunc.entityCollectioin(_ec, _ecs);
            if (_newTallyCollection.Where(x => (x.SumType == SumType.Transaction && x.EntityName == entityName) || (x.SumType == SumType.Subtotal && x.EntityName == entityName)).Count() > 0)
                return true;
            else
                return false;
        }

        private void genEntities(int entityID)
        {
            var _esr = new EntityServiceClient();
            var _entities = _esr.LoadSingleEntityList(entityID);
            var _transactionEntities = _entities.Where(x => x.SumType == SumType.Transaction);
            var _subTotalEntities = _entities.Where(x => x.SumType == SumType.Subtotal);
            var _nodeEntities = _entities.Where(x => x.SumType == SumType.Not && x.ParentID != 0 && x.IsAccount!=1);
            var _node1 = from e in _nodeEntities
                         where _transactionEntities.Select(x => x.EntityID).Contains(e.ParentID)
                         select e;
            var _node2 = from e in _nodeEntities
                         where _subTotalEntities.Select(x => x.EntityID).Contains(e.ParentID) 
                         select e;
            var _node3 = from e in _nodeEntities
                         where !_transactionEntities.Select(x => x.EntityID).Contains(e.ParentID) && !_subTotalEntities.Select(x => x.EntityID).Contains(e.ParentID)
                         select e;
            var _account = _entities.Where(x =>x.IsAccount==1);
            StringBuilder _sbResponse = new StringBuilder();
            nodeDiv(_transactionEntities.ToArray(),"transaction");
            _sbResponse.AppendFormat("{0},{1},{2},{3},{4},{5}", nodeDiv(_transactionEntities.ToArray(), "transaction"), nodeDiv(_node1.ToArray(), "nodeone"), nodeDiv(_subTotalEntities.ToArray(), "subtotal"), nodeDiv(_node2.ToArray(), "nodetwo"), accountDiv(_account.ToArray()), nodeDiv(_node3.ToArray(), "NotSet"));
            Response.Write(_sbResponse.ToString());
        }

        private void loadSubEntities(int entityID)
        {
            var _esr = new EntityServiceClient();
            try
            {
                Response.Write(JsonEntityFunc.LoadSubEntitiesTree(entityID));
            }
            catch
            {
                Response.Write("Fail!");
            }
        }

        private void setER(int entityID, string currency, decimal ER)
        {
            var _esc = new EntityServiceClient();
            try
            {
                _esc.SetCurrencyAndRate(entityID, currency, ER);
                Response.Write("Set Exchange Rate success!");
            }
            catch
            {
                Response.Write("Faled!");
            }            
        }

        private string nodeDiv(Entity[] nodeEntities, string type)
        {
            StringBuilder _sbNode = new StringBuilder();
            if (nodeEntities.Count() > 0)
            {
                _sbNode.Append("<ul>");
                for (int i = 0; i < nodeEntities.Count(); i++)
                {
                    var t = nodeEntities[i];
                    if (i != nodeEntities.Count()-1)
                        _sbNode.AppendFormat("<li id='{0}{1}'><a onclick='clickSubEntities(this.id)' id='{1}' entitytype='{2}' er='{3}' currency='{4}' parentid='{5}' entityname='{6}' sumtype='{7}' islastlevel='{8}'><span>{6}</span></a>", type, t.EntityID, t.EntityType, t.ExchangeRate, t.Currency.CurrencyID, t.ParentID, t.EntityName, t.SumType, t.IsLastLevel);
                    else
                        _sbNode.AppendFormat("<li id='{0}{1}' class='last'><a onclick='clickSubEntities(this.id)'  id='{1}' entitytype='{2}' er='{3}' currency='{4}' parentid='{5}' entityname='{6}' sumtype='{7}' islastlevel='{8}'><span>{6}</span></a>", type, t.EntityID, t.EntityType, t.ExchangeRate, t.Currency.CurrencyID, t.ParentID, t.EntityName, t.SumType, t.IsLastLevel);
                }
                _sbNode.Append("</ul>");
            }
            return _sbNode.ToString();
        }

        private string accountDiv(Entity[] account)
        {
            StringBuilder _sbAccount = new StringBuilder();
            if (account.Count() > 0)
            {
                _sbAccount.Append("<ul>");
                for (int i = 0; i < account.Count(); i++)
                {
                    var t = account[i];
                    var _esr = new EntityServiceClient();
                    var _account = _esr.LoadAccount2(t.EntityID)[0];
                    if (i != account.Count() - 1)
                        _sbAccount.AppendFormat("<li id='account{0}'><a onclick='clickSubEntities(this.id)' id='{0}' entitytype='{1}' er='{2}' currency='{3}' parentid='{4}' entityname='{5}' company='{6}' accountname='{7}' password='{8}' accounttype='{9}' bettinglimit='{10}' dateopen='{11}' personnel='{12}' ip='{13}' odds='{14}' issuesconditions='{15}' remarks='{16}' factor='{17}' perbet='{18}' status='{19}' accountid='{20}'  islastlevel='{21}'><span>{5}</span></a>",
                            t.EntityID, t.EntityType, t.ExchangeRate, t.Currency.CurrencyID, t.ParentID, t.EntityName, _account.Company, _account.AccountName, _account.Password, _account.AccountType, _account.BettingLimit, _account.DateOpen, _account.Personnel, _account.IP, _account.Odds, _account.IssuesConditions, _account.RemarksAcc, _account.Factor, _account.Perbet, _account.Status, _account.ID, t.IsLastLevel);
                    else
                        _sbAccount.AppendFormat("<li class='last' id='account{0}'><a onclick='clickSubEntities(this.id)' id='{0}' entitytype='{1}' er='{2}' currency='{3}' parentid='{4}' entityname='{5}' company='{6}' accountname='{7}' password='{8}' accounttype='{9}' bettinglimit='{10}' dateopen='{11}' personnel='{12}' ip='{13}' odds='{14}' issuesconditions='{15}' remarks='{16}' factor='{17}' perbet='{18}' status='{19}' accountid='{20}'  islastlevel='{21}'><span>{5}</span></a>",
                            t.EntityID, t.EntityType, t.ExchangeRate, t.Currency.CurrencyID, t.ParentID, t.EntityName, _account.Company, _account.AccountName, _account.Password, _account.AccountType, _account.BettingLimit, _account.DateOpen, _account.Personnel, _account.IP, _account.Odds, _account.IssuesConditions, _account.RemarksAcc, _account.Factor, _account.Perbet, _account.Status, _account.ID, t.IsLastLevel);
                }
                _sbAccount.Append("</ul>");
            }
            return _sbAccount.ToString();
        }

        public class JsonEntities
        {
            public int EntityID { get; set; }
            public int ParentID { get; set; }
            public string EntityName { get; set; }
            public string ParentType { get; set; }
            public string EntityType { get; set; }
            public int SumType { get; set; }
            public bool IsLastLevel { get; set; }
            public bool IsAccount { get; set; }
            public string Currency { get; set; }
            public decimal ER { get; set; }
            public JsonAccount Account { get; set; }
        }

        public class JsonAccount
        {
            public int AccountID { get; set; }
            public string AccountName { get; set; }
            public int Company { get; set; }
            public string Password { get; set; }
            public int AccountType { get; set; }
            public decimal BettingLimit { get; set; }
            public string DateOpen { get; set; }
            public string Factor { set; get; }
            public string Personnel { set; get; }
            public string IP { set; get; }
            public string Odds { set; get; }
            public string IssuesConditions { set; get; }
            public string RemarksAcc { set; get; }
            public decimal Perbet { set; get; }            
        }
    }
}