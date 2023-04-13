using System.Collections.Generic;
using AI_BehaviorTree_AIGameUtility;

namespace AI_BehaviorTree_AIImplementation
{
    class BehaviourTree
    {
        private readonly Node start;
        public Data data;

        public BehaviourTree()
        {
            start = new Node("root");
            data = new Data();
            data.Blackboard = new Dictionary<string, object>();
            data.GameWorld = null;
            data.Blackboard.Add("myPlayerPosition", null);
            data.Blackboard.Add("myPlayerId", null);
            data.Blackboard.Add("targetPosition", null);
            data.Blackboard.Add("targetIsEnemy", false);
            data.Blackboard.Add("enemyProximityLimit", 10);
            start.AssignData(ref data);
            InitSimpleTree();
            start.ParseNodes();
        }

        public void UpdateGameWorldData(ref GameWorldUtils currentGameWorld)
        {
            data.GameWorld = currentGameWorld;
        }

        /// <summary>
        /// Creation statique d'un Behaviour tree 
        /// en 2 étapes : 1 ajouter les nodes 2 les liers
        /// </summary>
        private void InitSimpleTree()
        {
            // creation des nodes
            Selector selector_0 = AddSelector();
            Sequence sequence_0 = AddSequence();
            Sequence sequence_1 = AddSequence();
            Attack attack_0 = AddAttack();
            Attack attack_1 = AddAttack();

            // liaison des nodes
            start.Attach(selector_0);
            selector_0.Attach(sequence_0);
            sequence_0.Attach(attack_0);
            selector_0.Attach(sequence_1);
            sequence_1.Attach(attack_1);
        }

        public Node AddNode()
        {
            Node newNode = new Node();
            newNode.AssignData(ref data);
            return newNode;
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

        public Condition AddCondition()
        {
            Condition newCondition = new Condition();
            newCondition.AssignData(ref data);
            newCondition.AssignCondition(newCondition.CloseToEnemyTarget);
            return newCondition;
        }

        public Action AddAction()
        {
            Action newAction = new Action();
            newAction.AssignData(ref data);
            return newAction;
        }

        public Attack AddAttack()
        {
            Attack newAttack = new Attack();
            newAttack.AssignData(ref data);
            return newAttack;
        }
    }
}
