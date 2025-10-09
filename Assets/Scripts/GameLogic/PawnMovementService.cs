using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions;
using JetBrains.Annotations;
using Nodes.Abstractions;
using Path;
using Pawns;
using Players;
using UI;

namespace GameLogic
{
    public class PawnMovementService
    {
        private readonly PlayerActionValidator _actionValidator;
        private readonly IMovement _movement;
        private readonly UIMessageManager _uiMessageManager;


        public PawnMovementService(PlayerActionValidator actionValidator, IMovement movement,
            UIMessageManager uiMessageManager)
        {
            _actionValidator = actionValidator;
            _movement = movement;
            _uiMessageManager = uiMessageManager;
        }

        public async Task MovePawn(Player player, IPawn pawn, int? step, Action onActionCompleted)
        {
            if (step == null)
            {
                _uiMessageManager.ShowAvoidToMovement(player.Name);
                return;
            }

            var path = ValidatePath(player, pawn, step.Value);

            if (path == null)
            {
                _uiMessageManager.ShowAvoidToMovement(player.Name);
                return;
            }

            await _movement.Move(pawn, path, onActionCompleted);
        }

        private List<INode> ValidatePath(Player player, IPawn pawn, int step)
        {
            var path = _actionValidator.CheckPathIsValid(player, pawn, step);
            return path;
        }
    }
}