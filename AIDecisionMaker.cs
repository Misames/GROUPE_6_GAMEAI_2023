using System.Collections.Generic;
using AI_BehaviorTree_AIGameUtility;
using UnityEngine;
using UnityEngine.Assertions;
using System;

namespace AI_BehaviorTree_AIImplementation
{
    public class AIDecisionMaker
    {
        private BehaviourTree myBehaviorTree = new BehaviourTree();

        // Debug values
        const int debug_mode = 0;
        DateTime timer = new DateTime();
        /// <summary>
        /// Ne pas supprimer des fonctions, ou changer leur signature sinon la DLL ne fonctionnera plus
        /// Vous pouvez unitquement modifier l'intérieur des fonctions si nécessaire (par exemple le nom)
        /// ComputeAIDecision en fait partit
        /// </summary>
        private int AIId = -1;
        public GameWorldUtils AIGameWorldUtils = new GameWorldUtils();

        // Ne pas utiliser cette fonction, elle n'est utile que pour le jeu qui vous Set votre Id, si vous voulez votre Id utilisez AIId
        public void SetAIId(int parAIId) {
            if (debug_mode == 2)
            {
                UnityEngine.Debug.LogError("SetAIId "+ parAIId);
            }
            AIId = parAIId;
            InitializeBehaviorTree();

        }

        // Vous pouvez modifier le contenu de cette fonction pour modifier votre nom en jeu
        public string GetName() {
            if (debug_mode == 2)
            {
                UnityEngine.Debug.LogError("GetName " + "Michel");
            }

            return "MichelAI"; 
        }

        public void SetAIGameWorldUtils(GameWorldUtils parGameWorldUtils)
        {
            AIGameWorldUtils = parGameWorldUtils;
            UpdateBlackboard();
        }

        //Fin du bloc de fonction nécessaire (Attention ComputeAIDecision en fait aussi partit)


        public List<AIAction> ComputeAIDecision()
        {
            if (debug_mode == 1)
            {
                timer = DateTime.Now;
            }

            BehaviourTree.Instance().UpdateGameWorldData(AIGameWorldUtils);

            List<AIAction> actionList = new List<AIAction>();

            actionList = myBehaviorTree.Compute();
            /*
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
            */
            if (debug_mode == 1)
            {
                UnityEngine.Debug.LogError("debug_ComputeTime: " + DateTime.Now.Subtract(timer).TotalMilliseconds);
            }

            return actionList;
        }

        private void InitializeBehaviorTree()
        {
            myBehaviorTree.data.Blackboard.Add(BlackboardVariable.myPlayerPosition, null);
            myBehaviorTree.data.Blackboard.Add(BlackboardVariable.myPlayerId, null);
            myBehaviorTree.data.Blackboard.Add(BlackboardVariable.targetPosition, null);
            myBehaviorTree.data.Blackboard.Add(BlackboardVariable.targetIsEnemy, false);
            myBehaviorTree.data.Blackboard.Add(BlackboardVariable.enemyProximityLimit, 10);

            // Creation statique d'un Behaviour tree
            // en 2 étapes : 1 ajouter les nodes 2 les liers

            // creation des nodes
            Selector selectorStart = myBehaviorTree.AddSelector();
            Sequence sequenceCombat = myBehaviorTree.AddSequence();
            //Condition conditionEnemyInSight = myBehaviorTree.AddCondition();
            Action actionFindTarget = myBehaviorTree.AddAction();
            Action actionShoot = myBehaviorTree.AddAction();
            Action actionMoveToTarget = myBehaviorTree.AddAction();


            //conditionEnemyInSight.AssignCondition(conditionEnemyInSight.EnemyInSight);
            actionFindTarget.AssignAction(actionFindTarget.ActionFindTarget);
            actionShoot.AssignAction(actionShoot.ActionShoot);
            actionMoveToTarget.AssignAction(actionMoveToTarget.ActionMoveToTarget);

            sequenceCombat.Attach(actionFindTarget);
            sequenceCombat.Attach(actionShoot);
            sequenceCombat.Attach(actionMoveToTarget);

            //selectorStart.Attach(conditionEnemyInSight);
            selectorStart.Attach(sequenceCombat);

            myBehaviorTree.start.Attach(selectorStart);


            //action_0.AssignAction();
            

            //UnityEngine.Debug.LogError("arbre fini ! UwU");
        }

        private void UpdateBlackboard()
        {
            List<PlayerInformations> playerInfos = AIGameWorldUtils.GetPlayerInfosList();
            PlayerInformations myPlayerInfo = GetPlayerInfos(AIId, playerInfos);
            myBehaviorTree.data.Blackboard[BlackboardVariable.myPlayerPosition] = myPlayerInfo.Transform.Position;
            myBehaviorTree.data.Blackboard[BlackboardVariable.myPlayerId] = myPlayerInfo.PlayerId;
            myBehaviorTree.data.Blackboard[BlackboardVariable.targetPosition] = null;
            myBehaviorTree.data.Blackboard[BlackboardVariable.targetIsEnemy] = false;
            myBehaviorTree.data.Blackboard[BlackboardVariable.enemyProximityLimit] = 10;
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
