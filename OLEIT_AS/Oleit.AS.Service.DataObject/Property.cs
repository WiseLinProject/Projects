using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Oleit.AS.Service.DataObject
{
    /// <summary>
    /// Property
    /// </summary>
    [DataContract]
    public class Property
    {
        [DataMember]
        public int PropertyID { set; get; }

        [DataMember]
        public string PropertyName { set; get; }

        [DataMember]
        public string PropertyValue { set; get; }

        public Property()
        {
            PropertyName = string.Empty;
            PropertyValue = string.Empty;
        }

        public static Property DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<Property>(json.Trim());
        }

        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    /// <summary>
    /// PropertyCollection
    /// </summary>
    public class PropertyCollection : List<Property>
    {
        public PropertyCollection()
            : base()
        {
        }

        public PropertyCollection(IEnumerable<Property> collection)
            : base(collection)
        {
        }

        public PropertyCollection(int capacity)
            : base(capacity)
        {
        }

        public static PropertyCollection DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<PropertyCollection>(json.Trim());
        }

        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
