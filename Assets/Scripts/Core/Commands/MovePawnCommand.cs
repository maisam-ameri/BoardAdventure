namespace BoardAdventures.Core.Commands
{
    public sealed class MovePawnCommand: ICommand
    {
        public byte PawnId { get;  }

        public MovePawnCommand(byte pawnId)
        {
            PawnId = pawnId;
        }
    }
}