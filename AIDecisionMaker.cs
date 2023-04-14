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
        private Enemy myEnemy;

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
            List<Enemy> possibleTargets = new List<Enemy>();

            for (int i = 0; i < enemiesList.Count; i++)
            {
                if (enemiesList[i].IsActive && enemiesList[i].EnemyId != AIId)
                    possibleTargets.Add(enemiesList[i]);
            }

            if (possibleTargets.Count == 0)
                return actionList;  // Pas de cible, on retourne une liste vide 

            Enemy chosenTarget = FindNearestActiveEnemy(possibleTargets);

            Enemy myEnemy = GetEnemyInfos(AIId, enemiesArray);
            if (myEnemy == null)
                return actionList;

            // Vérification des conditions pour atteindre et tirer sur la cible 
            if (Vector3.Distance(myEnemy.transform.transform.position, chosenTarget.transform.transform.position) < BestDistanceToFire)
            {
                // Déplacement vers la cible 
                AIActionMoveToDestination actionMove = new AIActionMoveToDestination();
                actionMove.Position = chosenTarget.transform.transform.position;
                actionList.Add(actionMove);

                // Orientation vers la cible 
                AIActionLookAtPosition actionLookAt = new AIActionLookAtPosition();
                actionLookAt.Position = chosenTarget.transform.transform.position;
                actionList.Add(actionLookAt);

                // Tir sur la cible atteinte 
                actionList.Add(new AIActionFire());
            }
            else
            {
                // Stop si la cible n'est pas à portée 
                AIActionStopMovement actionStop = new AIActionStopMovement();
                actionList.Add(actionStop);
            }

            return actionList;
        }

        private Enemy FindNearestActiveEnemy(List<Enemy> enemies)
        {
            myEnemy= GetEnemyInfos(AIId, enemiesArray);
            Enemy nearestEnemy = enemies[0];
            float nearestDistance = Vector3.Distance(myEnemy.transform.transform.position,nearestEnemy.transform.transform.position);

            for (int i = 1; i < enemies.Count; i++)
            {
                float distanceToEnemy = Vector3.Distance(myEnemy.transform.transform.position,enemies[i].transform.transform.position);

                if (distanceToEnemy < nearestDistance && enemies[i].IsActive)
                {
                    nearestDistance = distanceToEnemy;
                    nearestEnemy = enemies[i];
                }
            }

            return nearestEnemy;
        }

        private Enemy GetEnemyInfos(int aIId, Enemy[] enemiesArray)
        {
            for (int i = 0; i < enemiesArray.Length; i++)
            {
                if (enemiesArray[i].EnemyId == aIId)
                    return enemiesArray[i];
            }
            return null;
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
