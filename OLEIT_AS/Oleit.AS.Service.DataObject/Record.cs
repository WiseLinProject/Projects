using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Oleit.AS.Service.DataObject
{
    /// <summary>
    /// Record
    /// </summary>
    [DataContract]
    public class Record
    {
        [DataMember]
        public int RecordID { set; get; }

        [DataMember]
        public int EntityID { set; get; }

        [DataMember]
        public RecordType Type { set; get; }

        [DataMember]
        public Period Period { set; get; }

        [DataMember]
        public JournalCollection JournalCollection { set; get; }

        [DataMember]
        public RecordStatus RecordStatus { set; get; }

        public Record()
        {
            Type = RecordType.WinAndLoss;
            Period = new Period();
            JournalCollection = new JournalCollection();
            RecordStatus = DataObject.RecordStatus.Normal;
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
    /// RecordCollection
    /// </summary>
    public class RecordCollection : List<Record>
    {
        public RecordCollection()
        {
        }

        public RecordCollection(IEnumerable<Record> collection)
            : base(collection)
        {
        }

        public RecordCollection(int capacity)
            : base(capacity)
        {
        }

        public static RecordCollection DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<RecordCollection>(json.Trim());
        }

        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    /// <summary>
    /// RecordType
    /// </summary>
    public enum RecordType
    {
        WinAndLoss = 1, //1
        Transfer, //2
        Transaction, //3
    }

    /// <summary>
    /// RecordStatus
    /// </summary>
    public enum RecordStatus
    {
        Normal, //0
        Confirm, //1
        Avoid, //2
    }
}
