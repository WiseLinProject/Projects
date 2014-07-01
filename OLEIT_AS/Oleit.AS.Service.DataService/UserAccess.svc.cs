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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "UserAccess" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select UserAccess.svc or UserAccess.svc.cs at the Solution Explorer and start debugging.
    public class UserAccess : IUserAccess
    {
        string connectionString = ConfigurationManager.ConnectionStrings["AccountDataBase"].ConnectionString;
       
        public void Insert(User user)
        {//TODO
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_INS_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Users";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "2,3,4";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = user.UserName + "," + user.UserName + ",1";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Insert(UserCollection userCollection)
        {
            foreach (User user in userCollection)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = "SP_INS_Table";
                    SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                    _param.Value = "Users";
                    SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                    _param2.Value = "2,3,4";
                    SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                    _param3.Value = user.UserName + "," + user.UserName + ",1";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public UserCollection Query(int userID)
        {
            UserCollection collection = new UserCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Users";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "1";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = userID;
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
                        User user = new User();
                        user.UserID = Convert.ToInt32(reader["ID"]);
                        user.UserName = reader["UserAccount"].ToString();                     
                        user.Enable = Convert.ToInt32(reader["Enable"]);
                        collection.Add(user);
                    }
                }
                reader.Close();
                return collection;
            }
        }

        public UserCollection Query(string userName)
        {
            UserCollection collection = new UserCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Users";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "2";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = userName;
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
                        User user = new User();
                        user.UserID = Convert.ToInt32(reader["ID"]);
                        user.UserName = reader["UserAccount"].ToString();                       
                        user.Enable = Convert.ToInt32(reader["Enable"]);
                        collection.Add(user);
                    }
                }
                reader.Close();
                return collection;
            }
        }

        public UserCollection QueryAll()
        {
            UserCollection collection = new UserCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Users";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = "";
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
                        User user = new User();
                        user.UserID = Convert.ToInt32(reader["ID"]);
                        user.UserName = reader["UserAccount"].ToString();                   
                        user.Enable = Convert.ToInt32(reader["Enable"]);
                        collection.Add(user);
                    }
                }
                reader.Close();
                return collection;
            }
        }
       
        public void Update(User user)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_UPD_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Users";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "3";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = user.UserPWD;
                SqlParameter _param4 = command.Parameters.Add("@value4", System.Data.SqlDbType.VarChar);
                _param4.Value = "1";
                SqlParameter _param5 = command.Parameters.Add("@value5", System.Data.SqlDbType.VarChar);
                _param5.Value = user.UserID;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Update(UserCollection userCollection)
        {
            foreach (User user in userCollection)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = "SP_UPD_Table";
                    SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                    _param.Value = "Users";
                    SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                    _param2.Value = "3";
                    SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                    _param3.Value = user.UserPWD;
                    SqlParameter _param4 = command.Parameters.Add("@value4", System.Data.SqlDbType.VarChar);
                    _param4.Value = "1";
                    SqlParameter _param5 = command.Parameters.Add("@value5", System.Data.SqlDbType.VarChar);
                    _param5.Value = user.UserID;
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Disable(UserCollection collection)
        {
            foreach (User user in collection)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = "SP_UPD_Table";
                    SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                    _param.Value = "Users"; // Table Name
                    SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                    _param2.Value = "4"; // Update Column Order
                    SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                    _param3.Value = user.Enable; // update value
                    SqlParameter _param4 = command.Parameters.Add("@value4", System.Data.SqlDbType.VarChar);
                    _param4.Value = "1"; // Where Column Order
                    SqlParameter _param5 = command.Parameters.Add("@value5", System.Data.SqlDbType.VarChar);
                    _param5.Value = user.UserID; // Where value equal to 
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            
        }

        public bool DisableUser(int userID)
        {            
            if (new UserCollection(Query(userID))[0].Enable.Equals(0))
                return true;
            else
                return false;            
        }

        public void ChangePassword(string userName, string password)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_UPD_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Users";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "3";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = password;
                SqlParameter _param4 = command.Parameters.Add("@value4", System.Data.SqlDbType.VarChar);
                _param4.Value = "2";
                SqlParameter _param5 = command.Parameters.Add("@value5", System.Data.SqlDbType.VarChar);
                _param5.Value = userName;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                command.ExecuteNonQuery();
            }
           
        }

        public void UpdateRoles(string userName, Role role)
        {
            //using (SqlConnection connection = new SqlConnection(connectionString))
            //{
            //    SqlCommand command = new SqlCommand();
            //    command.Connection = connection;
            //    command.CommandText = "SP_Frank_TEST";
            //    SqlParameter ColumnParam = command.Parameters.Add("@UserName", System.Data.SqlDbType.VarChar);
            //    ColumnParam.Value = userName;
            //    SqlParameter ColumnParam1 = command.Parameters.Add("@Role_Name", System.Data.SqlDbType.VarChar);
            //    ColumnParam1.Value = role.RoleName;
            //    command.CommandType = System.Data.CommandType.StoredProcedure;
            //    connection.Open();
            //    command.ExecuteNonQuery();
            //}
        }

        public void UpdateRoles(string userName, RoleCollection roleCollection)
        {
            using (TransactionScope _ts = new TransactionScope())
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    UserCollection collection = new UserCollection();
                    collection = Query(userName);
                    User user = collection[0];
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = "SP_DEL_Table";
                    SqlParameter ColumnParam = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                    ColumnParam.Value = "System_Roles_R_User";
                    SqlParameter ColumnParam1 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                    ColumnParam1.Value = "1";
                    SqlParameter ColumnParam2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                    ColumnParam2.Value = user.UserID;
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    connection.Open();
                    command.ExecuteNonQuery();
                    foreach (Role role in roleCollection)
                    {
                        using (SqlConnection connection2 = new SqlConnection(connectionString))
                        {
                            SqlCommand command2 = new SqlCommand();
                            command2.Connection = connection2;
                            command.CommandText = "SP_INS_Table";
                            SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                            _param.Value = "System_Roles_R_User";
                            SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                            _param2.Value = "1,2";
                            SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                            _param3.Value = user.UserID.ToString()+","+role.ID.ToString();
                            command2.CommandType = System.Data.CommandType.StoredProcedure;
                            connection2.Open();
                            command2.ExecuteNonQuery();
                        }
                    }
                }
                _ts.Complete();
            }
        }

        public RoleCollection QueryRoles(string userName, string roleName)
        {//TODO Maybe not useful
            RoleCollection collection = new RoleCollection();
            //using (SqlConnection connection = new SqlConnection(connectionString))
            //{
            //    SqlCommand command = new SqlCommand();
            //    command.Connection = connection;
            //    command.CommandText = "SP_SEL_Table";
            //    SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
            //    _param.Value = "Users";
            //    SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
            //    _param2.Value = "2";
            //    SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
            //    _param3.Value = userName;
            //    SqlParameter _param4 = command.Parameters.Add("@order_by1", System.Data.SqlDbType.VarChar);
            //    _param4.Value = "2";
            //    SqlParameter _param5 = command.Parameters.Add("@order_by2", System.Data.SqlDbType.TinyInt);
            //    _param5.Value = 0;
            //    command.CommandType = System.Data.CommandType.StoredProcedure;
            //    connection.Open();
            //    SqlDataReader reader = command.ExecuteReader();
            //    if (reader.HasRows)
            //    {
            //        while (reader.Read())
            //        {
            //            User user = new User();
            //            user.UserID = Convert.ToInt32(reader["ID"]);
            //            user.UserName = reader["UserAccount"].ToString();
            //            user.EntityRelationID = Convert.ToInt32(reader["EntityRelationID"]);
            //            user.Enable = Convert.ToInt32(reader["Enable"]);
            //            collection.Add(user);
            //        }
            //    }
                return collection;
            //}

        }

        public bool CheckRole(string userName, Role role)
        {//TODO
            //using (SqlConnection connection = new SqlConnection(connectionString))
            //{
            //    SqlCommand command = new SqlCommand();
            //    command.Connection = connection;
            //    command.CommandText = "SP_Frank_TEST";
            //    SqlParameter ColumnParam = command.Parameters.Add("@userName", System.Data.SqlDbType.VarChar);
            //    ColumnParam.Value = userName;
            //    SqlParameter ColumnParam1 = command.Parameters.Add("@Role_Name", System.Data.SqlDbType.VarChar);
            //    ColumnParam1.Value = role.RoleName;
            //    command.CommandType = System.Data.CommandType.StoredProcedure;
            //    connection.Open();
            //    if (command.ExecuteScalar().Equals("1"))
            //        return true;
            //    else
            //        return false;
            //}
            return false;
        }

        public bool CheckPassword(string userName, string password)
        {
            UserCollection collection = new UserCollection();
            collection = Query(userName);
            User user = collection[0];
            if (password.Equals(user.UserPWD))
                return true;
            else
                return false;
        }

        public RoleCollection GetAllRoles(string userName)
        {//TODO
            RoleCollection collection = new RoleCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_Frank_TEST";
                SqlParameter ColumnParam = command.Parameters.Add("@UserAccount", System.Data.SqlDbType.VarChar);
                ColumnParam.Value = userName;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Role role = new Role();
                        role.RoleName = reader["Role_Name"].ToString();                       
                        collection.Add(role);
                    }
                }
                reader.Close();
                return collection;
            }
        }

        public FuncMenuCollection GetMenu(string userName)
        {//TODO
            FuncMenuCollection collection = new FuncMenuCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_Frank_TEST";
                SqlParameter ColumnParam = command.Parameters.Add("@UserAccount", System.Data.SqlDbType.VarChar);
                ColumnParam.Value = userName;             
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        FuncMenu menu = new FuncMenu();
                        menu.ItemID = Convert.ToInt32(reader["ItemID"]);
                        menu.Text = reader["Text"].ToString();
                        menu.Path = reader["Path"].ToString();
                        menu.ParentID = Convert.ToInt32(reader["ParentID"]);
                        menu.Sort = Convert.ToInt32(reader["Sort"]);

                        collection.Add(menu);
                    }
                }
                reader.Close();
                return collection;
            }
        }
    }
}
