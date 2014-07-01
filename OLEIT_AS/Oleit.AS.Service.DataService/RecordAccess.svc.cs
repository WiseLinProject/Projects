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
using System.Data;

namespace Oleit.AS.Service.DataService.WCFService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "RecordAccess" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select RecordAccess.svc or RecordAccess.svc.cs at the Solution Explorer and start debugging.
    public class RecordAccess : IRecordAccess
    {
        string connectionString = ConfigurationManager.ConnectionStrings["AccountDataBase"].ConnectionString;
        public void DoWork()
        {
        }

        /// <summary>
        ///  WinAndLoss = 1, //1 Transfer, //2 Transaction, //3      
        /// </summary>     
        public int Insert(Record record,JournalCollection jcollection)
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
                        _param.Value = "Journal_Batch";
                        SqlParameter _param2 = _command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                        _param2.Value = "2,3,5,6";
                        StringBuilder _content = new StringBuilder();
                        _content.AppendFormat("{0}{1}",record.Period.ID , ",");
                        _content.AppendFormat("{0}{1}", (int)record.Type , ",");
                        _content.AppendFormat("{0}{1}", (int)record.RecordStatus, ",");
                        _content.AppendFormat("{0}", (int)record.EntityID);
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
                    //SqlCommand _command = new SqlCommand();
                    //_command.Connection = connection_record;
                    //_command.CommandText = "SP_INS_Table";

                    if (!_batchid.Equals(-1))
                    {
                        foreach (Journal journal in jcollection)
                        {
                            using (SqlCommand _jcommand = new SqlCommand("SP_INS_Table", connection_record))
                            {
                                //SqlCommand _jcommand = new SqlCommand();
                                //_jcommand.Connection = connection_journal;
                                //_jcommand.Connection = connection_record;
                                //_jcommand.CommandText = "SP_INS_Table";
                                _jcommand.Transaction = tr;
                                SqlParameter _jparam1 = _jcommand.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                                _jparam1.Value = "Journal";
                                SqlParameter _jparam2 = _jcommand.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                                _jparam2.Value = "2,3,4,5,6,7,8";
                                StringBuilder _jcontent = new StringBuilder();
                                _jcontent.AppendFormat("{0}{1}", _batchid, ",");
                                _jcontent.AppendFormat("{0}{1}", journal.EntityID, ",");
                                _jcontent.AppendFormat("{0}{1}", journal.BaseCurrency, ",");
                                _jcontent.AppendFormat("{0}{1}", journal.ExchangeRate, ",");
                                _jcontent.AppendFormat("{0}{1}", journal.BaseAmount, ",");
                                _jcontent.AppendFormat("{0}{1}", journal.SGDAmount, ",");
                                _jcontent.AppendFormat("{0}", journal.EntryUser.UserID);
                                SqlParameter _jparam3 = _jcommand.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                                _jparam3.Value = _jcontent.ToString();
                                _jcommand.CommandType = System.Data.CommandType.StoredProcedure;
                                //connection_journal.Open();
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

        public void InsertDeletionLog(JournalCollection jcollection)
        {
            using (SqlConnection connection_record = new SqlConnection(connectionString))
            {
                try
                {
                    foreach (Journal journal in jcollection)
                    {
                        using (SqlCommand _command = new SqlCommand("SP_INS_Table", connection_record))
                        {
                            SqlParameter _param = _command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                            _param.Value = "Log_Journal";
                            SqlParameter _param2 = _command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                            _param2.Value = "2,3,4,5,6,7,8,9,11";
                            StringBuilder _content = new StringBuilder();
                            _content.AppendFormat("{0},", journal.SequenceNo);
                            _content.AppendFormat("{0},", journal.RecordID);
                            _content.AppendFormat("{0},", journal.EntityID);
                            _content.AppendFormat("{0},", journal.BaseCurrency);
                            _content.AppendFormat("{0},", journal.ExchangeRate);
                            _content.AppendFormat("{0},", journal.BaseAmount);
                            _content.AppendFormat("{0},", journal.SGDAmount);
                            _content.AppendFormat("{0},", journal.EntryUser.UserID);
                            _content.Append("2");
                            SqlParameter _param3 = _command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                            _param3.Value = _content.ToString();
                            _command.CommandType = System.Data.CommandType.StoredProcedure;
                            connection_record.Open();
                            _command.ExecuteNonQuery();
                            connection_record.Close();
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public void Insert(RecordCollection recordCollection)
        {
            throw new NotImplementedException();
        }

        public RecordCollection Query(int recordID)
        {
            RecordCollection collection = new RecordCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Journal_Batch";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "1";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = recordID;
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
                        Record record = new Record();
                        record.RecordID = Convert.ToInt32(reader["ID"]);
                        PeriodAccess _perioda = new PeriodAccess();                       
                        record.Period = new PeriodCollection(_perioda.Query(Convert.ToInt32(reader["Period_ID"])))[0];                      
                        record.Type = (RecordType)(Convert.ToInt32(reader["Type"]));
                        record.RecordStatus = (RecordStatus)(Convert.ToInt32(reader["status"]));
                        record.JournalCollection = QueryJournal(record.RecordID);
                        collection.Add(record);
                    }
                }
                reader.Close();
                return collection;
            }
        }

        public RecordCollection Query(RecordType type)
        {
            RecordCollection collection = new RecordCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Journal_Batch";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "3";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = (int)type;
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
                        Record record = new Record();
                        record.RecordID = Convert.ToInt32(reader["ID"]);                       
                        PeriodAccess _perioda = new PeriodAccess();
                        record.Period = new PeriodCollection(_perioda.Query(Convert.ToInt32(reader["Period_ID"])))[0];
                        record.Type = (RecordType)(Convert.ToInt32(reader["Type"]));
                        record.RecordStatus = (RecordStatus)(Convert.ToInt32(reader["status"]));
                        collection.Add(record);
                    }
                }
                reader.Close();
                return collection;
            }
        }

        public RecordCollection Query(Period period)
        {
            RecordCollection collection = new RecordCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Journal_Batch";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "2";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = period.ID;
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
                        Record record = new Record();
                        record.RecordID = Convert.ToInt32(reader["ID"]);                      
                        PeriodAccess _perioda = new PeriodAccess();                      
                        record.Period = new PeriodCollection(_perioda.Query(Convert.ToInt32(reader["Period_ID"])))[0];
                        record.Type = (RecordType)(Convert.ToInt32(reader["Type"]));
                        record.RecordStatus = (RecordStatus)(Convert.ToInt32(reader["status"]));
                        collection.Add(record);
                    }
                }
                reader.Close();
                return collection;
            }
        }

        public RecordCollection Query(RecordStatus status)
        {
            RecordCollection collection = new RecordCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Journal_Batch";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "5";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = (int)status;
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
                        Record record = new Record();
                        record.RecordID = Convert.ToInt32(reader["ID"]);                       
                        PeriodAccess _perioda = new PeriodAccess();                      
                        record.Period = new PeriodCollection(_perioda.Query(Convert.ToInt32(reader["Period_ID"])))[0];
                        record.Type = (RecordType)(Convert.ToInt32(reader["Type"]));
                        record.RecordStatus = (RecordStatus)(Convert.ToInt32(reader["status"]));
                        collection.Add(record);
                    }
                }
                reader.Close();
                return collection;
            }
        }

        public RecordCollection Query(int entityid, int periodid)
        {
            RecordCollection collection = new RecordCollection();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Journal_Batch";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "2,6";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = periodid+","+entityid;
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
                        Record record = new Record();
                        record.RecordID = Convert.ToInt32(reader["ID"]);
                        PeriodAccess _perioda = new PeriodAccess();
                        record.Period = new PeriodCollection(_perioda.Query(Convert.ToInt32(reader["Period_ID"])))[0];
                        record.Type = (RecordType)(Convert.ToInt32(reader["Type"]));
                        record.RecordStatus = (RecordStatus)(Convert.ToInt32(reader["status"]));
                        record.JournalCollection = QueryJournal(record.RecordID);
                        collection.Add(record);
                    }
                }
                reader.Close();
                return collection;
            }
        }

        public RecordCollection QueryAll()
        {
            RecordCollection collection = new RecordCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Journal_Batch";
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
                        Record record = new Record();
                        record.RecordID = Convert.ToInt32(reader["ID"]);
                        PeriodCollection _periodc = new PeriodCollection();
                        PeriodAccess _perioda = new PeriodAccess();
                        _periodc = _perioda.Query(Convert.ToInt32(reader["Period_ID"]));
                        record.Period = _periodc[0];
                        record.Type = (RecordType)(Convert.ToInt32(reader["Type"]));
                        record.RecordStatus = (RecordStatus)(Convert.ToInt32(reader["status"]));
                        collection.Add(record);
                    }
                }
                reader.Close();
                return collection;
            }
        }

        public int QueryRecordID(int entityid,int periodid)
        {
            int Recordid = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Journal";
                SqlParameter _param = command.Parameters.Add("@Entity", System.Data.SqlDbType.VarChar);
                _param.Value = entityid;
                SqlParameter _param2 = command.Parameters.Add("@Period_ID", System.Data.SqlDbType.VarChar);
                _param2.Value = periodid;                
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Recordid = Convert.ToInt32(reader["Batch_ID"]);                       
                    }
                }
                reader.Close();
                return Recordid;
            }
        }

        public DataSet LoadWinAndLossLog(int peroidID, int entityID)
        {
            int _recordID = loadRecordID(peroidID, entityID);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_LOG_Journal";
                SqlParameter _param = command.Parameters.Add("@Batch_ID", System.Data.SqlDbType.VarChar);
                _param.Value = _recordID;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                DataTable _dt = new DataTable();
                _dt.Columns.Add(new DataColumn("ID", typeof(int)));                
                _dt.Columns.Add(new DataColumn("BatchID", typeof(int)));
                _dt.Columns.Add(new DataColumn("JournalID", typeof(int)));
                _dt.Columns.Add(new DataColumn("EntityID", typeof(int)));
                _dt.Columns.Add(new DataColumn("BaseCurrency", typeof(string)));
                _dt.Columns.Add(new DataColumn("ExchangeRate", typeof(decimal)));
                _dt.Columns.Add(new DataColumn("BaseAmount", typeof(decimal)));
                _dt.Columns.Add(new DataColumn("SGDAmount", typeof(decimal)));
                _dt.Columns.Add(new DataColumn("ModifyUser", typeof(int)));
                _dt.Columns.Add(new DataColumn("LogDate", typeof(DateTime)));
                _dt.Columns.Add(new DataColumn("EditType", typeof(string)));
                _dt.Columns.Add(new DataColumn("UserAccount", typeof(string)));
                _dt.Columns.Add(new DataColumn("EntityName", typeof(string)));
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        DataRow _dr = _dt.NewRow();
                        _dr["ID"] = Convert.ToInt32(reader["ID"]);
                        _dr["BatchID"] = Convert.ToInt32(reader["Batch_ID"]);
                        _dr["JournalID"] = Convert.ToInt32(reader["Journal_Id"]);
                        _dr["EntityID"] = Convert.ToInt32(reader["Entity"]);
                        _dr["BaseCurrency"] = reader["Base_Currency"].ToString();
                        _dr["ExchangeRate"] = Convert.ToInt32(reader["Exchange_Rate"]);
                        _dr["BaseAmount"] = Convert.ToInt32(reader["Base_Amount"]);
                        _dr["SGDAmount"] = Convert.ToInt32(reader["SGD_Amount"]);
                        _dr["ModifyUser"] = Convert.ToInt32(reader["Modify_User"]);
                        _dr["LogDate"] = Convert.ToDateTime(reader["LogDate"]);
                        _dr["EditType"] = reader["EditType"].ToString();
                        _dr["UserAccount"] = reader["UserAccount"].ToString();
                        _dr["EntityName"] = reader["Entity_Name"].ToString();
                        _dt.Rows.Add(_dr);
                    }
                }
                DataSet _ds = new DataSet();
                _ds.Tables.Add(_dt);
                reader.Close();
                connection.Close();
                return _ds;
            }
        }

        private int loadRecordID(int peroidID, int entityID)
        {
            int _recordID=0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Journal_Batch";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "2,6";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = string.Format("{0},{1}",peroidID,entityID);
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
                        _recordID = int.Parse(reader["ID"].ToString());
                    }
                }
                else
                    _recordID = 0;
                reader.Close();
                connection.Close();
                return _recordID;
            }
        }

        public RecordCollection Query(User approveUser)
        {//TODO
            throw new NotImplementedException();
        }

        public JournalCollection QueryJournal(int recordID)
        {
            JournalCollection collection = new JournalCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Journal";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "2";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = recordID;
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
                        Journal journal = new Journal();
                        journal.SequenceNo = Convert.ToInt32(reader["ID"]);
                        journal.RecordID = Convert.ToInt32(reader["Batch_ID"]);
                        journal.EntityID = Convert.ToInt32(reader["Entity"]);

                        UserAccess usera = new UserAccess();
                        journal.EntryUser = new UserCollection(usera.Query(Convert.ToInt32(reader["Create_User"])))[0];

                        journal.BaseCurrency = reader["Base_Currency"].ToString();
                        journal.BaseAmount = Convert.ToDecimal(reader["Base_Amount"]);
                        journal.SGDAmount = Convert.ToDecimal(reader["SGD_Amount"]);
                        journal.ExchangeRate = Convert.ToDecimal(reader["Exchange_Rate"]);
                        journal.DataTime = Convert.ToDateTime(reader["LogDate"]);
                        collection.Add(journal);
                    }
                }
                reader.Close();
                return collection;
            }
        }

        public JournalCollection QueryJournal(int recordID, int sequenceNo)
        {
            JournalCollection collection = new JournalCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Journal";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "1,2";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = sequenceNo+","+recordID;
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
                        Journal journal = new Journal();
                        journal.SequenceNo = Convert.ToInt32(reader["ID"]);
                        journal.RecordID = Convert.ToInt32(reader["Batch_ID"]);
                        journal.EntityID = Convert.ToInt32(reader["Entity"]);

                        UserAccess usera = new UserAccess();                                           
                        journal.EntryUser = new UserCollection(usera.Query(Convert.ToInt32(reader["Create_User"])))[0];

                        journal.BaseCurrency = reader["Base_Currency"].ToString();
                        journal.BaseAmount = Convert.ToDecimal(reader["Base_Amount"]);
                        journal.SGDAmount = Convert.ToDecimal(reader["SGD_Amount"]);
                        journal.ExchangeRate = Convert.ToDecimal(reader["Exchange_Rate"]);
                        journal.DataTime = Convert.ToDateTime(reader["LogDate"]);
                        collection.Add(journal);
                    }
                }
                reader.Close();
                return collection;
            }
        }

        public JournalCollection QueryJournal(int recordID, string baseCurrency)
        {
            JournalCollection collection = new JournalCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Journal";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "2,4";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = recordID + "," + baseCurrency;
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
                        Journal journal = new Journal();
                        journal.SequenceNo = Convert.ToInt32(reader["ID"]);
                        journal.RecordID = Convert.ToInt32(reader["Batch_ID"]);
                        journal.EntityID = Convert.ToInt32(reader["Entity"]);
                        UserAccess usera = new UserAccess();                     
                        journal.EntryUser = new UserCollection(usera.Query(Convert.ToInt32(reader["Create_User"])))[0];
                        journal.BaseCurrency = reader["Base_Currency"].ToString();
                        journal.BaseAmount = Convert.ToDecimal(reader["Base_Amount"]);
                        journal.SGDAmount = Convert.ToDecimal(reader["SGD_Amount"]);
                        journal.ExchangeRate = Convert.ToDecimal(reader["Exchange_Rate"]);
                        journal.DataTime = Convert.ToDateTime(reader["LogDate"]);
                        collection.Add(journal);
                    }
                }
                reader.Close();
                return collection;
            }
        }

        public void Update(User user,Record record,JournalCollection jcollection)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                    SqlTransaction tr = connection.BeginTransaction();
                    try
                    {
                        using (SqlCommand _command = new SqlCommand("SP_UPD_Table", connection))
                        {
                            //SqlCommand _command = new SqlCommand();
                            //_command.Connection = connection;
                            // _command.CommandText = "SP_UPD_Table";
                            _command.Transaction = tr;
                            SqlParameter _param = _command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                            _param.Value = "Journal_Batch";
                            SqlParameter _param2 = _command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                            _param2.Value = "2,3,5";
                            StringBuilder _content = new StringBuilder();
                            _content.AppendFormat("{0}{1}", record.Period.ID, ",");
                            _content.AppendFormat("{0}{1}", (int)record.Type + ",");
                            _content.AppendFormat("{0}", (int)record.RecordStatus);
                            SqlParameter _param3 = _command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                            _param3.Value = _content.ToString();
                            SqlParameter _param4 = _command.Parameters.Add("@value4", System.Data.SqlDbType.VarChar);
                            _param4.Value = "1";
                            SqlParameter _param5 = _command.Parameters.Add("@value5", System.Data.SqlDbType.VarChar);
                            _param5.Value = record.RecordID;
                            _command.CommandType = System.Data.CommandType.StoredProcedure;
                            connection.Open();
                            _command.ExecuteNonQuery();
                        }

                        foreach (Journal journal in jcollection)
                        {
                            using (SqlCommand _jcommand = new SqlCommand("SP_UPD_Journal", connection))
                            {
                                //SqlCommand _jcommand = new SqlCommand();
                                //_jcommand.Connection = connection;
                                //_jcommand.CommandText = "SP_UPD_Journal";
                                SqlParameter _jparam1 = _jcommand.Parameters.Add("@Journal_ID", System.Data.SqlDbType.Int);
                                _jparam1.Value = journal.SequenceNo;
                                SqlParameter _jparam2 = _jcommand.Parameters.Add("@Batch_ID", System.Data.SqlDbType.Int);
                                _jparam2.Value = journal.RecordID;
                                SqlParameter _jparam3 = _jcommand.Parameters.Add("@Entity", System.Data.SqlDbType.VarChar);
                                _jparam3.Value = journal.EntityID;
                                SqlParameter _jparam4 = _jcommand.Parameters.Add("@Base_Currency", System.Data.SqlDbType.Decimal);
                                _jparam4.Value = journal.BaseCurrency;
                                SqlParameter _jparam5 = _jcommand.Parameters.Add("@Exchange_Rate", System.Data.SqlDbType.Decimal);
                                _jparam5.Value = journal.ExchangeRate;
                                SqlParameter _jparam6 = _jcommand.Parameters.Add("@Base_Amount", System.Data.SqlDbType.Decimal);
                                _jparam6.Value = journal.BaseAmount;
                                SqlParameter _jparam7 = _jcommand.Parameters.Add("@SGD_Amount", System.Data.SqlDbType.Decimal);
                                _jparam7.Value = journal.SGDAmount;
                                SqlParameter _jparam8 = _jcommand.Parameters.Add("@Modify_User", System.Data.SqlDbType.Decimal);
                                _jparam8.Value = journal.EntryUser.UserID;
                                _jcommand.CommandType = System.Data.CommandType.StoredProcedure;
                                //connection.Open();
                                _jcommand.ExecuteNonQuery();
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

        public void Update(RecordCollection recordCollection, JournalCollection journalCollection)
        {

            foreach (Record record in recordCollection)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {                    
                    connection.Open();
                    SqlTransaction tr = connection.BeginTransaction();
                    try
                    {
                        using (SqlCommand _command = new SqlCommand("SP_UPD_Table", connection))
                        {
                            _command.Transaction = tr;
                           // SqlCommand _command = new SqlCommand();
                           // _command.Connection = connection;
                           // _command.CommandText = "SP_UPD_Table";
                            SqlParameter _param = _command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                            _param.Value = "Journal_Batch";
                            SqlParameter _param2 = _command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                            _param2.Value = "2,3,5";
                            StringBuilder _content = new StringBuilder();
                            _content.AppendFormat("{0}{1}", record.Period.ID , ",");
                            _content.AppendFormat("{0}{1}", (int)record.Type , ",");
                            _content.AppendFormat("{0}", (int)record.RecordStatus);
                            SqlParameter _param3 = _command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                            _param3.Value = _content.ToString();
                            SqlParameter _param4 = _command.Parameters.Add("@value4", System.Data.SqlDbType.VarChar);
                            _param4.Value = "1";
                            SqlParameter _param5 = _command.Parameters.Add("@value5", System.Data.SqlDbType.VarChar);
                            _param5.Value = record.RecordID;
                            _command.CommandType = System.Data.CommandType.StoredProcedure;                            
                            _command.ExecuteNonQuery();

                        }
                        foreach (Journal journal in journalCollection)
                        {
                            using (SqlCommand _jcommand = new SqlCommand("SP_UPD_Journal", connection))
                            {
                                //SqlCommand _jcommand = new SqlCommand();
                               // _jcommand.Connection = connection;
                                //_jcommand.CommandText = "SP_UPD_Journal";
                                _jcommand.Transaction = tr;
                                SqlParameter _jparam1 = _jcommand.Parameters.Add("@Journal_ID", System.Data.SqlDbType.Int);
                                _jparam1.Value = journal.SequenceNo;
                                SqlParameter _jparam2 = _jcommand.Parameters.Add("@Batch_ID", System.Data.SqlDbType.Int);
                                _jparam2.Value = journal.RecordID;
                                SqlParameter _jparam3 = _jcommand.Parameters.Add("@Entity", System.Data.SqlDbType.VarChar);
                                _jparam3.Value = journal.EntityID;
                                SqlParameter _jparam4 = _jcommand.Parameters.Add("@Base_Currency", System.Data.SqlDbType.Decimal);
                                _jparam4.Value = journal.BaseCurrency;
                                SqlParameter _jparam5 = _jcommand.Parameters.Add("@Exchange_Rate", System.Data.SqlDbType.Decimal);
                                _jparam5.Value = journal.ExchangeRate;
                                SqlParameter _jparam6 = _jcommand.Parameters.Add("@Base_Amount", System.Data.SqlDbType.Decimal);
                                _jparam6.Value = journal.BaseAmount;
                                SqlParameter _jparam7 = _jcommand.Parameters.Add("@SGD_Amount", System.Data.SqlDbType.Decimal);
                                _jparam7.Value = journal.SGDAmount;
                                SqlParameter _jparam8 = _jcommand.Parameters.Add("@Modify_User", System.Data.SqlDbType.Decimal);
                                _jparam8.Value = journal.EntryUser.UserID;
                                _jcommand.CommandType = System.Data.CommandType.StoredProcedure;
                                connection.Open();
                                _jcommand.ExecuteNonQuery();
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
                
        }

        public void UpdateJournal(Journal journal)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand _jcommand = new SqlCommand();
                _jcommand.Connection = connection;
                _jcommand.CommandText = "SP_UPD_Journal";
                SqlParameter _jparam1 = _jcommand.Parameters.Add("@Journal_ID", System.Data.SqlDbType.Int);
                _jparam1.Value = journal.SequenceNo;
                SqlParameter _jparam2 = _jcommand.Parameters.Add("@Batch_ID", System.Data.SqlDbType.Int);
                _jparam2.Value = journal.RecordID;
                SqlParameter _jparam3 = _jcommand.Parameters.Add("@Entity", System.Data.SqlDbType.VarChar);
                _jparam3.Value = journal.EntityID;
                SqlParameter _jparam4 = _jcommand.Parameters.Add("@Base_Currency", System.Data.SqlDbType.Decimal);
                _jparam4.Value = journal.BaseCurrency;
                SqlParameter _jparam5 = _jcommand.Parameters.Add("@Exchange_Rate", System.Data.SqlDbType.Decimal);
                _jparam5.Value = journal.ExchangeRate;
                SqlParameter _jparam6 = _jcommand.Parameters.Add("@Base_Amount", System.Data.SqlDbType.Decimal);
                _jparam6.Value = journal.BaseAmount;
                SqlParameter _jparam7 = _jcommand.Parameters.Add("@SGD_Amount", System.Data.SqlDbType.Decimal);
                _jparam7.Value = journal.SGDAmount;
                SqlParameter _jparam8 = _jcommand.Parameters.Add("@Modify_User", System.Data.SqlDbType.Decimal);
                _jparam8.Value = journal.EntryUser.UserID;
                _jcommand.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                _jcommand.ExecuteNonQuery();
            }
        }

        public void UpdateJournal(JournalCollection journalCollection)
        {
            foreach (Journal journal in journalCollection)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand _jcommand = new SqlCommand();
                    _jcommand.Connection = connection;
                    _jcommand.CommandText = "SP_UPD_Journal";
                    SqlParameter _jparam1 = _jcommand.Parameters.Add("@Journal_ID", System.Data.SqlDbType.Int);
                    _jparam1.Value = journal.SequenceNo;
                    SqlParameter _jparam2 = _jcommand.Parameters.Add("@Batch_ID", System.Data.SqlDbType.Int);
                    _jparam2.Value = journal.RecordID;
                    SqlParameter _jparam3 = _jcommand.Parameters.Add("@Entity", System.Data.SqlDbType.VarChar);
                    _jparam3.Value = journal.EntityID;
                    SqlParameter _jparam4 = _jcommand.Parameters.Add("@Base_Currency", System.Data.SqlDbType.VarChar);
                    _jparam4.Value = journal.BaseCurrency;
                    SqlParameter _jparam5 = _jcommand.Parameters.Add("@Exchange_Rate", System.Data.SqlDbType.Decimal);
                    _jparam5.Value = journal.ExchangeRate;
                    SqlParameter _jparam6 = _jcommand.Parameters.Add("@Base_Amount", System.Data.SqlDbType.Decimal);
                    _jparam6.Value = journal.BaseAmount;
                    SqlParameter _jparam7 = _jcommand.Parameters.Add("@SGD_Amount", System.Data.SqlDbType.Decimal);
                    _jparam7.Value = journal.SGDAmount;
                    SqlParameter _jparam8 = _jcommand.Parameters.Add("@Modify_User", System.Data.SqlDbType.Decimal);
                    _jparam8.Value = journal.EntryUser.UserID;
                    _jcommand.CommandType = System.Data.CommandType.StoredProcedure;
                    connection.Open();
                    _jcommand.ExecuteNonQuery();
                }
            }
        }

        public void ChangeStatus(int recordID, RecordStatus status)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_UPD_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Journal_Batch";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "5";
                SqlParameter _param3 = command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = (int)status;
                SqlParameter _param4 = command.Parameters.Add("@value4", System.Data.SqlDbType.VarChar);
                _param4.Value = "1";
                SqlParameter _param5 = command.Parameters.Add("@value5", System.Data.SqlDbType.VarChar);
                _param5.Value = recordID;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }


        public List<decimal> GetjournalSum(int periodId, int typeid, int entityid)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                List<decimal> journal = new List<decimal>();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Journal_SUM";
                SqlParameter _param = command.Parameters.Add("@Period_ID", System.Data.SqlDbType.Int);
                _param.Value = periodId;
                SqlParameter _param2 = command.Parameters.Add("@Entity", System.Data.SqlDbType.Int);
                _param2.Value = entityid;
                SqlParameter _param3 = command.Parameters.Add("@type", System.Data.SqlDbType.Int);
                _param3.Value = typeid;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                SqlDataAdapter _dataap = new SqlDataAdapter();
                _dataap.SelectCommand = command;
                System.Data.DataTable dt = new System.Data.DataTable("JournalSum");
                _dataap.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    journal.Add(Convert.ToDecimal(dt.Rows[0]["Base_Amount"]));
                    if (typeid.Equals(1))//WinLost
                    {
                        MLJRecordAccess _mljsum = new MLJRecordAccess();
                        journal.Add(Convert.ToDecimal(dt.Rows[0]["SGD_Amount"]) + _mljsum.GetMLJSum(periodId, entityid));
                    }
                    else
                        journal.Add(Convert.ToDecimal(dt.Rows[0]["SGD_Amount"]));

                }
                return journal;
            }
        }

    }
}
