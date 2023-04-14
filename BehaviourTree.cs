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
            start = new Node();
            data = new Data();
            data.Blackboard = new List<object>();
            data.GameWorld = null;
            start.AssignData(ref data);
            data.Blackboard.Add(null); // Player Position
            data.Blackboard.Add(null); //Player ID
            data.Blackboard.Add(null); //Player Rotation
            data.Blackboard.Add(null); //bonus target
            data.Blackboard.Add(null); // Target
            data.Blackboard.Add(null); // Target position
            data.Blackboard.Add(false); // target is enemy
            data.Blackboard.Add(10); // enemy proximity limit
            data.Blackboard.Add(new List<AIAction>()); // action list

            //InitSimpleTree();
        }

        public void UpdateGameWorldData(ref GameWorldUtils currentGameWorld)
        {
            data.GameWorld = currentGameWorld;
        }

        public void InitSimpleTree()
        {


            // Creation statique d'un Behaviour tree
            // en 2 étapes : 1 ajouter les nodes 2 les liers

            // creation des nodes
            var selector_0 = AddSelector();
            var sequence_0 = AddSequence();
            var sequence_1 = AddSequence();
            var node_0 = AddNode();
            var node_1 = AddNode();
            var condition_0 = new Condition();
            condition_0.AssignCondition(condition_0.CloseToEnemyTarget);

            // liaison des nodes
            start.Attach(selector_0);
            selector_0.Attach(sequence_0);
            selector_0.Attach(sequence_1);
            sequence_0.Attach(condition_0);
            sequence_0.Attach(node_0);
            sequence_1.Attach(node_1);

            UnityEngine.Debug.LogError("arbre fini ! UwU");
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

        public Condition AddCondition()
        {
            Condition newCondition = new Condition();
            newCondition.AssignCondition(newCondition.CloseToEnemyTarget);
            return newCondition;
        }
    }
}
