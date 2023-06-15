using UnityEngine;
using UniRx;
using System;
using UnityEngine.UI;

public class DieMark : MonoBehaviour
{
    private IDisposable _timer = Disposable.Empty;
    public Image image;
    public Color markColor;

    private void SetActiveMark(bool isActive)
    {
        transform.gameObject.SetActive(isActive);
    }

    private void StopTimer()
    {
        _timer.Dispose();
        _timer = Disposable.Empty;
    }

    public void StartTimer()
    {
        StopTimer();
        markColor.a = 0f;
        SetActiveMark(true);

        _timer = Observable.EveryUpdate().TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                if (markColor.a >= 1f)
                {
                    StopTimer();
                    SetActiveMark(false);
                }
                else
                {
                    markColor.a += Time.deltaTime * 0.5f;
                    image.color = markColor;
                }
            });
    }
}
