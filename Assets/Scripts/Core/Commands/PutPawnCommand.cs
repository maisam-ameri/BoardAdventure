namespace BoardAdventures.Core.Commands
{
    public class PutPawnCommand : ICommand
    {
        public byte PawnId { get; }


        public PutPawnCommand( byte pawnId)
        {
            PawnId = pawnId;
        }
    }
}