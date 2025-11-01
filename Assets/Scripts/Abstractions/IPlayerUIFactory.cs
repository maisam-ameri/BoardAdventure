using System.Collections.Generic;
using BoardAdventures.UI.Players;
using UnityEngine;

namespace BoardAdventures.Abstractions
{
    public interface IPlayerUIFactory
    {
        PlayerUI Create(string playerName,List<Color> factionColors);
    }
}