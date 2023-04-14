using AI_BehaviorTree_AIGameUtility;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AI_BehaviorTree_AIImplementation
{
    class ChaseNode : Node
    {
        private PlayerInformations targetPlayer;

        public ChaseNode(PlayerInformations tp)
        {
            this.targetPlayer = tp;
        }

        public override State Evaluate(Data d)
        {


            float distance = Vector3.Distance(targetPlayer.Transform.Position, (Vector3)d.Blackboard[(int)BlackboardEnum.myPlayerPosition]);

            if (distance > 30f)
            {
                AIActionMoveToDestination actionMove = new AIActionMoveToDestination();
                actionMove.Position = targetPlayer.Transform.Position;

                ((List<AIAction>)d.Blackboard[(int)BlackboardEnum.actionList]).Add(actionMove);

                return State.SUCCESS;
            }
            else
            {
                AIActionMoveToDestination actionMove = new AIActionMoveToDestination();
                actionMove.Position = targetPlayer.Transform.Position - new Vector3(-15,0,0);
                ((List<AIAction>)d.Blackboard[(int)BlackboardEnum.actionList]).Add(actionMove);

                return State.SUCCESS;
            }
        }

    }
}
