namespace BoardAdventures.Gameplay.GameLogic.Abstractions
{
    public interface ITurnLogicService
    {
        bool HasReward { get; set; }
        TurnDecision ProcessRoll(int? step, bool canEnterPawn, bool canMovePawn);
    }
}