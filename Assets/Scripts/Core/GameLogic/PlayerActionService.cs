using System.Linq;
using System.Threading.Tasks;
using BoardAdventures.Abstractions;
using BoardAdventures.GameObjects.Pawns.Abstractions;
using BoardAdventures.Managers;
using Signals;
using Zenject;

namespace BoardAdventures.Core.GameLogic
{
    public class PlayerActionService
    {
        private readonly DiceManager _diceManager;
        private readonly IUIMessageManager _uiMessageManager;
        private readonly IMatchFlowService _matchFlowService;
        private readonly PawnManager _pawnManager;
        private readonly IPawnMovementService _pawnMovementService;


        public PlayerActionService(DiceManager diceManager, IUIMessageManager uiMessageManager,
            IMatchFlowService matchFlowService, PawnManager pawnManager, IPawnMovementService pawnMovementService
            , SignalBus signalBus)
        {
            _diceManager = diceManager;
            _uiMessageManager = uiMessageManager;
            _matchFlowService = matchFlowService;
            _pawnManager = pawnManager;
            _pawnMovementService = pawnMovementService;
            signalBus.Subscribe<OnSelectedPawnSignal>(HandelPawnSelected);
        }

        private void HandelPawnSelected(OnSelectedPawnSignal signal)
        {
            var pawn = signal.Pawn;
            
            if (!_diceManager.IsRolled)
            {
                _uiMessageManager.ShowRollMessage();
                return;
            }

            var faction = pawn.Faction;

            if (pawn.LocationState == PawnLocationState.InBase && _diceManager.Step == 6)
            {
                if (_matchFlowService.ActivePlayer.Factions.All(f => f != faction)) return;

                if (!faction.StartNode.IsEmpty)
                {
                    _uiMessageManager.ShowStartNodeMessage();
                    return;
                }

                _pawnManager.EnterPawnToGame(pawn);
            }
            else if (pawn.LocationState == PawnLocationState.InGame)
            {
                _ = HandleSelectedPawnAsync(pawn);
            }
        }

        private async Task HandleSelectedPawnAsync(IPawn pawn)
        {
            await _pawnMovementService.MovePawn(_matchFlowService.ActivePlayer, pawn, _diceManager.Step);
        }
    }
}