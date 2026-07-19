using System.Linq;
using BoardAdventures.Gameplay.GameLogic.Abstractions;
using BoardAdventures.Signals;
using Zenject;

namespace BoardAdventures.Gameplay.GameLogic
{
    public class GameRulesService : IGameRulesService
    {
        private readonly SignalBus _signalBus;

        public GameRulesService(SignalBus signalBus)
        {
            _signalBus = signalBus;
            
            _signalBus.Subscribe<OnPawnMoveCompletedSignal>(HandleMovementCompleted);
        }

        private void HandleMovementCompleted(OnPawnMoveCompletedSignal signal)
        {
            var player = signal.Player;

            if (player.Factions
                .Select(faction => faction.GoalNodes
                    .Any(n => n.IsEmpty)).Any(result => !result))
            {
                _signalBus.Fire(new OnGameOverSignal{Winner = player});
            }
            else
            {
                _signalBus.Fire(new OnPlayerActionCompletedSignal());
            }
        }
    }
}