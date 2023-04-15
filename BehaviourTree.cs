using System.Collections.Generic;
using AI_BehaviorTree_AIGameUtility;
using UnityEngine.Assertions;

namespace AI_BehaviorTree_AIImplementation
{
    public struct Data
    {
        public Dictionary<BlackboardVariable, object> Blackboard;
        public GameWorldUtils GameWorld;
    }

    public enum BlackboardVariable
    {
        myPlayerPosition,
        myPlayerId,
        targetPosition,
        targetIsEnemy,
        enemyProximityLimit
    }

    class BehaviourTree
    {
        public static BehaviourTree Instance() { Assert.IsNotNull(instance); return instance; }
        private static BehaviourTree instance;

        public Node start;
        public Data data;
        public List<AIAction> computeAction;

        public BehaviourTree()
        {
            instance = this;
            start = new Node();
            data = new Data();
            computeAction = new List<AIAction>();
            data.Blackboard = new Dictionary<BlackboardVariable, object>();
            data.GameWorld = null;
        }

        public List<AIAction> Compute()
        {
            computeAction.Clear();
            start.Evaluate();
            return computeAction;
        }
    }
}
