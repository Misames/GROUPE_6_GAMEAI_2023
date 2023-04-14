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
        protected Data data;
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

        /// <summary>
        /// Fonction permetant de faire la liaison entre toutes nos nodes dans notre arbre
        /// </summary>
        /// <param name="childNode">futur fils de la node actuel</param>
        public void Attach(Node childNode)
        {
            childNode.parent = this;
            children.Add(childNode);
            childNode.AssignData(ref data);
        }

        /// <summary>
        /// Permet de donner un acces aux données du world
        /// </summary>
        /// <param name="parentData"></param>
        public void AssignData(ref Data parentData)
        {
            data = parentData;
        }

        /// <summary>
        /// Permet de tester l'état du Node <br />
        /// Ca nous sera utile au moment de l'éxecution des Nodes <br />
        /// </summary>
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
