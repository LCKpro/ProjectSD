using UnityEngine;
using Redcode.Pools;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    private PoolManager poolManager_Monster;
    private PoolManager poolManager_Structure;
    private PoolManager poolManager_IncompleteStructure;
    private PoolManager poolManager_Projectile;

    //private Transform _target = null;

    private void Start()
    {
        var manager = GamePlay.Instance;
        poolManager_Monster = manager.poolManager_Monster;
        poolManager_Structure = manager.poolManager_Structure;
        poolManager_IncompleteStructure = manager.poolManager_IncompleteStructure;
        poolManager_Projectile = manager.poolManager_Projectile;

        SpawnMonster();
    }

    public void SpawnMonster()
    {
        for (int i = 0; i < 5; i++)
        {
            poolManager_Monster.GetFromPool<AIPlayer>(3);
        }

        //poolManager_Monster.GetFromPool<AIPlayer>(2);
        //poolManager_Monster.GetFromPool<AIPlayer>(0);
    }

    public Transform SpawnStructure(int index)
    {
        Transform newObj = poolManager_Structure.GetFromPool<Transform>(index);

        return newObj;
    }

    public Transform SpawnIncompleteStructure(int index)
    {
        Transform newObj = poolManager_IncompleteStructure.GetFromPool<Transform>(index);

        return newObj;
    }

    

    public void ReturnPool(AIPlayer clone)
    {
        poolManager_Monster.TakeToPool<AIPlayer>(clone.idName, clone);
    }

    public void ReturnProjectilePool(Transform clone)
    {
        poolManager_Projectile.TakeToPool<Transform>(clone);
    }
}
