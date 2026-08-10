namespace BoardAdventures.Core.Commands
{
    public class RollDiceCommand: ICommand
    {
        public string PlayerId { get; }
        
        
        public RollDiceCommand(string playerId)
        {
            PlayerId = playerId;
        }
    }
}