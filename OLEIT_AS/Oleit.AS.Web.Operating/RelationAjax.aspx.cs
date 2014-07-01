using System;
using System.Linq;
using Oleit.AS.Service.DataObject;
using Accounting_System.EntityServiceReference;
using System.Text;

namespace Accounting_System
{
    public partial class RelationAjax : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["UserID"] != null)
            //{
                string _type = Request["Type"];
                decimal _value;
                decimal.TryParse(Request["Value"], out _value);
                string _entityClass = Request["EntityClass"];
                int _entityId = int.Parse(Request["EntityId"]);
                int _targetEntityId;
                int.TryParse(Request["TargetId"], out _targetEntityId);

                int _relationValue;
                int.TryParse(Request["RelationValue"], out _relationValue);

                string _relationLoad = Request["RelationLoad"];
                if (_type.Equals("load", StringComparison.OrdinalIgnoreCase))
                {
                    loadRelation(_entityId);
                }
                else if (_type.Equals("add", StringComparison.OrdinalIgnoreCase))
                {
                    addRelation(_entityClass, _relationValue, _value, _entityId, _targetEntityId);
                }
                else if (_type.Equals("remove", StringComparison.OrdinalIgnoreCase))
                {
                    removeRelation(_entityId, _targetEntityId);
                }
                else
                {
                    Response.Write("Incorrect load!");
                }
            //}
            //else
            //{
            //    Response.Write("Session has expired.");
            //}
        }
        
        private void addRelation(string entityClass, int relationValue, decimal value, int entityId, int targetEntityId)
        {
            var _esc = new EntityServiceClient();
            //var _relationCollection = 
            if (_esc.GetRelateEntity(entityId).Any(x => x.Description == RelationDescription.Allocate) && relationValue==1)
                Response.Write("There is already a Allocate Relation!");
            else if (relationValue == 5 && entityClass.IndexOf("MainEntity", StringComparison.OrdinalIgnoreCase) == -1)
            {//when other entities chose the PnLSum
                Response.Write("PAndLSum type only can be chosen by P&L Main Entity!");
            }
            else if (entityClass.IndexOf("MainEntity", StringComparison.OrdinalIgnoreCase)!=-1 && relationValue<5)
            {//when PnL Main entity didn't choose the PnLSum type
                Response.Write("P&L Main Entity only can choose the PnLSum type!");
            }
            else
            {
                RelationDescription _newR = relationValue == 1 ? RelationDescription.Allocate : relationValue == 2 ? RelationDescription.Position : relationValue == 3 ? RelationDescription.Commission : relationValue == 4 ? RelationDescription.FollowBet : RelationDescription.PAndLSum;
                #region "Relation Value"
                /*

                switch (_relationValue)
                {
                    case 1:
                        {
                            _newRD = RelationDescription.Allocate;
                            break;
                        }
                    case 2:
                        {
                            _newRD = RelationDescription.Commission;
                            break;
                        }
                    case 3:
                        {
                            _newRD = RelationDescription.Position;
                            break;
                        }
                    case 4:
                        {
                            _newRD = RelationDescription.FollowBet;
                            break;
                        }
                }

                */
               #endregion
                Relation _newRelation = new Relation
                {
                    Entity = new Entity { EntityID = entityId },
                    TargetEntity = new Entity { EntityID = targetEntityId },
                    Description = _newR,
                    Numeric = value
                };
                if (entityId != targetEntityId)
                {
                    
                    try
                    {
                        _esc.NewRelation(_newRelation);
                        Response.Write("Success!");
                    }
                    catch (Exception)
                    {
                        Response.Write("Failed!");
                    }
                }
                else
                {
                    Response.Write("You can't relate to yourself!");
                }
            }
        }
        
        private void loadRelation(int entityId)
        {
            const string _deleteIcon = "<span title=\"Remove relation\" onclick=\"deleteRelation(this);\" class=\"ui-button-icon-primary ui-icon ui-icon-closethick \"></span>";
            StringBuilder sb = new StringBuilder();
            
            EntityServiceClient _esc = new EntityServiceClient();
            var _relationEntity = _esc.RelationEntity(entityId);
            if (!_relationEntity.Any())
            {
                sb.Append("<tr id=\"trRelationTitle\"><td class=\"relationTitle\">Relation</td><td class=\"relationTitle\">To</td><td class=\"relationTitle\">Value</td><td ></td></tr>");
            }
            else if (!_relationEntity.First().Description.Equals(RelationDescription.PAndLSum))
            {
                sb.Append("<tr id=\"trRelationTitle\"><td class=\"relationTitle\">Relation</td><td class=\"relationTitle\">To</td><td class=\"relationTitle\">Value</td><td ></td></tr>");
                foreach (var r in _relationEntity)
                {
                    sb.AppendFormat("<tr id='rId{0}' rtype='{1}' targetEntityId='{2}' ><td>{3}</td><td>{4}({5})</td><td>{6}</td><td>{7}</td></tr>", entityId, r.Description, r.TargetEntity.EntityID, r.Description, r.TargetEntity.EntityName, r.Entity.EntityName, r.Numeric, _deleteIcon);
                }
            }
            else
            {
                sb.Append("<tr id=\"trRelationTitle\"><td class=\"relationTitle\">Relation</td><td class=\"relationTitle\">To</td><td ></td></tr>");
                foreach (var r in _relationEntity)
                {
                    sb.AppendFormat("<tr id='rId{0}' rtype='{1}' targetEntityId='{2}' ><td>{3}</td><td>{4}({5})</td><td>{6}</td></tr>", entityId, r.Description, r.TargetEntity.EntityID, r.Description, r.TargetEntity.EntityName, r.Entity.EntityName, _deleteIcon);
                }
            }
            Response.Write(sb.ToString());
        }

        private void removeRelation(int entityId, int targetEntityId)
        {
            var _esc = new EntityServiceClient();
            try
            {
                _esc.RemoveRelation(entityId, targetEntityId);
                Response.Write("Success!");
            }
            catch (Exception)
            {
                Response.Write("Fail!");
            }
        }
    }
}