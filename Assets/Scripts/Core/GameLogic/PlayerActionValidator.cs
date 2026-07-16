using System.Collections.Generic;
using System.Linq;
using BoardAdventures.Board;
using BoardAdventures.Board.Nodes;
using BoardAdventures.Board.Pawns;
using BoardAdventures.Core.Path;
using BoardAdventures.Core.Players;

namespace BoardAdventures.Core.GameLogic
{
    public class PlayerActionValidator
    {
        public bool CheckToMovePawn(Player player, int? step)
        {
            return player.Factions
                .SelectMany(GetPawnsFromGame)
                .Any(pawn =>
                {
                    var path = CheckPathIsValid(player, pawn, step);
                    return path is not null && path.Count > 0;
                });
        }

        public List<INode> CheckPathIsValid(Player player, IPawn pawn, int? step)
        {
            if (player.Factions.All(f => f != pawn.Faction)) return null;

            var path = PathCalculator.DefinePath(step, pawn);

            return path.Count != 0 && PathValidator.CanMoveToNode(path[^1], player) ? path : null;
        }

        public bool CheckToEnterPawn(Player player, int? step)
        {
            if(step is not 6) return false;
            
            foreach (var faction in player.Factions)
            {
                var isExistPawnInBase = faction.Pawns.Any(p => p.LocationState == PawnLocationState.InBase);
                var isStartNodeEmpty = faction.StartNode.IsEmpty;

                if (isExistPawnInBase && isStartNodeEmpty)
                    return true;
            }

            return false;
        }

        private IEnumerable<IPawn> GetPawnsFromGame(Faction faction)
            => faction.Pawns.Where(p => p.LocationState == PawnLocationState.InGame);
    }
}