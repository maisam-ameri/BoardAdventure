namespace BoardAdventures.Core.Commands
{
    public class SelectPawnCommand: ICommand
    {
        public byte PawnId { get; }

        public SelectPawnCommand(byte pawnId)
        {
            PawnId = pawnId;
        }
    }
}