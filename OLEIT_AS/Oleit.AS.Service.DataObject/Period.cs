using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Oleit.AS.Service.DataObject
{
    /// <summary>
    /// Period
    /// </summary>
    [DataContract]
    public class Period
    {
        [DataMember]
        public string PeriodNo { set; get; }

        [DataMember]
        public int ID { set; get; }

        [DataMember]
        public DateTime StartDate { set; get; }

        [DataMember]
        public DateTime EndDate { set; get; }

        public Period()
        {
            PeriodNo = string.Empty;
        }

        public static Period DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<Period>(json.Trim());
        }

        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    /// <summary>
    /// PeriodCollection
    /// </summary>
    public class PeriodCollection : List<Period>
    {
        public PeriodCollection()
            : base()
        {
        }

        public PeriodCollection(IEnumerable<Period> collection)
            : base(collection)
        {
        }

        public PeriodCollection(int capacity)
            : base(capacity)
        {
        }

        public static PeriodCollection DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<PeriodCollection>(json.Trim());
        }

        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
