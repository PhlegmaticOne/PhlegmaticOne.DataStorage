using PhlegmaticOne.DataStorage.Infrastructure.Dispatcher.Actions;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource.Actions {
    public class MainThreadPlayerPrefsGetString : IMainThreadFunc<string> {
        private readonly string _key;
        public MainThreadPlayerPrefsGetString(string key) => _key = key;
        public string Execute() => PlayerPrefs.GetString(_key, string.Empty);
    }
}