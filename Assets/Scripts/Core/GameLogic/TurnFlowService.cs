using System;
using System.Collections.Generic;
using BoardAdventures.Core.Players;

namespace BoardAdventures.Core.GameLogic
{
    public class TurnFlowService
    {
        public Player CurrentPlayer => _players[_currentPlayerIndex];
        public Player LastPlayer => _lastPlayer;
        public event Action OnTurnSwitched;
        public event Action<Player, Player> OnTurnStarted;

        private Player _lastPlayer;
        private int _currentPlayerIndex;
        private List<Player> _players;

        
        public TurnFlowService(){}

        public void Initialize(List<Player> players)
        {
            if (_players != null)
                throw new InvalidOperationException("TurnFlowService already initialized.");

            _players = players ?? throw new ArgumentNullException(nameof(players));
            _currentPlayerIndex = 0;
        }

        internal void StartTurn()
        {
            OnTurnStarted?.Invoke(CurrentPlayer, _lastPlayer);
        }
        
        internal void SwitchTurn()
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