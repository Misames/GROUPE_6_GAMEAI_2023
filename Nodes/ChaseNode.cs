using AI_BehaviorTree_AIGameUtility;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AI_BehaviorTree_AIImplementation
{
    class ChaseNode : Node
    {

        public ChaseNode()
        {
        }

        public override State Evaluate(Data d)
        {

            var targetPlayer = (PlayerInformations)d.Blackboard[(int)BlackboardEnum.target];

            float distance = Vector3.Distance(targetPlayer.Transform.Position, (Vector3)d.Blackboard[(int)BlackboardEnum.myPlayerPosition]);

            if (distance > 20f)
            {
                AIActionMoveToDestination actionMove = new AIActionMoveToDestination();
                actionMove.Position = targetPlayer.Transform.Position;

                ((List<AIAction>)d.Blackboard[(int)BlackboardEnum.actionList]).Add(actionMove);

                return State.SUCCESS;
            }
            else
            {
                var startpos = (Vector3)d.Blackboard[(int)BlackboardEnum.myPlayerPosition];

                AIActionMoveToDestination actionMove = new AIActionMoveToDestination();

                PlayerInformations player = new PlayerInformations();

                foreach (var p in d.GameWorld.GetPlayerInfosList())
                {
                    if (p.PlayerId == (int)d.Blackboard[(int)BlackboardEnum.myPlayerId])
                    {
                        player = p;
                        break;
                    }
                }

                bool rotateRight = (Vector3)d.Blackboard[(int)BlackboardEnum.playerRotation] == Vector3.right ? true : false;

                if (Physics.Raycast(player.Transform.Position, (Vector3)d.Blackboard[(int)BlackboardEnum.playerRotation], out var hit, 5.0f))
                {
                    if (d.GameWorld.PlayerLayerMask != (d.GameWorld.PlayerLayerMask | (1 << hit.collider.gameObject.layer)) &&
                        d.GameWorld.BonusLayerMask != (d.GameWorld.BonusLayerMask | (1 << hit.collider.gameObject.layer)) &&
                        d.GameWorld.ProjectileLayerMask != (d.GameWorld.ProjectileLayerMask | (1 << hit.collider.gameObject.layer))) 
                    {
                        rotateRight = !rotateRight;

                        if (Physics.Raycast(player.Transform.Position, (Vector3)d.Blackboard[(int)BlackboardEnum.playerRotation], out var h, 5.0f))
                        {
                            if (d.GameWorld.PlayerLayerMask != (d.GameWorld.PlayerLayerMask | (1 << h.collider.gameObject.layer)) &&
                                d.GameWorld.BonusLayerMask != (d.GameWorld.BonusLayerMask | (1 << h.collider.gameObject.layer)) &&
                                d.GameWorld.ProjectileLayerMask != (d.GameWorld.ProjectileLayerMask | (1 << h.collider.gameObject.layer)))
                            {
                                actionMove.Position = player.Transform.Position + new Vector3(-15,0,0);
                                ((List<AIAction>)d.Blackboard[(int)BlackboardEnum.actionList]).Add(actionMove);

                                return State.SUCCESS;
                            }
                        }
                    }
                }

                float angle = rotateRight ? 80f : 280f;

                Vector3 targetDirection = (player.Transform.Position - targetPlayer.Transform.Position).normalized;

                Quaternion rot = Quaternion.AngleAxis(angle, Vector3.up);

                Vector3 rotatedVector = rot * targetDirection;

                actionMove.Position = player.Transform.Position + rotatedVector;
                ((List<AIAction>)d.Blackboard[(int)BlackboardEnum.actionList]).Add(actionMove);

                return State.SUCCESS;
            }
        }

    }
}
