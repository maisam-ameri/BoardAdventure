using System;
using System.Collections.Generic;
using BoardAdventures.Abstractions;
using BoardAdventures.Core.Players;
using Signals;
using Zenject;

namespace BoardAdventures.Core.GameLogic
{
    public class TurnFlowService: ITurnFlowService
    {
        public Player CurrentPlayer => _players[_currentPlayerIndex];
        public Player LastPlayer => _lastPlayer;
        public event Action OnTurnSwitched;
        public event Action<Player, Player> OnTurnStarted;

        private Player _lastPlayer;
        private int _currentPlayerIndex;
        private List<Player> _players;
        private SignalBus _signalBus;


        public TurnFlowService(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<OnPlayersCreatedSignal>(HandlePlayersCreated);
        }

        private void HandlePlayersCreated(OnPlayersCreatedSignal signal)
        {
            if (_players != null)
                throw new InvalidOperationException("TurnFlowService already initialized.");

            _players = signal.Players ?? throw new ArgumentNullException(nameof(signal.Players));
            _currentPlayerIndex = 0;
        }

        public void StartTurn()
        {
            OnTurnStarted?.Invoke(CurrentPlayer, _lastPlayer);
        }
        
        public void SwitchTurn()
        {
            CurrentPlayer.UI.StopTimer();
            NextPlayer();

            OnTurnSwitched?.Invoke();
        }

        private void NextPlayer()
        {
            _lastPlayer = CurrentPlayer;
            _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;
        }
    }
}