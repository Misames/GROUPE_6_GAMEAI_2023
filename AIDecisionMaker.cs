using System.Collections.Generic;
using AI_BehaviorTree_AIGameUtility;
using UnityEngine.Assertions;

namespace AI_BehaviorTree_AIImplementation
{
    public class AIDecisionMaker
    {
        private readonly BehaviourTree myBehaviorTree = new BehaviourTree();

        /// <summary>
        /// Ne pas supprimer des fonctions, ou changer leur signature sinon la DLL ne fonctionnera plus
        /// Vous pouvez unitquement modifier l'intérieur des fonctions si nécessaire (par exemple le nom)
        /// ComputeAIDecision en fait partit
        /// </summary>
        private int AIId = -1;

        public GameWorldUtils AIGameWorldUtils = new GameWorldUtils();

        // Ne pas utiliser cette fonction, elle n'est utile que pour le jeu qui vous Set votre Id, si vous voulez votre Id utilisez AIId
        public void SetAIId(int parAIId)
        {
            AIId = parAIId;
            InitializeBehaviorTree();
        }

        // Vous pouvez modifier le contenu de cette fonction pour modifier votre nom en jeu
        public string GetName() { return "WiZaR"; }

        public void SetAIGameWorldUtils(GameWorldUtils parGameWorldUtils)
        {
            AIGameWorldUtils = parGameWorldUtils;
            UpdateBlackboard();
        }

        // Fin du bloc de fonction nécessaire (Attention ComputeAIDecision en fait aussi partit)
        public List<AIAction> ComputeAIDecision()
        {
            BehaviourTree.Instance().UpdateGameWorldData(AIGameWorldUtils);
            return myBehaviorTree.Compute();
        }

        private void InitializeBehaviorTree()
        {
            myBehaviorTree.data.Blackboard.Add(BlackboardVariable.myPlayerPosition, null);
            myBehaviorTree.data.Blackboard.Add(BlackboardVariable.myPlayerId, null);
            myBehaviorTree.data.Blackboard.Add(BlackboardVariable.targetPosition, null);
            myBehaviorTree.data.Blackboard.Add(BlackboardVariable.targetIsEnemy, false);
            myBehaviorTree.data.Blackboard.Add(BlackboardVariable.enemyProximityLimit, 10);

            // creation des nodes
            Selector selectorStart = new Selector();
            Sequence sequenceCombat = new Sequence();
            Action actionFindTarget = new Action();
            Action actionShoot = new Action();
            Action actionMoveToTarget = new Action();

            actionFindTarget.AssignAction(actionFindTarget.ActionFindTarget);
            actionShoot.AssignAction(actionShoot.ActionShoot);
            actionMoveToTarget.AssignAction(actionMoveToTarget.ActionMoveToTarget);

            sequenceCombat.Attach(actionFindTarget);
            sequenceCombat.Attach(actionShoot);
            sequenceCombat.Attach(actionMoveToTarget);

            selectorStart.Attach(sequenceCombat);
            myBehaviorTree.start.Attach(selectorStart);
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
