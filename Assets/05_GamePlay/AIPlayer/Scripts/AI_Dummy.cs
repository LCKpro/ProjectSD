using System;
using UnityEngine;
using UniRx;
using Redcode.Pools;
using Random = UnityEngine.Random;
using UnityEngine.AI;

public class AI_Dummy : Stat
{
    public Animator anim;

    private NavMeshAgent _ai;
    private Rigidbody _rigid;
    private Transform _target;
    private IDisposable _stateController = Disposable.Empty;
    private float finalMoveSpeed = 0f;

    private void Start()
    {
        Init();
    }

    #region UniRx Start

    public void Init()
    {
        _target = GamePlay.Instance.playerManager.GetCatTower().transform;
        transform.LookAt(_target);
        _rigid = GetComponent<Rigidbody>();
        _ai = GetComponent<NavMeshAgent>();
        //SetPosition();
        AIControllerStart();
    }

    private void AIControllerStart()
    {
        StopAIController();
        StopAlMove();
        anim.SetInteger("animation", 15);
        finalMoveSpeed = moveSpeed;
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
                StopAIController(); // Ÿ�̸� ����
            });
    }

    private void AIMove()
    {
        if (_target != null)
        {
            _ai.isStopped = false;
            _ai.SetDestination(_target.position);
        }
        else
            Debug.Log("Ÿ�� NULL");
    }

    #endregion

    #region UniRx Finalize

    private void StopAIController()
    {
        _stateController.Dispose();
        _stateController = Disposable.Empty;
    }

    /// �̵� ���߱�
    private void StopAlMove()
    {
        if (_rigid != null)
            _rigid.velocity = Vector3.zero;
        _ai.isStopped = true;
        _ai.velocity = Vector3.zero;
    }

    #endregion

    #region ������ ����

    public override void DealDamage(GameObject target, float power = 0f)
    {
        base.DealDamage(target);

    }

    private AI_Structure_HpBar _hpBarObj = null;
    public override void TakeDamage(float damage, GameObject attacker = null, float power = 0f)
    {
        base.TakeDamage(damage);

        if (_hpBarObj == null)
        {
            var hpBar = GamePlay.Instance.spawnManager.GetHpBarFromPool();
            _hpBarObj = hpBar;
        }

        _hpBarObj.SetTarget(transform);
        _hpBarObj.SetHpBar(maxHealthValue, healthValue);

        if (power > 1f)
        {
            KnockBack(attacker, power);
        }
    }

    public void ReturnHpBar()
    {
        _hpBarObj.ResetActiveTimer();
        _hpBarObj = null;
    }

    private void KnockBack(GameObject target, float power)
    {
        var vec = this.transform.position - target.transform.position;
        transform.GetComponent<Rigidbody>().AddForce(vec * power, ForceMode.Impulse);
        anim.SetInteger("animation", 3);
        Invoke("ChangeAnim", 1.5f);
    }

    #endregion

    protected override void Die()
    {
        ReturnHpBar();
        anim.SetInteger("animation", 6);   // 6 or 7
        Invoke("ReturnToPool", 1);
        Invoke("DieAI", 1);
        // ������ ��� Ǯ�� �ٽ� �־��ִ� ���� �ʿ�.
        // �ǹ� ���� ���������� �־��ֱ�
    }

    public void DieAI()
    {
        transform.gameObject.SetActive(false);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            DieAI();
        }

        // �ε����� �μ��� �ൿ���� ����
    }
}
