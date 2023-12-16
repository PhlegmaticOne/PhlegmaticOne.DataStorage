namespace PhlegmaticOne.DataStorage.Infrastructure.Dispatcher.Actions
{
    public interface IMainThreadFunc<out T>
    {
        T Execute();
    }
}