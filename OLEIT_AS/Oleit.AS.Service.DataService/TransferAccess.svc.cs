using Oleit.AS.Service.DataObject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Oleit.AS.Service.DataService.WCFService 
{
    
    public class TransferAccess : ITransferAccess
    {
        string connectionString = ConfigurationManager.ConnectionStrings["AccountDataBase"].ConnectionString;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record">please with JournalCollection</param>
        /// <param name="transfer">please with TransferDetailCollection</param>
        public int Insert(Record record, Transfer transfer )
        {
            int _batchid = -1;
            using (SqlConnection connection_record = new SqlConnection(connectionString))
            {
               
                connection_record.Open();
                SqlTransaction _tr = connection_record.BeginTransaction();
                try
                {
                    using (SqlCommand _command = new SqlCommand("SP_INS_Table", connection_record))
                    {
                       // SqlCommand _command = new SqlCommand();
                       // _command.Connection = connection_record;
                       // _command.CommandText = "SP_INS_Table";
                        _command.Transaction = _tr;
                        SqlParameter _param = _command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                        _param.Value = "Journal_Batch";
                        SqlParameter _param2 = _command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                        _param2.Value = "2,3,5";
                        StringBuilder _content = new StringBuilder();
                        _content.AppendFormat("{0}{1}",record.Period.ID , ",");
                        _content.AppendFormat("{0}{1}", (int)record.Type, ",");
                        _content.AppendFormat("{0}", (int)record.RecordStatus);
                        SqlParameter _param3 = _command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                        _param3.Value = _content.ToString();
                        _command.CommandType = System.Data.CommandType.StoredProcedure;
                        //connection_record.Open();
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
                        foreach (Journal journal in record.JournalCollection)
                        {
                            using (SqlCommand _jcommand = new SqlCommand("SP_INS_Table", connection_record))
                            {
                                //SqlCommand _jcommand = new SqlCommand();
                               // _jcommand.Connection = connection2;
                               // _jcommand.CommandText = "SP_INS_Table";
                                _jcommand.Transaction = _tr;
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
                                _jcommand.ExecuteNonQuery();
                            }
                        }

                        using (SqlCommand _tcommand = new SqlCommand("SP_INS_Table", connection_record))
                        {
                            //SqlCommand _tcommand = new SqlCommand();
                            //_tcommand.Connection = _tranconnection;
                           // _tcommand.CommandText = "SP_INS_Table";
                            _tcommand.Transaction = _tr;
                            SqlParameter _tparam = _tcommand.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                            _tparam.Value = "Transfer";
                            SqlParameter _tparam2 = _tcommand.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                            _tparam2.Value = "1,2,3,4,5,6,7,8";
                            StringBuilder _tcontent = new StringBuilder();
                            _tcontent.AppendFormat("{0}{1}", _batchid, ",");
                            _tcontent.AppendFormat("{0}{1}", transfer.ToEntity.EntityID, ",");
                            _tcontent.AppendFormat("{0}{1}", transfer.Currency.CurrencyID, ",");
                            _tcontent.AppendFormat("{0}{1}", transfer.ExchangeRate, ",");
                            _tcontent.AppendFormat("{0}{1}", transfer.BaseBefore, ",");
                            _tcontent.AppendFormat("{0}{1}", transfer.SGDBefore, ",");
                            _tcontent.AppendFormat("{0}{1}", transfer.BaseResult, ",");
                            _tcontent.AppendFormat("{0}", transfer.SGDResult);
                            SqlParameter _tparam3 = _tcommand.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                            _tparam3.Value = _tcontent.ToString();
                            _tcommand.CommandType = System.Data.CommandType.StoredProcedure;
                            //_tranconnection.Open();
                            _tcommand.ExecuteNonQuery();
                            foreach (TransferDetail _tran in transfer.TransferDetailCollection)
                            {
                                using (SqlCommand _tdcommand = new SqlCommand("SP_INS_Table", connection_record))
                                {
                                    //SqlCommand _tdcommand = new SqlCommand();
                                    //_tdcommand.Connection = _tranconnect;
                                    //_tdcommand.CommandText = "SP_INS_Table";
                                    _tdcommand.Transaction = _tr;
                                    SqlParameter _jparam1 = _tdcommand.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                                    _jparam1.Value = "Transfer_Detail";
                                    SqlParameter _jparam2 = _tdcommand.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                                    _jparam2.Value = "2,3,4,5,6,7,8,9,10,11,12";
                                    StringBuilder _tdcontent = new StringBuilder();
                                    _tdcontent.AppendFormat("{0}{1}", _batchid , ",");
                                    _tdcontent.AppendFormat("{0}{1}", _tran.Entity.EntityID, ",");
                                    _tdcontent.AppendFormat("{0}{1}", _tran.BaseCurrency, ",");
                                    _tdcontent.AppendFormat("{0}{1}", _tran.ExchangeRate, ",");
                                    _tdcontent.AppendFormat("{0}{1}", _tran.BaseBefore, ",");
                                    _tdcontent.AppendFormat("{0}{1}", _tran.SGDBefore, ",");
                                    _tdcontent.AppendFormat("{0}{1}", _tran.BaseTransfer, ",");
                                    _tdcontent.AppendFormat("{0}{1}", _tran.SGDTransfer, ",");
                                    _tdcontent.AppendFormat("{0}{1}", _tran.ProfitAndLoss, ",");
                                    _tdcontent.AppendFormat("{0}{1}", _tran.BaseResult, ",");
                                    _tdcontent.AppendFormat("{0}", _tran.SGDResult);
                                    SqlParameter _jparam3 = _tdcommand.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                                    _jparam3.Value = _tdcontent.ToString();
                                    _tdcommand.CommandType = System.Data.CommandType.StoredProcedure;
                                    //_tranconnect.Open();
                                    _tdcommand.ExecuteNonQuery();
                                }
                            }//foreach detail
                        }
                        _tr.Commit();
                      
                    }
                     
                }
                catch(Exception)
                {
                    _tr.Rollback();
                }
            }
            return _batchid;
           
        }

        public void Insert(TransferDetail transferdetail)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand _jcommand = new SqlCommand();
                _jcommand.Connection = connection;
                _jcommand.CommandText = "SP_INS_Table";
                SqlParameter _jparam1 = _jcommand.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _jparam1.Value = "Transfer_Detail";
                SqlParameter _jparam2 = _jcommand.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _jparam2.Value = "2,3,4,5,6,7,8,9,10,11,12";
                StringBuilder _tcontent = new StringBuilder();
                _tcontent.AppendFormat("{0}{1}", transferdetail.RecordID, ",");
                _tcontent.AppendFormat("{0}{1}", transferdetail.Entity.EntityID, ",");
                _tcontent.AppendFormat("{0}{1}", transferdetail.BaseCurrency, ",");
                _tcontent.AppendFormat("{0}{1}", transferdetail.ExchangeRate, ",");
                _tcontent.AppendFormat("{0}{1}", transferdetail.BaseBefore, ",");
                _tcontent.AppendFormat("{0}{1}", transferdetail.SGDBefore, ",");
                _tcontent.AppendFormat("{0}{1}", transferdetail.BaseTransfer, ",");
                _tcontent.AppendFormat("{0}{1}", transferdetail.SGDTransfer, ",");
                _tcontent.AppendFormat("{0}{1}", transferdetail.ProfitAndLoss, ",");
                _tcontent.AppendFormat("{0}{1}", transferdetail.BaseResult, ",");
                _tcontent.AppendFormat("{0}", transferdetail.SGDResult);
                SqlParameter _jparam3 = _jcommand.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _jparam3.Value = _tcontent.ToString();
                _jcommand.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                _jcommand.ExecuteNonQuery();
            }
        }
        
        public void Update(Transfer transfer)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand _command = new SqlCommand();
                _command.Connection = connection;
                _command.CommandText = "SP_UPD_Table";
                SqlParameter _param = _command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Transfer";
                SqlParameter _param2 = _command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "2,3,4,5,6,7,8";
                StringBuilder _content = new StringBuilder();
                _content.AppendFormat("{0}{1}", transfer.ToEntity.EntityID + ",");
                _content.AppendFormat("{0}{1}", transfer.Currency.CurrencyID + ",");
                _content.AppendFormat("{0}{1}", transfer.ExchangeRate , ",");
                _content.AppendFormat("{0}{1}", transfer.BaseBefore , ",");
                _content.AppendFormat("{0}{1}", transfer.SGDBefore , ",");
                _content.AppendFormat("{0}{1}", transfer.BaseResult , ",");
                _content.AppendFormat("{0}", transfer.SGDResult);
                SqlParameter _param3 = _command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = _content.ToString();
                SqlParameter _param4 = _command.Parameters.Add("@value4", System.Data.SqlDbType.VarChar);
                _param4.Value = "1";
                SqlParameter _param5 = _command.Parameters.Add("@value5", System.Data.SqlDbType.VarChar);
                _param5.Value = transfer.RecordID;
                _command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                _command.ExecuteNonQuery();
            }
        }

        public void Update(TransferDetail transferdetail)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand _command = new SqlCommand();
                _command.Connection = connection;
                _command.CommandText = "SP_UPD_Table";
                SqlParameter _param = _command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Transfer_Detail";
                SqlParameter _param2 = _command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "3,4,5,6,7,8,9,10,11,12";
                StringBuilder _content = new StringBuilder();
                _content.AppendFormat("{0}{1}", transferdetail.Entity.EntityID, ",");
                _content.AppendFormat("{0}{1}", transferdetail.BaseCurrency, ",");
                _content.AppendFormat("{0}{1}", transferdetail.ExchangeRate, ",");
                _content.AppendFormat("{0}{1}", transferdetail.BaseBefore, ",");
                _content.AppendFormat("{0}{1}", transferdetail.SGDBefore, ",");
                _content.AppendFormat("{0}{1}", transferdetail.BaseTransfer, ",");
                _content.AppendFormat("{0}{1}", transferdetail.SGDTransfer, ",");
                _content.AppendFormat("{0}{1}", transferdetail.ProfitAndLoss, ",");
                _content.AppendFormat("{0}{1}", transferdetail.BaseResult, ",");
                _content.AppendFormat("{0}", transferdetail.SGDResult);
                SqlParameter _param3 = _command.Parameters.Add("@value3", System.Data.SqlDbType.VarChar);
                _param3.Value = _content.ToString();
                SqlParameter _param4 = _command.Parameters.Add("@value4", System.Data.SqlDbType.VarChar);
                _param4.Value = "1";
                SqlParameter _param5 = _command.Parameters.Add("@value5", System.Data.SqlDbType.VarChar);
                _param5.Value = transferdetail.ID;
                _command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                _command.ExecuteNonQuery();
            }
        }

        public void Update(TransferDetailCollection transferdetailcollection)
        {
            foreach (TransferDetail transferdetail in transferdetailcollection)
            {
                Update(transferdetail);
            }
        }

        public TransferCollection Query(int recordID)
        {
            TransferCollection collection = new TransferCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Transfer";
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
                        Transfer transfer = new Transfer();
                        transfer.RecordID = Convert.ToInt32(reader["Record_ID"]);
                        
                        EntityAccess entitya = new EntityAccess();                       
                        transfer.ToEntity = new EntityCollection(entitya.Query(Convert.ToInt32(reader["To_Entity"])))[0];

                        transfer.Currency.CurrencyID = reader["Currency"].ToString();
                        transfer.ExchangeRate = Convert.ToDecimal(reader["Exchange_Rate"]);
                        transfer.BaseBefore = Convert.ToDecimal(reader["Base_Before"]);
                        transfer.SGDBefore = Convert.ToDecimal(reader["SGD_Before"]);
                        transfer.BaseResult = Convert.ToDecimal(reader["Base_Result"]);
                        transfer.SGDResult = Convert.ToDecimal(reader["SGD_Result"]);
                        collection.Add(transfer);
                    }
                }
                reader.Close();
                return collection; 
            }
        }

        public TransferDetailCollection QueryDetailCollection(int recordID)
        {
            TransferDetailCollection collection = new TransferDetailCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Transfer_Detail";
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
                        TransferDetail transfer = new TransferDetail();
                        transfer.ID = Convert.ToInt32(reader["ID"]);
                        transfer.RecordID = Convert.ToInt32(reader["Record_ID"]);

                        EntityAccess entitya = new EntityAccess();                       
                        Entity entity = new EntityCollection(entitya.Query(Convert.ToInt32(reader["To_Entity"])))[0];
                        transfer.Entity = entity;

                        transfer.BaseCurrency = reader["Base_Currency"].ToString();
                        transfer.ExchangeRate = Convert.ToDecimal(reader["Exchange_Rate"]);
                        transfer.BaseBefore = Convert.ToDecimal(reader["Base_Before"]);
                        transfer.SGDBefore = Convert.ToDecimal(reader["SGD_Before"]);
                        transfer.BaseTransfer = Convert.ToDecimal(reader["Base_Transfer"]);
                        transfer.SGDTransfer = Convert.ToDecimal(reader["SGD_Transfer"]);
                        transfer.ProfitAndLoss = Convert.ToDecimal(reader["Profit_or_Loss"]);
                        transfer.BaseResult = Convert.ToDecimal(reader["Base_Result"]);
                        transfer.SGDResult = Convert.ToDecimal(reader["SGD_Result"]);

                        collection.Add(transfer);
                    }

                }
                reader.Close();
                return collection;
            }
        }

        public TransferCollection QueryByToEntityID(int entityID)
        {
            TransferCollection collection = new TransferCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Transfer";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "1";
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
                        Transfer transfer = new Transfer();
                        transfer.RecordID = Convert.ToInt32(reader["Record_ID"]);

                        EntityAccess entitya = new EntityAccess();                       
                        transfer.ToEntity = new EntityCollection(entitya.Query(Convert.ToInt32(reader["To_Entity"])))[0];

                        transfer.Currency.CurrencyID = reader["Currency"].ToString();
                        transfer.ExchangeRate = Convert.ToDecimal(reader["Exchange_Rate"]);
                        transfer.BaseBefore = Convert.ToDecimal(reader["Base_Before"]);
                        transfer.SGDBefore = Convert.ToDecimal(reader["SGD_Before"]);
                        transfer.BaseResult = Convert.ToDecimal(reader["Base_Result"]);
                        transfer.SGDResult = Convert.ToDecimal(reader["SGD_Result"]);
                        collection.Add(transfer);
                    }
                }
                reader.Close();
                return collection;
            }
        }

        public TransferDetailCollection QueryDetailCollectionToEntityID(int entityID)
        {
            //TODO
            throw new NotImplementedException();
        }

        public TransferDetailCollection QueryByEntityID(int entityID)
        {
            TransferDetailCollection collection = new TransferDetailCollection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_SEL_Table";
                SqlParameter _param = command.Parameters.Add("@value1", System.Data.SqlDbType.VarChar);
                _param.Value = "Transfer_Detail";
                SqlParameter _param2 = command.Parameters.Add("@value2", System.Data.SqlDbType.VarChar);
                _param2.Value = "3";
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
                        TransferDetail transfer = new TransferDetail();
                        transfer.ID = Convert.ToInt32(reader["ID"]);
                        transfer.RecordID = Convert.ToInt32(reader["Record_ID"]);

                        EntityAccess entitya = new EntityAccess();                       
                        Entity entity = new EntityCollection(entitya.Query(Convert.ToInt32(reader["To_Entity"])))[0];
                        transfer.Entity = entity;

                        transfer.BaseCurrency = reader["Base_Currency"].ToString();
                        transfer.ExchangeRate = Convert.ToDecimal(reader["Exchange_Rate"]);
                        transfer.BaseBefore = Convert.ToDecimal(reader["Base_Before"]);
                        transfer.SGDBefore = Convert.ToDecimal(reader["SGD_Before"]);
                        transfer.BaseTransfer = Convert.ToDecimal(reader["Base_Transfer"]);
                        transfer.SGDTransfer = Convert.ToDecimal(reader["SGD_Transfer"]);
                        transfer.ProfitAndLoss = Convert.ToDecimal(reader["Profit_or_Loss"]);
                        transfer.BaseResult = Convert.ToDecimal(reader["Base_Result"]);
                        transfer.SGDResult = Convert.ToDecimal(reader["SGD_Result"]);

                        collection.Add(transfer);
                    }
                }
                reader.Close();
                return collection;
            }
        }

        



    }
}
