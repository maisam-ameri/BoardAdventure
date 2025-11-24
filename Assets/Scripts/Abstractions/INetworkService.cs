namespace BoardAdventures.Abstractions
{
    public interface INetworkService
    {
        void Connect();
        void JoinToRoom(byte maxPlayer);
        void SetPlayerReady(bool isReady);
    }
}