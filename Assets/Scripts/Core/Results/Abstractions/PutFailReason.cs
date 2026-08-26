namespace BoardAdventures.Core.Results
{
    public enum PutFailReason
    {
        None,
        PawnNotFound,
        PawnDoesNotBelongToPlayer,
        NotPlayersTurn,
        StartNodeOccupied
    }
}