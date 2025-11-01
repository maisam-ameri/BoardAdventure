using System;
using BoardAdventures.Abstractions;
using UnityEngine;

namespace BoardAdventures.Managers
{
    public class GameInputHandler: MonoBehaviour, IGameInputHandler
    {
        public event Action OnDiceRollRequested;

        public void HandleDiceRollRequest()
        {
            OnDiceRollRequested?.Invoke();
        }
    }
}