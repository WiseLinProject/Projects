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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "EndPeriodAccess" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select EndPeriodAccess.svc or EndPeriodAccess.svc.cs at the Solution Explorer and start debugging.
    public class EndPeriodAccess : IEndPeriodAccess
    {
        string connectionString = ConfigurationManager.ConnectionStrings["AccountDataBase"].ConnectionString;
        public void DoWork()
        {

        }

        public EndPeriodCollection Query(EndPeriod endPeriod)
        {
            EndPeriodCollection collection = new EndPeriodCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "EndPeriod";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "1,2";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = string.Format("{0},{1}", endPeriod.Period_ID, endPeriod.Currency.CurrencyID);
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
                        EndPeriod _endPeriod = new EndPeriod();
                        _endPeriod.Period_ID = int.Parse(reader["Period_ID"].ToString());
                        _endPeriod.Currency.CurrencyID = reader["Currency"].ToString();
                        _endPeriod.ExchangeRate = decimal.Parse(reader["Exchange_Rate"].ToString());
                        collection.Add(_endPeriod);
                    }
                }
                return collection;
            }
        }

        public void Insert(EndPeriod endPeriod)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_INS_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "SP_SEL_Table";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "1,2,3";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = string.Format("{0},{1},{2}", endPeriod.Period_ID, endPeriod.Currency.CurrencyID, endPeriod.ExchangeRate);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
