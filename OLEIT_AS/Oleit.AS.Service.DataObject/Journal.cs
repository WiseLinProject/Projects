using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Oleit.AS.Service.DataObject
{
    /// <summary>
    /// Journal
    /// </summary>
    [DataContract]
    public class Journal
    {
        [DataMember]
        public int SequenceNo { set; get; }

        [DataMember]
        public DateTime DataTime { set; get; }

        [DataMember]
        public int RecordID { set; get; }

        [DataMember]
        public int EntityID { set; get; }

        [DataMember]
        public string BaseCurrency { set; get; }

        [DataMember]
        public decimal ExchangeRate { set; get; }

        [DataMember]
        public decimal BaseAmount { set; get; }

        [DataMember]
        public decimal SGDAmount { set; get; }

        [DataMember]
        public User EntryUser { set; get; }

        public Journal()
        {
            DataTime = DateTime.Now;
            BaseCurrency = string.Empty;
            ExchangeRate = 1;
            EntryUser = new User();
        }

        public static Journal DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<Journal>(json.Trim());
        }

        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    /// <summary>
    /// JournalCollection
    /// </summary>
    public class JournalCollection : List<Journal>
    {
        public JournalCollection()
        {
        }

        public JournalCollection(IEnumerable<Journal> collection)
            : base(collection)
        {
        }

        public JournalCollection(int capacity)
            : base(capacity)
        {
        }

        public static JournalCollection DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<JournalCollection>(json.Trim());
        }

        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
