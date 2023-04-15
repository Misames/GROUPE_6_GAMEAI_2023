namespace AI_BehaviorTree_AIImplementation
{
    public class Decorator_FORCE_FAILURE : Node
    {
        public override State Evaluate()
        {
            foreach (Node child in children)
            {
                child.Evaluate();
            }
            state = State.FAILURE;
            return state;
        }
    }

    public class Decorator_FORCE_SUCCESS : Node
    {
        public override State Evaluate()
        {
            foreach (Node child in children)
            {
                child.Evaluate();
            }
            state = State.SUCCESS;
            return state;
        }
    }

    public class Decorator_INVERTER : Node
    {
        public override State Evaluate()
        {
            foreach (Node child in children)
            {
                state = child.Evaluate();
            }
            state = state == State.SUCCESS ? State.FAILURE : State.SUCCESS;
            return state;
        }
    }
    public class Decorator_RETRY : Node
    {
        int nbRetry;

        public Decorator_RETRY(int nbRetry = 1) : base()
        {
            this.nbRetry = nbRetry;
        }
        public override State Evaluate()
        {
            foreach (Node child in children)
            {
                state = child.Evaluate();
            }

            uint i = 0;
            if (state == State.FAILURE)
            {
                while (i < nbRetry && state != State.SUCCESS)
                {
                    foreach (Node child in children)
                    {
                        state = child.Evaluate();
                    }
                    i++;
                }
            }
            return state;
        }
    }


    public class Decorator_REPEAT : Node
    {
        int nbRepeat;

        public Decorator_REPEAT(int nbRepeat = 1) : base()
        {
            this.nbRepeat = nbRepeat;
        }
        public override State Evaluate()
        {
            for (int i = 0; i < nbRepeat; i++)
            {
                foreach (Node child in children)
                {
                    state = child.Evaluate();
                }
            }
            return state;
        }
    }
}
