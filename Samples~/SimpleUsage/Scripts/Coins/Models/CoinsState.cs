using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using PhlegmaticOne.DataStorage.Contracts;

namespace SimpleUsage.Coins.Models
{
    [Serializable]
    [DataContract]
    public class CoinsState : IModel
    {
        [JsonProperty]
        [DataMember]
        public int Coins { get; set; }
    }
}