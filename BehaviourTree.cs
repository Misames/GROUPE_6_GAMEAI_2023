using System.Collections.Generic;
using UnityEngine;
using AI_BehaviorTree_AIGameUtility;

namespace AI_BehaviorTree_AIImplementation
{
    class BehaviourTree
    {
        public Node start = null;
        public Data data;

        public BehaviourTree()
        {
            start = new Node();
            data = new Data();
            data.Blackboard = new Dictionary<string, object>();
            data.GameWorld = null;
            start.AssignData(ref data);
        }

        public void UpdateGameWorldData(ref GameWorldUtils currentGameWorld)
        {
            data.GameWorld = currentGameWorld;
        }

        public Selector AddSelector()
        {
            Selector newSlector = new Selector();
            newSlector.AssignData(ref data);
            return newSlector;
        }

        public Sequence AddSequence()
        {
            Sequence newSquence = new Sequence();
            newSquence.AssignData(ref data);
            return newSquence;
        }

        public Node AddNode()
        {
            Node newNode = new Node();
            newNode.AssignData(ref data);
            return newNode;
        }
    }
}
