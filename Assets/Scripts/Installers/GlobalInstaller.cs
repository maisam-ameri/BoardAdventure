using BoardAdventures.Abstractions;
using BoardAdventures.Authentication;
using BoardAdventures.Network;
using Signals;
using Zenject;

namespace Installers
{
    public class GlobalInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            // Signals
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<OnConnectionStatusChangedSignal>();
            Container.DeclareSignal<OnRegisterRequestedSignal>();
            Container.DeclareSignal<OnShowRegistrationUISignal>();
            Container.DeclareSignal<OnPlayerLoggedInSignal>();
            Container.DeclareSignal<OnLobbyStateChangedSignal>();
            Container.DeclareSignal<OnLobbyStateUiChangedSignal>();
            Container.DeclareSignal<OnConnectionRequestSignal>();
            Container.DeclareSignal<OnJoinToRoomRequestSignal>();

            // Services
            Container.Bind<IAccountService>().To<LocalAccountService>().AsSingle();
            Container.Bind<ILobbyService>().To<LobbyService>().AsSingle();
            Container.Bind<INetworkTime>().To<PhotonTimeProvider>().AsSingle();

            // MonoBehaviours
            //Container.Bind<INetworkService>().To<NetworkMockProvider>().FromNewComponentOnNewGameObject().AsSingle();
             Container.Bind<INetworkService>().To<PhotonLauncher>().FromNewComponentOnNewGameObject().AsSingle();
        }
    }
}