# Data Storage

### Data storage for Unity with PlayerPrefs and Files support

## ```IDataStorage```

Interface defines methods for reading and writing data. Default implementation can read and save data in PlayerPrefs and Files.

## ```ChangeTracker```

Package provides ChangeTracker, which saves all changes made by user, if it was started.

## ```IValueSource<T>```

Objects that implement this interface work directly with ```IDataStorage``` and have similar methods for reading and saving data.
It seems to be more convenient to work with ```IValueSource<T>``` instead of ```IDataStorage```.

### ```Value``` and ```TrackableValue```

- ```TrackableValue``` - should be called for editable operations - ```ChangeTracker``` will save model if ```TrackedChanges > 0``` and ```AsTrackable``` increases that value
- ```Value``` - should be called for readonly operations - ```TrackedChanges``` will not be increased

If ```ChangeTracker``` is not working calling ```Value``` or ```TrackableValue``` is the same.

# Usage

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

## Change Tracker Config

- ```TimeInterval``` - saves all tracked changes every ```TimeInterval``` seconds
- ```TimeDelay``` - initial delay before change tracking starts

Available at Create -> Data Storage -> Infrastructure -> Change Tracker Config

## Key Resolver Configuration

Creates an object that defines the key by which the data will be saved: for files this is the file name, for PlayerPrefs this is the key for PlayerPrefs. 
The standard implementation returns the class name as the key - ```typeof(T).Name``` formatted with specified format

Available at Create -> Data Storage -> Infrastructure -> Type Name Key Resolver Configuration

## Logger Configuration

Defines single configuration which is logger Log level. 
This field is an enum with flags attribute and therefore its values can be conbined in different ways.

Available at Create -> Data Storage -> Infrastructure -> Logger Config

## Operation Queue Configuration

Defines single configuration which is queue operations capacity.
Is this option is ```-1``` then Logger queue can have unlimited operation, if this option is positive number then it is operations queue capacity.

Available at Create -> Data Storage -> Infrastructure -> Operations Queue Config

## Storage configs

Package includes following storages and their configs:
- ```InMemory``` - all data is stored in memory only in the current session and is cleared when exiting the game
- ```PlayerPrefs``` - data is stroring in PlayerPrefs in one of 2 possible formats: JSON or XML. Also data encryption/decryption is available.
- ```Files``` - data is stroring in files in one of 3 possible formats: JSON, XML or Binary. Also data encryption/decryption is available.

Available at Create -> Data Storage -> Storages -> {StorageType}

## Data Storage Provider config

![image](https://github.com/PhlegmaticOne/PhlegmaticOne.DataStorage/assets/73738250/e709c8e7-fd8c-4470-b726-7b840846cb1b)

Includes infrastructure configs such as Logger Config, Operations Queue Config, Change Tracker Config and one of configured Storage configs.

Config has Editor button, which creates all infrastructure configs and all storage configs in directory (with default values), where this config is located.

# Setup from Editor

## Test scene

![image](https://github.com/PhlegmaticOne/PhlegmaticOne.DataStorage/assets/73738250/a50630be-a9a9-4919-958e-e1e70911b813)

## Test scripts setup on Scene

![image](https://github.com/PhlegmaticOne/PhlegmaticOne.DataStorage/assets/73738250/0d306d7b-d481-45d1-bb62-9304f393f058)

## Test script
```cs
public class SampleDataStorageUsage : MonoBehaviour {
    [SerializeField] private DataStorageProviderConfig _dataStorageProviderConfig;
    [SerializeField] private ChangePlayerCoinsController _changePlayerCoinsController;
    [SerializeField] private QueueTextLoggingController _queueTextLoggingController;
    [SerializeField] private MainThreadDispatcherTest _mainThreadDispatcher;

    private DataStorageCreationResult _creationResult;

    private void Awake() {
        _creationResult = _dataStorageProviderConfig.CreateDataStorageFromThisConfig();
        var dataStorage = _creationResult.DataStorage;
        var valueSource = dataStorage.GetOrCreateValueSource<CoinsState>();
        var coinsService = new CoinsService(valueSource);
        var queueObserver = dataStorage.GetQueueObserver();
    
        _changePlayerCoinsController.Construct(coinsService);
        _queueTextLoggingController.Construct(queueObserver, _mainThreadDispatcher);
    }

    private async void Start() {
        _ = _creationResult.ChangeTracker.TrackAsync();
        await _changePlayerCoinsController.InitializeAsync();
    }

    private void OnApplicationQuit() {
        _changePlayerCoinsController.OnReset();
        _queueTextLoggingController.OnReset();
        _creationResult.CancellationProvider.Cancel();
    }
}
```

## Zenject Installer

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
    public IValueSource<T> Create() => _dataStorage.GetOrCreateValueSource<T>();
}

public class DataStorageInstaller : MonoInstaller {
    [SerializeField] private DataStorageProviderConfig _dataStorageProvider;
    
    public override void InstallBindings() {
        BindDataStorage();
    }

    private void BindDataStorage()
    {
        var creationResult = _dataStorageProvider.CreateDataStorageFromThisConfig();
        
        Container.Bind<IDataStorage>().FromInstance(creationResult.DataStorage).AsSingle();
        
        //Container.BindValueSource<AnyModel>();
    }
}
```
