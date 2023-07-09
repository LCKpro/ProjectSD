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
    // ���Ÿ� ����ü ��� ��ġ
    public Transform startPos;
    public string projectileKey;
    public GameDefine.AtkStructureType atkStructType = GameDefine.AtkStructureType.None;
    public GameObject shotEffect;
    public AI_Structure_HpBar aI_Structure_HpBar;


    // �����ϴµ� ���������� Ÿ�̸� ����
    private void StopAttack()
    {
        _atkController.Dispose();
        _atkController = Disposable.Empty;
    }

    private Transform targetObj = null;

    private bool isShot = false;
    public void RangeAttackStart(Collider other, float detectTime)
    {
        if(_isFinishCrafting == false)
        {
            return;
        }

        if(isShot == true)
        {
            return;
        }

        isShot = true;
        ExcuteLookAt(other.transform.position);

        targetObj = other.transform;
        StopAttack();     // �����ϴµ� ���������� Ÿ�̸� ����
        anim.SetTrigger("Shot");    // ���� �ִϸ��̼�
        _atkController = Observable.Interval(TimeSpan.FromSeconds(detectTime)).TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                ExcuteLookAt(other.transform.position);

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
                var targetPos = new Vector3(targetObj.transform.position.x, projectile.position.y, targetObj.transform.position.z);
                projectile.transform.LookAt(targetPos);
                SoundManager.instance.PlaySound("Balista");
                projectile.GetComponent<Projectile_Balista>().ReadyAndShot_Balista(this, targetObj);
                break;
            case GameDefine.AtkStructureType.Cannon:
                //ExcuteLookAt(targetObj.position);
                SoundManager.instance.PlaySound("Cannon");
                projectile.GetComponent<Projectile_Structure>().ReadyAndShot(this, targetObj);
                ActiveShotEffect();
                break;
            case GameDefine.AtkStructureType.Flame:
                SoundManager.instance.PlaySound("Flame");
                projectile.GetComponent<Projectile_Flame>().ReadyAndShot(this, targetObj.transform);
                break;
            case GameDefine.AtkStructureType.Stun:
                SoundManager.instance.PlaySound("Lightning");
                projectile.GetComponent<Projectile_Stun>().ReadyAndShot(this, targetObj.transform);
                break;
            case GameDefine.AtkStructureType.None:
                break;
            default:
                break;
        }
    }

    private void ActiveShotEffect()
    {
        if(shotEffect != null)
        {
            shotEffect.SetActive(false);
            shotEffect.SetActive(true);
        }
    }
}
