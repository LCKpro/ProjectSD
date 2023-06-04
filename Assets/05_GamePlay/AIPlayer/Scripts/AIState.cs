using UnityEngine.AI;
using System;
using UnityEngine;
using UniRx;
using Redcode.Pools;

public class AIState : Stat, IPoolObject
{
    private IDisposable _stateController = Disposable.Empty;
    private IDisposable _attackController = Disposable.Empty;   // 공격할 때 쓸 타이머
    private IDisposable _attackTimeLimit = Disposable.Empty;   // 만약 시간 내에 공격 못했을 경우

    private Transform _target;
    private NavMeshAgent _ai;

    public GameDefine.AIStateType stateType = GameDefine.AIStateType.None;
    public GameDefine.AttackType attackType = GameDefine.AttackType.None;

    public Animator anim;

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

    public void Init()
    {
        _target = GamePlay.Instance.playerManager.GetPlayer().transform;
        AIControllerStart();
    }

    private void AIMove()
    {
        _ai.SetDestination(_target.position);
    }

    #region UniRx Start

    private void AIControllerStart()
    {
        stateType = GameDefine.AIStateType.Chase_CatTower;
        StopAlMove();
        anim.SetInteger("animation", 15);
        _stateController = Observable.EveryUpdate().TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                AIMove();
            });
    }

    private void AITimeLimitStart()
    {
        _stateController = Observable.Interval(TimeSpan.FromSeconds(5f)).TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                StopAttackTimeLimit(); // 타이머 끄고
                stateType = GameDefine.AIStateType.Breakable;
            });
    }

    private GameObject targetObj = null;
    // 공격 시작할 때 필요한거
    public void MeleeAttackStart(Collider other)
    {
        StopAttackTimeLimit();      // 공격하는데 성공했으면 타이머 끄기
        stateType = GameDefine.AIStateType.Attack;
        targetObj = other.gameObject;
        StopAlMove();  // 이동 멈추고
        StopAIAttack();   // 공격 일단 먼저 멈추고
        anim.SetInteger("animation", 13);    // 공격 애니메이션
        _attackController = Observable.Interval(TimeSpan.FromSeconds(1f)).TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                if (targetObj.activeSelf == false)
                {
                    targetObj = null;
                    StopAIAttack();
                    AIControllerStart();
                }
            });
    }

    #endregion

    #region UniRx Finalize

    /// 이동 멈추기
    private void StopAlMove()
    {
        _stateController.Dispose();
        _stateController = Disposable.Empty;

        _ai.isStopped = true;
        _ai.velocity = Vector3.zero;
    }

    /// 공격 멈추기
    private void StopAIAttack()
    {
        _attackController.Dispose();
        _attackController = Disposable.Empty;
    }

    /// 추격 대기 타이머 멈추기
    private void StopAttackTimeLimit()
    {
        _attackTimeLimit.Dispose();
        _attackTimeLimit = Disposable.Empty;
    }

    #endregion

    public void MeleeAttack()
    {
        if (targetObj != null)
        {
            DealDamage(targetObj);
        }
        else
        {
            Debug.Log("AI 공격 버그");
        }
    }

    #region 데미지 관련

    public override void DealDamage(GameObject target)
    {
        base.DealDamage(target);

        CheckDamageType();
        KnockBack(target);
    }

    public override void TakeDamage(float damage, GameObject attacker = null)
    {
        base.TakeDamage(damage);

        // 공격자가 있으면(유닛이던 건물이던) 그쪽을 먼저 추격
        if (attacker != null && stateType != GameDefine.AIStateType.Chase_Attacker)
        {
            _target = attacker.transform;
            stateType = GameDefine.AIStateType.Chase_Attacker;
            AITimeLimitStart(); // 공격 5초 세고 공격했으면 끝
        }
    }

    // 공격 타입 체크한 후 공격하기
    private void CheckDamageType()
    {
        if(attackType == GameDefine.AttackType.Melee)
        {
            var atk = GetComponent<AIMeleeAttack>();
            atk.Attack();
        }
        else if(attackType == GameDefine.AttackType.Range)
        {
            var atk = GetComponent<AIRangeAttack>();
            atk.Attack();
        }
        else if(attackType == GameDefine.AttackType.Suicide)
        {
            var atk = GetComponent<AISuicide>();
            atk.Attack();
        }
    }

    private void KnockBack(GameObject target)
    {
        var vec = this.transform.position - target.transform.position;
        _ai.isStopped = true;
        _ai.velocity = vec * 10;
    }

    #endregion

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            FinalizeAI();
        }

        // 부딪히면 부수는 행동으로 변경
    }

    /// <summary>
    /// 스스로 사라지도록 수정
    /// </summary>
    private void FinalizeAI()
    {
        GamePlay.Instance.spawnManager.ReturnPool(this);
    }
}
