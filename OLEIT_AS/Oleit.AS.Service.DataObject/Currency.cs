using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Oleit.AS.Service.DataObject
{
    /// <summary>
    /// Currency
    /// </summary>
    [DataContract]
    public class Currency
    {
        [DataMember]
        public string CurrencyID { set; get; }

        public Currency()
        {
            CurrencyID = string.Empty;
        }

        public static Currency DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<Currency>(json.Trim());
        }

        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    /// <summary>
    /// CurrencyCollection
    /// </summary>
    public class CurrencyCollection : List<Currency>
    {
        public CurrencyCollection()
        {
        }

        public CurrencyCollection(IEnumerable<Currency> collection)
            : base(collection)
        {
        }

        public CurrencyCollection(int capacity)
            : base(capacity)
        {
        }

        public static CurrencyCollection DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<CurrencyCollection>(json.Trim());
        }

        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
