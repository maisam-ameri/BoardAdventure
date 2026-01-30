using System.Collections.Generic;
using BoardAdventures.Core.Players;

namespace BoardAdventures.Abstractions
{
    public interface ITurnVisualizer
    {
        void Initial(List<Player> players);
        void UpdatePawnHighlights(Player currentPlayer, Player lastPlayer);
        void UpdatePlayerPanels(Player currentPlayer, Player lastPlayer);
        void DeactivateTurnVisuals();
        void DeactivatePlayerVisuals();
    }
}