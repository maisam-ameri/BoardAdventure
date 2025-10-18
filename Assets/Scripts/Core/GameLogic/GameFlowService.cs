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
            _diceManager.OnFirstSixRolled += HandleFirstSixVisual;
        }

        public void InitializePlayers(List<Player> players)
        {
            _turnFlowService.Initialize(players);
        }

        private void StartTurn(Player player)
        {
            _uiMessageManager.ShowPlayerTurnMessage(player.Name);
            _turnFlowService.StartTurn(player);
        }

        public void SwitchTurn()
        {
            _turnFlowService.SwitchTurn();
        }

        private async void HandleSwitchTurn(Player player)
        {
            await Task.Delay(1000);
            _diceManager.Reset();
            _diceManager.SetActivateDice(true);
            StartTurn(player);
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