using System.Collections.Generic;
using BoardAdventures.Abstractions;
using BoardAdventures.Core.Players;
using Signals;
using Zenject;

namespace BoardAdventures.Core.GameLogic
{
    public class TurnManagementService : ITurnManagementService
    {
        private IDiceManager _diceManager;
        private IPawnManager _pawnManager;
        private List<Player> _players;
        private int _currentPlayerIndex;
        private bool _firstSix;
        private bool _isDiceRolled;
        private IGameFlowService _gameFlowService;
        private SignalBus _signalBus;


        [Inject]
        public void Initialize(IGameFlowService gameFlowService
            , IDiceManager diceManager, IPawnManager pawnManager
            , SignalBus signalBus)
        {
            _signalBus = signalBus;
            _gameFlowService = gameFlowService;
            _diceManager = diceManager;
            _pawnManager = pawnManager;

            _signalBus.Subscribe<OnRewardGrantedSignal>(HandleRewardGranted);
            _signalBus.Subscribe<OnTurnTimerExpiredSignal>(OnTurnTimerExpired);
            _signalBus.Subscribe<OnCapturedSignal>(HandleCapturePawn);
        }

        private void HandleCapturePawn(OnCapturedSignal signal)
        {
            _pawnManager.ReturnPawnToBase(signal.Pawn);
        }

        private void HandleRewardGranted()
        {
            _diceManager.SetActivateDice(true);
        }

        private void SwitchTurn()
        {
            _gameFlowService.SwitchTurn();
        }

        private void OnTurnTimerExpired()
        {
            SwitchTurn();
        }
    }
}