using System.Collections.Generic;
using AI_BehaviorTree_AIGameUtility;
using UnityEngine;

namespace AI_BehaviorTree_AIImplementation
{
    class ChaseBonusNode : Node
    {

        public ChaseBonusNode() { }

        public override State Evaluate(Data data)
        {
            List<BonusInformations> bonus = data.GameWorld.GetBonusInfosList();

            var minDistance = Vector3.Distance(bonus[0].Position, (Vector3)data.Blackboard[(int)BlackboardEnum.myPlayerPosition]);
            var minVector = bonus[0].Position;

            foreach (var b in bonus)
            {

                var distanceFromBonus = Vector3.Distance(b.Position, (Vector3)data.Blackboard[(int)BlackboardEnum.myPlayerPosition]);

                if (distanceFromBonus < minDistance)
                {
                    minDistance = distanceFromBonus;
                    minVector = b.Position;
                }
                
            }

            AIActionMoveToDestination actionMove = new AIActionMoveToDestination();
            actionMove.Position = minVector;

            ((List<AIAction>)data.Blackboard[(int)BlackboardEnum.actionList]).Add(actionMove);

            return State.SUCCESS;

        }


    }
}
