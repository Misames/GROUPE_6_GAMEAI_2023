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
            foreach (Node node in children)
                Attach(node);
        }

        public void Attach(Node childNode)
        {
            childNode.parent = this;
            children.Add(childNode);
        }

        /// <summary>
        /// Permet de tester l'état du Node <br />
        /// Ca nous sera utile au moment de l'éxecution des Nodes <br />
        /// </summary>
        public virtual State Evaluate()
        {
            UnityEngine.Debug.LogError("node");
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
