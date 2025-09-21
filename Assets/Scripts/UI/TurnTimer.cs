using System;
using System.Collections;
using Abstractions;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TurnTimer : MonoBehaviour, ITimer
    {
        [SerializeField] private Slider timerUI;
        private float _maxTime;

        private Coroutine _timerRoutine;

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
            
            TimerExpired?.Invoke();
            IsRunning = false;
        }

        public void StopTimer()
        {
            IsRunning = false;
            
            if (_timerRoutine != null)
                StopCoroutine(_timerRoutine);

            timerUI.value = 0;
        }

        public bool IsRunning { get; private set; }
        public event Action TimerExpired;
    }
}