using System.Collections.Generic;
using BoardAdventures.Presentation.Players;
using UnityEngine;

namespace BoardAdventures.Presentation
{
    public interface IPlayerUIFactory
    {
        PlayerUI Create(string playerName,List<Color> factionColors);
    }
}