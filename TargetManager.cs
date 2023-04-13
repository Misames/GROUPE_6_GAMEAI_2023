using System;
using AI_BehaviorTree_AIGameUtility;
using AI_BehaviorTree_AIImplementation;
using UnityEngine;

class Enemy
{
    public float closingSpeed;
    public float speed;
    internal GameObject transform;
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
        chosenTarget = enemies[1];
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
        //mise à jour des données du jeu actuel
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

    internal void TrackTargetMovement(object v, Enemy target, Vector3 predictedPos)
    {
        throw new NotImplementedException();
    }

    internal void UpdateTargetPosition(Enemy target, Vector3 predictedPos)
    {
        throw new NotImplementedException();
    }
}
static class TargetManagerExtensions
{
    public static void UpdateGameWorldData(this TargetManager targetManager, ref GameWorldUtils currentGameWorld)
    {
        // Mise à jour des données de l'environnement de jeu actuel
    }
}