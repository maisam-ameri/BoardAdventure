using System.Collections.Generic;
using System.Threading.Tasks;
using BoardAdventures.Board.Nodes;
using BoardAdventures.Board.Pawns;
using BoardAdventures.Gameplay.GameLogic.Abstractions;
using BoardAdventures.Gameplay.Path;
using BoardAdventures.Signals;
using Zenject;

namespace BoardAdventures.Gameplay.Movement
{
    public class Mover : IMovement
    {
        private int _delay = 500;
        private readonly SignalBus _signalBus;
        private readonly IActivePlayerProvider _playerProvider;

        public int Delay
        {
            set => _delay = value;
        }


        public Mover(IActivePlayerProvider activePlayerProvider, SignalBus signalBus)
        {
            _playerProvider = activePlayerProvider;
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
            _signalBus.Fire(new OnPawnMoveCompletedSignal{Player = _playerProvider.ActivePlayer});
        }
    }
}