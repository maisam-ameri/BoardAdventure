using System.Collections.Generic;
using BoardAdventures.Core.Players;
using BoardAdventures.GameObjects.Nodes.Abstractions;
using BoardAdventures.GameObjects.Pawns.Abstractions;

namespace BoardAdventures.Abstractions
{
    public interface IPlayerActionValidator
    {
        bool CheckToMovePawn(Player player, int? step);
        List<INode> CheckPathIsValid(Player player, IPawn pawn, int? step);
        bool CheckToEnterPawn(Player player, int? step);
    }
}