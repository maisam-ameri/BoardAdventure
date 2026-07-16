using BoardAdventures.Authentication;
using BoardAdventures.Config;
using BoardAdventures.Network;
using BoardAdventures.Signals;
using Zenject;

namespace BoardAdventures.Installers
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
            Container.DeclareSignal<OnTurnEndTimeChangedSignal>();
            Container.DeclareSignal<OnDiceRollRequestedNetSignal>();
            Container.DeclareSignal<OnDiceRolledSignal>();

            // Services
            Container.Bind<IAccountService>().To<LocalAccountService>().AsSingle();
            Container.Bind<ILobbyService>().To<LobbyService>().AsSingle();
            Container.Bind<INetworkTime>().To<PhotonTimeProvider>().AsSingle();
            Container.Bind<INetworkConfigProvider>().To<PhotonConfigProvider>().AsSingle();

            // MonoBehaviours
            //Container.Bind<INetworkService>().To<NetworkMockProvider>().FromNewComponentOnNewGameObject().AsSingle();
             Container.Bind<INetworkService>().To<PhotonLauncher>().FromNewComponentOnNewGameObject().AsSingle();
        }
    }
}