using UnityEngine;
using GameCreator.Core;
using GameCreator.Characters;
using GameCreator.Inventory;

public class PlayView : MonoBehaviour
{
    #region SingleTon

    private static PlayView _instance = null;

    public static PlayView Instance
    {
        get
        {
            SetInstance();
            return _instance;
        }
    }

    public static void SetInstance()
    {
        if (_instance == null)
        {
            GameObject go = GameObject.Find("PlayView");
            if (go != null)
            {
                _instance = go.GetComponent<PlayView>();
                if (_instance == null)
                {
                    Debug.Log("PlayView Instance Null");
                }
            }
        }
    }

    void OnDestroy()
    {
        _instance = null;
    }

    void OnDisable()
    {
        _instance = null;
    }

    #endregion

    public void OnClick_Setting()
    {
        Core.Instance.uiPopUpManager.Show("UI_Setting");
    }
}
