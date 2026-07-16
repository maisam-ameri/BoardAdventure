using BoardAdventures.Core.GameLogic;
using BoardAdventures.Core.Movement;
using BoardAdventures.Managers;
using BoardAdventures.Presentation;
using BoardAdventures.Presentation.Common;
using BoardAdventures.Presentation.Players;
using BoardAdventures.Signals;
using Zenject;

namespace BoardAdventures.Installers
{
    public class GameInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {

            // Signals
            Container.DeclareSignal<OnTurnTimerExpiredSignal>();
            Container.DeclareSignal<OnCapturedSignal>();
            Container.DeclareSignal<OnPlayersCreatedSignal>();
            Container.DeclareSignal<OnSelectedPawnSignal>();
            Container.DeclareSignal<OnDiceRollRequestedSignal>();
   
            Container.DeclareSignal<OnPlayerActionStartedSignal>();
            Container.DeclareSignal<OnPawnMoveCompletedSignal>();
            Container.DeclareSignal<OnPlayerActionCompletedSignal>();
            Container.DeclareSignal<OnGameOverSignal>();
            Container.DeclareSignal<OnStartMatchSignal>();
            
            // Services
            Container.Bind<IMatchFlowService>().To<MatchFlowService>().AsSingle();
            Container.Bind<IGameRulesService>().To<GameRulesService>().AsSingle().NonLazy();
            Container.BindInterfacesTo<TurnFlowService>().AsSingle();
            Container.Bind<ITurnLogicService>().To<TurnLogicService>().AsSingle();
            Container.Bind<PlayerActionValidator>().AsSingle();
            Container.Bind<PlayerActionService>().AsSingle().NonLazy();
            Container.Bind<IPlayerSetupService>().To<PlayerSetupService>().AsSingle();
            Container.Bind<PawnManager>().AsSingle();
            Container.Bind<PawnStateService>().AsSingle();
            Container.Bind<IPawnMovementService>().To<PawnMovementService>().AsSingle();
            Container.Bind<IMovement>().To<Mover>().AsSingle();
            
            // MonoBehaviours
            Container.Bind<IUIMessageManager>().To<UIMessageManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<DiceManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<ITurnVisualizer>().To<TurnVisualizer>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IGameInputHandler>().To<GameInputHandler>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IPlayerUIFactory>().To<PlayerUIFactory>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IPawnFactory>().To<PawnFactory>().FromComponentInHierarchy().AsSingle();
            Container.Bind<MatchManager>().FromComponentInHierarchy().AsSingle();
            
            
        }
    }
}