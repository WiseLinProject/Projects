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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "WeeklySummaryAccess" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select WeeklySummaryAccess.svc or WeeklySummaryAccess.svc.cs at the Solution Explorer and start debugging.
    public class WeeklySummaryAccess : IWeeklySummaryAccess
    {
        public void DoWork()
        {
        }
        string connectionString = ConfigurationManager.ConnectionStrings["AccountDataBase"].ConnectionString;

        public void Insert(WeeklySummary _week)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand _command = new SqlCommand();
                _command.Connection = connection;
                _command.CommandText = "SP_INS_Table";
                SqlParameter _param = _command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Weekly_Summary";
                SqlParameter _param2 = _command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
               
                StringBuilder _content = new StringBuilder();
                _content.AppendFormat("{0}{1}",_week.Period.ID , ",");
                _content.AppendFormat("{0}{1}", _week.Entity.EntityID, ",");
                _content.AppendFormat("{0}{1}", _week.BaseCurrency, ",");
                _content.AppendFormat("{0}{1}", _week.ExchangeRate, ",");
                _content.AppendFormat("{0}{1}", _week.BasePrevBalance, ",");
                _content.AppendFormat("{0}{1}", _week.SGDPrevBalance, ",");
                _content.AppendFormat("{0}{1}", _week.BaseWinAndLoss, ",");
                _content.AppendFormat("{0}{1}", _week.SGDWinAndLoss, ",");
                _content.AppendFormat("{0}{1}", _week.BaseTransfer, ",");
                _content.AppendFormat("{0}{1}", _week.SGDTransfer, ",");
                _content.AppendFormat("{0}{1}", _week.BaseTransaction, ",");
                _content.AppendFormat("{0}{1}", _week.SGDTransaction, ",");
                _content.AppendFormat("{0}{1}", _week.BaseBalance, ",");
                _content.AppendFormat("{0}{1}", _week.SGDBalance, ",");
                try
                {
                    if (_week.ConfirmUser.UserID.Equals(0))
                    {
                        _param2.Value = "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15";
                        _content.AppendFormat("{0}", (int)_week.Status);
                    }
                    else
                    {
                        _param2.Value = "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16";
                        _content.AppendFormat("{0}{1}", (int)_week.Status, ",");
                        _content.AppendFormat("{0}", _week.ConfirmUser.UserID, ",");
                    }
                }
                catch
                {
                    _param2.Value = "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15";
                    _content.AppendFormat("{0}", (int)_week.Status);
                }
                SqlParameter _param3 = _command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = _content.ToString();
                _command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                _command.ExecuteNonQuery();
            }
        }

        public void Insert(WeeklySummaryCollection weeklySummaryCollection)
        {
            foreach (WeeklySummary _week in weeklySummaryCollection)
            {
                Insert(_week);
            }
        }

        public WeeklySummaryCollection Query(int entityID)
        {
            WeeklySummaryCollection collection = new WeeklySummaryCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Weekly_Summary";
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
                        WeeklySummary _week = new WeeklySummary();
                        PeriodCollection _periodc = new PeriodCollection();
                        PeriodAccess _perioda = new PeriodAccess();
                        _periodc = _perioda.Query(Convert.ToInt32(reader["Period_ID"]));
                        _week.Period = _periodc[0];

                        EntityCollection _entityc = new EntityCollection();
                        EntityAccess _entity = new EntityAccess();
                        _entityc = _entity.Query(Convert.ToInt32(reader["Entity_Id"]));
                        _week.Entity = _entityc[0];

                        _week.BaseCurrency = reader["Base_Currency"].ToString();
                        _week.ExchangeRate = Convert.ToDecimal(reader["Exchange_Rate"].ToString());
                        _week.BasePrevBalance = Convert.ToDecimal(reader["Base_Prev_Balance"].ToString());
                        _week.SGDPrevBalance = Convert.ToDecimal(reader["SGD_Prev_Balance"].ToString());
                        _week.BaseWinAndLoss = Convert.ToDecimal(reader["Base_Win_Lose"].ToString());
                        _week.SGDWinAndLoss = Convert.ToDecimal(reader["SGD_Win_Lose"].ToString());
                        _week.BaseTransfer = Convert.ToDecimal(reader["Base_Transfer"].ToString());
                        _week.SGDTransfer = Convert.ToDecimal(reader["SGD_Transfer"].ToString());
                        _week.BaseTransaction = Convert.ToDecimal(reader["Base_Transaction"].ToString());
                        _week.SGDTransaction = Convert.ToDecimal(reader["SGD_Transaction"].ToString());
                        _week.BaseBalance = Convert.ToDecimal(reader["Base_Balance"].ToString());
                        _week.SGDBalance = Convert.ToDecimal(reader["SGD_Balance"].ToString());
                        _week.Status = (WeeklySummaryStatus)Convert.ToInt32(reader["Status"].ToString());

                        if (!reader["Confirm_User"].ToString().Equals(""))
                        {
                            UserAccess usera = new UserAccess();                           
                            _week.ConfirmUser = new UserCollection(usera.Query(Convert.ToInt32(reader["Confirm_User"])))[0];
                        }
                        else
                            _week.ConfirmUser = new User();
                        
                        collection.Add(_week);
                    }
                }
                reader.Close();
                return collection;
            }
        }

        public WeeklySummaryCollection QuerybyPeriod(int periodID)
        {
            WeeklySummaryCollection collection = new WeeklySummaryCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Weekly_Summary";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "1";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = periodID;
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
                        WeeklySummary _week = new WeeklySummary();
                        PeriodCollection _periodc = new PeriodCollection();
                        PeriodAccess _perioda = new PeriodAccess();
                        _periodc = _perioda.Query(Convert.ToInt32(reader["Period_ID"]));
                        _week.Period = _periodc[0];

                        EntityCollection _entityc = new EntityCollection();
                        EntityAccess _entity = new EntityAccess();
                        _entityc = _entity.Query(Convert.ToInt32(reader["Entity_Id"]));
                        _week.Entity = _entityc[0];

                        _week.BaseCurrency = reader["Base_Currency"].ToString();
                        _week.ExchangeRate = Convert.ToDecimal(reader["Exchange_Rate"].ToString());
                        _week.BasePrevBalance = Convert.ToDecimal(reader["Base_Prev_Balance"].ToString());
                        _week.SGDPrevBalance = Convert.ToDecimal(reader["SGD_Prev_Balance"].ToString());
                        _week.BaseWinAndLoss = Convert.ToDecimal(reader["Base_Win_Lose"].ToString());
                        _week.SGDWinAndLoss = Convert.ToDecimal(reader["SGD_Win_Lose"].ToString());
                        _week.BaseTransfer = Convert.ToDecimal(reader["Base_Transfer"].ToString());
                        _week.SGDTransfer = Convert.ToDecimal(reader["SGD_Transfer"].ToString());
                        _week.BaseTransaction = Convert.ToDecimal(reader["Base_Transaction"].ToString());
                        _week.SGDTransaction = Convert.ToDecimal(reader["SGD_Transaction"].ToString());
                        _week.BaseBalance = Convert.ToDecimal(reader["Base_Balance"].ToString());
                        _week.SGDBalance = Convert.ToDecimal(reader["SGD_Balance"].ToString());
                        _week.Status = (WeeklySummaryStatus)Convert.ToInt32(reader["Status"].ToString());
                        try
                        {
                            UserAccess usera = new UserAccess();                            
                            _week.ConfirmUser = new UserCollection(usera.Query(Convert.ToInt32(reader["Confirm_User"])))[0];
                        }
                        catch
                        {
                            _week.ConfirmUser = new User();
                        }
                        collection.Add(_week);
                    }
                }
                reader.Close();
                return collection;
            }
        }

        public WeeklySummaryCollection Query(int periodID,int entityID)
        {
            WeeklySummaryCollection collection = new WeeklySummaryCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Weekly_Summary";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "1,2";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = periodID + "," + entityID;
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
                        WeeklySummary _week = new WeeklySummary();                      
                        PeriodAccess _perioda = new PeriodAccess();
                        _week.Period = new PeriodCollection(_perioda.Query(Convert.ToInt32(reader["Period_ID"])))[0]; //_periodc[0];
                        
                        EntityAccess _entity = new EntityAccess();                       
                        _week.Entity = new EntityCollection(_entity.Query(Convert.ToInt32(reader["Entity_Id"])))[0];

                        _week.BaseCurrency = reader["Base_Currency"].ToString();
                        _week.ExchangeRate = Convert.ToDecimal(reader["Exchange_Rate"].ToString());
                        _week.BasePrevBalance = Convert.ToDecimal(reader["Base_Prev_Balance"].ToString());
                        _week.SGDPrevBalance = Convert.ToDecimal(reader["SGD_Prev_Balance"].ToString());
                        _week.BaseWinAndLoss = Convert.ToDecimal(reader["Base_Win_Lose"].ToString());
                        _week.SGDWinAndLoss = Convert.ToDecimal(reader["SGD_Win_Lose"].ToString());
                        _week.BaseTransfer = Convert.ToDecimal(reader["Base_Transfer"].ToString());
                        _week.SGDTransfer = Convert.ToDecimal(reader["SGD_Transfer"].ToString());
                        _week.BaseTransaction = Convert.ToDecimal(reader["Base_Transaction"].ToString());
                        _week.SGDTransaction = Convert.ToDecimal(reader["SGD_Transaction"].ToString());
                        _week.BaseBalance = Convert.ToDecimal(reader["Base_Balance"].ToString());
                        _week.SGDBalance = Convert.ToDecimal(reader["SGD_Balance"].ToString());
                        _week.Status = (WeeklySummaryStatus)Convert.ToInt32(reader["Status"].ToString());
                        string userinfo = reader["Confirm_User"].ToString();
                        try
                        {
                            UserAccess usera = new UserAccess();
                            _week.ConfirmUser = new UserCollection(usera.Query(Convert.ToInt32(reader["Confirm_User"])))[0];
                        }
                        catch
                        {
                            _week.ConfirmUser = new User();
                        }
                        collection.Add(_week);
                    }
                }
                reader.Close();
                return collection;
            }
        }

        public WeeklySummaryCollection Query(int periodID, int[] entity) 
        {
            WeeklySummaryCollection _collection = new WeeklySummaryCollection();
            foreach (int _entityid in entity)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = "SP_SEL_Table";
                    SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                    _param.Value = "Weekly_Summary";
                    SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                    _param2.Value = "1,2";
                    SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                    _param3.Value = periodID + "," + _entityid;
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
                            WeeklySummary _week = new WeeklySummary();
                            PeriodAccess _perioda = new PeriodAccess();
                            _week.Period = new PeriodCollection(_perioda.Query(Convert.ToInt32(reader["Period_ID"])))[0]; //_periodc[0];

                            EntityAccess _weekentity = new EntityAccess();
                            _week.Entity = new EntityCollection(_weekentity.Query(Convert.ToInt32(reader["Entity_Id"])))[0];

                            _week.BaseCurrency = reader["Base_Currency"].ToString();
                            _week.ExchangeRate = Convert.ToDecimal(reader["Exchange_Rate"].ToString());
                            _week.BasePrevBalance = Convert.ToDecimal(reader["Base_Prev_Balance"].ToString());
                            _week.SGDPrevBalance = Convert.ToDecimal(reader["SGD_Prev_Balance"].ToString());
                            _week.BaseWinAndLoss = Convert.ToDecimal(reader["Base_Win_Lose"].ToString());
                            _week.SGDWinAndLoss = Convert.ToDecimal(reader["SGD_Win_Lose"].ToString());
                            _week.BaseTransfer = Convert.ToDecimal(reader["Base_Transfer"].ToString());
                            _week.SGDTransfer = Convert.ToDecimal(reader["SGD_Transfer"].ToString());
                            _week.BaseTransaction = Convert.ToDecimal(reader["Base_Transaction"].ToString());
                            _week.SGDTransaction = Convert.ToDecimal(reader["SGD_Transaction"].ToString());
                            _week.BaseBalance = Convert.ToDecimal(reader["Base_Balance"].ToString());
                            _week.SGDBalance = Convert.ToDecimal(reader["SGD_Balance"].ToString());
                            _week.Status = (WeeklySummaryStatus)Convert.ToInt32(reader["Status"].ToString());
                            string userinfo = reader["Confirm_User"].ToString();
                            try
                            {
                                UserAccess usera = new UserAccess();
                                _week.ConfirmUser = new UserCollection(usera.Query(Convert.ToInt32(reader["Confirm_User"])))[0];
                            }
                            catch
                            {
                                _week.ConfirmUser = new User();
                            }
                            _collection.Add(_week);
                        }
                    }
                    
                    reader.Close();
                   
                }
            } 
            
            return _collection;
            
        }

        public WeeklySummaryCollection QueryAll()
        {
            WeeklySummaryCollection collection = new WeeklySummaryCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Weekly_Summary";
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
                        WeeklySummary week = new WeeklySummary(); 
                      
                        PeriodAccess _perioda = new PeriodAccess();    
                        week.Period = new PeriodCollection(_perioda.Query(Convert.ToInt32(reader["Period_ID"])))[0];

                        EntityAccess _entity = new EntityAccess();                      
                        week.Entity = new EntityCollection(_entity.Query(Convert.ToInt32(reader["Entity_Id"])))[0];

                        week.BaseCurrency = reader["Base_Currency"].ToString();
                        week.ExchangeRate = Convert.ToDecimal(reader["Exchange_Rate"].ToString());
                        week.BasePrevBalance = Convert.ToDecimal(reader["Base_Prev_Balance"].ToString());
                        week.SGDPrevBalance = Convert.ToDecimal(reader["SGD_Prev_Balance"].ToString());
                        week.BaseWinAndLoss = Convert.ToDecimal(reader["Base_Win_Lose"].ToString());
                        week.SGDWinAndLoss = Convert.ToDecimal(reader["SGD_Win_Lose"].ToString());
                        week.BaseTransfer = Convert.ToDecimal(reader["Base_Transfer"].ToString());
                        week.SGDTransfer = Convert.ToDecimal(reader["SGD_Transfer"].ToString());
                        week.BaseTransaction = Convert.ToDecimal(reader["Base_Transaction"].ToString());
                        week.SGDTransaction = Convert.ToDecimal(reader["SGD_Transaction"].ToString());
                        week.BaseBalance = Convert.ToDecimal(reader["Base_Balance"].ToString());
                        week.SGDBalance = Convert.ToDecimal(reader["SGD_Balance"].ToString());
                        week.Status = (WeeklySummaryStatus)Convert.ToInt32(reader["Status"].ToString());
                        try
                        {
                            UserAccess usera = new UserAccess();                           
                            week.ConfirmUser = new UserCollection(usera.Query(Convert.ToInt32(reader["Confirm_User"])))[0];
                        }
                        catch
                        {
                            week.ConfirmUser = new User();
                        }
                        collection.Add(week);
                    }
                }
                reader.Close();
                return collection;
            }
        }

        public void Update(WeeklySummary _week)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand _command = new SqlCommand();
                _command.Connection = connection;
                _command.CommandText = "SP_UPD_Table";
                SqlParameter _param = _command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Weekly_Summary";
                SqlParameter _param2 = _command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "3,4,5,6,7,8,9,10,11,12,13,14,15,16";
                StringBuilder _content = new StringBuilder();
                _content.AppendFormat("{0}{1}", _week.BaseCurrency , ",");
                _content.AppendFormat("{0}{1}", _week.ExchangeRate, ",");
                _content.AppendFormat("{0}{1}", _week.BasePrevBalance, ",");
                _content.AppendFormat("{0}{1}", _week.SGDPrevBalance, ",");
                _content.AppendFormat("{0}{1}", _week.BaseWinAndLoss, ",");
                _content.AppendFormat("{0}{1}", _week.SGDWinAndLoss, ",");
                _content.AppendFormat("{0}{1}", _week.BaseTransfer, ",");
                _content.AppendFormat("{0}{1}", _week.SGDTransfer, ",");
                _content.AppendFormat("{0}{1}", _week.BaseTransaction, ",");
                _content.AppendFormat("{0}{1}", _week.SGDTransaction, ",");
                _content.AppendFormat("{0}{1}", _week.BaseBalance, ",");
                _content.AppendFormat("{0}{1}", _week.SGDBalance, ",");
               
                try
                {
                    if (_week.ConfirmUser.UserID.Equals(0))
                    {
                        _param2.Value = "3,4,5,6,7,8,9,10,11,12,13,14,15";
                        _content.AppendFormat("{0}", (int)_week.Status);
                    }
                    else
                    {
                        _param2.Value = "3,4,5,6,7,8,9,10,11,12,13,14,15,16";
                        _content.AppendFormat("{0}{1}", (int)_week.Status,",");
                        _content.AppendFormat("{0}", _week.ConfirmUser.UserID, ",");
                    }
                }
                catch
                {
                    _param2.Value = "3,4,5,6,7,8,9,10,11,12,13,14,15";
                    _content.AppendFormat("{0}", (int)_week.Status);
                }

                SqlParameter _param3 = _command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = _content.ToString();
                SqlParameter _param4 = _command.Parameters.Add("@value4", System.Data.SqlDbType.VarChar);
                _param4.Value = "1,2";
                SqlParameter _param5 = _command.Parameters.Add("@value5", System.Data.SqlDbType.VarChar);
                _param5.Value = _week.Period.ID+","+_week.Entity.EntityID;
                _command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
               _command.ExecuteNonQuery();
            }
        }

        public void Update(WeeklySummaryCollection weeklySummaryCollection)
        {
            foreach (WeeklySummary _week in weeklySummaryCollection)
            {
                Update(_week);
            }
        }

        public void SubTotalConfirm(WeeklySummaryCollection weeklySummaryCollection)
        {
             foreach (WeeklySummary _week in weeklySummaryCollection)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand _command = new SqlCommand();
                    _command.Connection = connection;
                    _command.CommandText = "SP_UPD_Table";
                    SqlParameter _param = _command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                    _param.Value = "Weekly_Summary";
                    SqlParameter _param2 = _command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                    _param2.Value = "9,10,13,14,15,16";
                    StringBuilder _content = new StringBuilder();                  
                    _content.AppendFormat("{0}{1}", _week.BaseTransfer, ",");
                    _content.AppendFormat("{0}{1}", _week.SGDTransfer, ",");
                    _content.AppendFormat("{0}{1}", _week.BaseBalance, ",");
                    _content.AppendFormat("{0}{1}", _week.SGDBalance, ",");
                    _content.AppendFormat("{0}{1}", (int)_week.Status, ",");
                    _content.AppendFormat("{0}", _week.ConfirmUser.UserID, ",");
                    SqlParameter _param3 = _command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                    _param3.Value = _content.ToString();
                    SqlParameter _param4 = _command.Parameters.Add("@value4", System.Data.SqlDbType.VarChar);
                    _param4.Value = "1,2";
                    SqlParameter _param5 = _command.Parameters.Add("@value5", System.Data.SqlDbType.VarChar);
                    _param5.Value = _week.Period.ID + "," + _week.Entity.EntityID;
                    _command.CommandType = System.Data.CommandType.StoredProcedure;
                    connection.Open();
                    _command.ExecuteNonQuery();
                }
            }
        }

        public void WinLossConfirm(WeeklySummaryCollection weeklySummaryCollection)
        {
            foreach (WeeklySummary _week in weeklySummaryCollection)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand _command = new SqlCommand();
                    _command.Connection = connection;
                    _command.CommandText = "SP_UPD_Table";
                    SqlParameter _param = _command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                    _param.Value = "Weekly_Summary";
                    SqlParameter _param2 = _command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                    _param2.Value = "7,8,10,13,14,15,16";
                    StringBuilder _content = new StringBuilder();
                    _content.AppendFormat("{0}{1}", _week.BaseTransfer, ",");
                    _content.AppendFormat("{0}{1}", _week.SGDTransfer, ",");
                    _content.AppendFormat("{0}{1}", _week.BaseBalance, ",");
                    _content.AppendFormat("{0}{1}", _week.SGDBalance, ",");
                    _content.AppendFormat("{0}{1}", (int)_week.Status, ",");
                    _content.AppendFormat("{0}", _week.ConfirmUser.UserID, ",");
                    SqlParameter _param3 = _command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                    _param3.Value = _content.ToString();
                    SqlParameter _param4 = _command.Parameters.Add("@value4", System.Data.SqlDbType.VarChar);
                    _param4.Value = "1,2";
                    SqlParameter _param5 = _command.Parameters.Add("@value5", System.Data.SqlDbType.VarChar);
                    _param5.Value = _week.Period.ID + "," + _week.Entity.EntityID;
                    _command.CommandType = System.Data.CommandType.StoredProcedure;
                    connection.Open();
                    _command.ExecuteNonQuery();
                }
            }
        }
        
        public bool IsWeeklyConfirm(int periodID, int entityID)
        {
            WeeklySummaryCollection collection = new WeeklySummaryCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Weekly_Summary";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "1,2";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = periodID + "," + entityID;
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
                        WeeklySummary _week = new WeeklySummary();
                        PeriodAccess _perioda = new PeriodAccess();
                        _week.Period = new PeriodCollection(_perioda.Query(Convert.ToInt32(reader["Period_ID"])))[0]; //_periodc[0];

                        EntityAccess _entity = new EntityAccess();
                        _week.Entity = new EntityCollection(_entity.Query(Convert.ToInt32(reader["Entity_Id"])))[0];
                        _week.BaseCurrency = reader["Base_Currency"].ToString();
                        _week.ExchangeRate = Convert.ToDecimal(reader["Exchange_Rate"].ToString());
                        _week.BasePrevBalance = Convert.ToDecimal(reader["Base_Prev_Balance"].ToString());
                        _week.SGDPrevBalance = Convert.ToDecimal(reader["SGD_Prev_Balance"].ToString());
                        _week.BaseWinAndLoss = Convert.ToDecimal(reader["Base_Win_Lose"].ToString());
                        _week.SGDWinAndLoss = Convert.ToDecimal(reader["SGD_Win_Lose"].ToString());
                        _week.BaseTransfer = Convert.ToDecimal(reader["Base_Transfer"].ToString());
                        _week.SGDTransfer = Convert.ToDecimal(reader["SGD_Transfer"].ToString());
                        _week.BaseTransaction = Convert.ToDecimal(reader["Base_Transaction"].ToString());
                        _week.SGDTransaction = Convert.ToDecimal(reader["SGD_Transaction"].ToString());
                        _week.BaseBalance = Convert.ToDecimal(reader["Base_Balance"].ToString());
                        _week.SGDBalance = Convert.ToDecimal(reader["SGD_Balance"].ToString());
                        _week.Status = (WeeklySummaryStatus)Convert.ToInt32(reader["Status"].ToString());
                        string userinfo = reader["Confirm_User"].ToString();
                        try
                        {
                            UserAccess usera = new UserAccess();
                            _week.ConfirmUser = new UserCollection(usera.Query(Convert.ToInt32(reader["Confirm_User"])))[0];
                        }
                        catch
                        {
                            _week.ConfirmUser = new User();
                        }
                        collection.Add(_week);
                    }
                    reader.Close();
                    if (collection.Count > 0)
                    {
                        if (collection[0].ConfirmUser.UserID.Equals(0))
                            return false;
                        else
                            return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    reader.Close();
                    return false;
                }
            }
        }


    }
}
