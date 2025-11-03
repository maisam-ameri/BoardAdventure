using System.Threading.Tasks;
using BoardAdventures.Abstractions;
using BoardAdventures.Core.Players;
using Signals;
using Zenject;

namespace BoardAdventures.Core.GameLogic
{
    public class GameFlowService : IGameFlowService
    {
        public Player CurrentPlayer => _turnFlowService.CurrentPlayer;
        private Player LastPlayer => _turnFlowService.LastPlayer;

        private readonly IUIMessageManager _uiMessageManager;
        private readonly ITurnFlowService _turnFlowService;
        private readonly IDiceManager _diceManager;
        private readonly ITurnVisualizer _turnVisualizer;
        private readonly IPlayerActionValidator _playerActionValidator;
        private readonly ITurnLogicService _turnLogicService;
        private bool _firstSixRolled;
        private readonly SignalBus _signalBus;
        private readonly IPlayerSetupService _playerSetupService;

        public GameFlowService(IUIMessageManager uiMessageManager
            , ITurnFlowService turnFlowService
            , IDiceManager diceManager
            , ITurnVisualizer turnVisualizer
            , IPlayerActionValidator playerActionValidator
            , ITurnLogicService turnLogicService
            , IPlayerSetupService playerSetupService
            , SignalBus signalBus)
        {
            _uiMessageManager = uiMessageManager;
            _turnFlowService = turnFlowService;
            _diceManager = diceManager;
            _turnVisualizer = turnVisualizer;
            _playerActionValidator = playerActionValidator;
            _turnLogicService = turnLogicService;
            _playerSetupService = playerSetupService;
            _signalBus = signalBus;

            _signalBus.Subscribe<OnTurnSwitchedSignal>(HandleSwitchTurn);
            _signalBus.Subscribe<OnTurnStartedSignal>(HandleTurnStarted);
            _signalBus.Subscribe<OnTurnTimerExpiredSignal>(SwitchTurn);
            _signalBus.Subscribe<OnFirstSixRolledSignal>(HandleFirstSixVisual);
            _signalBus.Subscribe<OnDiceRolledSignal>(HandleDiceRolled);
            _signalBus.Subscribe<OnDiceRollRequestedSignal>(OnDiceButtonClicked);
            _signalBus.Subscribe<OnPlayerActionStartedSignal>(HandlePlayerActionStarted);
            _signalBus.Subscribe<OnPlayerActionCompletedSignal>(HandlePlayerActionCompleted);
            
        }

        public void StartGame()
        {
            _playerSetupService.Setup();
            var players = _playerSetupService.Players;
            _turnVisualizer.Initial(players);
            _turnVisualizer.DeactivateTurnVisuals();
            _turnVisualizer.DeactivatePlayerVisuals();
            _turnVisualizer.UpdatePlayerPanels(CurrentPlayer, null);
        }

        private void OnDiceButtonClicked()
        {
            _diceManager.RollDice();
        }

        private void StartTurn()
        {
            _uiMessageManager.ShowPlayerTurnMessage(CurrentPlayer.Name);
            HandleTurnStarted(null);
        }

        private void HandleTurnStarted(OnTurnStartedSignal signal)
        {
            if (_firstSixRolled)
                _turnVisualizer.UpdatePawnHighlights(CurrentPlayer, LastPlayer);

            _turnVisualizer.UpdatePlayerPanels(CurrentPlayer, LastPlayer);
            CurrentPlayer.UI.StartTurnTimer(10);
        }

        public void SwitchTurn()
        {
            HandleSwitchTurn();
        }

        private async void HandleSwitchTurn()
        {
            await Task.Delay(1000);
            _diceManager.Reset();
            _diceManager.SetActivateDice(true);
            CurrentPlayer.UI.StopTimer();
            _turnFlowService.NextPlayer();
            StartTurn();
        }

        private void HandleFirstSixVisual()
        {
            _firstSixRolled = true;
            _turnVisualizer.UpdatePawnHighlights(CurrentPlayer, LastPlayer);
        }

        public void GrantReward(Player player)
        {
            _uiMessageManager.ShowRewardMessage(player.Name);
            _diceManager.SetActivateDice(true);
            CurrentPlayer.UI.StartTurnTimer(10);
        }

        private void HandleDiceRolled(OnDiceRolledSignal signal)
        {
            var step = signal.Step;
            
            if (step is null) return;

            _diceManager.IsRolled = true;
            _diceManager.SetActivateDice(false);
            CurrentPlayer.UI.StartTurnTimer(10);


            var canEnter = _playerActionValidator.CheckToEnterPawn(CurrentPlayer, step);
            var canMove = _playerActionValidator.CheckToMovePawn(CurrentPlayer, step);

            var decision = _turnLogicService.ProcessRoll(step, canEnter, canMove);

            switch (decision)
            {
                case TurnDecision.WaitForAction:
                    _uiMessageManager.ShowActionAvailableMessage(CurrentPlayer.Name, step.Value);
                    CurrentPlayer.UI.StartTurnTimer(10);
                    break;
                case TurnDecision.RollReward:
                    _uiMessageManager.ShowRewardMessage(CurrentPlayer.Name);
                    _diceManager.SetActivateDice(true);
                    break;

                case TurnDecision.SwitchTurn:
                    SwitchTurn();
                    break;
            }
        }

        public void HandlePlayerActionCompleted()
        {
            _diceManager.Reset();

            if (_turnLogicService.HasReward)
            {
                GrantReward(CurrentPlayer);
            }
            else
            {
                SwitchTurn();
            }
        }

        public void HandlePlayerActionStarted()
        {
            CurrentPlayer.UI.PauseTimer();
        }
    }
}