using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using PhlegmaticOne.DataStorage.Contracts;

namespace LoadTest.Processors.Levels
{
    [Serializable]
    [DataContract]
    public class LevelState : IModel
    {
        [JsonConstructor]
        public LevelState(int packId, int levelId, bool isScoringLevel, List<char> characters, List<string> words)
        {
            Characters = characters;
            Words = words;
            PackId = packId;
            LevelId = levelId;
            IsScoringLevel = isScoringLevel;
        }

        public static LevelState Initial => new LevelState(0, 0, false, new List<char>(), new List<string>());

        [DataMember] public int PackId { get; set; }

        [DataMember] public int LevelId { get; set; }

        [DataMember] public bool IsScoringLevel { get; set; }

        [JsonProperty]
        [DataMember]
        public List<char> Characters { get; }

        [JsonProperty]
        [DataMember]
        public List<string> Words { get; }
    }
}