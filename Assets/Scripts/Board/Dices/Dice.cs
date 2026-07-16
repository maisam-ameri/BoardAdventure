using UnityEngine;

namespace BoardAdventures.Board
{
    public class Dice
    {
        public int Roll() => Random.Range(1, 7);
    }
}