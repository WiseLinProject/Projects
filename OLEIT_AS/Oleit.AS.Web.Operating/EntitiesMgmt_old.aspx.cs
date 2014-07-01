using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Accounting_System.EntityServiceReference;
using Accounting_System.CurrencyServiceReference;
using Oleit.AS.Service.DataObject;
using Newtonsoft.Json;

namespace Accounting_System
{
    public partial class EntityMgmt : System.Web.UI.Page
    {
        public string JsonEntityTreeString = "";
        public string JsonRelationTreeString = "";
        public int UserId;

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckLimit.CheckPage(Request["menuid"]);
            if (!IsPostBack)
            {
                UserId = int.Parse(Session["UserId"].ToString());
                JsonEntityTreeString = JsonEntityFunc.LoadEntityTree();
                JsonRelationTreeString = JsonEntityFunc.LoadRelationEntityTree();
                loadCurrency();
            }
        }

        private void loadCurrency()
        {
            var _csr = new CurrencyServiceClient();
            var _allCurrency = _csr.AllCurrency().Select(x => x.CurrencyID);
            ddlCurrency.DataSource = _allCurrency.ToList();
            ddlCurrency.DataBind();
        }

        protected void btnMainEntity_Click(object sender, EventArgs e)
        {
            var _entityType = entityTypeFunc(hfMainEntityType.Value);
            entityUpdate(txtMainEntityName.Value, _entityType, 0, 0, 0, SumType.Not);
            Library.Alert("Update success!");
            JsonEntityTreeString = JsonEntityFunc.LoadEntityTree();
            JsonRelationTreeString = JsonEntityFunc.LoadRelationEntityTree();
            // Page.ClientScript.RegisterStartupScript(GetType(), "Add MainEntity success string", "refreshTree();", true);
        }

        private void entityInsert(string _entityName, EntityType _entityType, int _parentId, int _isLastLevel,
                                  int _isAccount, SumType _sumType)
        {
            var _esc = new EntityServiceClient();
            var _newEntity = new Entity
                                 {
                                     EntityName = _entityName,
                                     EntityType = _entityType,
                                     ParentID = _parentId,
                                     IsLastLevel = _isLastLevel,
                                     IsAccount = _isAccount,
                                     Enable = 1,
                                     //Currency = new Currency{CurrencyID = "SGD"},
                                     //ExchangeRate = 0,
                                     SumType = _sumType
                                 };
            if (SelectRole.Value.Equals("Cash", StringComparison.OrdinalIgnoreCase) && _parentId == 0)
            {
                #region "New Cash Entity"
                var _newCashEntity = new CashEntity
                                         {
                                             ContractNumber = txtNewContactNumber.Value,
                                             CreditLimit = decimal.Parse(txtNewCreditLimit.Value),
                                             Email = txtNewEmail.Value,
                                             QQ = txtNewQQ.Value,
                                             RecommendedBy = txtNewRecommendedby.Value,
                                             SettlementName = txtNewSettlementName.Value,
                                             SettlementNumber = txtNewSettlementNumber.Value,
                                             Skype = txtNewSkype.Value,
                                             TallyName = txtNewTallyName.Value,
                                             TallyNumber = txtNewTallyNumber.Value
                                         };
                #endregion
                _esc.NewEntity2(_newEntity, _newCashEntity);
            }
            else if (_isAccount == 1)
            {
                #region "_accountType"

                var _accountType = new AccountType();
                switch (int.Parse(ddlNewAccType.Value))
                {
                    case 1:
                        {
                            _accountType = AccountType.SuperSenior;
                            break;
                        }
                    case 2:
                        {
                            _accountType = AccountType.Senior;
                            break;
                        }
                    case 3:
                        {
                            _accountType = AccountType.Master;
                            break;
                        }
                    case 4:
                        {
                            _accountType = AccountType.Agent;
                            break;
                        }
                    case 5:
                        {
                            _accountType = AccountType.Members;
                            break;
                        }
                }

                #endregion

                #region "Status"

                var _status = new Status();
                switch (int.Parse(ddlNewAccStatus.Value))
                {
                    case 1:
                        {
                            _status = Status.Suspended;
                            break;
                        }
                    case 2:
                        {
                            _status = Status.Closed;
                            break;
                        }
                    case 3:
                        {
                            _status = Status.NoFight;
                            break;
                        }
                    case 4:
                        {
                            _status = Status.FollowBet;
                            break;
                        }
                    case 5:
                        {
                            _status = Status.LousyOdds;
                            break;
                        }
                    case 6:
                        {
                            _status = Status.NeedToOpenBack;
                            break;
                        }
                }

                #endregion

                #region "New Account"
                decimal _bettingLimit;
                if (!decimal.TryParse(txtNewBettingLimit.Value, out _bettingLimit))
                    _bettingLimit = 0;
                decimal _perbet;//txtNewPerBet.Value
                if(!decimal.TryParse(txtNewPerBet.Value, out _perbet))
                    _perbet = 0;
                var _newAccount = new Account
                                      {
                                          Company = int.Parse(ddlNewAccCompany.Value),
                                          AccountName = txtNewAccName.Value,
                                          AccountType = _accountType,
                                          BettingLimit = _bettingLimit,
                                          Password = txtNewAccPwd.Value,
                                          Status = _status,
                                          DateOpen = txtNewDateOpen.Value,
                                          IP = txtNewIP.Value,
                                          Odds = txtNewOdds.Value,
                                          IssuesConditions = txtNewIssuesConditions.Value,
                                          RemarksAcc = txtNewRemarks.Value,
                                          Factor = txtNewFactor.Value,
                                          Perbet = _perbet,
                                          Personnel = ""
                                      };
                #endregion
                _esc.NewEntity3(_newEntity, _newAccount);
            }
            else
            {
                _esc.NewEntity1(_newEntity);
            }

        }

