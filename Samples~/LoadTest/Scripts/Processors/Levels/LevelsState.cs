using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using PhlegmaticOne.DataStorage.Contracts;

namespace LoadTest.Processors.Levels
{
    [Serializable]
    [DataContract]
    public class LevelsState : IModel
    {
        [JsonConstructor]
        public LevelsState(Dictionary<string, LevelState> levelStates) => LevelStates = levelStates;

        public static LevelsState Initial => new LevelsState(new Dictionary<string, LevelState>
        {
            {"ru", LevelState.Initial},
            {"en", LevelState.Initial}
        });

        [JsonIgnore]
        [IgnoreDataMember]
        [field: DataMember]
        [field: JsonProperty]
        public Dictionary<string, LevelState> LevelStates { get; }
    }
}