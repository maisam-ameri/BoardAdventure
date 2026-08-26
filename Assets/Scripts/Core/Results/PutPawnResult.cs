namespace BoardAdventures.Core.Results
{
    public class PutPawnResult
    {
        public byte PawnId { get;  }
        public byte? NodeId { get; set; }
        public PutFailReason FailReason { get; }

        public PutPawnResult(byte pawnId,  PutFailReason failReason, byte? nodeId = null)
        {
            PawnId = pawnId;
            NodeId = nodeId;
            FailReason = failReason;
        }
    }
}