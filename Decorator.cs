namespace AI_BehaviorTree_AIImplementation
{
    public enum DecoratorType
    {
        NONE,
        FORCE_FAILURE,
        FORCE_SUCCESS,
        INVERTER,
        REPEAT,
        RETRY,
    }

    public struct Decorator{
        public DecoratorType type;
        public int retry;
        public int repetition;

        public Decorator(DecoratorType dType)
        {
            type = dType;
            retry = 0;
            repetition = 0;
        }
    }
}
