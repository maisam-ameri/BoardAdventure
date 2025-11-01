using System.Collections.Generic;
using BoardAdventures.Core.Players;
using BoardAdventures.GameObjects.Pawns.Abstractions;

namespace Signals
{
    public class OnRewardGrantedSignal
    {
    }

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

    public class OnGameSetupCompletedSignal
    {
    }

    public class OnSelectedPawnSignal
    {
        public IPawn Pawn;
    }
}