using System.Collections.Generic;
using System.Threading.Tasks;
using BoardAdventures.Board.Nodes;
using BoardAdventures.Board.Pawns;
using BoardAdventures.Gameplay.Movement;
using BoardAdventures.Gameplay.Players;
using BoardAdventures.Managers;
using BoardAdventures.Presentation;

namespace BoardAdventures.Gameplay.GameLogic
{
    public class PawnMovementService: IPawnMovementService
    {
        private readonly PlayerActionValidator _actionValidator;
        private readonly IMovement _movement;
        private readonly IUIMessageManager _uiMessageManager;


        public PawnMovementService(PlayerActionValidator actionValidator, IMovement movement,
            IUIMessageManager uiMessageManager)
        {
            _actionValidator = actionValidator;
            _movement = movement;
            _uiMessageManager = uiMessageManager;
        }

        public async Task MovePawn(Player player, IPawn pawn, int? step)
        {
            if (step == null)
            {
                _uiMessageManager.ShowAvoidToMovement(player.Nickname);
                return;
            }

            var path = ValidatePath(player, pawn, step.Value);

            if (path == null)
            {
                _uiMessageManager.ShowAvoidToMovement(player.Nickname);
                return;
            }

            //onActionStarted?.Invoke();
            await _movement.Move(pawn, path);
        }

        private List<INode> ValidatePath(Player player, IPawn pawn, int step)
        {
            var path = _actionValidator.CheckPathIsValid(player, pawn, step);
            return path;
        }
    }
}