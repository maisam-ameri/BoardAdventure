using System.Collections.Generic;
using BoardAdventures.Abstractions;
using BoardAdventures.Core.Players;
using UnityEngine;

namespace BoardAdventures.UI.Common
{
    public class TurnVisualizer : MonoBehaviour,ITurnVisualizer
    {

        private List<Player> _players;

        public void Initial(List<Player> players)
        {
            _players = players;
        }

        public void UpdatePawnHighlights(Player currentPlayer, Player lastPlayer)
        {
            lastPlayer?.Factions.ForEach(f => f.Pawns.ForEach(p => p.IsActive = false));
            currentPlayer.Factions.ForEach(f => f.Pawns.ForEach(p => p.IsActive = true));
        }

        public void UpdatePlayerPanels(Player currentPlayer, Player lastPlayer)
        {
            lastPlayer?.UI.SetActivate(false);
            currentPlayer.UI.SetActivate(true);
            currentPlayer.UI.StartTurnTimer(5);
        }
        
        public void DeactivateTurnVisuals()
        {
            foreach (var player in _players)
            {
                var factions = player.Factions;

                //factions.ForEach(f => f.SetActivate(false));
                factions.ForEach(f => f.Pawns.ForEach(p => p.IsActive = false));
            }
        }
        
        public void DeactivatePlayerVisuals()
        {
            foreach (var player in _players)
            {
                player.UI.SetActivate(false);
                player.UI.StopTimer();
            } 
        }

    }
}