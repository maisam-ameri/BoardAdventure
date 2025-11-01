using BoardAdventures.Abstractions;
using UnityEngine;

namespace BoardAdventures.UI.Common
{
    public class UIMessageManager : MonoBehaviour, IUIMessageManager
    {
        [SerializeField] private UIMessage uiMessage;
        [SerializeField] private Transform parent;

        public void ShowRollMessage() => ShowMessage("Please roll the dice");
        
        public void ShowStartNodeMessage() => ShowMessage("The start node isn't empty");
        
        public void ShowPlayerTurnMessage(string playerName) =>
            ShowMessage($"the turn is {playerName}");
        
        public void ShowEndTurnMessage(string playerName)=>
            ShowMessage($"the turn of {playerName} Ended");
        
        public void ShowActionAvailableMessage(string playerName, int step) =>
            ShowMessage($"{playerName} rolled {step}! Choose a pawn to move or enter.");
        
        public void ShowRewardMessage(string playerName) =>
            ShowMessage($"{playerName} got 6! Roll again!");

        public void ShowAvoidToMovement(string playerName)=>
            ShowMessage($"{playerName} can't move!");

        private void ShowMessage(string message, float duration = 1f)
        {
            var uiMsg = GetUiMessage();
            uiMsg.ShowMessage(message, duration);
        }

        private UIMessage GetUiMessage()
        {
            var uiMsg = Instantiate(uiMessage, parent);
            uiMsg.name = "uiMessage";
            return uiMsg;
        }


    }
}