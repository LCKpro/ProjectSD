using System;
using UnityEngine;
using UniRx;

public class Projectile_Flame : MonoBehaviour
{
    public float power;
    public int durationTime;  // 화염포 지속시간
    public string idName;

    public Animator anim;

    public float damagePerTick = 0f;
    public int burnDuration = 1;    // 화상 지속시간

    private IDisposable _atkController = Disposable.Empty;
    private IDisposable _lookAtTimer = Disposable.Empty;
    private AI_Structure ai_Structure;
    public BoxCollider coll;

    // 공격하는데 성공했으면 타이머 끄기
    private void StopAttack()
    {
        _atkController.Dispose();
        _atkController = Disposable.Empty;

        _lookAtTimer.Dispose();
        _atkController = Disposable.Empty;
    }

    private bool isShot = false;
    public void ReadyAndShot(AI_Structure structure, Transform target)
    {
        int remainTime = durationTime;

        StopAttack();

        anim.SetTrigger("LaunchStart");
        ai_Structure = structure;

        LookAtTarget(target);
        _atkController = Observable.Interval(TimeSpan.FromSeconds(1f)).TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                coll.enabled = false;
                coll.enabled = true;

                remainTime--;

                if (remainTime == 1)
                {
                    Debug.Log("그만 호출");
                    anim.SetTrigger("LaunchEnd");
                }
                else if(remainTime == 0)
                {
                    GamePlay.Instance.spawnManager.ReturnProjectilePool(idName, this.transform);
                    StopAttack();
                }
            });

        _lookAtTimer = Observable.EveryUpdate().TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                LookAtTarget(target);
            });
    }

    private void LookAtTarget(Transform target)
    {
        var targetPos = target.position + new Vector3(0, 3, 0);
        transform.LookAt(targetPos);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Monster")
        {
            ai_Structure.Burn(other.gameObject, damagePerTick, burnDuration);
            Debug.Log("화염 피해");
        }
    }
}
