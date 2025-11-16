using BoardAdventures.Abstractions;
using BoardAdventures.UI.Menu;
using Zenject;

namespace Installers
{
    public class MenuInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IMenuManager>().To<MenuManager>().FromComponentInHierarchy().AsSingle();
        }
    }
}