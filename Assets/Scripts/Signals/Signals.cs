using System.Collections.Generic;
using BoardAdventures.Core.Players;
using BoardAdventures.GameObjects.Pawns.Abstractions;

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
    
    public class OnFirstSixRolledSignal
    {
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
    
}