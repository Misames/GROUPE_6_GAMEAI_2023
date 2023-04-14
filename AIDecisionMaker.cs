using System.Collections.Generic;
using AI_BehaviorTree_AIGameUtility;
using UnityEngine;
using UnityEngine.Assertions;

namespace AI_BehaviorTree_AIImplementation
{
    public class AIDecisionMaker
    {
        private BehaviourTree myBehaviorTree = new BehaviourTree();

        /// <summary>
        /// Ne pas supprimer des fonctions, ou changer leur signature sinon la DLL ne fonctionnera plus
        /// Vous pouvez unitquement modifier l'intérieur des fonctions si nécessaire (par exemple le nom)
        /// ComputeAIDecision en fait partit
        /// </summary>
        private int AIId = -1;
        public GameWorldUtils AIGameWorldUtils = new GameWorldUtils();

        // Ne pas utiliser cette fonction, elle n'est utile que pour le jeu qui vous Set votre Id, si vous voulez votre Id utilisez AIId
        public void SetAIId(int parAIId) { AIId = parAIId; }

        // Vous pouvez modifier le contenu de cette fonction pour modifier votre nom en jeu
        public string GetName() { return "MichelAI"; }

        public void SetAIGameWorldUtils(GameWorldUtils parGameWorldUtils)
        {
            AIGameWorldUtils = parGameWorldUtils;
            UpdateBlackboard();
        }

        //Fin du bloc de fonction nécessaire (Attention ComputeAIDecision en fait aussi partit)

        private float BestDistanceToFire = 10.0f;

        public List<AIAction> ComputeAIDecision()
        {
            InitializeBehaviorTree();

            myBehaviorTree.UpdateGameWorldData(ref AIGameWorldUtils);

            UpdateBlackboard();

            List<AIAction> actionList = new List<AIAction>();

            List<PlayerInformations> playerInfos = AIGameWorldUtils.GetPlayerInfosList();

            PlayerInformations target = null;
            foreach (PlayerInformations playerInfo in playerInfos)
            {
                if (!playerInfo.IsActive)
                    continue;

                if (playerInfo.PlayerId == AIId)
                    continue;

                target = playerInfo;
                break;
            }

            if (target == null)
                return actionList;

            PlayerInformations myPlayerInfo = GetPlayerInfos(AIId, playerInfos);
            if (myPlayerInfo == null)
                return actionList;

            myBehaviorTree.start.Evaluate(myBehaviorTree.data);

            actionList = (List<AIAction>)myBehaviorTree.data.Blackboard[(int)BlackboardEnum.actionList];

            AIActionLookAtPosition actionLookAt = new AIActionLookAtPosition();
            actionLookAt.Position = target.Transform.Position;
            actionList.Add(actionLookAt);
            actionList.Add(new AIActionFire());

            return actionList;
        }

        private void InitializeBehaviorTree()
        {
            List<PlayerInformations> playerInfos = AIGameWorldUtils.GetPlayerInfosList();

            PlayerInformations target = null;
            foreach (PlayerInformations playerInfo in playerInfos)
            {
                if (!playerInfo.IsActive)
                    continue;

                if (playerInfo.PlayerId == AIId)
                    continue;

                target = playerInfo;
                break;
            }

            ChaseNode chaseNode = new ChaseNode(target);
            chaseNode.AssignData(ref myBehaviorTree.data);
            IsInRangeNode chasingRangeNode = new IsInRangeNode(500f, target);
            chasingRangeNode.AssignData(ref myBehaviorTree.data);

            Sequence chaseSequence = new Sequence(new List<Node>() { chasingRangeNode, chaseNode });
            Selector topSelector = new Selector(new List<Node>() { chaseSequence });

            myBehaviorTree.start = topSelector;
        }


        private void UpdateBlackboard()
        {
            List<PlayerInformations> playerInfos = AIGameWorldUtils.GetPlayerInfosList();
            PlayerInformations myPlayerInfo = GetPlayerInfos(AIId, playerInfos);
            myBehaviorTree.data.Blackboard[(int)BlackboardEnum.myPlayerPosition] = myPlayerInfo.Transform.Position;
            myBehaviorTree.data.Blackboard[(int)BlackboardEnum.myPlayerId] = myPlayerInfo.PlayerId;
            myBehaviorTree.data.Blackboard[(int)BlackboardEnum.targetPosition] = null;
            myBehaviorTree.data.Blackboard[(int)BlackboardEnum.targetIsEnemy] = false;
            myBehaviorTree.data.Blackboard[(int)BlackboardEnum.enemyProximityLimit] = 10;
            myBehaviorTree.data.Blackboard[(int)BlackboardEnum.actionList] = new List<AIAction>();
        }

        public PlayerInformations GetPlayerInfos(int parPlayerId, List<PlayerInformations> parPlayerInfosList)
        {
            foreach (PlayerInformations playerInfo in parPlayerInfosList)
            {
                if (playerInfo.PlayerId == parPlayerId)
                    return playerInfo;
            }

            Assert.IsTrue(false, "GetPlayerInfos : PlayerId not Found");
            return null;
        }
    }
}
