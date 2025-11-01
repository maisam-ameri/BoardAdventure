using System.Collections;
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

        private bool IsRunning { get; set; }
        


        [Inject]
        public void Initialize(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        public void StartTimer(float maxTime)
        {
            _maxTime = maxTime;
            IsRunning = true;

            if (_timerRoutine != null)
                StopCoroutine(_timerRoutine);

            _timerRoutine = StartCoroutine(RunTimer());
        }

        private IEnumerator RunTimer()
        {
            timerUI.maxValue = _maxTime;
            timerUI.value = _maxTime;
            var remainingTime = _maxTime;

            while (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
                timerUI.value = remainingTime;
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