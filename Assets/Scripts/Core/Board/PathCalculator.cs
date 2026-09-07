using System.Collections.Generic;
using System.Linq;
using BoardAdventures.Core.State;
using UnityEngine;

namespace BoardAdventures.Core.Board
{
    public class PathCalculator: IPathCalculator
    {
        private readonly IBoardDefinition _boardDefinition;


        public PathCalculator(IBoardDefinition boardDefinition)
        {
            _boardDefinition = boardDefinition;
        }


        public IReadOnlyList<byte> Calculate(PawnState pawn, byte? step)
        {
            if (step == null) return null;

            var currentNode = _boardDefinition.GetNodeById(pawn.NodeId);
            var remainedSteps = step;
            var path = new List<INode>();

            while (remainedSteps > 0)
            {
                if (currentNode.NodeType == NodeType.Gate)
                {
                    if (pawn.FactionType == currentNode.FactionType)
                    {
                        var goalNodes = _boardDefinition.GetGoalNodes(pawn.FactionType);

                        if (goalNodes is null || remainedSteps > goalNodes.Count)
                            return null;

                        foreach (var goal in goalNodes)
                        {
                            if (remainedSteps <= 0) break;
                            path.Add(goal);
                            remainedSteps--;
                        }
                    }
                    else
                    {
                        currentNode = _boardDefinition.GetNodeById(currentNode.NextNodeId);
                    }
                }
                else
                {
                    currentNode = _boardDefinition.GetNodeById(currentNode.NextNodeId);

                    if (currentNode == null) break;

                    path.Add(currentNode);
                    remainedSteps--;
                }
            }
            return path.Select(n => n.NodeId).ToList();
        }

    }
}