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

    public GameObject ui_UnitSkill;

    public GameObject skyDome;

    private IDisposable _clockTimer = Disposable.Empty;
    private IDisposable _dayNightTimer = Disposable.Empty;

    public void Init()
    {
        StartDayTimer();        // 텍스트 수정하는 타이머
        StartDayNightTimer();   // 조명 수정하는 타이머
        SoundManager.instance.PlayBGM("Day");
        ui_UnitSkill.SetActive(false);
    }

    private void StartDayTimer()
    {
        _clockTimer.Dispose();
        _clockTimer = Disposable.Empty;
        float fullTime = dayTimeSecond * 2; // 낮 밤 합친 시간
        float time = dayTimeSecond / 72;    // 계산할 시간

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
                        isDay = !isDay;     // 낮밤 바꿔주기

                        hour = 0;
                    }
                }

                if(isDay == false && hour == 6 && minute == 0)  // 밤 정각에 몬스터 스폰
                {
                    ActionOnNight();
                }
                else if(isDay == true && hour == 6 && minute == 0)
                {
                    ActionOnDay();
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

        float fullTime = dayTimeSecond * 2; // 낮 밤 합친 시간
        float time = fullTime * 0.75f;    // 계산할 시간
        _dayNightTimer = Observable.EveryUpdate().TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                // 600초 지나면 리셋
                if (time >= fullTime)
                {
                    time = 0;
                    return;
                }

                skyDome.transform.Rotate(Vector3.up * Time.deltaTime);

                // 낮밤 다 돌아가기 전까지 시계 초침 계속 돌림
                var dt = Time.deltaTime;
                time += dt;

                // 밤이면 밝기 점점 올리고, 낮이면 점점 낮추기
                if (time >= dayTimeSecond)
                {
                    dayLight.intensity += (dt / dayTimeSecond);
                    skyDome.transform.Translate(Vector3.down * Time.deltaTime * 0.02f);
                }
                else
                {
                    if(dayLight.intensity <= 0.2f)
                    {
                        return;
                    }

                    skyDome.transform.Translate(Vector3.up * Time.deltaTime * 0.02f);
                    dayLight.intensity -= (dt / dayTimeSecond);
                }
            });
    }

    private void ActionOnNight()
    {
        GamePlay.Instance.gameDataManager.SaveData();
        ui_UnitSkill.SetActive(true);
        GamePlay.Instance.stageManager.StartSequence();
    }

    private void ActionOnDay()
    {
        GamePlay.Instance.gameDataManager.SaveData();
        ui_UnitSkill.SetActive(false);
    }
}
