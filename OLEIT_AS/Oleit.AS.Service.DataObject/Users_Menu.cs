using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Oleit.AS.Service.DataObject
{
    [DataContract]
    public class Users_Menu
    {
        [DataMember]
        public int ID { set; get; }

        [DataMember]
        public int ParentID { set; get; }

        [DataMember]
        public int Sort { set; get; }

        //[DataMember]
        //public int Level { set; get; }

        [DataMember]
        public string Text { set; get; }

        //[DataMember]
        //public RoleCollection Roles { set; get; }

        [DataMember]
        public string Path { set; get; }

        //[DataMember]
        //public Users_MenuCollection Submenu { set; get; }

        public Users_Menu()
        {
            Text = string.Empty;
            //Roles = new RoleCollection();
            Path = string.Empty;
            //Submenu = new Users_MenuCollection();
        }

        public static Users_Menu DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<Users_Menu>(json.Trim());
        }

        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    /// <summary>
    /// Users_MenuCollection
    /// </summary>
    public class Users_MenuCollection : List<Users_Menu>
    {
        public Users_MenuCollection()
        {
        }

        public Users_MenuCollection(IEnumerable<Users_Menu> collection)
            : base(collection)
        {
        }

        public Users_MenuCollection(int capacity)
            : base(capacity)
        {
        }

        public static Users_MenuCollection DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<Users_MenuCollection>(json.Trim());
        }

        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
