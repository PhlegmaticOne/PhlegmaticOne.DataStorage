# Data Storage

### Data storage for Unity with PlayerPrefs and Files support

##

## Zenject Installer

```cs
public class DataStorageInstaller : MonoInstaller {
    [SerializeField] private DataStorageMonoProvider _dataStorageProvider;

    public override void InstallBindings() {
        _dataStorageProvider.Initialize();
        _dataStorageProvider.StartChangeTracker();
        BindDataStorage();
    }

    private void BindDataStorage() {
        Container.BindInterfacesAndSelfTo<DataStorage>()
            .FromInstance(_dataStorageProvider.DataStorage)
            .AsSingle();
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

### Interface

```cs
public interface IPlayerCurrencyService {
    Task InitializeAsync();

    void ChangeCurrency(int delta, CurrencyType currencyType);

    int GetCurrency(CurrencyType currencyType);
}
```

```cs
public class PlayerCurrencyService : IPlayerCurrencyService {
    private readonly IDataStorage _dataStorage;
    private IValueSource<PlayerState> _playerState;

    public PlayerCurrencyService(IDataStorage dataStorage) {
        _dataStorage = dataStorage;
    }
    
    public async Task InitializeAsync() {
        _playerState = await _dataStorage.ReadAsync<PlayerState>();
        
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
or

```cs
public class PlayerCurrencyService : IPlayerCurrencyService {
    private readonly IValueSource<PlayerState> _playerState;

    public PlayerCurrencyService(IValueSource<PlayerState> playerState) {
        _playerState = playerState;
    }
    
    public async Task InitializeAsync() {
        await _playerState.InitializeAsync();
        
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

Available at Create -> Data Storage -> Type Name Key Resolver Configuration

## Storage configs

![image](https://github.com/PhlegmaticOne/PhlegmaticOne.DataStorage/assets/73738250/1fae1f2b-8c7c-45e0-9d83-9b33685dce3c)

Package includes following storages and their configs:
- ```InMemory``` - all data is stored in memory only in the current session and is cleared when exiting the game
- ```PlayerPrefs``` - data is stroring in PlayerPrefs in one of 2 possible formats: JSON or XML
- ```Files``` - data is stroring in files in one of 3 possible formats: JSON, XML or Binary.

Available at Create -> Data Storage -> Storages -> {StorageType}

## Data Storage Provider config

![image](https://github.com/PhlegmaticOne/PhlegmaticOne.DataStorage/assets/73738250/73fcbe68-3916-4643-bff6-34fc40ad2d67)

Includes Change Tracker config and Storage configs. Storage configs can have multiple configs, but for DataStorage instantiation the **_first_ config will be used**

# Setup from Editor

## Test scene

![image](https://github.com/PhlegmaticOne/PhlegmaticOne.DataStorage/assets/73738250/2af5b842-03df-4da9-ab1b-cddfcd2afe4c)

## Test scripts on GameObject

![image](https://github.com/PhlegmaticOne/PhlegmaticOne.DataStorage/assets/73738250/9336883a-6b86-42c6-82c8-67e477d54e2d)

## Test script
```cs
public class SampleDataStorageUsage : MonoBehaviour {
    [SerializeField] private DataStorageMonoProvider _dataStorageProvider;
    [SerializeField] private Button _addCoinsButton;
    [SerializeField] private Button _subtractCoinsButton;
    [SerializeField] private Text _infoText;
    [SerializeField] private int _testCoins;

    private IPlayerCurrencyService _playerCurrencyService;

    private void Awake() {
        _dataStorageProvider.Initialize();
        _addCoinsButton.onClick.AddListener(AddCoins);
        _subtractCoinsButton.onClick.AddListener(SubtractCoins);

        var playerState = _dataStorageProvider.NewValueSource<PlayerState>();
        _playerCurrencyService = new PlayerCurrencyService(playerState);
    }

    private async void Start() {
        _dataStorageProvider.StartChangeTracker();
        await _playerCurrencyService.InitializeAsync();
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
