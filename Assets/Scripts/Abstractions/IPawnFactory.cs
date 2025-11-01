using BoardAdventures.GameObjects.Pawns.Abstractions;
using Zenject;

namespace BoardAdventures.Abstractions
{
    public interface IPawnFactory
    {
        public IPawn Create();
    }
}