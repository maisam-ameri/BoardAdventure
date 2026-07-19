using BoardAdventures.Gameplay.GameLogic.Abstractions;

namespace BoardAdventures.Gameplay.GameLogic
{
    public enum TurnDecision
    {
        WaitForAction,
        SwitchTurn,
        RollReward
    }

    public class TurnLogicService: ITurnLogicService
    {
        public bool HasReward { get; set; }

        public TurnDecision ProcessRoll(int? step, bool canEnterPawn, bool canMovePawn)
        {
            if (step == 6)
            {
                HasReward = true;

                if (canEnterPawn || canMovePawn)
                    return TurnDecision.WaitForAction;

                return TurnDecision.RollReward;
            }

            HasReward = false;

            return canMovePawn ? TurnDecision.WaitForAction : TurnDecision.SwitchTurn;
        }
    }
}