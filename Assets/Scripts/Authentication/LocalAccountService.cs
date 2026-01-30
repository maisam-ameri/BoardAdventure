using BoardAdventures.Abstractions;
using Signals;
using UnityEngine;
using Zenject;

namespace BoardAdventures.Authentication
{
    public class LocalAccountService : IAccountService
    {
        public string Nickname { get; set; }

        private SignalBus _signalBus;
        private const string NicknameKey = "NICKNAME";

        public LocalAccountService(SignalBus signalBus)
        {
            _signalBus = signalBus;
            
            _signalBus.Subscribe<OnRegisterRequestedSignal>(OnRegisterRequested);
        }

        private void OnRegisterRequested(OnRegisterRequestedSignal signal)
        {
            Nickname = signal.Nickname;
            PlayerPrefs.SetString("NICKNAME", signal.Nickname);
            PlayerPrefs.Save();
            
            _signalBus.Fire(new OnPlayerLoggedInSignal());
            
        }

        public void CheckAuth()
        {
            Nickname = PlayerPrefs.GetString(NicknameKey,"");

            if (string.IsNullOrEmpty(Nickname))
            {
                _signalBus.Fire(new OnShowRegistrationUISignal());
                return;
            }

            _signalBus.Fire(new OnPlayerLoggedInSignal());
        }
    }
}