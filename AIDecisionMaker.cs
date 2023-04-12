using System.Collections.Generic;
using AI_BehaviorTree_AIGameUtility;
using UnityEngine;
using UnityEngine.Assertions;

namespace AI_BehaviorTree_AIImplementation
{
    public class AIDecisionMaker
    {

        /// <summary>
        /// Ne pas supprimer des fonctions, ou changer leur signature sinon la DLL ne fonctionnera plus
        /// Vous pouvez unitquement modifier l'intérieur des fonctions si nécessaire (par exemple le nom)
        /// ComputeAIDecision en fait partit
        /// </summary>
        private int AIId = -1;
        public GameWorldUtils AIGameWorldUtils = new GameWorldUtils();

        // Behavior tree
        private BehaviourTree myBehaviorTree;

        // Ne pas utiliser cette fonction, elle n'est utile que pour le jeu qui vous Set votre Id, si vous voulez votre Id utilisez AIId
        public void SetAIId(int parAIId) { AIId = parAIId; }

        // Vous pouvez modifier le contenu de cette fonction pour modifier votre nom en jeu
        public string GetName() { return "MichelAI"; }

        public void SetAIGameWorldUtils(GameWorldUtils parGameWorldUtils)
        {
            AIGameWorldUtils = parGameWorldUtils;
            myBehaviorTree = new BehaviourTree();
            InitializeBehaviorTree();
        }

        //Fin du bloc de fonction nécessaire (Attention ComputeAIDecision en fait aussi partit)

        private float BestDistanceToFire = 10.0f;

        public List<AIAction> ComputeAIDecision()
        {
            myBehaviorTree.UpdateGameWorldData(ref AIGameWorldUtils);

            List<AIAction> actionList = new List<AIAction>();

            List<PlayerInformations> playerInfos = AIGameWorldUtils.GetPlayerInfosList();

            PlayerInformations target = null;
            foreach (PlayerInformations playerInfo in playerInfos)
            {
                if (!playerInfo.IsActive)
                    continue;

                if (playerInfo.PlayerId == AIId)
                    continue;

                target = playerInfo;
                break;
            }

            if (target == null)
                return actionList;

            PlayerInformations myPlayerInfo = GetPlayerInfos(AIId, playerInfos);
            if (myPlayerInfo == null)
                return actionList;

            if (Vector3.Distance(myPlayerInfo.Transform.Position, target.Transform.Position) < BestDistanceToFire)
            {
                AIActionStopMovement actionStop = new AIActionStopMovement();

                actionList.Add(actionStop);
            }
            else
            {
                AIActionMoveToDestination actionMove = new AIActionMoveToDestination();
                actionMove.Position = target.Transform.Position;
                actionList.Add(actionMove);
            }

            AIActionLookAtPosition actionLookAt = new AIActionLookAtPosition();
            actionLookAt.Position = target.Transform.Position;
            actionList.Add(actionLookAt);
            actionList.Add(new AIActionFire());

            return actionList;
        }

        private void InitializeBehaviorTree()
        {
            // Michel
            List<PlayerInformations> playerInfos = AIGameWorldUtils.GetPlayerInfosList();

            PlayerInformations myPlayerInfo = GetPlayerInfos(AIId, playerInfos);
            myBehaviorTree.data.Blackboard.Add("myPlayerPosition", myPlayerInfo.Transform.Position);
            myBehaviorTree.data.Blackboard.Add("myPlayerId", myPlayerInfo.PlayerId);
            myBehaviorTree.data.Blackboard.Add("targetPosition", null);
            myBehaviorTree.data.Blackboard.Add("targetIsEnemy", false);
            myBehaviorTree.data.Blackboard.Add("enemyProximityLimit", 10);

            // Creation statique d'un Behaviour tree
            // en 2 étapes : 1 ajouter les nodes 2 les liers

            // creation des nodes
            var selector_0 = myBehaviorTree.AddSelector();
            var sequence_0 = myBehaviorTree.AddSequence();
            var sequence_1 = myBehaviorTree.AddSequence();
            var node_0 = myBehaviorTree.AddNode();
            var node_1 = myBehaviorTree.AddNode();
            var condition_0 = new Condition();
            condition_0.AssignCondition(condition_0.CloseToEnemyTarget);
            
            // liaison des nodes
            myBehaviorTree.start.Attach(selector_0);
            selector_0.Attach(sequence_0);
            selector_0.Attach(sequence_1);
            sequence_0.Attach(condition_0);
            sequence_0.Attach(node_0);
            sequence_1.Attach(node_1);
        }

        public PlayerInformations GetPlayerInfos(int parPlayerId, List<PlayerInformations> parPlayerInfosList)
        {
            foreach (PlayerInformations playerInfo in parPlayerInfosList)
            {
                if (playerInfo.PlayerId == parPlayerId)
                    return playerInfo;
            }

            Assert.IsTrue(false, "GetPlayerInfos : PlayerId not Found");
            return null;
        }
    }
}
