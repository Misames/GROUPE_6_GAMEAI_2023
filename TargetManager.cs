using System;
using AI_BehaviorTree_AIGameUtility;
using AI_BehaviorTree_AIImplementation;
using UnityEngine;

class Enemy
{
    public float closingSpeed;
    public float speed;
    internal GameObject transform;
    internal bool IsActive;
    internal int EnemyId;
}

static class EnemyArrayExtensions
{
    public static int Length(this Enemy[] array)
    {
        return array.Length;
    }
}

class TargetManager
{
    Enemy chosenTarget;
    Vector3 predictedTargetPos;

    public Enemy ChooseTarget(Enemy[] enemies)
    {
        // Parcours la liste des ennemis et choisis le meilleur 
        if (enemies.Length > 0)
        {
            chosenTarget = enemies[enemies.Length - 1];
        }
        else
        {
            // enemies est vide, on ne choisit pas de cible
        }
        for (int i = 1; i < enemies.Length; i++)
        {
            if (enemies[i].closingSpeed > chosenTarget.closingSpeed)
            {
                chosenTarget = enemies[i];
            }
        }
        return chosenTarget;
    }

    public void UpdateGameWorldData(ref GameWorldUtils currentGameWorld)
    {
        // Met à jour les données du monde de jeu 
        currentGameWorld.GetPlayerInfosList();
    }

    public void TrackTargetMovement(object v1, object v, Transform targetTransform)
    {
        // Suit le déplacement de la cible d'une frame à l'autre 
        predictedTargetPos = targetTransform.position;
    }

    public Vector3 PredictTargetPos(Vector3 startPos, Vector3 direction, float speed, int framesCount)
    {
        // Prédit la position future de la cible en N frames 
        Vector3 pos = startPos;
        for (int i = 0; i < framesCount; i++)
        {
            pos += direction * speed * Time.deltaTime;
        }
        return pos;
    }

    internal void UpdateTargetPosition(Enemy target, Vector3 predictedPos)
    {
        target.transform.transform.position = predictedPos;
    }
}
static class TargetManagerExtensions
{
    public static void UpdateGameWorldData(this TargetManager targetManager, ref GameWorldUtils currentGameWorld)
    {
        targetManager.UpdateGameWorldData(ref currentGameWorld);
    }
}