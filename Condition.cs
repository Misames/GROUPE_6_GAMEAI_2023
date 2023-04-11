namespace AI_BehaviorTree_AIImplementation
{
    public class Condition : Node
    {
        public delegate State Del();

        public Del EvaluateCondition;

        public Condition(Del callback) {
            EvaluateCondition = callback;
        }
        
        public static State CloseToTarget()
        {
            // test close to target using blackboard
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
