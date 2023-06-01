using System;
using UnityEngine;
using UnityEngine.AI;
using Redcode.Pools;
using UniRx;


public class AIPlayer : Health, IPoolObject
{
    public string idName;
    public Animator anim;
    public Transform target;

    private NavMeshAgent _ai;

    // 이동할 때 쓸 타이머
    private IDisposable _moveController = Disposable.Empty;

    // 공격할 때 쓸 타이머
    private IDisposable _attackController = Disposable.Empty;

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

    private void FinalizeAttack()
    {
        _attackController.Dispose();
        _attackController = Disposable.Empty;
    }

    private void Init()
    {
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

    #region 데미지 관련

    public override void DealDamage(GameObject target)
    {
        base.DealDamage(target);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }

    #endregion

    private GameObject targetObj = null;
    // 공격 시작할 때 필요한거
    public void AttackStart(Collider other)
    {
        targetObj = other.gameObject;
        FinalizeControl();  // 이동 멈추고
        _ai.isStopped = true;
        _ai.velocity = Vector3.zero;
        FinalizeAttack();   // 공격 일단 먼저 멈추고
        anim.SetInteger("animation", 13);    // 공격 애니메이션
        _attackController = Observable.Interval(TimeSpan.FromSeconds(1f)).TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                if(targetObj.activeSelf == false)
                {
                    targetObj = null;
                    FinalizeAttack();
                    AIControllerStart();
                }
            });
    }

    public void AIAttack()
    {
        if(targetObj != null)
        {
            DealDamage(targetObj);
        }
        else
        {
            Debug.Log("AI 공격 버그");
        }
        
    }

    protected override void Die()
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
