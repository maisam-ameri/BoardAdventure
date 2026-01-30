using System.Linq;
using BoardAdventures.Core.Players;
using BoardAdventures.GameObjects.Nodes.Abstractions;
using BoardAdventures.GameObjects.Pawns.Abstractions;

namespace BoardAdventures.Core.Path
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