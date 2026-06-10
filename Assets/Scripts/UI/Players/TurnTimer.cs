using System.Collections;
using BoardAdventures.Abstractions;
using Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BoardAdventures.UI.Players
{
    public class TurnTimer : MonoBehaviour
    {
        [SerializeField] private Slider timerUI;
        private float _maxTime;
        private Coroutine _timerRoutine;
        private SignalBus _signalBus;

        private double _endTime;
        private double _duration;
        private bool IsRunning { get; set; }
        private INetworkTime _timeProvider;


        [Inject]
        public void Initialize(SignalBus signalBus, INetworkTime timeProvider)
        {
            _signalBus = signalBus;
            _timeProvider = timeProvider;
        }

        public void StartTimer(float duration)
        {
            _duration = duration;
            _endTime = _timeProvider.GetCurrentTime() + duration;
            IsRunning = true;

            if (_timerRoutine != null)
                StopCoroutine(_timerRoutine);

            _timerRoutine = StartCoroutine(TimerProcess());
        }

        private IEnumerator TimerProcess()
        {
            var isRunning = true;

            timerUI.maxValue = (float) _endTime;
            timerUI.value = (float) _duration;

            while (isRunning)
            {
                var currentTime = _timeProvider.GetCurrentTime();
                var remainingTime = _endTime - currentTime;

                if (remainingTime <= 0)
                {
                    timerUI.value = 0;
                    isRunning = false;
                }

                timerUI.value = (float) (remainingTime / _duration);
                yield return null;
            }

            _signalBus.Fire(new OnTurnTimerExpiredSignal());
            IsRunning = false;
        }

        public void StopTimer()
        {
            IsRunning = false;

            if (_timerRoutine != null)
                StopCoroutine(_timerRoutine);

            timerUI.value = 0;
        }

        public void PauseTimer()
        {
            IsRunning = false;

            if (_timerRoutine != null)
                StopCoroutine(_timerRoutine);
        }
    }
}