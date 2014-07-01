using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Oleit.AS.Service.DataObject
{
    /// <summary>
    /// CashEntity
    /// </summary>
    [DataContract]
    public class CashEntity
    {
        [DataMember]
        public int EntityID { set; get; }

        [DataMember]
        public string ContractNumber { set; get; }

        [DataMember]
        public string TallyName { set; get; }

        [DataMember]
        public string TallyNumber { set; get; }

        [DataMember]
        public string SettlementName { set; get; }

        [DataMember]
        public string SettlementNumber { set; get; }

        [DataMember]
        public string RecommendedBy { set; get; }

        [DataMember]
        public string Skype { set; get; }

        [DataMember]
        public string QQ { set; get; }

        [DataMember]
        public string Email { set; get; }

        [DataMember]
        public decimal CreditLimit { set; get; }

        public CashEntity()
        {
            ContractNumber = string.Empty;
            TallyName = string.Empty;
            TallyNumber = string.Empty;
            SettlementName = string.Empty;
            SettlementNumber = string.Empty;
            RecommendedBy = string.Empty;
            Skype = string.Empty;
            QQ = string.Empty;
            Email = string.Empty;
        }

        public static CashEntity DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<CashEntity>(json.Trim());
        }

        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    /// <summary>
    /// CashEntityCollection
    /// </summary>
    public class CashEntityCollection : List<CashEntity>
    {
        public CashEntityCollection()
        {
        }

        public CashEntityCollection(IEnumerable<CashEntity> collection)
            : base(collection)
        {
        }

        public CashEntityCollection(int capacity)
            : base(capacity)
        {
        }

        public static CashEntityCollection DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<CashEntityCollection>(json.Trim());
        }

        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
