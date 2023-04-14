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
        enemyProximityLimit,
        bulletIncoming
        
    }
    class BehaviourTree
    {
        public Node start;
        public Data data;
        public List<AIAction> computeAction;
        public static BehaviourTree Instance() { Assert.IsNotNull(_instance);return _instance; }
        private static BehaviourTree _instance;

        public BehaviourTree()
        {
            BehaviourTree._instance = this;
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

        public void UpdateGameWorldData(GameWorldUtils currentGameWorld)
        {
            data.GameWorld = currentGameWorld;
        }

        public Selector AddSelector()
        {
            Selector newSlector = new Selector();
            return newSlector;
        }

        public Sequence AddSequence()
        {
            Sequence newSquence = new Sequence();
            return newSquence;
        }

        public Action AddAction()
        {
            Action newAction = new Action();
            return newAction;
        }

        public Condition AddCondition()
        {
            Condition newCondition = new Condition();
            return newCondition;
        }
    }
}
