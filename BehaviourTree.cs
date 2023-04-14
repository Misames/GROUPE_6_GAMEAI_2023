using System.Collections.Generic;
using AI_BehaviorTree_AIGameUtility;

namespace AI_BehaviorTree_AIImplementation
{
    class BehaviourTree
    {
        public Node start;
        public Data data;

        public BehaviourTree()
        {
            start = new Node();
            data = new Data();
            data.Blackboard = new Dictionary<string, object>();
            data.GameWorld = null;
            start.AssignData(ref data);
        }

        public void Compute()
        {
            start.Evaluate();
        }

        public void UpdateGameWorldData(GameWorldUtils currentGameWorld)
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

        public Action AddAction()
        {
            Action newAction = new Action();
            newAction.AssignData(ref data);
            return newAction;
        }

        public Condition AddCondition()
        {
            Condition newCondition = new Condition();
            newCondition.AssignData(ref data);
            return newCondition;
        }
    }
}
