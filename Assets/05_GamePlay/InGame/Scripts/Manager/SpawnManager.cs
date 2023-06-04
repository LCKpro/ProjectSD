using UnityEngine;
using Redcode.Pools;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    private PoolManager poolManager_Monster;
    private PoolManager poolManager_Structure;
    private PoolManager poolManager_IncompleteStructure;

    private Transform _target = null;

    private void Start()
    {
        var manager = GamePlay.Instance;
        poolManager_Monster = manager.poolManager_Monster;
        poolManager_Structure = manager.poolManager_Structure;
        poolManager_IncompleteStructure = manager.poolManager_IncompleteStructure;

        SpawnMonster();
    }

    public void SpawnMonster()
    {
        for (int i = 0; i < 5; i++)
        {
            AIState newObj = poolManager_Monster.GetFromPool<AIState>(0);
            SetPosition(newObj);
        }
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

    private void SetPosition(AIState player)
    {
        if(_target == null)
            _target = GamePlay.Instance.playerManager.GetPlayer().transform;

        int x, z;
        var r = GameUtils.RandomBool();

        if (r)
        {
            x = Random.Range(0, 23);
            z = 30;

            if (x % 2 == 0) // x좌표는 좌우 구분을 위해 음수도 추가되어야 함
            {
                x *= -1;
            }
        }
        else
        {
            x = 23;
            z = Random.Range(0, 30);

            if (z % 2 == 0) // x좌표는 좌우 구분을 위해 음수도 추가되어야 함
            {
                x *= -1;
            }
        }

        player.transform.position = new Vector3(_target.position.x + x, 0, _target.position.z + z);
    }

    public void ReturnPool(AIState clone)
    {
        poolManager_Monster.TakeToPool<AIState>(clone.idName, clone);
    }
}
