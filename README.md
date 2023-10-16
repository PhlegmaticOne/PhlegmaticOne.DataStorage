# Data Storage

### Data storage for Unity with PlayerPrefs and Files support

##

## Zenject Installer

```cs
public class DataStorageInstaller : MonoInstaller {
    [SerializeField] private DataStorageConfigBase _dataStorageConfig;
    [SerializeField] private ChangeTrackerConfig _changeTrackerConfig;

    private class DataStorageFactory : IFactory<DataStorage> {
        private readonly IDataSourceFactory _dataSourceFactory;
        private readonly IList<IService> _services;

        public DataStorageFactory(IDataSourceFactory dataSourceFactory, IList<IService> services) {
            _dataSourceFactory = dataSourceFactory;
            _services = services;
        }
        
        public DataStorage Create() {
            var set = new DataSourcesSet(_dataSourceFactory);
            return new DataStorage(set, _services);
        }
    }
    
    private class ChangeTrackerTimeIntervalFactory : IFactory<ChangeTrackerTimeInterval> {
        private readonly DataStorage _dataStorage;
        private readonly ChangeTrackerConfiguration _configuration;
        private readonly IChangeTrackerLogger _changeTrackerLogger;

        public ChangeTrackerTimeIntervalFactory(
            DataStorage dataStorage,
            ChangeTrackerConfiguration configuration,
            IChangeTrackerLogger changeTrackerLogger) {
            _dataStorage = dataStorage;
            _configuration = configuration;
            _changeTrackerLogger = changeTrackerLogger;
        }
        
        public ChangeTrackerTimeInterval Create() {
            return new ChangeTrackerTimeInterval(_dataStorage, _configuration, _changeTrackerLogger);
        }
    }
    
    public override void InstallBindings() {
        BindDataStorage();
        BindChangeTracker();
        BindDataSourcesSet();
        BindApplicationServices();
    }

    private void BindDataStorage() {
        Container.BindInterfacesAndSelfTo<DataStorage>().FromFactory<DataStorage, DataStorageFactory>().AsSingle();
    }

    private void BindChangeTracker() {
        var configuration = _changeTrackerConfig.GetChangeTrackerConfig();
        Container.Bind<ChangeTrackerConfiguration>().FromInstance(configuration).AsSingle();
        Container.Bind<IChangeTrackerLogger>().To<ChangeTrackerLoggerDebug>().AsSingle();
        Container.BindInterfacesTo<ChangeTrackerTimeInterval>()
            .FromFactory<ChangeTrackerTimeInterval, ChangeTrackerTimeIntervalFactory>()
            .AsSingle();
    }

    private void BindDataSourcesSet() {
        var sourceFactory = _dataStorageConfig.GetSourceFactory();
        Container.BindInstance(sourceFactory).AsSingle();
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

### ```AsTrackable``` and ```AsNoTrackable```

- ```AsTrackable``` - should be called for editable operations - ```ChangeTracker``` will save model if ```TrackedChanges > 0``` and ```AsTrackable``` increases that value
- ```AsNoTrackable``` - should be called for readonly operations - ```TrackedChanges``` will not be increased

# Usage

## Model

```cs
public enum CurrencyType {
    Coins = 0,
    Gems = 1
}

[Serializable]
[DataContract]
public class PlayerState : IModel {
    [JsonProperty] [DataMember] private string _name;
    [JsonProperty] [DataMember] private int _coins;
    [JsonProperty] [DataMember] private int _gems;

    [JsonConstructor]
    public PlayerState(string name, int coins, int gems) {
        _name = name;
        _coins = coins;
        _gems = gems;
    }

    public static PlayerState Initial => new PlayerState(string.Empty, 0, 0);

    [JsonIgnore] public int Coins => _coins;

    [JsonIgnore] public int Gems => _gems;

    public void ChangeName(string name) => _name = name;
    public void ChangeCoins(int delta) {
        _coins += delta;
        _coins = Mathf.Clamp(_coins, 0, int.MaxValue);
    }

    public void ChangeGems(int delta) {
        _gems += delta;
        _gems = Mathf.Clamp(_gems, 0, int.MaxValue);
    }
}
```

## Service

```cs
public interface IPlayerCurrencyService {
    Task InitializeAsync(CancellationToken cancellationToken);
    void ChangeCurrency(int delta, CurrencyType currencyType);
    int GetCurrency(CurrencyType currencyType);
}

public class PlayerCurrencyService : IPlayerCurrencyService {
    private readonly IDataStorage _dataStorage;
    private IValueSource<PlayerState> _playerState;

    public PlayerCurrencyService(IDataStorage dataStorage) {
        _dataStorage = dataStorage;
    }
    
