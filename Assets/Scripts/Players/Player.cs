using System.Collections.Generic;
using Factions;
using Players.UI;

namespace Players
{
    public  class Player
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public PlayerUI UI { get; set; }
        public List<Faction> Factions { get; set; } = new ();
    }
}