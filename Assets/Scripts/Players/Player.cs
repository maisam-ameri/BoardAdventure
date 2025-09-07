using System.Collections.Generic;
using Factions;

namespace Players
{
    public  class Player
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public List<Faction> Factions { get; set; } = new ();
    }
}