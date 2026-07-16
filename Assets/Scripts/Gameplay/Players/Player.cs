using System.Collections.Generic;
using BoardAdventures.Board;
using BoardAdventures.Presentation.Players;

namespace Gameplay.Players
{
    public  class Player
    {
        public int Id { get; set; }
        public string Nickname { get; set; }
        public bool IsActive { get; set; }
        public PlayerUI UI { get; set; }
        public List<Faction> Factions { get; set; } = new ();
    }
}