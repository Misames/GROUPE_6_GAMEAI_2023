using System.Collections.Generic;

namespace AI_BehaviorTree_AIImplementation
{
    public class Sequence : Node
    {
        public Sequence() : base()
        {
            nodeType = NodeType.SEQUENCE;
        }

        public Sequence(List<Node> children) : base(children)
        {
            nodeType = NodeType.SEQUENCE;
        }

        public override State Evaluate()
        {
            UnityEngine.Debug.LogError("sequence");

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
                        break;
                    default:
                        state = State.SUCCESS;
                        return state;
                }
            }

            return state;
        }
    }
}
