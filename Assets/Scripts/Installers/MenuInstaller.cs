using BoardAdventures.Abstractions;
using BoardAdventures.Authentication;
using BoardAdventures.UI.Lobby;
using BoardAdventures.UI.Menu;
using Signals;
using Zenject;

namespace Installers
{
    public class MenuInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            // Signals
            Container.DeclareSignal<OnRegisterRequestedSignal>();
            Container.DeclareSignal<OnShowRegistrationUISignal>();
            Container.DeclareSignal<OnPlayerLoggedInSignal>();
            
            Container.Bind<IMenuManager>().To<MenuManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<ILobbyManager>().To<LobbyManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IAccountService>().To<LocalAccountService>().AsSingle();

        }
    }
}