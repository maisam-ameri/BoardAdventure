using BoardAdventures.Core.GameLogic;

namespace BoardAdventures.Abstractions
{
    public interface ITurnLogicService
    {
        bool HasReward { get; set; }
        TurnDecision ProcessRoll(int? step, bool canEnterPawn, bool canMovePawn);
    }
}