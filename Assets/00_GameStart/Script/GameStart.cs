using UnityEngine;

public class GameStart : MonoBehaviour
{
    private static GameStart _instance = null;

    public static GameStart Instance
    {
        get
        {
            return _instance;
        }
    }

    public void Awake()
    {
        if (_instance == null)
        {
            GameObject go = GameObject.Find("GameStart");
            if (go != null)
            {
                _instance = go.GetComponent<GameStart>();
                if (_instance == null)
                {
                    GameUtils.Log("GameStart Instance Null");
                }
            }
        }
    }

    void OnDestroy()
    {
        _instance = null;
    }

    void Start()
    {
        GameUtils.Log("Loaded GameStart");

        ManagedStart();
    }

    public void ManagedStart()
    {
        Core.Instance.gameSceneManager.LoadScene("05_MainMenu");
    }
}