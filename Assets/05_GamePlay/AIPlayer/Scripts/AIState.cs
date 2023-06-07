using System;
using UnityEngine;
using UniRx;
using Redcode.Pools;
using Random = UnityEngine.Random;

public partial class AIPlayer : IPoolObject
{
    private IDisposable _stateController = Disposable.Empty;

    private Transform _target;
    private Rigidbody _rigid;

    private GameDefine.AIStateType _stateType = GameDefine.AIStateType.None;

    public Animator anim;

    public void SetStateType(GameDefine.AIStateType type)
    {
        _stateType = type;
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
        _rigid = GetComponent<Rigidbody>();
        SetPosition();
        AIControllerStart();
    }

    private void SetPosition()
    {
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

        transform.position = new Vector3(_target.position.x + x, 0, _target.position.z + z);
        transform.LookAt(_target);
    }

    private void AIMove()
    {
        //_ai.SetDestination(_target.position);
        
        _rigid.velocity = Vector3.zero;
        if (_target != null)
        {
            _rigid.velocity = (_target.position - this.transform.position) * moveSpeed;
        }
        else
            Debug.Log("타겟 NULL");
    }

    #region UniRx Start

    private void AIControllerStart()
    {
        _stateType = GameDefine.AIStateType.Chase_CatTower;
        StopAIController();
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
        StopAIController();
        _stateController = Observable.Interval(TimeSpan.FromSeconds(5f)).TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                StopAIController(); // 타이머 끄고
                _stateType = GameDefine.AIStateType.Breakable;
            });
    }

    #endregion

    #region UniRx Finalize

    private void StopAIController()
    {
        _stateController.Dispose();
        _stateController = Disposable.Empty;
    }

    /// 이동 멈추기
    private void StopAlMove()
    {
        _rigid.velocity = Vector3.zero;
        //_ai.isStopped = true;
        //_ai.velocity = Vector3.zero;
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

    }

    public override void TakeDamage(float damage, GameObject attacker = null)
    {
        base.TakeDamage(damage);

        if(attacker != null)
        {
            KnockBack(attacker);
        }

        // 공격자가 있으면(유닛이던 건물이던) 그쪽을 먼저 추격
        if (attacker != null && _stateType != GameDefine.AIStateType.Chase_Attacker)
        {
            _target = attacker.transform;
            _stateType = GameDefine.AIStateType.Chase_Attacker;
            AITimeLimitStart(); // 공격 5초 세고 공격했으면 끝
        }
    }

    private void KnockBack(GameObject target)
    {
        var vec = this.transform.position - target.transform.position;
        _rigid.AddForce(vec * 2, ForceMode.Impulse);
    }

    #endregion

    protected override void Die()
    {
        anim.SetInteger("animation", 6);   // 6 or 7
        Invoke("ReturnToPool", 1);
        // 몬스터의 경우 풀에 다시 넣어주는 로직 필요.
        // 건물 역시 마찬가지로 넣어주기
    }

    /// <summary>
    /// Invoke용 함수! 삭제x
    /// </summary>
    public void ReturnToPool()
    {
        GamePlay.Instance.spawnManager.ReturnPool(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
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
