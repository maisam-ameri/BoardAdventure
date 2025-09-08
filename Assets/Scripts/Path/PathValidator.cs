using System.Linq;
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

            if (targetNode.Pawn is null)
            {
                return targetNode.IsEmpty;
            }

            if (currentPlayer.Factions.Any(f => f == targetNode.Pawn.Faction))
            {
                return false;
            }

            Capture(targetNode.Pawn);
            return false;
        }

        private static void Capture(IPawn pawn)
        {
            Debug.LogWarning($"Capture {pawn.Color}");
            // TODO: Return the pawn to its base
            
            // var emptyBaseNode = GetEmptyNodeBase(pawn.Faction);
            //
            // if (emptyBaseNode is null)
            // {
            //     Debug.LogWarning("There is no empty node in the base");
            //     return;
            // }
            //
            // pawn.Position = emptyBaseNode.Position;
            // pawn.CurrentNode = emptyBaseNode;
            // emptyBaseNode.Pawn = pawn;
            // emptyBaseNode.IsEmpty = false;
            // pawn.Collider.enabled = false;
            // pawn.State = "InBase";
        }
    }
}