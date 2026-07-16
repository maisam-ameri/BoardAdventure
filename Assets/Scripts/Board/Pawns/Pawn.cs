using BoardAdventures.Board.Nodes;
using BoardAdventures.Signals;
using UnityEngine;
using Zenject;

namespace BoardAdventures.Board.Pawns
{
    public class Pawn : MonoBehaviour, IPawn
    {
        public PawnLocationState LocationState { get; set; } = PawnLocationState.InBase;
        public Color Color
        {
            get => GetComponent<SpriteRenderer>().color;
            set
            {
                _tempColor ??= value;
                GetComponent<SpriteRenderer>().color = value;
            }
        }

        public Vector2 Position
        {
            get => transform.position;
            set => transform.position = value;
        }
        public bool IsActive
        {
            set
            {
                Color = value ? _tempColor ?? Color : Color = Color.gray;
                Collider.enabled = value;
            } 
        }
        public Faction Faction { get; set; }
        public INode CurrentNode { get; set; }
        public Collider2D Collider => GetComponent<Collider2D>();

        private Color? _tempColor;
        private SignalBus _signalBus;


        [Inject]
        public void Initialize(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        private void OnMouseDown()
        {
            _signalBus.Fire(new OnSelectedPawnSignal{Pawn = this});
        }

    }
}