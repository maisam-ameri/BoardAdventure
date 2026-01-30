using BoardAdventures.Abstractions;
using BoardAdventures.GameObjects.Pawns;
using BoardAdventures.GameObjects.Pawns.Abstractions;
using UnityEngine;
using Zenject;

namespace BoardAdventures.Managers
{
    public class PawnFactory: MonoBehaviour, IPawnFactory
    {
        [SerializeField] private Pawn pawn;
        private DiContainer _container;


        [Inject]
        public void Initialize(DiContainer container)
        {
            _container = container;
        }

        public IPawn Create() => _container.InstantiatePrefabForComponent<Pawn>(pawn);
    }
}