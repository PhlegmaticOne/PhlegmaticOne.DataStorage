# Data Storage

### Data storage for Unity with PlayerPrefs and Files support

##

## Zenject Installer

```cs
public class DataStorageInstaller : MonoInstaller {
    [SerializeField] private DataStorageConfiguration _configuration;
    
    private class DataStorageFactory : IFactory<DataStorage> {
        private readonly DataStorageSourcesContainer _container;
        private readonly IList<IService> _services;

        public DataStorageFactory(DataStorageSourcesContainer container, IList<IService> services) {
            _container = container;
            _services = services;
        }
        
        public DataStorage Create() {
            return new DataStorage(_container, _services);
        }
    }
    
    private class ChangeTrackerTimeIntervalFactory : IFactory<ChangeTrackerTimeInterval> {
        private readonly DataStorage _dataStorage;
        private readonly ChangeTrackerConfiguration _configuration;

        public ChangeTrackerTimeIntervalFactory(DataStorage dataStorage, ChangeTrackerConfiguration configuration) {
            _dataStorage = dataStorage;
            _configuration = configuration;
        }

        public ChangeTrackerTimeInterval Create() {
            return new ChangeTrackerTimeInterval(_dataStorage, _configuration);
        }
    }
    
    public override void InstallBindings() {
        BindDataStorage();
        BindChangeTracker();
        BindSourcesContainer();
        BindApplicationServices();
    }

    private void BindDataStorage() {
        Container.BindInterfacesAndSelfTo<DataStorage>().FromFactory<DataStorage, DataStorageFactory>().AsSingle();
    }

    private void BindChangeTracker() {
        var configuration = _configuration.GetChangeTrackerConfiguration();
        Container.Bind<ChangeTrackerConfiguration>().FromInstance(configuration).AsSingle();
        Container.BindInterfacesTo<ChangeTrackerTimeInterval>()
            .FromFactory<ChangeTrackerTimeInterval, ChangeTrackerTimeIntervalFactory>()
            .AsSingle();
    }

    private void BindSourcesContainer() {
        var infrastructure = _configuration.GetSourceContainer();
        Container.BindInstance(infrastructure).AsSingle();
    }

    private void BindApplicationServices() {
        var serviceType = typeof(IService);
        var types = Assembly.GetAssembly(typeof(DataStorageInstaller)).GetTypes()
            .Where(x => serviceType.IsAssignableFrom(x) && x.IsAbstract == false);
        
        foreach (var type in types) {
            Container.BindInterfacesTo(type).AsSingle();
        }
    }
}
```

## UniTask frame interval ChangeTracker

```cs
public class ChangeTrackerFrameInterval : ChangeTrackerDataStorage {
    public ChangeTrackerFrameInterval(DataStorage dataStorage, ChangeTrackerConfiguration configuration) 
        : base(dataStorage, configuration) { }

    public override async Task TrackAsync(CancellationToken cancellationToken = default) {
        var intervalFrames = Configuration.FramesInterval;

        try {
            await UniTask.DelayFrame(Configuration.DelayFrames, cancellationToken: cancellationToken);
            await foreach (var _ in UniTaskAsyncEnumerable
                               .IntervalFrame(intervalFrames)
                               .WithCancellation(cancellationToken)) {
                await SaveChanges(cancellationToken);
            }
        }
        catch (OperationCanceledException) {
            Logger.LogCancellation();
        }
        catch (Exception e) {
            Debug.LogException(new ChangeTrackerException(e));
        }
    }
}
```

# Usage

## Model

```cs
[Serializable]
[DataContract]
public class PlayerState : IModel {
    [JsonProperty("coins")] [DataMember] private int _coins;

    [JsonConstructor]
    public PlayerState(int coins) => _coins = coins;

    [JsonIgnore] 
    public int Coins => _coins;

    public void ChangeCoins(int delta) {
        var newCoins = _coins + delta;
        
        if (newCoins < 0) {
            throw new Exception("no");
        }

        _coins = newCoins;
    }
}
```

## Service

```cs
public class TestService {
    private readonly IDataStorage _dataStorage;
    
    private IValueSource<PlayerState> _playerState;

    public TestService(IDataStorage dataStorage) {
        _dataStorage = dataStorage;
    }

    public async Task InitializeAsync() {
        _playerState = await _dataStorage.ReadAsync<PlayerState>();
    }

    public int Coins => _playerState.AsNoTrackable().Coins;

    public void AddCoins(int coins) => _playerState.AsTrackable().ChangeCoins(coins);
    
    public void SubtractCoins(int coins) => AddCoins(-coins);
}
```

### ```AsTrackable``` and ```AsNoTrackable```

- ```AsTrackable``` - should be called for editable operations - ```ChangeTracker``` will save model if ```TrackedChanges > 0``` and ```AsTrackable``` increases that value
- ```AsNoTrackable``` - should be called for readonly operations - ```TrackedChanges``` will not be increased
