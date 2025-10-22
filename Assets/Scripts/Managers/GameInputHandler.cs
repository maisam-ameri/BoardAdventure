using System;
using UnityEngine;

namespace BoardAdventures.Managers
{
    public class GameInputHandler: MonoBehaviour
    {
        public event Action OnDiceRollRequested;

        public void HandleDiceRollRequest()
        {
            OnDiceRollRequested?.Invoke();
        }
    }
}