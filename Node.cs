using System.Collections.Generic;

namespace AI_BehaviorTree_AIImplementation
{
    public enum State
    {
        FAILURE,
        NOTEXECUTED,
        RUNNING,
        SUCCESS,
    }

    public class Node
    {
        protected State state;
        protected Node parent;
        protected List<Node> children = new List<Node>();

        public Node()
        {
            parent = null;
        }

        public Node(List<Node> children)
        {
            parent = null;
            foreach (Node child in children)
                Attach(child);
        }

        public void Attach(Node child)
        {
            child.parent = this;
            children.Add(child);
        }

        public virtual State Evaluate()
        {
            foreach (Node child in children)
            {
                switch (child.Evaluate())
                {
                    case State.FAILURE:
                        state = State.FAILURE;
                        return state;
                    case State.SUCCESS:
                        state = State.SUCCESS;
                        continue;
                    case State.RUNNING:
                        state = State.RUNNING;
                        break;
                    default:
                        state = State.SUCCESS;
                        return state;
                }
            }
            return state;
        }
    }
}
