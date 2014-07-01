using Oleit.AS.Service.DataObject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Transactions;

namespace Oleit.AS.Service.DataService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "EntityAccess" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select EntityAccess.svc or EntityAccess.svc.cs at the Solution Explorer and start debugging.
    public class EntityAccess : IEntityAccess
    {
        string connectionString = ConfigurationManager.ConnectionStrings["AccountDataBase"].ConnectionString;

        public void RemoveRelation(int entityID, int targetEntityID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand _command = new SqlCommand();
                _command.Connection = connection;
                _command.CommandText = "SP_DEL_Table";
                SqlParameter _param = _command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Entity_R_Table";
                SqlParameter _param2 = _command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "1,2";
                SqlParameter _param3 = _command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = string.Format("{0},{1}", entityID, targetEntityID);
                _command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                _command.ExecuteNonQuery();
            }
        }

        public int Insert(Entity entity)
        {//OK
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand _command = new SqlCommand();
                _command.Connection = connection;
                _command.CommandText = "SP_INS_Table";
                SqlParameter _param = _command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Entity";
                SqlParameter _param2 = _command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "2,3,4,5,6,7,8,9,10";
                StringBuilder _content = new StringBuilder();
                _content.AppendFormat("{0}{1}",entity.ParentID.ToString(),",");
                _content.AppendFormat("{0}{1}", entity.EntityName, ",");
                _content.AppendFormat("{0}{1}", (int)entity.EntityType, ",");
                _content.AppendFormat("{0}{1}", (int)entity.SumType, ",");
                _content.AppendFormat("{0}{1}", entity.Currency.CurrencyID, ",");
                _content.AppendFormat("{0}{1}", entity.ExchangeRate, ",");
                _content.AppendFormat("{0}{1}", entity.IsAccount, ",");
                _content.AppendFormat("{0}{1}", entity.Enable, ",");
                _content.AppendFormat("{0}", entity.IsLastLevel);
                SqlParameter _param3 = _command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = _content.ToString();
                _command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = _command.ExecuteReader();
                int _entityID = -1;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        _entityID = Convert.ToInt32(reader["identity_ID"].ToString());
                    }
                }
                reader.Close();
                return _entityID;
            }
        }

        public void Insert(Account account)
        {//OK
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_INS_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Entity_Account";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "2,3,4,5,6,7,8,9,10,11,12,13,14,15,16";
                StringBuilder _content = new StringBuilder();
                _content.AppendFormat("{0}{1}", account.EntityID, ",");
                _content.AppendFormat("{0}{1}", account.Company, ",");
                _content.AppendFormat("{0}{1}", account.AccountName, ",");
                _content.AppendFormat("{0}{1}", account.Password, ",");
                _content.AppendFormat("{0}{1}", (int)account.AccountType, ",");
                _content.AppendFormat("{0}{1}", account.BettingLimit, ",");
                _content.AppendFormat("{0}{1}", (int)account.Status, ",");
                _content.AppendFormat("{0}{1}", account.Factor, ",");
                _content.AppendFormat("{0}{1}", account.Perbet, ",");
                _content.AppendFormat("{0}{1}", account.DateOpen, ",");
                _content.AppendFormat("{0}{1}", account.Personnel, ",");
                _content.AppendFormat("{0}{1}", account.IP, ",");
                _content.AppendFormat("{0}{1}", account.Odds, ",");
                _content.AppendFormat("{0}{1}", account.IssuesConditions, ",");
                _content.AppendFormat("{0}", account.RemarksAcc);    
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = _content.ToString();
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// New Cash Entity
        /// </summary>     
        public void Insert(Entity entity, CashEntity cashEntity)
        {//OK
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_INS_Entity_Cash";
                SqlParameter _param1 = command.Parameters.Add("@ParentID", System.Data.SqlDbType.Int);
                _param1.Value = entity.ParentID;
                SqlParameter _param2 = command.Parameters.Add("@Entity_Name", System.Data.SqlDbType.VarChar);
                _param2.Value = entity.EntityName;
                SqlParameter _param3 = command.Parameters.Add("@SumType", System.Data.SqlDbType.Int);
                _param3.Value = (int)entity.SumType;
                SqlParameter _param4 = command.Parameters.Add("@Entity_Type", System.Data.SqlDbType.Int);
                _param4.Value = (int)entity.EntityType;
                SqlParameter _param5 = command.Parameters.Add("@Currency", System.Data.SqlDbType.VarChar);
                _param5.Value = entity.Currency.CurrencyID;
                SqlParameter _param6 = command.Parameters.Add("@Exchange_Rate", System.Data.SqlDbType.Decimal);
                _param6.Value = entity.ExchangeRate;
                SqlParameter _param7 = command.Parameters.Add("@IsAccount", System.Data.SqlDbType.TinyInt);
                _param7.Value = entity.IsAccount;
                //SqlParameter _param8 = command.Parameters.Add("@Enable", System.Data.SqlDbType.TinyInt);
                //_param8.Value = entity.Enable;
                SqlParameter _param9 = command.Parameters.Add("@IsLastLevel", System.Data.SqlDbType.TinyInt);
                _param9.Value = entity.IsLastLevel;
                //Cash Entity Attribute
                SqlParameter _cashparam1 = command.Parameters.Add("@Contract_Number", System.Data.SqlDbType.VarChar);
                _cashparam1.Value = cashEntity.ContractNumber;
                SqlParameter _cashparam2 = command.Parameters.Add("@Tally_Name", System.Data.SqlDbType.VarChar);
                _cashparam2.Value = cashEntity.TallyName;
                SqlParameter _cashparam3 = command.Parameters.Add("@Tally_Number", System.Data.SqlDbType.VarChar);
                _cashparam3.Value = cashEntity.TallyNumber;
                SqlParameter _cashparam4 = command.Parameters.Add("@Settlement_Name", System.Data.SqlDbType.VarChar);
                _cashparam4.Value = cashEntity.SettlementName;
                SqlParameter _cashparam5 = command.Parameters.Add("@Settlement_Number", System.Data.SqlDbType.VarChar);
                _cashparam5.Value = cashEntity.SettlementNumber;
                SqlParameter _cashparam6 = command.Parameters.Add("@Recommended_By", System.Data.SqlDbType.VarChar);
                _cashparam6.Value = cashEntity.RecommendedBy;
                SqlParameter _cashparam7 = command.Parameters.Add("@Skype", System.Data.SqlDbType.VarChar);
                _cashparam7.Value = cashEntity.Skype;
                SqlParameter _cashparam8 = command.Parameters.Add("@QQ", System.Data.SqlDbType.VarChar);
                _cashparam8.Value = cashEntity.QQ;
                SqlParameter _cashparam9 = command.Parameters.Add("@Email", System.Data.SqlDbType.VarChar);
                _cashparam9.Value = cashEntity.Email;
                SqlParameter _cashparam10 = command.Parameters.Add("@Credit_Limit", System.Data.SqlDbType.Decimal);
                _cashparam10.Value = cashEntity.CreditLimit;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                command.ExecuteNonQuery();
            }
            
        }
        /// <summary>
        /// New Account Entity
        /// </summary>      
        public void Insert(Entity entity, Account account)
        {//OK
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_INS_Entity_Account";
                SqlParameter _param = command.Parameters.Add("@ParentID", System.Data.SqlDbType.Int);
                _param.Value = entity.ParentID;
                SqlParameter _param1 = command.Parameters.Add("@Entity_Name", System.Data.SqlDbType.VarChar);
                _param1.Value = entity.EntityName;
                SqlParameter _param2 = command.Parameters.Add("@SumType", System.Data.SqlDbType.Int);
                _param2.Value = (int)entity.SumType;
                SqlParameter _param3 = command.Parameters.Add("@Entity_Type", System.Data.SqlDbType.Int);
                _param3.Value = (int)entity.EntityType;
                SqlParameter _param4 = command.Parameters.Add("@Currency", System.Data.SqlDbType.VarChar);
                _param4.Value = entity.Currency.CurrencyID;
                SqlParameter _param5 = command.Parameters.Add("@Exchange_Rate", System.Data.SqlDbType.Decimal);
                _param5.Value = entity.ExchangeRate;
                SqlParameter _param6 = command.Parameters.Add("@IsAccount", System.Data.SqlDbType.TinyInt);
                _param6.Value = 1;
                //SqlParameter _param7 = command.Parameters.Add("@Enable", System.Data.SqlDbType.TinyInt);
                //_param7.Value = entity.Enable;
                SqlParameter _param8 = command.Parameters.Add("@IsLastLevel", System.Data.SqlDbType.TinyInt);
                _param8.Value = 1;
                //Account Entity Attribute
                //SqlParameter _accparam1 = command.Parameters.Add("@Entity_ID", System.Data.SqlDbType.Int);
                //_accparam1.Value = account.EntityID;
                SqlParameter _accparam2 = command.Parameters.Add("@Company", System.Data.SqlDbType.Int);
                _accparam2.Value = account.Company;
                SqlParameter _accparam3 = command.Parameters.Add("@Account_Name", System.Data.SqlDbType.VarChar);
                _accparam3.Value = account.AccountName;
                SqlParameter _accparam4 = command.Parameters.Add("@Password", System.Data.SqlDbType.VarChar);
                _accparam4.Value = account.Password;
                SqlParameter _accparam5 = command.Parameters.Add("@Account_Type", System.Data.SqlDbType.Int);
                _accparam5.Value = (int)account.AccountType;
                SqlParameter _accparam6 = command.Parameters.Add("@Betting_Limit", System.Data.SqlDbType.Decimal);
                _accparam6.Value = account.BettingLimit;
                SqlParameter _accparam7 = command.Parameters.Add("@Status", System.Data.SqlDbType.Int);
                _accparam7.Value = (int)account.Status;
                SqlParameter _accparam8 = command.Parameters.Add("@Factor", System.Data.SqlDbType.VarChar);
                _accparam8.Value = account.Factor;
                SqlParameter _accparam9 = command.Parameters.Add("@Perbet", System.Data.SqlDbType.Decimal);
                _accparam9.Value = account.Perbet;
                SqlParameter _accparam10 = command.Parameters.Add("@DateOpen", System.Data.SqlDbType.VarChar);
                _accparam10.Value = account.DateOpen;
                SqlParameter _accparam11 = command.Parameters.Add("@Personnel", System.Data.SqlDbType.VarChar);
                _accparam11.Value = account.Personnel;
                SqlParameter _accparam12 = command.Parameters.Add("@IP", System.Data.SqlDbType.VarChar);
                _accparam12.Value = account.IP;
                SqlParameter _accparam13 = command.Parameters.Add("@Odds", System.Data.SqlDbType.VarChar);
                _accparam13.Value = account.Odds;
                SqlParameter _accparam14 = command.Parameters.Add("@IssuesConditions", System.Data.SqlDbType.VarChar);
                _accparam14.Value = account.IssuesConditions;
                SqlParameter _accparam15 = command.Parameters.Add("@RemarksAcc", System.Data.SqlDbType.VarChar);
                _accparam15.Value = account.RemarksAcc;
                
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public EntityCollection Query()
        {//OK
            EntityCollection collection = new EntityCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Entity";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = "";
                SqlParameter _param4 = command.Parameters.Add("@order_by1", System.Data.SqlDbType.VarChar);
                _param4.Value = "1";
                SqlParameter _param5 = command.Parameters.Add("@order_by2", System.Data.SqlDbType.TinyInt);
                _param5.Value = 0;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Entity _entity = new Entity();
                        _entity.EntityID = Convert.ToInt32(reader["ID"]);                      
                        _entity.ParentID = Convert.ToInt32(reader["ParentID"]);
                        _entity.EntityName = reader["Entity_Name"].ToString();
                        _entity.EntityType = (EntityType)Convert.ToInt32(reader["Entity_Type"]);
                        _entity.SumType = (SumType)Convert.ToInt32(reader["SumType"]);
                        _entity.Currency.CurrencyID = reader["Currency"].ToString();
                        _entity.ExchangeRate = Convert.ToDecimal(reader["Exchange_Rate"]);
                        _entity.IsAccount = Convert.ToInt32(reader["IsAccount"]);
                        _entity.Enable = Convert.ToInt32(reader["Enable"]);
                        _entity.IsLastLevel = Convert.ToInt32(reader["IsLastLevel"]);

                        if (_entity.Enable == 1)
                        {
                            collection.Add(_entity);
                        }
                    }
                }
                reader.Close();
                return collection;
            }
        }

        public EntityCollection Query(int _entityID)
        {//OK
            EntityCollection collection = new EntityCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Entity";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "1";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = _entityID;
                SqlParameter _param4 = command.Parameters.Add("@order_by1", System.Data.SqlDbType.VarChar);
                _param4.Value = "1";
                SqlParameter _param5 = command.Parameters.Add("@order_by2", System.Data.SqlDbType.TinyInt);
                _param5.Value = 0;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Entity _entity = new Entity();
                        _entity.EntityID = Convert.ToInt32(reader["ID"]);                     
                        _entity.ParentID = Convert.ToInt32(reader["ParentID"]);
                        _entity.EntityName = reader["Entity_Name"].ToString();
                        _entity.EntityType = (EntityType)Convert.ToInt32(reader["Entity_Type"]);
                        _entity.SumType = (SumType)Convert.ToInt32(reader["SumType"]);
                        _entity.Currency.CurrencyID = reader["Currency"].ToString();
                        _entity.ExchangeRate = Convert.ToDecimal(reader["Exchange_Rate"]);
                        _entity.IsAccount = Convert.ToInt32(reader["IsAccount"]);
                        _entity.Enable = Convert.ToInt32(reader["Enable"]);
                        _entity.IsLastLevel = Convert.ToInt32(reader["IsLastLevel"]);
                        //if (_entity.Enable == 1)
                        //{
                        //    collection.Add(_entity);
                        //}
                        collection.Add(_entity);
                    }
                }
                reader.Close();
                return collection;
            }
        }

        /// <summary>
        /// Query two level up to this entity(Allocate,Position)
        /// </summary>
        /// <param name="_entityID"></param>
        /// <returns></returns>
        public EntityCollection QueryRelation(int _entityID)
        {
            EntityCollection collection = new EntityCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                EntityCollection _ent = new EntityCollection(Query(_entityID));
                Entity _entParent = QueryMainEntity(_entityID);

                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Entity";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "1";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = _entParent.EntityID;
                SqlParameter _param4 = command.Parameters.Add("@order_by1", System.Data.SqlDbType.VarChar);
                _param4.Value = "1";
                SqlParameter _param5 = command.Parameters.Add("@order_by2", System.Data.SqlDbType.TinyInt);
                _param5.Value = 0;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Entity _entity = new Entity();
                        _entity.EntityID = Convert.ToInt32(reader["ID"]);
                        _entity.ParentID = Convert.ToInt32(reader["ParentID"]);
                        _entity.EntityName = reader["Entity_Name"].ToString();
                        _entity.EntityType = (EntityType)Convert.ToInt32(reader["Entity_Type"]);
                        _entity.SumType = (SumType)Convert.ToInt32(reader["SumType"]);
                        _entity.Currency.CurrencyID = reader["Currency"].ToString();
                        _entity.ExchangeRate = Convert.ToDecimal(reader["Exchange_Rate"]);
                        _entity.IsAccount = Convert.ToInt32(reader["IsAccount"]);
                        _entity.Enable = Convert.ToInt32(reader["Enable"]);
                        _entity.IsLastLevel = Convert.ToInt32(reader["IsLastLevel"]);
                        collection.Add(_entity);
                    }
                }
                reader.Close();
                return collection;
            }
        }

        public EntityCollection Query(string _entityName)
        {//OK
            EntityCollection collection = new EntityCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Entity";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "3";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = _entityName;
                SqlParameter _param4 = command.Parameters.Add("@order_by1", System.Data.SqlDbType.VarChar);
                _param4.Value = "1";
                SqlParameter _param5 = command.Parameters.Add("@order_by2", System.Data.SqlDbType.TinyInt);
                _param5.Value = 0;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Entity _entity = new Entity();
                        _entity.EntityID = Convert.ToInt32(reader["ID"]);                        
                        _entity.ParentID = Convert.ToInt32(reader["ParentID"]);
                        _entity.EntityName = reader["Entity_Name"].ToString();
                        _entity.EntityType = (EntityType)Convert.ToInt32(reader["Entity_Type"]);
                        _entity.SumType = (SumType)Convert.ToInt32(reader["SumType"]);
                        _entity.Currency.CurrencyID = reader["Currency"].ToString();
                        _entity.ExchangeRate = Convert.ToDecimal(reader["Exchange_Rate"]);
                        _entity.IsAccount = Convert.ToInt32(reader["IsAccount"]);
                        _entity.Enable = Convert.ToInt32(reader["Enable"]);
                        _entity.IsLastLevel = Convert.ToInt32(reader["IsLastLevel"]);

                        if (_entity.Enable == 1)
                        {
                            collection.Add(_entity);
                        }
                    }
                }
                reader.Close();
                return collection;
            }
        }

        public EntityCollection QueryMainCash()
        {//OK
            EntityCollection collection = new EntityCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Entity";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "2,4";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = "0,2";
                SqlParameter _param4 = command.Parameters.Add("@order_by1", System.Data.SqlDbType.VarChar);
                _param4.Value = "1";
                SqlParameter _param5 = command.Parameters.Add("@order_by2", System.Data.SqlDbType.TinyInt);
                _param5.Value = 0;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Entity _entity = new Entity();
                        _entity.EntityID = Convert.ToInt32(reader["ID"]);
                        _entity.ParentID = Convert.ToInt32(reader["ParentID"]);
                        _entity.EntityName = reader["Entity_Name"].ToString();
                        _entity.EntityType = (EntityType)Convert.ToInt32(reader["Entity_Type"]);
                        _entity.SumType = (SumType)Convert.ToInt32(reader["SumType"]);
                        _entity.Currency.CurrencyID = reader["Currency"].ToString();
                        _entity.ExchangeRate = Convert.ToDecimal(reader["Exchange_Rate"]);
                        _entity.IsAccount = Convert.ToInt32(reader["IsAccount"]);
                        _entity.Enable = Convert.ToInt32(reader["Enable"]);
                        _entity.IsLastLevel = Convert.ToInt32(reader["IsLastLevel"]);

                        if (_entity.Enable == 1)
                        {
                            collection.Add(_entity);
                        }
                    }
                }
                reader.Close();
                return collection;
            }
        }

        public CashEntityCollection QueryCashEntity()
        {//TODO
            CashEntityCollection collection = new CashEntityCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Entity_Cash";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = "";
                SqlParameter _param4 = command.Parameters.Add("@order_by1", System.Data.SqlDbType.VarChar);
                _param4.Value = "1";
                SqlParameter _param5 = command.Parameters.Add("@order_by2", System.Data.SqlDbType.TinyInt);
                _param5.Value = 0;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        CashEntity _entity = new CashEntity();
                        _entity.EntityID = Convert.ToInt32(reader["Entity_ID"]);                       
                        _entity.ContractNumber = reader["Contract_Number"].ToString();
                        _entity.TallyName = reader["Tally_Name"].ToString();
                        _entity.TallyNumber = reader["Tally_Number"].ToString();
                        _entity.SettlementName = reader["Settlement_Name"].ToString();
                        _entity.SettlementNumber= reader["Settlement_Number"].ToString();
                        _entity.RecommendedBy = reader["Recommended_By"].ToString();
                        _entity.Skype = reader["Skype"].ToString();
                        _entity.QQ = reader["QQ"].ToString();
                        _entity.Email = reader["Email"].ToString();
                        _entity.CreditLimit = Convert.ToDecimal(reader["Credit_Limit"]);                       
                        collection.Add(_entity);
                    }
                }
                reader.Close();
                return collection;
            }
        }

        public CashEntityCollection QueryCashEntity(int _entityID)
        {//TODO
            CashEntityCollection collection = new CashEntityCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Entity_Cash";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "1";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = _entityID;
                SqlParameter _param4 = command.Parameters.Add("@order_by1", System.Data.SqlDbType.VarChar);
                _param4.Value = "1";
                SqlParameter _param5 = command.Parameters.Add("@order_by2", System.Data.SqlDbType.TinyInt);
                _param5.Value = 0;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        CashEntity _entity = new CashEntity();
                        _entity.EntityID = Convert.ToInt32(reader["Entity_ID"]);                        
                        _entity.ContractNumber = reader["Contract_Number"].ToString();
                        _entity.TallyName = reader["Tally_Name"].ToString();
                        _entity.TallyNumber = reader["Tally_Number"].ToString();
                        _entity.SettlementName = reader["Settlement_Name"].ToString();
                        _entity.SettlementNumber = reader["Settlement_Number"].ToString();
                        _entity.RecommendedBy = reader["Recommended_By"].ToString();
                        _entity.Skype = reader["Skype"].ToString();
                        _entity.QQ = reader["QQ"].ToString();
                        _entity.Email = reader["Email"].ToString();
                        _entity.CreditLimit = Convert.ToDecimal(reader["Credit_Limit"]);
                        collection.Add(_entity);
                    }
                }
                reader.Close();
                return collection;
            }
        }

        public CashEntityCollection QueryCashEntity(string _contractNumber)
        {//TODO
            CashEntityCollection collection = new CashEntityCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Entity_Cash";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "2";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = _contractNumber;
                SqlParameter _param4 = command.Parameters.Add("@order_by1", System.Data.SqlDbType.VarChar);
                _param4.Value = "1";
                SqlParameter _param5 = command.Parameters.Add("@order_by2", System.Data.SqlDbType.TinyInt);
                _param5.Value = 0;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        CashEntity _entity = new CashEntity();
                        _entity.EntityID = Convert.ToInt32(reader["Entity_ID"]);                       
                        _entity.ContractNumber = reader["Contract_Number"].ToString();
                        _entity.TallyName = reader["Tally_Name"].ToString();
                        _entity.TallyNumber = reader["Tally_Number"].ToString();
                        _entity.SettlementName = reader["Settlement_Name"].ToString();
                        _entity.SettlementNumber = reader["Settlement_Number"].ToString();
                        _entity.RecommendedBy = reader["Recommended_By"].ToString();
                        _entity.Skype = reader["Skype"].ToString();
                        _entity.QQ = reader["QQ"].ToString();
                        _entity.Email = reader["Email"].ToString();
                        _entity.CreditLimit = Convert.ToDecimal(reader["Credit_Limit"]);
                        collection.Add(_entity);
                    }
                }
                reader.Close();
                return collection;
            }
        }

        public AccountCollection QueryAccount()
        {//TODO
            AccountCollection collection = new AccountCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Entity_Account";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = "";
                SqlParameter _param4 = command.Parameters.Add("@order_by1", System.Data.SqlDbType.VarChar);
                _param4.Value = "1";
                SqlParameter _param5 = command.Parameters.Add("@order_by2", System.Data.SqlDbType.TinyInt);
                _param5.Value = 0;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Account _account = new Account();                        
                        _account.EntityID = Convert.ToInt32(reader["Entity_ID"]);
                        _account.Company = Convert.ToInt32(reader["Company"]);
                        _account.AccountName = reader["Account_Name"].ToString();
                        _account.Password = reader["Password"].ToString();
                        _account.AccountType = (AccountType)Convert.ToInt32(reader["Account_Type"]);
                        _account.BettingLimit = Convert.ToDecimal(reader["Betting_Limit"]);
                        _account.Status = (Status)Convert.ToDecimal(reader["Status"]);
                        _account.Factor = reader["Factor"].ToString();
                        _account.DateOpen = reader["DateOpen"].ToString();
                        _account.Personnel = reader["Personnel"].ToString();
                        _account.IP = reader["IP"].ToString();
                        _account.Odds = reader["Odds"].ToString();
                        _account.IssuesConditions = reader["IssuesConditions"].ToString();
                        _account.RemarksAcc = reader["RemarksAcc"].ToString();
                        if (!reader["Perbet"].ToString().Equals(""))
                            _account.Perbet = Convert.ToDecimal(reader["Perbet"]);
                        else
                            _account.Perbet = 0;
                        collection.Add(_account);
                    }
                }
                reader.Close();
                return collection;
            }
        }

        public AccountCollection QueryAccount(int _entityID)
        {
            AccountCollection collection = new AccountCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Entity_Account";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "2";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = _entityID;
                SqlParameter _param4 = command.Parameters.Add("@order_by1", System.Data.SqlDbType.VarChar);
                _param4.Value = "1";
                SqlParameter _param5 = command.Parameters.Add("@order_by2", System.Data.SqlDbType.TinyInt);
                _param5.Value = 0;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Account _account = new Account();
                        _account.ID = Convert.ToInt32(reader["ID"]);
                        _account.EntityID = Convert.ToInt32(reader["Entity_ID"]);
                        _account.Company = Convert.ToInt32(reader["Company"]);
                        _account.AccountName = reader["Account_Name"].ToString();
                        _account.Password = reader["Password"].ToString();
                        _account.AccountType = (AccountType)Convert.ToInt32(reader["Account_Type"]);
                        _account.BettingLimit = Convert.ToDecimal(reader["Betting_Limit"]);
                        _account.Status = (Status)Convert.ToInt32(reader["Status"]);
                        MLJRecordAccess _mlj = new MLJRecordAccess();
                        _account.Color = new StatusColorCollection(_mlj.QueryOneStatusColor(Convert.ToInt32(reader["Status"])))[0].MLJColor;
                        _account.Factor = reader["Factor"].ToString();
                        _account.DateOpen = reader["DateOpen"].ToString();
                        _account.Personnel = reader["Personnel"].ToString();
                        _account.IP = reader["IP"].ToString();
                        _account.Odds = reader["Odds"].ToString();
                        _account.IssuesConditions = reader["IssuesConditions"].ToString();
                        _account.RemarksAcc = reader["RemarksAcc"].ToString();
                        if (!reader["Perbet"].ToString().Equals(""))
                            _account.Perbet = Convert.ToDecimal(reader["Perbet"]);
                        else
                            _account.Perbet = 0;

                        collection.Add(_account);
                    }
                }
                reader.Close();
                return collection;
            }
        }

        public AccountCollection QueryAccount(string _accountName)
        {
            AccountCollection collection = new AccountCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Entity_Account";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "4";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = _accountName;
                SqlParameter _param4 = command.Parameters.Add("@order_by1", System.Data.SqlDbType.VarChar);
                _param4.Value = "1";
                SqlParameter _param5 = command.Parameters.Add("@order_by2", System.Data.SqlDbType.TinyInt);
                _param5.Value = 0;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Account _account = new Account();
                        _account.ID = Convert.ToInt32(reader["ID"]);
                        _account.EntityID = Convert.ToInt32(reader["Entity_ID"]);
                        _account.Company = Convert.ToInt32(reader["Company"]);
                        _account.AccountName = reader["Account_Name"].ToString();
                        _account.Password = reader["Password"].ToString();
                        _account.AccountType = (AccountType)Convert.ToInt32(reader["Account_Type"]);
                        _account.BettingLimit = Convert.ToDecimal(reader["Betting_Limit"]);
                        _account.Factor = reader["Factor"].ToString();
                        _account.DateOpen = reader["DateOpen"].ToString();
                        _account.Personnel = reader["Personnel"].ToString();
                        _account.IP = reader["IP"].ToString();
                        _account.Odds = reader["Odds"].ToString();
                        _account.IssuesConditions = reader["IssuesConditions"].ToString();
                        _account.RemarksAcc = reader["RemarksAcc"].ToString();
                        if (!reader["Perbet"].ToString().Equals(""))
                            _account.Perbet = Convert.ToDecimal(reader["Perbet"]);
                        else
                            _account.Perbet = 0;
                        _account.Status = (Status)Convert.ToDecimal(reader["Status"]);
                        collection.Add(_account);
                    }
                }
                reader.Close();
                return collection;
            }
        }

        public EntityCollection QuerySumTypeEntity(SumType _sumType)
        {
            EntityCollection collection = new EntityCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Entity";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "5";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = (int)_sumType;
                SqlParameter _param4 = command.Parameters.Add("@order_by1", System.Data.SqlDbType.VarChar);
                _param4.Value = "1";
                SqlParameter _param5 = command.Parameters.Add("@order_by2", System.Data.SqlDbType.TinyInt);
                _param5.Value = 0;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Entity _entity = new Entity();
                        _entity.EntityID = Convert.ToInt32(reader["ID"]);
                        _entity.ParentID = Convert.ToInt32(reader["ParentID"]);
                        _entity.EntityName = reader["Entity_Name"].ToString();
                        _entity.EntityType = (EntityType)Convert.ToInt32(reader["Entity_Type"]);
                        _entity.SumType = (SumType)Convert.ToInt32(reader["SumType"]);
                        _entity.Currency.CurrencyID = reader["Currency"].ToString();
                        _entity.ExchangeRate = Convert.ToDecimal(reader["Exchange_Rate"]);
                        _entity.IsAccount = Convert.ToInt32(reader["IsAccount"]);
                        _entity.Enable = Convert.ToInt32(reader["Enable"]);
                        _entity.IsLastLevel = Convert.ToInt32(reader["IsLastLevel"]);

                        if (_entity.Enable == 1)
                        {
                            collection.Add(_entity);
                        }
                    }
                }
                reader.Close();
                return collection;
            }
        }

        public EntityCollection Query(EntityType _type)
        {
            EntityCollection collection = new EntityCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Entity";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "4";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = (int)_type;
                SqlParameter _param4 = command.Parameters.Add("@order_by1", System.Data.SqlDbType.VarChar);
                _param4.Value = "1";
                SqlParameter _param5 = command.Parameters.Add("@order_by2", System.Data.SqlDbType.TinyInt);
                _param5.Value = 0;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Entity _entity = new Entity();
                        _entity.EntityID = Convert.ToInt32(reader["ID"]);
                        _entity.ParentID = Convert.ToInt32(reader["ParentID"]);
                        _entity.EntityName = reader["Entity_Name"].ToString();
                        _entity.EntityType = (EntityType)Convert.ToInt32(reader["Entity_Type"]);
                        _entity.SumType = (SumType)Convert.ToInt32(reader["SumType"]);
                        _entity.Currency.CurrencyID = reader["Currency"].ToString();
                        _entity.ExchangeRate = Convert.ToDecimal(reader["Exchange_Rate"]);
                        _entity.IsAccount = Convert.ToInt32(reader["IsAccount"]);
                        _entity.Enable = Convert.ToInt32(reader["Enable"]);
                        _entity.IsLastLevel = Convert.ToInt32(reader["IsLastLevel"]);

                        if (_entity.Enable == 1)
                        {
                            collection.Add(_entity);
                        }
                    }
                }
                reader.Close();
                return collection;
            }
        }

        public EntityCollection QueryMLJEntity()
        {
            EntityCollection collection = new EntityCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Entity";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "4,10";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = 5+","+1;
                SqlParameter _param4 = command.Parameters.Add("@order_by1", System.Data.SqlDbType.VarChar);
                _param4.Value = "1";
                SqlParameter _param5 = command.Parameters.Add("@order_by2", System.Data.SqlDbType.TinyInt);
                _param5.Value = 0;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Entity _entity = new Entity();
                        _entity.EntityID = Convert.ToInt32(reader["ID"]);
                        _entity.ParentID = Convert.ToInt32(reader["ParentID"]);
                        _entity.EntityName = reader["Entity_Name"].ToString();
                        _entity.EntityType = (EntityType)Convert.ToInt32(reader["Entity_Type"]);
                        _entity.SumType = (SumType)Convert.ToInt32(reader["SumType"]);
                        _entity.Currency.CurrencyID = reader["Currency"].ToString();
                        _entity.ExchangeRate = Convert.ToDecimal(reader["Exchange_Rate"]);
                        _entity.IsAccount = Convert.ToInt32(reader["IsAccount"]);
                        _entity.Enable = Convert.ToInt32(reader["Enable"]);
                        _entity.IsLastLevel = Convert.ToInt32(reader["IsLastLevel"]);

                        if (_entity.Enable == 1)
                        {
                            collection.Add(_entity);
                        }
                    }
                }
                reader.Close();
                return collection;
            }
        }

        public EntityCollection QuerySubEntitiesList(int entityID)
        {
            EntityCollection _collection = new EntityCollection(QueryAllSub(entityID));
            foreach (Entity _entity in _collection)
            {
                _subcollection.Add(_entity);
                QuerySubEntitiesList(_entity.EntityID);
            }
            return _subcollection;
        }

        EntityCollection _relationCollection = new EntityCollection();
        public EntityCollection QueryRelationEntities()
        {
            EntityCollection _collection = new EntityCollection(Query().Where(x => x.ParentID == 0));

            foreach (Entity _entity in _collection)
            {
                _entity.SubEntities = QueryTransactionList(_entity.EntityID);
                _relationCollection.Add(_entity);
            }
            return _relationCollection;
        }

        
        public EntityCollection QueryTransactionList(int entityID)
        {
            EntityCollection _transactionCollection = new EntityCollection();
            EntityCollection _collection = new EntityCollection(QueryAllSub(entityID));
            foreach (Entity _entity in _collection)
            {
                if(_entity.SumType==SumType.Transaction)
                    _transactionCollection.Add(_entity);
                QueryTransactionList(_entity.EntityID);
            }
            return _transactionCollection;
        } 

        /// <summary>
        /// Don't use this class 
        /// </summary>
        /// <param name="_entityID"></param>
        /// <param name="_sumType"></param>
        /// <returns></returns>
        public EntityCollection QuerySumTypeEntity(int _entityID, SumType _sumType)
        {
            return null;
        }

        EntityCollection _subcollection = new EntityCollection();
        bool IsAddMLJ = false;

        public EntityCollection QueryAllMLJ(int entityID)
        {            
            EntityCollection _collection = new EntityCollection(QueryAllSub(entityID));
            foreach (Entity _entity in _collection)
            {
                if (_entity.IsAccount.Equals(1))
                {
                    _subcollection.Add(_entity);
                }  
                QueryAllMLJ(_entity.EntityID);
            }
            if (!IsAddMLJ)
            {
                foreach (Entity _entity in QueryMLJEntity())
                {
                    _subcollection.Add(_entity);
                    IsAddMLJ = true;
                }
            }
            return _subcollection;
        }

        EntityCollection _subentitycollection = new EntityCollection();        

        public EntityCollection QueryAllSubEntity(int entityID)
        {
            EntityCollection _collection = new EntityCollection(QueryAllSub(entityID));
            foreach (Entity _entity in _collection)
            {
                _subentitycollection.Add(_entity);
                QueryAllSubEntity(_entity.EntityID);
            }
            return _subentitycollection;
        }

        public EntityCollection QueryParentSubTotalEntity(int entityID)
        {
            EntityCollection _entity = Query(entityID);
            if (_entity[0].SumType !=SumType.Subtotal)
                _entity = QueryParentSubTotalEntity(_entity[0].ParentID);
            return _entity;
        }

        public EntityCollection QueryParentTransactionEntity(int entityID)
        {
            EntityCollection _entity = Query(entityID);
            if (_entity[0].SumType != SumType.Transaction)
                _entity = QueryParentTransactionEntity(_entity[0].ParentID);
            return _entity;
        }

        public Entity QueryMainEntity(int entityID)
        {
            Entity _entity = Query(entityID)[0];
            if (_entity.ParentID != 0)
               _entity= QueryMainEntity(_entity.ParentID);
            return _entity;
        }

        public EntityCollection QueryAllSub(int entityID)
        {
            EntityCollection collection = new EntityCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Entity";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "2";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = entityID;
                SqlParameter _param4 = command.Parameters.Add("@order_by1", System.Data.SqlDbType.VarChar);
                _param4.Value = "1";
                SqlParameter _param5 = command.Parameters.Add("@order_by2", System.Data.SqlDbType.TinyInt);
                _param5.Value = 0;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Entity _entity = new Entity();
                        _entity.EntityID = Convert.ToInt32(reader["ID"]);
                        _entity.ParentID = Convert.ToInt32(reader["ParentID"]);
                        _entity.EntityName = reader["Entity_Name"].ToString();
                        _entity.EntityType = (EntityType)Convert.ToInt32(reader["Entity_Type"]);
                        _entity.SumType = (SumType)Convert.ToInt32(reader["SumType"]);
                        _entity.Currency.CurrencyID = reader["Currency"].ToString();
                        _entity.ExchangeRate = Convert.ToDecimal(reader["Exchange_Rate"]);
                        _entity.IsAccount = Convert.ToInt32(reader["IsAccount"]);
                        _entity.Enable = Convert.ToInt32(reader["Enable"]);
                        _entity.IsLastLevel = Convert.ToInt32(reader["IsLastLevel"]);

                        if (_entity.Enable == 1)
                        {
                            collection.Add(_entity);
                        }
                    }
                }
                reader.Close();
                return collection;
            }
        }

        public EntityCollection QueryAllSub(string entityName)
        {
            EntityCollection collection = new EntityCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Entity";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "3";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = entityName;
                SqlParameter _param4 = command.Parameters.Add("@order_by1", System.Data.SqlDbType.VarChar);
                _param4.Value = "1";
                SqlParameter _param5 = command.Parameters.Add("@order_by2", System.Data.SqlDbType.TinyInt);
                _param5.Value = 0;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Entity _entity = new Entity();
                        _entity.EntityID = Convert.ToInt32(reader["ID"]);
                        _entity.ParentID = Convert.ToInt32(reader["ParentID"]);
                        _entity.EntityName = reader["Entity_Name"].ToString();
                        _entity.EntityType = (EntityType)Convert.ToInt32(reader["Entity_Type"]);
                        _entity.SumType = (SumType)Convert.ToInt32(reader["SumType"]);
                        _entity.Currency.CurrencyID = reader["Currency"].ToString();
                        _entity.ExchangeRate = Convert.ToDecimal(reader["Exchange_Rate"]);
                        _entity.IsAccount = Convert.ToInt32(reader["IsAccount"]);
                        _entity.Enable = Convert.ToInt32(reader["Enable"]);
                        _entity.IsLastLevel = Convert.ToInt32(reader["IsLastLevel"]);

                        if (_entity.Enable == 1)
                        {
                            collection.Add(_entity);
                        }
                    }
                }
                reader.Close();
                return collection;
            }
        }

        public void Update(Entity entity)
        {//OK
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_UPD_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Entity";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "2,3,4,5,6,7,8,9,10";
                StringBuilder _content = new StringBuilder();
                _content.AppendFormat("{0}{1}",entity.ParentID.ToString() , ",");
                _content.AppendFormat("{0}{1}", entity.EntityName, ",");
                _content.AppendFormat("{0}{1}", (int)entity.EntityType, ",");
                _content.AppendFormat("{0}{1}", (int)entity.SumType, ",");
                _content.AppendFormat("{0}{1}", entity.Currency.CurrencyID, ",");
                _content.AppendFormat("{0}{1}", entity.ExchangeRate, ",");
                _content.AppendFormat("{0}{1}", entity.IsAccount, ",");
                _content.AppendFormat("{0}{1}", entity.Enable, ",");
                _content.AppendFormat("{0}", entity.IsLastLevel);
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = _content.ToString();
                SqlParameter _param4 = command.Parameters.Add("@value4", System.Data.SqlDbType.VarChar);
                _param4.Value = "1";
                SqlParameter _param5 = command.Parameters.Add("@value5", System.Data.SqlDbType.VarChar);
                _param5.Value = entity.EntityID;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Update(Entity entity, CashEntity cashEntity)
        {//OK
            using (SqlConnection _connection1 = new SqlConnection(connectionString))
            {
                SqlCommand _command = new SqlCommand();
                _command.Connection = _connection1;
                _command.CommandText = "SP_UPD_Entity_Cash";
                SqlParameter _param1 = _command.Parameters.Add("@ParentID", System.Data.SqlDbType.Int);
                _param1.Value = entity.ParentID;
                SqlParameter _param2 = _command.Parameters.Add("@Entity_Name", System.Data.SqlDbType.VarChar);
                _param2.Value = entity.EntityName;
                SqlParameter _param3 = _command.Parameters.Add("@SumType", System.Data.SqlDbType.Int);
                _param3.Value = (int)entity.SumType;
                SqlParameter _param4 = _command.Parameters.Add("@Entity_Type", System.Data.SqlDbType.Int);
                _param4.Value = (int)entity.EntityType;
                SqlParameter _param5 = _command.Parameters.Add("@Currency", System.Data.SqlDbType.VarChar);
                _param5.Value = entity.Currency.CurrencyID;
                SqlParameter _param6 = _command.Parameters.Add("@Exchange_Rate", System.Data.SqlDbType.Decimal);
                _param6.Value = entity.ExchangeRate;
                SqlParameter _param7 = _command.Parameters.Add("@IsAccount", System.Data.SqlDbType.TinyInt);
                _param7.Value = entity.IsAccount;
                SqlParameter _param8 = _command.Parameters.Add("@Enable", System.Data.SqlDbType.TinyInt);
                _param8.Value = entity.Enable;
                SqlParameter _param9 = _command.Parameters.Add("@IsLastLevel", System.Data.SqlDbType.TinyInt);
                _param9.Value = entity.IsLastLevel;
                SqlParameter _param10 = _command.Parameters.Add("@Entity_ID", System.Data.SqlDbType.Int);
                _param10.Value = entity.EntityID;
                //Cash Entity Attribute
                SqlParameter _cashparam1 = _command.Parameters.Add("@Contract_Number", System.Data.SqlDbType.VarChar);
                _cashparam1.Value = cashEntity.ContractNumber;
                SqlParameter _cashparam2 = _command.Parameters.Add("@Tally_Name", System.Data.SqlDbType.VarChar);
                _cashparam2.Value = cashEntity.TallyName;
                SqlParameter _cashparam3 = _command.Parameters.Add("@Tally_Number", System.Data.SqlDbType.VarChar);
                _cashparam3.Value = cashEntity.TallyNumber;
                SqlParameter _cashparam4 = _command.Parameters.Add("@Settlement_Name", System.Data.SqlDbType.VarChar);
                _cashparam4.Value = cashEntity.SettlementName;
                SqlParameter _cashparam5 = _command.Parameters.Add("@Settlement_Number", System.Data.SqlDbType.VarChar);
                _cashparam5.Value = cashEntity.SettlementNumber;
                SqlParameter _cashparam6 = _command.Parameters.Add("@Recommended_By", System.Data.SqlDbType.VarChar);
                _cashparam6.Value = cashEntity.RecommendedBy;
                SqlParameter _cashparam7 = _command.Parameters.Add("@Skype", System.Data.SqlDbType.VarChar);
                _cashparam7.Value = cashEntity.Skype;
                SqlParameter _cashparam8 = _command.Parameters.Add("@QQ", System.Data.SqlDbType.VarChar);
                _cashparam8.Value = cashEntity.QQ;
                SqlParameter _cashparam9 = _command.Parameters.Add("@Email", System.Data.SqlDbType.VarChar);
                _cashparam9.Value = cashEntity.Email;
                SqlParameter _cashparam10 = _command.Parameters.Add("@Credit_Limit", System.Data.SqlDbType.Decimal);
                _cashparam10.Value = cashEntity.CreditLimit;             
                _command.CommandType = System.Data.CommandType.StoredProcedure;
                _connection1.Open();
                _command.ExecuteNonQuery();
            }
            
        }

        /// <summary>
        /// Update Account Entity
        /// </summary>      
        public void Update(int userID, Entity entity, Account account)
        {//OK
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_UPD_Entity_Account";
                SqlParameter _param1 = command.Parameters.Add("@Entity_ID", System.Data.SqlDbType.Int);
                _param1.Value = entity.EntityID;
                SqlParameter _param2 = command.Parameters.Add("@ParentID", System.Data.SqlDbType.Int);
                _param2.Value = entity.ParentID;
                SqlParameter _param3 = command.Parameters.Add("@Entity_Name", System.Data.SqlDbType.VarChar);
                _param3.Value = entity.EntityName;
                SqlParameter _param4 = command.Parameters.Add("@SumType", System.Data.SqlDbType.Int);
                _param4.Value = (int)entity.SumType;
                SqlParameter _param5 = command.Parameters.Add("@Entity_Type", System.Data.SqlDbType.Int);
                _param5.Value = (int)entity.EntityType;
                SqlParameter _param6 = command.Parameters.Add("@Currency", System.Data.SqlDbType.VarChar);
                _param6.Value = entity.Currency.CurrencyID;
                SqlParameter _param7 = command.Parameters.Add("@Exchange_Rate", System.Data.SqlDbType.Decimal);
                _param7.Value = entity.ExchangeRate;
                SqlParameter _param8 = command.Parameters.Add("@IsAccount", System.Data.SqlDbType.TinyInt);
                _param8.Value = entity.IsAccount;
                SqlParameter _param9 = command.Parameters.Add("@Enable", System.Data.SqlDbType.TinyInt);
                _param9.Value = entity.Enable;
                SqlParameter _param10 = command.Parameters.Add("@IsLastLevel", System.Data.SqlDbType.TinyInt);
                _param10.Value = entity.IsLastLevel;
                //Account Entity Attribute
                SqlParameter _accparam1 = command.Parameters.Add("@Entity_Account_ID", System.Data.SqlDbType.Int);
                _accparam1.Value = account.ID;
                SqlParameter _accparam2 = command.Parameters.Add("@Company", System.Data.SqlDbType.Int);
                _accparam2.Value = account.Company;
                SqlParameter _accparam3 = command.Parameters.Add("@Account_Name", System.Data.SqlDbType.VarChar);
                _accparam3.Value = account.AccountName;
                SqlParameter _accparam4 = command.Parameters.Add("@Password", System.Data.SqlDbType.VarChar);
                _accparam4.Value = account.Password;
                SqlParameter _accparam5 = command.Parameters.Add("@Account_Type", System.Data.SqlDbType.Int);
                _accparam5.Value = (int)account.AccountType;
                SqlParameter _accparam6 = command.Parameters.Add("@Betting_Limit", System.Data.SqlDbType.Decimal);
                _accparam6.Value = account.BettingLimit;
                SqlParameter _accparam7 = command.Parameters.Add("@Status", System.Data.SqlDbType.Int);
                _accparam7.Value = (int)account.Status;

                SqlParameter _accparam8 = command.Parameters.Add("@User_ID", System.Data.SqlDbType.Int);
                _accparam8.Value = userID;

                SqlParameter _accparam9 = command.Parameters.Add("@Factor", System.Data.SqlDbType.VarChar);
                _accparam9.Value = account.Factor;
                SqlParameter _accparam10 = command.Parameters.Add("@Perbet", System.Data.SqlDbType.Decimal);
                _accparam10.Value = account.Perbet;
                SqlParameter _accparam11 = command.Parameters.Add("@DateOpen", System.Data.SqlDbType.VarChar);
                _accparam11.Value = account.DateOpen;
                SqlParameter _accparam12 = command.Parameters.Add("@Personnel", System.Data.SqlDbType.VarChar);
                _accparam12.Value = account.Personnel;
                SqlParameter _accparam13 = command.Parameters.Add("@IP", System.Data.SqlDbType.VarChar);
                _accparam13.Value = account.IP;
                SqlParameter _accparam14 = command.Parameters.Add("@Odds", System.Data.SqlDbType.VarChar);
                _accparam14.Value = account.Odds;
                SqlParameter _accparam15 = command.Parameters.Add("@IssuesConditions", System.Data.SqlDbType.VarChar);
                _accparam15.Value = account.IssuesConditions;
                SqlParameter _accparam16 = command.Parameters.Add("@RemarksAcc", System.Data.SqlDbType.VarChar);
                _accparam16.Value = account.RemarksAcc;

                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                command.ExecuteNonQuery();
            
            }
        }

        public void Disable(int entityID)
        {//OK
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_UPD_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Entity";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "9";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = 0;
                SqlParameter _param4 = command.Parameters.Add("@value4", System.Data.SqlDbType.VarChar);
                _param4.Value = "1";
                SqlParameter _param5 = command.Parameters.Add("@value5", System.Data.SqlDbType.VarChar);
                _param5.Value = entityID;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Enable(int entityID)
        {//OK
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_UPD_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Entity";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "9";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = 1;
                SqlParameter _param4 = command.Parameters.Add("@value4", System.Data.SqlDbType.VarChar);
                _param4.Value = "1";
                SqlParameter _param5 = command.Parameters.Add("@value5", System.Data.SqlDbType.VarChar);
                _param5.Value = entityID;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void SetRate(int entityID, decimal exchangeRate,Currency currency)
        {//OK
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_UPD_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Entity";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "6,7";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = currency.CurrencyID+","+ exchangeRate;
                SqlParameter _param4 = command.Parameters.Add("@value4", System.Data.SqlDbType.VarChar);
                _param4.Value = "1";
                SqlParameter _param5 = command.Parameters.Add("@value5", System.Data.SqlDbType.VarChar);
                _param5.Value = entityID;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void ChangeStatus(User user, int entityID, DataObject.Status status)
        {//OK
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_INS_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Entity_Account_Status";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "1,2,4";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = entityID + "," + (int)status + "," + user.UserID;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void SetAttributes(User user, int entityID, Account account)
        {//TODO
            //using (TransactionScope _ts = new TransactionScope())
            //{
            //    using (SqlConnection connection = new SqlConnection(connectionString))
            //    {
            //        SqlCommand command = new SqlCommand();
            //        command.Connection = connection;
            //        command.CommandText = "SP_Frank_TEST";
            //        SqlParameter _accparam1 = command.Parameters.Add("@Entity_ID", System.Data.SqlDbType.Int);
            //        _accparam1.Value = account.EntityID;
            //        SqlParameter _accparam2 = command.Parameters.Add("@Company", System.Data.SqlDbType.Int);
            //        _accparam2.Value = account.Company;
            //        SqlParameter _accparam3 = command.Parameters.Add("@Account_Name", System.Data.SqlDbType.VarChar);
            //        _accparam3.Value = account.AccountName;
            //        SqlParameter _accparam4 = command.Parameters.Add("@Password", System.Data.SqlDbType.VarChar);
            //        _accparam4.Value = account.Password;
            //        SqlParameter _accparam5 = command.Parameters.Add("@Account_Type", System.Data.SqlDbType.Int);
            //        _accparam5.Value = (int)account.AccountType;
            //        SqlParameter _accparam6 = command.Parameters.Add("@Betting_Limit", System.Data.SqlDbType.Decimal);
            //        _accparam6.Value = account.BettingLimit;
            //        SqlParameter _accparam7 = command.Parameters.Add("@Status", System.Data.SqlDbType.Int);
            //        _accparam7.Value = (int)account.Status;
            //        command.CommandType = System.Data.CommandType.StoredProcedure;
            //        connection.Open();
            //        command.ExecuteNonQuery();
            //        ChangeStatus(user, entityID, account.Status);
                    
            //    }
            //    _ts.Complete();
            //}
        }

        public void Allocate(string accountName, int entityID)
        {
            //throw new NotImplementedException(); //TODO:
        }

        public void SetRelateEntity(Relation relation)
        {//OK
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_INS_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Entity_R_Table";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "1,2,3,4";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = relation.Entity.EntityID+","+relation.TargetEntity.EntityID+","+(int)relation.Description+","+relation.Numeric;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public RelationCollection GetRelateEntity(int entityID)
        {//OK
            RelationCollection collection = new RelationCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Entity_R_Table";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "1";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = entityID;
                SqlParameter _param4 = command.Parameters.Add("@order_by1", System.Data.SqlDbType.VarChar);
                _param4.Value = "1";
                SqlParameter _param5 = command.Parameters.Add("@order_by2", System.Data.SqlDbType.TinyInt);
                _param5.Value = 0;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Relation _relation = new Relation();
                        int _entityID = Convert.ToInt32(reader["Entity_ID"]);
                        _relation.Entity = new EntityCollection(Query(_entityID))[0];
                        _relation.TargetEntity = new EntityCollection(Query(Convert.ToInt32(reader["R_Entity_ID"])))[0];

                        _relation.Description = (RelationDescription)Convert.ToInt32(reader["Type"]);
                        _relation.Numeric = Convert.ToDecimal(reader["Value"]);
                        collection.Add(_relation);
                    }
                }
                reader.Close();
                return collection;
            }
        }

        public RelationCollection GetTargetRelation(int targetEntityID)
        {//OK
            RelationCollection collection = new RelationCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Entity_R_Table";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "2";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = targetEntityID;
                SqlParameter _param4 = command.Parameters.Add("@order_by1", System.Data.SqlDbType.VarChar);
                _param4.Value = "1";
                SqlParameter _param5 = command.Parameters.Add("@order_by2", System.Data.SqlDbType.TinyInt);
                _param5.Value = 0;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Relation _relation = new Relation();

                        _relation.Entity = new Entity { EntityID = Convert.ToInt32(reader["Entity_ID"]) };
                        _relation.TargetEntity = new Entity { EntityID = Convert.ToInt32(reader["R_Entity_ID"]) };
                        _relation.Description = (RelationDescription)Convert.ToInt32(reader["Type"]);
                        _relation.Numeric = Convert.ToDecimal(reader["Value"]);
                        collection.Add(_relation);
                    }
                }
                reader.Close();
                return collection;
            }
        }

        public RelationCollection GetRelateEntityWandL(int entityID)
        {//OK
            RelationCollection collection = new RelationCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Entity_R_Table";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "2";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = entityID;
                SqlParameter _param4 = command.Parameters.Add("@order_by1", System.Data.SqlDbType.VarChar);
                _param4.Value = "1";
                SqlParameter _param5 = command.Parameters.Add("@order_by2", System.Data.SqlDbType.TinyInt);
                _param5.Value = 0;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                int EntityID = -1;
                int Type = -1;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {                        
                        EntityID = Convert.ToInt32(reader["Entity_ID"]);
                        Type = Convert.ToInt32(reader["Type"]);
                    }
                }
                reader.Close();
                RelationCollection _rec = new RelationCollection();
                if (Type<4)
                {
                    _rec = GetRelateEntity(EntityID);
                    //foreach (Relation _re in _rec)
                    //{
                    //    if ((int)_re.Description<4)
                    //    {
                    //        Entity _ent = _re.Entity;
                    //        _re.Entity = _re.TargetEntity;
                    //        _re.TargetEntity = _ent;
                    //    }
                    //}
                }
                return _rec;
            }
        }

        public RelationCollection GetTallyRelationEntity(int entityID)
        {//OK
            RelationCollection collection = new RelationCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Entity_R_Table";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "2";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = entityID;
                SqlParameter _param4 = command.Parameters.Add("@order_by1", System.Data.SqlDbType.VarChar);
                _param4.Value = "1";
                SqlParameter _param5 = command.Parameters.Add("@order_by2", System.Data.SqlDbType.TinyInt);
                _param5.Value = 0;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Relation _relation = new Relation();

                        _relation.Entity = new EntityCollection(Query(Convert.ToInt32(reader["Entity_ID"])))[0];
                        _relation.TargetEntity = new EntityCollection(Query(Convert.ToInt32(reader["R_Entity_ID"])))[0];

                        _relation.Description = (RelationDescription)Convert.ToInt32(reader["Type"]);
                        _relation.Numeric = Convert.ToDecimal(reader["Value"]);
                        collection.Add(_relation);
                    }
                }
                reader.Close();
                return collection;
            }
        }

        
    }
}
