using BoardAdventures.Abstractions;
using BoardAdventures.Core.GameLogic;
using BoardAdventures.Core.Movement;
using BoardAdventures.Core.Path;
using BoardAdventures.Core.Players;
using BoardAdventures.UI;
using BoardAdventures.UI.Common;
using BoardAdventures.UI.Players;
using UnityEngine;

namespace BoardAdventures.Managers
{
    public static class GameServices
    {
        public static DiceManager DiceManager { get; private set; }
        public static UIManager UIManager { get; private set; }
        public static TurnLogicService TurnLogicService { get; private set; }
        public static TurnVisualizer TurnVisualizer { get; private set; }
        public static UIMessageManager UIMessageManager { get; private set; }
        public static PawnManager PawnManager { get; private set; }
        public static PlayerActionValidator PlayerActionValidator { get; private set; }
        public static PawnMovementService PawnMovementService { get; private set; }
        public static IMovement Mover { get; private set; }
        public static GameFlowService GameFlowService { get; private set; }
        public static PlayerSetupService PlayerSetupService { get; private set; }
        public static PlayerActionService PlayerActionService { get; private set; }
        public static GameInputHandler GameInputHandler { get; private set; }
        private static TurnFlowService TurnFlowService { get; set; }


        public static void Initialize()
        {
            var pathCalculator = new PathCalculator();
            Mover = new Mover();

            DiceManager = Object.FindObjectOfType<DiceManager>();
            UIManager = Object.FindObjectOfType<UIManager>();
            TurnVisualizer = Object.FindObjectOfType<TurnVisualizer>();
            UIMessageManager = Object.FindObjectOfType<UIMessageManager>();
            PawnManager = Object.FindObjectOfType<PawnManager>();
            GameInputHandler = Object.FindObjectOfType<GameInputHandler>();
            PlayerActionValidator = new PlayerActionValidator(pathCalculator);
            PawnMovementService = new PawnMovementService(PlayerActionValidator, Mover, UIMessageManager);
            TurnLogicService = new TurnLogicService();
            var playerUIFactory = new PlayerUIFactory(UIManager.PlayerUIPrefab, UIManager.PlayerUIParent);
            PlayerSetupService = new PlayerSetupService(PawnManager, playerUIFactory);
            TurnFlowService = new TurnFlowService();
            GameFlowService =
                new GameFlowService(UIMessageManager
                    , TurnFlowService
                    , DiceManager
                    , TurnVisualizer
                    , GameInputHandler
                    , PlayerActionValidator
                    , TurnLogicService);
            PlayerActionService = new PlayerActionService(DiceManager, UIMessageManager, GameFlowService, PawnManager,
                PawnMovementService);

            PlayerSetupService.OnSelectedPawn += PlayerActionService.HandelPawnSelected;
        }
    }
}