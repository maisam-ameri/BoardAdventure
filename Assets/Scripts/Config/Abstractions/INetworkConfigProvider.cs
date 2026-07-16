using Photon.Realtime;

namespace BoardAdventures.Config
{
    public interface INetworkConfigProvider
    {
        AppSettings GetConfig();
    }
}