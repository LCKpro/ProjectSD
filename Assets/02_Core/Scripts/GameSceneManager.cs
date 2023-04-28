using System;
using System.Collections;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    public Action LoadSceneCompleteCallBack = null;

    public bool allowLadingViewHide = true; // �ε��� ���� ���ϵ���!

    public void SetLoadingView(bool isView)
    {
        GameUtils.Log("SetLoadingView = " + isView);

        // �ε��� ���� ���ϵ���!
        if ((isView == false) && (allowLadingViewHide == false))
        {
            return;
        }

        if ((isLoadScene == true) && (isView == false))
        {
            // ���ε����� ���� �ε� �ݱ⸦ �����Ѵ�.
            // ��Ʈ��ũ�� ��ġ�� ��찡 ����
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

    private bool isLoadScene = false; // �� �ε����ΰ�?

    public void LoadScene(string sceneName)
    {
        GameUtils.Log("LoadScene / " + sceneName);

        isLoadScene = true; // �� �ε����ΰ�?
        SetLoadingView(true);
        StartCoroutine(AsynchronousLoad(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Single));
    }

    public void LoadSceneAdditive(string sceneName)
    {
        GameUtils.Log("LoadScene / " + sceneName);

        isLoadScene = true; // �� �ε����ΰ�?
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

        // �ݹ� ó��
        if (LoadSceneCompleteCallBack != null)
        {
            LoadSceneCompleteCallBack();
            LoadSceneCompleteCallBack = null;
        }

        isLoadScene = false; // �� �ε����ΰ�?
    }
}