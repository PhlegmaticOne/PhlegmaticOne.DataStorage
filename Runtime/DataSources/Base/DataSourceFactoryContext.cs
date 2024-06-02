using PhlegmaticOne.DataStorage.Infrastructure.Dispatcher;
using PhlegmaticOne.DataStorage.Infrastructure.Helpers;

namespace PhlegmaticOne.DataStorage.DataSources.Base
{
    public class DataSourceFactoryContext
    {
        public DataSourceFactoryContext(IMainThreadDispatcher dispatcher)
        {
            MainThreadDispatcher = ExceptionHelper.EnsureNotNull(dispatcher, nameof(dispatcher));
        }

        public IMainThreadDispatcher MainThreadDispatcher { get; }
    }
}