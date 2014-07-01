using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Oleit.AS.Service.DataObject
{
    /// <summary>
    /// Account
    /// </summary>
    [DataContract]
    public class Account
    {
        [DataMember]
        public int ID { set; get; }

        [DataMember]
        public int EntityID { set; get; }

        [DataMember]
        public int Company { set; get; }

        [DataMember]
        public string AccountName { set; get; }

        [DataMember]
        public string Password { set; get; }

        [DataMember]
        public AccountType AccountType { set; get; }

        [DataMember]
        public decimal BettingLimit { set; get; }

        [DataMember]
        public Status Status { set; get; }

        [DataMember]
        public string Color { set; get; }

        [DataMember]
        public string Factor { set; get; }

        [DataMember]
        public string DateOpen { set; get; }

        [DataMember]
        public string Personnel { set; get; }

        [DataMember]
        public string IP { set; get; }

        [DataMember]
        public string Odds { set; get; }

        [DataMember]
        public string IssuesConditions { set; get; }

        [DataMember]
        public string RemarksAcc { set; get; }

        [DataMember]
        public decimal Perbet { set; get; }

        public Account()
        {
            AccountName = string.Empty;
            Password = string.Empty;
            AccountType = DataObject.AccountType.SuperSenior;
            Status = DataObject.Status.Suspended;
            Perbet = 0;
            ID = 0;
        }

        public static Account DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<Account>(json.Trim());
        }

        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    /// <summary>
    /// AccountCollection
    /// </summary>
    public class AccountCollection : List<Account>
    {
        public AccountCollection()
        {
        }

        public AccountCollection(IEnumerable<Account> collection)
            : base(collection)
        {
        }

        public AccountCollection(int capacity)
            : base(capacity)
        {
        }

        public static AccountCollection DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<AccountCollection>(json.Trim());
        }

        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    /// <summary>
    /// AccountType
    /// </summary>
    public enum AccountType
    {
        SuperSenior = 1, //1
        Senior, //2
        Master, //3
        Agent, //4
        Members, //5
    }

    /// <summary>
    /// Status
    /// </summary>
    public enum Status
    {
        Suspended = 1, //1
        Closed, //2
        NoFight, //3
        FollowBet, //4
        LousyOdds, //5

        /// <summary>
        /// take statement
        /// </summary>
        NeedToOpenBack, //6
        Others, //7
    }
}
