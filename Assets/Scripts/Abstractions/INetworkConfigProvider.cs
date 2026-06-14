using Photon.Realtime;

namespace BoardAdventures.Abstractions
{
    public interface INetworkConfigProvider
    {
        AppSettings GetConfig();
    }
}