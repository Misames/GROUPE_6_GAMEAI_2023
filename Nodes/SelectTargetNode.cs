using AI_BehaviorTree_AIGameUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AI_BehaviorTree_AIImplementation
{
    class SelectTargetNode : Node
    {

        public SelectTargetNode()
        { }

        public override State Evaluate(Data d)
        {
            List<PlayerInformations> temp = d.GameWorld.GetPlayerInfosList();
            List<PlayerInformations> playerInfos = new List<PlayerInformations>();

            foreach (PlayerInformations playerInfo in temp)
            {
                if (!playerInfo.IsActive)
                    continue;

                if (playerInfo.PlayerId == (int)d.Blackboard[(int)BlackboardEnum.myPlayerId])
                    continue;

                playerInfos.Add(playerInfo);
            }

            if(playerInfos.Count == 0) return State.FAILURE;

            PlayerInformations target = playerInfos[0];

            foreach(var v in playerInfos)
            {
                if (v.CurrentHealth < target.CurrentHealth) target = v;
            }

            d.Blackboard[(int)BlackboardEnum.target] = target;

            return State.SUCCESS;
        }

    }
}
