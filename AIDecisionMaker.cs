using System.Collections.Generic;
using AI_BehaviorTree_AIGameUtility;
using UnityEngine.Assertions;

namespace AI_BehaviorTree_AIImplementation
{
    public class AIDecisionMaker
    {
        private readonly BehaviourTree myBehaviorTree = new BehaviourTree();

        private int AIId = -1;

        public void SetAIId(int parAIId)
        {
            AIId = parAIId;
            InitializeBehaviorTree();
        }

        public string GetName() { return "WiZaR"; }

        public void SetAIGameWorldUtils(GameWorldUtils parGameWorldUtils)
        {
            BehaviourTree.Instance().data.GameWorld = parGameWorldUtils;
            List<PlayerInformations> playerInfos = parGameWorldUtils.GetPlayerInfosList();
            PlayerInformations myPlayerInfo = GetPlayerInfos(AIId, playerInfos);
            myBehaviorTree.data.Blackboard[BlackboardVariable.myPlayerPosition] = myPlayerInfo.Transform.Position;
            myBehaviorTree.data.Blackboard[BlackboardVariable.myPlayerId] = myPlayerInfo.PlayerId;
            myBehaviorTree.data.Blackboard[BlackboardVariable.targetPosition] = null;
            myBehaviorTree.data.Blackboard[BlackboardVariable.targetIsEnemy] = false;
            myBehaviorTree.data.Blackboard[BlackboardVariable.enemyProximityLimit] = 10;
        }

        public List<AIAction> ComputeAIDecision()
        {
            return myBehaviorTree.Compute();
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

        private void InitializeBehaviorTree()
        {
            myBehaviorTree.data.Blackboard.Add(BlackboardVariable.myPlayerPosition, null);
            myBehaviorTree.data.Blackboard.Add(BlackboardVariable.myPlayerId, null);
            myBehaviorTree.data.Blackboard.Add(BlackboardVariable.targetPosition, null);
            myBehaviorTree.data.Blackboard.Add(BlackboardVariable.targetIsEnemy, false);
            myBehaviorTree.data.Blackboard.Add(BlackboardVariable.enemyProximityLimit, 10);

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
    }
}