    public async Task InitializeAsync(CancellationToken cancellationToken) {
        _playerState = await _dataStorage.ReadAsync<PlayerState>(cancellationToken);
        
        if (_playerState.NoValue()) {
            _playerState.SetRaw(PlayerState.Initial);  
        }
    }

    public int GetCurrency(CurrencyType currencyType) {
        return currencyType switch {
            CurrencyType.Coins => _playerState.AsNoTrackable().Coins,
            CurrencyType.Gems => _playerState.AsNoTrackable().Gems,
            _ => throw new ArgumentOutOfRangeException(nameof(currencyType), currencyType, null)
        };
    }

    public void ChangeCurrency(int delta, CurrencyType currencyType) {
        switch (currencyType) {
            case CurrencyType.Coins:
                _playerState.AsTrackable().ChangeCoins(delta);
                break;
            case CurrencyType.Gems:
                _playerState.AsTrackable().ChangeGems(delta);
                break;
        }
    }
}
```

## Change Tracker Config

![image](https://github.com/PhlegmaticOne/PhlegmaticOne.DataStorage/assets/73738250/c3635274-af43-4582-bc5b-e6c5e7f4f414)

- ```TimeInterval``` - saves all tracked changed every ```TimeInterval``` seconds
- ```TimeDelay``` - initial delay before change tracking starts
- ```IsChangeTrackerVerbose``` - if ```true``` enables logging for information such as: amount of tracked changes and errors

Available at Create -> Data Storage -> Change Tracker Config

## Key Resolver Configuration

Availdable at Create -> Data Storage -> Type Name Key Resolver Configuration

## Storage configs

![image](https://github.com/PhlegmaticOne/PhlegmaticOne.DataStorage/assets/73738250/1fae1f2b-8c7c-45e0-9d83-9b33685dce3c)

Package includes following storages and their configs:
- ```InMemory``` - all data is stored in memory only in the current session and is cleared when exiting the game
- ```PlayerPrefs``` - data is stroring in PlayerPrefs in one of 2 possible formats: JSON or XML
- ```Files``` - data is stroring in files in one of 3 possible formats: JSON, XML or Binary. 

## Script for initializing DataStorage and ChangeTracker

```cs
public class DataStorageProvider : MonoBehaviour {
    [SerializeField] private DataStorageConfigBase _dataStorageConfig;
    [SerializeField] private ChangeTrackerConfig _changeTrackerConfig;

    private IChangeTracker _changeTracker;
    public IDataStorage DataStorage { get; private set; }
    public CancellationTokenSource TokenSource { get; private set; }
    
    public void Initialize() {
        var set = new DataSourcesSet(_dataStorageConfig.GetSourceFactory());
        var changeTrackerConfig = _changeTrackerConfig.GetChangeTrackerConfig();
        
        var dataStorage = new DataStorage(set);
        var logger = new ChangeTrackerLoggerDebug(changeTrackerConfig);
        
        DataStorage = dataStorage;
        TokenSource = new CancellationTokenSource();
        _changeTracker = new ChangeTrackerTimeInterval(dataStorage, changeTrackerConfig, logger);
    }

    private void Start() {
        _changeTracker.TrackAsync(TokenSource.Token);
    }

    private void OnDestroy() {
        TokenSource.Cancel();
    }
}
```

## Setup from Editor

![image](https://github.com/PhlegmaticOne/PhlegmaticOne.DataStorage/assets/73738250/93f06280-0199-4e40-a9b6-5cf6ddc5c916)

## Test script
```cs
public class SampleDataStorageUsage : MonoBehaviour {
    [SerializeField] private DataStorageProvider _dataStorageProvider;
    [SerializeField] private Button _addCoinsButton;
    [SerializeField] private Button _subtractCoinsButton;
    [SerializeField] private Text _infoText;
    [SerializeField] private int _testCoins;

    private IPlayerCurrencyService _playerCurrencyService;

    private void Awake() {
        _dataStorageProvider.Initialize();
        _addCoinsButton.onClick.AddListener(AddCoins);
        _subtractCoinsButton.onClick.AddListener(SubtractCoins);
        _playerCurrencyService = new PlayerCurrencyService(_dataStorageProvider.DataStorage);
    }

    private async void Start() {
        var token = _dataStorageProvider.TokenSource.Token;
        await _playerCurrencyService.InitializeAsync(token);
        UpdateInfoText();
    }
    
    private void SubtractCoins() {
        _playerCurrencyService.ChangeCurrency(-_testCoins, CurrencyType.Coins);
        UpdateInfoText();
    }

    private void AddCoins() {
        _playerCurrencyService.ChangeCurrency(_testCoins, CurrencyType.Coins);
        UpdateInfoText();
    }

    private void UpdateInfoText() {
        var coins = _playerCurrencyService.GetCurrency(CurrencyType.Coins);
        var text = $"Player has {coins} coins now!";
        _infoText.text = text;
    }
}
```
