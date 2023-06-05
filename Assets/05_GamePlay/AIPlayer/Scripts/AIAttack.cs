using UnityEngine;
using UniRx;
using System;

public partial class AIPlayer
{
    public GameDefine.AttackType atkType;

    #region ���Ÿ� ���� ���� �ν�����

    // ���Ÿ� ����ü Ǯ�� �Ŵ��� �ε��� ��ȣ
    public int poolProjectileIndex = 0;

    // ���Ÿ� ����ü ��� ��ġ
    public Transform startPos;

    // ����ü �ӵ�
    public float projectileSpeed = 5f;

    #endregion

    #region ���� ���� ���� �ν�����

    public float suicideRange = 5f;

    #endregion

    private GameObject targetObj = null;
    // ���� ������ �� �ʿ��Ѱ�
    public void MeleeAttackStart(Collider other)
    {
        SetStateType(GameDefine.AIStateType.Attack);
        targetObj = other.gameObject;
        StopAIController();     // �����ϴµ� ���������� Ÿ�̸� ����
        StopAlMove();  // �̵� ���߰�
        anim.SetInteger("animation", 13);    // ���� �ִϸ��̼�
        _stateController = Observable.Interval(TimeSpan.FromSeconds(1f)).TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                if (targetObj.activeSelf == false)
                {
                    targetObj = null;
                    StopAIController();
                    AIControllerStart();
                }
            });
    }

    // ���� ������ �� �ʿ��Ѱ�
    public void RangeAttackStart(Collider other)
    {
        SetStateType(GameDefine.AIStateType.Attack);
        targetObj = other.gameObject;
        StopAIController();     // �����ϴµ� ���������� Ÿ�̸� ����
        StopAlMove();  // �̵� ���߰�
        anim.SetInteger("animation", 20);    // ���� �ִϸ��̼�
        _stateController = Observable.Interval(TimeSpan.FromSeconds(1f)).TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                if (targetObj.activeSelf == false)
                {
                    targetObj = null;
                    StopAIController();
                    AIControllerStart();
                }
            });
    }

    public void RangeAttack(Collider other)
    {
        //StopAIController();     // �����ϴµ� ���������� Ÿ�̸� ����
        //StopAlMove();  // �̵� ���߰�
        //anim.SetInteger("animation", 20);    // ���� �ִϸ��̼�

        var projectile = GamePlay.Instance.poolManager_Projectile.GetFromPool<Transform>(poolProjectileIndex);

        projectile.position = startPos.position;
        projectile.GetComponent<Projectile>().SetPlayer(this);
        projectile.GetComponent<Rigidbody>().velocity = (targetObj.transform.position - projectile.position) * projectileSpeed;
    }

    public void SuicideAttackStart()
    {
        SetStateType(GameDefine.AIStateType.Attack);
        StopAIController();     // �����ϴµ� ���������� Ÿ�̸� ����
        StopAlMove();  // �̵� ���߰�
        anim.SetInteger("animation", 19);    // ���� �ִϸ��̼�
        _stateController = Observable.Interval(TimeSpan.FromSeconds(2f)).TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                if(_stateType != GameDefine.AIStateType.Die)
                {
                    Suicide();
                    // ���� ����Ʈ �����Ÿ� ���⿡
                    FinalizeAI();
                }
            });
    }

    private void Suicide()
    {
        Collider[] hitCol = Physics.OverlapSphere(transform.position, suicideRange);

        for (int i = 0; i < hitCol.Length; i++)
        {
            if(hitCol[i].gameObject.CompareTag("Breakable") == true)
            {
                DealDamage(hitCol[i].gameObject);
                //hitCol[i].GetComponent<Stat>().DealDamage(this.gameObject);
            }
        }
    }
}
