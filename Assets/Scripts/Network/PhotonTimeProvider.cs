using Photon.Pun;

namespace BoardAdventures.Network
{
    public class PhotonTimeProvider: INetworkTime
    {
        public double GetCurrentTime() => PhotonNetwork.Time;
    }
}