using UnityEngine;
using UnityEngine.UI;
using System;
using UniRx;

public class CraftSlider : MonoBehaviour
{
    private Slider _craftSlider;
    private IDisposable sliderTimer = Disposable.Empty;

    private float barValue = 0f;

    private void Start()
    {
        _craftSlider = GetComponent<Slider>();
        barValue = 0f;
    }

    private void OnEnable()
    {
        Locate();
        ActiveLoadingBar();
    }

    private void Locate()
    {
        Vector3 playerPos = GamePlay.Instance.playerManager.GetPlayer().transform.position;
        playerPos.y = 0;
        Vector3 uiPos = Camera.main.WorldToScreenPoint(playerPos);

        transform.position = uiPos;
        var rect = transform.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y + 80f);
    }

    private void ActiveLoadingBar()
    {
        SoundManager.instance.PlaySound("CraftStart");

        sliderTimer = Disposable.Empty;
        sliderTimer.Dispose();
        sliderTimer = Observable.EveryUpdate().TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                if(barValue < 1f)
                {
                    barValue += Time.deltaTime;
                    _craftSlider.value = barValue;
                }
                else
                {
                    barValue = 0f;
                }
            });
    }
}
