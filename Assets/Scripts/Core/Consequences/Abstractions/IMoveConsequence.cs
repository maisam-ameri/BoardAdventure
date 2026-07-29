using BoardAdventures.Core.Results;
using BoardAdventures.Core.State;

namespace BoardAdventures.Core.Consequences
{
    public interface IMoveConsequence
    {
        public int Order { get; }
        void Execute(MatchState matchState, MovePawnResult movePawnResult);
    }
}