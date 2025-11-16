using BoardAdventures.Abstractions;
using BoardAdventures.Network;
using Signals;
using Zenject;

namespace Installers
{
    public class GlobalInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            // Signals
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<OnConnectionStatusChangedSignal>();
            
            // MonoBehaviours
            Container.Bind<INetworkService>().To<PhotonLauncher>().FromNewComponentOnNewGameObject().AsSingle();


        }
    }
}