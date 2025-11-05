using System.Collections.Generic;
using System.Linq;
using BoardAdventures.Abstractions;
using BoardAdventures.GameObjects.Factions;
using BoardAdventures.GameObjects.Pawns.Abstractions;
using Core.Data;
using Signals;
using Zenject;

namespace BoardAdventures.Core.Players
{
    public class PlayerSetupService : IPlayerSetupService
    {
        private readonly IPawnManager _pawnManager;
        private readonly IPlayerUIFactory _playerUIFactory;
        private readonly SignalBus _signalBus;

        public List<Player> Players { get; private set; }


        public PlayerSetupService(IPawnManager pawnManager, IPlayerUIFactory playerUIFactory
            , SignalBus signalBus)
        {
            _pawnManager = pawnManager;
            _playerUIFactory = playerUIFactory;
            _signalBus = signalBus;
        }

        public void Setup(GameMode mode)
        {
            var factions = InitializeFactions();
            Players = CreatePlayer(factions);
            InitialPlayerUIs();

            _signalBus.Fire(new OnPlayersCreatedSignal {Players = Players});
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
                var ui = _playerUIFactory.Create(player.Name, player.Factions.Select(f => f.Color).ToList()
                );

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
                    _pawnManager.CreatePawn(faction, baseNode);
                });
            }

            return factions;
        }
    }
}