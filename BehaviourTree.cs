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

        public void Run()
        {
        }
    }
}
