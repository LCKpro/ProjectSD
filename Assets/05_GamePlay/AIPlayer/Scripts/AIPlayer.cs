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
        //TODO::등장하는 위치 정하고 설정해야 함
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
    /// 스스로 사라지도록 수정
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

        // 부딪히면 부수는 행동으로 변경
    }

    public void Die()
    {
        anim.SetInteger("animation", 6);   // 6 or 7
        Invoke("ReturnToPool", 2);
        // 몬스터의 경우 풀에 다시 넣어주는 로직 필요.
        // 건물 역시 마찬가지로 넣어주기
    }

    private void ReturnToPool()
    {
        GamePlay.Instance.spawnManager.ReturnPool(this);
    }

    // State 만들어야 함~
}
