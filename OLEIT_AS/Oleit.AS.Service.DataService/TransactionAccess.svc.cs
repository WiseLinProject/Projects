using Oleit.AS.Service.DataObject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Oleit.AS.Service.DataService.WCFService
{
    
    public class TransactionAccess : ITransactionAccess
    {
        string connectionString = ConfigurationManager.ConnectionStrings["AccountDataBase"].ConnectionString;
        
        public TransactionCollection QueryAll()
        {
            TransactionCollection collection = new TransactionCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Journal_Transaction";
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
                        Oleit.AS.Service.DataObject.Transaction _transaction = new Oleit.AS.Service.DataObject.Transaction();
                        _transaction.ID = Convert.ToInt32(reader["ID"]);
                        PeriodAccess _perioda = new PeriodAccess();
                        _transaction.Period = new PeriodCollection(_perioda.Query(Convert.ToInt32(reader["Period_ID"])))[0];

                        EntityAccess entitya = new EntityAccess();
                        _transaction.FromEntity = new EntityCollection(entitya.Query(Convert.ToInt32(reader["From_EntityID"])))[0];
                        _transaction.ToEntity = new EntityCollection(entitya.Query(Convert.ToInt32(reader["To_EntityID"])))[0];
                        _transaction.Amount = Convert.ToDecimal(reader["Amount"]);
                        if (!reader["To_Amount"].ToString().Equals(""))
                            _transaction.To_Amount = Convert.ToDecimal(reader["To_Amount"]);
                        else
                            _transaction.To_Amount = 0;
                        UserAccess _usera = new UserAccess();
                        if (!reader["Notice_UserID"].ToString().Equals(""))                        
                            _transaction.NoticeUser = new UserCollection(_usera.Query(Convert.ToInt32(reader["Notice_UserID"])))[0];                       
                        else
                            _transaction.NoticeUser = new User();
                     
                        if (!reader["Notice_Time"].ToString().Equals(""))
                            _transaction.NoticeTime = Convert.ToDateTime(reader["Notice_Time"]);

                        _transaction.IsPay = (IsPay)Convert.ToInt32(reader["IsPay"]);

                        if (!reader["Confirm_UserID"].ToString().Equals(""))
                            _transaction.ConfirmUser = new UserCollection(_usera.Query(Convert.ToInt32(reader["Confirm_UserID"])))[0];
                        else
                            _transaction.ConfirmUser = new User();

                        if (!reader["Confirm_Time"].ToString().Equals(""))
                            _transaction.ConfirmTime = Convert.ToDateTime(reader["Confirm_Time"]);

                        if (!reader["Confirm_UserID"].ToString().Equals(""))
                            _transaction.Updater = new UserCollection(_usera.Query(Convert.ToInt32(reader["Updater"])))[0];
                        else
                            _transaction.Updater = new User();
                        if (!reader["UpdateTime"].ToString().Equals(""))
                            _transaction.UpdateTime = Convert.ToDateTime(reader["UpdateTime"]);
                        if (!reader["Confirm_UserID"].ToString().Equals(""))
                            _transaction.Creator = new UserCollection(_usera.Query(Convert.ToInt32(reader["Creator"])))[0];
                        else
                            _transaction.Creator = new User();

                        if (!reader["CreateTime"].ToString().Equals(""))
                            _transaction.CreateTime = Convert.ToDateTime(reader["CreateTime"]);

                        _transaction.FromCurrency = reader["From_Currency"].ToString();
                        _transaction.ToCurrency = reader["To_Currency"].ToString();
                        if (!reader["Exchange_Rate"].ToString().Equals(""))
                            _transaction.ExchangeRate = Convert.ToDecimal(reader["Exchange_Rate"]);
                        else
                            _transaction.ExchangeRate = 1;
                        collection.Add(_transaction);

                    }
                }
                reader.Close();
                return collection;
            }
        }

        public TransactionCollection QueryByPeriodid(int _periodid)
        {
            TransactionCollection collection = new TransactionCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Journal_Transaction";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "2";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = _periodid;
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
                        Oleit.AS.Service.DataObject.Transaction _transaction = new Oleit.AS.Service.DataObject.Transaction();
                        _transaction.ID = Convert.ToInt32(reader["ID"]);
                        PeriodAccess _perioda = new PeriodAccess();
                        _transaction.Period = new PeriodCollection(_perioda.Query(Convert.ToInt32(reader["Period_ID"])))[0];

                        EntityAccess entitya = new EntityAccess();
                        _transaction.FromEntity = new EntityCollection(entitya.Query(Convert.ToInt32(reader["From_EntityID"])))[0];
                        _transaction.ToEntity = new EntityCollection(entitya.Query(Convert.ToInt32(reader["To_EntityID"])))[0];
                        _transaction.Amount = Convert.ToDecimal(reader["Amount"]);
                        if (!reader["To_Amount"].ToString().Equals(""))
                            _transaction.To_Amount = Convert.ToDecimal(reader["To_Amount"]);
                        else
                            _transaction.To_Amount = 0;
                        UserAccess _usera = new UserAccess();
                        if (!reader["Notice_UserID"].ToString().Equals(""))
                            _transaction.NoticeUser = new UserCollection(_usera.Query(Convert.ToInt32(reader["Notice_UserID"])))[0];
                        else
                            _transaction.NoticeUser = new User();

                        if (!reader["Notice_Time"].ToString().Equals(""))
                            _transaction.NoticeTime = Convert.ToDateTime(reader["Notice_Time"]);

                        _transaction.IsPay = (IsPay)Convert.ToInt32(reader["IsPay"]);

                        if (!reader["Confirm_UserID"].ToString().Equals(""))
                            _transaction.ConfirmUser = new UserCollection(_usera.Query(Convert.ToInt32(reader["Confirm_UserID"])))[0];
                        else
                            _transaction.ConfirmUser = new User();

                        if (!reader["Confirm_Time"].ToString().Equals(""))
                            _transaction.ConfirmTime = Convert.ToDateTime(reader["Confirm_Time"]);
                       
                        if (!reader["Confirm_UserID"].ToString().Equals(""))
                            _transaction.Updater = new UserCollection(_usera.Query(Convert.ToInt32(reader["Updater"])))[0];
                        else
                            _transaction.Updater = new User();
                        if (!reader["UpdateTime"].ToString().Equals(""))
                            _transaction.UpdateTime = Convert.ToDateTime(reader["UpdateTime"]);
                        if (!reader["Confirm_UserID"].ToString().Equals(""))
                            _transaction.Creator = new UserCollection(_usera.Query(Convert.ToInt32(reader["Creator"])))[0];
                        else
                            _transaction.Creator = new User();

                        if (!reader["CreateTime"].ToString().Equals(""))
                            _transaction.CreateTime = Convert.ToDateTime(reader["CreateTime"]);
                        collection.Add(_transaction);

                        _transaction.FromCurrency = reader["From_Currency"].ToString();
                        _transaction.ToCurrency = reader["To_Currency"].ToString();
                        if(!reader["Exchange_Rate"].ToString().Equals(""))
                            _transaction.ExchangeRate = Convert.ToDecimal(reader["Exchange_Rate"]);
                        else
                            _transaction.ExchangeRate = 1;

                    }
                }
                reader.Close();
                return collection;
            }
        }

        public TransactionCollection QueryByID(int _id)
        {
            TransactionCollection collection = new TransactionCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Journal_Transaction";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "1";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = _id;
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
                        Oleit.AS.Service.DataObject.Transaction _transaction = new Oleit.AS.Service.DataObject.Transaction();
                        _transaction.ID = Convert.ToInt32(reader["ID"]);
                        PeriodAccess _perioda = new PeriodAccess();
                        _transaction.Period = new PeriodCollection(_perioda.Query(Convert.ToInt32(reader["Period_ID"])))[0];

                        EntityAccess entitya = new EntityAccess();
                        _transaction.FromEntity = new EntityCollection(entitya.Query(Convert.ToInt32(reader["From_EntityID"])))[0];
                        _transaction.ToEntity = new EntityCollection(entitya.Query(Convert.ToInt32(reader["To_EntityID"])))[0];
                        _transaction.Amount = Convert.ToDecimal(reader["Amount"]);
                        if (!reader["To_Amount"].ToString().Equals(""))
                            _transaction.To_Amount = Convert.ToDecimal(reader["To_Amount"]);
                        else
                            _transaction.To_Amount = 0;
                        UserAccess _usera = new UserAccess();
                        if (!reader["Notice_UserID"].ToString().Equals(""))
                            _transaction.NoticeUser = new UserCollection(_usera.Query(Convert.ToInt32(reader["Notice_UserID"])))[0];
                        else
                            _transaction.NoticeUser = new User();

                        if (!reader["Notice_Time"].ToString().Equals(""))
                            _transaction.NoticeTime = Convert.ToDateTime(reader["Notice_Time"]);

                        _transaction.IsPay = (IsPay)Convert.ToInt32(reader["IsPay"]);

                        if (!reader["Confirm_UserID"].ToString().Equals(""))
                            _transaction.ConfirmUser = new UserCollection(_usera.Query(Convert.ToInt32(reader["Confirm_UserID"])))[0];
                        else
                            _transaction.ConfirmUser = new User();

                        if (!reader["Confirm_Time"].ToString().Equals(""))
                            _transaction.ConfirmTime = Convert.ToDateTime(reader["Confirm_Time"]);

                        if (!reader["Confirm_UserID"].ToString().Equals(""))
                            _transaction.Updater = new UserCollection(_usera.Query(Convert.ToInt32(reader["Updater"])))[0];
                        else
                            _transaction.Updater = new User();
                        if (!reader["UpdateTime"].ToString().Equals(""))
                            _transaction.UpdateTime = Convert.ToDateTime(reader["UpdateTime"]);
                        if (!reader["Confirm_UserID"].ToString().Equals(""))
                            _transaction.Creator = new UserCollection(_usera.Query(Convert.ToInt32(reader["Creator"])))[0];
                        else
                            _transaction.Creator = new User();

                        if (!reader["CreateTime"].ToString().Equals(""))
                            _transaction.CreateTime = Convert.ToDateTime(reader["CreateTime"]);
                        collection.Add(_transaction);

                        _transaction.FromCurrency = reader["From_Currency"].ToString();
                        _transaction.ToCurrency = reader["To_Currency"].ToString();
                        if (!reader["Exchange_Rate"].ToString().Equals(""))
                            _transaction.ExchangeRate = Convert.ToDecimal(reader["Exchange_Rate"]);
                        else
                            _transaction.ExchangeRate = 1;

                    }
                }
                reader.Close();
                return collection;
            }
        }

        public void InsertTransaction(Transaction _transaction)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand _jcommand = new SqlCommand();
                _jcommand.Connection = connection;
                _jcommand.CommandText = "SP_INS_Table";
                SqlParameter _jparam1 = _jcommand.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _jparam1.Value = "Journal_Transaction";
                SqlParameter _jparam2 = _jcommand.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _jparam2.Value = "2,3,4,5,8,14,16,17,18,19";
                StringBuilder _tcontent = new StringBuilder();
                _tcontent.AppendFormat("{0}{1}", _transaction.Period.ID, ",");
                _tcontent.AppendFormat("{0}{1}", _transaction.FromEntity.EntityID, ",");
                _tcontent.AppendFormat("{0}{1}", _transaction.ToEntity.EntityID, ",");
                _tcontent.AppendFormat("{0}{1}", _transaction.Amount, ",");
                _tcontent.AppendFormat("{0}{1}", (int)_transaction.IsPay, ",");
                _tcontent.AppendFormat("{0}{1}", _transaction.Creator.UserID, ",");
                _tcontent.AppendFormat("{0}{1}", _transaction.FromCurrency, ",");
                _tcontent.AppendFormat("{0}{1}", _transaction.ToCurrency, ",");
                _tcontent.AppendFormat("{0}{1}", _transaction.ExchangeRate, ",");
                _tcontent.AppendFormat("{0}", _transaction.To_Amount);

                SqlParameter _jparam3 = _jcommand.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _jparam3.Value = _tcontent.ToString();
                _jcommand.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                _jcommand.ExecuteNonQuery();
            }
        }

        public void InsertTransactionCollection(TransactionCollection _collection)
        {
            foreach (Transaction _transaction in _collection)
            {
                InsertTransaction(_transaction);
            }
        }

        public void SetNotices(int _id,int _userid)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand _command = new SqlCommand();
                _command.Connection = connection;
                _command.CommandText = "SP_UPD_Table";
                SqlParameter _param = _command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Journal_Transaction";
                SqlParameter _param2 = _command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "6,7,12,13";
                StringBuilder _content = new StringBuilder();
                _content.AppendFormat("{0}{1}", _userid , ",");
                _content.AppendFormat("{0}{1}", "getdate()" , ",");
                _content.AppendFormat("{0}{1}", _userid , ",");
                _content.AppendFormat("{0}", "getdate()");
                SqlParameter _param3 = _command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = _content.ToString();
                SqlParameter _param4 = _command.Parameters.Add("@value4", System.Data.SqlDbType.VarChar);
                _param4.Value = "1";
                SqlParameter _param5 = _command.Parameters.Add("@value5", System.Data.SqlDbType.VarChar);
                _param5.Value = _id;
                _command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                _command.ExecuteNonQuery();
            }
        }

        public void SetConfirm(int _id, int _userid,int _periodid)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand _command = new SqlCommand();
                _command.Connection = connection;
                _command.CommandText = "SP_UPD_Table";
                SqlParameter _param = _command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Journal_Transaction";
                SqlParameter _param2 = _command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "8,9,10,11,12,13";
                StringBuilder _content = new StringBuilder();
                _content.AppendFormat("{0}{1}", (int)IsPay.Y , ",");
                _content.AppendFormat("{0}{1}", _userid , ",");
                _content.AppendFormat("{0}{1}", "getdate()" , ",");
                _content.AppendFormat("{0}{1}", _periodid , ",");
                _content.AppendFormat("{0}{1}", _userid , ",");
                _content.AppendFormat("{0}", "getdate()");
               
                SqlParameter _param3 = _command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = _content.ToString();
                SqlParameter _param4 = _command.Parameters.Add("@value4", System.Data.SqlDbType.VarChar);
                _param4.Value = "1";
                SqlParameter _param5 = _command.Parameters.Add("@value5", System.Data.SqlDbType.VarChar);
                _param5.Value = _id;
                _command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                _command.ExecuteNonQuery();
            }
        }

        public void Update(Transaction transaction)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand _command = new SqlCommand();
                _command.Connection = connection;
                _command.CommandText = "SP_UPD_Table";
                SqlParameter _param = _command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Journal_Transaction";
                SqlParameter _param2 = _command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "3,4,5,12,13,16,17,18,19";
                StringBuilder _content = new StringBuilder();
                _content.AppendFormat("{0}{1}", transaction.FromEntity.EntityID , ",");
                _content.AppendFormat("{0}{1}", transaction.ToEntity.EntityID, ",");
                _content.AppendFormat("{0}{1}", transaction.Amount , ",");
                _content.AppendFormat("{0}{1}", transaction.Updater.UserID , ",");
                _content.AppendFormat("{0}{1}", "getdate()",",");
                _content.AppendFormat("{0}{1}", transaction.FromCurrency, ",");
                _content.AppendFormat("{0}{1}", transaction.ToCurrency, ",");
                _content.AppendFormat("{0}{1}", transaction.ExchangeRate, ",");
                _content.AppendFormat("{0}", transaction.To_Amount);

                SqlParameter _param3 = _command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = _content.ToString();
                SqlParameter _param4 = _command.Parameters.Add("@value4", System.Data.SqlDbType.VarChar);
                _param4.Value = "1";
                SqlParameter _param5 = _command.Parameters.Add("@value5", System.Data.SqlDbType.VarChar);
                _param5.Value = transaction.ID;
                _command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                _command.ExecuteNonQuery();
            }
        }

    }
}