        protected void btnCashMainEntity_Click(object sender, EventArgs e)
        {
            entityUpdate(txtCashMainEntityName.Value, EntityType.Cash, 0, 0, 0, SumType.Not);
            JsonEntityTreeString = JsonEntityFunc.LoadEntityTree();
            JsonRelationTreeString = JsonEntityFunc.LoadRelationEntityTree();
        }

        

        protected void btnCurrencyER_Click(object sender, EventArgs e)
        {
            var _esc = new EntityServiceClient();
            int _entityId = int.Parse(hfCurrencyEntityId.Value);
            string _currency = ddlCurrency.Value;
            decimal _ER = decimal.Parse(txtCurrencyER.Value);
            _esc.SetCurrencyAndRate(_entityId, _currency, _ER);
            Library.Alert("Set Exchange Rate success!");
            JsonEntityTreeString = JsonEntityFunc.LoadEntityTree();
            JsonRelationTreeString = JsonEntityFunc.LoadRelationEntityTree();
            loadCurrency();
        }

        protected void btnNewMainEntity_Click(object sender, EventArgs e)
        {
            var _entityType = entityTypeFunc(SelectRole.Value);
            string _entityName =
                SelectRole.Value.Equals("Cash", StringComparison.OrdinalIgnoreCase)
                    ? txtCashMainEntityName.Value
                    : txtNewMainEntityName.Value;
            entityInsert(_entityName, _entityType, 0, 0, 0, SumType.Not);
            Library.Alert("Add success!");
            JsonEntityTreeString = JsonEntityFunc.LoadEntityTree();
            JsonRelationTreeString = JsonEntityFunc.LoadRelationEntityTree();
        }

        protected void btnNewNodeEntity_Click(object sender, EventArgs e)
        {
            var _entityType = hfNewNodeEntityParentType.Value.Equals("PnL")
                                  ? EntityType.PAndL
                                  : hfNewNodeEntityParentType.Value.Equals("Cash")
                                        ? EntityType.Cash
                                        : EntityType.Expence;
            string _entityName = txtNewNodeEntityName.Value;
            if (rbNewNodeEntityLastLVN.Checked)
            {
                entityInsert(_entityName, _entityType, int.Parse(hfNewNodeEntityParentId.Value), 0, 0, SumType.Not);
            }
            else
            {
                entityInsert(_entityName, _entityType, int.Parse(hfNewNodeEntityParentId.Value), 1,
                             cbNewAccount.Checked ? 1 : 0, SumType.Not);
            }
            Library.Alert("Add success!");
            JsonEntityTreeString = JsonEntityFunc.LoadEntityTree();
            JsonRelationTreeString = JsonEntityFunc.LoadRelationEntityTree();
        }

