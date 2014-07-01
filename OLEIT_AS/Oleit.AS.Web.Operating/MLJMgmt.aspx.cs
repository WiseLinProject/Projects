using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Accounting_System.InternalUserServiceReference;
using Accounting_System.MLJServiceReference;
using Accounting_System.PropertiesServiceReference;
using Accounting_System.EntityServiceReference;
using Accounting_System.SystemDataServiceReference;
using Accounting_System.MenuServiceReference;
using Oleit.AS.Service.DataObject;
using AjaxControlToolkit;
using System.Data;


namespace Accounting_System
{
    public partial class MLJMgmt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckLimit.CheckPage(Request["menuid"]);
            if (!IsPostBack)
            {
                using (MenuServiceClient _menuclient = new MenuServiceClient())
                {
                    RoleCollection _roleco = new RoleCollection(_menuclient.QueryAllRole());
                    foreach (Role _role in _roleco)
                    {
                        if (_role.ID.Equals(5) || _role.ID.Equals(7))
                            DDL_MLJRole.Items.Add(new ListItem(_role.RoleName, _role.ID.ToString()));
                    }
                }
                GetData();
                GetColor();
                GetMLJ();

            }
        }

        private void GetData()
        {           
            using (MenuServiceClient _menuclient = new MenuServiceClient())
            {               
                UserCollection _userc = new UserCollection(_menuclient.QueryRoleUser(Convert.ToInt32(DDL_MLJRole.SelectedValue)));
                Listb_User.DataTextField = "UserName";
                Listb_User.DataValueField = "UserID";
                Listb_User.DataSource = _userc.ToList();
                Listb_User.DataBind();
            }
        }

        private void GetMLJ()
        {
            PropertiesServiceClient _proclient = new PropertiesServiceClient();
            int _MLJEntityID = Convert.ToInt32((_proclient.GetPropertyValue2("MLJEntity"))[0].PropertyValue);
            EntityServiceClient _entityclient = new EntityServiceClient();
            EntityCollection _accountcollection = new EntityCollection(_entityclient.QueryAllMLJ(_MLJEntityID));
            CBL_MLJ.DataTextField = "EntityName";
            CBL_MLJ.DataValueField = "EntityID";
            if (DDL_MLJ.SelectedValue.Equals("1"))
            {
                var list = _accountcollection.Where(e => e.IsAccount == 0);
                CBL_MLJ.DataSource = list;
            }
            else if (DDL_MLJ.SelectedValue.Equals("2"))
            {
                var list = _accountcollection.Where(e => e.IsAccount == 1);
                CBL_MLJ.DataSource = list;
            }
            else
                CBL_MLJ.DataSource = _accountcollection.ToList();
            CBL_MLJ.DataBind();
            CBL_MLJ.Items.Insert(0,new ListItem("Select All","0"));
            CBL_MLJ.Items[0].Attributes.Add("style", "font-weight:bold");
        }

        private void GetColor()
        {
            //getallstatus
            using (MLJServiceClient _mljclient = new MLJServiceClient())
            {
                StatusColorCollection _collection = new StatusColorCollection(_mljclient.QueryStatusColor());
                gv_StatusColor.DataSource = _collection.ToList();
                gv_StatusColor.DataBind();
            }
            Udate_Color.Update();
        }
        protected void gv_StatusColor_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                
                TextBox _Tx_Color = (TextBox)e.Row.Cells[1].FindControl("Tx_Color");
                Label _Lb_Color = (Label)e.Row.Cells[0].FindControl("Lb_Color");
                ColorPickerExtender _Color = (ColorPickerExtender)e.Row.Cells[1].FindControl("ColorPickerExtender1");
                e.Row.Cells[0].Text = ((Oleit.AS.Service.DataObject.Status)Convert.ToInt32(_Lb_Color.Text)).ToString();
                string _color = "#" + _Tx_Color.Text;
                e.Row.Cells[0].BackColor = System.Drawing.ColorTranslator.FromHtml(_color);
                _Tx_Color.Attributes.Add("readonly", "true");
                 
            }
        }

        protected void Btn_SaveColor_Click(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(1500);
            using (MLJServiceClient _mljclient = new MLJServiceClient())
            {
                StatusColorCollection _collection = new StatusColorCollection();
                for (int i = 0; i < gv_StatusColor.Rows.Count; i++)
                {
                    StatusColor _color = new StatusColor();
                    _color.MLJStatus = Convert.ToInt32(gv_StatusColor.DataKeys[i].Values[0].ToString());
                    _color.StatusType = (Status)Convert.ToInt32(gv_StatusColor.DataKeys[i].Values[0].ToString());
                    _color.MLJColor = ((TextBox)gv_StatusColor.Rows[i].Cells[1].FindControl("Tx_Color")).Text;
                    _collection.Add(_color);
                }
                _mljclient.UpdateColor(_collection.ToArray());
            }
            Alert("Save Completed!!!");
            GetColor();
        }

        protected void Btn_SaveUser_Click(object sender, EventArgs e)
        {
            if (Listb_User.SelectedIndex < 0)
            {
                Alert("Please select one System User");
                return;
            }
            else
            {
                int _Userid = Convert.ToInt32(Listb_User.SelectedValue);
                EntityCollection _collection = new EntityCollection();
                foreach (ListItem _mljli in CBL_MLJ.Items)
                {
                    if (_mljli.Selected && _mljli.Enabled)
                    {
                        Entity _entity = new Entity();
                        _entity.EntityID = Convert.ToInt32(_mljli.Value);
                        _collection.Add(_entity);
                    }

                }                
                using (MLJServiceClient _mljclient = new MLJServiceClient())
                {
                    _mljclient.UpdateUserMLJ(_Userid, _collection.ToArray());
                }
                Alert("Save Completed!!!");
                Update_User.Update();
            }
            
        }

        private void Alert(string message)
        {
            string script = string.Format("alert('{0}');", HttpUtility.HtmlEncode(message.Replace("\n", "\\n")));
            JavaScrpit("window_aler", script);
        }
        private void JavaScrpit(string name, string script)
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), name, script, true);
        }

        protected void DDL_MLJ_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetMLJ();
            if (Listb_User.SelectedIndex > -1)
                CheckMLJ();
        }

        protected void Listb_User_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckMLJ();
        }

        private void CheckMLJ()
        {
            string UserID = Listb_User.SelectedItem.Value;
            using (SystemDataServiceClient _dataclient = new SystemDataServiceClient())
            {
                DataTable _dtuserRMLJ = _dataclient.GetUserMLJEntity().Tables[0];
                if (_dtuserRMLJ.Rows.Count > 0)
                {

                    foreach (ListItem _mljli in CBL_MLJ.Items)
                    {
                        _mljli.Selected = false;
                        _mljli.Enabled = true;
                        foreach (DataRow _row in _dtuserRMLJ.Rows)
                        {
                            string _Entityid = _row["Entityid"].ToString();
                            string _Userid = _row["Userid"].ToString();
                            if (_mljli.Value.Equals(_Entityid) && !_Userid.Equals(UserID))
                            {
                                _mljli.Enabled = false;
                                _mljli.Selected = true;
                                //  _mljli.Text = _mljli.Text + " ( " + _row["UserName"].ToString() + " ) ";
                            }
                            else if (_mljli.Value.Equals(_Entityid) && _Userid.Equals(UserID))
                            {
                                _mljli.Selected = true;
                            }

                            //if (_Userid.Equals(UserID))
                            //    _mljli.Selected = true;
                            //else
                            //    _mljli.Selected = false;
                        }
                    }

                }
            }
        }

        protected void DDL_MLJRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetData();
        }

        protected void CBL_MLJ_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CBL_MLJ.Items[0].Selected && !Listb_User.SelectedIndex.Equals(-1) && !CBL_MLJ.Items[0].Text.Equals("Cancel All"))
            {
                foreach (ListItem _item in CBL_MLJ.Items)
                {
                    if (_item.Enabled && !_item.Selected)
                        _item.Selected = true;
                }
                CBL_MLJ.Items[0].Text = "Cancel All";
            }
            else if (!CBL_MLJ.Items[0].Selected && !Listb_User.SelectedIndex.Equals(-1) && CBL_MLJ.Items[0].Text.Equals("Cancel All"))
            {
                foreach (ListItem _item in CBL_MLJ.Items)
                {
                    if (_item.Enabled && _item.Selected)
                        _item.Selected = false;
                }
                CBL_MLJ.Items[0].Text = "Select All";
            }
            CBL_MLJ.Items[0].Attributes.Add("style", "font-weight:bold");
        }
    }
}