﻿using AI_BehaviorTree_AIGameUtility;
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
            Vector3 target = (Vector3)data.Blackboard[BlackboardVariable.targetPosition];

            if (target == null)
                return State.FAILURE;

            AIActionLookAtPosition actionLookAt = new AIActionLookAtPosition(target);
            BehaviourTree.Instance().computeAction.Add(actionLookAt);
            BehaviourTree.Instance().computeAction.Add(new AIActionFire());

            return State.SUCCESS;
        }

        public State ActionFindTarget()
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

            data.Blackboard[BlackboardVariable.targetPosition] = target.Transform.Position;
            return State.SUCCESS;
        }

        public State ActionMoveToTarget()
        {
            Data data = BehaviourTree.Instance().data;

            Vector3 target = (Vector3)data.Blackboard[BlackboardVariable.targetPosition];
            if (target == null)
                return State.FAILURE;

            float distance = Vector3.Distance((Vector3)data.Blackboard[BlackboardVariable.myPlayerPosition], target);
            if (distance < 1)
                return State.SUCCESS;

            AIActionMoveToDestination newAction = new AIActionMoveToDestination(target);
            BehaviourTree.Instance().computeAction.Add(newAction);
            return State.RUNNING;
        }

        public override State Evaluate()
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
