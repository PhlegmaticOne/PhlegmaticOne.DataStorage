namespace PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource.Converters {
    public interface IEntityConverter {
        string Convert<T>(T value);
        T ConvertBack<T>(string value);
    }
}