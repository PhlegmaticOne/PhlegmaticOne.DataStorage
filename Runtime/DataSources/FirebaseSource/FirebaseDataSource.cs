using System.Threading;
using System.Threading.Tasks;
using Firebase.Database;
using Newtonsoft.Json;
using PhlegmaticOne.DataStorage.Contracts;
using PhlegmaticOne.DataStorage.DataSources.Base;
using PhlegmaticOne.DataStorage.Infrastructure.Helpers;
using PhlegmaticOne.DataStorage.KeyResolvers.Base;

namespace PhlegmaticOne.DataStorage.DataSources.FirebaseSource {
    internal sealed class FirebaseDataSource<T> : DataSourceBase<T> where T : class, IModel {
        private readonly string _referencePath;
        private readonly DatabaseReference _reference;

        public FirebaseDataSource(IKeyResolver keyResolver) {
            ExceptionHelper.EnsureNotNull(keyResolver, nameof(keyResolver));
            _reference = FirebaseDatabase.DefaultInstance.RootReference;
            _referencePath = keyResolver.ResolveKey<T>();
        }

        protected override Task WriteAsync(T value, CancellationToken cancellationToken) {
            return NodeReference().SetRawJsonValueAsync(ToJson(value));
        }

        public override Task DeleteAsync(CancellationToken cancellationToken) {
            return NodeReference().RemoveValueAsync();
        }

        public override async Task<T> ReadAsync(CancellationToken cancellationToken) {
            var snapshot = await NodeReference().GetValueAsync();
            return GetValueFromDatabase(snapshot);
        }

        private static T GetValueFromDatabase(DataSnapshot snapshot) {
            var json = snapshot.GetRawJsonValue();
            return string.IsNullOrEmpty(json) ? default : JsonConvert.DeserializeObject<T>(json);
        }

        private DatabaseReference NodeReference() => _reference.Child(_referencePath);

        private static string ToJson(T value) => JsonConvert.SerializeObject(value);
    }
}