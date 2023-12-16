using UnityEngine;

namespace LoadTest.Processors
{
    public class CoinsProcessor : ProcessorDataBase<CoinsState>
    {
        protected override CoinsState GetInitialValue() => CoinsState.Initial;

        protected override void DoRandomAction(CoinsState value)
        {
            value.Coins = Random.Range(0, 1000);
        }
    }
}