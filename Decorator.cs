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
            switch(parent.Evaluate())
            {
                case State.SUCCESS:
                    return State.FAILURE;
                case State.FAILURE:
                    return State.SUCCESS;
                default:
                    return State.RUNNING;
            }
        }
    }

    public class Repeat : Node
    {
        private readonly int numRepeats;
        private uint currentRepeats;

        public Repeat(int numRepeats = 1)
        {
            this.numRepeats = numRepeats;
            currentRepeats = 0;
        }

        public override State Evaluate()
        {
            // Si on a fini toutes les répétitions, renvoyer Success
            if (currentRepeats == numRepeats)
            {
                currentRepeats = 0;
                return State.SUCCESS;
            }

            // Évaluer le nœud parent
            State parentState = parent.Evaluate();

            // Si le nœud parent renvoie Success ou Failure, réinitialiser le compteur de répétitions
            if (parentState != State.RUNNING)
                currentRepeats = 0;

            // Si le nœud parent renvoie Running, répéter
            if (parentState == State.RUNNING)
                return State.RUNNING;

            // Si le nœud parent renvoie Failure, renvoyer Failure
            if (parentState == State.FAILURE)
            {
                currentRepeats = 0;
                return State.FAILURE;
            }

            // Si le nœud parent renvoie Success, répéter jusqu'à ce qu'on atteigne le nombre de répétitions souhaité
            currentRepeats++;
            return Evaluate();
        }
    }

    public class Retry : Node
    {
        private readonly uint maxRetries;
        private uint numRetries;

        public Retry(uint maxRetries = 1, uint numRetries = 0)
        {
            this.maxRetries = maxRetries;
            this.numRetries = numRetries;
        }

        public override State Evaluate()
        {
            State parentSate = parent.Evaluate();

            // Si le nœud parent réussit, réinitialiser le compteur de tentatives et renvoyer Success
            if (parentSate == State.SUCCESS)
            {
                numRetries = 0;
                return State.SUCCESS;
            }

            // Si le nœud parent est en cours d'exécution, renvoyer Running
            if (parentSate == State.SUCCESS)
                return State.RUNNING;

            // Si le nombre de tentatives est inférieur au nombre maximal de tentatives, réessayer
            if (numRetries < maxRetries)
            {
                numRetries++;
                return parent.Evaluate();
            }

            // Si le nombre de tentatives atteint le maximum, réinitialiser le compteur de tentatives et renvoyer Failure
            numRetries = 0;
            return State.FAILURE;
        }
    }
}
