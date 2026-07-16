using System.Threading.Tasks;
using BoardAdventures.Board.Pawns;
using BoardAdventures.Core.Players;

namespace BoardAdventures.Managers
{
    public interface IPawnMovementService
    {
        Task MovePawn(Player player, IPawn pawn, int? step);
    }
}