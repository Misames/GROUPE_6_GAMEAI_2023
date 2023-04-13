using System.Collections.Generic;
using AI_BehaviorTree_AIGameUtility;

namespace AI_BehaviorTree_AIImplementation
{
    class BehaviourTree
    {
        public Node start = null;
        public Data data;

        public BehaviourTree()
        {
            start = new Node("root");
            data = new Data();
            data.Blackboard = new Dictionary<string, object>();
            data.GameWorld = null;
            start.AssignData(ref data);
            data.Blackboard.Add("myPlayerPosition", null);
            data.Blackboard.Add("myPlayerId", null);
            data.Blackboard.Add("targetPosition", null);
            data.Blackboard.Add("targetIsEnemy", false);
            data.Blackboard.Add("enemyProximityLimit", 10);
            InitSimpleTree();
            ParseNodes(start);
        }

        public void UpdateGameWorldData(ref GameWorldUtils currentGameWorld)
        {
            data.GameWorld = currentGameWorld;
        }

        /// <summary>
        /// Creation statique d'un Behaviour tree 
        /// en 2 étapes : 1 ajouter les nodes 2 les liers
        /// </summary>
        public void InitSimpleTree()
        {
            // creation des nodes
            Selector selector_0 = AddSelector();
            Sequence sequence_0 = AddSequence();
            Sequence sequence_1 = AddSequence();
            Node node_0 = AddNode();
            Node node_1 = AddNode();

            // liaison des nodes
            start.Attach(selector_0);
            selector_0.Attach(sequence_0);
            selector_0.Attach(sequence_1);
            sequence_0.Attach(node_0);
            sequence_1.Attach(node_1);
        }

       /// <summary>
       ///  Lance la fonction d'évalutation de toutes les nodes de notre arbre
       /// </summary>
       /// <param name="node"></param>
        public void ParseNodes(Node node)
        {
            foreach (Node child in node.children)
            {
                child.Evaluate();
                ParseNodes(child);
            }
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
    }
}
