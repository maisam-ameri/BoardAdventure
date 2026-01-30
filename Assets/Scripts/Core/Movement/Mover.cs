using System.Collections.Generic;
using System.Threading.Tasks;
using BoardAdventures.Abstractions;
using BoardAdventures.Core.Path;
using BoardAdventures.GameObjects.Nodes.Abstractions;
using BoardAdventures.GameObjects.Pawns.Abstractions;
using Signals;
using Zenject;

namespace BoardAdventures.Core.Movement
{
    public class Mover : IMovement
    {
        private int _delay = 500;
        private readonly SignalBus _signalBus;
        private readonly ICurrentPlayerProvider _playerProvider;

        public int Delay
        {
            set => _delay = value;
        }


        public Mover(ICurrentPlayerProvider currentPlayerProvider, SignalBus signalBus)
        {
            _playerProvider = currentPlayerProvider;
            _signalBus = signalBus;
        }


        public async Task Move(IPawn pawn, List<INode> path)
        {
            _signalBus.Fire(new OnPlayerActionStartedSignal());
            
            if (path == null || path.Count == 0) return;

            pawn.CurrentNode.Pawn = null;
            pawn.CurrentNode.IsEmpty = true;

            foreach (var node in path)
            {
                if (path.Count > 1 && node.Equals(path[^2]))
                {
                    if (PathValidator.CheckNodeToCapture(pawn, path[^1]))
                        _signalBus.Fire(new OnCapturedSignal {Pawn = path[^1].Pawn});
                }

                pawn.Position = node.Position;
                await Task.Delay(_delay);
            }

            pawn.CurrentNode = path[^1];
            pawn.CurrentNode.Pawn = pawn;
            pawn.CurrentNode.IsEmpty = false;
            _signalBus.Fire(new OnPawnMoveCompletedSignal{Player = _playerProvider.CurrentPlayer});
        }
    }
}