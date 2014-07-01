using Accounting_System.InternalUserServiceReference;
using Accounting_System.CurrencyServiceReference;
using Accounting_System.PeriodServiceReference;
using Accounting_System.PropertiesServiceReference;
using Accounting_System.EntityServiceReference;
using Accounting_System.CalculateServiceReference;
using Accounting_System.SettleServiceReference;
using Accounting_System.DataEntryServiceReference;

using Oleit.AS.Service.DataObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;

namespace Accounting_System
{
    public partial class TestCode2 : System.Web.UI.Page
    {
        public string aa = "Trevor";
        public void RaisePostBackEvent(string eventArgument) { Response.Write("trevor"); }
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["userid"] = "Trevor";
            //tree
            List<MyObject> list = new List<MyObject>();

            EntityServiceClient _intclient = new EntityServiceClient();
            foreach (Entity entity in new EntityCollection(_intclient.LoadAll()))
            {

                list.Add(new MyObject() { Id = entity.EntityID, Name = entity.EntityName, ParentId = entity.ParentID });

            }
            BindTree(list, null);
        }
        private void BindTree(IEnumerable<MyObject> list, TreeNode parentNode)
        {
            var nodes = list.Where(x => parentNode == null ? x.ParentId == 0 : x.ParentId == int.Parse(parentNode.Value));
            foreach (var node in nodes)
            {
                TreeNode newNode = new TreeNode(node.Name, node.Id.ToString());
                if (parentNode == null)
                {
                    Tree_Entity.Nodes.Add(newNode);
                }
                else
                {
                    parentNode.ChildNodes.Add(newNode);
                }
                BindTree(list, newNode);
            }
        }

        public class MyObject
        {
            public int Id;
            public int ParentId;
            public string Name;
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            //InternalUserServiceClient AD = new InternalUserServiceClient();
            //Response.Write(AD.QueryAllAD());

            InternalUserServiceClient _ad = new InternalUserServiceClient();

            string _adlist = new UserCollection(_ad.QueryADuser()).SerializeToJson();
            Response.Write(_adlist);
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            //InternalUserServiceClient AD = new InternalUserServiceClient();
            //string returnVal = AD.CheckPassword("Y", TextBox1.Text, TextBox2.Text);
            //if (!returnVal.Equals("False"))
            //{
            //    Response.Write(returnVal + "     Welcome !!!");
            //    Session["User_Name"] = returnVal;
            //}
            //else
            //    Response.Write("Login Fail !!!");


        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            CurrencyServiceClient _allcurrency = new CurrencyServiceClient();
            CurrencyCollection collection = new CurrencyCollection(_allcurrency.AllCurrency());
            string _currency = collection.SerializeToJson();
            Response.Write(_currency);
            //collection = CurrencyCollection.DeserializeFromJson(_currency);
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            //CurrencyServiceClient _allcurrency = new CurrencyServiceClient();
            //Oleit.AS.Service.DataObject.Currency currency = new Oleit.AS.Service.DataObject.Currency();
            //currency.CurrencyID = TextBox3.Text;
            //_allcurrency.DelCurrency(currency);

            CurrencyServiceClient _allcurrency = new CurrencyServiceClient();
            Oleit.AS.Service.DataObject.Currency currency = new Oleit.AS.Service.DataObject.Currency();
            currency.CurrencyID = TextBox3.Text;
            _allcurrency.NewCurrency(currency);

        }

        protected void Button5_Click(object sender, EventArgs e)
        {
            //PeriodServiceClient _allcurrency = new PeriodServiceClient();
            //Oleit.AS.Service.DataObject.Period period = new Oleit.AS.Service.DataObject.Period();
            //period.PeriodNo = TextBox4.Text;
            //period.StartDate = Convert.ToDateTime("2012/12/24");
            //period.EndDate = Convert.ToDateTime("2012/12/31");
            //_allcurrency.SetPeriod(period);
            PeriodServiceClient _allperiod = new PeriodServiceClient();
            // Oleit.AS.Service.DataObject.Period period = new Oleit.AS.Service.DataObject.Period();
            // period = _allcurrency.PeriodByDate(Convert.ToDateTime("2012.12.4"))[0];
            // Response.Write(period.PeriodNo);
            _allperiod.SetPeriod(2015);
            //  PeriodCollection collection = ;
            //  string _period = new PeriodCollection(_allperiod.GetPeriods()).SerializeToJson();
            //  Response.Write(_period.ToString());
        }

