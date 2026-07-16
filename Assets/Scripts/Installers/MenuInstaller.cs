using BoardAdventures.Network;
using BoardAdventures.Presentation.Lobby;
using BoardAdventures.Presentation.Menu;
using Zenject;

namespace BoardAdventures.Installers
{
    public class MenuInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            
            Container.Bind<MenuManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<ILobbyManager>().To<LobbyManager>().FromComponentInHierarchy().AsSingle();

        }
    }
}