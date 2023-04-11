using System.Collections.Generic;

namespace AI_BehaviorTree_AIImplementation
{
    public enum State
    {
        FAILURE,
        RUNNING,
        SUCCESS,
    }

    public class Node
    {
        public Dictionary<string, object> Blackboard = null;

        protected State state;
        public Node parent;
        protected List<Node> children = new List<Node>();

        public Node()
        {
            parent = null;
        }

        public Node(List<Node> children)
        {
            foreach (Node node in children)
            {
                Attach(node);
            }
        }

        public void Attach(Node node)
        {
            node.parent = this;
            this.children.Add(node);
            node.AssignBlackboard(ref this.Blackboard);
        }

        public void AssignBlackboard(ref Dictionary<string, object> data)
        {
            Blackboard = data;
        }

        public virtual State Evaluate() => State.FAILURE;
    }
}
