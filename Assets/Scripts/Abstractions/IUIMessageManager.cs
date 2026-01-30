namespace BoardAdventures.Abstractions
{
    public interface IUIMessageManager
    {
        void ShowRollMessage();
        void ShowStartNodeMessage();
        void ShowPlayerTurnMessage(string playerName);
        void ShowEndTurnMessage(string playerName);
        void ShowActionAvailableMessage(string playerName, int step);
        void ShowRewardMessage(string playerName);
        void ShowAvoidToMovement(string playerName);
    }
}