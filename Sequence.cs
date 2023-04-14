using System.Collections.Generic;

namespace AI_BehaviorTree_AIImplementation
{
    public class Sequence : Node
    {
        public Sequence() : base() { }

        public Sequence(List<Node> children) : base(children) { }

        public override State Evaluate(Data data)
        {
            bool anyChildIsRunning = false;

            foreach (Node node in children)
            {
                switch (node.Evaluate(data))
                {
                    case State.FAILURE:
                        state = State.FAILURE;
                        return state;
                    case State.SUCCESS:
                        continue;
                    case State.RUNNING:
                        anyChildIsRunning = true;
                        continue;
                    default:
                        state = State.SUCCESS;
                        return state;
                }
            }

            state = anyChildIsRunning ? State.RUNNING : State.SUCCESS;
            return state;
        }
    }
}
