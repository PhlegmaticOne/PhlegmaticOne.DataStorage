using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using PhlegmaticOne.DataStorage.Contracts;
using UnityEngine;

namespace Common.Models {
    [Serializable]
    [DataContract]
    public class PlayerState : IModel {
        [JsonProperty] [DataMember] private string _name;
        [JsonProperty] [DataMember] private int _coins;
        [JsonProperty] [DataMember] private int _gems;

        [JsonConstructor]
        public PlayerState(string name, int coins, int gems) {
            _name = name;
            _coins = coins;
            _gems = gems;
        }

        public static PlayerState Initial => new PlayerState(string.Empty, 0, 0);

        [JsonIgnore] public int Coins => _coins;

        [JsonIgnore] public int Gems => _gems;

        public void ChangeName(string name) => _name = name;
        public void ChangeCoins(int delta) {
            _coins += delta;
            _coins = Mathf.Clamp(_coins, 0, int.MaxValue);
        }

        public void ChangeGems(int delta) {
            _gems += delta;
            _gems = Mathf.Clamp(_gems, 0, int.MaxValue);
        }
    }
}