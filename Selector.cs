using System.Collections.Generic;

namespace AI_BehaviorTree_AIImplementation
{
    public class Selector : Node
    {
        public Selector() : base() { name = "new selector"; }

        public Selector(List<Node> children) : base(children) { name = "new selector"; }

        public override State Evaluate()
        {
            foreach (Node node in children)
            {
                switch (node.Evaluate())
                {
                    case State.FAILURE:
                        continue;
                    case State.SUCCESS:
                        state = State.SUCCESS;
                        return state;
                    case State.RUNNING:
                        state = State.RUNNING;
                        return state;
                    default:
                        continue;
                }
            }

            state = State.FAILURE;
            return state;
        }
    }
}