        protected void Button6_Click(object sender, EventArgs e)
        {
            //PropertiesServiceClient _allproperty = new PropertiesServiceClient();
            //Oleit.AS.Service.DataObject.Property property = new Oleit.AS.Service.DataObject.Property();
            //property.PropertyName = "Currency";
            //property.PropertyValue = "RMB";
            //Oleit.AS.Service.DataObject.Property property2 = new Oleit.AS.Service.DataObject.Property();
            //property2.PropertyName = "Name";
            //property2.PropertyValue = "Frank";
            //PropertyCollection propertyCollection = new PropertyCollection();
            //propertyCollection.Add(property);
            //propertyCollection.Add(property2);
            //_allproperty.NewProperty(propertyCollection.ToArray());

            //PropertiesServiceClient _allproperty2 = new PropertiesServiceClient();
            //PropertyCollection collection = new PropertyCollection(_allproperty2.GetPropertyValue2("Currency"));
            //string _currency = collection.SerializeToJson();
            //Response.Write(_currency);

            PropertiesServiceClient _allproperty = new PropertiesServiceClient();
            Oleit.AS.Service.DataObject.Property property = new Oleit.AS.Service.DataObject.Property();
            property.PropertyName = "CurrencyBBB";

            property.PropertyValue = "RMB";

            _allproperty.SetProperty("CurrencyAAA", property);
        }

        protected void Button7_Click(object sender, EventArgs e)
        {
            InternalUserServiceClient _intclient = new InternalUserServiceClient();
            Oleit.AS.Service.DataObject.User _user = new Oleit.AS.Service.DataObject.User();
            _user.UserName = "yang";
            _user.UserPWD = "yang";
            _user.Enable = 1;
            Oleit.AS.Service.DataObject.User _user2 = new Oleit.AS.Service.DataObject.User();
            _user2.UserName = "trevorlin";
            _user2.UserPWD = "trevorlin";
            _user2.Enable = 1;
            UserCollection _collection = new UserCollection();
            _collection.Add(_user);
            _collection.Add(_user2);

            _intclient.NewUserCollection(_collection.ToArray());

            //currency.CurrencyID = TextBox3.Text;
            //_allcurrency.NewCurrency(currency);

        }

        protected void Button8_Click(object sender, EventArgs e)
        {
            EntityServiceClient _intclient = new EntityServiceClient();
            Oleit.AS.Service.DataObject.Entity _entity = new Oleit.AS.Service.DataObject.Entity();
            _entity.EntityID = 22;
            _entity.ParentID = 9;
            _entity.EntityName = "NewTestEntityNode_Change";
            _entity.EntityType = (EntityType)1;
            _entity.SumType = (SumType)0;
            _entity.Currency.CurrencyID = "SGD";
            _entity.ExchangeRate = (decimal)1.0000;
            _entity.IsAccount = 1;
            _entity.IsLastLevel = 1;
            _entity.Enable = 1;
            //  _intclient.ChangeRate(3, (decimal).0987);
            //Oleit.AS.Service.DataObject.CashEntity _cashentity = new Oleit.AS.Service.DataObject.CashEntity();
            //_cashentity.ContractNumber = "097777777";
            //_cashentity.TallyName = "Panicel";
            //_cashentity.TallyNumber = "345";
            //_cashentity.SettlementName = "SEEDNET";
            //_cashentity.SettlementNumber = "7373";
            //_cashentity.RecommendedBy = "KNN";
            //_cashentity.Skype = "1234345@hotmail.com";
            //_cashentity.QQ = "9876534";
            //_cashentity.Email = "1111@kdi.com";
            //_cashentity.CreditLimit = 2000000;

            Oleit.AS.Service.DataObject.Account _account = new Account();
            _account.EntityID = 12;
            _account.Company = 2;
            _account.AccountName = "BigBoy";
            _account.Password = "123qweasdc";
            _account.AccountType = (AccountType)1;
            _account.BettingLimit = (decimal)22222222222.00;
            _account.Status = (Status)1;
            _account.ID = 3;
            _intclient.SaveEntity3(26, _entity, _account);

            // EntityCollection collection = new EntityCollection(_intclient.;
            //string _currency = collection.SerializeToJson();
            // Response.Write(_currency);

        }

