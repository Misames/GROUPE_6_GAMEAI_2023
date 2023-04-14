﻿using System.Collections.Generic;
using AI_BehaviorTree_AIGameUtility;
using UnityEngine;

namespace AI_BehaviorTree_AIImplementation
{
    public class Condition : Node
    {
        public delegate State Del();

        public Del EvaluateCondition;

        public Condition()
        {
            nodeType = NodeType.CONDITION;
        }

        public void AssignCondition(Del conditionFunction)
        {
            EvaluateCondition = conditionFunction;
        }

        public State CloseToEnemyTarget()
        {
            Data data = BehaviourTree.Instance().data;
            List<PlayerInformations> playerInfos = data.GameWorld.GetPlayerInfosList();
            PlayerInformations target = null;
            foreach (PlayerInformations playerInfo in playerInfos) { }
            // test close to target using blackboard
            if ((bool)data.Blackboard[BlackboardVariable.targetIsEnemy] == true) { }
            return State.SUCCESS;
        }

        public State EnemyInSight()
        {
            Data data = BehaviourTree.Instance().data;
            //UnityEngine.Debug.LogError("enemyInSight");
            List<PlayerInformations> playerInfos = data.GameWorld.GetPlayerInfosList();
            Vector3 myPlayerPos = (Vector3)data.Blackboard[BlackboardVariable.myPlayerPosition];

            PlayerInformations target = null;
            
            foreach (PlayerInformations playerInfo in playerInfos)
            {
                if (playerInfo.PlayerId != (int)data.Blackboard[BlackboardVariable.myPlayerId])
                {
                    RaycastHit[] hits = Physics.RaycastAll(myPlayerPos, playerInfo.Transform.Position, 100f);

                    //UnityEngine.Debug.LogError("hit:" + playerInfo.PlayerId);
                    
                    foreach (RaycastHit hit in hits)
                    {
                        //UnityEngine.Debug.LogError(hit.collider.gameObject.name);
                    }

                }
            }
            return State.SUCCESS;
        }

        public State BulletIncoming()
        {
            Data data = BehaviourTree.Instance().data;
            UnityEngine.Debug.LogError("bulletIncoming");
            List<ProjectileInformations> playerInfos = data.GameWorld.GetProjectileInfosList();
            Vector3 myPlayerPos = (Vector3)data.Blackboard[BlackboardVariable.myPlayerPosition];
            PlayerInformations target = null;
            foreach (ProjectileInformations projectileinfo in playerInfos)
            {

                if (projectileinfo.PlayerId != (int)data.Blackboard[BlackboardVariable.myPlayerId])
                {
                    RaycastHit[] hits = Physics.RaycastAll(myPlayerPos, projectileinfo.Transform.Position, 100f);
                    foreach (RaycastHit hit in hits)
                    {
                        //UnityEngine.Debug.LogError(hit.collider.gameObject.name);
                    }
                }
            }
            return State.SUCCESS;
        }

        public override State Evaluate()
        {
            //UnityEngine.Debug.LogError("condition");

            state = EvaluateCondition();

            if (state == State.SUCCESS)
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
