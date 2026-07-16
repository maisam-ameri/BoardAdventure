namespace BoardAdventures.Core.GameLogic
{
    public interface ITurnLogicService
    {
        bool HasReward { get; set; }
        TurnDecision ProcessRoll(int? step, bool canEnterPawn, bool canMovePawn);
    }
}