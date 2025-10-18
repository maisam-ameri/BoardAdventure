using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BoardAdventures.Core.Players;
using BoardAdventures.UI.Common;

namespace BoardAdventures.Core.GameLogic
{
    public class TurnFlowService
    {
        public Player CurrentPlayer => _players[_currentPlayerIndex];
        public Player LastPlayer => _lastPlayer;

        private Player _lastPlayer;
        private int _currentPlayerIndex;
        private readonly TurnVisualizer _turnVisualizer;
        private List<Player> _players;
        public event Action<Player> OnTurnSwitched;

        public TurnFlowService(TurnVisualizer turnVisualizer)
        {
            _turnVisualizer = turnVisualizer;
        }

        public void Initialize(List<Player> players)
        {
            if (_players != null)
                throw new InvalidOperationException("TurnFlowService already initialized.");

            _players = players ?? throw new ArgumentNullException(nameof(players));
            _currentPlayerIndex = 0;
        }


        internal void StartTurn(Player current)
        {
            HandleTurnStarted(current);
        }

        private void HandleTurnStarted(Player player)
        {
            _turnVisualizer.UpdatePawnHighlights(player, _lastPlayer);
            _turnVisualizer.UpdatePlayerPanels(player, _lastPlayer);
            player.UI.StartTurnTimer(10);
        }

        internal void SwitchTurn()
        {
            CurrentPlayer.UI.StopTimer();
            NextPlayer();

            OnTurnSwitched?.Invoke(CurrentPlayer);
        }

        private void NextPlayer()
        {
            _lastPlayer = CurrentPlayer;
            _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;
        }
    }
}