# Data storage

### Unity PlayerPrefs and Files

##

## Zenject Installer

```cs
public class DataStorageInstaller : MonoInstaller {
    [SerializeField] private DataStorageConfiguration _configuration;    
    
    public override void InstallBindings() {
        BindDataStorage();
        BindChangeTracker();
        BindSourcesContainer();
        BindApplicationServices();
    }

    private void BindDataStorage() {
        Container.BindInterfacesAndSelfTo<DataStorage>().AsSingle();
    }

    private void BindChangeTracker() {
        var configuration = _configuration.GetChangeTrackerConfiguration();
        Container.Bind<ChangeTrackerConfiguration>().FromInstance(configuration).AsSingle();
        Container.BindInterfacesTo<ChangeTrackerTimeInterval>().AsSingle();
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
            LogCancellation();
        }
        catch (Exception e) {
            Debug.LogException(new ChangeTrackerException(e));
        }
    }
}
```

## License

[MIT](https://choosealicense.com/licenses/mit/)
