using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Oleit.AS.Service.DataObject
{
    /// <summary>
    /// TransferDetail
    /// </summary>
    [DataContract]
    public class TransferDetail
    {
        [DataMember]
        public int ID { set; get; }

        [DataMember]
        public int RecordID { set; get; }

        [DataMember]
        public Entity Entity { set; get; }

        [DataMember]
        public string BaseCurrency { set; get; }

        [DataMember]
        public decimal ExchangeRate { set; get; }

        [DataMember]
        public decimal BaseBefore { set; get; }

        [DataMember]
        public decimal SGDBefore { set; get; }

        [DataMember]
        public decimal BaseTransfer { set; get; }

        [DataMember]
        public decimal SGDTransfer { set; get; }

        [DataMember]
        public decimal ProfitAndLoss { set; get; }

        [DataMember]
        public decimal BaseResult { set; get; }

        [DataMember]
        public decimal SGDResult { set; get; }

        public TransferDetail()
        {
            Entity = new Entity();
            BaseCurrency = string.Empty;
            ExchangeRate = 1;
        }

        public static TransferDetail DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<TransferDetail>(json.Trim());
        }

        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    /// <summary>
    /// TransferDetailCollection
    /// </summary>
    public class TransferDetailCollection : List<TransferDetail>
    {
        public TransferDetailCollection()
        {
        }

        public TransferDetailCollection(IEnumerable<TransferDetail> collection)
            : base(collection)
        {
        }

        public TransferDetailCollection(int capacity)
            : base(capacity)
        {
        }

        public static TransferDetailCollection DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<TransferDetailCollection>(json.Trim());
        }

        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
