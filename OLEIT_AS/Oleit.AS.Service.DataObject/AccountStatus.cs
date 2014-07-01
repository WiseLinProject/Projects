using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Oleit.AS.Service.DataObject
{
    /// <summary>
    /// AccountStatus
    /// </summary>
    [DataContract]
    public class AccountStatus
    {
        [DataMember]
        public int EntityID { set; get; }

        [DataMember]
        public Status Status { set; get; }

        [DataMember]
        public DateTime UpdateTime { set; get; }

        [DataMember]
        public User UpdateUser { set; get; }

        public AccountStatus()
        {
            Status = DataObject.Status.Suspended;
            UpdateUser = new User();
        }

        public static AccountStatus DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<AccountStatus>(json.Trim());
        }

        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    /// <summary>
    /// AccountStatusCollection
    /// </summary>
    public class AccountStatusCollection : List<AccountStatus>
    {
        public AccountStatusCollection()
        {
        }

        public AccountStatusCollection(IEnumerable<AccountStatus> collection)
            : base(collection)
        {
        }

        public AccountStatusCollection(int capacity)
            : base(capacity)
        {
        }

        public static AccountStatusCollection DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<AccountStatusCollection>(json.Trim());
        }

        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
