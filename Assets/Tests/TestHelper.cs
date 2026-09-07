using System.Collections.Generic;
using BoardAdventures.Core.Board;
using BoardAdventures.Core.State;

namespace Tests
{
    public static class TestHelper
    {
        public static MatchState BuildMatchState()
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

        public static IBoardDefinition BuildBoardDefinition()
        {
            return new FakeBoardDefinition(new List<INode>()
                {
                    new Node(
                        0,
                        null,
                        1,
                        NodeType.Base,
                        null
                    ),
                    new Node(
                        1,
                        0,
                        2,
                        NodeType.Path,
                        null
                    ),
                    new Node(
                        2,
                        1,
                        3,
                        NodeType.Path,
                        null
                    ),
                    new Node(
                        3,
                        2,
                        6,
                        NodeType.Gate,
                        FactionType.Blue
                    ),
                    new Node(
                        4,
                        null,
                        null,
                        NodeType.Goal,
                        FactionType.Blue
                    ),
                    new Node(
                        5,
                        null,
                        null,
                        NodeType.Goal,
                        FactionType.Blue
                    ),
                    new Node(
                        6,
                        3,
                        7,
                        NodeType.Start,
                        FactionType.Blue
                    ),
                    new Node(
                        7,
                        6,
                        null,
                        NodeType.Path,
                        null
                    )
                }
            );
        }
    }
}