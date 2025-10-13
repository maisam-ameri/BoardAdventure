using System;
using System.Collections.Generic;
using System.Linq;
using BoardAdventures.GameObjects.Factions;
using BoardAdventures.GameObjects.Pawns.Abstractions;
using BoardAdventures.Managers;
using BoardAdventures.UI;

namespace BoardAdventures.Core.Players
{
    public class PlayerSetupService
    {
        private UIManager _uiManager;
        private PawnManager _pawnManager;
        public List<Player>  Players { get; private set; }
        public event Action OnTurnTimerExpired;
        public event Action<IPawn> OnSelectedPawn;

        
        public void Initialize(UIManager uiManager, PawnManager pawnManager)
        {
            _uiManager = uiManager;
            _pawnManager = pawnManager;
            
            var factions = InitializeFactions();
            Players = CreatePlayer(factions);
            InitialPlayerUIs();
        }
        private List<Player> CreatePlayer(List<Faction> factions)
        {
            var factionPlayer1 = factions.GetRange(0, 2);
            var factionPlayer2 = factions.GetRange(2, 2);

            var players = new List<Player>
            {
                new()
                {
                    Name = "mesi",
                    IsActive = true,
                    Factions = factionPlayer1
                },
                new()
                {
                    Name = "keren",
                    IsActive = true,
                    Factions = factionPlayer2
                }
            };

            return players;
        }

        private void InitialPlayerUIs()
        {
            foreach (var player in Players)
            {
                var ui = _uiManager.CreatePlayerUI();
                ui.SetPlayerUI(player.Name
                    , player.Factions.Select(f => f.Color).ToList()
                    , OnTurnTimerExpired);

                player.UI = ui;
            }
        }

        private List<Faction> InitializeFactions()
        {
            var factions = UnityEngine.Object.FindObjectsOfType<Faction>().ToList();

            foreach (var faction in factions)
            {
                faction.Pawns = new List<IPawn>();
                faction.BaseNodes.ForEach(baseNode =>
                {
                    var newPawn = _pawnManager.CreatePawn(faction, baseNode);
                    newPawn.OnSelectPawn += OnSelectedPawn;
                });
            }

            return factions;
        }
    }
}