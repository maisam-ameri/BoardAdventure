using BoardAdventures.Core.State;

namespace BoardAdventures.Core.Board
{
    public interface INode
    {
        public byte NodeId { get;  }
        public byte? PrevNodeId { get;  }
        public byte? NextNodeId { get;  }
        public NodeType NodeType { get;  }
        public FactionType? FactionType { get;  }
    }
}