        protected void Button9_Click(object sender, EventArgs e)
        {
            EntityServiceClient _intclient = new EntityServiceClient();
            //AccountCollection collection = new AccountCollection(_intclient.LoadAccount3("BigBoy"));
            //string _currency = collection.SerializeToJson();
            //Response.Write(_currency);
           // EntityCollection collection = new EntityCollection(_intclient.)
          //  string ParentName = _intclient.RelationName(22);
          //  Response.Write(ParentName);

            //Oleit.AS.Service.DataObject.Entity _entity = new Oleit.AS.Service.DataObject.Entity();
            //_entity.EntityID = 3;
            //_entity.ParentID = 0;
            //_entity.EntityName = "God_Yang";
            //_entity.EntityType = EntityType.Cash;
            //_entity.SumType = SumType.Subtotal;
            //_entity.Currency.CurrencyID = "SGD";
            //_entity.ExchangeRate = (decimal)5.321;
            // _

            //PropertyCollection propertyCollection = new PropertyCollection();
            //propertyCollection.Add(property);
            //propertyCollection.Add(property2);
            //_allproperty.NewProperty(propertyCollection.ToArray());

        }



        protected void Button10_Click(object sender, EventArgs e)
        {
            EntityServiceClient _intclient = new EntityServiceClient();
            //_intclient.Disable(3);
            //_intclient.SetCurrencyAndRate(3, "SGD", (decimal)5.7680);
            User user = new User();
            user.UserID = 12;
            _intclient.ChangeStatus(user, 6, Status.FollowBet);
        }

        protected void Button11_Click(object sender, EventArgs e)
        {
            //EntityServiceClient _intclient = new EntityServiceClient();
            //RelationCollection _collection = new RelationCollection();
            //Relation _relation1 = new Relation();
            //_relation1.Entity.EntityID = 3;
            //_relation1.TargetEntity.EntityID = 6;
            //_relation1.Description = RelationDescription.Allocate;
            //_relation1.Numeric = (decimal)4.3214;
            //_intclient.SetRelation(3, _relation1);

            EntityServiceClient _intclient = new EntityServiceClient();
            RelationCollection _collection = new RelationCollection(_intclient.GetRelateEntity(3));

            string _currency = _collection.SerializeToJson();
            Response.Write(_currency);

        }

        protected void Button13_Click(object sender, EventArgs e)
        {
            //JournalCollection collection = new JournalCollection();
            //Journal journal = new Journal();
            //journal.EntityID = 12;
            //journal.ExchangeRate = (decimal)5.5;
            //journal.SGDAmount = -280;
            //journal.EntryUser.UserID = 34;
            //collection.Add(journal);
            //CalculateServiceClient _intclient = new CalculateServiceClient();
            //EntityServiceClient _entclient = new EntityServiceClient();
            //JournalCollection backcollection = new JournalCollection(_intclient.AutoJournal2(collection.ToArray()));
            //if (collection.Count > 0)
            //{
            //    StringBuilder _content = new StringBuilder();
            //    _content.Append("<table class=\"tableS\">");
            //    _content.Append("<tr><td></td><td>Entity</td><td>Currency</td><td>Rate</td><td>Base Amount</td><td>Amount</td></tr>");
            //    foreach (Journal _journal in backcollection)
            //    {
            //        Entity _entity = new EntityCollection(_entclient.LoadEntity2(_journal.EntityID))[0];
            //        _content.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td></tr>", _entity.EntityType.ToString(), _entity.EntityName, _entity.Currency.CurrencyID, _journal.ExchangeRate, _journal.BaseAmount, _journal.SGDAmount);
            //    }
            //    _content.Append("</table>");
            //    Response.Write(_content.ToString());
           // }
        }

        protected void Button14_Click(object sender, EventArgs e)
        {
            SettleServiceClient _sett = new SettleServiceClient();
            //PropertiesServiceClient _pro = new PropertiesServiceClient();

            Property _property = new Property();
            _property.PropertyName = "ColsedPeriod";
            _property.PropertyValue = "2013.7.4";
            //if (_sett.CloseEntry(_property))
            //    Response.Write("Success");
            //else
            //    Response.Write("Please check every WeeklySummary are confirm");

         //   _sett.CloseEntry(_property);
        }

        protected void Button15_Click(object sender, EventArgs e)
        {
            Record record = new Record();
            record.Period.ID = 447;
            record.RecordStatus = RecordStatus.Normal;
            record.Type = RecordType.WinAndLoss;
            JournalCollection collection = new JournalCollection();
            Journal journal = new Journal();
            journal.EntityID = 12;
            journal.ExchangeRate = (decimal)5.5;
            journal.SGDAmount = -280;
            journal.EntryUser.UserID = 34;
            collection.Add(journal);
            DataEntryServiceClient _intclient = new DataEntryServiceClient();
            _intclient.InsertRecord(record, collection.ToArray());
        }

