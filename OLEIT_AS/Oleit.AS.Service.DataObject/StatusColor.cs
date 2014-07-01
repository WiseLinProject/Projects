using Newtonsoft.Json;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Oleit.AS.Service.DataObject
{
    [DataContract]
    public class StatusColor
    {
        [DataMember]
        public int MLJStatus { set; get; }

        [DataMember]
        public string MLJColor { set; get; }

        [DataMember]
        public Status StatusType { set; get; }
    }
    public class StatusColorCollection : List<StatusColor>
    {
        public StatusColorCollection()
        {

        }
        public StatusColorCollection(IEnumerable<StatusColor> collection)
            : base(collection)
        {
        }
        public StatusColorCollection(int capacity)
            : base(capacity)
        {
        }

        public static StatusColorCollection DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<StatusColorCollection>(json.Trim());
        }

        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

    }
}
