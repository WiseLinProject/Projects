﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18047
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Accounting_System.MLJServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="MLJServiceReference.IMLJService")]
    public interface IMLJService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMLJService/CheckAndAdd", ReplyAction="http://tempuri.org/IMLJService/CheckAndAddResponse")]
        void CheckAndAdd(int PeriodID, int userID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMLJService/Query", ReplyAction="http://tempuri.org/IMLJService/QueryResponse")]
        Oleit.AS.Service.DataObject.MLJJournal[] Query(int PeriodID, string EntityName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMLJService/UpdateJournal", ReplyAction="http://tempuri.org/IMLJService/UpdateJournalResponse")]
        void UpdateJournal(Oleit.AS.Service.DataObject.MLJJournal journal);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMLJService/Approve", ReplyAction="http://tempuri.org/IMLJService/ApproveResponse")]
        void Approve(int MLJRecordID, int userID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMLJService/QueryRecordByID", ReplyAction="http://tempuri.org/IMLJService/QueryRecordByIDResponse")]
        Oleit.AS.Service.DataObject.MLJRecord QueryRecordByID(int RecordID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMLJService/QueryStatusColor", ReplyAction="http://tempuri.org/IMLJService/QueryStatusColorResponse")]
        Oleit.AS.Service.DataObject.StatusColor[] QueryStatusColor();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMLJService/UpdateColor", ReplyAction="http://tempuri.org/IMLJService/UpdateColorResponse")]
        void UpdateColor(Oleit.AS.Service.DataObject.StatusColor[] collection);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMLJService/UpdateUserMLJ", ReplyAction="http://tempuri.org/IMLJService/UpdateUserMLJResponse")]
        void UpdateUserMLJ(int userid, Oleit.AS.Service.DataObject.Entity[] collection);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMLJService/GetMLJSum", ReplyAction="http://tempuri.org/IMLJService/GetMLJSumResponse")]
        decimal GetMLJSum(int periodId, int entityid);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IMLJServiceChannel : Accounting_System.MLJServiceReference.IMLJService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class MLJServiceClient : System.ServiceModel.ClientBase<Accounting_System.MLJServiceReference.IMLJService>, Accounting_System.MLJServiceReference.IMLJService {
        
        public MLJServiceClient() {
        }
        
        public MLJServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public MLJServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MLJServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MLJServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void CheckAndAdd(int PeriodID, int userID) {
            base.Channel.CheckAndAdd(PeriodID, userID);
        }
        
        public Oleit.AS.Service.DataObject.MLJJournal[] Query(int PeriodID, string EntityName) {
            return base.Channel.Query(PeriodID, EntityName);
        }
        
        public void UpdateJournal(Oleit.AS.Service.DataObject.MLJJournal journal) {
            base.Channel.UpdateJournal(journal);
        }
        
        public void Approve(int MLJRecordID, int userID) {
            base.Channel.Approve(MLJRecordID, userID);
        }
        
        public Oleit.AS.Service.DataObject.MLJRecord QueryRecordByID(int RecordID) {
            return base.Channel.QueryRecordByID(RecordID);
        }
        
        public Oleit.AS.Service.DataObject.StatusColor[] QueryStatusColor() {
            return base.Channel.QueryStatusColor();
        }
        
        public void UpdateColor(Oleit.AS.Service.DataObject.StatusColor[] collection) {
            base.Channel.UpdateColor(collection);
        }
        
        public void UpdateUserMLJ(int userid, Oleit.AS.Service.DataObject.Entity[] collection) {
            base.Channel.UpdateUserMLJ(userid, collection);
        }
        
        public decimal GetMLJSum(int periodId, int entityid) {
            return base.Channel.GetMLJSum(periodId, entityid);
        }
    }
}