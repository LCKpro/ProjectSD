using UnityEngine;
using UniRx;
using System;

public class NormalAttack_0002 : Stat
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

    public void AttackStart()
    {
        SetWaterActive(true);
        isHit = false;
        StopAttack();
        _atkTimer = Observable.Interval(TimeSpan.FromSeconds(1.0f)).TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                StopAttack();
                SetWaterActive(false);
            });
    }

    private bool isHit = false;
    private void OnTriggerEnter(Collider other)
    {
        if(isHit == true)
        {
            return;
        }

        if(other.gameObject.CompareTag("Monster") == true)
        {
            DealDamage(other.gameObject);
            isHit = true;
        }
    }
}
