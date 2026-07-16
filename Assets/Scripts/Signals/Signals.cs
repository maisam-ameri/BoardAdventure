using System.Collections.Generic;
using BoardAdventures.Board.Pawns;
using BoardAdventures.Network;
using BoardAdventures.Presentation.Menu;
using Gameplay.Players;

namespace BoardAdventures.Signals
{
    public class OnTurnTimerExpiredSignal
    {
        public double TurnEndTime;
    }

    public class OnCapturedSignal
    {
        public IPawn Pawn;
    }

    public class OnPlayersCreatedSignal
    {
        public List<Player> Players;
    }

    public class OnSelectedPawnSignal
    {
        public IPawn Pawn;
    }

    public class OnDiceRolledSignal
    {
        public int? Step;
    }

    public class OnDiceRollRequestedSignal
    {
    }

    public class OnPlayerActionStartedSignal
    {
    }

    public class OnPawnMoveCompletedSignal
    {
        public Player Player;
    }

    public class OnPlayerActionCompletedSignal
    {
    }

    public class OnGameOverSignal
    {
        public Player Winner;
    }

    public class OnStartMatchSignal
    {
        public float TurnDuration;
    }


    // network

    public class OnConnectionRequestSignal
    {
    }

    public class OnJoinToRoomRequestSignal
    {
        public byte MaxPlayers;
    }

    public class OnConnectionStatusChangedSignal
    {
        public ConnectionState State;
    }

    public class OnLobbyStateChangedSignal
    {
    }

    public class OnLobbyStateUiChangedSignal
    {
        public LobbyState State { get; set; }
        public bool IsMaster { get; set; }
    }

    public class OnTurnEndTimeChangedSignal
    {
    }

    public class OnDiceRollRequestedNetSignal
    {
    }


    // Auth
    public class OnRegisterRequestedSignal
    {
        public string Nickname;
    }

    public class OnShowRegistrationUISignal
    {
    }

    public class OnPlayerLoggedInSignal
    {
    }
}