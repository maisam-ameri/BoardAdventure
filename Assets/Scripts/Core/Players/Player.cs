using System.Collections.Generic;
using BoardAdventures.GameObjects.Factions;
using BoardAdventures.UI.Players;

namespace BoardAdventures.Core.Players
{
    public  class Player
    {
        public string Nickname { get; set; }
        public bool IsActive { get; set; }
        public PlayerUI UI { get; set; }
        public List<Faction> Factions { get; set; } = new ();
    }
}