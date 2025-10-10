using System.Linq;
using Factions;
using GameLogic;
using Managers;
using Nodes.Abstractions;
using Pawns;
using Players;
using UnityEngine;

namespace Path
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

        // public static void Capture(IPawn pawn)
        // {
        //     PawnLifecycleService pawnService = new PawnLifecycleService(null);
        //     pawnService.ReturnPawnToBase(pawn);
        // }
        //
        // private static INode GetEmptyBaseNode(Faction faction)
        // {
        //     return faction.BaseNodes.FirstOrDefault(n => n.IsEmpty);
        // }
    }
}