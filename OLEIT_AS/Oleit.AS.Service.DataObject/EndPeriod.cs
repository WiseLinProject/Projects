using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;

namespace Oleit.AS.Service.DataObject
{
    [DataContract]
    public class EndPeriod
    {
        [DataMember]
        public int Period_ID { set; get; }

        [DataMember]
        public Currency Currency { set; get; }

        [DataMember]
        public decimal ExchangeRate { set; get; }

        public EndPeriod()
        {
            Currency = new Currency();
        }


        public static EndPeriod DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<EndPeriod>(json.Trim());
        }

        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    /// <summary>
    /// EndPeriodCollection
    /// </summary>
    public class EndPeriodCollection : List<EndPeriod>
    {
        public EndPeriodCollection()
            : base()
        {
        }

        public EndPeriodCollection(IEnumerable<EndPeriod> collection)
            : base(collection)
        {
        }

        public EndPeriodCollection(int capacity)
            : base(capacity)
        {
        }

        public static EndPeriodCollection DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<EndPeriodCollection>(json.Trim());
        }

        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
