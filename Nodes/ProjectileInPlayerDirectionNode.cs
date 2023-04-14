using AI_BehaviorTree_AIGameUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace AI_BehaviorTree_AIImplementation
{
    class ProjectileInPlayerDirectionNode : Node
    {

        public ProjectileInPlayerDirectionNode()
        {
        }

        public override State Evaluate(Data d)
        {
            var allProjectiles = d.GameWorld.GetProjectileInfosList();

            List<ProjectileInformations> hitableProjectile = new List<ProjectileInformations>();

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


            foreach(var proj in allProjectiles)
            {
                var forwardProjectile = (proj.Transform.Rotation * Vector3.forward).normalized;

                var angle = Vector3.Angle(forwardProjectile, (player.Transform.Position - proj.Transform.Position).normalized);

                if (Mathf.Abs(angle) < 0.5f) hitableProjectile.Add(proj);
            }

            Vector3 directionToDodge = new Vector3(0, 0, 0);

            foreach (var p in hitableProjectile)
            {
                //float distance = Vector3.Distance(p.Transform.Position, (Vector3)d.Blackboard[(int)BlackboardEnum.myPlayerPosition]);

                Vector3 direction = (player.Transform.Position - p.Transform.Position).normalized;

                float angle = 90f;

                Quaternion rot = Quaternion.AngleAxis(angle, Vector3.up);

                directionToDodge += rot * direction;

            }

            directionToDodge /= hitableProjectile.Count;

            if(hitableProjectile.Count > 0)
            {
                if (player.IsDashAvailable)
                {
                    actionDash.Direction = player.Transform.Rotation * Vector3.right;
                    ((List<AIAction>)d.Blackboard[(int)BlackboardEnum.actionList]).Add(actionDash);

                    return State.FAILURE;
                }
                else
                {
                    float distance = Vector3.Distance(directionToDodge, (Vector3)d.Blackboard[(int)BlackboardEnum.myPlayerPosition]);

                    NavMesh.SamplePosition(player.Transform.Position, out var hit, distance, NavMesh.AllAreas);

                    actionMove.Position = hit.position;
                    ((List<AIAction>)d.Blackboard[(int)BlackboardEnum.actionList]).Add(actionMove);

                    return State.FAILURE;
                }
            }

            return State.FAILURE;
        }

    }
}
