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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PropertyAccess" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select PropertyAccess.svc or PropertyAccess.svc.cs at the Solution Explorer and start debugging.
    public class PropertyAccess : IPropertyAccess
    {
        string connectionString = ConfigurationManager.ConnectionStrings["AccountDataBase"].ConnectionString;
        

        public void Insert(Property property)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {                
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_INS_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "System_Property";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "1,2";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = property.PropertyName + "," + property.PropertyValue;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Insert(PropertyCollection propertyCollection)
        {
            foreach (Property property in propertyCollection)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = "SP_INS_Table";
                    SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                    _param.Value = "System_Property";
                    SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                    _param2.Value = "1,2";
                    SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                    _param3.Value = property.PropertyName + "," + property.PropertyValue;
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public PropertyCollection Query(int propertyID)
        {        
            //PropertyCollection collection = new PropertyCollection();
            //string connectionString = ConfigurationManager.ConnectionStrings["AccountDataBase"].ConnectionString;
            //using (SqlConnection connection = new SqlConnection(connectionString))
            //{
            //    SqlCommand command = new SqlCommand();
            //    command.Connection = connection;
            //    command.CommandText = "SP_SEL_Table";
            //    SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
            //    _param.Value = "System_Property";
            //    SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
            //    _param2.Value = "1";
            //    SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
            //    _param3.Value = propertyID;
            //    SqlParameter _param4 = command.Parameters.Add("@order_by1", System.Data.SqlDbType.VarChar);
            //    _param4.Value = "1";
            //    SqlParameter _param5 = command.Parameters.Add("@order_by2", System.Data.SqlDbType.TinyInt);
            //    _param5.Value = 0;
            //    command.CommandType = System.Data.CommandType.StoredProcedure;
            //    connection.Open();
            //    SqlDataReader reader = command.ExecuteReader();
            //    if (reader.HasRows)
            //    {
            //        while (reader.Read())
            //        {
            //            Property proty = new Property();
            //            proty.PropertyID = Convert.ToInt32(reader["ID"]);
            //            proty.PropertyName = reader["Property_Name"].ToString();
            //            proty.PropertyValue = reader["Property_Value"].ToString();
            //            collection.Add(proty);
            //        }
            //    }
            //    return collection;
            //}
            return null;
        }

        public PropertyCollection Query(string propertyName)
        {
            PropertyCollection collection = new PropertyCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "System_Property";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "1";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = propertyName;
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
                        Property proty = new Property();
                        proty.PropertyName = reader["Property_Name"].ToString();
                        proty.PropertyValue = reader["Property_Value"].ToString();
                        collection.Add(proty);
                    }
                }
                return collection;
            }
        }

        public PropertyCollection QueryAll()
        {
            PropertyCollection collection = new PropertyCollection();
            string connectionString = ConfigurationManager.ConnectionStrings["AccountDataBase"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "System_Property";
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
                        Property proty = new Property();
                        proty.PropertyName = reader["Property_Name"].ToString();
                        proty.PropertyValue = reader["Property_Value"].ToString();
                        collection.Add(proty);
                    }
                }
                return collection;
            }
        }

        public void Update(string propertyKey, Property property)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_UPD_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "System_Property";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "1,2";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = property.PropertyName+","+property.PropertyValue;
                SqlParameter _param4 = command.Parameters.Add("@value4", System.Data.SqlDbType.VarChar);
                _param4.Value = "1";
                SqlParameter _param5 = command.Parameters.Add("@value5", System.Data.SqlDbType.VarChar);
                _param5.Value = propertyKey;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                command.ExecuteNonQuery();
            }
           
        }

        public void Update(PropertyCollection propertyCollection)
        {
            foreach (Property property in propertyCollection)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = "SP_UPD_Table";
                    SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                    _param.Value = "System_Property";
                    SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                    _param2.Value = "1,2";
                    SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                    _param3.Value = property.PropertyName + "," + property.PropertyValue;
                    SqlParameter _param4 = command.Parameters.Add("@value4", System.Data.SqlDbType.VarChar);
                    _param4.Value = "1";
                    SqlParameter _param5 = command.Parameters.Add("@value5", System.Data.SqlDbType.VarChar);
                    _param5.Value = property.PropertyName;
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
