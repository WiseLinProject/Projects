using Oleit.AS.Service.DataObject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;


namespace Oleit.AS.Service.DataService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "MenuAccess" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select MenuAccess.svc or MenuAccess.svc.cs at the Solution Explorer and start debugging.
    public class MenuAccess : IMenuAccess
    {
        string connectionString = ConfigurationManager.ConnectionStrings["AccountDataBase"].ConnectionString;
        public void Insert(Role role, FuncMenu menu)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";

                SqlParameter ColumnParam = command.Parameters.Add("@Menu_ID", System.Data.SqlDbType.Int);
                ColumnParam.Value = menu.ItemID;
                SqlParameter ColumnParam1 = command.Parameters.Add("@Role_ID", System.Data.SqlDbType.Int);
                ColumnParam1.Value = role.ID;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        //public void Insert(Role role, MenuCollection menucollection)
        //{
        //    foreach (Menu menu in menucollection)
        //    {
        //        using (SqlConnection connection = new SqlConnection(connectionString))
        //        {
        //            SqlCommand command = new SqlCommand();
        //            command.Connection = connection;
        //            command.CommandText = "SP_SEL_Table";

        //            SqlParameter ColumnParam = command.Parameters.Add("@Menu_ID", System.Data.SqlDbType.Int);
        //            ColumnParam.Value = menu.ItemID;
        //            SqlParameter ColumnParam1 = command.Parameters.Add("@Role_ID", System.Data.SqlDbType.Int);
        //            ColumnParam1.Value = role.ID;
        //            command.CommandType = System.Data.CommandType.StoredProcedure;
        //            connection.Open();
        //            command.ExecuteNonQuery();
        //        }
        //    }
        //}

        //public MenuCollection Query(Role role)
        //{
        //    MenuCollection collection = new MenuCollection();
        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        SqlCommand command = new SqlCommand();
        //        command.Connection = connection;
        //        command.CommandText = "SP_SEL_Table";

        //        SqlParameter ColumnParam = command.Parameters.Add("@Role_ID", System.Data.SqlDbType.Int);
        //        ColumnParam.Value = role.ID;
        //        command.CommandType = System.Data.CommandType.StoredProcedure;
        //        connection.Open();
        //        SqlDataReader reader = command.ExecuteReader();
        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                Menu menu = new Menu();
        //                menu.ItemID = Convert.ToInt32(reader["Menu_ID"]);
        //                menu.Text = reader["Text"].ToString();
        //                menu.Path = reader["Path"].ToString();
        //                menu.ParentID = Convert.ToInt32(reader["ParentID"]);
        //                menu.Sort = Convert.ToInt32(reader["Sort"]);
        //                collection.Add(menu);
        //            }
        //        }
        //        return collection;
        //    }
            
        //}

        //public Tuple<MenuCollection,RoleCollection> Query()
        //{
        //    return new Tuple<MenuCollection, RoleCollection>(queryAllMenus(),queryAllRoles());
        //}

        public FuncMenuCollection QueryAllMenu()
        {
            FuncMenuCollection collection = new FuncMenuCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "System_Menu";
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
                        FuncMenu funcMenu = new FuncMenu();
                        funcMenu.ItemID = Convert.ToInt32(reader["ID"]);
                        funcMenu.Text = reader["Text"].ToString();
                        funcMenu.Path = reader["Path"].ToString();
                        funcMenu.ParentID = Convert.ToInt32(reader["ParentID"]);
                        funcMenu.Sort = Convert.ToInt32(reader["Sort"]);
                        collection.Add(funcMenu);
                    }
                }
                return collection;
            }
        }

        /// <summary>
        /// Input roleID and output single Role class
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public Role QueryRole(int roleID)
        {
            RoleCollection _roleCollection = new RoleCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "System_Roles";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "1";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = roleID;
                SqlParameter _param4 = command.Parameters.Add("@order_by1", System.Data.SqlDbType.VarChar);
                _param4.Value = "1";
                SqlParameter _param5 = command.Parameters.Add("@order_by2", System.Data.SqlDbType.TinyInt);
                _param5.Value = 0;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                Role _role = new Role();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        
                        _role.ID = Convert.ToInt32(reader["ID"]);
                        _role.RoleName = reader["Role_Name"].ToString();
                        
                    }
                }
                reader.Close();
                return _role;
            }
        }

        /// <summary>
        /// Query all roles
        /// </summary>
        /// <returns></returns>
        public RoleCollection QueryAllRole()
        {
            RoleCollection _roleCollection = new RoleCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "System_Roles";
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
                        Role _role = new Role();
                        _role.ID = Convert.ToInt32(reader["ID"]);
                        _role.RoleName = reader["Role_Name"].ToString();
                        _roleCollection.Add(_role);
                    }
                }
                reader.Close();
                return _roleCollection;
            }
        }

        public DataSet QueryRoleMenuRelation()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", SqlDbType.VarChar);
                _param.Value = "System_Roles_R_Menu";
                SqlParameter _param2 = command.Parameters.Add("@value2", SqlDbType.VarChar);
                _param2.Value = "";
                SqlParameter _param3 = command.Parameters.Add("@value3", SqlDbType.VarChar);
                _param3.Value = "";
                SqlParameter _param4 = command.Parameters.Add("@order_by1", SqlDbType.VarChar);
                _param4.Value = "1";
                SqlParameter _param5 = command.Parameters.Add("@order_by2", SqlDbType.TinyInt);
                _param5.Value = 0;
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                DataTable _dt = new DataTable();
                _dt.Columns.Add(new DataColumn("Menu_ID", typeof(int)));
                _dt.Columns.Add(new DataColumn("Role_ID", typeof(int)));
                if (reader.HasRows)
                {
                    
                    while (reader.Read())
                    {
                        DataRow _dr = _dt.NewRow();
                        _dr["Menu_ID"] = Convert.ToInt32(reader["Menu_ID"]);
                        _dr["Role_ID"] = Convert.ToInt32(reader["Role_ID"]);
                        _dt.Rows.Add(_dr);
                    }
                    
                }
                DataSet _ds = new DataSet();
                _ds.Tables.Add(_dt);
                return _ds;
            }
        }

        public DataSet QueryRoleUserRelation()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", SqlDbType.VarChar);
                _param.Value = "System_Roles_R_User";
                SqlParameter _param2 = command.Parameters.Add("@value2", SqlDbType.VarChar);
                _param2.Value = "";
                SqlParameter _param3 = command.Parameters.Add("@value3", SqlDbType.VarChar);
                _param3.Value = "";
                SqlParameter _param4 = command.Parameters.Add("@order_by1", SqlDbType.VarChar);
                _param4.Value = "1";
                SqlParameter _param5 = command.Parameters.Add("@order_by2", SqlDbType.TinyInt);
                _param5.Value = 0;
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                DataTable _dt = new DataTable();
                _dt.Columns.Add(new DataColumn("User_ID", typeof(int)));
                _dt.Columns.Add(new DataColumn("Role_ID", typeof(int)));
                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        DataRow _dr = _dt.NewRow();
                        _dr["User_ID"] = Convert.ToInt32(reader["User_ID"]);
                        _dr["Role_ID"] = Convert.ToInt32(reader["Role_ID"]);
                        _dt.Rows.Add(_dr);
                    }

                }
                DataSet _ds = new DataSet();
                _ds.Tables.Add(_dt);
                return _ds;
            }
        }

        public FuncMenuCollection QueryUserMenu(int userID)
        {
            FuncMenuCollection collection = new FuncMenuCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Users_Menu";
                SqlParameter _param = command.Parameters.Add("@User_Id", System.Data.SqlDbType.VarChar);
                _param.Value = userID;
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        FuncMenu funcMenu = new FuncMenu();
                        funcMenu.ItemID = Convert.ToInt32(reader["ID"]);
                        funcMenu.Text = reader["Text"].ToString();
                        funcMenu.Path = reader["Path"].ToString();
                        funcMenu.ParentID = Convert.ToInt32(reader["ParentID"]);
                        funcMenu.Sort = Convert.ToInt32(reader["Sort"]);
                        collection.Add(funcMenu);
                    }
                }
                return collection;
            }
        }

        /// <summary>
        /// Input roleID and output relation UserCollection
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public UserCollection QueryRoleUser(int roleID)
        {
            UserCollection _userCollection = new UserCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", SqlDbType.VarChar);
                _param.Value = "System_Roles_R_User";
                SqlParameter _param2 = command.Parameters.Add("@value2", SqlDbType.VarChar);
                _param2.Value = "2";
                SqlParameter _param3 = command.Parameters.Add("@value3", SqlDbType.VarChar);
                _param3.Value = roleID;
                SqlParameter _param4 = command.Parameters.Add("@order_by1", SqlDbType.VarChar);
                _param4.Value = "1";
                SqlParameter _param5 = command.Parameters.Add("@order_by2", SqlDbType.TinyInt);
                _param5.Value = 0;
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        User _user = new User();
                        UserAccess _userac = new UserAccess();
                        _user = _userac.Query(Convert.ToInt32(reader["User_ID"]))[0];
                        _userCollection.Add(_user);
                    }
                }
                reader.Close();
                return _userCollection;
            }
        }

        public RoleCollection QueryUserRole(int userID)
        {
            RoleCollection _roleCollection = new RoleCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", SqlDbType.VarChar);
                _param.Value = "System_Roles_R_User";
                SqlParameter _param2 = command.Parameters.Add("@value2", SqlDbType.VarChar);
                _param2.Value = "1";
                SqlParameter _param3 = command.Parameters.Add("@value3", SqlDbType.VarChar);
                _param3.Value = userID;
                SqlParameter _param4 = command.Parameters.Add("@order_by1", SqlDbType.VarChar);
                _param4.Value = "1";
                SqlParameter _param5 = command.Parameters.Add("@order_by2", SqlDbType.TinyInt);
                _param5.Value = 0;
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Role _role = new Role();
                        _role.ID = Convert.ToInt32(reader["Role_ID"]);
                        _roleCollection.Add(_role);
                    }
                }
                reader.Close();
                return _roleCollection;
            }
        }

        public FuncMenuCollection Query(int userId)
        {
            FuncMenuCollection collection = new FuncMenuCollection();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Users_Menu";

                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        FuncMenu funcMenu = new FuncMenu();
                        funcMenu.ItemID = Convert.ToInt32(reader["Menu_ID"]);
                        funcMenu.Text = reader["Text"].ToString();
                        funcMenu.Path = reader["Path"].ToString();
                        funcMenu.ParentID = Convert.ToInt32(reader["ParentID"]);
                        funcMenu.Sort = Convert.ToInt32(reader["Sort"]);
                        collection.Add(funcMenu);
                    }
                }
                return collection;
            }
        }

        public void UpdateRole(Role role)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_UPD_Table";
                SqlParameter _param = command.Parameters.Add("@value1", SqlDbType.VarChar);
                _param.Value = "System_Roles";
                SqlParameter _param2 = command.Parameters.Add("@value2", SqlDbType.VarChar);
                _param2.Value = "2";
                SqlParameter _param3 = command.Parameters.Add("@value3", SqlDbType.VarChar);
                _param3.Value = role.RoleName;
                SqlParameter _param4 = command.Parameters.Add("@value4", SqlDbType.VarChar);
                _param4.Value = "1";
                SqlParameter _param5 = command.Parameters.Add("@value5", SqlDbType.VarChar);
                _param5.Value = role.ID;
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        //public void Update(Menu menu)
        //{//TODO
        //    throw new NotImplementedException();
        //}

        public void Update(FuncMenuCollection menuCollection)
        {//TODO
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete Role
        /// </summary>
        /// <param name="roleID"></param>
        public void DeleteRole(int roleID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_DEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", SqlDbType.VarChar);
                _param.Value = "System_Roles";
                SqlParameter _param2 = command.Parameters.Add("@value2", SqlDbType.VarChar);
                _param2.Value = 1;
                SqlParameter _param3 = command.Parameters.Add("@value3", SqlDbType.VarChar);
                _param3.Value = roleID;
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void InsertRole(string roleName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_INS_Table";
                SqlParameter _param = command.Parameters.Add("@value1", SqlDbType.VarChar);
                _param.Value = "System_Roles";
                SqlParameter _param2 = command.Parameters.Add("@value2", SqlDbType.VarChar);
                _param2.Value = 2;
                SqlParameter _param3 = command.Parameters.Add("@value3", SqlDbType.VarChar);
                _param3.Value = roleName;
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

       
        //public bool SetRoles(User user, Role role)
        //{
        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        SqlCommand command = new SqlCommand();
        //        command.Connection = connection;
        //        command.CommandText = "SP_Frank_TEST";

        //        SqlParameter ColumnParam = command.Parameters.Add("@User_ID", SqlDbType.Int);
        //        ColumnParam.Value = user.UserID;
        //        SqlParameter ColumnParam1 = command.Parameters.Add("@Role_ID", SqlDbType.Int);
        //        ColumnParam1.Value = role.ID;
        //        command.CommandType = CommandType.StoredProcedure;
        //        connection.Open();
        //        string result = command.ExecuteScalar().ToString();
        //        if (result.Equals("0"))
        //            return false;
        //        else
        //            return true;
        //    }
        //}

        public bool SetRoles(User user, RoleCollection roleCollection)
        {
            bool IsSuccess= false;
            foreach (Role role in roleCollection)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = "SP_Frank_TEST";

                    SqlParameter ColumnParam = command.Parameters.Add("@User_ID", SqlDbType.Int);
                    ColumnParam.Value = user.UserID;
                    SqlParameter ColumnParam1 = command.Parameters.Add("@Role_ID", SqlDbType.Int);
                    ColumnParam1.Value = role.ID;
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    string result = command.ExecuteScalar().ToString();
                    if (result.Equals("0"))
                        IsSuccess = false;
                    else
                        IsSuccess = true;
                }
            }
            return IsSuccess;
        }

        public void DeleteRoleMenuRelation(int menuID,int roleID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_DEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", SqlDbType.VarChar);
                _param.Value = "System_Roles_R_Menu";
                SqlParameter _param2 = command.Parameters.Add("@value2", SqlDbType.VarChar);
                _param2.Value = "1,2";
                SqlParameter _param3 = command.Parameters.Add("@value3", SqlDbType.VarChar);
                _param3.Value = string.Format("{0},{1}",menuID, roleID);
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void InsertRoleMenuRelation(int menuID, int roleID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_INS_Table";
                SqlParameter _param = command.Parameters.Add("@value1", SqlDbType.VarChar);
                _param.Value = "System_Roles_R_Menu";
                SqlParameter _param2 = command.Parameters.Add("@value2", SqlDbType.VarChar);
                _param2.Value = "1,2";
                SqlParameter _param3 = command.Parameters.Add("@value3", SqlDbType.VarChar);
                _param3.Value = string.Format("{0},{1}",menuID,roleID);
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void InsertRoleUserRelation(int userID, int roleID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_INS_Table";
                SqlParameter _param = command.Parameters.Add("@value1", SqlDbType.VarChar);
                _param.Value = "System_Roles_R_User";
                SqlParameter _param2 = command.Parameters.Add("@value2", SqlDbType.VarChar);
                _param2.Value = "1,2";
                SqlParameter _param3 = command.Parameters.Add("@value3", SqlDbType.VarChar);
                _param3.Value = string.Format("{0},{1}", userID, roleID);
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void DeleteRoleUserRelation(int userID,int roleID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_DEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", SqlDbType.VarChar);
                _param.Value = "System_Roles_R_User";
                SqlParameter _param2 = command.Parameters.Add("@value2", SqlDbType.VarChar);
                _param2.Value = "1,2";
                SqlParameter _param3 = command.Parameters.Add("@value3", SqlDbType.VarChar);
                _param3.Value = string.Format("{0},{1}", userID, roleID);
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

    }
}
