using System;
using System.Collections.Generic;
using BoardAdventures.Abstractions;
using BoardAdventures.Core.Players;
using Signals;
using Zenject;

namespace BoardAdventures.Core.GameLogic
{
    public class TurnFlowService : ITurnFlowService, IActivePlayerProvider
    {
        private readonly INetworkService _networkService;
        private readonly IActivePlayerProvider _activePlayerProvider;
        public Player ActivePlayer => _players[_activePlayerIndex];
        public Player PreviousPlayer => _previousPlayer;

        private Player _previousPlayer;
        private int _activePlayerIndex;
        private List<Player> _players;


        public TurnFlowService(SignalBus signalBus, INetworkService networkService)
        {
            _networkService = networkService;
            signalBus.Subscribe<OnPlayersCreatedSignal>(HandlePlayersCreated);
        }

        private void HandlePlayersCreated(OnPlayersCreatedSignal signal)
        {
            if (_players != null)
                throw new InvalidOperationException("TurnFlowService already initialized.");

            _players = signal.Players ?? throw new ArgumentNullException(nameof(signal.Players));
            _activePlayerIndex = 0;
        }

        public void NextPlayer()
        {
            _previousPlayer = ActivePlayer;
            _activePlayerIndex = (_activePlayerIndex + 1) % _players.Count;
        }

        public bool IsActivePlayerTurn()
        {
            return _networkService.LocalPlayer.ActorNumber == ActivePlayer.Id;
        }
    }
}