        protected void Button16_Click(object sender, EventArgs e)
        {
            SettleServiceClient _settle = new SettleServiceClient();
            WeeklySummaryCollection _weekcollection = new WeeklySummaryCollection();
            WeeklySummary week = new WeeklySummary();
            week.Entity.EntityID = 12;
            week.Period.ID = 450;
            week.BaseCurrency = "EUR";
            week.ExchangeRate = (decimal)3.000;
            week.BaseWinAndLoss = (decimal)66;
            week.SGDWinAndLoss = (decimal)22;

            _weekcollection.Add(week);


           // _settle.WinLossConfirm(_weekcollection.ToArray());
        }

        protected void Button17_Click(object sender, EventArgs e)
        {
            Record record = new Record();
            record.Period.ID = 447;
            record.RecordStatus = RecordStatus.Normal;
            record.Type = RecordType.Transfer;
          
            Journal journal = new Journal();
            journal.EntityID = 12;
            journal.BaseCurrency = "RMB";
            journal.ExchangeRate = (decimal)5.5;
            journal.BaseAmount = -8000;
            journal.SGDAmount = (decimal)-1590.45;
            journal.EntryUser.UserID = 34;

            Journal journal2 = new Journal();
            journal2.EntityID = 22;
            journal2.BaseCurrency = "RMB";
            journal2.ExchangeRate = (decimal)5.3;
            journal.BaseAmount = 3000;
            journal2.SGDAmount = (decimal)-545.45;
            journal2.EntryUser.UserID = 34;

            Journal journal3 = new Journal();
            journal3.EntityID = 24;
            journal3.BaseCurrency = "RMB";
            journal3.ExchangeRate = (decimal)4.75;
            journal3.BaseAmount = 3000;
            journal3.SGDAmount = (decimal)1052.53;
            journal3.EntryUser.UserID = 34;
            
            record.JournalCollection.Add(journal);
            record.JournalCollection.Add(journal2);
            record.JournalCollection.Add(journal3);

            Transfer tran = new Transfer();
            tran.Currency.CurrencyID = "RMB";
            tran.ExchangeRate = (decimal)5.03;
            tran.ToEntity.EntityID = 9 ;
            tran.BaseBefore = (decimal)-10000;
            tran.SGDBefore = (decimal)-1988.07;
            tran.BaseResult = (decimal)-18000;
            tran.SGDResult = (decimal)-8578.32;

            TransferDetailCollection tdc = new TransferDetailCollection();
            TransferDetail trand = new TransferDetail();
            trand.Entity.EntityID = 22;
            trand.BaseCurrency = "RMB";
            trand.ExchangeRate = (decimal)5.5;
            trand.BaseBefore = -3000;
            trand.SGDBefore = -3000;
            trand.BaseTransfer = -3000;
            trand.SGDTransfer = (decimal)-595.42;
          //  trand.ProfitOrLoss = (decimal)-50.97;
            trand.BaseResult = 0;
            trand.SGDResult = 0;
            tdc.Add(trand);
            TransferDetail trand2 = new TransferDetail();
            trand2.Entity.EntityID = 24;
            trand2.BaseCurrency = "RMB";
            trand2.ExchangeRate = (decimal)4.75;
            trand2.BaseBefore = -3000;
            trand2.SGDBefore = -3000;
            trand2.BaseTransfer = -3000;
            trand2.SGDTransfer = (decimal)-994.03;
          //  trand2.ProfitOrLoss = (decimal)-58.5;
            trand2.BaseResult = 0;
            trand2.SGDResult = 0;
            tdc.Add(trand2);
            tran.TransferDetailCollection = tdc;
            //tran.TransferDetailCollection.Add(trand);
            //tran.TransferDetailCollection.Add(trand2);

            DataEntryServiceClient _intclient = new DataEntryServiceClient();
            _intclient.InsertTransfer(record, tran);
        }

        protected void Button18_Click(object sender, EventArgs e)
        {
            DataEntryServiceClient _intclient = new DataEntryServiceClient();
            //Transaction _tr = new Transaction();
            //_tr.Amount = 3400;
            //_tr.Period.ID = 447;
            //_tr.FromEntity.EntityID = 37;
            //_tr.ToEntity.EntityID = 33;
            //_tr.Creator.UserID = 34;
            //_tr.IsPay = (int)IsPay.N;
            //_intclient.InsertTransaction(_tr);
            //_intclient.SetNotices(5, 26);
            //_intclient.SetConfirm(5, 26,448);
         //   _intclient.Updatetransaction(5, 26,33,37,3200);
        }

        protected void Button21_Click(object sender, EventArgs e)
        {
         //   mdlPopup.Show();
        }


      


    }
}