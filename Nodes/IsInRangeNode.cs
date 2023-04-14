using AI_BehaviorTree_AIGameUtility;
using System.Collections.Generic;
using UnityEngine;

namespace AI_BehaviorTree_AIImplementation
{
    class IsInRangeNode : Node
    {
        private float range;
        private PlayerInformations target;

        public IsInRangeNode(float r, PlayerInformations p)
        {
            this.range = r;
            this.target = p;
        }

        public override State Evaluate(Data data)
        {
            float distance = Vector3.Distance(target.Transform.Position, (Vector3)data.Blackboard[(int)BlackboardEnum.myPlayerPosition]);
            return distance <= range ? State.SUCCESS : State.FAILURE;
        }

    }
}
