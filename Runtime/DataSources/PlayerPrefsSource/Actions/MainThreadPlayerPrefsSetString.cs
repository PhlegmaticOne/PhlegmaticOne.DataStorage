using PhlegmaticOne.DataStorage.Infrastructure.Dispatcher.Actions;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource.Actions {
    public class MainThreadPlayerPrefsSetString : IMainThreadAction {
        private readonly string _key;
        private readonly string _value;
        public MainThreadPlayerPrefsSetString(string key, string value) {
            _key = key;
            _value = value;
        }

        public void Execute() => PlayerPrefs.SetString(_key, _value);
    }
}