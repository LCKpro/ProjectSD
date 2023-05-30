using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;

public class PoolManagerA : MonoBehaviour
{
    private PoolManager poolManager;

    private void Awake()
    {
        poolManager = GamePlay.Instance.poolManager;
    }

    private void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        for (int i = 0; i < 5; i++)
        {
            AIPlayer newObj = poolManager.GetFromPool<AIPlayer>(0);
        }
    }

    public void ReturnPool(AIPlayer clone)
    {
        poolManager.TakeToPool<AIPlayer>(clone.idName, clone);
    }
}
