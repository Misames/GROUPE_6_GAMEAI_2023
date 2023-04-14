using System.Collections.Generic;
using AI_BehaviorTree_AIGameUtility;
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

        // Ne pas utiliser cette fonction, elle n'est utile que pour le jeu qui vous Set votre Id, si vous voulez votre Id utilisez AIId
        public void SetAIId(int parAIId) {
            AIId = parAIId;
            InitializeBehaviorTree();
        }

        // Vous pouvez modifier le contenu de cette fonction pour modifier votre nom en jeu
        public string GetName() { return "MichelAI"; }

        public void SetAIGameWorldUtils(GameWorldUtils parGameWorldUtils)
        {
            AIGameWorldUtils = parGameWorldUtils;
            UpdateBlackboard();
        }

        // Fin du bloc de fonction nécessaire (Attention ComputeAIDecision en fait aussi partit)

        public List<AIAction> ComputeAIDecision()
        {
            List<AIAction> actionList = new List<AIAction>();
            myBehaviorTree.UpdateGameWorldData(AIGameWorldUtils);
            myBehaviorTree.Compute();
            return actionList;
        }

        private void InitializeBehaviorTree()
        {
            myBehaviorTree.data.Blackboard.Add("myPlayerPosition", null);
            myBehaviorTree.data.Blackboard.Add("myPlayerId", null);
            myBehaviorTree.data.Blackboard.Add("targetPosition", null);
            myBehaviorTree.data.Blackboard.Add("targetIsEnemy", false);
            myBehaviorTree.data.Blackboard.Add("enemyProximityLimit", 10);

            // creation des nodes
            Selector selector_0 = myBehaviorTree.AddSelector();
            Condition condition_0 = myBehaviorTree.AddCondition();
            condition_0.AssignCondition(condition_0.EnemyInSight);
            
            // attach nodes
            selector_0.Attach(condition_0);
            myBehaviorTree.start.Attach(selector_0);
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
