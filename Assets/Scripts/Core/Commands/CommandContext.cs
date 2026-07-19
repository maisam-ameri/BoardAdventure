namespace BoardAdventures.Core.Commands
{
    public sealed class CommandContext
    {
        public string PlayerId { get; }

        public CommandContext(string playerId)
        {
            PlayerId = playerId;
        }
    }
}