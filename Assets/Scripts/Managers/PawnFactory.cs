using Abstractions;
using Pawns;
using UnityEngine;

namespace Managers
{
    public class PawnFactory: IPawnFactory
    {
        private readonly Pawn _pawn;

        public PawnFactory(Pawn pawn)
        {
            _pawn = pawn;
        }

        public IPawn Create() => Object.Instantiate(_pawn);
    }
}