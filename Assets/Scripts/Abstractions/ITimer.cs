using System;

namespace BoardAdventures.Abstractions
{
    public interface ITimer
    {
        void StartTimer(float maxTime);
        void StopTimer();
        void PauseTimer();
        bool IsRunning { get;  }
        event Action TimerExpired;
    }
}