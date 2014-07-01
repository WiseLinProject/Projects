using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Accounting_System.TallyServiceReference;
using Accounting_System.EntityServiceReference;
using Oleit.AS.Service.DataObject;
using Newtonsoft.Json;

namespace Accounting_System
{
    public partial class TallyAjax : System.Web.UI.Page
    {
        public string JsonTallyTreeString = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] != null)
            {
                int _entityId;
                int.TryParse(Request["entityId"], out _entityId);

                int _userID;
                int.TryParse(SessionData.UserID.ToString(), out _userID);

                string _periodID = Request["periodID"];
                if (Request["type"].Equals("load", StringComparison.OrdinalIgnoreCase))
                {
                    Response.Write(loadTallyTree(_entityId, _periodID));
                }
                else if (Request["type"].Equals("refCash", StringComparison.OrdinalIgnoreCase))
                {
                    Response.Write(loadRefCashTree(_entityId));
                }
                else if (Request["type"].Equals("loadPnL", StringComparison.OrdinalIgnoreCase))
                {
                    Response.Write(loadPnLTallyTree(_entityId, _periodID));
                }
                else
                {
                    confirmTally(_userID, _entityId);
                }
            }
            else
            {
                Response.Write("Session has expired.");
            }
        }
        
        private string loadTallyTree(int entityId,string periodID)
        {
            string _periodID = periodID;
            var _tsr = new TallyServiceClient();
            var _loadEntity = _tsr.LoadEntity(entityId);
            EntityCollection _tallyTree = new EntityCollection(_loadEntity.m_Item1);//SerializeToJson
            string _tallyStr = JsonEntityFunc.LoadEntityTree(_tallyTree, _loadEntity.m_Item2);
            return _tallyStr;
        }

        private string loadPnLTallyTree(int entityId, string periodID)
        {
            string _periodID = periodID;
            var _tsr = new TallyServiceClient();
            var _loadEntity = _tsr.LoadEntity(entityId);
            EntityCollection _tallyTree = new EntityCollection(_loadEntity.m_Item1);//SerializeToJson
            string _tallyStr = JsonEntityFunc.LoadPnLEntityTree(_tallyTree, _loadEntity.m_Item2);
            return _tallyStr;
        }

        private string loadRefCashTree(int entityId)
        {
            var _esr = new EntityServiceClient();
            return _esr.QuerySingleBranchEntity(entityId);
        }

        private void confirmTally(int userID, int entityId)
        {
            var _tsr = new TallyServiceClient();
            var _wsJson = JsonConvert.SerializeObject(_tsr.Confirm(userID, entityId));
            Response.Write(_wsJson);

        }
    }
}