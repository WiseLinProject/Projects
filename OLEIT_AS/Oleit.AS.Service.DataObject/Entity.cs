using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Oleit.AS.Service.DataObject
{
    /// <summary>
    /// Entity
    /// </summary>
    [DataContract]
    public class Entity
    {
        [DataMember]
        public int EntityID { set; get; }

        [DataMember]
        public int ParentID { set; get; }//required first level 0

        [DataMember]
        public int IsLastLevel { set; get; }//required 0 or 1 ; if mainEntity is 0

        [DataMember]
        public int IsAccount { set; get; }//required 0 or 1 ; if mainEntity is 0

        [DataMember]
        public int Enable { set; get; }//required 0 or 1

        [DataMember]
        public string EntityName { set; get; }//required

        [DataMember]
        public EntityType EntityType { set; get; }//required query the enum

        [DataMember]
        public Currency Currency { set; get; }

        [DataMember]
        public decimal ExchangeRate { set; get; }

        [DataMember]
        public SumType SumType { set; get; }//required query the enum

        [DataMember]
        public EntityCollection SubEntities;

        public Entity()
        {
            EntityName = string.Empty;
            EntityType = DataObject.EntityType.PAndL;
            Currency = new Currency();
            ExchangeRate = 1;
            SumType = DataObject.SumType.Not;
            SubEntities = new EntityCollection();
            Enable = 1;
        }

        public static Entity DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<Entity>(json.Trim());
        }

        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    /// <summary>
    /// EntityCollection
    /// </summary>
    public class EntityCollection : List<Entity>
    {
        public EntityCollection()
        {
        }

        public EntityCollection(IEnumerable<Entity> collection)
            : base(collection)
        {
        }

        public EntityCollection(int capacity)
            : base(capacity)
        {
        }

        public static EntityCollection DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<EntityCollection>(json.Trim());
        }

        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    public enum EntityType
    {
        PAndL = 1, //1
        Cash, //2
        Expence, //3
        BadDebt, //4
        MLJ//5
    }

    public enum SumType
    {
        Not, //0        
        Transaction, //1
        Subtotal, //2
        Super, //3
        Master, //4
        Agent, //5
    }
}
