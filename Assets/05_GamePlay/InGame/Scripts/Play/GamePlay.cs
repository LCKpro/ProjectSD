using UnityEngine;
using Redcode.Pools;

public class GamePlay : MonoBehaviour
{
    #region SingleTon

    private static GamePlay _instance = null;

    public static GamePlay Instance
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
            GameObject go = GameObject.Find("GamePlay");
            if (go != null)
            {
                _instance = go.GetComponent<GamePlay>();
                if (_instance == null)
                {
                    Debug.Log("GamePlay Instance Null");
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

    public PlayerManager playerManager;
    public NatureManager natureManager;
    public CraftingManager craftingManager;
    public UI_System ui_System;
    public InGamePools inGamePools;
    public PoolManager poolManager_Monster;
    public PoolManager poolManager_Structure;
    public PoolManager poolManager_IncompleteStructure;
    public PoolManager poolManager_Projectile;
    public SpawnManager spawnManager;
    public UI_ClockSystem uI_ClockSystem;

    #region ¹öÆ°

    public void OnClick_ShowCraftSystem()
    {
        ui_System.gameObject.SetActive(true);
        ui_System.OnClick_StartCraftSystem();
    }

    #endregion
}
