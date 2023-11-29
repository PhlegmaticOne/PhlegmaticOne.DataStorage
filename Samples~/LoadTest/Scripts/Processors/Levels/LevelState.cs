using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using PhlegmaticOne.DataStorage.Contracts;

namespace LoadTest.Processors.Levels {
    [Serializable]
    [DataContract]
    public class LevelState : IModel {
        [JsonProperty] [DataMember] private List<char> _characters;
        [JsonProperty] [DataMember] private List<string> _words;

        [JsonConstructor]
        public LevelState(int packId, int levelId, bool isScoringLevel, List<char> characters, List<string> words) {
            _characters = characters;
            _words = words;
            PackId = packId;
            LevelId = levelId;
            IsScoringLevel = isScoringLevel;
        }

        public static LevelState Initial => new LevelState(0, 0, false, new List<char>(), new List<string>());

        [DataMember]
        public int PackId { get; set; }
        [DataMember]
        public int LevelId { get; set; }
        [DataMember]
        public bool IsScoringLevel { get; set; }

        [IgnoreDataMember] [JsonIgnore] 
        public List<char> Characters => _characters;
        
        [IgnoreDataMember] [JsonIgnore] 
        public List<string> Words => _words;
    }
}