using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;

public class PoolManagerA : MonoBehaviour
{
    private PoolManager poolManager;

    private void Awake()
    {
        poolManager = GetComponent<PoolManager>();
    }

    public void Spawn()
    {
        int ran = Random.Range(0, 6);
        AIPlayer newObj = poolManager.GetFromPool<AIPlayer>(ran);
    }

    public void ReturnPool(AIPlayer clone)
    {
        poolManager.TakeToPool<AIPlayer>(clone.idName, clone);
    }
}
