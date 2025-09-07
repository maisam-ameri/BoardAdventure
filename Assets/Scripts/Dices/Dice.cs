using UnityEngine;

namespace Dices
{
    public class Dice
    {
        public int Roll() => Random.Range(1, 7);
    }
}