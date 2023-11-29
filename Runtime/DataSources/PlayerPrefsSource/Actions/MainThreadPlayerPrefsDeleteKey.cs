using PhlegmaticOne.DataStorage.Infrastructure.Dispatcher.Actions;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource.Actions {
    public class MainThreadPlayerPrefsDeleteKey : IMainThreadAction {
        private readonly string _key;
        public MainThreadPlayerPrefsDeleteKey(string key) => _key = key;
        public void Execute() => PlayerPrefs.DeleteKey(_key);
    }
}