using PhlegmaticOne.DataStorage.Infrastructure.Dispatcher.Actions;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource.Actions
{
    internal sealed class MainThreadPlayerPrefsGetString : IMainThreadFunc<string>
    {
        private readonly string _key;
        
        public MainThreadPlayerPrefsGetString(string key)
        {
            _key = key;
        }

        public string Execute()
        {
            return PlayerPrefs.GetString(_key, string.Empty);
        }
    }
}