using Firebase.Auth;
using PhlegmaticOne.DataStorage.DataSources.FirebaseSource.Options;
using PhlegmaticOne.DataStorage.Infrastructure.Helpers;
using PhlegmaticOne.DataStorage.KeyResolvers.Base;

namespace PhlegmaticOne.DataStorage.DataSources.FirebaseSource.KeyResolvers {
    internal sealed class FirebaseKeyResolver : IKeyResolver {
        private readonly FirebaseSourceOptions _options;
        private readonly IKeyResolver _keyResolver;

        public FirebaseKeyResolver(IKeyResolver keyResolver, FirebaseSourceOptions options) {
            _options = ExceptionHelper.EnsureNotNull(options, nameof(options));
            _keyResolver = ExceptionHelper.EnsureNotNull(keyResolver, nameof(keyResolver));
        }
        
        public string ResolveKey<T>() {
            var key = _keyResolver.ResolveKey<T>();
            var userId = ResolveUserId();
            return string.Concat("/", userId, "/", key);
        }

        private string ResolveUserId() {
            var userId = _options.IsTestUser ? _options.TestUserId : FirebaseAuth.DefaultInstance.CurrentUser.UserId;
            return userId;
        }
    }
}