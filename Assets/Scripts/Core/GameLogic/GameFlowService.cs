using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BoardAdventures.Abstractions;
using BoardAdventures.Core.Players;
using BoardAdventures.Managers;
using BoardAdventures.UI.Common;
using Signals;
using Zenject;

namespace BoardAdventures.Core.GameLogic
{
    public class GameFlowService: IGameFlowService
    {
        //public event Action OnRewardGranted;
        public Player CurrentPlayer => _turnFlowService.CurrentPlayer;
        private Player LastPlayer => _turnFlowService.LastPlayer;
        public List<Player> _players;

        private readonly IUIMessageManager _uiMessageManager;
        private readonly ITurnFlowService _turnFlowService;
        private readonly IDiceManager _diceManager;
        private readonly ITurnVisualizer _turnVisualizer;
        private readonly IGameInputHandler _gameInputHandler;
        private readonly IPlayerActionValidator _playerActionValidator;
        private readonly ITurnLogicService _turnLogicService;
        private bool _firstSixRolled;
        private readonly SignalBus _signalBus;
        private readonly IPlayerSetupService _playerSetupService;

        public GameFlowService(IUIMessageManager uiMessageManager
            , ITurnFlowService turnFlowService
            , IDiceManager diceManager
            , ITurnVisualizer turnVisualizer
            , IGameInputHandler gameInputHandler
            , IPlayerActionValidator playerActionValidator
            , ITurnLogicService turnLogicService
            ,IPlayerSetupService playerSetupService
            , SignalBus signalBus)
        {
            _uiMessageManager = uiMessageManager;
            _turnFlowService = turnFlowService;
            _diceManager = diceManager;
            _turnVisualizer = turnVisualizer;
            _gameInputHandler = gameInputHandler;
            _playerActionValidator = playerActionValidator;
            _turnLogicService = turnLogicService;
            _playerSetupService = playerSetupService;
            _signalBus = signalBus;

            _turnFlowService.OnTurnSwitched += HandleSwitchTurn;
            _turnFlowService.OnTurnStarted += HandleTurnStarted;
            _diceManager.OnFirstSixRolled += HandleFirstSixVisual;
            _diceManager.OnDiceRolled += HandleDiceRolled;
            _gameInputHandler.OnDiceRollRequested += OnDiceButtonClicked;
        }

        public void StartGame()
        {
            _playerSetupService.Setup();
            _players = _playerSetupService.Players;
            _turnVisualizer.Initial(_players);
            _turnVisualizer.DeactivateTurnVisuals();
            _turnVisualizer.DeactivatePlayerVisuals();
            _turnVisualizer.UpdatePlayerPanels(CurrentPlayer, null);
        }
        
        private void OnDiceButtonClicked()
        {
            _diceManager.RollDice();
        }

        private void StartTurn()
        {
            _uiMessageManager.ShowPlayerTurnMessage(CurrentPlayer.Name);
            _turnFlowService.StartTurn();
        }

        private void HandleTurnStarted(Player currentPlayer, Player lastPlayer)
        {
            if (_firstSixRolled)
                _turnVisualizer.UpdatePawnHighlights(CurrentPlayer, lastPlayer);

            _turnVisualizer.UpdatePlayerPanels(CurrentPlayer, lastPlayer);
            CurrentPlayer.UI.StartTurnTimer(10);
        }

        public void SwitchTurn()
        {
            _turnFlowService.SwitchTurn();
        }

        private async void HandleSwitchTurn()
        {
            await Task.Delay(1000);
            _diceManager.Reset();
            _diceManager.SetActivateDice(true);
            StartTurn();
        }

        private void HandleFirstSixVisual()
        {
            _firstSixRolled = true;
            _turnVisualizer.UpdatePawnHighlights(CurrentPlayer, LastPlayer);
        }

        public void GrantReward(Player player)
        {
            _uiMessageManager.ShowRewardMessage(player.Name);
            _signalBus.Fire(new OnRewardGrantedSignal());
            //OnRewardGranted?.Invoke();
        }


        private void HandleDiceRolled(int? step)
        {
            if (step is null) return;

            _diceManager.IsRolled = true;
            _diceManager.SetActivateDice(false);
            CurrentPlayer.UI.StopTimer();
            CurrentPlayer.UI.StartTurnTimer(10);


            var canEnter = _playerActionValidator.CheckToEnterPawn(CurrentPlayer, step);
            var canMove = _playerActionValidator.CheckToMovePawn(CurrentPlayer, step);

            var decision = _turnLogicService.ProcessRoll(step, canEnter, canMove);

            switch (decision)
            {
                case TurnDecision.WaitForAction:
                    _uiMessageManager.ShowActionAvailableMessage(CurrentPlayer.Name, step.Value);
                    CurrentPlayer.UI.StartTurnTimer(10);
                    break;
                case TurnDecision.RollReward:
                    _uiMessageManager.ShowRewardMessage(CurrentPlayer.Name);
                    _diceManager.SetActivateDice(true);
                    break;

                case TurnDecision.SwitchTurn:
                    SwitchTurn();
                    break;
            }
        }

        public void HandleActionCompleted()
        {
            _diceManager.Reset();

            if (_turnLogicService.HasReward)
            {
                GrantReward(CurrentPlayer);
            }
            else
            {
                SwitchTurn();
            }
        }

        public void HandleActionStarted()
        {
            CurrentPlayer.UI.PauseTimer();
        }
    }
}