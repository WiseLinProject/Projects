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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PeriodAccess" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select PeriodAccess.svc or PeriodAccess.svc.cs at the Solution Explorer and start debugging.
    public class PeriodAccess : IPeriodAccess
    {
        string connectionString = ConfigurationManager.ConnectionStrings["AccountDataBase"].ConnectionString;
        public void DoWork()
        {
        }

        public void Set(Period period)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_INS_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Journal_Period";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "2,3,4";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = period.PeriodNo + "," + period.StartDate.ToShortDateString() + "," + period.EndDate.ToShortDateString();
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Set(PeriodCollection periodCollection)
        {
            foreach (Period period in periodCollection)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = "SP_INS_Table";
                    SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                    _param.Value = "Journal_Period";
                    SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                    _param2.Value = "2,3,4";
                    SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                    _param3.Value = period.PeriodNo + "," + period.StartDate.ToShortDateString() + "," + period.EndDate.ToShortDateString();
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public PeriodCollection Query(int id)
        {
            PeriodCollection collection = new PeriodCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Journal_Period";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "1";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = id;
                SqlParameter _param4 = command.Parameters.Add("@order_by1", System.Data.SqlDbType.VarChar);
                _param4.Value = "2";
                SqlParameter _param5 = command.Parameters.Add("@order_by2", System.Data.SqlDbType.TinyInt);
                _param5.Value = 0;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Period period = new Period();
                        period.ID = Convert.ToInt32(reader["ID"].ToString());
                        period.PeriodNo = reader["Period"].ToString();
                        period.StartDate = Convert.ToDateTime(reader["Start_Date"]);
                        period.EndDate = Convert.ToDateTime(reader["End_Date"]);
                        collection.Add(period);
                    }
                }
                return collection;
            }
        }

        public PeriodCollection Query(string periodNo)
        {
            PeriodCollection collection = new PeriodCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Journal_Period";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "2";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = periodNo;
                SqlParameter _param4 = command.Parameters.Add("@order_by1", System.Data.SqlDbType.VarChar);
                _param4.Value = "3";
                SqlParameter _param5 = command.Parameters.Add("@order_by2", System.Data.SqlDbType.TinyInt);
                _param5.Value = 0;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Period period = new Period();
                        period.ID = Convert.ToInt32(reader["ID"].ToString());
                        period.PeriodNo = reader["Period"].ToString();
                        period.StartDate = Convert.ToDateTime(reader["Start_Date"]);
                        period.EndDate = Convert.ToDateTime(reader["End_Date"]);
                        collection.Add(period);
                    }
                }
                return collection;
            }
        }

        public PeriodCollection Query(DateTime startDate)
        {
            PeriodCollection collection = new PeriodCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Journal_Period";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "3";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = startDate;
                SqlParameter _param4 = command.Parameters.Add("@order_by1", System.Data.SqlDbType.VarChar);
                _param4.Value = "3";
                SqlParameter _param5 = command.Parameters.Add("@order_by2", System.Data.SqlDbType.TinyInt);
                _param5.Value = 0;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Period period = new Period();
                        period.ID = Convert.ToInt32(reader["ID"].ToString());
                        period.PeriodNo = reader["Period"].ToString();
                        period.StartDate = Convert.ToDateTime(reader["Start_Date"]);
                        period.EndDate = Convert.ToDateTime(reader["End_Date"]);
                        collection.Add(period);
                    }
                }
                return collection;
            }
        }

        public PeriodCollection QueryAll()
        {
            PeriodCollection collection = new PeriodCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Journal_Period";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = "";
                SqlParameter _param4 = command.Parameters.Add("@order_by1", System.Data.SqlDbType.VarChar);
                _param4.Value = "3";
                SqlParameter _param5 = command.Parameters.Add("@order_by2", System.Data.SqlDbType.TinyInt);
                _param5.Value = 0;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Period period = new Period();
                        period.ID = Convert.ToInt32(reader["ID"].ToString());
                        period.PeriodNo = reader["Period"].ToString();
                        period.StartDate = Convert.ToDateTime(reader["Start_Date"]);
                        period.EndDate = Convert.ToDateTime(reader["End_Date"]);
                        collection.Add(period);
                    }
                }
                return collection;
            }
        }
        /// <summary>
        /// Get next or last period , it will return one period class.
        /// </summary>
        /// <param name="Period"></param>
        /// <param name="flag"> use 1 or -1 or 2 or -2 ...etc </param>
        /// <returns></returns>
        public PeriodCollection Query(string PeriodNo, int flag)
        {
            PeriodCollection collection = new PeriodCollection();            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Journal_Period";
                SqlParameter _param = command.Parameters.Add("@Period", System.Data.SqlDbType.VarChar);
                _param.Value = PeriodNo;
                SqlParameter _param2 = command.Parameters.Add("@flag", System.Data.SqlDbType.Int);
                _param2.Value = flag;                
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {    
                        Period period = new Period();                    
                        period.ID = Convert.ToInt32(reader["ID"].ToString());
                        period.PeriodNo = reader["Period"].ToString();
                        period.StartDate = Convert.ToDateTime(reader["Start_Date"]);
                        period.EndDate = Convert.ToDateTime(reader["End_Date"]);
                        collection.Add(period);
                    }
                }               
            }
            return collection;
        }

    }
}
