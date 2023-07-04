using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;
using System;

public class GameOverManager : MonoBehaviour
{
    public Image blackBG;
    public float directingBGTime = 1;     // 페이드 연출 시간

    public TextMeshProUGUI gameOverText;
    public RectTransform gameOverRect;
    public float directingTextTime = 1;     // 페이드 연출 시간
    public float directintPositionTime = 1;     // 페이드 연출 시간
    public float buttonAppearTime = 1f;     // 버튼 등장 지연시간

    public GameObject btn_Restart;
    public GameObject btn_GoTitle;

    public GameObject image_GameOver;

    private IDisposable _directingTimer = Disposable.Empty;

    public void GameOver()
    {
        SetDefault();
        AnimateBG();
    }

    private void AnimateBG()
    {
        blackBG.gameObject.SetActive(true);
        blackBG.color = new Color(0, 0, 0, 0);
        float alpha = 0;
        float fadeTime = 1 / directingBGTime;

        _directingTimer = Observable.EveryUpdate().TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                if(_isSkip == true)
                {
                    SetDefaultTimer();
                    SkipDirecting();
                    return;
                }

                alpha += Time.deltaTime * fadeTime;
                blackBG.color = new Color(0, 0, 0, alpha);

                if(alpha >= 1)
                {
                    SetDefaultTimer();
                    AnimateAlphaText();
                }
            });
    }

    private void AnimateAlphaText()
    {
        gameOverText.color = new Color(1, 1, 1, 0);
        float alpha = 0;
        float fadeTime = 1 / directingTextTime;

        _directingTimer = Observable.EveryUpdate().TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                if (_isSkip == true)
                {
                    SetDefaultTimer();
                    SkipDirecting();
                    return;
                }

                alpha += Time.deltaTime * fadeTime;
                gameOverText.color = new Color(1, 1, 1, alpha);

                if (alpha >= 1)
                {
                    SetDefaultTimer();
                    AnimatePositionText();
                }
            });
    }

    private void AnimatePositionText()
    {
        gameOverRect.anchoredPosition = new Vector2(0, 0);

        _directingTimer = Observable.EveryUpdate().TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                if (_isSkip == true)
                {
                    SetDefaultTimer();
                    SkipDirecting();
                    return;
                }

                var y = gameOverRect.anchoredPosition.y + (Time.deltaTime * 90 * (1 / directintPositionTime));
                gameOverRect.anchoredPosition = new Vector2(gameOverRect.anchoredPosition.x, y);

                if (y >= 90)
                {
                    SetDefaultTimer();

                    Invoke("SetActiveButton", buttonAppearTime);
                }
            });
    }

    private void SkipDirecting()
    {
        blackBG.color = new Color(0, 0, 0, 1);
        gameOverText.color = new Color(1, 1, 1, 1);

        gameOverRect.anchoredPosition = new Vector2(gameOverRect.anchoredPosition.x, 90);

        SetActiveButton();
    }

    public void SetActiveButton()
    {
        image_GameOver.SetActive(true);

        btn_Restart.SetActive(true);
        btn_GoTitle.SetActive(true);
    }

    private void SetDefault()
    {
        SetDefaultColor();
        SetDefaultTimer();
    }

    private void SetDefaultColor()
    {
        blackBG.gameObject.SetActive(false);
        blackBG.color = new Color(0, 0, 0, 0);
        gameOverText.color = new Color(1, 1, 1, 0);
        gameOverRect.anchoredPosition = new Vector2(0, 0);
        btn_Restart.SetActive(false);
        btn_GoTitle.SetActive(false);
        image_GameOver.SetActive(false);
    }

    private void SetDefaultTimer()
    {
        _directingTimer.Dispose();
        _directingTimer = Disposable.Empty;
    }

    #region 버튼

    public void OnClick_Restart()
    {
        Core.Instance.gameSceneManager.LoadScene("GamePlay");
    }

    public void OnClick_GoTitle()
    {
        Core.Instance.gameSceneManager.LoadScene("GameStart");
    }

    private bool _isSkip = false;
    public void OnClick_Skip()
    {
        _isSkip = true;
    }

    #endregion
}
