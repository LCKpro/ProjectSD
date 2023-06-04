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

    private void AIMove()
    {
        _ai.SetDestination(_target.position);
    }

    private GameObject targetObj = null;
    // ���� ������ �� �ʿ��Ѱ�
    public void AttackStart(Collider other)
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

    public void AIAttack()
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
