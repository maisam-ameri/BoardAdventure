using BoardAdventures.Abstractions;
using BoardAdventures.GameObjects.Pawns;
using BoardAdventures.GameObjects.Pawns.Abstractions;
using UnityEngine;

namespace BoardAdventures.Managers
{
    public class PawnFactory: IPawnFactory
    {
        private readonly Pawn _pawn;

        public PawnFactory(Pawn pawn)
        {
            _pawn = pawn;
        }

        public PawnFactory()
        {
            
        }

        public IPawn Create() => Object.Instantiate(_pawn);
    }
}