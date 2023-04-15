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
        public string GetName() { return "TOM AI"; }

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

            myBehaviorTree.start.Evaluate(myBehaviorTree.data);

            actionList = (List<AIAction>)myBehaviorTree.data.Blackboard[(int)BlackboardEnum.actionList];

            AIActionLookAtPosition actionLookAt = new AIActionLookAtPosition();
            actionLookAt.Position = ((PlayerInformations)myBehaviorTree.data.Blackboard[(int)BlackboardEnum.target]).Transform.Position;
            ((List<AIAction>)myBehaviorTree.data.Blackboard[(int)BlackboardEnum.actionList]).Add(actionLookAt);
            ((List<AIAction>)myBehaviorTree.data.Blackboard[(int)BlackboardEnum.actionList]).Add(new AIActionFire());

            return actionList;
        }

        private void InitializeBehaviorTree()
        {

            //ChaseBonusNode chaseBonusNode = new ChaseBonusNode();
            SelectTargetNode selectTargetNode = new SelectTargetNode();
            ChaseNode chaseNode = new ChaseNode();
            ProjectileInPlayerDirectionNode projectileInPlayerDirectionNode = new ProjectileInPlayerDirectionNode();
            ShootNode shootNode = new ShootNode();

            Sequence chaseSequence = new Sequence(new List<Node>() { selectTargetNode, chaseNode });
            Selector topSelector = new Selector(new List<Node>() { projectileInPlayerDirectionNode, chaseSequence });

            myBehaviorTree.start = topSelector;
        }


        private void UpdateBlackboard()
        {
            List<PlayerInformations> playerInfos = AIGameWorldUtils.GetPlayerInfosList();
            PlayerInformations myPlayerInfo = GetPlayerInfos(AIId, playerInfos);
            myBehaviorTree.data.Blackboard[(int)BlackboardEnum.myPlayerPosition] = myPlayerInfo.Transform.Position;
            myBehaviorTree.data.Blackboard[(int)BlackboardEnum.myPlayerId] = myPlayerInfo.PlayerId;
            myBehaviorTree.data.Blackboard[(int)BlackboardEnum.playerRotation] = Vector3.right;
            myBehaviorTree.data.Blackboard[(int)BlackboardEnum.bonusTarget] = null;
            myBehaviorTree.data.Blackboard[(int)BlackboardEnum.target] = null;
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
