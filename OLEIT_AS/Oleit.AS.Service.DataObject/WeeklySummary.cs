using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Oleit.AS.Service.DataObject
{
    /// <summary>
    /// WeeklySummary
    /// </summary>
    [DataContract]
    public class WeeklySummary
    {
        [DataMember]
        public Period Period { set; get; }

        [DataMember]
        public Entity Entity { set; get; }

        [DataMember]
        public string BaseCurrency { set; get; }

        [DataMember]
        public decimal ExchangeRate { set; get; }

        [DataMember]
        public decimal BasePrevBalance { set; get; }

        [DataMember]
        public decimal SGDPrevBalance { set; get; }

        [DataMember]
        public decimal BaseWinAndLoss { set; get; }

        [DataMember]
        public decimal SGDWinAndLoss { set; get; }

        [DataMember]
        public decimal BaseTransfer { set; get; }

        [DataMember]
        public decimal SGDTransfer { set; get; }

        [DataMember]
        public decimal BasePrevTransaction { set; get; }

        [DataMember]
        public decimal SGDPrevTransaction { set; get; }

        [DataMember]
        public decimal BaseTransaction { set; get; }

        [DataMember]
        public decimal SGDTransaction { set; get; }

        [DataMember]
        public decimal BaseBalance { set; get; }

        [DataMember]
        public decimal SGDBalance { set; get; }

        [DataMember]
        public WeeklySummaryStatus Status { set; get; }

        [DataMember]
        public User ConfirmUser { set; get; }

        public WeeklySummary(Period period = null, Entity entity = null)
        {
            Period = (period ?? new Period());

            if (entity == null)
            {
                Entity = new Entity();
                BaseCurrency = string.Empty;
                ExchangeRate = 1;
            }
            else
            {
                Entity = entity;
                BaseCurrency = entity.Currency.CurrencyID;
                ExchangeRate = entity.ExchangeRate;
            }

            Status = WeeklySummaryStatus.None;
            ConfirmUser = new User();
        }

        public static WeeklySummary DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<WeeklySummary>(json.Trim());
        }

        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    /// <summary>
    /// WeeklySummaryCollection
    /// </summary>
    public class WeeklySummaryCollection : List<WeeklySummary>
    {
        public WeeklySummaryCollection()
        {
        }

        public WeeklySummaryCollection(IEnumerable<WeeklySummary> collection)
            : base(collection)
        {
        }

        public WeeklySummaryCollection(int capacity)
            : base(capacity)
        {
        }

        public static WeeklySummaryCollection DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<WeeklySummaryCollection>(json.Trim());
        }

        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    /// <summary>
    /// WeeklySummaryStatus
    /// </summary>
    public enum WeeklySummaryStatus
    {
        None, //0
        Confirm, //1
    }
}
