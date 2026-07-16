using System.Linq;
using BoardAdventures.Board.Nodes;
using BoardAdventures.Board.Pawns;
using Gameplay.Players;

namespace Gameplay.Path
{
    public static class PathValidator
    {
        public static bool CanMoveToNode(INode targetNode, Player currentPlayer)
        {
            if (targetNode == null) return false;

            return targetNode.Pawn is null || currentPlayer.Factions.All(f => f != targetNode.Pawn.Faction);
        }

        public static bool CheckNodeToCapture(IPawn pawn, INode targetNode)
        => targetNode.Pawn is not null && (targetNode.Pawn.Color != pawn.Color);

    }
}