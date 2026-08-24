using BoardAdventures.Core.Board;
using BoardAdventures.Core.State;

namespace Core.Board
{
    public class Node: INode
    {
        public byte NodeId { get;  }
        public byte? PrevNodeId { get;  }
        public byte? NextNodeId { get;  }
        public NodeType NodeType { get;  } = NodeType.Path;
        public FactionType? FactionType { get;  }
    }
}