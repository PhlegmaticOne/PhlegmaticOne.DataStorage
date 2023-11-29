using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Contracts;
using PhlegmaticOne.DataStorage.Storage.Base;
using PhlegmaticOne.DataStorage.Storage.ValueSources;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LoadTest.Processors {
    public abstract class ProcessorDataBase : MonoBehaviour {
        public float TimeToNextAction { get; set; }
        public abstract Task OnInitialize(IDataStorage dataStorage);
        public abstract void OnActionTrackInstant(IDataStorage dataStorage);
        public abstract void OnAction(IDataStorage dataStorage);
    }

    public abstract class ProcessorDataBase<T> : ProcessorDataBase where T : class, IModel {
        [SerializeField] private TextMeshProUGUI _nameText;

        private void OnValidate() {
            if (_nameText != null) {
                _nameText.text = typeof(T).Name;
            }
        }

        public override Task OnInitialize(IDataStorage dataStorage) {
            return dataStorage.GetOrCreateValueSource<T>().InitializeAsync();
        }

        public override void OnActionTrackInstant(IDataStorage dataStorage) {
            var valueSource = dataStorage.GetOrCreateValueSource<T>();
            
            if (valueSource.HasNoValue()) {
                valueSource.SetRawValue(GetInitialValue());
            }
            else {
                DoRandomAction(valueSource.Value);
            }
            
            valueSource.EnqueueForSaving();
        }

        public override void OnAction(IDataStorage dataStorage) {
            var valueSource = dataStorage.GetOrCreateValueSource<T>();
            
            if (valueSource.HasNoValue()) {
                valueSource.SetRawValue(GetInitialValue());
            }
            else {
                DoRandomAction(valueSource.TrackableValue);
            }
        }

        protected abstract T GetInitialValue();

        protected abstract void DoRandomAction(T value);

        protected void ProcessRandomItemInList<TItem>(
            List<TItem> items,
            Func<TItem> itemFactory,
            Func<TItem, TItem> itemChangeAction) 
        {
            if (items.Count >= 50) {
                if (Random.value <= 0.7) {
                    var removeIndex = Random.Range(0, items.Count);
                    items.RemoveAt(removeIndex);
                }
                return;
            }

            if (Random.value <= 0.5 || items.Count == 0) {
                var item = itemFactory();
                items.Add(item);
            }
            else {
                var random = Random.Range(0, items.Count);
                items[random] = itemChangeAction(items[random]);
            }
        }
    }
}