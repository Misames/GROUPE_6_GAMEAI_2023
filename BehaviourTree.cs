using System;
using System.Collections.Generic;
using System.Numerics;
using AI_BehaviorTree_AIGameUtility;
using UnityEngine;
using Windows.UI.Xaml.Controls;

namespace AI_BehaviorTree_AIImplementation
{
    class BehaviourTree
    {
        public Node start = null;
        public Data data;
        public TargetManager targetManager;
        

        public BehaviourTree()
        {
            start = new Node();
            data = new Data();
            data.Blackboard = new Dictionary<string, object>();
            data.GameWorld = null;
            start.AssignData(ref data);
            data.Blackboard.Add("myPlayerPosition", null);
            data.Blackboard.Add("myPlayerId", null);
            data.Blackboard.Add("targetPredictedPos", null);
            data.Blackboard.Add("targetPosition", null);
            data.Blackboard.Add("targetIsEnemy", false);
            data.Blackboard.Add("chosenTarget", null);
            data.Blackboard.Add("enemyProximityLimit", 10);

            targetManager = new TargetManager();

            InitSimpleTree();
        }

        public void UpdateGameWorldData(ref GameWorldUtils gameWorld)
        {
            data.GameWorld = currentGameWorld;
            targetManager.UpdateGameWorldData(ref gameWorld, currentGameWorld);
        }

        public void InitSimpleTree()
        {


            // Creation statique d'un Behaviour tree
            // en 2 étapes : 1 ajouter les nodes 2 les liers

            // creation des nodes
            Enemy[] enemies= new Enemy[0];
            Enemy chosenTarget = targetManager.ChooseTarget(enemies);

            data.Blackboard["chosenTarget"] = chosenTarget;
            var selector_0 = AddSelector();
            var sequence_0 = AddSequence();
            var sequence_1 = AddSequence();
            var node_0 = AddNode();
            var node_1 = AddNode();
            var condition_0 = new Condition();
            condition_0.AssignCondition(condition_0.CloseToEnemyTarget);
            var ShootToPredictedPos = AddNode();

            // liaison des nodes
            start.Attach(selector_0);
            selector_0.Attach(sequence_0);
            selector_0.Attach(sequence_1);
            sequence_0.Attach(condition_0);
            sequence_0.Attach(node_0);
            sequence_1.Attach(node_1);
            sequence_0.Attach(ShootToPredictedPos);

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

        public void Update(Enemy chosenTarget)
        {
            Enemy chosenTarget = GetChoosenTarget();
            Vector3 predictedPos = (Vector3)data.Blackboard["predictedTargetPos"];
            targetManager.TrackTargetMovement(chosenTarget.transform);
        }

        public Enemy GetChoosenTarget()
        {
            return (Enemy)data.Blackboard["chosenTarget"];
        }
    }
}
