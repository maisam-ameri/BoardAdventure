using System.Linq;
using System.Threading.Tasks;
using BoardAdventures.Abstractions;
using BoardAdventures.GameObjects.Pawns.Abstractions;
using BoardAdventures.Managers;
using BoardAdventures.UI.Common;
using Signals;
using UnityEngine;
using Zenject;

namespace BoardAdventures.Core.GameLogic
{
    public class PlayerActionService: IPlayerActionService
    {
        private readonly IDiceManager _diceManager;
        private readonly IUIMessageManager _uiMessageManager;
        private readonly IGameFlowService _gameFlowService;
        private readonly IPawnManager _pawnManager;
        private readonly IPawnMovementService _pawnMovementService;


        public PlayerActionService(IDiceManager diceManager, IUIMessageManager uiMessageManager,
            IGameFlowService gameFlowService, IPawnManager pawnManager, IPawnMovementService pawnMovementService
            , SignalBus signalBus)
        {
            _diceManager = diceManager;
            _uiMessageManager = uiMessageManager;
            _gameFlowService = gameFlowService;
            _pawnManager = pawnManager;
            _pawnMovementService = pawnMovementService;
            signalBus.Subscribe<OnSelectedPawnSignal>(HandelPawnSelected);
        }

        private void HandelPawnSelected(OnSelectedPawnSignal signal)
        {
            Debug.Log(signal.Pawn.Color);
            var pawn = signal.Pawn;
            
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