using AI_BehaviorTree_AIGameUtility;
using System.Collections.Generic;
using UnityEngine;

namespace AI_BehaviorTree_AIImplementation
{
    public class Action : Node
    {
        public delegate State Del();

        public Del DoAction;

        public Action() : base() { }

        public void AssignAction(Del actionFunction)
        {
            DoAction = actionFunction;
        }

        public State ActionShoot()
        {
            Data data = BehaviourTree.Instance().data;
            Vector3 target = (Vector3)data.Blackboard[BlackboardVariable.enemyTargetPosition];

            if (target == null)
                return State.FAILURE;

            AIActionLookAtPosition actionLookAt = new AIActionLookAtPosition();
            actionLookAt.Position = target;
            BehaviourTree.Instance().computeAction.Add(actionLookAt);
            BehaviourTree.Instance().computeAction.Add(new AIActionFire());

            return State.SUCCESS;
        }

        public State ActionFindShootTarget()
        {
            Data data = BehaviourTree.Instance().data;
            List<PlayerInformations> playerInfos = data.GameWorld.GetPlayerInfosList();

            PlayerInformations target = null;

            float minDistance = 100000;

            foreach (PlayerInformations playerInfo in playerInfos)
            {
                if (playerInfo.PlayerId != (int)data.Blackboard[BlackboardVariable.myPlayerId])
                {
                    float distance = Vector3.Distance((Vector3)data.Blackboard[BlackboardVariable.myPlayerPosition], playerInfo.Transform.Position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        target = playerInfo;
                    }
                }
            }

            if (target == null)
                return State.FAILURE;

            data.Blackboard[BlackboardVariable.enemyTargetPosition] = target.Transform.Position;
            return State.SUCCESS;
        }

        public State ActionFindMoveBonus()
        {
            Data data = BehaviourTree.Instance().data;
            List<BonusInformations> bonusInfos = data.GameWorld.GetBonusInfosList();
            List<PlayerInformations> playerInfos = data.GameWorld.GetPlayerInfosList();

            BonusInformations target = null;

            float minDistance = 100000;

            foreach (BonusInformations bonusInfo in bonusInfos)
            {

                float myDistance = Vector3.Distance((Vector3)data.Blackboard[BlackboardVariable.myPlayerPosition], bonusInfo.Position);

                bool closest = true;
                foreach (PlayerInformations playerInfo in playerInfos)
                {
                    if (playerInfo.PlayerId != (int)data.Blackboard[BlackboardVariable.myPlayerId])
                    {
                        float othersDistance = Vector3.Distance(bonusInfo.Position, playerInfo.Transform.Position);
                        if (othersDistance < myDistance)
                        {
                            closest = false;
                            break;
                        }
                    }
                }
                if (closest)
                {
                    target = bonusInfo;
                    break;
                }

                if (myDistance < minDistance)
                {
                    minDistance = myDistance;
                    target = bonusInfo;
                }

            }

            if (target == null)
                return State.FAILURE;

            data.Blackboard[BlackboardVariable.bonusTargetPosition] = target.Position;
            return State.SUCCESS;
        }

        public State ActionMoveDashToBonus()
        {
            Data data = BehaviourTree.Instance().data;

            if (data.Blackboard[BlackboardVariable.bonusTargetPosition] != null)
            {
                Vector3 direction = (Vector3)data.Blackboard[BlackboardVariable.bonusTargetPosition] - (Vector3)data.Blackboard[BlackboardVariable.myPlayerPosition];
                AIActionDash newAction = new AIActionDash(direction);
                BehaviourTree.Instance().computeAction.Add(newAction);
                return State.SUCCESS;

            }
            return State.FAILURE;
        }


            public State ActionMoveToTarget()
        {
            Data data = BehaviourTree.Instance().data;

            Vector3 target;

            Vector3 bonus = (Vector3)data.Blackboard[BlackboardVariable.bonusTargetPosition];
            Vector3 enemy = (Vector3)data.Blackboard[BlackboardVariable.enemyTargetPosition];

            if (bonus == null && enemy == null)
                return State.FAILURE;

            UnityEngine.Debug.LogError("bonus:"+ bonus + ", enemy:" + enemy);
            target = bonus;
            if ((bool) data.Blackboard[BlackboardVariable.bonusExist]==false)
                target = enemy;

            float distance = Vector3.Distance((Vector3)data.Blackboard[BlackboardVariable.myPlayerPosition], target);
            if (distance < 0.5)
                return State.SUCCESS;

            AIActionMoveToDestination newAction = new AIActionMoveToDestination(target);
            BehaviourTree.Instance().computeAction.Add(newAction);
            return State.RUNNING;
        }

        public override State privateEvaluate()
        {
            state = DoAction();
            if (state == State.RUNNING)
            {
                foreach (Node child in children)
                {
                    switch (child.Evaluate())
                    {
                        case State.FAILURE:
                            state = State.FAILURE;
                            return state;
                        case State.SUCCESS:
                            state = State.SUCCESS;
                            continue;
                        case State.RUNNING:
                            state = State.RUNNING;
                            return State.RUNNING;
                        default:
                            continue;
                    }
                }
            }
            return state;
        }
    }
}
