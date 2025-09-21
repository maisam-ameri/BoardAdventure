using System;

namespace Abstractions
{
    public interface ITimer
    {
        void StartTimer(float maxTime);
        void StopTimer();
        bool IsRunning { get;  }
        event Action TimerExpired;
    }
}