using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoardAdventures.Abstractions;
using BoardAdventures.Core.Players;
using BoardAdventures.Network;
using Signals;
using UnityEngine;
using Zenject;

namespace BoardAdventures.Core.GameLogic
{
    public class MatchFlowService : IMatchFlowService
    {
        public Player ActivePlayer => _turnFlowService.ActivePlayer;
        private Player LastPlayer => _turnFlowService.PreviousPlayer;

        private readonly IUIMessageManager _uiMessageManager;
        private readonly ITurnFlowService _turnFlowService;
        private readonly IDiceManager _diceManager;
        private readonly ITurnVisualizer _turnVisualizer;
        private readonly IPlayerActionValidator _playerActionValidator;
        private readonly ITurnLogicService _turnLogicService;
        private readonly SignalBus _signalBus;
        private readonly IPlayerSetupService _playerSetupService;
        private readonly INetworkService _networkService;
        
        private float _turnDuration;
        private bool _isTurnBeginning = true;

        public MatchFlowService(IUIMessageManager uiMessageManager
            , ITurnFlowService turnFlowService
            , IDiceManager diceManager
            , ITurnVisualizer turnVisualizer
            , IPlayerActionValidator playerActionValidator
            , ITurnLogicService turnLogicService
            , IPlayerSetupService playerSetupService
            , INetworkService networkService
            , SignalBus signalBus)
        {
            _uiMessageManager = uiMessageManager;
            _turnFlowService = turnFlowService;
            _diceManager = diceManager;
            _turnVisualizer = turnVisualizer;
            _playerActionValidator = playerActionValidator;
            _turnLogicService = turnLogicService;
            _playerSetupService = playerSetupService;
            _networkService = networkService;
            _signalBus = signalBus;

            _signalBus.Subscribe<OnTurnTimerExpiredSignal>(HandleTurnExpired);
            _signalBus.Subscribe<OnTurnEndTimeChangedSignal>(SwitchTurn);
            _signalBus.Subscribe<OnDiceRolledSignal>(HandleDiceRolled);
            _signalBus.Subscribe<OnDiceRollRequestedSignal>(OnDiceButtonClicked);
            _signalBus.Subscribe<OnPlayerActionStartedSignal>(HandlePlayerActionStarted);
            _signalBus.Subscribe<OnPlayerActionCompletedSignal>(HandlePlayerActionCompleted);
            _signalBus.Subscribe<OnGameOverSignal>(HandleEndMatch);
            _signalBus.Subscribe<OnStartMatchSignal>(HandleStartMatch);
        }

        private void HandleStartMatch(OnStartMatchSignal matchSignal)
        {
            var playerNetModels = _networkService.GetPlayers();
            var playerGameModels = MapPlayersNetModel(playerNetModels);
            _playerSetupService.Setup(playerGameModels);

            var players = _playerSetupService.Players;
            _turnVisualizer.Initial(players);
            _turnVisualizer.DeactivateTurnVisuals();
            _turnVisualizer.DeactivatePlayerVisuals();
            _turnVisualizer.UpdatePlayerPanels(ActivePlayer, null);
            _turnVisualizer.UpdatePawnHighlights(ActivePlayer, null);
            _diceManager.Reset();
            _turnDuration = matchSignal.TurnDuration;

            HandleTurnExpired(null);
        }

        private List<Player> MapPlayersNetModel(List<Photon.Realtime.Player> players) =>
            players.Select(p => new Core.Players.Player
            {
                Id = p.ActorNumber,
                Nickname = p.NickName
            }).ToList();

        private void HandleEndMatch(OnGameOverSignal signal)
        {
            ActivePlayer.UI.StopTimer();
            Debug.Log($"{signal.Winner.Nickname} won");
        }

        private void OnDiceButtonClicked()
        {
            _diceManager.RollDice();
        }

        private void StartNewTurn()
        {
            HandleStartTurn();

            _isTurnBeginning = false;
        }


        private void HandleStartTurn()
        {
            _uiMessageManager.ShowPlayerTurnMessage(ActivePlayer.Nickname);
            _turnVisualizer.UpdatePlayerPanels(ActivePlayer, LastPlayer);
            _turnVisualizer.UpdatePawnHighlights(ActivePlayer, LastPlayer);
            ActivePlayer.UI.StartTurnTimer(_turnDuration);
            _diceManager.SetActivateDice(_turnFlowService.IsActivePlayerTurn());

        }

        private void HandleTurnExpired(OnTurnTimerExpiredSignal signal)
        {
            if (_networkService.IsMasterClient)
                _networkService.SetPlayerReady(NetworkKeys.TurnEndTime, signal?.TurnEndTime ?? 0);
        }

        public async void SwitchTurn()
        {
            if (_isTurnBeginning)
            {
                StartNewTurn();
                return;
            }

            ActivePlayer.UI.PauseTimer();
            await Task.Delay(1000);
            _diceManager.Reset();
            
            // if(_turnFlowService.IsActivePlayerTurn())
            //     _diceManager.SetActivateDice(true);
            
            ActivePlayer.UI.StopTimer();
            _turnFlowService.NextPlayer();

            HandleStartTurn();
        }

        public void GrantReward(Player player)
        {
            _uiMessageManager.ShowRewardMessage(player.Nickname);
            
            if(_turnFlowService.IsActivePlayerTurn())
                _diceManager.SetActivateDice(true);
            
            ActivePlayer.UI.StartTurnTimer(_turnDuration);
        }

        private void HandleDiceRolled(OnDiceRolledSignal signal)
        {
            var step = signal.Step;

            if (step is null) return;

            _diceManager.IsRolled = true;
            _diceManager.SetActivateDice(false);


            var canEnter = _playerActionValidator.CheckToEnterPawn(ActivePlayer, step);
            var canMove = _playerActionValidator.CheckToMovePawn(ActivePlayer, step);

            var decision = _turnLogicService.ProcessRoll(step, canEnter, canMove);

            switch (decision)
            {
                case TurnDecision.WaitForAction:
                    _uiMessageManager.ShowActionAvailableMessage(ActivePlayer.Nickname, step.Value);
                    ActivePlayer.UI.StartTurnTimer(_turnDuration);
                    break;
                case TurnDecision.RollReward:
                    _uiMessageManager.ShowRewardMessage(ActivePlayer.Nickname);
                    _diceManager.SetActivateDice(true);
                    break;

                case TurnDecision.SwitchTurn:
                    SwitchTurn();
                    break;
            }
        }

        public void HandlePlayerActionCompleted()
        {
            _diceManager.Reset();

            if (_turnLogicService.HasReward)
            {
                GrantReward(ActivePlayer);
            }
            else
            {
                SwitchTurn();
            }
        }

        public void HandlePlayerActionStarted()
        {
            ActivePlayer.UI.PauseTimer();
        }
    }
}