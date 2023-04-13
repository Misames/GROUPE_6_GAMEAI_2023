namespace AI_BehaviorTree_AIImplementation
{
    public class Action : Node
    {
        public delegate State Del();

        public Del DoAction;

        public Action() { name = "new action"; }

        public State ActionCombat()
        {
            // test close to target using blackboard
            return State.SUCCESS;
        }

        public override State Evaluate()
        {
            state = DoAction();

            if (state == State.RUNNING)
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
