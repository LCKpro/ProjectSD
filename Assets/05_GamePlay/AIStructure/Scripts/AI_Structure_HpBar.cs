using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

public class AI_Structure_HpBar : MonoBehaviour
{
    public GameObject hpBar_Frame;
    public Image hpBar_Fill;
    public float hpDownSpeed = 1f;
    private float prevHp = 1f;
    private float originLimitTime = 3f;

    private IDisposable _countDownTimer = Disposable.Empty;
    private IDisposable _hpTimer = Disposable.Empty;
    private IDisposable _checkTimer = Disposable.Empty;
    private IDisposable _activeTimer = Disposable.Empty;
    private Transform _target;

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    public void SetHpBar(float maxValue, float currentValue)
    {
        hpBar_Frame.SetActive(true);
        _limitTime = originLimitTime;
        
        ActiveHpBar();
        StopTimer();

        float targetFillAmount = currentValue / maxValue;

        if(prevHp < hpBar_Fill.fillAmount)
        {
            hpBar_Fill.fillAmount = prevHp;
        }

        prevHp = targetFillAmount;

        SetHpCountDown();   // N초가 지나면 HP바 사라지게 함
        SetHpValue();       // 시간에 따라 hp를 낮추거나 높임
        CheckHp(targetFillAmount);  // 도달했으면 타이머 멈추기

        _checkTimer = Observable.Interval(TimeSpan.FromSeconds(0.5f))
            .TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                CheckHp(targetFillAmount);
            });
    }

    private float _limitTime = 3f;
    private void SetHpCountDown()
    {
        _countDownTimer = Observable.EveryUpdate()
            .TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                _limitTime -= Time.deltaTime;

                if (_limitTime <= 0f)
                {
                    _limitTime = originLimitTime;
                    hpBar_Frame.SetActive(false);
                }
            });
    }

    private void StopCountDown()
    {
        _countDownTimer.Dispose();
        _countDownTimer = Disposable.Empty;
    }

    private void SetHpValue()
    {
        _hpTimer = Observable.EveryUpdate()
            .TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                hpBar_Fill.fillAmount -= (Time.deltaTime * (1 / hpDownSpeed));
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
        _activeTimer = Observable.EveryLateUpdate()
            .TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                Vector3 uiPos = Camera.main.WorldToScreenPoint(_target.position) + new Vector3(0, 25, 0);
                transform.position = uiPos;
            });
    }

    private void StopFollow()
    {
        _activeTimer.Dispose();
        _activeTimer = Disposable.Empty;
    }

    public void ResetActiveTimer()
    {
        Debug.Log("HPBar 종료");
        StopTimer();
        StopCountDown();
        StopFollow();
        GamePlay.Instance.spawnManager.ReturnHpBarPool(this);
    }

    
}
