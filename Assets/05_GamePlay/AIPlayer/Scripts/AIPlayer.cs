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

    // �̵��� �� �� Ÿ�̸�
    private IDisposable _moveController = Disposable.Empty;

    // ������ �� �� Ÿ�̸�
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

    #region ������ ����

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
    // ���� ������ �� �ʿ��Ѱ�
    public void AttackStart(Collider other)
    {
        targetObj = other.gameObject;
        FinalizeControl();  // �̵� ���߰�
        _ai.isStopped = true;
        _ai.velocity = Vector3.zero;
        FinalizeAttack();   // ���� �ϴ� ���� ���߰�
        anim.SetInteger("animation", 13);    // ���� �ִϸ��̼�
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
            Debug.Log("AI ���� ����");
        }
        
    }

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

    // State ������ ��~
}
