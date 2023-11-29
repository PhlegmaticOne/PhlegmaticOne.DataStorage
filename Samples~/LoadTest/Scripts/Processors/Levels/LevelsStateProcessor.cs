using System.Linq;
using LoadTest.Common;
using UnityEngine;

namespace LoadTest.Processors.Levels {
    public class LevelsStateProcessor : ProcessorDataBase<LevelsState> {
        protected override LevelsState GetInitialValue() {
            return LevelsState.Initial;
        }

        protected override void DoRandomAction(LevelsState levelsState) {
            var rnd = Random.Range(0, levelsState.LevelStates.Count);
            var value = levelsState.LevelStates.ElementAt(rnd).Value;
            
            value.LevelId = Random.Range(0, 100);
            value.PackId = Random.Range(0, 100);
            
            ProcessRandomItemInList(value.Characters, 
                Create.RandomChar,
                _ => Create.RandomChar());
            
            ProcessRandomItemInList(value.Words,
                () => Create.RandomString(2, 16),
                _ => Create.RandomString(2, 16));
        }
    }
}