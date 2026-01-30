using System;
using System.Collections.Generic;
using BoardAdventures.Abstractions;
using BoardAdventures.Core.Players;
using Signals;
using Zenject;

namespace BoardAdventures.Core.GameLogic
{
    public class TurnFlowService : ITurnFlowService, ICurrentPlayerProvider
    {
        public Player CurrentPlayer => _players[_currentPlayerIndex];
        public Player LastPlayer => _lastPlayer;

        private Player _lastPlayer;
        private int _currentPlayerIndex;
        private List<Player> _players;


        public TurnFlowService(SignalBus signalBus)
        {
            signalBus.Subscribe<OnPlayersCreatedSignal>(HandlePlayersCreated);
        }

        private void HandlePlayersCreated(OnPlayersCreatedSignal signal)
        {
            if (_players != null)
                throw new InvalidOperationException("TurnFlowService already initialized.");

            _players = signal.Players ?? throw new ArgumentNullException(nameof(signal.Players));
            _currentPlayerIndex = 0;
        }

        public void NextPlayer()
        {
            _lastPlayer = CurrentPlayer;
            _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;
        }
    }
}