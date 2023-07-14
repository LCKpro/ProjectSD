using System;
using UnityEngine;
using UniRx;
using Redcode.Pools;
using Random = UnityEngine.Random;
using UnityEngine.AI;

public partial class AIPlayer : IPoolObject
{
    private IDisposable _stateController = Disposable.Empty;

    private Transform _target;
    private Rigidbody _rigid;

    private GameDefine.AIStateType _stateType = GameDefine.AIStateType.None;

    private float finalMoveSpeed = 0f;

    public Animator anim;
    public AI_Structure_HpBar ai_Structure_HpBar;
    public int goldReward = 0;
    public int pointReward = 0;
    private NavMeshAgent _ai;

    public void SetStateType(GameDefine.AIStateType type)
    {
        _stateType = type;
    }

    public GameDefine.AIStateType GetStateType()
    {
        return _stateType;
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
        _target = GamePlay.Instance.playerManager.GetCatTower().transform;
        transform.LookAt(_target);
        _rigid = GetComponent<Rigidbody>();
        _ai = GetComponent<NavMeshAgent>();
        //SetPosition();
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

            if (x % 2 == 0) // x��ǥ�� �¿� ������ ���� ������ �߰��Ǿ�� ��
            {
                x *= -1;
            }
        }
        else
        {
            x = 23;
            z = Random.Range(0, 30);

            if (z % 2 == 0) // x��ǥ�� �¿� ������ ���� ������ �߰��Ǿ�� ��
            {
                x *= -1;
            }
        }

        transform.position = new Vector3(_target.position.x + x, 0, _target.position.z + z);
        transform.LookAt(_target);
    }

    #region UniRx Start

    private void AIControllerStart()
    {
        _stateType = GameDefine.AIStateType.Chase_CatTower;
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
                _stateType = GameDefine.AIStateType.Breakable;
            });
    }

    private void AIMove()
    {
        //_ai.SetDestination(_target.position);

        /*_rigid.velocity = Vector3.zero;
        if (_target != null)
        {
            _rigid.velocity = (_target.position - this.transform.position) * finalMoveSpeed;
        }
        else
            Debug.Log("Ÿ�� NULL");*/

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

    public void MeleeAttack()
    {
        if (targetObj != null)
        {
            SoundManager.instance.PlaySound("NormalAtk");
            DealDamage(targetObj);
        }
        else
        {
            Debug.Log("AI ���� ����");
        }
    }

    #region ������ ����

    public override void DealDamage(GameObject target, float power = 0f)
    {
        base.DealDamage(target);

    }

    private AI_Structure_HpBar _hpBarObj = null;
    public override void TakeDamage(float damage, GameObject attacker = null, float power = 0f)
    {
        base.TakeDamage(damage);

        if(_hpBarObj == null)
        {
            var hpBar = GamePlay.Instance.spawnManager.GetHpBarFromPool();
            _hpBarObj = hpBar;
        }

        _hpBarObj.SetTarget(transform);
        _hpBarObj.SetHpBar(maxHealthValue, healthValue);

        if (_stateType == GameDefine.AIStateType.Die)
        {
            return;
        }

        if (power > 1f)
        {
            KnockBack(attacker, power);
        }

        // �����ڰ� ������(�����̴� �ǹ��̴�) ������ ���� �߰�
        if (attacker != null && _stateType != GameDefine.AIStateType.Chase_Attacker)
        {
            _target = attacker.transform;
            _stateType = GameDefine.AIStateType.Chase_Attacker;
            AITimeLimitStart(); // ���� 5�� ���� ���������� ��
        }
    }

    private void KnockBack(GameObject target, float power)
    {
        var vec = this.transform.position - target.transform.position;
        transform.GetComponent<Rigidbody>().AddForce(vec * power, ForceMode.Impulse);
        anim.SetInteger("animation", 3);
        Invoke("ChangeAnim", 1.5f);
    }

    public void ChangeAnim()
    {
        int state = 1;

        if (_stateType == GameDefine.AIStateType.Idle ||
           _stateType == GameDefine.AIStateType.None)
        {
            state = 1;
        }
        else if (_stateType == GameDefine.AIStateType.Attack)
        {
            state = 6;
        }
        else if (_stateType == GameDefine.AIStateType.Chase_CatTower ||
                 _stateType == GameDefine.AIStateType.Chase_Attacker)
        {
            state = 15;
        }

        anim.SetInteger("animation", state);
    }

    #endregion

    protected override void Die()
    {
        _stateType = GameDefine.AIStateType.Die;
        anim.SetInteger("animation", 6);   // 6 or 7
        GamePlay.Instance.gameDataManager.AddGold(goldReward);
        GamePlay.Instance.gameDataManager.AddPoint(pointReward);
        Invoke("ReturnToPool", 1);
        //Invoke("DieAI", 1);
        // ������ ��� Ǯ�� �ٽ� �־��ִ� ���� �ʿ�.
        // �ǹ� ���� ���������� �־��ֱ�
    }

    public void DieAI()
    {
        transform.gameObject.SetActive(false);
    }

    /// <summary>
    /// Invoke�� �Լ�! ����x
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
