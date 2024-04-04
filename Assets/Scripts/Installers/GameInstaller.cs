using Managers;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind(typeof(IUIManager),typeof(IInitializable)).To<UIManager>().AsSingle().NonLazy();
        Container.Bind<PoolManager>().AsSingle().NonLazy();
        Container.Bind<HttpClientHelper>().AsSingle().NonLazy();
    }
}
