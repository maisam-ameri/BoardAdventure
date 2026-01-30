namespace BoardAdventures.Abstractions
{
    public interface IDiceManager
    {
        int? Step { get;}
        bool IsRolled { get; set; }
        void RollDice();
        void SetActivateDice(bool isActive);
        void Reset();
    }
}