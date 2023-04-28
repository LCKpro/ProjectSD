using System;
using System.Collections;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    public Action LoadSceneCompleteCallBack = null;

    public bool allowLadingViewHide = true; // 로딩뷰 끄지 못하도록!

    public void SetLoadingView(bool isView)
    {
        GameUtils.Log("SetLoadingView = " + isView);

        // 로딩뷰 끄지 못하도록!
        if ((isView == false) && (allowLadingViewHide == false))
        {
            return;
        }

        if ((isLoadScene == true) && (isView == false))
        {
            // 씬로딩중일 때는 로딩 닫기를 무시한다.
            // 네트워크와 겹치는 경우가 있음
            return;
        }
    }

    private bool isGamePlayToMainMenu = false;

    public void setIsGamePlayToMainMenu(bool setBool)
    {
        isGamePlayToMainMenu = setBool;
    }

    public void LoadScene_GamePlayToMainMenu()
    {
        isGamePlayToMainMenu = true;
        Time.timeScale = 1f;

        LoadScene("05_MainMenu");
    }

    private bool isLoadScene = false; // 씬 로딩중인가?

    public void LoadScene(string sceneName)
    {
        GameUtils.Log("LoadScene / " + sceneName);

        isLoadScene = true; // 씬 로딩중인가?
        SetLoadingView(true);
        StartCoroutine(AsynchronousLoad(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Single));
    }

    public void LoadSceneAdditive(string sceneName)
    {
        GameUtils.Log("LoadScene / " + sceneName);

        isLoadScene = true; // 씬 로딩중인가?
        SetLoadingView(true);
        StartCoroutine(AsynchronousLoad(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive));
    }
    IEnumerator AsynchronousLoad(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode)
    {
        yield return null;
        AsyncOperation ao = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
        while (!ao.isDone)
        {
            yield return null;
        }

        // 콜백 처리
        if (LoadSceneCompleteCallBack != null)
        {
            LoadSceneCompleteCallBack();
            LoadSceneCompleteCallBack = null;
        }

        isLoadScene = false; // 씬 로딩중인가?
    }
}