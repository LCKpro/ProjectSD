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
        float fullTime = dayTimeSecond * 2; // �� �� ��ģ �ð�
        float time = 0f;    // ����� �ð�
        Vector3 hand = new Vector3(0, 0, 0);
        _clockTimer = Observable.EveryUpdate().TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                // 600�� ������ ����
                if (time >= fullTime)
                {
                    time = 0;
                    hand = new Vector3(0, 0, 0);
                    return;
                }

                // ���� �� ���ư��� ������ �ð� ��ħ ��� ����
                var dt = Time.deltaTime;
                time += dt;
                hand.z = time / dayTimeSecond * -180f;
                clockHand.transform.eulerAngles = hand;

                // ���̸� ��� ���� �ø���, ���̸� ���� ���߱�
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
