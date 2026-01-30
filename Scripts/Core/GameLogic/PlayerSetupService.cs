using System.Collections.Generic;
using System.Linq;
using BoardAdventures.Abstractions;
using BoardAdventures.Core.Players;
using BoardAdventures.GameObjects.Factions;
using BoardAdventures.GameObjects.Pawns.Abstractions;
using Signals;
using Zenject;

namespace BoardAdventures.Core.GameLogic
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

        public void Setup(List<Player> players)
        {
            var factions = InitializeFactions();
            Players = CreatePlayer(factions, players);
            InitialPlayerUIs();

            _signalBus.Fire(new OnPlayersCreatedSignal {Players = Players});
        }

        private List<Player> CreatePlayer(List<Faction> factions,List<Player> players)
        {

            var numberOfPlayers = players.Count;
            var playerFactions = SplitFactions(numberOfPlayers, factions);
            var inMatchPlayers = new List<Player>();
            
            for (var i = 0; i < numberOfPlayers; i++)
            {
                var player = new Player()
                {
                    Nickname = players[i].Nickname,
                    IsActive = true,
                    Factions = playerFactions[i]
                };
                
                inMatchPlayers.Add(player);
            }

            return inMatchPlayers;
        }

        private List<List<Faction>> SplitFactions(int playerCount, List<Faction> factions)
        {
            List<List<Faction>> result = new List<List<Faction>>();
            var factionsPerPlayer = factions.Count / playerCount;
            
            for (var i = 0; i < playerCount; i++)
            {
                var playerFactions = factions
                    .Skip(i * factionsPerPlayer)
                    .Take(factionsPerPlayer).ToList();
                
                result.Add(playerFactions);
            }

            return result;
        }

        private void InitialPlayerUIs()
        {
            foreach (var player in Players)
            {
                var ui = _playerUIFactory.Create(player.Nickname, player.Factions.Select(f => f.Color).ToList()
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