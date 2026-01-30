using System;
using System.Threading.Tasks;
using BoardAdventures.Core.Players;
using BoardAdventures.GameObjects.Pawns.Abstractions;

namespace BoardAdventures.Abstractions
{
    public interface IPawnMovementService
    {
        Task MovePawn(Player player, IPawn pawn, int? step);
    }
}