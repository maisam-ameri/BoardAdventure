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
        private bool _firstSix;

        public GameFlowService(UIMessageManager uiMessageManager
            , TurnFlowService turnFlowService
            , DiceManager diceManager
            , TurnVisualizer turnVisualizer)
        {
            _uiMessageManager = uiMessageManager;
            _turnFlowService = turnFlowService;
            _diceManager = diceManager;
            _turnVisualizer = turnVisualizer;
            
            _turnFlowService.OnTurnSwitched += HandleSwitchTurn;
            _turnFlowService.OnTurnStarted += HandleTurnStarted;
            _diceManager.OnFirstSixRolled += HandleFirstSixVisual;
            
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
            if (_firstSix) return;

            _firstSix = true;
            _turnVisualizer.UpdatePawnHighlights(CurrentPlayer, LastPlayer);
            _turnVisualizer.UpdatePlayerPanels(CurrentPlayer, LastPlayer);
        }

        public void GrantReward(Player player)
        {
            _uiMessageManager.ShowRewardMessage(player.Name);
            OnRewardGranted?.Invoke(player);
        }

    }
}