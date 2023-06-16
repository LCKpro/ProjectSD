using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using TMPro;

public class UI_DayNightSystem : MonoBehaviour
{
    public Light dayLight;

    public float dayTimeSecond = 300f;
    public TextMeshProUGUI timeTxt;

    private IDisposable _clockTimer = Disposable.Empty;
    private IDisposable _dayNightTimer = Disposable.Empty;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        StartDayTimer();
        StartDayNightTimer();
    }

    private void StartDayTimer()
    {
        _clockTimer.Dispose();
        _clockTimer = Disposable.Empty;
        float fullTime = dayTimeSecond * 2; // �� �� ��ģ �ð�
        float time = dayTimeSecond / 72;    // ����� �ð�

        int hour = 6;
        int minute = 0;

        bool isDay = true;

        timeTxt.text = string.Format("AM 06 : 00");

        _clockTimer = Observable.Interval(TimeSpan.FromSeconds(time)).TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                minute += 10;

                if (minute >= 60)
                {
                    minute = 0;
                    hour++;

                    if(hour >= 12)
                    {
                        isDay = !isDay;     // ���� �ٲ��ֱ�
                        hour = 0;
                    }
                }

                if(isDay == true)
                {
                    timeTxt.text = string.Format($"AM {hour:D2} : {minute:D2}");
                }
                else
                {
                    timeTxt.text = string.Format($"PM {hour:D2} : {minute:D2}");
                }

            });
    }

    private void StartDayNightTimer()
    {
        _dayNightTimer.Dispose();
        _dayNightTimer = Disposable.Empty;

        dayLight.intensity = 0.5f;

        float fullTime = dayTimeSecond * 2; // �� �� ��ģ �ð�
        float time = 450;    // ����� �ð�
        _dayNightTimer = Observable.EveryUpdate().TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                // 600�� ������ ����
                if (time >= fullTime)
                {
                    time = 0;
                    return;
                }

                // ���� �� ���ư��� ������ �ð� ��ħ ��� ����
                var dt = Time.deltaTime;
                time += dt;

                // ���̸� ��� ���� �ø���, ���̸� ���� ���߱�
                if (time >= dayTimeSecond)
                {
                    dayLight.intensity += (dt / dayTimeSecond);
                }
                else
                {
                    if(dayLight.intensity <= 0.2f)
                    {
                        return;
                    }

                    dayLight.intensity -= (dt / dayTimeSecond);
                }
            });
    }
}
