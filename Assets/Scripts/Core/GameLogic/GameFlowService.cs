using System;
using System.Threading.Tasks;
using BoardAdventures.Core.Players;
using BoardAdventures.UI.Common;

namespace BoardAdventures.Core.GameLogic
{
    public class GameFlowService
    {
        public event Action<Player> OnTurnStarted;
        public event Action<Player> OnTurnEnded;
        public event Action<Player> OnRewardGranted;

        private readonly UIMessageManager _uiMessageManager;

        public GameFlowService(UIMessageManager uiMessageManager)
        {
            _uiMessageManager = uiMessageManager;
        }

        public void StartTurn(Player player)
        {
            _uiMessageManager.ShowPlayerTurnMessage(player.Name);
            OnTurnStarted?.Invoke(player);
        }

        public void EndTurn(Player player)
        {
            _uiMessageManager.ShowEndTurnMessage(player.Name);
            OnTurnEnded?.Invoke(player);
        }

        public void GrantReward(Player player)
        {
            _uiMessageManager.ShowRewardMessage(player.Name);
            OnRewardGranted?.Invoke(player);
        }

        public async Task DelayBetweenTurns(int delay)
        {
            await Task.Delay(delay);
        }
    }
}