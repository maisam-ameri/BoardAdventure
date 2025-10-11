using Abstractions;
using GameLogic;
using Movement;
using Path;
using UI;
using UnityEngine;

namespace Managers
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

        
        public static void Initialize()
        {
            var pathCalculator = new PathCalculator();
            Mover = new Mover();
            
            DiceManager = Object.FindObjectOfType<DiceManager>();
            UIManager = Object.FindObjectOfType<UIManager>();
            TurnVisualizer = Object.FindObjectOfType<TurnVisualizer>();
            UIMessageManager = Object.FindObjectOfType<UIMessageManager>();
            PawnManager = Object.FindObjectOfType<PawnManager>();
            PlayerActionValidator = new PlayerActionValidator(pathCalculator);
            PawnMovementService = new PawnMovementService(PlayerActionValidator, Mover, UIMessageManager);
            GameFlowService = new GameFlowService(UIMessageManager);
            TurnLogicService = new TurnLogicService();
        }
    }
}