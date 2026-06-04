using System.Collections.Generic;
using BoardAdventures.Abstractions;
using Photon.Realtime;
using Signals;
using UI.Menu;
using Zenject;

namespace BoardAdventures.Network
{
    public class LobbyService : ILobbyService
    {
        private SignalBus _signalBus;
        private INetworkService _networkService;
        private LobbyState _currentState = LobbyState.None;

        public List<Player> Players => _networkService.GetPlayers();

        
        
        [Inject]
        public void Initialize(SignalBus signalBus, INetworkService networkService)
        {
            _signalBus = signalBus;
            _networkService = networkService;

            _signalBus.Subscribe<OnLobbyStateChangedSignal>(EvaluateLobbyState);
        }

        public bool IsPlayerReady(Player player) =>
            _networkService.GetPlayerProp<bool>(player, NetworkKeys.ReadyToPlayKey);

        private void EvaluateLobbyState()
        {
            var players = _networkService.GetPlayers();
            var maxPlayers = _networkService.MaxPlayers;
            var newState = CalculateState(players, maxPlayers);

            _currentState = newState;
            
            _signalBus.Fire(new OnLobbyStateUiChangedSignal
            {
                State = _currentState,
                IsMaster = _networkService.IsMasterClient,
            });
        }

        private LobbyState CalculateState(List<Player> players, byte maxPlayers)
        {

            if (players.Count < maxPlayers)
                return LobbyState.WaitingForPlayers;

            if (!_networkService.CheckAllPlayersReady())
                return LobbyState.WaitingForReady;

            if (!_networkService.IsMasterClient)
                return LobbyState.WaitingForHost;

            return LobbyState.ReadyToStart;
        }

        public void ToggleReady()
        {
            var isReady = IsPlayerReady(null);
            _networkService.SetPlayerReady(NetworkKeys.ReadyToPlayKey, !isReady);
        }

        public void StartMatch(string levelName)
        {
            if (_networkService.IsMasterClient)
                _networkService.LoadLevel(levelName);
        }
    }
}