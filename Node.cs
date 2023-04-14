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

    public enum NodeType
    {
        NODE,
        START,
        SELECTOR,
        SEQUENCE,
        CONDITION,
        ACTION
    }

    public class Node
    {
        private static uint nbNode;

        protected NodeType nodeType;

        protected State state;
        protected Node parent;
        protected List<Node> children = new List<Node>();

        ~Node()
        {
            nbNode -= 1;
        }

        public Node()
        {
            nbNode += 1;
            nodeType = NodeType.NODE;
            parent = null;
        }

        public Node(NodeType nType)
        {
            nbNode += 1;
            this.nodeType = nType;
        }

        public Node(Node parent)
        {
            nodeType = NodeType.NODE;
            this.parent = parent;
        }

        public Node(NodeType nType, Node parent)
        {
            this.nodeType = nType;
            this.parent = parent;
        }

        public Node(List<Node> children)
        {
            foreach (Node node in children)
                Attach(node);
        }

        /// <summary>
        /// Fonction permetant de faire la liaison entre toutes nos nodes dans notre arbre
        /// </summary>
        /// <param name="childNode">futur fils de la node actuel</param>
        public void Attach(Node childNode)
        {
            childNode.parent = this;
            children.Add(childNode);
        }       

        protected static State CastEvaluate(Node node)
        {
            State childState;

            switch (node.nodeType)
            {
                case NodeType.SELECTOR:
                    Selector tempSelector = (Selector)node;
                    childState = tempSelector.Evaluate();
                    break;

                case NodeType.SEQUENCE:
                    Sequence tempSequence = (Sequence)node;
                    childState = tempSequence.Evaluate();
                    break;

                case NodeType.CONDITION:
                    Condition tempCondition = (Condition)node;
                    childState = tempCondition.Evaluate();
                    break;

                case NodeType.ACTION:
                    Action tempAction = (Action)node;
                    childState = tempAction.Evaluate();
                    break;

                default:
                    childState = node.Evaluate();
                    break;
            }
            return childState;
        }


        /// <summary>
        /// Permet de tester l'état du Node <br />
        /// Ca nous sera utile au moment de l'éxecution des Nodes <br />
        /// </summary>
        public virtual State Evaluate()
        {
            //UnityEngine.Debug.LogError("node");
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
