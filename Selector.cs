using System.Collections.Generic;

namespace AI_BehaviorTree_AIImplementation
{
    public class Selector : Node
    {
        public Selector() : base()
        {
            nodeType = NodeType.SELECTOR;
        }

        public Selector(List<Node> children) : base(children)
        {
            nodeType = NodeType.SELECTOR;
        }

        public override State Evaluate()
        {
            UnityEngine.Debug.LogError("selector");
            foreach (Node child in children)
            {
                switch (child.Evaluate())
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
