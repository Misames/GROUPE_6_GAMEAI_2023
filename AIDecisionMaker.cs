using System;
using System.Collections.Generic;
using AI_BehaviorTree_AIGameUtility;
using UnityEngine;
using UnityEngine.Assertions;

namespace AI_BehaviorTree_AIImplementation
{
    public class AIDecisionMaker
    {
        private BehaviourTree myBehaviorTree = new BehaviourTree();

        /// <summary>
        /// Ne pas supprimer des fonctions, ou changer leur signature sinon la DLL ne fonctionnera plus
        /// Vous pouvez unitquement modifier l'intérieur des fonctions si nécessaire (par exemple le nom)
        /// ComputeAIDecision en fait partit
        /// </summary>
        private int AIId = -1;
        public GameWorldUtils AIGameWorldUtils = new GameWorldUtils();

        List<Enemy> enemiesList;
        Enemy[] enemiesArray;

        // Ne pas utiliser cette fonction, elle n'est utile que pour le jeu qui vous Set votre Id, si vous voulez votre Id utilisez AIId
        public void SetAIId(int parAIId) { AIId = parAIId; }

        // Vous pouvez modifier le contenu de cette fonction pour modifier votre nom en jeu
        public string GetName() { return "SamyAI"; }

        public void SetAIGameWorldUtils(GameWorldUtils parGameWorldUtils)
        {
            AIGameWorldUtils = parGameWorldUtils;
            UpdateBlackboard();
        }

        //Fin du bloc de fonction nécessaire (Attention ComputeAIDecision en fait aussi partit)

        private float BestDistanceToFire = 10.0f;

        public List<AIAction> ComputeAIDecision()
        {
            myBehaviorTree.UpdateGameWorldData(ref enemiesList);

            List<AIAction> actionList = new List<AIAction>();

            Enemy target = null;
            for (int i = 0; i < enemiesList.Count; i++)
            {
                if (enemiesList[i].IsActive && enemiesList[i].EnemyId != AIId)
                    target = enemiesList[i];
            }

            if (target == null)
                return actionList;

            Enemy myEnemy = GetEnemyInfos(AIId, enemiesArray);
            if (myEnemy == null)
                return actionList;

            if (Vector3.Distance(myEnemy.transform.position, target.transform.position) <= BestDistanceToFire)
            {
                AIActionMoveToDestination actionMove = new AIActionMoveToDestination();
                actionMove.Position = myEnemy.transform.position;
                actionList.Add(actionMove);
            }
            else
            {
                AIActionStopMovement actionStop = new AIActionStopMovement();
                actionList.Add(actionStop);
            }

            AIActionLookAtPosition actionLookAt = new AIActionLookAtPosition();
            actionLookAt.Position = target.transform.position;
            actionList.Add(actionLookAt);
            actionList.Add(new AIActionFire());

            return actionList;
        }

        private Enemy GetEnemyInfos(int aIId, Enemy[] enemiesArray)
        {
            throw new NotImplementedException();
        }

        private void UpdateBlackboard()
        {
            List<PlayerInformations> playerInfos = AIGameWorldUtils.GetPlayerInfosList();
            PlayerInformations myPlayerInfo = GetPlayerInfos(AIId, playerInfos);
            myBehaviorTree.data.Blackboard["myPlayerPosition"] = myPlayerInfo.Transform.Position;
            myBehaviorTree.data.Blackboard["myPlayerId"] = myPlayerInfo.PlayerId;
            myBehaviorTree.data.Blackboard["targetPosition"] = null;
            myBehaviorTree.data.Blackboard["targetIsEnemy"] = false;
            myBehaviorTree.data.Blackboard["enemyProximityLimit"] = 10;
            Enemy[] enemies = new Enemy[AIId];
            myBehaviorTree.data.Blackboard["chosenTarget"] = enemies[0];
            myBehaviorTree.data.Blackboard["enemiesList"] = enemiesList;
            myBehaviorTree.data.Blackboard["enemiesArray"] = enemiesArray;
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
