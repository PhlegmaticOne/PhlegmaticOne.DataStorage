# Data storage

### Storage - PlayerPrefs and Files

## Zenject Installer

```csharp
public class DataStorageInstaller : MonoInstaller {
    [SerializeField] private DataStorageConfiguration _configuration;
    
    public override void InstallBindings() {
        BindDataStorage();
        BindSourcesContainer();
        BindGameServices();
    }

    private void BindDataStorage() {
        Container.Bind<IDataStorage>().To<DataStorage>().AsSingle();
    }

    private void BindSourcesContainer() {
        var infrastructure = _configuration.GetSourceContainer();
        Container.BindInstance(infrastructure).AsSingle();
    }
    
    private void BindGameServices() {
        var serviceType = typeof(IService);
        var types = Assembly.GetAssembly(typeof(DataStorageInstaller)).GetTypes()
            .Where(x => serviceType.IsAssignableFrom(x) && x.IsAbstract == false);
        
        foreach (var type in types) {
            Container.BindInterfacesTo(type).AsSingle();
        }
    }
}
```

## License

[MIT](https://choosealicense.com/licenses/mit/)