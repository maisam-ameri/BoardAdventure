using Players;

namespace GameLogic
{
    public enum TurnDecision
    {
        WaitForAction,
        SwitchTurn,
        RollReward
    }

    public class TurnLogicService
    {
        public bool HasReward { get; set; }

        private TurnDecision ProcessRoll(int? step, Player currentPlayer, bool canEnterPawn, bool canMovePawn)
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