        protected void btnNodeEntity_Click(object sender, EventArgs e)
        {
            var _entityType = hfNodeEntityType.Value.Equals("PnL", StringComparison.OrdinalIgnoreCase)
                                  ? EntityType.PAndL
                                  : hfNodeEntityType.Value.Equals("Cash", StringComparison.OrdinalIgnoreCase)
                                        ? EntityType.Cash
                                        : EntityType.Expence;
            int _isLastLevel = rbNodeEntityLastLVN.Checked ? 0 : 1;
            int _isAccount = cbAccount.Checked ? 1 : 0;
            var _sumType = int.Parse(ddlSumType.Value) == 0
                               ? SumType.Not
                               : int.Parse(ddlSumType.Value) == 1 ? SumType.Subtotal : SumType.Transaction;
            entityUpdate(txtNodeEntityName.Value, _entityType, int.Parse(hfNodeEntityParentId.Value), _isLastLevel,
                         _isAccount, _sumType);
            Library.Alert("Update success!");
            JsonEntityTreeString = JsonEntityFunc.LoadEntityTree();
            JsonRelationTreeString = JsonEntityFunc.LoadRelationEntityTree();
        }

        private void entityUpdate(string _entityName, EntityType _entityType, int _parentId, int _isLastLevel,
                                  int _isAccount, SumType _sumType)
        {
            var _esc = new EntityServiceClient();
            #region "Upate Entity"
            var _Entity = new Entity
                              {
                                  EntityName = _entityName,
                                  EntityType = _entityType,
                                  ParentID = _parentId,
                                  IsLastLevel = _isLastLevel,
                                  IsAccount = _isAccount,
                                  Enable = 1,
                                  SumType = _sumType
                              };
            #endregion
            if (_entityType == EntityType.Cash && _parentId == 0)
            {
                _Entity.EntityID = int.Parse(hfCashEntityId.Value);
                _Entity.Currency = new Currency {CurrencyID = txtCashMainEntityCurrency.Value};
                _Entity.ExchangeRate = decimal.Parse(txtCashMainEntityER.Value);
                #region "Cash Entity"
                var _CashEntity = new CashEntity
                                      {
                                          ContractNumber = txtContactNumber.Value,
                                          CreditLimit = decimal.Parse(txtCreditLimit.Value),
                                          Email = txtEmail.Value,
                                          QQ = txtQQ.Value,
                                          RecommendedBy = txtRecommendedby.Value,
                                          SettlementName = txtSettlementName.Value,
                                          SettlementNumber = txtSettlementNumber.Value,
                                          Skype = txtSkype.Value,
                                          TallyName = txtTallyName.Value,
                                          TallyNumber = txtTallyNumber.Value
                                      };
                #endregion
                _esc.SaveEntity2(_Entity, _CashEntity);
            }
            else if (_parentId != 0)
            {
                _Entity.EntityID = int.Parse(hfNodeEntityId.Value);
                if (txtEntityCurrency.Value != "")
                    _Entity.Currency = new Currency {CurrencyID = txtEntityCurrency.Value};
                else
                {
                    _Entity.Currency = new Currency {CurrencyID = ""};
                }
                _Entity.ExchangeRate = decimal.Parse(txtEntityER.Value);

                #region "_accountType"

                var _accountType = new AccountType();
                switch (int.Parse(ddlAccType.Value))
                {
                    case 1:
                        {
                            _accountType = AccountType.SuperSenior;
                            break;
                        }
                    case 2:
                        {
                            _accountType = AccountType.Senior;
                            break;
                        }
                    case 3:
                        {
                            _accountType = AccountType.Master;
                            break;
                        }
                    case 4:
                        {
                            _accountType = AccountType.Agent;
                            break;
                        }
                    case 5:
                        {
                            _accountType = AccountType.Members;
                            break;
                        }
                }

                #endregion

                #region "Status"

                var _status = new Status();
                switch (int.Parse(ddlAccStatus.Value))
                {
                    case 1:
                        {
                            _status = Status.Suspended;
                            break;
                        }
                    case 2:
                        {
                            _status = Status.Closed;
                            break;
                        }
                    case 3:
                        {
                            _status = Status.NoFight;
                            break;
                        }
                    case 4:
                        {
                            _status = Status.FollowBet;
                            break;
                        }
                    case 5:
                        {
                            _status = Status.LousyOdds;
                            break;
                        }
                    case 6:
                        {
                            _status = Status.NeedToOpenBack;
                            break;
                        }
                }

                #endregion

                if (cbAccount.Checked)
                {
                    decimal _bettingLimit;
                    decimal.TryParse(txtAccBetLimit.Value, out _bettingLimit);
                    int _account;
                    int.TryParse(hfNodeEntityAccountID.Value,out _account);
                    decimal _perbet;//txtNewPerBet.Value
                    if (!decimal.TryParse(txtNewPerBet.Value, out _perbet))
                        _perbet = 0;
                    #region "Upate New Account"
                    var _newAccount = new Account
                                          {
                                              Company = int.Parse(ddlAccCompany.Value),
                                              AccountName = txtAccName.Value,
                                              AccountType = _accountType,
                                              BettingLimit = _bettingLimit,
                                              Password = txtAccPwd.Value,
                                              Status = _status,
                                              ID = _account,
                                              DateOpen = txtDateOpen.Value,
                                              IP = txtIP.Value,
                                              Odds = txtOdds.Value,
                                              IssuesConditions = txtIssuesConditions.Value,
                                              RemarksAcc = txtRemarks.Value,
                                              Factor = txtFactor.Value,
                                              Perbet = _perbet,
                                              Personnel = txtPersonnel.Value
                                          };
                    #endregion
                    _esc.SaveEntity3(UserId, _Entity, _newAccount);
                }
                else
                {
                    _esc.SaveEntity1(_Entity);
                }
            }
            else
            {
                decimal _exchangeRate;
                decimal.TryParse(txtEntityER.Value, out _exchangeRate);
                _Entity.EntityID = int.Parse(hfNodeEntityId.Value);
                _Entity.Currency = new Currency {CurrencyID = txtEntityCurrency.Value};
                _Entity.ExchangeRate = _exchangeRate;

                _esc.SaveEntity1(_Entity);
            }
        }

