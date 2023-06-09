using System;
using UnityEngine;
using UniRx;
using GameCreator.Characters;
using GameCreator.Core;

public partial class AI_Structure : Stat
{
    private IDisposable _atkController = Disposable.Empty;
    private IDisposable _repairController = Disposable.Empty;
    public Animator anim;
    public NavigationMarker marker;
    public Actions lookAtAction;
    public int poolProjectileIndex;
    // 원거리 투사체 출발 위치
    public Transform startPos;
    


    // 공격하는데 성공했으면 타이머 끄기
    private void StopAttack()
    {
        _atkController.Dispose();
        _atkController = Disposable.Empty;
    }

    private GameObject targetObj = null;

    public void RangeAttackStart(Collider other)
    {
        ExcuteLookAt(other.transform.position);

        targetObj = other.gameObject;
        StopAttack();     // 공격하는데 성공했으면 타이머 끄기
        anim.SetTrigger("Shot");    // 공격 애니메이션
        _atkController = Observable.Interval(TimeSpan.FromSeconds(1f)).TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                if (targetObj.activeSelf == false)
                {
                    targetObj = null;
                    anim.SetTrigger("ShotEnd");
                    StopAttack();
                }
            });
    }

    private void ExcuteLookAt(Vector3 pos)
    {
        marker.transform.position = pos;
        lookAtAction.Execute();
    }

    public void RangeAttack()
    {
        var projectile = GamePlay.Instance.poolManager_Projectile.GetFromPool<Transform>("P0201");

        projectile.position = startPos.position;
        projectile.GetComponent<Projectile_Structure>().ReadyAndShot(this, targetObj);
    }

  
}
