using BoardAdventures.Core.State;

namespace Tests
{
    public static class TestHelper
    {
        public static MatchState CreateMatchState()
        {
            var match = new MatchState();
            var board = new BoardState();
            var dice = new DiceState();
            var turn = new TurnState();

            match.BoardState = board;
            match.TurnState = turn;
            match.DiceState = dice;

            return match;
        }
    }
}