using System;
using UnityEngine;
using UnityEngine.AI;
using Redcode.Pools;
using UniRx;
using Random = UnityEngine.Random;


public class AIPlayer : MonoBehaviour, IPoolObject
{
    public string idName;
    public Animator anim;
    public Transform target;

    private NavMeshAgent _ai;

    private IDisposable _moveController = Disposable.Empty;

    private void Start()
    {
        _ai = GetComponent<NavMeshAgent>();
    }

    public void OnCreatedInPool()
    {
    }

    public void OnGettingFromPool()
    {
        Init();
    }

    private void FinalizeControl()
    {
        _moveController.Dispose();
        _moveController = Disposable.Empty;
    }

    private void Init()
    {
        //TODO::�����ϴ� ��ġ ���ϰ� �����ؾ� ��
        //transform.position = Vector3.zero;
        //Transform[] spawnPos = GameManager.instance.points;
        //ai.SetDestination(spawnPos[Random.Range(0, spawnPos.Length)].position);
        target = GamePlay.Instance.playerManager.GetPlayer().transform;
        AIControllerStart();
    }

    public void AIControllerStart()
    {
        FinalizeControl();
        anim.SetInteger("animation", 15);
        _moveController = Observable.EveryUpdate().TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                AIMove();
            });
    }

    private void AIMove()
    {
        _ai.SetDestination(target.position);
    }


    /// <summary>
    /// ������ ��������� ����
    /// </summary>
    private void FinalizeAI()
    {
        GamePlay.Instance.spawnManager.ReturnPool(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            FinalizeControl();
            FinalizeAI();
        }

        // �ε����� �μ��� �ൿ���� ����
    }

    public void Die()
    {
        anim.SetInteger("animation", 6);   // 6 or 7
        Invoke("ReturnToPool", 2);
        // ������ ��� Ǯ�� �ٽ� �־��ִ� ���� �ʿ�.
        // �ǹ� ���� ���������� �־��ֱ�
    }

    private void ReturnToPool()
    {
        GamePlay.Instance.spawnManager.ReturnPool(this);
    }

    // State ������ ��~
}
