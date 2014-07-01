using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Oleit.AS.Service.DataObject
{
    /// <summary>
    /// MLJJournal
    /// </summary>
    [DataContract]
    public class MLJJournal
    {
        [DataMember]
        public int SequenceNo { set; get; }

        [DataMember]
        public int MLJRecordID { set; get; }

        [DataMember]
        public int EntityID { set; get; }

        [DataMember]
        public string EntityName { set; get; }

        [DataMember]
        public string BaseCurrency { set; get; }

        [DataMember]
        public Account Account { set; get; }

        [DataMember]
        public decimal ExchangeRate { set; get; }

        [DataMember]
        public int UserID { set; get; }


        [DataMember]
        public int Mon { set; get; }

        [DataMember]
        public int Tue { set; get; }

        [DataMember]
        public int Wed { set; get; }

        [DataMember]
        public int Thu { set; get; }

        [DataMember]
        public int Fri { set; get; }

        [DataMember]
        public int Sat { set; get; }

        [DataMember]
        public int Sun { set; get; }

              
        [DataMember]
        public User EntryUser { set; get; }

        [DataMember]
        public User Personnel { set; get; }

        public MLJJournal()
        {
            BaseCurrency = string.Empty;
            ExchangeRate = 1;
            EntryUser = new User();
            Personnel = new User();
            EntityID = 0;
            Mon = 0;
            Tue = 0;
            Wed = 0;
            Thu = 0;
            Fri = 0;
            Sat = 0;
            Sun = 0;
        }

        public static MLJJournal DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<MLJJournal>(json.Trim());
        }

        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    /// <summary>
    /// MLJJournalCollection
    /// </summary>
    public class MLJJournalCollection : List<MLJJournal>
    {      

        public MLJJournalCollection()
        {
        }

        public MLJJournalCollection(IEnumerable<MLJJournal> collection)
            : base(collection)
        {
        }

        public MLJJournalCollection(int capacity)
            : base(capacity)
        {
        }

        public static MLJJournalCollection DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<MLJJournalCollection>(json.Trim());
        }

        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
