using AI_BehaviorTree_AIGameUtility;
using System.Collections.Generic;

namespace AI_BehaviorTree_AIImplementation
{
    public class Condition : Node
    {
        public delegate State Del();

        public Del EvaluateCondition;

        public Condition() {
        }

        public void AssignCondition(Del conditionFunction)
        {
            EvaluateCondition = conditionFunction;
        }
        
        public State CloseToEnemyTarget()
        {
            List<PlayerInformations> playerInfos = data.GameWorld.GetPlayerInfosList();

            PlayerInformations target = null;
            foreach (PlayerInformations playerInfo in playerInfos)
            { 
                            
            }
            // test close to target using blackboard
            if ((bool)data.Blackboard["targetIsEnemy"] == true)
            {

            }
            return State.SUCCESS;
        }

        public override State Evaluate()
        {
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
