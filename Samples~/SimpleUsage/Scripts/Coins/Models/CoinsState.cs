using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using PhlegmaticOne.DataStorage.Contracts;

namespace SimpleUsage.Coins.Models {
    [Serializable]
    [DataContract]
    public class CoinsState : IModel {
        [JsonProperty] [DataMember] private int _coins;

        [JsonConstructor]
        public CoinsState(int coins) {
            _coins = coins;
        }

        public static CoinsState Initial => new CoinsState(0);

        [JsonIgnore]
        public int Coins {
            get => _coins;
            set => _coins = value;
        }
    }
}