using Oleit.AS.Service.DataObject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Oleit.AS.Service.DataService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CurrencyAccess" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select CurrencyAccess.svc or CurrencyAccess.svc.cs at the Solution Explorer and start debugging.
    public class CurrencyAccess : ICurrencyAccess
    {
        string connectionString = ConfigurationManager.ConnectionStrings["AccountDataBase"].ConnectionString;
        public void DoWork()
        {
        }

        public void Insert(Currency currency)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_INS_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "System_Currency";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "1";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = currency.CurrencyID;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                command.ExecuteNonQuery();
            }
            
        }

        public void Insert(CurrencyCollection currencyCollection)
        {
            foreach (Currency currency in currencyCollection)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = "SP_INS_Table";
                    SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                    _param.Value = "System_Currency";
                    SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                    _param2.Value = "1";
                    SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                    _param3.Value = currency.CurrencyID;
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(Currency currency)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_DEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "System_Currency";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "1";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = currency.CurrencyID;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                command.ExecuteNonQuery();
            }            
            
        }

        public CurrencyCollection Query(string currencyID)
        {
            CurrencyCollection collection = new CurrencyCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "System_Currency";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "1";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = currencyID;
                SqlParameter _param4 = command.Parameters.Add("@order_by1", System.Data.SqlDbType.VarChar);
                _param4.Value = "";
                SqlParameter _param5 = command.Parameters.Add("@order_by2", System.Data.SqlDbType.TinyInt);
                _param5.Value = 0 ;

                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Currency currency = new Currency();
                        currency.CurrencyID = reader["Currency_ID"].ToString();
                        collection.Add(currency);
                    }
                }
                return collection;
            }
        }

        public CurrencyCollection QueryAll()
        {
            CurrencyCollection collection = new CurrencyCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "System_Currency";
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
                        Currency currency = new Currency();
                        currency.CurrencyID = reader["Currency_ID"].ToString();
                        collection.Add(currency);
                    }
                }
                return collection;
            }
        }
    }
}
