using System.Collections.Generic;
using AI_BehaviorTree_AIGameUtility;
using UnityEngine;

namespace AI_BehaviorTree_AIImplementation
{
    public class Condition : Node
    {
        public delegate State Del();

        public Del EvaluateCondition;

        public Condition(): base(){ }

        public void AssignCondition(Del conditionFunction)
        {
            EvaluateCondition = conditionFunction;
        }

        public State BonusAvailable()
        {
            Data data = BehaviourTree.Instance().data;

            List<BonusInformations> bonusInfos = data.GameWorld.GetBonusInfosList();

            if (bonusInfos.Count != 0)
            {
                data.Blackboard[BlackboardVariable.bonusExist] = true;
                return State.SUCCESS;
            }
            return State.FAILURE;
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
                        default:
                            continue;
                    }
                }
            }
            return state;
        }
    }
}
