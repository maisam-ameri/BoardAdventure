using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BoardAdventures.Core.Players;
using BoardAdventures.Managers;
using BoardAdventures.UI.Common;

namespace BoardAdventures.Core.GameLogic
{
    public class GameFlowService
    {
        public event Action<Player> OnRewardGranted;
        public Player CurrentPlayer => _turnFlowService.CurrentPlayer;
        private Player LastPlayer => _turnFlowService.LastPlayer;

        private readonly UIMessageManager _uiMessageManager;
        private readonly TurnFlowService _turnFlowService;
        private readonly DiceManager _diceManager;
        private readonly TurnVisualizer _turnVisualizer;
        private readonly GameInputHandler _gameInputHandler;
        private readonly PlayerActionValidator _playerActionValidator;
        private readonly TurnLogicService _turnLogicService;
        private bool _firstSixRolled;

        public GameFlowService(UIMessageManager uiMessageManager
            , TurnFlowService turnFlowService
            , DiceManager diceManager
            , TurnVisualizer turnVisualizer
            , GameInputHandler gameInputHandler
            ,PlayerActionValidator playerActionValidator
            ,TurnLogicService turnLogicService)
        {
            _uiMessageManager = uiMessageManager;
            _turnFlowService = turnFlowService;
            _diceManager = diceManager;
            _turnVisualizer = turnVisualizer;
            _gameInputHandler = gameInputHandler;
            _playerActionValidator = playerActionValidator;
            _turnLogicService = turnLogicService;

            _turnFlowService.OnTurnSwitched += HandleSwitchTurn;
            _turnFlowService.OnTurnStarted += HandleTurnStarted;
            _diceManager.OnFirstSixRolled += HandleFirstSixVisual;
            _diceManager.OnDiceRolled += HandleDiceRolled;
            _gameInputHandler.OnDiceRollRequested += OnDiceButtonClicked;
        }

        private void OnDiceButtonClicked()
        {
            _diceManager.RollDice();
        }

        public void InitializePlayers(List<Player> players)
        {
            _turnFlowService.Initialize(players);
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
            OnRewardGranted?.Invoke(player);
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
    }
}