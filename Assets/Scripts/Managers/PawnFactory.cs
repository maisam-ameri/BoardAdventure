using Factions;
using Pawns;
using UnityEngine;

namespace Managers
{
    public class PawnFactory
    {
        private readonly Pawn _pawn;

        public PawnFactory(Pawn pawn)
        {
            _pawn = pawn;
        }

        public IPawn Create() => Object.Instantiate(_pawn);
    }
}