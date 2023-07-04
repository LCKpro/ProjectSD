using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

public class AI_Structure_HpBar : MonoBehaviour
{
    public Image hpBar_Fill;
    public float hpDownSpeed = 1f;
    private float prevHp = 1f;

    private IDisposable _hpTimer = Disposable.Empty;
    private IDisposable _checkTimer = Disposable.Empty;
    private IDisposable _activeTimer = Disposable.Empty;

    public void SetHpBar(float maxValue, float currentValue)
    {
        ActiveHpBar();

        StopTimer();

        float targetFillAmount = currentValue / maxValue;

        if(prevHp < hpBar_Fill.fillAmount)
        {
            hpBar_Fill.fillAmount = prevHp;
        }

        prevHp = targetFillAmount;

        _hpTimer = Observable.EveryUpdate()
            .TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
        {
            hpBar_Fill.fillAmount -= (Time.deltaTime * (1 / hpDownSpeed));
        });

        CheckHp(targetFillAmount);  // 도달했으면 타이머 멈추기

        _checkTimer = Observable.Interval(TimeSpan.FromSeconds(0.5f))
            .TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                CheckHp(targetFillAmount);
            });
    }

    private void CheckHp(float targetFillAmount)
    {
        if (hpBar_Fill.fillAmount <= targetFillAmount)
        {
            StopTimer();
        }
    }

    private void StopTimer()
    {
        _hpTimer.Dispose();
        _hpTimer = Disposable.Empty;

        _checkTimer.Dispose();
        _checkTimer = Disposable.Empty;
    }

    private void ActiveHpBar()
    {
        ResetActiveTimer();
        transform.gameObject.SetActive(true);

        _activeTimer = Observable.Interval(TimeSpan.FromSeconds(3f))
            .TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                transform.gameObject.SetActive(false);
                ResetActiveTimer();
            });
    }

    private void ResetActiveTimer()
    {
        _activeTimer.Dispose();
        _activeTimer = Disposable.Empty;
    }
}
