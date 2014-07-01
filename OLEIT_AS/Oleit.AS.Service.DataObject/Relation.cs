using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Oleit.AS.Service.DataObject
{
    /// <summary>
    /// Relation
    /// </summary>
    [DataContract]
    public class Relation
    {
        [DataMember]
        public Entity TargetEntity { set; get; }

        [DataMember]
        public RelationDescription Description { set; get; }

        [DataMember]
        public decimal Numeric { set; get; }

        [DataMember]
        public Entity Entity { set; get; }
     
        public Relation()
        {
            TargetEntity = new Entity();
            Entity = new Entity();
            Description = RelationDescription.Allocate;
        }

        public static Relation DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<Relation>(json.Trim());
        }

        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    /// <summary>
    /// RelationCollection
    /// </summary>
    public class RelationCollection : List<Relation>
    {
        public RelationCollection()
        {
        }

        public RelationCollection(IEnumerable<Relation> collection)
            : base(collection)
        {
        }

        public RelationCollection(int capacity)
            : base(capacity)
        {
        }

        public static RelationCollection DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<RelationCollection>(json.Trim());
        }

        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    public enum RelationDescription
    {
        Allocate = 1, //1
        Position, //2
        Commission, //3
        FollowBet, //4
        PAndLSum//5
    }
}
