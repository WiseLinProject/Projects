using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Oleit.AS.Service.DataObject
{
    /// <summary>
    /// Transfer
    /// </summary>
    [DataContract]
    public class Transfer
    {
        [DataMember]
        public int RecordID { set; get; }

        [DataMember]
        public Record RecordNotInDB { set; get; }

        [DataMember]
        public Entity ToEntity { set; get; }

        [DataMember]
        public Currency Currency { set; get; }

        [DataMember]
        public decimal ExchangeRate { set; get; }

        [DataMember]
        public TransferDetailCollection TransferDetailCollection { set; get; }

        [DataMember]
        public decimal BaseBefore { set; get; }

        [DataMember]
        public decimal SGDBefore { set; get; }

        [DataMember]
        public decimal BaseResult { set; get; }

        [DataMember]
        public decimal SGDResult { set; get; }

        public Transfer()
        {
            RecordNotInDB = null;

            ToEntity = new Entity();
            Currency = new Currency();
            ExchangeRate = 1;
            TransferDetailCollection = new TransferDetailCollection();
        }

        public static Transfer DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<Transfer>(json.Trim());
        }

        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    /// <summary>
    /// TransferCollection
    /// </summary>
    public class TransferCollection : List<Transfer>
    {
        public TransferCollection()
        {
        }

        public TransferCollection(IEnumerable<Transfer> collection)
            : base(collection)
        {
        }

        public TransferCollection(int capacity)
            : base(capacity)
        {
        }

        public static TransferCollection DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<TransferCollection>(json.Trim());
        }

        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
