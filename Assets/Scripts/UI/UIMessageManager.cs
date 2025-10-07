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
        //public void ShowPlayerTurnMessage(string playerName) => ShowMessage($"The turn is {playerName}");
        
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