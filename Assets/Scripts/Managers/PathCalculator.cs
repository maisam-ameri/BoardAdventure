using System.Collections.Generic;
using System.Linq;
using Nodes;
using Nodes.Abstractions;
using Pawns;

namespace Managers
{
    public class PathCalculator
    {
        public List<INode> DefinePath(int? step, IPawn pawn)
        {
            if (step == null) return null;

            var path = new List<INode>();
            var currentNode = pawn.CurrentNode;

            var remainedSteps = step.Value;

            while (remainedSteps > 0)
            {
                if (currentNode is Gateway gateway)
                {
                    if (pawn.Faction.GatewayNode == gateway)
                    {
                        currentNode = pawn.Faction.GoalNodes[0];
                        path.Add(currentNode);
                        remainedSteps--;
                    }
                    else
                    {
                        currentNode = gateway.NextNode?.NextNode;
                        if (currentNode == null) break;
                        path.Add(currentNode);
                        remainedSteps--;
                    }
                }
                else if (currentNode is Goal)
                {
                    var currentIndex = pawn.Faction.GoalNodes.IndexOf((Node) currentNode);
                    var remainedStepsInGoalArea = (pawn.Faction.GoalNodes.Count - 1) - currentIndex;
                    if (remainedSteps > remainedStepsInGoalArea)
                        break;

                    currentNode = currentNode.NextNode ?? currentNode;
                    path.Add(currentNode);
                    remainedSteps--;
                }
                else
                {
                    currentNode = currentNode.NextNode;

                    if (currentNode == null) break;

                    path.Add(currentNode);
                    remainedSteps--;
                }
            }

            return path.Distinct().ToList();

        }
        
        
    }
}