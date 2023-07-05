using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class SpawnManager : MonoBehaviour
{
    public void SpawnMonster(int monsterIndex, int monsterAmount, Vector3 spawnPos)
    {
        UniTask.Create(async () =>
        {
            try
            {
                for (int i = 0; i < monsterAmount; i++)
                {
                    var monster = GamePlay.Instance.poolManager_Monster.GetFromPool<AIPlayer>(monsterIndex);
                    monster.transform.position = spawnPos;

                    await UniTask.Delay(TimeSpan.FromSeconds(0.5f)).SuppressCancellationThrow();
                }
            }
            catch (Exception ex)
            {
                GameUtils.Error(ex.ToString());
            }

        });

        //poolManager_Monster.GetFromPool<AIPlayer>(2);
        //poolManager_Monster.GetFromPool<AIPlayer>(0);
    }

    public Transform SpawnStructure(int structIndex, int index)
    {
        Transform newObj = GamePlay.Instance.poolManager_StructureArray[structIndex].GetFromPool<Transform>(index);

        return newObj;
    }

    public void ReturnStructurePool(int structIndex, string idName, Transform clone)
    {
        GamePlay.Instance.poolManager_StructureArray[structIndex].TakeToPool<Transform>(idName, clone);
    }

    public Transform SpawnIncompleteStructure(int structIndex, int index)
    {
        Transform newObj = GamePlay.Instance.poolManager_IncompleteStructureArray[structIndex].GetFromPool<Transform>(index);

        return newObj;
    }

    public void ReturnIncompleteStructurePool(int structIndex, string idName, Transform clone)
    {
        GamePlay.Instance.poolManager_IncompleteStructureArray[structIndex].TakeToPool<Transform>(idName, clone);
    }


    public void ReturnPool(AIPlayer clone)
    {
        GamePlay.Instance.poolManager_Monster.TakeToPool<AIPlayer>(clone.idName, clone);
    }

    public void ReturnProjectilePool(string idName, Transform clone)
    {
        GamePlay.Instance.poolManager_Projectile.TakeToPool<Transform>(idName, clone);
    }

    public void GetFromPool(string idName, Vector3 pos)
    {
        var effect = GamePlay.Instance.poolManager_Effect.GetFromPool<Transform>(idName);
        effect.position = pos + new Vector3(0, 3, 0);
    }

    public void ReturnEffectPool(string idName, Transform clone)
    {
        GamePlay.Instance.poolManager_Effect.TakeToPool<Transform>(idName, clone);
    }
}
