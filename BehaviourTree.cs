using System.Collections.Generic;
using UnityEngine;

namespace AI_BehaviorTree_AIImplementation
{
    class BehaviourTree
    {
        public Node start = null;
        public Dictionary<string, object> blackboard = null;

        public BehaviourTree()
        {
            blackboard = new Dictionary<string, object>();
            start = new Node();
            start.AssignBlackboard(ref blackboard);
        }

        public void Run()
        {
        }
    }
}
