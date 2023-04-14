using System.Collections.Generic;

namespace AI_BehaviorTree_AIImplementation
{
    public class Selector : Node
    {
        public Selector() : base() { }

        public Selector(List<Node> children) : base(children) { }

        public override State Evaluate(Data data)
        {
            foreach (Node node in children)
            {
                switch (node.Evaluate(data))
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
