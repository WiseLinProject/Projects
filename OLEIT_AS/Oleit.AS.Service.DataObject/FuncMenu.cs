using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Oleit.AS.Service.DataObject
{
    /// <summary>
    /// Menu
    /// </summary>
    [DataContract]
    public class FuncMenu
    {
        [DataMember]
        public int ItemID { set; get; }

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
        //public MenuCollection Submenu { set; get; }

        public FuncMenu()
        {
            Text = string.Empty;
            //Roles = new RoleCollection();
            Path = string.Empty;
            //Submenu = new MenuCollection();
        }

        public static FuncMenu DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<FuncMenu>(json.Trim());
        }

        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    /// <summary>
    /// FuncMenuCollection
    /// </summary>
    public class FuncMenuCollection : List<FuncMenu>
    {
        public FuncMenuCollection()
        {
        }

        public FuncMenuCollection(IEnumerable<FuncMenu> collection)
            : base(collection)
        {
        }

        public FuncMenuCollection(int capacity)
            : base(capacity)
        {
        }

        public static FuncMenuCollection DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<FuncMenuCollection>(json.Trim());
        }

        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
