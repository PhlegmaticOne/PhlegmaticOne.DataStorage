using LoadTest.Common;
using UnityEngine;

namespace LoadTest.Processors.Levels {
    public class LevelStateProcessor : ProcessorDataBase<LevelState> {
        protected override LevelState GetInitialValue() {
            return LevelState.Initial;
        }

        protected override void DoRandomAction(LevelState value) {
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