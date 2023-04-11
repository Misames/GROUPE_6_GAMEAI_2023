namespace AI_BehaviorTree_AIImplementation
{
    public class Condition : Node
    {
        public delegate bool Del();

        public Del EveluateCondition;

        public Condition(Del callback) { }
        
        public static bool CloseToTarget()
        {
            // test close to target using blackboard
            return true;
        }
    }
}
