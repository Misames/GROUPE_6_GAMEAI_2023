namespace AI_BehaviorTree_AIImplementation
{
    public class AlwaysSuccess : Node
    {
        public override State Evaluate()
        {
            return State.SUCCESS;
        }
    }

    public class AlwaysFailure : Node
    {
        public override State Evaluate()
        {
            return State.FAILURE;
        }
    }

    public class Inverter : Node
    {
        public override State Evaluate()
        {
            return parent.Evaluate() == State.SUCCESS ? State.FAILURE : State.SUCCESS;
        }
    }

    public class Repeat : Node
    {
        private readonly Node child;
        private readonly uint numRepeats;
        private uint currentRepeats;

        public Repeat(uint numRepeats)
        {
            this.numRepeats = numRepeats;
            currentRepeats = 0;
        }

        public override State Evaluate()
        {
            if (currentRepeats == numRepeats)
            {
                currentRepeats = 0;
                return State.SUCCESS;
            }

            State childState = child.Evaluate();

            if (childState != State.RUNNING)
                currentRepeats = 0;

            if (childState == State.RUNNING)
                return State.RUNNING;

            if (childState == State.FAILURE)
            {
                currentRepeats = 0;
                return State.FAILURE;
            }

            currentRepeats++;
            return Evaluate();
        }

    }

    public class Retry : Node
    {
        private readonly Node child;
        private readonly uint maxRetries;
        private uint numRetries;

        public Retry(uint maxRetries, uint numRetries)
        {
            this.maxRetries = maxRetries;
            this.numRetries = numRetries;
        }

        public override State Evaluate()
        {
            State childState = child.Evaluate();

            if (childState == State.SUCCESS)
            {
                numRetries = 0;
                return State.SUCCESS;
            }

            if (childState == State.SUCCESS)
                return State.RUNNING;

            if (numRetries < maxRetries)
            {
                numRetries++;
                return parent.Evaluate();
            }

            numRetries = 0;
            return State.FAILURE;
        }
    }
}
