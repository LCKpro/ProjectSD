using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

public class UI_ClockSystem : MonoBehaviour
{
    public GameObject clockHand;

    public Light dayLight;

    public float dayTimeSecond = 300f;

    private IDisposable _clockTimer = Disposable.Empty;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        StartDayTimer();
    }

    private void StartDayTimer()
    {
        _clockTimer.Dispose();
        _clockTimer = Disposable.Empty;
        float fullTime = dayTimeSecond * 2; // 낮 밤 합친 시간
        float time = 0f;    // 계산할 시간
        Vector3 hand = new Vector3(0, 0, 0);
        _clockTimer = Observable.EveryUpdate().TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                // 600초 지나면 리셋
                if (time >= fullTime)
                {
                    time = 0;
                    hand = new Vector3(0, 0, 0);
                    return;
                }

                // 낮밤 다 돌아가기 전까지 시계 초침 계속 돌림
                var dt = Time.deltaTime;
                time += dt;
                hand.z = time / dayTimeSecond * -180f;
                clockHand.transform.eulerAngles = hand;

                // 밤이면 밝기 점점 올리고, 낮이면 점점 낮추기
                if (time >= dayTimeSecond)
                {
                    dayLight.intensity += (dt / dayTimeSecond);
                }
                else
                {
                    dayLight.intensity -= (dt / dayTimeSecond);
                }
            });
    }
}
