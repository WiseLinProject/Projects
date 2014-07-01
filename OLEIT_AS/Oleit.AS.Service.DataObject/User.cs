using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Oleit.AS.Service.DataObject
{
    /// <summary>
    /// User
    /// </summary>
    [DataContract]
    public class User
    {
        [DataMember]
        public int UserID { set; get; }

        [DataMember]
        public string UserName { set; get; }

        [DataMember]
        public string UserPWD { set; get; }

        [DataMember]
        public int Enable { set; get; }

        public User()
        {
            UserName = string.Empty;
            UserPWD = string.Empty;         
        
        }

        public static User DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<User>(json.Trim());
        }

        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    /// <summary>
    /// UserCollection
    /// </summary>
    public class UserCollection : List<User>
    {
        public UserCollection()
        {
        }

        public UserCollection(IEnumerable<User> collection)
            : base(collection)
        {
        }

        public UserCollection(int capacity)
            : base(capacity)
        {
        }

        public static UserCollection DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<UserCollection>(json.Trim());
        }

        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
