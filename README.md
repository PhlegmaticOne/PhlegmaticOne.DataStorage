# Data Storage

### Data storage for Unity with PlayerPrefs and Files support

## ```IDataStorage```

Interface defines methods for reading and writing data. Default implementation can read and save data in Memory, PlayerPrefs and Files.

## ```ChangeTracker```

Package provides ChangeTracker, which saves all changes made by user, if it was started.

## ```IValueSource<T>```

Objects that implement this interface work directly with ```IDataStorage``` and have similar methods for reading and saving data.
It seems to be more convenient to work with ```IValueSource<T>``` instead of ```IDataStorage```.

### ```Value``` and ```TrackableValue```

- ```TrackableValue``` - should be called for editable operations - ```ChangeTracker``` will save model if ```TrackedChanges > 0``` and ```TrackableValue``` increases that value
- ```Value``` - should be called for readonly operations - ```TrackedChanges``` will not be increased

If ```ChangeTracker``` is not working calling ```Value``` or ```TrackableValue``` is the same.

# Usage

## Build ```IDataStorage```

```cs
var dataStorage = DataStorageBuilder.Create()
                .UseDataSource(x => x.InMemory())
                //.UseDataSource(x => x.PlayerPrefs())
                //.UseDataSource(x => x.File())
                .UseLogger() // Not required - no logs
                .UseChangeTracker() //Not required - change tracker will not start
                .Build();
```

## Model

```cs
[Serializable]
[DataContract]
public class CoinsState : IModel {
    [JsonProperty] [DataMember] private int _coins;

    [JsonConstructor]
    public CoinsState(int coins) {
        _coins = coins;
    }

    public static CoinsState Initial => new CoinsState(0);

    [JsonIgnore]
    public int Coins {
        get => _coins;
        set => _coins = value;
    }
}
```

## Service

### Interface

```cs
public interface ICoinsService {
    Task InitializeAsync();
    int Coins { get; }
    void ChangeCoins(int delta);
}
```

### Implementation

```cs
public class CoinsService : ICoinsService {
    private readonly IValueSource<CoinsState> _coinsState;

    public CoinsService(IValueSource<CoinsState> coinsState) {
        _coinsState = coinsState;
    }
    
    public async Task InitializeAsync() {
        await _coinsState.InitializeAsync();
        
        if (_coinsState.HasNoValue()) {
            _coinsState.SetRawValue(CoinsState.Initial);  
        }
    }

    public int Coins => _coinsState.Value.Coins;
    public void ChangeCoins(int delta) => _coinsState.TrackableValue.Coins += delta;
}
```

## Test script
```cs
public class SampleDataStorageUsage : MonoBehaviour
{
    private IDataStorage _dataStorage;

    private void Awake()
    {
        _dataStorage = DataStorageBuilder.Create()
            .UseDataSource(x => x.PlayerPrefs())
            .UseLogger()
            .UseChangeTracker()
            .Build();
        
        var valueSource = _dataStorage.GetValueSource<CoinsState>("coins");
        var coinsService = new CoinsService(valueSource);
    }

    private void OnApplicationQuit()
    {
        _dataStorage.Cancel();
    }
}
```

## Zenject Installer

```DataStorage``` and ```ValueSource<T>``` types can be registered in DI-container. Way to setup registration of these types for Zenject is shown below.

```cs
public static class DataStorageZenjectExtensions
{
    public static void BindValueSource<T>(this DiContainer diContainer) where T : class, IModel
    {
        diContainer.Bind<IValueSource<T>>().FromFactory<ValueSourceFactory<T>>().AsSingle();
    }
}

public class ValueSourceFactory<T> : IFactory<IValueSource<T>> where T : class, IModel
{
    private readonly IDataStorage _dataStorage;
    public ValueSourceFactory(IDataStorage dataStorage) => _dataStorage = dataStorage;
    public IValueSource<T> Create() => _dataStorage.GetValueSource<T>();
}

public class DataStorageInstaller : MonoInstaller {
    
    public override void InstallBindings() {
        BindDataStorage();
    }

    private void BindDataStorage()
    {
        var dataStorage = DataStorageBuilder.Create()
                .UseDataSource(x => x.PlayerPrefs())
                .UseLogger()
                .UseChangeTracker()
                .Build();
        
        Container.Bind<IDataStorage>().FromInstance(dataStorage).AsSingle();
        
        //Container.BindValueSource<AnyModel>();
    }
}
```
