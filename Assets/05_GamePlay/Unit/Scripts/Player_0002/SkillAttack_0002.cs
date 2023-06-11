using UnityEngine;
using UniRx;
using System;

public class SkillAttack_0002 : Stat
{
    private IDisposable _atkTimer = Disposable.Empty;

    private void StopAttack()
    {
        _atkTimer.Dispose();
        _atkTimer = Disposable.Empty;
    }

    public void SetWaterActive(bool isActive)
    {
        transform.gameObject.SetActive(isActive);
    }

    public void SkillAttackStart()
    {
        SetWaterActive(true);
        hitCount = 0;
        StopAttack();
        _atkTimer = Observable.Interval(TimeSpan.FromSeconds(1.0f)).TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                StopAttack();
                SetWaterActive(false);
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
            DealDamage(other.gameObject);
            hitCount++;
        }
    }
}
