using UnityEngine;

namespace AI_BehaviorTree_AIImplementation
{
    public class Attack : Node
    {
        public override State Evaluate()
        {
            // Check if the NPC is close to the player
            if (IsCloseToPlayer())
            {
                // Attack the player
                AttackAction();

                // Wait for a short time
                new WaitForSeconds(0.5f);

                return State.RUNNING;
            }

            return State.FAILURE;
        }

        private bool IsCloseToPlayer()
        {
            // TODO: Implement this method
            return false;
        }

        private void AttackAction()
        {
            // TODO: Implement this method
        }
    }
}
