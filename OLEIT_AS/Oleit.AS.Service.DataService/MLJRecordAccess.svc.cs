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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "MLJRecordAccess" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select MLJRecordAccess.svc or MLJRecordAccess.svc.cs at the Solution Explorer and start debugging.
    public class MLJRecordAccess : IMLJRecordAccess
    {
        string connectionString = ConfigurationManager.ConnectionStrings["AccountDataBase"].ConnectionString;
       
        public int Insert(MLJRecord record)
        {
            int _batchid = -1;
            using (SqlConnection connection_record = new SqlConnection(connectionString))
            {
                connection_record.Open();
                SqlTransaction tr = connection_record.BeginTransaction();
                try
                {
                    using (SqlCommand _command = new SqlCommand("SP_INS_Table", connection_record))
                    {
                        _command.Transaction = tr;
                        SqlParameter _param = _command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                        _param.Value = "MLJ_Journal_Batch";
                        SqlParameter _param2 = _command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                        _param2.Value = "2,3";
                        StringBuilder _content = new StringBuilder();
                        _content.AppendFormat("{0}{1}", record.Period.ID, ",");                     
                        _content.AppendFormat("{0}", (int)record.RecordStatus);
                        SqlParameter _param3 = _command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                        _param3.Value = _content.ToString();
                        _command.CommandType = System.Data.CommandType.StoredProcedure;
                        SqlDataReader reader = _command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                _batchid = Convert.ToInt32(reader["identity_ID"].ToString());
                            }
                        }
                        reader.Close();
                    }                  
                    if (!_batchid.Equals(-1))
                    {
                        foreach (MLJJournal journal in record.MLJJournalCollection)
                        {
                            using (SqlCommand _jcommand = new SqlCommand("SP_INS_Table", connection_record))
                            {                                
                                _jcommand.Transaction = tr;
                                SqlParameter _jparam1 = _jcommand.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                                _jparam1.Value = "MLJ_Journal";
                                SqlParameter _jparam2 = _jcommand.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                                _jparam2.Value = "2,3,4,5,6,7,8,9,10,11,12,13";
                                StringBuilder _jcontent = new StringBuilder();
                                _jcontent.AppendFormat("{0}{1}", _batchid, ",");
                                _jcontent.AppendFormat("{0}{1}", journal.EntityID, ",");
                                _jcontent.AppendFormat("{0}{1}", journal.Mon, ",");
                                _jcontent.AppendFormat("{0}{1}", journal.Tue, ",");
                                _jcontent.AppendFormat("{0}{1}", journal.Wed, ",");
                                _jcontent.AppendFormat("{0}{1}", journal.Thu, ",");
                                _jcontent.AppendFormat("{0}{1}", journal.Fri, ",");
                                _jcontent.AppendFormat("{0}{1}", journal.Sat, ",");
                                _jcontent.AppendFormat("{0}{1}", journal.Sun, ",");
                                _jcontent.AppendFormat("{0}{1}", journal.BaseCurrency, ",");
                                _jcontent.AppendFormat("{0}{1}", journal.ExchangeRate, ",");
                                _jcontent.AppendFormat("{0}", journal.EntryUser.UserID);
                                SqlParameter _jparam3 = _jcommand.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                                _jparam3.Value = _jcontent.ToString();
                                _jcommand.CommandType = System.Data.CommandType.StoredProcedure;                           
                                _jcommand.ExecuteNonQuery();
                            }

                        }
                    }
                    tr.Commit();

                }
                catch (Exception)
                {
                    tr.Rollback();
                    throw;
                }
            }

            return _batchid;
        }

        public void Insert(MLJJournal journal)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_INS_Table";

                SqlParameter _jparam1 = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _jparam1.Value = "MLJ_Journal";
                SqlParameter _jparam2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _jparam2.Value = "2,3,4,5,6,7,8,9,10,11,12,13";
                StringBuilder _jcontent = new StringBuilder();
                _jcontent.AppendFormat("{0}{1}", journal.MLJRecordID, ",");
                _jcontent.AppendFormat("{0}{1}", journal.EntityID, ",");
                _jcontent.AppendFormat("{0}{1}", journal.Mon, ",");
                _jcontent.AppendFormat("{0}{1}", journal.Tue, ",");
                _jcontent.AppendFormat("{0}{1}", journal.Wed, ",");
                _jcontent.AppendFormat("{0}{1}", journal.Thu, ",");
                _jcontent.AppendFormat("{0}{1}", journal.Fri, ",");
                _jcontent.AppendFormat("{0}{1}", journal.Sat, ",");
                _jcontent.AppendFormat("{0}{1}", journal.Sun, ",");
                _jcontent.AppendFormat("{0}{1}", journal.BaseCurrency, ",");
                _jcontent.AppendFormat("{0}{1}", journal.ExchangeRate, ",");
                _jcontent.AppendFormat("{0}", journal.EntryUser.UserID);
                SqlParameter _jparam3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _jparam3.Value = _jcontent.ToString();
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void InsertUserRMLJ(int userid,int entityid)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_INS_Table";

                SqlParameter _jparam1 = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _jparam1.Value = "User_R_MLJEntity";
                SqlParameter _jparam2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _jparam2.Value = "1,2";
                StringBuilder _jcontent = new StringBuilder();               
                _jcontent.AppendFormat("{0}{1}", userid, ",");
                _jcontent.AppendFormat("{0}", entityid);
                SqlParameter _jparam3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _jparam3.Value = _jcontent.ToString();
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                command.ExecuteNonQuery();
            }

        }

        public StatusColorCollection QueryStatusColor()
        {
            StatusColorCollection _collection = new StatusColorCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "MLJ_StatusColor";
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
                        StatusColor _color = new StatusColor();
                        _color.MLJStatus =Convert.ToInt32(reader["Status"]);
                        _color.StatusType = (Status)Convert.ToInt32(reader["Status"]);
                        _color.MLJColor = reader["Color"].ToString();                    
                        _collection.Add(_color);
                    }
                }
                reader.Close();
                return _collection;
            }
        }

        public StatusColorCollection QueryOneStatusColor(int statusid)
        {
            StatusColorCollection _collection = new StatusColorCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "MLJ_StatusColor";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "1";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = statusid;
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
                        StatusColor _color = new StatusColor();
                        _color.MLJStatus = Convert.ToInt32(reader["Status"]);
                        _color.MLJColor = reader["Color"].ToString();
                        _collection.Add(_color);
                    }
                }
                reader.Close();
                return _collection;
            }
        }

        public MLJRecordCollection Query(int MLJRecordID)
        {
            MLJRecordCollection _recordcollection = new MLJRecordCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "MLJ_Journal_Batch";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "1";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = MLJRecordID;
                SqlParameter _param4 = command.Parameters.Add("@order_by1", System.Data.SqlDbType.VarChar);
                _param4.Value = "1";
                SqlParameter _param5 = command.Parameters.Add("@order_by2", System.Data.SqlDbType.TinyInt);
                _param5.Value = 0;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if(reader.HasRows)
                {
                    while (reader.Read())
                    {
                        MLJRecord _re = new MLJRecord();
                        _re.MLJRecordID = Convert.ToInt32(reader["ID"]);
                        PeriodAccess _perioda = new PeriodAccess();
                        _re.Period = new PeriodCollection(_perioda.Query(Convert.ToInt32(reader["Period_ID"])))[0];
                        _re.RecordStatus = (RecordStatus)Convert.ToInt32(reader["Status"]);
                        if (reader["Approve_User"].ToString().Equals("") || reader["Approve_User"].ToString().Equals("0"))
                        {
                            _re.ApproveUser = new User();
                        }
                        else
                        {
                            UserAccess usera = new UserAccess();
                            _re.ApproveUser = new UserCollection(usera.Query(Convert.ToInt32(reader["Approve_User"])))[0];
                        }
                        _re.MLJJournalCollection = QueryJournal(MLJRecordID);
                        _recordcollection.Add(_re);
                    }
                }
                reader.Close();
                return _recordcollection;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="PeriodID"></param>
        /// <param name="EntityName">like condition</param>
        /// <returns></returns>
        public MLJJournalCollection Query(int PeriodID, string EntityName)
        {
            MLJJournalCollection _jurc = new MLJJournalCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_MLJ_Journal";
                SqlParameter _param = command.Parameters.Add("@PeriodID", System.Data.SqlDbType.VarChar);
                _param.Value = PeriodID;
                SqlParameter _param2 = command.Parameters.Add("@EntityName", System.Data.SqlDbType.VarChar);
                _param2.Value = EntityName;
               
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        MLJJournal journal = new MLJJournal();
                        journal.SequenceNo = Convert.ToInt32(reader["ID"]);
                        journal.MLJRecordID = Convert.ToInt32(reader["Batch_ID"]);
                        journal.EntityID = Convert.ToInt32(reader["Entity_ID"]);
                        journal.EntityName = reader["Entity_Name"].ToString();
                        UserAccess usera = new UserAccess();
                        if (!reader["UserID"].ToString().Equals(""))
                        {
                            journal.UserID = Convert.ToInt32(reader["UserID"].ToString());
                            journal.Personnel = new UserCollection(usera.Query(Convert.ToInt32(reader["UserID"])))[0];
                        }
                        else
                            journal.UserID = 0;
                       
                        journal.EntryUser = new UserCollection(usera.Query(Convert.ToInt32(reader["Entry_User"])))[0];

                        if (reader["IsAccount"].ToString().Equals("1"))
                        {
                            EntityAccess _entityacc = new EntityAccess();
                            journal.Account = new AccountCollection(_entityacc.QueryAccount(Convert.ToInt32(reader["Entity_ID"])))[0];
                        }
                        else
                            journal.Account = new Account();
                        journal.BaseCurrency = reader["Base_Currency"].ToString();
                        journal.ExchangeRate = Convert.ToInt32(reader["Exchange_Rate"]);
                        journal.Mon = Convert.ToInt32(reader["Mon"]);
                        journal.Tue = Convert.ToInt32(reader["Tue"]);
                        journal.Wed = Convert.ToInt32(reader["Wed"]);
                        journal.Thu = Convert.ToInt32(reader["Thu"]);
                        journal.Fri = Convert.ToInt32(reader["Fri"]);
                        journal.Sat = Convert.ToInt32(reader["Sat"]);
                        journal.Sun = Convert.ToInt32(reader["Sun"]);
                        _jurc.Add(journal);
                    }
                }
                reader.Close();
                return _jurc;
            }
        }

        public bool IsApprove(int PeriodID)
        {
            bool Is = false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "MLJ_Journal_Batch";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "2";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = PeriodID;
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
                        RecordStatus _status = new RecordStatus();
                        _status = (RecordStatus)Convert.ToInt32(reader["Status"]);
                        if (_status.Equals("Normal"))
                            Is = false;
                        else
                            Is = true;
                    }
                }
                else
                    Is = false;
                reader.Close();              
            }
            return Is;
        }

        public MLJJournalCollection QueryJournal(int MLJRecordID)
        {
            MLJJournalCollection collection = new MLJJournalCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "MLJ_Journal";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "2";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = MLJRecordID;
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
                        MLJJournal journal = new MLJJournal();
                        journal.SequenceNo = Convert.ToInt32(reader["ID"]);
                        journal.MLJRecordID = Convert.ToInt32(reader["Batch_ID"]);
                        journal.EntityID = Convert.ToInt32(reader["Entity_ID"]);
                        UserAccess usera = new UserAccess();
                        journal.EntryUser = new UserCollection(usera.Query(Convert.ToInt32(reader["Entry_User"])))[0];
                        journal.BaseCurrency = reader["Base_Currency"].ToString();
                        journal.ExchangeRate = Convert.ToInt32(reader["Exchange_Rate"]);
                        journal.Mon = Convert.ToInt32(reader["Mon"]);
                        journal.Tue = Convert.ToInt32(reader["Tue"]);
                        journal.Wed = Convert.ToInt32(reader["Wed"]);
                        journal.Thu = Convert.ToInt32(reader["Thu"]);
                        journal.Fri = Convert.ToInt32(reader["Fri"]);
                        journal.Sat = Convert.ToInt32(reader["Sat"]);
                        journal.Sun = Convert.ToInt32(reader["Sun"]);
                        collection.Add(journal);
                    }
                }
                reader.Close();
                return collection;
            }
        }
       
        public void Update(MLJRecord record)
        {
            throw new NotImplementedException();
        }

        public void UpdateJournal(MLJJournal journal)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_UPD_MLJ_Journal";
                SqlParameter _param = command.Parameters.Add("@MLJ_Journal_ID", System.Data.SqlDbType.Int);
                _param.Value = journal.SequenceNo;
                SqlParameter _param2 = command.Parameters.Add("@Batch_ID", System.Data.SqlDbType.Int);
                _param2.Value = journal.MLJRecordID;
                SqlParameter _param3 = command.Parameters.Add("@Entity_ID", System.Data.SqlDbType.Int);
                _param3.Value = journal.EntityID;
                SqlParameter _param4 = command.Parameters.Add("@Mon", System.Data.SqlDbType.Decimal);
                _param4.Value = journal.Mon;
                SqlParameter _param5 = command.Parameters.Add("@Tue", System.Data.SqlDbType.Decimal);
                _param5.Value = journal.Tue;
                SqlParameter _param6 = command.Parameters.Add("@Wed", System.Data.SqlDbType.Decimal);
                _param6.Value = journal.Wed;
                SqlParameter _param7 = command.Parameters.Add("@Thu", System.Data.SqlDbType.Decimal);
                _param7.Value = journal.Thu;
                SqlParameter _param8 = command.Parameters.Add("@Fri", System.Data.SqlDbType.Decimal);
                _param8.Value = journal.Fri;
                SqlParameter _param9 = command.Parameters.Add("@Sat", System.Data.SqlDbType.Decimal);
                _param9.Value = journal.Sat;
                SqlParameter _param10 = command.Parameters.Add("@Sun", System.Data.SqlDbType.Decimal);
                _param10.Value = journal.Sun;
                SqlParameter _param11 = command.Parameters.Add("@Base_Currency", System.Data.SqlDbType.VarChar);
                _param11.Value = journal.BaseCurrency;
                SqlParameter _param12 = command.Parameters.Add("@Exchange_Rate", System.Data.SqlDbType.Decimal);
                _param12.Value = journal.ExchangeRate;
                SqlParameter _param13 = command.Parameters.Add("@Entry_User", System.Data.SqlDbType.Int);
                _param13.Value = journal.EntryUser.UserID;               
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void UpdateJournal(MLJJournalCollection journalCollection)
        {
            throw new NotImplementedException();
        }

        public void ChangeStatus(int MLJRecordID, RecordStatus status,int userID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_UPD_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "MLJ_Journal_Batch";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "3,4";
                StringBuilder _content = new StringBuilder();
                _content.AppendFormat("{0}{1}", (int)status, ",");
                _content.AppendFormat("{0}", userID);
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = _content.ToString();
                SqlParameter _param4 = command.Parameters.Add("@value4", System.Data.SqlDbType.VarChar);
                _param4.Value = "1";
                SqlParameter _param5 = command.Parameters.Add("@value5", System.Data.SqlDbType.VarChar);
                _param5.Value = MLJRecordID;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void CheckAndAdd(int periodid,int userid)
        {            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "MLJ_Journal_Batch";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "2";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = periodid;
                SqlParameter _param4 = command.Parameters.Add("@order_by1", System.Data.SqlDbType.VarChar);
                _param4.Value = "1";
                SqlParameter _param5 = command.Parameters.Add("@order_by2", System.Data.SqlDbType.TinyInt);
                _param5.Value = 0;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                int _MLJEntityID = Convert.ToInt32(new Oleit.AS.Service.DataObject.PropertyCollection(new PropertyAccess().Query("MLJEntity"))[0].PropertyValue);
                EntityCollection _accountcollection = new EntityCollection(new EntityAccess().QueryAllMLJ(_MLJEntityID));
                if (!reader.HasRows)
                {
                    //First Get Peoperty MLJ EntityID

                    MLJRecord _MLJ = new MLJRecord();
                    _MLJ.Period.ID = periodid;
                    _MLJ.RecordStatus = RecordStatus.Normal;
                    foreach (Entity _entity in _accountcollection)
                    {
                        MLJJournal _journal = new MLJJournal();
                        _journal.EntityID = _entity.EntityID;
                        _journal.BaseCurrency = _entity.Currency.CurrencyID;
                        _journal.ExchangeRate = _entity.ExchangeRate;
                        _journal.Mon = 0; _journal.Tue = 0; _journal.Wed = 0; _journal.Thu = 0; _journal.Fri = 0; _journal.Sat = 0; _journal.Sun = 0;
                        _journal.EntryUser.UserID = userid;
                        _MLJ.MLJJournalCollection.Add(_journal);
                    }
                    Insert(_MLJ);
                }
                else
                {
                    MLJJournalCollection _mljjour = Query(periodid, "");
                    if (_accountcollection.Count > _mljjour.Count)
                    {
                        foreach (Entity _entity in _accountcollection)
                        {
                            bool _iscontain = false;
                            foreach (MLJJournal _jour in _mljjour)
                            {                                
                                if (_entity.EntityID.Equals(_jour.EntityID))
                                    _iscontain = true;
                            }
                            if (!_iscontain)
                            { 
                                MLJJournal _injour = new MLJJournal();
                                _injour.MLJRecordID = _mljjour[0].MLJRecordID;
                                _injour.EntityID = _entity.EntityID;
                                _injour.BaseCurrency = _entity.Currency.CurrencyID;
                                _injour.ExchangeRate = _entity.ExchangeRate;
                                _injour.EntryUser.UserID = userid;
                                Insert(_injour);
                            }
                        }
                    }

                }
                reader.Close();
            }
        }

        public void UpdateColor(StatusColor color)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_UPD_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "MLJ_StatusColor";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "2";
                StringBuilder _content = new StringBuilder();
                _content.AppendFormat("{0}", color.MLJColor);
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = _content.ToString();
                SqlParameter _param4 = command.Parameters.Add("@value4", System.Data.SqlDbType.VarChar);
                _param4.Value = "1";
                SqlParameter _param5 = command.Parameters.Add("@value5", System.Data.SqlDbType.VarChar);
                _param5.Value = color.MLJStatus;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void UpdateColor(StatusColorCollection collection)
        {
            foreach (StatusColor _color in collection)
            {
                UpdateColor(_color);
            }       
        }

        public DataSet GetUserMLJEntity()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "User_R_MLJEntity";
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
                SqlDataAdapter _dataap = new SqlDataAdapter();
                _dataap.SelectCommand = command;
                DataTable dt = new DataTable("MLJ");
                
                _dataap.Fill(dt);
                dt.Columns.Add("UserName");
                foreach (DataRow _row in dt.Rows)
                {
                    DataRow newRow = dt.NewRow();
                    UserAccess usera = new UserAccess();
                    _row["UserName"] = new UserCollection(usera.Query(Convert.ToInt32(_row["Userid"])))[0].UserName;
                }
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                return ds;
            }
        }

        public void UpdateUserMLJ(int userid , EntityCollection collection)
        {
            using (SqlConnection connection_record = new SqlConnection(connectionString))
            {
                connection_record.Open();
                SqlTransaction tr = connection_record.BeginTransaction();
                try
                {
                    using (SqlCommand _command = new SqlCommand("SP_DEL_Table", connection_record))
                    {
                        _command.Transaction = tr;
                        SqlParameter _param = _command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                        _param.Value = "User_R_MLJEntity";
                        SqlParameter _param2 = _command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                        _param2.Value = "1";
                        SqlParameter _param3 = _command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                        _param3.Value = userid;
                        _command.CommandType = System.Data.CommandType.StoredProcedure;
                        _command.ExecuteNonQuery();
                    }
                    foreach (Entity _entity in collection)
                    {
                        using (SqlCommand _ecommand = new SqlCommand("SP_INS_Table", connection_record))
                        {
                            _ecommand.Transaction = tr;
                            SqlParameter _jparam1 = _ecommand.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                            _jparam1.Value = "User_R_MLJEntity";
                            SqlParameter _jparam2 = _ecommand.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                            _jparam2.Value = "1,2";
                            StringBuilder _jcontent = new StringBuilder();
                            _jcontent.AppendFormat("{0}{1}", userid, ",");
                            _jcontent.AppendFormat("{0}", _entity.EntityID);
                            SqlParameter _jparam3 = _ecommand.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                            _jparam3.Value = _jcontent.ToString();
                            _ecommand.CommandType = System.Data.CommandType.StoredProcedure;
                            _ecommand.ExecuteNonQuery();
                        }
                    }

                    tr.Commit();

                }
                catch (Exception)
                {
                    tr.Rollback();
                    throw;
                }
            }
        }

        public DataSet GetAccountStatusLog(int entityid)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Log_Entity_Account_status";
                SqlParameter _param = command.Parameters.Add("@Entity_ID", System.Data.SqlDbType.Int);
                _param.Value = entityid;               
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                SqlDataAdapter _dataap = new SqlDataAdapter();
                _dataap.SelectCommand = command;
                DataTable dt = new DataTable("UserStatus");
                _dataap.Fill(dt);
                dt.Columns.Add("StatusName"); 
                foreach (DataRow _row in dt.Rows)
                {
                    DataRow newRow = dt.NewRow();
                    _row["StatusName"] = (Status)Convert.ToInt32(_row["Status"]);
                }
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                return ds;
            }
        }

        public DataSet GetMLJLog(int periodId,string entityName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_LOG_MLJ_Journal";
                SqlParameter _param = command.Parameters.Add("@PeriodID", System.Data.SqlDbType.Int);
                _param.Value = periodId;
                SqlParameter _param2 = command.Parameters.Add("@EntityName", System.Data.SqlDbType.VarChar);
                _param2.Value = entityName;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                SqlDataAdapter _dataap = new SqlDataAdapter();
                _dataap.SelectCommand = command;
                DataTable dt = new DataTable("MLJLOG");
                _dataap.Fill(dt);
              //  dt.Columns.Add("StatusName");
                //foreach (DataRow _row in dt.Rows)
                //{
                //    DataRow newRow = dt.NewRow();
                //    _row["StatusName"] = (Status)Convert.ToInt32(_row["Status"]);
                //}
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                return ds;
            }
        }

        public decimal GetMLJSum(int periodId, int entityid)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_MLJ_Journal_SUM";
                SqlParameter _param = command.Parameters.Add("@PeriodID", System.Data.SqlDbType.Int);
                _param.Value = periodId;
                SqlParameter _param2 = command.Parameters.Add("@EntityID", System.Data.SqlDbType.Int);
                _param2.Value = entityid;               
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                SqlDataAdapter _dataap = new SqlDataAdapter();
                _dataap.SelectCommand = command;
                DataTable dt = new DataTable("MLJSum");
                _dataap.Fill(dt);
                if (dt.Rows.Count > 0)
                    return Convert.ToDecimal(dt.Rows[0]["sum_something"]);
                else
                    return 0;
            }
            
        }
        
    }
}
