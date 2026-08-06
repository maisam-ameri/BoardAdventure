using System.Collections.Generic;
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
            var pawns = new List<PawnState>();
            var matchInfo = new MatchInfo();

            match.BoardState = board;
            match.TurnState = turn;
            match.DiceState = dice;
            match.BoardState.PawnsState = pawns;
            match.MatchInfo = matchInfo;

            return match;
        }
    }
}