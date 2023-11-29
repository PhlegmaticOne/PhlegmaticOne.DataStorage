using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using PhlegmaticOne.DataStorage.Contracts;

namespace LoadTest.Processors.Levels {
    [Serializable]
    [DataContract]
    public class LevelsState : IModel {
        [DataMember] [JsonProperty] private Dictionary<string, LevelState> _levelStates;
        
        [JsonConstructor]
        public LevelsState(Dictionary<string, LevelState> levelStates) {
            _levelStates = levelStates;
        }

        public static LevelsState Initial => new LevelsState(new Dictionary<string, LevelState> {
            { "ru", LevelState.Initial },
            { "en", LevelState.Initial }
        });

        [JsonIgnore] [IgnoreDataMember] 
        public Dictionary<string, LevelState> LevelStates => _levelStates;
    }
}