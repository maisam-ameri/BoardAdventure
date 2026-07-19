namespace BoardAdventures.Core.Results
{
    public enum MoveFailReason: byte
    {
        None,
        PawnNotFound,
        PawnDoesNotBelongToPlayer,
        NotPlayersTurn,
        InvalidPath,
        ContinueValidate,

    }
}