        protected void btnNewCashMainEntity_Click(object sender, EventArgs e)
        {
            entityInsert(txtNewCashMainEntityName.Value, EntityType.Cash, 0, 0, 0, SumType.Not);
            Library.Alert("Add success!");
            JsonEntityTreeString = JsonEntityFunc.LoadEntityTree();
            JsonRelationTreeString = JsonEntityFunc.LoadRelationEntityTree();
        }

        protected void btnRemoveEntity_Click(object sender, EventArgs e)
        {
            var _esr = new EntityServiceClient();
            int _removeEntityId;
            if(int.TryParse(hfChoseEntity.Value,out _removeEntityId))
                _esr.Disable(_removeEntityId);
            Library.Alert("Delete success!");
            JsonEntityTreeString = JsonEntityFunc.LoadEntityTree();
            JsonRelationTreeString = JsonEntityFunc.LoadRelationEntityTree();
        }

        private EntityType entityTypeFunc(string _entityTypeStr)
        {
            EntityType _entityType = _entityTypeStr.Equals("PnL", StringComparison.OrdinalIgnoreCase)
                                  ? EntityType.PAndL
                                  : _entityTypeStr.Equals("Cash", StringComparison.OrdinalIgnoreCase)
                                        ? EntityType.Cash
                                        : _entityTypeStr.Equals("Exp", StringComparison.OrdinalIgnoreCase)
                                              ? EntityType.Expence
                                              : EntityType.BadDebt;
            return _entityType;
        }

        protected void btnBadDebt_Click(object sender, EventArgs e)
        {
            var _esr = new EntityServiceClient();
            int _badDebtEntityId;
            if (int.TryParse(hfChoseEntity.Value, out _badDebtEntityId))
                _esr.ChangeEntityType(_badDebtEntityId,EntityType.BadDebt);
            JsonEntityTreeString = JsonEntityFunc.LoadEntityTree();
            JsonRelationTreeString = JsonEntityFunc.LoadRelationEntityTree();
        }
    }
}