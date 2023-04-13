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
        public static uint nbNode;

        public string name;
        public State state = State.FAILURE;
        public Data data;
        public Node parent;
        public List<Node> children = new List<Node>();

        ~Node()
        {
            nbNode -= 1;
        }

        public Node()
        {
            nbNode += 1;
            name = "new node " + nbNode;
            parent = null;
        }

        public Node(string name)
        {
            nbNode += 1;
            this.name = name;
        }

        public Node(Node parent)
        {
            nbNode += 1;
            name = "new node " + nbNode;
            this.parent = parent;
        }

        public Node(string name, Node parent)
        {
            nbNode += 1;
            this.name = name;
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
        /// Par défaut retourne une FAILURE
        /// </summary>
        public virtual State Evaluate() => State.FAILURE;
    }
}
