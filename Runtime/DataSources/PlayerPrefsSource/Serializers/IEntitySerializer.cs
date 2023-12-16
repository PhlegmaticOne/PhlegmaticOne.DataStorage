namespace PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource.Serializers
{
    public interface IEntitySerializer
    {
        string Serialize<T>(T value);
        T Deserialize<T>(string value);
    }
}