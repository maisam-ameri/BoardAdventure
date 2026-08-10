namespace BoardAdventures.Core.Results
{
    public class RollDiceResult
    {
        public byte Step { get; }
        public RollFailReason FailReason { get; }


        public RollDiceResult(byte step, RollFailReason failReason)
        {
            Step = step;
            FailReason = failReason;
        }
    }
}