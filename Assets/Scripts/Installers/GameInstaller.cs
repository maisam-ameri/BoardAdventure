using BoardAdventures.Abstractions;
using BoardAdventures.Core.GameLogic;
using BoardAdventures.Core.Movement;
using BoardAdventures.Core.Path;
using BoardAdventures.Core.Players;
using BoardAdventures.Managers;
using BoardAdventures.UI.Common;
using BoardAdventures.UI.Players;
using Signals;
using Zenject;

namespace Installers
{
    public class GameInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {

            // Signals
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<OnRewardGrantedSignal>();
            Container.DeclareSignal<OnTurnTimerExpiredSignal>();
            Container.DeclareSignal<OnCapturedSignal>();
            Container.DeclareSignal<OnPlayersCreatedSignal>();
            Container.DeclareSignal<OnGameSetupCompletedSignal>();
            Container.DeclareSignal<OnSelectedPawnSignal>();
            Container.DeclareSignal<OnTurnSwitchedSignal>();
            Container.DeclareSignal<OnTurnStartedSignal>();
            Container.DeclareSignal<OnFirstSixRolledSignal>();
            Container.DeclareSignal<OnDiceRollRequestedSignal>();
            Container.DeclareSignal<OnDiceRolledSignal>();
            Container.DeclareSignal<OnPlayerActionStartedSignal>();
            Container.DeclareSignal<OnPlayerActionCompletedSignal>();
            
            
            // Services
            Container.Bind<IGameFlowService>().To<GameFlowService>().AsSingle();
            Container.Bind<ITurnManagementService>().To<TurnManagementService>().AsSingle().NonLazy();
            Container.Bind<ITurnFlowService>().To<TurnFlowService>().AsSingle();
            Container.Bind<ITurnLogicService>().To<TurnLogicService>().AsSingle();
            Container.Bind<IPlayerActionValidator>().To<PlayerActionValidator>().AsSingle();
            Container.Bind<IPlayerActionService>().To<PlayerActionService>().AsSingle().NonLazy();
            Container.Bind<IPlayerSetupService>().To<PlayerSetupService>().AsSingle();
            Container.Bind<IPawnManager>().To<PawnManager>().AsSingle();
            Container.Bind<IPawnStateService>().To<PawnStateService>().AsSingle();
            Container.Bind<IPawnMovementService>().To<PawnMovementService>().AsSingle();
            Container.Bind<IPathCalculator>().To<PathCalculator>().AsSingle();
            Container.Bind<IMovement>().To<Mover>().AsSingle();
            
            // MonoBehaviours
            Container.Bind<IUIMessageManager>().To<UIMessageManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IDiceManager>().To<DiceManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<ITurnVisualizer>().To<TurnVisualizer>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IGameInputHandler>().To<GameInputHandler>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IPlayerUIFactory>().To<PlayerUIFactory>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IPawnFactory>().To<PawnFactory>().FromComponentInHierarchy().AsSingle();
            
            
            
            
        }
    }
}