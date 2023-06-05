using UnityEngine;
using UniRx;
using System;

public partial class AIPlayer
{
    public GameDefine.AttackType atkType;

    #region 원거리 공격 전용 인스펙터

    // 원거리 투사체 풀링 매니저 인덱스 번호
    public int poolProjectileIndex = 0;

    // 원거리 투사체 출발 위치
    public Transform startPos;

    // 투사체 속도
    public float projectileSpeed = 5f;

    #endregion

    #region 자폭 공격 전용 인스펙터

    public float suicideRange = 5f;

    #endregion

    private GameObject targetObj = null;
    // 공격 시작할 때 필요한거
    public void MeleeAttackStart(Collider other)
    {
        SetStateType(GameDefine.AIStateType.Attack);
        targetObj = other.gameObject;
        StopAIController();     // 공격하는데 성공했으면 타이머 끄기
        StopAlMove();  // 이동 멈추고
        anim.SetInteger("animation", 13);    // 공격 애니메이션
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

    // 공격 시작할 때 필요한거
    public void RangeAttackStart(Collider other)
    {
        SetStateType(GameDefine.AIStateType.Attack);
        targetObj = other.gameObject;
        StopAIController();     // 공격하는데 성공했으면 타이머 끄기
        StopAlMove();  // 이동 멈추고
        anim.SetInteger("animation", 20);    // 공격 애니메이션
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
        //StopAIController();     // 공격하는데 성공했으면 타이머 끄기
        //StopAlMove();  // 이동 멈추고
        //anim.SetInteger("animation", 20);    // 공격 애니메이션

        var projectile = GamePlay.Instance.poolManager_Projectile.GetFromPool<Transform>(poolProjectileIndex);

        projectile.position = startPos.position;
        projectile.GetComponent<Projectile>().SetPlayer(this);
        projectile.GetComponent<Rigidbody>().velocity = (targetObj.transform.position - projectile.position) * projectileSpeed;
    }

    public void SuicideAttackStart()
    {
        SetStateType(GameDefine.AIStateType.Attack);
        StopAIController();     // 공격하는데 성공했으면 타이머 끄기
        StopAlMove();  // 이동 멈추고
        anim.SetInteger("animation", 19);    // 공격 애니메이션
        _stateController = Observable.Interval(TimeSpan.FromSeconds(2f)).TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                if(_stateType != GameDefine.AIStateType.Die)
                {
                    Suicide();
                    // 자폭 이펙트 넣을거면 여기에
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
