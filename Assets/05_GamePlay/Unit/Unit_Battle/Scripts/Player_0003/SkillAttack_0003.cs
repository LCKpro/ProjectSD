using UnityEngine;
using UniRx;
using System;

public class SkillAttack_0003 : Stat
{
    private IDisposable _atkTimer = Disposable.Empty;

    private void StopAttack()
    {
        _atkTimer.Dispose();
        _atkTimer = Disposable.Empty;
    }

    public void SetTyphoonActive(bool isActive)
    {
        transform.gameObject.SetActive(isActive);
    }

    public void SkillAttackStart()
    {
        SetTyphoonActive(true);
        hitCount = 0;
        StopAttack();
        float sTime = 0f;
        _atkTimer = Observable.EveryUpdate().TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                sTime += Time.deltaTime;

                if(sTime > 2.0f)
                {
                    // 종료
                    sTime = 0f;
                    StopAttack();
                    SetTyphoonActive(false);
                }
                else if(sTime > 1.5f)
                {
                    // 내려가기
                    transform.Translate(Vector3.down * Time.deltaTime * 10f, Space.World);
                }
                else if(sTime < 0.5f)
                {
                    // 올라가기
                    transform.Translate(Vector3.up * Time.deltaTime * 10f, Space.World);
                }
            });
    }

    private int hitCount = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (hitCount >= 10)
        {
            return;
        }

        if (other.gameObject.CompareTag("Monster") == true)
        {
            DealCrowdControl(other.gameObject, GameDefine.CCType.Stun, 1.5f);
            DealDamage(other.gameObject);
            hitCount++;
        }
    }
}
