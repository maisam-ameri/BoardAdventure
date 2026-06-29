namespace BoardAdventures.Abstractions
{
    public interface IDiceManager
    {
        int? Step { get;}
        bool IsRolled { get; set; }
        void RollDice();
        void UpdateDiceUI(int step);
        void SetActivateDice(bool isActive);
        void Reset();
    }
}