using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Oleit.AS.Service.DataObject
{
    /// <summary>
    /// MLJRecord
    /// </summary>
    [DataContract]
    public class MLJRecord
    {
        [DataMember]
        public int MLJRecordID { set; get; }

        [DataMember]
        public Period Period { set; get; }

        [DataMember]
        public int WeekDay { set; get; }

        [DataMember]
        public MLJJournalCollection MLJJournalCollection { set; get; }

        [DataMember]
        public RecordStatus RecordStatus { set; get; }

        [DataMember]
        public User ApproveUser { set; get; }

        public MLJRecord()
        {
            Period = new Period();
            MLJJournalCollection = new MLJJournalCollection();
            RecordStatus = DataObject.RecordStatus.Normal;
            ApproveUser = new User();
        }

        public static MLJRecord DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<MLJRecord>(json.Trim());
        }

        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    /// <summary>
    /// MLJRecordCollection
    /// </summary>
    public class MLJRecordCollection : List<MLJRecord>
    {
        public MLJRecordCollection()
        {
        }

        public MLJRecordCollection(IEnumerable<MLJRecord> collection)
            : base(collection)
        {
        }

        public MLJRecordCollection(int capacity)
            : base(capacity)
        {
        }

        public static MLJRecordCollection DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<MLJRecordCollection>(json.Trim());
        }

        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
