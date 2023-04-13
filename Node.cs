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

    public struct Data
    {
        public Dictionary<string, object> Blackboard;
        public GameWorldUtils GameWorld;
    }

    public class Node
    {
        public static uint nbNode;

        protected NodeType nodeType;
        protected Data data;
        protected State state;
        protected Node parent;
        protected List<Node> children = new List<Node>();

        public Node()
        {
            nodeType = NodeType.NODE;
            parent = null;
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

        public void Attach(Node node)
        {
            node.parent = this;
            children.Add(node);
            node.AssignData(ref this.data);
        }

        public void AssignData(ref Data parentData)
        {
            data = parentData;
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
        /// Par défaut retourne une FAILURE
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
