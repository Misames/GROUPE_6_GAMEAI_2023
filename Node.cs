using System.Collections.Generic;

namespace AI_BehaviorTree_AIImplementation
{
    public enum State
    {
        FAILURE,
        RUNNING,
        SUCCESS,
    }

    public class Node
    {
        protected State state;
        protected Node parent;
        protected List<Node> children = new List<Node>();
        public Decorator decorator;

        public Node()
        {
            parent = null;
        }

        public Node(List<Node> children)
        {
            parent = null;
            foreach (Node child in children)
                Attach(child);
        }

        public void Attach(Node child)
        {
            child.parent = this;
            children.Add(child);
        }

        public State Evaluate()
        {
            if (decorator.type == DecoratorType.FORCE_FAILURE)
            {
                privateEvaluate();
                state = State.FAILURE;
                return state;
            }
            else if (decorator.type == DecoratorType.FORCE_SUCCESS)
            {
                privateEvaluate();
                state = State.SUCCESS;
                return state;
            }
            else if (decorator.type == DecoratorType.INVERTER)
            {
                state = privateEvaluate() == State.SUCCESS ? State.FAILURE : State.SUCCESS;
                return state;
            }
            else if (decorator.type == DecoratorType.RETRY)
            {
                state = privateEvaluate();
                uint i = 0;
                if(state == State.FAILURE)
                {
                    while(i<decorator.retry && state != State.SUCCESS)
                    {
                        state = privateEvaluate() ;
                        i++;
                    }
                }
                return state;
            }
            else if (decorator.type == DecoratorType.REPEAT)
            {
                for (int i = 0; i < decorator.repetition; i++)
                {
                    state = privateEvaluate();
                }
                return state;
            }
            
            state = privateEvaluate();
            return state;
        }

        /// <summary>
        /// Permet de tester l'état du Node <br />
        /// Ca nous sera utile au moment de l'éxecution des Nodes <br />
        /// </summary>
        public virtual State privateEvaluate()
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
