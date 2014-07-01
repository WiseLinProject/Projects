using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Accounting_System.PropertiesServiceReference;
using Oleit.AS.Service.DataObject;


namespace Accounting_System
{
    public partial class PropertiesSet : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckLimit.CheckPage(Request["menuid"]);
            if (!IsPostBack)
            {
                loadPropertis();
            }
        }

        /// <summary>
        /// Initial Properties repeater
        /// </summary>
        private void loadPropertis()
        {
            var _psc = new PropertiesServiceClient();
            propertyRPT.DataSource= _psc.GetAllProperties().ToList();
            propertyRPT.DataBind();
        }

        /// <summary>
        /// Add a new Property
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            var _ptsc = new PropertiesServiceClient();
            var _p = new Property
                        {
                            PropertyName = txtPropertyNameAdd.Value,
                            PropertyValue = txtPropertyValueAdd.Value
                        };
            _ptsc.Insert(_p);
            loadPropertis();
        }

        /// <summary>
        /// Edit the Property
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            var _ptsc = new PropertiesServiceClient();
            var _editP = new Property
                             {
                                 PropertyName = txtPropertyNameEdit.Value,
                                 PropertyValue = txtPropertyValueEdit.Value
                             };
            _ptsc.SetProperty(hfEditValueId.Value, _editP);
            loadPropertis();
        }
    }
}