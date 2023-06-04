using UnityEngine.AI;
using System;
using UnityEngine;
using UniRx;
using Redcode.Pools;

public class AIState : Stat, IPoolObject
{
    private IDisposable _stateController = Disposable.Empty;
    private IDisposable _attackController = Disposable.Empty;   // ������ �� �� Ÿ�̸�
    private IDisposable _attackTimeLimit = Disposable.Empty;   // ���� �ð� ���� ���� ������ ���

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
                StopAttackTimeLimit(); // Ÿ�̸� ����
                stateType = GameDefine.AIStateType.Breakable;
            });
    }

    private GameObject targetObj = null;
    // ���� ������ �� �ʿ��Ѱ�
    public void MeleeAttackStart(Collider other)
    {
        StopAttackTimeLimit();      // �����ϴµ� ���������� Ÿ�̸� ����
        stateType = GameDefine.AIStateType.Attack;
        targetObj = other.gameObject;
        StopAlMove();  // �̵� ���߰�
        StopAIAttack();   // ���� �ϴ� ���� ���߰�
        anim.SetInteger("animation", 13);    // ���� �ִϸ��̼�
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

    /// �̵� ���߱�
    private void StopAlMove()
    {
        _stateController.Dispose();
        _stateController = Disposable.Empty;

        _ai.isStopped = true;
        _ai.velocity = Vector3.zero;
    }

    /// ���� ���߱�
    private void StopAIAttack()
    {
        _attackController.Dispose();
        _attackController = Disposable.Empty;
    }

    /// �߰� ��� Ÿ�̸� ���߱�
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
            Debug.Log("AI ���� ����");
        }
    }

    #region ������ ����

    public override void DealDamage(GameObject target)
    {
        base.DealDamage(target);

        CheckDamageType();
        KnockBack(target);
    }

    public override void TakeDamage(float damage, GameObject attacker = null)
    {
        base.TakeDamage(damage);

        // �����ڰ� ������(�����̴� �ǹ��̴�) ������ ���� �߰�
        if (attacker != null && stateType != GameDefine.AIStateType.Chase_Attacker)
        {
            _target = attacker.transform;
            stateType = GameDefine.AIStateType.Chase_Attacker;
            AITimeLimitStart(); // ���� 5�� ���� ���������� ��
        }
    }

    // ���� Ÿ�� üũ�� �� �����ϱ�
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
        // ������ ��� Ǯ�� �ٽ� �־��ִ� ���� �ʿ�.
        // �ǹ� ���� ���������� �־��ֱ�
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

        // �ε����� �μ��� �ൿ���� ����
    }

    /// <summary>
    /// ������ ��������� ����
    /// </summary>
    private void FinalizeAI()
    {
        GamePlay.Instance.spawnManager.ReturnPool(this);
    }
}
