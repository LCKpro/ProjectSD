using System;
using UnityEngine;
using UniRx;

public class Projectile_Stun : MonoBehaviour
{
    public float power;
    public int durationTime;  // 전자포 지속시간
    public string idName;

    public Animator anim;

    public int slowDuration = 1;    // 슬로우 지속시간
    public float slowPercent = 0.2f;   // 슬로우 %
    public float rotateSpeed = 2f;      // 회전 속도(n초에 1바퀴)

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
                remainTime--;

                if (remainTime == 1)
                {
                    Debug.Log("그만 호출");
                    anim.SetTrigger("LaunchEnd");
                }
                else if (remainTime == 0)
                {
                    GamePlay.Instance.spawnManager.ReturnProjectilePool(idName, this.transform);
                    StopAttack();
                }
            });

        var vec = Vector3.zero;
        _lookAtTimer = Observable.EveryUpdate().TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                //LookAtTarget(target);
                vec.y = Time.deltaTime * 360 / rotateSpeed;
                transform.Rotate(vec);
            });
    }

    private void LookAtTarget(Transform target)
    {
        var targetPos = target.position + new Vector3(0, -1.5f, 0);
        transform.LookAt(targetPos);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Monster")
        {
            ai_Structure.DealCrowdControl(other.gameObject, GameDefine.CCType.Slow, slowDuration, slowPercent);
            ai_Structure.DealDamage(other.gameObject);
            Debug.Log("둔화 피해");
        }
    }
}
