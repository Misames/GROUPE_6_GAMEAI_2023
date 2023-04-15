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
            BehaviourTree.Instance().UpdateGameWorldData(AIGameWorldUtils);
            List<AIAction> actionList = new List<AIAction>();
            actionList = myBehaviorTree.Compute();
            return actionList;
        }

        private void InitializeBehaviorTree()
        {
            myBehaviorTree.data.Blackboard.Add(BlackboardVariable.myPlayerPosition, null);
            myBehaviorTree.data.Blackboard.Add(BlackboardVariable.myPlayerId, null);
            myBehaviorTree.data.Blackboard.Add(BlackboardVariable.bonusTargetPosition, null);
            myBehaviorTree.data.Blackboard.Add(BlackboardVariable.enemyTargetPosition, null);
            myBehaviorTree.data.Blackboard.Add(BlackboardVariable.enemyProximityLimit, 10);
            myBehaviorTree.data.Blackboard.Add(BlackboardVariable.bonusExist, false);

            // creation des nodes
            Selector selectorStart = new Selector();
            Sequence sequenceCombat = new Sequence();

            Condition conditionBonusAvailable = new Condition();

            Action actionFindMoveBonus = new Action();
            Action actionFindShootTarget = new Action();
            Action actionShoot = new Action();
            Action actionMoveToTarget = new Action();
            Action actionDashBonus = new Action();

            Decorator_FORCE_FAILURE forceFailureBonus = new Decorator_FORCE_FAILURE();

            conditionBonusAvailable.AssignCondition(conditionBonusAvailable.BonusAvailable);

            actionFindMoveBonus.AssignAction(actionFindMoveBonus.ActionFindMoveBonus);
            actionFindShootTarget.AssignAction(actionFindShootTarget.ActionFindShootTarget);
            actionShoot.AssignAction(actionShoot.ActionShoot);
            actionMoveToTarget.AssignAction(actionMoveToTarget.ActionMoveToTarget);
            actionDashBonus.AssignAction(actionDashBonus.ActionMoveDashToBonus);

            conditionBonusAvailable.Attach(actionFindMoveBonus);
            conditionBonusAvailable.Attach(actionDashBonus);

            forceFailureBonus.Attach(conditionBonusAvailable);


            sequenceCombat.Attach(actionFindShootTarget);
            sequenceCombat.Attach(actionShoot);
            sequenceCombat.Attach(actionMoveToTarget);

            selectorStart.Attach(forceFailureBonus);
            selectorStart.Attach(sequenceCombat);

            myBehaviorTree.start.Attach(selectorStart);
        }

        private void UpdateBlackboard()
        {
            List<PlayerInformations> playerInfos = AIGameWorldUtils.GetPlayerInfosList();
            PlayerInformations myPlayerInfo = GetPlayerInfos(AIId, playerInfos);
            myBehaviorTree.data.Blackboard[BlackboardVariable.myPlayerPosition] = myPlayerInfo.Transform.Position;
            myBehaviorTree.data.Blackboard[BlackboardVariable.myPlayerId] = myPlayerInfo.PlayerId;
            myBehaviorTree.data.Blackboard[BlackboardVariable.bonusTargetPosition] = null;
            myBehaviorTree.data.Blackboard[BlackboardVariable.enemyTargetPosition] = null;
            myBehaviorTree.data.Blackboard[BlackboardVariable.bonusExist] = false;


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
