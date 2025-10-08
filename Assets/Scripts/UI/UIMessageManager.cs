using UnityEngine;

namespace UI
{
    public class UIMessageManager: MonoBehaviour
    {
        [SerializeField] private UIMessage uiMessage;
        [SerializeField] private Transform parent;

        public void ShowRollMessage() => ShowMessage("Please roll the dice");
        public void ShowStartNodeMessage() => ShowMessage("The start node isn't empty");
        public void ShowEnterPawnMessage(string playerName) => ShowMessage($"{playerName} can enter a pawn");
        public void ShowMovePawnMessage(string playerName) => ShowMessage($"{playerName} can move a pawn");
        public void ShowActionAvailableMessage(string playerName, int step) =>
            ShowMessage($"{playerName} rolled {step}! Choose a pawn to move or enter.");

        public void ShowRewardMessage(string playerName) =>
            ShowMessage($"{playerName} got 6! Roll again!");
        
        private void ShowMessage(string message, float duration = 1f)
        {
            var uiMsg = GetUiMessage();
            uiMsg.ShowMessage(message, duration);
        }

        private UIMessage GetUiMessage()
        {
            var uiMsg = Instantiate(uiMessage, parent);
            uiMsg.name = "uiMessage";
            return  uiMsg;
        }
    }
}