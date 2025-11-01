using System;

namespace BoardAdventures.Abstractions
{
    public interface IDiceManager
    {
        event Action<int?> OnDiceRolled;
        event Action OnFirstSixRolled;
        int? Step { get;}
        bool IsRolled { get; set; }
        void RollDice();
        void SetActivateDice(bool isActive);
        void Reset();
    }
}