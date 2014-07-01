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
using System.Text;

namespace Accounting_System
{
    public partial class EntityMgmt2 : System.Web.UI.Page
    {
        public string JsonEntityTreeString = "";
        public string JsonRelationTreeString = "";
        public string MainEntityString = "";
        public int UserId;

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckLimit.CheckPage(Request["menuid"]);
            if (!IsPostBack)
            {
                UserId = int.Parse(Session["UserId"].ToString());
                loadCurrency();
                loadTree();
                mainEntityString();
            }
        }

        private void mainEntityString()
        {
            var _esr = new EntityServiceClient();
            var _allEntities = _esr.LoadEntity1();
            var _mainEntities = _allEntities.Where(x => x.ParentID == 0).Select(x => x.EntityName);
            StringBuilder _sbNames = new StringBuilder();
            foreach (var e in _mainEntities)
            {
                _sbNames.AppendFormat("{0},",e);
            }
            if (_sbNames.Length > 0)
            {
                _sbNames.Length = _sbNames.Length - 1;
                MainEntityString = _sbNames.ToString();
            }
        }

        private void loadCurrency()
        {
            var _csr = new CurrencyServiceClient();
            var _allCurrency = _csr.AllCurrency().Select(x => x.CurrencyID);
            ddlNewCurrency.DataSource = _allCurrency.ToList();
            ddlNewCurrency.DataBind();
            ddlCurrency.DataSource = _allCurrency.ToList();
            ddlCurrency.DataBind();
        }

        private void loadTree()
        {
            JsonEntityTreeString = JsonEntityFunc.LoadMainEntityTree(0);
            JsonRelationTreeString = JsonEntityFunc.LoadRelationEntityTree();
            mainEntityString();
        }

        protected void btnCashMainEntity_Click(object sender, EventArgs e)
        {
            var _entityType = EntitiesFunc.accoutnTypeFunc(int.Parse(ddlAccType.Value));
            #region "Entity"
            Entity _entity = new Entity
            {
                EntityID = int.Parse(hfCashEntityId.Value),
                EntityName = txtCashMainEntityName.Value,
                EntityType = EntityType.Cash,
                IsAccount = 0,
                IsLastLevel = 0,
                ParentID = 0,
                Enable = 1,
                ExchangeRate = 1,
                SumType = SumType.Not
            };
            #endregion

            #region "Cash Entity"
            CashEntity _cashEntity = new CashEntity
            {
                ContractNumber= txtContactNumber.Value,
                TallyName = txtTallyName.Value,
                TallyNumber = txtTallyNumber.Value,
                SettlementName = txtSettlementName.Value,
                SettlementNumber = txtSettlementNumber.Value,
                RecommendedBy = txtRecommendedby.Value,
                Skype = txtSkype.Value,
                QQ = txtQQ.Value,
                Email = txtEmail.Value,
                CreditLimit = decimal.Parse(txtCreditLimit.Value),

            };
            #endregion
            try
            {
                EntitiesFunc.entityUpdate(_entity,_cashEntity);
                Library.Alert("Success!");
            }
            catch
            {
                Library.Alert("Fail!");
            }
            btnSearchMainEntity_Click(sender, e);
            hfReload.Value = string.Format("{0},{1}", _entity.EntityID, _entity.EntityName);
        }


        protected void btnNewCashMainEntity_Click(object sender, EventArgs e)
        {
            EntityType _entityType = EntitiesFunc.entityTypeFunc(int.Parse(SelectRole.Value));

            #region "New Entity"
            var _entity = new Entity
            {
                EntityName = txtNewCashMainEntityName.Value,
                EntityType = _entityType,
                ParentID = 0,
                IsLastLevel = 0,
                IsAccount = 0,
                Enable = 1,
                SumType = SumType.Not
            };
            #endregion

            #region "Cash Attributes"
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

            EntitiesFunc.entityInsert(_entity, _newCashEntity);
            Library.Alert("Add success!");
            loadTree();
            JsonEntityTreeString = JsonEntityFunc.LoadMainEntityTree(_entity.EntityName, 0);
        }

        protected void btnNewMainEntity_Click(object sender, EventArgs e)
        {
            EntityType _entityType = EntitiesFunc.entityTypeFunc(int.Parse(SelectRole.Value));

            #region "Upate Entity"
            var _entity = new Entity
            {
                EntityName = txtNewMainEntityName.Value,
                EntityType = _entityType,
                ParentID = 0,
                IsLastLevel = 0,
                IsAccount = 0,
                Enable = 1,
                SumType = SumType.Not
            };
            #endregion

            EntitiesFunc.entityInsert(_entity);
            Library.Alert("Add success!");
            loadTree();
            JsonEntityTreeString = JsonEntityFunc.LoadMainEntityTree(_entity.EntityName, 0);
        }

        protected void btnMainEntity_Click(object sender, EventArgs e)
        {
            var _entityType = EntitiesFunc.accoutnTypeFunc(int.Parse(ddlAccType.Value));
            #region "Entity"
            Entity _entity = new Entity
            {
                EntityID = int.Parse(hfMainEntityId.Value),
                EntityName = txtMainEntityName.Value,
                EntityType = EntitiesFunc.entityTypeFunc(hfMainEntityType.Value),
                IsAccount = 0,
                IsLastLevel = 0,
                ParentID = 0,
                Enable = 1,
                ExchangeRate = 1,
                SumType = SumType.Not
            };
            #endregion
            try
            {
                EntitiesFunc.entityUpdate(_entity);
                Library.Alert("Success!");
            }
            catch
            {
                Library.Alert("Fail!");
            }
            btnSearchMainEntity_Click(sender,e);
            hfReload.Value=string.Format("{0},{1}", _entity.EntityID, _entity.EntityName);
        }

        protected void btnSearchMainEntity_Click(object sender, EventArgs e)
        {
            string _mainEntityName = txtMainEntitySearch.Value;
            int _entityType = int.Parse(ddlEntityType.Value);
            if(_mainEntityName!="")
                JsonEntityTreeString = JsonEntityFunc.LoadMainEntityTree(_mainEntityName, _entityType);
            else
                JsonEntityTreeString = JsonEntityFunc.LoadMainEntityTree(_entityType);
            JsonRelationTreeString = JsonEntityFunc.LoadRelationEntityTree();
            mainEntityString();
        } 
    }
}