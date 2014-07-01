using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Oleit.AS.Service.DataObject
{
    /// <summary>
    /// Role
    /// </summary>
    [DataContract]
    public class Role
    {
        [DataMember]
        public string RoleName { set; get; }
        [DataMember]
        public int ID { set; get; }

        public Role()
        {
            RoleName = string.Empty;
        }

        public static Role DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<Role>(json.Trim());
        }

        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    /// <summary>
    /// RoleCollection
    /// </summary>
    public class RoleCollection : List<Role>
    {
        public RoleCollection()
        {
        }

        public RoleCollection(IEnumerable<Role> collection)
            : base(collection)
        {
        }

        public RoleCollection(int capacity)
            : base(capacity)
        {
        }

        public static RoleCollection DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<RoleCollection>(json.Trim());
        }

        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
