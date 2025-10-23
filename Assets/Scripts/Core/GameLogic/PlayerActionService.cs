using System.Linq;
using System.Threading.Tasks;
using BoardAdventures.GameObjects.Pawns.Abstractions;
using BoardAdventures.Managers;
using BoardAdventures.UI.Common;

namespace BoardAdventures.Core.GameLogic
{
    public class PlayerActionService
    {
        private readonly DiceManager _diceManager;
        private readonly UIMessageManager _uiMessageManager;
        private readonly GameFlowService _gameFlowService;
        private readonly PawnManager _pawnManager;
        private readonly PawnMovementService _pawnMovementService;


        public PlayerActionService(DiceManager diceManager, UIMessageManager uiMessageManager,
            GameFlowService gameFlowService, PawnManager pawnManager, PawnMovementService pawnMovementService)
        {
            _diceManager = diceManager;
            _uiMessageManager = uiMessageManager;
            _gameFlowService = gameFlowService;
            _pawnManager = pawnManager;
            _pawnMovementService = pawnMovementService;
        }

        public void HandelPawnSelected(IPawn pawn)
        {
            if (!_diceManager.IsRolled)
            {
                _uiMessageManager.ShowRollMessage();
                return;
            }

            var faction = pawn.Faction;

            if (pawn.State == PawnState.InBase && _diceManager.Step == 6)
            {
                if (_gameFlowService.CurrentPlayer.Factions.All(f => f != faction)) return;

                if (!faction.StartNode.IsEmpty)
                {
                    _uiMessageManager.ShowStartNodeMessage();
                    return;
                }

                _pawnManager.EnterPawnToGame(pawn, null, _gameFlowService.HandleActionCompleted);
            }
            else if (pawn.State == PawnState.InGame)
            {
                _ = HandleSelectedPawnAsync(pawn);
            }
        }

        private async Task HandleSelectedPawnAsync(IPawn pawn)
        {
            await _pawnMovementService.MovePawn(_gameFlowService.CurrentPlayer, pawn, _diceManager.Step,
                _gameFlowService.HandleActionStarted, _gameFlowService.HandleActionCompleted);
        }
    }
}