using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Redcode.Pools;

public class AIPlayer : MonoBehaviour, IPoolObject
{
    public string idName;
    public Animator anim;
    public Vector3 targetPos;
    private bool _isAtDestination;

    private NavMeshAgent ai;

    private void Awake()
    {
        ai = GetComponent<NavMeshAgent>();
    }

    private void Init()
    {
        //TODO::�����ϴ� ��ġ ���ϰ� �����ؾ� ��
        transform.position = Vector3.zero;
        //Transform[] spawnPos = GameManager.instance.points;
        //ai.SetDestination(spawnPos[Random.Range(0, spawnPos.Length)].position);
    }

    /// <summary>
    /// ������ ��������� ����
    /// </summary>
    private void FinalizeAI()
    {
        GamePlay.Instance.poolManagerA.ReturnPool(this);
    }

    public void OnCreatedInPool()
    {
    }

    public void OnGettingFromPool()
    {
        Init();
    }

    // State ������ ��~
}
