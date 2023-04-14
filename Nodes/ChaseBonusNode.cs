using System.Collections.Generic;
using AI_BehaviorTree_AIGameUtility;
using UnityEngine;

namespace AI_BehaviorTree_AIImplementation
{
    class ChaseBonusNode : Node
    {

        public ChaseBonusNode() { }

        public override State Evaluate(Data d)
        {

            #region Player 

            PlayerInformations player = null;

            AIActionMoveToDestination actionMove = new AIActionMoveToDestination();
            AIActionDash actionDash = new AIActionDash();

            foreach (var p in d.GameWorld.GetPlayerInfosList())
            {
                if (p.PlayerId == (int)d.Blackboard[(int)BlackboardEnum.myPlayerId])
                {
                    player = p;
                    break;
                }
            }

            if (player == null) return State.SUCCESS;

            #endregion

            List<BonusInformations> bonus = d.GameWorld.GetBonusInfosList();

            if (bonus == null || bonus.Count == 0) return State.FAILURE;

            var minDistance = Vector3.Distance(bonus[0].Position, (Vector3)d.Blackboard[(int)BlackboardEnum.myPlayerPosition]);
            var minBonus = bonus[0];

            foreach (var b in bonus)
            {
                var distanceFromBonus = Vector3.Distance(b.Position, (Vector3)d.Blackboard[(int)BlackboardEnum.myPlayerPosition]);

                if (distanceFromBonus < minDistance)
                {
                    minDistance = distanceFromBonus;
                    minBonus = b;
                }

            }

            actionMove.Position = minBonus.Position;

            if(player.IsDashAvailable)
            {
                actionDash.Direction = minBonus.Position;
                ((List<AIAction>)d.Blackboard[(int)BlackboardEnum.actionList]).Add(actionDash);
            }

            ((List<AIAction>)d.Blackboard[(int)BlackboardEnum.actionList]).Add(actionMove);
            d.Blackboard[(int)BlackboardEnum.bonusTarget] = minBonus;

            return State.SUCCESS;

        }


    }
}
