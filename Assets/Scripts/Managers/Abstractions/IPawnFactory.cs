using BoardAdventures.Board.Pawns;

namespace BoardAdventures.Managers
{
    public interface IPawnFactory
    {
        public IPawn Create();
    }
}