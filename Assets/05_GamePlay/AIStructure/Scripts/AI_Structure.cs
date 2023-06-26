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
    // ���Ÿ� ����ü ��� ��ġ
    public Transform startPos;
    public string projectileKey;
    public GameDefine.AtkStructureType atkStructType = GameDefine.AtkStructureType.None;
    


    // �����ϴµ� ���������� Ÿ�̸� ����
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
        StopAttack();     // �����ϴµ� ���������� Ÿ�̸� ����
        anim.SetTrigger("Shot");    // ���� �ִϸ��̼�
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
        var projectile = GamePlay.Instance.poolManager_Projectile.GetFromPool<Transform>(projectileKey);

        projectile.position = startPos.position;

        switch (atkStructType)
        {
            case GameDefine.AtkStructureType.Balista:
                projectile.transform.LookAt(targetObj.transform);
                projectile.GetComponent<Projectile_Balista>().ReadyAndShot(this, targetObj);
                break;
            case GameDefine.AtkStructureType.Cannon:
                projectile.GetComponent<Projectile_Structure>().ReadyAndShot(this, targetObj);
                break;
            case GameDefine.AtkStructureType.None:
                break;
            default:
                break;
        }
    }
}
