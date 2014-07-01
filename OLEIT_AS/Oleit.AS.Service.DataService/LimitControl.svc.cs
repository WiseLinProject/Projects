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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "LimitControl" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select LimitControl.svc or LimitControl.svc.cs at the Solution Explorer and start debugging.
    public class LimitControl : ILimitControl
    {
        string connectionString = ConfigurationManager.ConnectionStrings["AccountDataBase"].ConnectionString;

        public EntityCollection GetEntities(User user)
        {
            EntityCollection collection = new EntityCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_Frank_TEST";

                SqlParameter ColumnParam = command.Parameters.Add("@User_ID", System.Data.SqlDbType.Int);
                ColumnParam.Value = user.UserID;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Entity entity = new Entity();
                        entity.EntityID = Convert.ToInt32(reader["ID"]);
                        entity.EntityName = reader["Name"].ToString();
                        entity.EntityType = (EntityType)Convert.ToInt32(reader["Type"]);
                        entity.Currency.CurrencyID = reader["Currency"].ToString();
                        entity.ExchangeRate = Convert.ToDecimal(reader["Exchange_Rate"]);
                        entity.SumType = (SumType)Convert.ToInt32(reader["SumType"]);
                        collection.Add(entity);
                    }
                }
                return collection;
            } 
        }

        public AccountCollection GetAccounts(User user)
        {
            AccountCollection collection = new AccountCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_Frank_TEST";

                SqlParameter ColumnParam = command.Parameters.Add("@User_ID", System.Data.SqlDbType.Int);
                ColumnParam.Value = user.UserID;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Account account = new Account();
                        account.AccountName = reader["Name"].ToString();
                        account.AccountType = (AccountType)(Convert.ToInt32(reader["Type"]));
                        account.BettingLimit = Convert.ToDecimal(reader["Betting_Limit"]);
                        account.Status = (Status)(Convert.ToInt32(reader["Status"]));                        
                        collection.Add(account);
                    }
                }
                return collection;
            } 
        }

        public FuncMenuCollection GetMenu(User user)
        {
            FuncMenuCollection collection = new FuncMenuCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_Frank_TEST";

                SqlParameter ColumnParam = command.Parameters.Add("@User_ID", System.Data.SqlDbType.Int);
                ColumnParam.Value = user.UserID;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        FuncMenu menu = new FuncMenu();
                        menu.ItemID = Convert.ToInt32(reader["Menu_ID"]);
                        menu.Text = reader["Text"].ToString();
                        menu.Path = reader["Path"].ToString();
                        menu.ParentID = Convert.ToInt32(reader["ParentID"]);
                        menu.Sort = Convert.ToInt32(reader["Sort"]);
                        collection.Add(menu);
                    }
                }
                return collection;
            }
        }

        public FuncMenuCollection GetMenuByUserID(string UserID)
        {
            FuncMenuCollection collection = new FuncMenuCollection();
            string connectionString = ConfigurationManager.ConnectionStrings["AccountDataBase"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Users_Menu";
                SqlParameter _param = command.Parameters.Add("@User_ID", System.Data.SqlDbType.Int);
                _param.Value = UserID;

                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        FuncMenu menu = new FuncMenu();
                        menu.ItemID = int.Parse(reader["ID"].ToString());
                        menu.Text = reader["Text"].ToString();
                        menu.Sort = int.Parse(reader["Sort"].ToString());
                        menu.ParentID = int.Parse(reader["ParentID"].ToString());
                        menu.Path = reader["Path"].ToString();
                        
                        collection.Add(menu);
                    }
                }
                reader.Close();
            }
            return collection;
        }

        public SortedList<int,string> getUserFunctions(string UserID, string MenuID)
        {
            SortedList<int,string> userFunctionList = new SortedList<int,string> ();

            string connectionString = ConfigurationManager.ConnectionStrings["AccountDataBase"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_UserFunctions";
                SqlParameter _param = command.Parameters.Add("@User_ID", System.Data.SqlDbType.Int);
                _param.Value = UserID;

                SqlParameter _param2 = command.Parameters.Add("@MenuID", System.Data.SqlDbType.Int);
                _param2.Value = MenuID;

                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        userFunctionList.Add(Convert.ToInt32(reader["FunctionID"]), reader["Name"].ToString());
                    }
                }
                reader.Close();
            }
            return userFunctionList;
        }

        public SortedList<int, string> getUserFunctionByMenuID(string MenuID)
        {
            SortedList<int, string> Functions = new SortedList<int, string>();
           string connectionString = ConfigurationManager.ConnectionStrings["AccountDataBase"].ConnectionString;
         
           using (SqlConnection connection = new SqlConnection(connectionString))
           {
               SqlCommand command = new SqlCommand();
               command.Connection = connection;
               command.CommandText = "SP_SEL_Table";
               SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
               _param.Value = "System_Functions";
               SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
               _param2.Value = "2";
               SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
               _param3.Value = MenuID;
               SqlParameter _param4 = command.Parameters.Add("@order_by1", System.Data.SqlDbType.VarChar);
               _param4.Value = "1";
               SqlParameter _param5 = command.Parameters.Add("@order_by2", System.Data.SqlDbType.TinyInt);
               _param5.Value ="0";
               command.CommandType = System.Data.CommandType.StoredProcedure;
               connection.Open();
               SqlDataReader reader = command.ExecuteReader();
               if (reader.HasRows)
               {
                   while (reader.Read())
                   {
                       int FunctionID = Convert.ToInt32(reader["ID"]);
                       string FunctionName = reader["Name"].ToString();
                       Functions.Add(FunctionID, FunctionName);
                   }
               }
               reader.Close();
             
           }
           return Functions;
        }

        public bool insertUserFunction(string UserID, string FunctionID)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["AccountDataBase"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = "SP_INS_Table";
                    SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                    _param.Value = "System_User_R_Function";
                    SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                    _param2.Value ="1,2" ;
                    SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                    _param3.Value =UserID +"," + FunctionID ;
              
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    connection.Open();
                    command.ExecuteNonQuery();
                    return true;
                }
            }
            catch
            {
                return false;
            }

       
            
        }

        public bool deleteUserFunction(string UserID, string FunctionID)
        {

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["AccountDataBase"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = "SP_DEL_Table";
                    SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                    _param.Value = "System_User_R_Function";
                    SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                    _param2.Value = "1,2";
                    SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                    _param3.Value = UserID + "," + FunctionID;

                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    connection.Open();
                    command.ExecuteNonQuery();
                    return true;
                }
            }
            catch
            {
                return false;
            }

        }

        public bool insertMenuToRole(String MenuID, String RoleID)
        {
         try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["AccountDataBase"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = "SP_INS_TABLE";
                    SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                    _param.Value = "System_Roles_R_Menu";
                    SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                    _param2.Value = "1,2";
                    SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                    _param3.Value = MenuID + "," + RoleID;

                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    connection.Open();
                    command.ExecuteNonQuery();
                    return true;
                }
            }
            catch
            {
                return false;
            }

        }

        public bool insertRoleToUser(String UserID, String RoleID)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["AccountDataBase"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = "SP_INS_TABLE";
                    SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                    _param.Value = "System_Roles_R_User";
                    SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                    _param2.Value = "1,2";
                    SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                    _param3.Value = UserID + "," + RoleID;

                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    connection.Open();
                    command.ExecuteNonQuery();
                    return true;
                }
            }
            catch
            {
                return false;
            }

        }

        public bool isFunctionAuthorized(String UserID, String FunctionID)
        {

            string connectionString = ConfigurationManager.ConnectionStrings["AccountDataBase"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "System_User_R_Function";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "1,2";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = UserID +"," + FunctionID;
                SqlParameter _param4 = command.Parameters.Add("@order_by1", System.Data.SqlDbType.VarChar);
                _param4.Value = "1";
                SqlParameter _param5 = command.Parameters.Add("@order_by2", System.Data.SqlDbType.TinyInt);
                _param5.Value = "0";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                int i = 0;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        i += 2;
                    }
                }
                reader.Close();

                if (i > 0)
                { 
                    return true;
                }
            }
            return false;

        }


        public List<string> getUserRole(string userID)
        {
            List<string> Roles = new List<string>();
            string connectionString = ConfigurationManager.ConnectionStrings["AccountDataBase"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Users_Role";
                SqlParameter _param = command.Parameters.Add("@User_ID", System.Data.SqlDbType.Int);
                _param.Value = userID;

                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                     string RoleID = reader["ID"].ToString();
                     Roles.Add(RoleID);
                    }
                }
                reader.Close();
            }
            return Roles;
        
        
        }
      
       

       




        public bool CheckLimit(User user, int itemID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_Frank_TEST";

                SqlParameter ColumnParam = command.Parameters.Add("@User_ID", System.Data.SqlDbType.Int);
                ColumnParam.Value = user.UserID;
                SqlParameter ColumnParam1 = command.Parameters.Add("@ID", System.Data.SqlDbType.Int);
                ColumnParam1.Value = itemID;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();

                string Islimit = command.ExecuteScalar().ToString();
                if (Islimit.Equals("0"))
                    return false;
                else
                    return true;
               
            }
        }
    }
}
