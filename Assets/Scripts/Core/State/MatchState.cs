using System.Collections.Generic;

namespace BoardAdventures.Core.State
{
    public class MatchState
    {
        public MatchInfo MatchInfo { get; set; }
        public TurnState TurnState { get; set; }
        public DiceState DiceState { get; set; }
        public List<PlayerState> PlayersState { get; set; } = new();
        public BoardState BoardState { get; set; }
        public string WinnerId { get; set; }
    }
    
    public class BoardState
    {
        public List<PawnState> PawnsState { get; set; }
    }

    public class PawnState
    {
        public byte PawnId { get; set; }
        public string OwnerPlayerId { get; set; }
        public int? NodeId { get; set; }
        public FactionType FactionType { get; set; }
        public PawnLocationState PawnLocationState { get; set; }
    }

    public class PlayerState
    {
        public string PlayerId { get; set; }
        public List<FactionType> Factions { get; set; }
    }

    public enum FactionType : byte
    {
        Red,
        Yellow,
        Blue,
        Green
    }

    public class DiceState
    {
        public byte? Step { get; set; }
    }

    public class TurnState
    {
        public string CurrentPlayerId { get; set; }
        public bool ExtraTurnGranted { get; set; }
        public TurnPhase Phase { get; set; }
    }

    public enum TurnPhase: byte
    {
        WaitingForRoll,
        WaitingForPawnSelection,
        ExecutingAction
    }

    public class MatchInfo
    {
        public byte PlayerCount { get; set; }
        public MatchPhase Phase { get; set; }
    }

    public enum MatchPhase : byte
    {
        WaitingForPlayers,
        Playing,
        Finished
    }

    public enum PawnLocationState: byte
    {
        InBase,
        OnBoard,
        InFinalGoal
    }
}