using Pawns;
using UnityEngine;

namespace Managers
{
    public class PawnManager : MonoBehaviour
    {
        [SerializeField] private Pawn pawnPrefab;
        private PawnFactory _pawnFactory;

        private void Start()
        {
            _pawnFactory = new PawnFactory(pawnPrefab);
        }

        public IPawn CreatePawn(PawnDataForCreate pawnData)
        {
            var newPawn = _pawnFactory.Create();
            newPawn.Position = pawnData.Position;
            newPawn.Color = pawnData.Color;
            newPawn.State = "InBase";

            return newPawn;
        }
    }
}