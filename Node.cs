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

    public enum BlackboardEnum
    {
        myPlayerPosition,
        myPlayerId,
        playerRotation,
        target,
        targetPosition,
        targetIsEnemy,
        enemyProximityLimit,
        actionList
    }

    public struct Data
    {
        public List<object> Blackboard;
        public GameWorldUtils GameWorld;
    }

    public class Node
    {
        public static uint nbNode;

        protected string name;
        protected Data data;
        protected State state;
        protected Node parent;
        protected List<Node> children = new List<Node>();

        public Node()
        {
            name = string.Empty;
            parent = null;
        }

        public Node(Node parent)
        {
            name= string.Empty;
            this.parent = parent;
        }

        public Node(string name, Node parent)
        {
            this.name = name;
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

        public override string ToString()
        {
            return name;
        }

        /// <summary>
        /// Permet de tester l'état du Node <br />
        /// Ca nous sera utile au moment de l'éxecution des Nodes <br />
        /// Par défaut retourne une FAILURE
        /// </summary>
        public virtual State Evaluate(Data data) => State.FAILURE;
    }
}
