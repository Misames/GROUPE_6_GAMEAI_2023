using System.Collections.Generic;
using AI_BehaviorTree_AIGameUtility;

namespace AI_BehaviorTree_AIImplementation
{
    public enum State
    {
        FAILURE,
        RUNNING,
        SUCCESS,
    }
    public struct Data
    {
        public Dictionary<string, object> Blackboard;
        public GameWorldUtils GameWorld;
    }

    public class Node
    {
        public Data data;

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
            node.AssignData(ref this.data);
        }

        public void AssignData(ref Data parentData)
        {
            data = parentData;
        }

        public override string ToString()
        {
            return "nombre de fils : " + children.Count;
        }

        public virtual State Evaluate() => State.FAILURE;
    }
}
