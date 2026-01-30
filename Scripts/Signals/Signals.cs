using System.Collections.Generic;
using BoardAdventures.Core.Players;
using BoardAdventures.GameObjects.Pawns.Abstractions;
using BoardAdventures.Network;

namespace Signals
{
    public class OnTurnTimerExpiredSignal
    {
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

    // public class OnFirstSixRolledSignal
    // {
    // }

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
        public int PlayerCount;
    }


    // network
    public class OnConnectionStatusChangedSignal
    {
        public ConnectionState State;
    }

    public class OnPlayerListUpdatedSignal
    {
        public byte MaxPlayer;
        public Dictionary<int,Photon.Realtime.Player> Players;
    }
    
    public class OnPlayersReadyToPlaySignal
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