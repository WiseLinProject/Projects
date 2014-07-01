using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Oleit.AS.Service.DataObject
{
    [DataContract]
    public class Transaction
    {
        [DataMember]
        public int ID { set; get; }

        [DataMember]
        public string FromCurrency { set; get; }

        [DataMember]
        public string ToCurrency { set; get; }

        [DataMember]
        public Period Period { set; get; }

        [DataMember]
        public Entity FromEntity { set; get; }

        [DataMember]
        public Entity ToEntity { set; get; }

        [DataMember]
        public decimal Amount { set; get; }

        [DataMember]
        public decimal To_Amount { set; get; }

        [DataMember]
        public decimal ExchangeRate { set; get; }

        [DataMember]
        public User NoticeUser { set; get; }

        [DataMember]
        public DateTime NoticeTime { set; get; }

        [DataMember]
        public IsPay IsPay { set; get; }

        [DataMember]
        public User ConfirmUser { set; get; }

        [DataMember]
        public DateTime ConfirmTime { set; get; }

        [DataMember]
        public Period ConfirmPeriod { set; get; }

        [DataMember]
        public User Updater { set; get; }

        [DataMember]
        public DateTime UpdateTime { set; get; }

        [DataMember]
        public User Creator { set; get; }

        [DataMember]
        public DateTime CreateTime { set; get; }

        public Transaction()
        {
            IsPay = IsPay.N;
            Period = new Period();
            FromEntity = new Entity();
            ToEntity = new Entity();
            NoticeUser = new User();
            ConfirmUser = new User();
            Updater = new User();
            Creator = new User();
            ExchangeRate = 1;
        }

        public static Record DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<Record>(json.Trim());
        }

        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    /// <summary>
    /// TransactionCollection
    /// </summary>
    public class TransactionCollection : List<Transaction>
    {
        public TransactionCollection()
        {
        }

        public TransactionCollection(IEnumerable<Transaction> collection)
            : base(collection)
        {
        }

        public TransactionCollection(int capacity)
            : base(capacity)
        {
        }

        public static TransactionCollection DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<TransactionCollection>(json.Trim());
        }

        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    /// <summary>
    ///     public enum IsPay

    /// </summary>
    public enum IsPay
    {
        N = 0, //0
        Y, //1
        
    }
}
