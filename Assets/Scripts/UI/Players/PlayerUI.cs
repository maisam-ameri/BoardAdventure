using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BoardAdventures.UI.Players
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerName;
        [SerializeField] private List<Image> images;
        [SerializeField] private TurnTimer timer;

        private Action _onTurnTimerExpired;
        private CanvasGroup _canvas;

        private void Awake()
        {
            _canvas = GetComponent<CanvasGroup>();
        }

        public void SetPlayerUI(string pName, List<Color> colors, Action onTurnTimerExpired)
        {
            playerName.text = pName;
            _onTurnTimerExpired = onTurnTimerExpired;

            for (var i = 0; i < images.Count; i++)
            {
                if (i > colors.Count - 1)
                {
                    images[i].enabled = false;
                    continue;
                }

                images[i].enabled = true;
                images[i].color = colors[i];
            }

            _onTurnTimerExpired = onTurnTimerExpired;
            timer.TimerExpired += onTurnTimerExpired;
        }

        private void OnDisable()
        {
            timer.TimerExpired -= _onTurnTimerExpired;
        }

        public void SetActivate(bool isActive)
        {
            if (_canvas is null) return;

            _canvas.alpha = isActive ? 1f : 0.2f;
        }

        public void StartTurnTimer(float time)
        {
            timer.StartTimer(time);
        }

        public void StopTimer()
        {
            timer.StopTimer();
        }

        public void PauseTimer()
        {
            timer.PauseTimer();
        }
    }
}