using AI_BehaviorTree_AIGameUtility;
using System.Collections.Generic;
using UnityEngine;

namespace AI_BehaviorTree_AIImplementation
{
    public class Action : Node
    {
        public delegate State Del();

        public Del DoAction;

        public Action()
        {
            nodeType = NodeType.ACTION;
        }

        public void AssignAction(Del actionFunction)
        {
            DoAction = actionFunction;
        }

        public State ActionShoot()
        {
            //UnityEngine.Debug.LogError("Shoot");

            Data data = BehaviourTree.Instance().data;
            Vector3 target = (Vector3)data.Blackboard[BlackboardVariable.targetPosition];

            if (target == null)
                return State.FAILURE;

            AIActionLookAtPosition actionLookAt = new AIActionLookAtPosition();
            actionLookAt.Position = target;
            BehaviourTree.Instance().computeAction.Add(actionLookAt);
            BehaviourTree.Instance().computeAction.Add(new AIActionFire());
            return State.SUCCESS;
        }

        public State ActionFindTarget()
        {
            //UnityEngine.Debug.LogError("FindTarget");
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
            //UnityEngine.Debug.LogError("MoveToTarget");

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

        public State ActionDogeBullet()
        {
           
            Data data = BehaviourTree.Instance().data;
            if (data.Blackboard[BlackboardVariable.bulletIncoming] == null)
            {
                
                return State.FAILURE;
            }
            AIActionDash actionDash = new AIActionDash();
            //dash to the left of 
            if (Time.frameCount % 2 == 0)
            {
                actionDash.Direction = new Vector3(1, 0, 1);
            }
            else
            {
                actionDash.Direction = new Vector3(-1, 0, -1);
            }
            Vector3 dashDirection = actionDash.Direction;
            dashDirection.Normalize();
            actionDash.Direction = dashDirection;
            BehaviourTree.Instance().computeAction.Add(actionDash);
            return State.SUCCESS;
            
            
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
                    }
                }
            }

            return state;
        }
    }
}
