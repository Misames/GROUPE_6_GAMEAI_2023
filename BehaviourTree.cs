using System.Collections.Generic;

namespace AI_BehaviorTree_AIImplementation
{
    class BehaviourTree
    {
        public static List<Node> nodeList = new List<Node>();
        public Dictionary<string, object> blackboard = null;

        public BehaviourTree()
        {
            blackboard = new Dictionary<string, object>();
            nodeList.Add(new Node());
            nodeList[0].AssignBlackboard(ref blackboard);
        }

        public Selector AddSelector()
        {
            Selector newSlector = new Selector();
            newSlector.AssignBlackboard(ref blackboard);
            nodeList.Add(newSlector);
            return newSlector;
        }

        public Sequence AddSequence()
        {
            Sequence newSquence = new Sequence();
            newSquence.AssignBlackboard(ref blackboard);
            nodeList.Add(newSquence);
            return newSquence;
        }

        public Node AddNode()
        {
            Node newNode = new Node();
            newNode.AssignBlackboard(ref blackboard);
            nodeList.Add(newNode);
            return newNode;
        }
    }
}
