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
    // 원거리 투사체 출발 위치
    public Transform startPos;
    public string projectileKey;
    public GameDefine.AtkStructureType atkStructType = GameDefine.AtkStructureType.None;
    


    // 공격하는데 성공했으면 타이머 끄기
    private void StopAttack()
    {
        _atkController.Dispose();
        _atkController = Disposable.Empty;
    }

    private Transform targetObj = null;

    private bool isShot = false;
    public void RangeAttackStart(Collider other, float detectTime)
    {
        if(isShot == true)
        {
            return;
        }

        isShot = true;
        //ExcuteLookAt(other.transform.position);

        targetObj = other.transform;
        StopAttack();     // 공격하는데 성공했으면 타이머 끄기
        anim.SetTrigger("Shot");    // 공격 애니메이션
        _atkController = Observable.Interval(TimeSpan.FromSeconds(detectTime)).TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                if (other.gameObject.activeSelf == false)
                {
                    isShot = false;
                    targetObj = null;
                    anim.SetTrigger("ShotEnd");
                    StopAttack();
                }
            });
    }

    private void ExcuteLookAt(Vector3 pos)
    {
        var newPos = pos + new Vector3(0, 3, 0);
        marker.transform.position = newPos;
        lookAtAction.Execute();
    }

    public void RangeAttack()
    {
        var projectile = GamePlay.Instance.poolManager_Projectile.GetFromPool<Transform>(projectileKey);

        projectile.position = startPos.position;
        //projectile.transform.LookAt(targetObj.transform);

        switch (atkStructType)
        {
            case GameDefine.AtkStructureType.Balista:
                ExcuteLookAt(targetObj.position);
                projectile.GetComponent<Projectile_Balista>().ReadyAndShot(this, targetObj);
                break;
            case GameDefine.AtkStructureType.Cannon:
                ExcuteLookAt(targetObj.position);
                projectile.GetComponent<Projectile_Structure>().ReadyAndShot(this, targetObj);
                break;
            case GameDefine.AtkStructureType.Flame:
                projectile.GetComponent<Projectile_Flame>().ReadyAndShot(this, targetObj.transform);
                break;
            case GameDefine.AtkStructureType.Stun:
                break;
            case GameDefine.AtkStructureType.None:
                break;
            default:
                break;
        }
    }
}
