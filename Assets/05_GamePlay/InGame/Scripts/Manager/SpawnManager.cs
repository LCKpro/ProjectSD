using UnityEngine;
using Redcode.Pools;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    private PoolManager poolManager_Monster;

    private PoolManager[] poolManager_Structure_Array;
    private PoolManager[] poolManager_IncompleteStructure_Array;

    private PoolManager poolManager_Projectile;

    //private Transform _target = null;

    private void Start()
    {
        var manager = GamePlay.Instance;
        poolManager_Monster = manager.poolManager_Monster;
        poolManager_Projectile = manager.poolManager_Projectile;

        poolManager_Structure_Array = manager.poolManager_StructureArray;
        poolManager_IncompleteStructure_Array = manager.poolManager_IncompleteStructureArray;

        SpawnMonster();
    }

    public void SpawnMonster()
    {
        /*for (int i = 0; i < 5; i++)
        {
            poolManager_Monster.GetFromPool<AIPlayer>(0);
        }*/

        //poolManager_Monster.GetFromPool<AIPlayer>(2);
        //poolManager_Monster.GetFromPool<AIPlayer>(0);
    }

    public Transform SpawnStructure(int structIndex, int index)
    {
        Transform newObj = poolManager_Structure_Array[structIndex].GetFromPool<Transform>(index);

        return newObj;
    }

    public Transform SpawnIncompleteStructure(int structIndex, int index)
    {
        Transform newObj = poolManager_IncompleteStructure_Array[structIndex].GetFromPool<Transform>(index);

        return newObj;
    }


    public void ReturnPool(AIPlayer clone)
    {
        poolManager_Monster.TakeToPool<AIPlayer>(clone.idName, clone);
    }

    public void ReturnProjectilePool(string idName, Transform clone)
    {
        poolManager_Projectile.TakeToPool<Transform>(idName, clone);
    }
}
