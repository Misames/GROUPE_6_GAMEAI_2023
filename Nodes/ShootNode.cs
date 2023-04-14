using AI_BehaviorTree_AIGameUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_BehaviorTree_AIImplementation
{
    class ShootNode : Node
    {

        public ShootNode() { }

        public override State Evaluate(Data d)
        {

            AIActionLookAtPosition actionLookAt = new AIActionLookAtPosition();
            actionLookAt.Position = ((PlayerInformations)d.Blackboard[(int)BlackboardEnum.target]).Transform.Position;
            ((List<AIAction>)d.Blackboard[(int)BlackboardEnum.actionList]).Add(actionLookAt);
            ((List<AIAction>)d.Blackboard[(int)BlackboardEnum.actionList]).Add(new AIActionFire());

            return State.SUCCESS;
        }

    }
}
